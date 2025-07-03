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
            //
            // Task.Run(async () =>
            // {
            //     await
            UpdateCurrencyRate();
            //     _logger.LogInformation("Update currency rate Async task completed.");
            // });
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
                var existingCountry = await _context.Country.FindAsync(item.Id);
                
                if (existingCountry == null)
                {
                    _logger.LogWarning($"Country with ID {item.Id} not found.");
                    return null;
                }
                
                // Update the properties of the existing country
                existingCountry.Name = item.Name;
                existingCountry.CountryCodeNum3 = item.CountryCodeNum3;
                existingCountry.CountryCodeALF3 = item.CountryCodeALF3;
                existingCountry.IsActive = item.IsActive;
                existingCountry.LastUpdatedDate = DateTime.Now;
                existingCountry.Description = item.Description;
                existingCountry.Note = item.Note;

                _context.Country.Update(existingCountry);
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
                if (item == null)
                {
                    _logger.LogWarning($"Country to add is null");
                    return null;
                }

                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                await _context.Country.AddAsync(item);
                await _context.SaveChangesAsync();

                return await _context.Country.FindAsync(item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding new Country: {ex.Message}");
                return null;

            }

        }

        public async Task<Country> DeleteCountry(Country item)
        {
            try
            {
                var existingCountry = await _context.Country.FindAsync(item.Id);
                
                if (existingCountry == null)
                {
                    _logger.LogWarning($"Country with ID {item.Id} not found.");
                    return null;
                }
                existingCountry.LastUpdatedDate = DateTime.Now;
                existingCountry.IsActive = false;

                _context.Country.Update(existingCountry);
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
                var result = await _context.Currency.Where(x => x!.IsActive).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Currency> GetCurrency(int id)
        {
            try
            {
                var result = await _context.Currency.FindAsync(id);
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
                var existingCurrency = await _context.Currency.FindAsync(item.Id);
                
                if (existingCurrency == null)
                {
                    _logger.LogWarning($"Currency with ID {item.Id} not found.");
                    return null;
                }
                // Update the properties of the existing currency
                existingCurrency.Name = item.Name;
                existingCurrency.CurrencyCodeNum3 = item.CurrencyCodeNum3;
                existingCurrency.CurrencyCodeALF3 = item.CurrencyCodeALF3;
                existingCurrency.IsActive = item.IsActive;
                existingCurrency.LastUpdatedDate = DateTime.Now;
                existingCurrency.Description = item.Description;
                existingCurrency.Note = item.Note;

                _context.Currency.Update(existingCurrency);
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
                if (item == null)
                {
                    _logger.LogWarning($"Currency to add is null");
                    return null;
                }

                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                await _context.Currency.AddAsync(item);
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
                var existingCurrency = await _context.Currency.FindAsync(item.Id);
                
                if (existingCurrency == null)
                {
                    _logger.LogWarning($"Currency with ID {item.Id} not found.");
                    return null;
                }
                existingCurrency.LastUpdatedDate = DateTime.Now;
                existingCurrency.IsActive = false;

                _context.Currency.Update(existingCurrency);
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

        public async Task<CurrencyConversionRate> GetCurrencyConversion(int id)
        {
            try
            {
                var result = await _context.CurrencyConversionRates.FindAsync(id);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<CurrencyConversionRate> GetCurrencyRate(string currencyAlf3)
        {
            try
            {
                var result = await _context.CurrencyConversionRates.Where(c => c.IsActive == true && c.CurrencyCodeALF3.ToUpper() == currencyAlf3.ToUpper())
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
                var existingRate = await _context.CurrencyConversionRates.FindAsync(item.Id);
                if (existingRate == null)
                {
                    _logger.LogWarning($"CurrencyConversionRate with ID {item.Id} not found.");
                    return null;
                }
                // Update the properties of the existing rate
                existingRate.RateValue = item.RateValue;
                existingRate.CurrencyCodeALF3 = item.CurrencyCodeALF3;
                existingRate.IsActive = item.IsActive;
                existingRate.LastUpdatedDate = DateTime.Now;
                existingRate.ReferringDate = item.ReferringDate;
                existingRate.UniqueKey = item.UniqueKey;
                existingRate.Note = item.Note;
                
                _context.CurrencyConversionRates.Update(existingRate);
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
            var _startTime = DateTime.Now;
            _logger.LogInformation($"Start Update Currency Conversion Rates ... ");
            
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

                        if (!currentCurrencies.Any(x => x.CurrencyCodeALF3 == currency)) continue;
                        var rate = Decimal.Parse(node.Attributes["rate"].Value, NumberStyles.Any, new CultureInfo("en-Us"));
                        var uniqueK = string.Concat(currency, rate.ToString(CultureInfo.InvariantCulture), DateTime.Now.Date);

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
                        var result = await _context.SaveChangesAsync();
                        
                        _logger.LogInformation($"Currency rate updated in {_utilityService.TimeDiff(_startTime, DateTime.Now)}");
                        return result;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error updating currency rate: {ex.Message}");
                        return -2;
                    }

                }

                _logger.LogWarning($"Currency rates NOT updated {_utilityService.TimeDiff(_startTime, DateTime.Now)}");
                return -1;
            }

            _logger.LogInformation($"Currency rates already updated {_utilityService.TimeDiff(_startTime, DateTime.Now)}");
            return 1;

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
                        var uniqueK = string.Concat(currency, rate.ToString(CultureInfo.InvariantCulture), DateTime.Now.Date);

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
                        var result = await _context.SaveChangesAsync();
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

                await _context.SaveChangesAsync();
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
                var existingUser = await _context.ServiceUser.FindAsync(item.Id);
                if (existingUser == null)
                {
                    _logger.LogWarning($"ServiceUser with ID {item.Id} not found.");
                    return null;
                }
                // Update the properties of the existing user
                existingUser.Name = item.Name;
                existingUser.Surname = item.Surname;
                existingUser.IsActive = item.IsActive;
                existingUser.Note = item.Note;
                existingUser.LastUpdatedDate = DateTime.Now;

                _context.ServiceUser.Update(existingUser);

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

                await _context.ServiceUser.AddAsync(item);
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
                var existingUser = await _context.ServiceUser.FindAsync(item.Id);
                if (existingUser == null)
                {
                    _logger.LogWarning($"ServiceUser with ID {item.Id} not found.");
                    return null;
                }
                
                existingUser.LastUpdatedDate = DateTime.Now;
                existingUser.IsActive = false;

                _context.ServiceUser.Update(existingUser);
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
                var existingSupplier = await _context.suppliers.FindAsync(item.Id);
                if (existingSupplier == null)
                {
                    _logger.LogWarning($"Supplier with ID {item.Id} not found.");
                    return null;
                }
                // Update the properties of the existing supplier
                existingSupplier.Name = item.Name;
                existingSupplier.IsActive = item.IsActive;
                existingSupplier.Note = item.Note;
                existingSupplier.Description = item.Description;
                existingSupplier.Contract = item.Contract;
                existingSupplier.LastUpdatedDate = DateTime.Now;
                existingSupplier.UnitMeasure = item.UnitMeasure;
                existingSupplier.Type = item.Type;

                 _context.suppliers.Update(existingSupplier);
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

                await _context.suppliers.AddAsync(item);
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
                var existingSupplier = await _context.suppliers.FindAsync(item.Id);
                if (existingSupplier == null)
                {
                    _logger.LogWarning($"Supplier with ID {item.Id} not found.");
                    return null;
                }
                
                existingSupplier.LastUpdatedDate = DateTime.Now;
                existingSupplier.IsActive = false;

                _context.suppliers.Update(existingSupplier);
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


        public async Task<ICollection<ReadInBill>?> GetActiveReadInBillList()
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

        public async Task<ReadInBill?> UpdateReadInBill(ReadInBill? item)
        {
            try
            {
                var supplier = await _context.suppliers.FindAsync(item.Supplier.Id);
                var existingBill = await _context.readInBill.FindAsync(item.Id);
                if (existingBill == null)
                {
                    _logger.LogWarning($"ReadInBill with ID {item.Id} not found.");
                    return null;
                }
                // Update the properties of the existing bill
                existingBill.BillProperty = item.BillProperty;
                existingBill.KeyWord = item.KeyWord;
                existingBill.PropertyDataType = item.PropertyDataType;
                existingBill.RegexString = item.RegexString;
                existingBill.Supplier = supplier;
                existingBill.LastUpdatedDate = DateTime.Now;
                existingBill.IsActive = item.IsActive;
                existingBill.Note = item.Note;
                
                _context.readInBill.Update(existingBill);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<ReadInBill?> AddReadInBill(ReadInBill? item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                item.Supplier = await _context.suppliers.FindAsync(item.Supplier.Id);

                await _context.readInBill.AddAsync(item);
                await _context.SaveChangesAsync();

                return await _context.readInBill.FindAsync(item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<ReadInBill?> DeleteReadInBill(ReadInBill? item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                _context.readInBill.Update(item);
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
