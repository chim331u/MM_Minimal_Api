using MoneyManagement.Models.IdentityAccess;

namespace MoneyManagement.Interfaces
{
    public interface IUtilityService
    {
        //string WriteLog(LogType logType, string message);


        #region Data Protection
        string EncryptString(string plainText);

        string EncryptString(ISA_Accounts item);

        string DecryptString(string cipherText);

        string DecryptString(ISA_Accounts item);
        #endregion

        string TimeDiff(DateTime start, DateTime end);
    }
}
