using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Balance;
using MoneyManagement.Models.Statistics;

namespace MoneyManagement.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<StatisticService> _logger;
        private readonly IAncillaryService _anchillaryService;
        private readonly ISalaryService _salaryService;

        public StatisticService(ILogger<StatisticService> logger, ApplicationContext context, IAncillaryService anchillaryService, ISalaryService salaryService)
        {
            _context = context;
            _logger = logger;
            _anchillaryService = anchillaryService;
            _salaryService = salaryService;
        }

        #region Dashboard

        public async Task<Dashboard> GetDashboard()
        {
            Dashboard dashboard = new Dashboard();

            var summaryAccountsBalance = await GetAvailableBalances();
            var CHFRate = (await _anchillaryService.GetCurrencyRate("CHF")).RateValue;

            dashboard.BalanceLineChar = await GetBalanceLineChart(summaryAccountsBalance, CHFRate);
            dashboard.SalaryCharts = await GetSalaryChart();
            dashboard.SpentChar = await GetSpentChart(dashboard.BalanceLineChar, dashboard.SalaryCharts);
            dashboard.BalancesSummary = await GetBalancesSummary(summaryAccountsBalance, CHFRate);
            return dashboard;
        }

        public async Task<IList<Balance>> GetAvailableBalances()
        {
            var summaryAccounts = await _context.Balance.Include(b => b.Account).Include(b => b.Account.Currency)
                    .Where(b => b.Account.AccountType == "Available")
                    .OrderByDescending(b => b.DateBalance).ToListAsync();

            return summaryAccounts;
        }

        public async Task<IList<BalanceLineChart>> GetBalanceLineChart(IList<Balance> summaryAccounts, decimal CHFRate)
        {

            if (summaryAccounts != null)
            {
                var totBalances = new List<BalanceLineChart>();

                try
                {
                   
                    foreach (var item in summaryAccounts)
                    {

                        var balanceDTO = new BalanceLineChart
                        {
                            DateBalance = item.DateBalance,
                            BalanceValue = item.BalanceValue
                        };

                        if (item.Account.Currency.CurrencyCodeALF3 == "CHF")
                        {
                            balanceDTO.BalanceValue = item.BalanceValue / Convert.ToDouble(CHFRate);
                        }

                        if (item.Account.Currency.CurrencyCodeALF3 != "EUR" && item.Account.Currency.CurrencyCodeALF3 != "CHF")
                        {
                            var exchangeRate = await _anchillaryService.GetCurrencyRate(item.Account.Currency.CurrencyCodeALF3);
                            balanceDTO.BalanceValue = item.BalanceValue / Convert.ToDouble(exchangeRate.RateValue);
                        }

                        totBalances.Add(balanceDTO);

                    }

                    var totBalanceByDateEur = (from b in totBalances //.Include(b => b.Account.Currency)
                                               orderby b.DateBalance descending
                                               //where b.IsActive == true
                                               group b by new
                                               {
                                                   //b.Account,
                                                   b.DateBalance.Year,
                                                   b.DateBalance.Month
                                               } into bal
                                               select new BalanceLineChart()
                                               {
                                                   //Account = bal.First().Account,
                                                   DateBalance = new DateTime(bal.Key.Year, bal.Key.Month, 01),
                                                   BalanceValue = bal.Sum(c => c.BalanceValue)
                                               }).OrderByDescending(e => e.DateBalance).ToList();

                    return totBalanceByDateEur;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }

            }

            _logger.LogInformation("Account is null or empty");
            return null;
        }

        public async Task<IList<SalaryChart>> GetSalaryChart()
        {
            try
            {
                // salary total
                var salaryAll = await _context.Salary.Where(s => s.IsActive == true).OrderByDescending(e => e.SalaryDate)
                        .GroupBy(s => new { s.ReferYear, s.ReferMonth })
                         .Select(s => new SalaryChart
                         {
                             RefYear = s.Key.ReferYear,
                             RefMonth = s.Key.ReferMonth,
                             SalaryAmountEur = s.Sum(x => x.SalaryValueEur),
                             RefDate = new DateTime(Convert.ToInt32(s.Key.ReferYear), Convert.ToInt32(s.Key.ReferMonth), 01)
                         }).ToListAsync();

                return salaryAll;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }


        }

        public Task<IList<SpentChart>> GetSpentChart(IList<BalanceLineChart> totBalanceByDateEur, IList<SalaryChart> salaryAll)
        {
            IList<SpentChart> spentChar = new List<SpentChart>();

            //Exit = Balance 1 + Salary 1 - balance 0
            for (int i = 0; i < 25; i++)
            {
                double salaryAmount = 0;

                try
                {
                    var _temp = salaryAll.Where(s => s.RefMonth == totBalanceByDateEur[i].DateBalance.Month.ToString()
                    && s.RefYear == totBalanceByDateEur[i].DateBalance.Year.ToString()).FirstOrDefault();

                    if (_temp != null)
                    {
                        salaryAmount = _temp.SalaryAmountEur;
                    }

                }
                catch (Exception ex)
                {
                    salaryAmount = 0;
                    _logger.LogError(ex.Message);
                }

                double exit = 0;

                try
                {
                    exit = (totBalanceByDateEur[i].BalanceValue + salaryAmount) - totBalanceByDateEur[i + 1].BalanceValue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Task.FromResult<IList<SpentChart>>(null);
                }


                //if (i == 24)
                //{
                //    dashboardDTO.exitTotEur = exit;
                //    dashboardDTO.exitRefPeriod = string.Concat(totBalanceByDateEur[i].DateBalance.Month, "/", totBalanceByDateEur[i].DateBalance.Year);
                //}

                //if (i == 23)
                //{
                //    dashboardDTO.prevExitTotEur = exit;
                //}

                spentChar.Add(new SpentChart { SpentDate = new DateTime(totBalanceByDateEur[i].DateBalance.Year, totBalanceByDateEur[i].DateBalance.Month, 1), SpentAmount = Math.Round(exit, 2) });
            }

            return Task.FromResult(spentChar);

        }

        public async Task<IList<BalancesSummary>> GetBalancesSummary(IList<Balance> summaryAccounts, decimal CHFRate)
        {
            var balancesSummary = new List<BalancesSummary>();

            var activeAccounts = summaryAccounts.Where(x=>x.Account.IsActive).ToList();

            try
            {
                if (activeAccounts != null)
                {
                    foreach (var item in activeAccounts.Select(b => b.Account).Distinct())
                    {
                        var balances = activeAccounts.Where(x => x.Account == item).OrderByDescending(x => x.DateBalance).ToList();

                        var lastBalance = balances.First();

                        double prevBalance;

                        if (balances.Count > 1)
                        {
                            prevBalance = balances.Skip(1).First().BalanceValue;
                        }
                        else
                        {
                            prevBalance = 0;
                        }

                        double diff = lastBalance.BalanceValue - prevBalance;

                        var balanceSummary = new BalancesSummary
                        {
                            AccountName = item.Name,
                            BalanceAmount = lastBalance.BalanceValue,
                            BalanceDate = lastBalance.DateBalance,
                            Currency = item.Currency.CurrencyCodeALF3,
                            DifferenceAmount = diff,
                        };

                        balancesSummary.Add(balanceSummary);
                    }
                }

                double grandTotalEur = 0;
                double grandDiffEur = 0;

                var currencies = activeAccounts.Select(x => x.Account.Currency.CurrencyCodeALF3).Distinct().ToList();

                //calc totals
                foreach (var item in currencies)
                {
                    var totAmount = balancesSummary.Where(e => e.Currency == item).Sum(e => e.BalanceAmount);
                    var totDiff = balancesSummary.Where(e => e.Currency == item).Sum(e => e.DifferenceAmount);

                    var balanceSummaryTotal = new BalancesSummary
                    {
                        AccountName = string.Concat("Total ", item),
                        BalanceAmount = totAmount,
                        DifferenceAmount = totDiff, Currency = item, BalanceDate= DateTime.Now  
                    };

                    balancesSummary.Add(balanceSummaryTotal);

                    switch (item)
                    {
                        case "EUR":
                            grandTotalEur += totAmount;
                            grandDiffEur += totDiff;
                            break;

                        case "CHF":
                            grandTotalEur += totAmount / Convert.ToDouble(CHFRate);
                            grandDiffEur += totDiff / Convert.ToDouble(CHFRate);
                            break;

                        default:
                            var rate = (await _anchillaryService.GetCurrencyRate(item)).RateValue;
                            grandTotalEur += totAmount / Convert.ToDouble(rate);
                            grandDiffEur += totDiff / Convert.ToDouble(rate);
                            break;
                    }

                }

                var balanceSummaryGrandTotal = new BalancesSummary
                {
                    AccountName = string.Concat("Grand Total"),
                    BalanceAmount = grandTotalEur,
                    DifferenceAmount = grandDiffEur,
                    Currency = "EUR",
                    BalanceDate = DateTime.Now
                };

                balancesSummary.Add(balanceSummaryGrandTotal);               

                return balancesSummary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }

        #endregion

        #region Salary

        public async Task<List<SalaryStatistics>> GetSalaryStatistic(int userId)
        {
            var salaries = await _salaryService.GetActiveSalaryList();

            //salaries = salaries.Where(s=>s.ReferMonth == "12").ToList();

            if (userId > 0)
            {               
                salaries = salaries.Where(s=>s.User.Id == userId).ToList();
            }

            var salariesMiddle = (from s in salaries
                                  group s by new
                                  {
                                      s.ReferYear,
                                      s.ReferMonth
                                  } into salaryTot
                                  select new SalaryStatistics
                                  {
                                      RefYear = salaryTot.Key.ReferYear,
                                      RefMonth = salaryTot.Key.ReferMonth,
                                      RefAmount = salaryTot.Sum(c => c.SalaryValueEur),
                                      RefDate = new DateTime(Convert.ToInt32(salaryTot.Key.ReferYear), Convert.ToInt32(salaryTot.Key.ReferMonth), 1)
                                  }).OrderByDescending(s => s.RefDate).ToList();

            return salariesMiddle;



        }

        #endregion
    }
}
