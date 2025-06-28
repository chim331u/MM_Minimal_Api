using MoneyManagement.Models.Transactions;

namespace MoneyManagement.Interfaces
{
    public interface IMlTransactionService
    {
        string PredictCategory(string fileNameToPredict);
        string TrainAndSaveModel();
        void AddToTrain(Transaction transaction);
    }
}
