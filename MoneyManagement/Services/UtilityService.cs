using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.IdentityAccess;

namespace MoneyManagement.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly ApplicationContext _context;
        private string myKey;
        private readonly ILogger<UtilityService> _logger;

        public UtilityService(ILogger<UtilityService> logger, ApplicationContext context)
        {
            _context = context;
            _logger = logger;
            myKey = _context.ServiceConfigs.Where(k => k.Key == "CryptoKey").FirstOrDefault().Value.ToString();
        }

        //public string WriteLog(LogType logType, string message)
        //{
        //    var logMessage = $"{DateTime.Now}  [{logType}] - {message}";

        //    Console.WriteLine(logMessage);

        //    return logMessage + Environment.NewLine;
        //}

        #region DataProtection


        public string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            string key = "";

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string EncryptString(ISA_Accounts item)
        {
            string key = string.Concat(myKey, item.CreatedDate.ToString(CultureInfo.InvariantCulture));
            string plainText = item.Password;
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            string key = "";

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public string DecryptString(ISA_Accounts item)
        {
            try
            {
                string key = string.Concat(myKey, item.CreatedDate.ToString(CultureInfo.InvariantCulture));
                string cipherText = item.Password;
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);


                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    aes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                var clearPsw = streamReader.ReadToEnd();
                                return clearPsw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            
        }

        #endregion
        
        
        public string TimeDiff(DateTime start, DateTime end)
        {
            TimeSpan _span = end - start;
            return string.Concat(((int)_span.TotalMilliseconds).ToString(), " ms");
        }
    }
}
