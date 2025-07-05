using System.Globalization;
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

        public TransactionService(ILogger<TransactionService> logger, ApplicationContext context,
            IUtilityService utilityService, IMlTransactionService mlservice)
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
                var result = await _context.Transaction.Include(c => c.Account).Include(c => c.Account.Currency)
                    .Where(x => x.IsActive).OrderByDescending(x => x.TxnDate).ToListAsync();


                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving active transactions: {ex.Message}");
                return null;
            }
        }

        public async Task<Transaction> GetTransaction(int balanceId)
        {
            try
            {
                var result = await _context.Transaction.Include(c => c.Account).Include(c => c.Account.Currency)
                    .Where(x => x.Id == balanceId).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving transaction with ID {balanceId}: {ex.Message}");
                return null;
            }
        }

        public async Task<Transaction> UpdateTransaction(Transaction item)
        {
            try
            {
                var existingTransaction = await _context.Transaction.Include(c => c.Account).Where(x => x.Id == item.Id)
                    .FirstOrDefaultAsync();

                if (existingTransaction == null)
                {
                    _logger.LogWarning($"Transaction with ID {item.Id} not found.");
                    return null;
                }

                // Update the properties of the existing transaction
                existingTransaction.TxnDate = item.TxnDate;
                existingTransaction.TxnAmount = item.TxnAmount;
                existingTransaction.Description = item.Description;
                existingTransaction.IsActive = item.IsActive;
                existingTransaction.IsCatConfirmed = item.IsCatConfirmed;
                existingTransaction.UniqueKey = item.UniqueKey;
                existingTransaction.Note = item.Note;

                existingTransaction.LastUpdatedDate = DateTime.Now;

                if (item.Area != null)
                {
                    item.Area = item.Area.ToUpper();
                }

                existingTransaction.Area = item.Area;

                _context.Transaction.Update(existingTransaction);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating transaction with ID {item.Id}: {ex.Message}");
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
                await UpdateTransaction(item);

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error confirming category for transaction with ID {item.Id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Transaction> CategorizeTransaction(Transaction item)
        {
            try
            {
                item.Area = _mlService.PredictCategory(item.Description).ToUpper();
                await UpdateTransaction(item);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error categorizing transaction with ID {item.Id}: {ex.Message}");
                return null;
            }
        }

        public async Task<string> CategorizeAllTransaction()
        {
            var transactionsToCat =
                await _context.Transaction.Where(x => x.IsActive && x.IsCatConfirmed == false).ToListAsync();

            int ok = 0;
            int ko = 0;

            foreach (var item in transactionsToCat)
            {
                try
                {
                    // item.LastUpdatedDate = DateTime.Now;
                    item.Area = _mlService.PredictCategory(item.Description);

                    await UpdateTransaction(item);
                    // var result = _context.Transaction.Update(item);
                    // await _context.SaveChangesAsync();
                    ok++;
                }
                catch (Exception ex)
                {
                    ko++;
                    _logger.LogError($"Error categorizing transaction with ID {item.Id}: {ex.Message}");
                }
            }

            return $"File categorized: {ok}, with error: {ko} (check the log)";
        }

        public async Task<string> TrainModelTransaction()
        {
            try
            {
                var result = _mlService.TrainAndSaveModel();

                return !string.IsNullOrEmpty(result) ? result : "Error in train model: check the log";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error training model: {ex.Message}");
                return null;
            }
        }

        public async Task<Transaction> AddTransaction(Transaction item)
        {
            var account = await _context.AccountMasterData.Include(c => c.Currency)
                .Where(x => x.Id == item.Account.Id)
                .FirstOrDefaultAsync();

            try
            {
                item.LastUpdatedDate = DateTime.Now;
                if (item.Area != null)
                {
                    item.Area = item.Area.ToUpper();
                }

                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.UniqueKey = MD5UniqueKey(item.Account.Name, item.TxnDate.ToString(CultureInfo.InvariantCulture),
                    item.TxnAmount.ToString(CultureInfo.InvariantCulture), item.Description);
                item.Account = account;

                if (_context.Transaction.Any(x => x.UniqueKey == item.UniqueKey))
                {
                    //record already present: Duplicate Record
                    item.Id = 0;
                    return item;
                }

                await _context.Transaction.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding transaction: {ex.Message}");
                return null;
            }
        }

        public async Task<Transaction> DeleteTransaction(Transaction item)
        {
            try
            {
                var existingTransaction = await _context.Transaction.Include(c => c.Account)
                    .Where(x => x.Id == item.Id).FirstOrDefaultAsync();
                
                if (existingTransaction == null)
                {
                    _logger.LogWarning($"Transaction with ID {item.Id} not found for deletion.");
                    return null;
                }
                
                existingTransaction.LastUpdatedDate = DateTime.Now;
                existingTransaction.IsActive = false;

                _context.Transaction.Update(existingTransaction);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting transaction with ID {item.Id}: {ex.Message}");
                return null;
            }
        }

        public async Task<string> UploadCsv(IList<Transaction> transactions)
        {
            if (transactions.Count==0)
            {
                _logger.LogWarning("No transactions to upload.");
                return string.Empty;

            }

            int loaded = 0;
            int notLoaded = 0;
            int duplicated = 0;

            foreach (var item in transactions)
            {
                var result = await AddTransaction(item);

                if (result is null)
                {
                    //return BadRequest("Error adding Transaction");
                    notLoaded++;
                }

                if (result.Id < 1)
                {
                    duplicated++;
                }

                if (result.Id > 0)
                {
                    loaded++;
                }

            }

            return $"Loaded: {loaded} - Not Loaded: {notLoaded} - Duplicated: {duplicated}";
        }

        private string MD5UniqueKey(string accountName, string txnDate, string txnAmount, string txnDescription)
        {
            using MD5 md5Hash = MD5.Create();
            var hash = GetMd5Hash(md5Hash, string.Concat(accountName, txnDate, txnAmount, txnDescription));

            return hash;
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