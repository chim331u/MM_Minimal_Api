using System.Globalization;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Services
{
    public class AncillaryService : IAncillaryService
    {
        private readonly ApplicationContext _context;
        private readonly IUtilityService _utilityService;
        private readonly ILogger<AncillaryService> _logger;

        public AncillaryService(ILogger<AncillaryService> logger, ApplicationContext context, IUtilityService utilityService)
        {
            _logger = logger;
            _context = context;
            _utilityService = utilityService;
            UpdateCurrencyRate();
        }

        #region Country


        public async Task<ICollection<Country>> GetActiveCountryList()
        {
            try
            {
                var result = await _context.Country.Where(x => x.IsActive).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Country> GetCountry(int countryId)
        {
            try
            {
                var result = await _context.Country.FindAsync(countryId);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Country> UpdateCountry(Country item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.Country.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Country> AddCountry(Country item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                var result = await _context.Country.AddAsync(item);
                await _context.SaveChangesAsync();

                return await _context.Country.FindAsync(item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Country> DeleteCountry(Country item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.Country.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        #endregion

        #region Currency


        public async Task<ICollection<Currency>> GetActiveCurrencyList()
        {
            try
            {
                var result = await _context.Currency.Where(x => x.IsActive).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Currency> GetCurrency(int Id)
        {
            try
            {
                var result = await _context.Currency.FindAsync(Id);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Currency> UpdateCurrency(Currency item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.Currency.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Currency> AddCurrency(Currency item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                var result = await _context.Currency.AddAsync(item);
                await _context.SaveChangesAsync();

                return await _context.Currency.FindAsync(item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Currency> DeleteCurrency(Currency item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.Currency.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        #endregion

        #region Currency Conversion Rate


        public async Task<ICollection<CurrencyConversionRate>> GetActiveCurrencyConversionList()
        {
            try
            {
                var result = await _context.CurrencyConversionRates.Where(x => x.IsActive).OrderByDescending(x => x.ReferringDate).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<CurrencyConversionRate> GetCurrencyConversion(int Id)
        {
            try
            {
                var result = await _context.CurrencyConversionRates.FindAsync(Id);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<CurrencyConversionRate> GetCurrencyRate(string currencyALF3)
        {
            try
            {
                var result = await _context.CurrencyConversionRates.Where(c => c.IsActive == true && c.CurrencyCodeALF3.ToUpper() == currencyALF3.ToUpper())
                .OrderByDescending(c => c.ReferringDate)
                .FirstOrDefaultAsync();

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<CurrencyConversionRate> UpdateCurrencyConversion(CurrencyConversionRate item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.CurrencyConversionRates.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<CurrencyConversionRate> AddCurrencyConversion(CurrencyConversionRate item)
        {
            var uniqueK = string.Concat(item.CurrencyCodeALF3, item.RateValue.ToString(), DateTime.Now.Date);

            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                var result = await _context.CurrencyConversionRates.AddAsync(item);
                await _context.SaveChangesAsync();

                return await _context.CurrencyConversionRates.FindAsync(item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<CurrencyConversionRate> DeleteCurrencyConversion(CurrencyConversionRate item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.CurrencyConversionRates.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        private async Task<DateTime> GetLastUpdateDate()
        {
            try
            {
                var lastDateUpdate = await _context.CurrencyConversionRates.Where(c => c.IsActive == true)
                                        .OrderByDescending(c => c.ReferringDate).Select(c => c.ReferringDate)
                                        .FirstOrDefaultAsync();

                return lastDateUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return DateTime.MinValue;

            }

        }

        public async Task<int> UpdateCurrencyRate()
        {

            if (await GetLastUpdateDate() < DateTime.Now.Date)
            {
                var doc = new XmlDocument();
                doc.Load(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

                XmlNodeList nodes = doc.SelectNodes("//*[@currency]");

                if (nodes != null)
                {
                    var currentCurrencies = await GetActiveCurrencyList();

                    //int counter = 0;
                    foreach (XmlNode node in nodes)
                    {
                        var currency = node.Attributes["currency"].Value;

                        if (currentCurrencies.Where(x => x.CurrencyCodeALF3 == currency).Any())
                        {
                            var rate = Decimal.Parse(node.Attributes["rate"].Value, NumberStyles.Any, new CultureInfo("en-Us"));
                            var uniqueK = string.Concat(currency, rate.ToString(), DateTime.Now.Date);

                            bool isCodeExist = _context.CurrencyConversionRates.Any(c => c.UniqueKey == uniqueK && c.IsActive == true);

                            if (!isCodeExist)
                            {
                                await AddCurrencyConversion(new CurrencyConversionRate
                                {
                                    CreatedDate = DateTime.Now,
                                    CurrencyCodeALF3 = currency,
                                    IsActive = true,
                                    LastUpdatedDate = DateTime.Now,
                                    ReferringDate = DateTime.Now,
                                    RateValue = rate,
                                    UniqueKey = uniqueK
                                });
                            }

                        }

                    }

                    try
                    {
                        var result = _context.SaveChanges();
                        _logger.LogInformation("Currency rate updated");
                        return result;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        return -2;
                    }

                }

                _logger.LogWarning("Currency rate NOT updated");
                return -1;
            }
            else
            {
                _logger.LogInformation("Currency rate already updated");
                return 1;
            }

        }

        public async Task<int> UpdateAllCurrencyRate()
        {
            if (await GetLastUpdateDate() < DateTime.Now.Date)
            {
                var doc = new XmlDocument();
                doc.Load(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

                XmlNodeList nodes = doc.SelectNodes("//*[@currency]");

                if (nodes != null)
                {
                    //int counter = 0;
                    foreach (XmlNode node in nodes)
                    {
                        var currency = node.Attributes["currency"].Value;
                        var rate = Decimal.Parse(node.Attributes["rate"].Value, NumberStyles.Any, new CultureInfo("en-Us"));
                        var uniqueK = string.Concat(currency, rate.ToString(), DateTime.Now.Date);

                        bool isCodeExist = _context.CurrencyConversionRates.Any(c => c.UniqueKey == uniqueK && c.IsActive == true);

                        if (!isCodeExist)
                        {
                            await AddCurrencyConversion(new CurrencyConversionRate
                            {
                                CreatedDate = DateTime.Now,
                                CurrencyCodeALF3 = currency,
                                IsActive = true,
                                LastUpdatedDate = DateTime.Now,
                                ReferringDate = DateTime.Now,
                                RateValue = rate,
                                UniqueKey = uniqueK
                            });
                        }

                    }

                    try
                    {
                        var result = _context.SaveChanges();
                        _logger.LogInformation("Currency rate updated");
                        return result;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        return -2;
                    }

                }
                _logger.LogWarning("Currency rate NOT updated");
                return -1;
            }
            else
            {
                _logger.LogInformation("Currency rate already updated");
                return 1;
            }
        }

        public async Task<string> ClearUnusedRates()
        {
            var currentCurrencies = await _context.Currency.Where(x => x.IsActive).Select(x => x.CurrencyCodeALF3).ToListAsync();

            var currentCurrencyRates = await GetActiveCurrencyConversionList();

            var rejectList = currentCurrencyRates.Where(i => currentCurrencies.Contains(i.CurrencyCodeALF3));
            var filteredList = currentCurrencyRates.Except(rejectList);
            try
            {
                foreach (var item in filteredList)
                {
                    //await DeleteCurrencyConversion(item);
                    item.LastUpdatedDate = DateTime.Now;
                    item.IsActive = false;
                    _context.CurrencyConversionRates.Update(item);
                }

                var result = _context.SaveChanges();
                _logger.LogInformation("Clear Currency rate completed");
                return "Deleted unused currency rates";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }


        }
        #endregion

        #region Service User
        public async Task<ICollection<ServiceUser>> GetActiveServiceUserList()
        {
            try
            {
                var result = await _context.ServiceUser.Where(x => x.IsActive).OrderBy(x => x.CreatedDate).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ServiceUser> GetServiceUser(int serviceUserId)
        {
            try
            {
                var result = await _context.ServiceUser.FindAsync(serviceUserId);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ServiceUser> UpdateServiceUser(ServiceUser item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.ServiceUser.Update(item);

                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<ServiceUser> AddServiceUser(ServiceUser item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.LastUpdatedDate = DateTime.Now;

                var result = await _context.ServiceUser.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<ServiceUser> DeleteServiceUser(ServiceUser item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.ServiceUser.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }


        #endregion

        #region Supplier


        public async Task<ICollection<Supplier>> GetActiveSupplierList()
        {
            try
            {
                var result = await _context.suppliers.Where(x => x.IsActive).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Supplier> GetSupplier(int supplierId)
        {
            try
            {
                var result = await _context.suppliers.FindAsync(supplierId);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Supplier> UpdateSupplier(Supplier item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.suppliers.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Supplier> AddSupplier(Supplier item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                var result = await _context.suppliers.AddAsync(item);
                await _context.SaveChangesAsync();

                return await _context.suppliers.FindAsync(item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Supplier> DeleteSupplier(Supplier item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.suppliers.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        #endregion

        #region ReadInBill


        public async Task<ICollection<ReadInBill>> GetActiveReadInBillList()
        {
            try
            {
                var result = await _context.readInBill.Include(c => c.Supplier).Where(x => x.IsActive).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }        
        
        public async Task<ICollection<ReadInBill>> GetActiveReadInBillBySupplierList(int id)
        {
            try
            {
                var result = await _context.readInBill.Include(c => c.Supplier).Where(x => x.IsActive && x.Supplier.Id==id).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ReadInBill> GetReadInBill(int readInBillId)
        {
            try
            {
                var result = await _context.readInBill.Include(c => c.Supplier).Where(x=>x.Id==readInBillId).FirstOrDefaultAsync(); 
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ReadInBill> UpdateReadInBill(ReadInBill item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.readInBill.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<ReadInBill> AddReadInBill(ReadInBill item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                item.Supplier = await _context.suppliers.FindAsync(item.Supplier.Id);

                var result = await _context.readInBill.AddAsync(item);
                await _context.SaveChangesAsync();

                return await _context.readInBill.FindAsync(item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<ReadInBill> DeleteReadInBill(ReadInBill item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.readInBill.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        #endregion
    }
}
