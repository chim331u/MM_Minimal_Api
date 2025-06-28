using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Transactions;

namespace MoneyManagement.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IMlTransactionService _mlService;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ILogger<TransactionService> logger, ApplicationContext context, IUtilityService utilityService, IMlTransactionService mlservice)
        {
            _context = context;
            _utilityService = utilityService;
            _mlService = mlservice;
            _logger = logger;
        }

        public async Task<ICollection<Transaction>> GetActiveTransactionList()
        {

            try
            {
                var result = await _context.Transaction.Include(c => c.Account).Include(c => c.Account.Currency).Where(x => x.IsActive).OrderByDescending(x => x.TxnDate).ToListAsync();

                
                return result;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }

        public async Task<Transaction> GetTransaction(int balanceId)
        {
            try
            {
                var result = await _context.Transaction.Include(c => c.Account).Include(c=>c.Account.Currency).Where(x => x.Id == balanceId).FirstOrDefaultAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Transaction> UpdateTransaction(Transaction item)
        {

            try
            {
                item.LastUpdatedDate = DateTime.Now;
                if (item.Area != null)
                {
                    item.Area = item.Area.ToUpper();
                }

                var result = _context.Transaction.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Transaction> CategoryConfirmed(Transaction item)
        {
            if (item.IsCatConfirmed)
            {
                //Confirmed Category
                //add train data
                _mlService.AddToTrain(item);
            }

            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.Transaction.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }        
        
        public async Task<Transaction> CategorizeTransaction(Transaction item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.Area = _mlService.PredictCategory(item.Description).ToUpper();

                var result = _context.Transaction.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }        
        
        public async Task<string> CategorizeAllTransaction()
        {
            var transactionsToCat = await _context.Transaction.Where(x => x.IsActive && x.IsCatConfirmed == false).ToListAsync();

            int ok = 0;
            int ko = 0;

            foreach (var item in transactionsToCat)
            {
                try
                {
                    item.LastUpdatedDate = DateTime.Now;
                    item.Area = _mlService.PredictCategory(item.Description);

                    var result = _context.Transaction.Update(item);
                    await _context.SaveChangesAsync();
                    ok++;
                }
                catch (Exception ex)
                {
                    ko++;
                    _logger.LogError(ex.Message);

                }
            }

            return $"File categorized: {ok}, with error: {ko} (check the log)";

        }

        public async Task<string> TrainModelTransaction()
        {
            try
            {
                var result = _mlService.TrainAndSaveModel();

                if (result!=null)
                {
                    return result;
                }
                else
                {
                    return "Error in train model: check the log";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }

        public async Task<Transaction> AddTransaction(Transaction item)
        {
            var account = await _context.AccountMasterData.Include(c=>c.Currency).Where(x=>x.Id == item.Account.Id).FirstOrDefaultAsync();
           
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                if (item.Area != null)
                {
                    item.Area = item.Area.ToUpper();
                }
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.UniqueKey = MD5UniqueKey(item.Account.Name, item.TxnDate.ToString(), item.TxnAmount.ToString(), item.Description);
                item.Account = account;

                if (_context.Transaction.Where(x => x.UniqueKey == item.UniqueKey).Any())
                {
                    //record already present: Duplicate Record
                    item.Id = 0;
                    return item;
                }

                var result = await _context.Transaction.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Transaction> DeleteTransaction(Transaction item)
        {
           
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.Transaction.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        private string MD5UniqueKey(string accountName, string txnDate, string txnAmount, string txnDescription)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, string.Concat(accountName, txnDate, txnAmount, txnDescription));

                return hash;
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
