using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Interfaces
{
    public interface IAncillaryService
    {
        Task<ICollection<Country>> GetActiveCountryList();
        Task<Country> GetCountry(int countryId);
        Task<Country> AddCountry(Country country);
        Task<Country> UpdateCountry(Country country);
        Task<Country> DeleteCountry(Country country);

        Task<ICollection<Currency>> GetActiveCurrencyList();
        Task<Currency> GetCurrency(int currencyId);
        Task<Currency> AddCurrency(Currency currency);
        Task<Currency> UpdateCurrency(Currency currency);
        Task<Currency> DeleteCurrency(Currency currency);        
        
        Task<ICollection<CurrencyConversionRate>> GetActiveCurrencyConversionList();
        Task<CurrencyConversionRate> GetCurrencyConversion(int currencyConversionRateId);
        Task<CurrencyConversionRate> GetCurrencyRate(string currencyALF3);
        Task<int> UpdateCurrencyRate();
        Task<int> UpdateAllCurrencyRate();
        Task<string> ClearUnusedRates();
        Task<CurrencyConversionRate> AddCurrencyConversion(CurrencyConversionRate currencyConversionRate);
        Task<CurrencyConversionRate> UpdateCurrencyConversion(CurrencyConversionRate currencyConversionRate);
        Task<CurrencyConversionRate> DeleteCurrencyConversion(CurrencyConversionRate currencyConversionRate);


        Task<ICollection<ServiceUser>> GetActiveServiceUserList();
        Task<ServiceUser> GetServiceUser(int serviceUserId);
        Task<ServiceUser> AddServiceUser(ServiceUser serviceUser);
        Task<ServiceUser> UpdateServiceUser(ServiceUser serviceUser);
        Task<ServiceUser> DeleteServiceUser(ServiceUser serviceUser);

        Task<ICollection<Supplier>> GetActiveSupplierList();
        Task<Supplier> GetSupplier(int supplierId);
        Task<Supplier> AddSupplier(Supplier supplier);
        Task<Supplier> UpdateSupplier(Supplier supplier);
        Task<Supplier> DeleteSupplier(Supplier supplier);

        Task<ICollection<ReadInBill>> GetActiveReadInBillList();
        Task<ICollection<ReadInBill>> GetActiveReadInBillBySupplierList(int id);
        Task<ReadInBill> GetReadInBill(int readInBillId);
        Task<ReadInBill> AddReadInBill(ReadInBill readInBill);
        Task<ReadInBill> UpdateReadInBill(ReadInBill readInBill);
        Task<ReadInBill> DeleteReadInBill(ReadInBill readInBill);
    }
}
