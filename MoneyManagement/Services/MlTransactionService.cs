using Microsoft.ML;
using Microsoft.ML.Data;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Transactions;

namespace MoneyManagement.Services
{
    public class MlTransactionService : IMlTransactionService
    {
        private readonly ApplicationContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IConfiguration _config;
        private readonly ILogger<MlTransactionService> _logger;
        ITransformer loadedModel;

        public MlTransactionService(ILogger<MlTransactionService> logger, ApplicationContext context, IUtilityService utilityService, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _utilityService = utilityService;
            _config = config;
            _mlContext = new MLContext(seed: 0);
            LoadConfig();
            CheckFolderAndFiles();
            loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);
        }


        private void LoadConfig()
        {
            TransactionMlConfig.MlPath = _config["TransactionMlSettings:MlDir"];
            TransactionMlConfig.TrainFileName = _config["TransactionMlSettings:TrainName"];
            TransactionMlConfig.TestFileName = _config["TransactionMlSettings:TestName"];
            TransactionMlConfig.ModelName = _config["TransactionMlSettings:ModelName"];
        }

        #region MachineLearning

        static IDataView _trainingDataView;
        private MLContext _mlContext;
        private static PredictionEngine<TransactionMlImportedFileFormat, TransactionMlNamePrediction> _predEngine;
        private static ITransformer _trainedModel;

        public static string _trainDataPath => Path.Combine(TransactionMlConfig.MlPath, TransactionMlConfig.TrainFileName);
        private static string _testDataPath => Path.Combine(TransactionMlConfig.MlPath, TransactionMlConfig.TestFileName);
        private static string _modelPath => Path.Combine(TransactionMlConfig.MlPath, TransactionMlConfig.ModelName);


        //train

        public IEstimator<ITransformer> BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            _trainedModel = trainingPipeline.Fit(trainingDataView);

            _predEngine = _mlContext.Model.CreatePredictionEngine<TransactionMlImportedFileFormat, TransactionMlNamePrediction>(_trainedModel);

            return trainingPipeline;
        }
        public void TrainModel()
        {
            _logger.LogInformation("Training Model: Start load data from DB for train");
            //_mlContext = new MLContext(seed: 0);

            _trainingDataView = _mlContext.Data.LoadFromTextFile<TransactionMlImportedFileFormat>(_trainDataPath, hasHeader: false, separatorChar: ';');

            var pipeline = ProcessData();

            var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);

        }

        public string TrainAndSaveModel()
        {
            _logger.LogInformation("Training Model: Start load data from DB for train");

            CheckFolderAndFiles();

            //_mlContext = new MLContext(seed: 0);

            try
            {
                _trainingDataView = _mlContext.Data.LoadFromTextFile<TransactionMlImportedFileFormat>(_trainDataPath, hasHeader: false, separatorChar: ';');

                var pipeline = ProcessData();

                var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);

                _logger.LogInformation("Training Model: Completed");

                _logger.LogInformation("Saving Model");
                SaveModelAsFile(_mlContext, _trainingDataView.Schema, _trainedModel);

                _logger.LogInformation("Model Saved");
                return "Model trained and saved";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Training Model: ERROR {ex.Message}");
                return null;
            }



        }

        public void ForceTrainAndSaveModel()
        {
            _logger.LogInformation("Training Model: Start load data from DB for train");

            CheckFolder(TransactionMlConfig.MlPath);
            CreateNewTrainFile();

            try
            {
                //_mlContext = new MLContext(seed: 0);

                _trainingDataView = _mlContext.Data.LoadFromTextFile<TransactionMlImportedFileFormat>(_trainDataPath, hasHeader: false, separatorChar: ';');

                var pipeline = ProcessData();

                var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);

                _logger.LogInformation("Training Model: Completed");

                _logger.LogInformation("Saving Model");
                SaveModelAsFile(_mlContext, _trainingDataView.Schema, _trainedModel);
                _logger.LogInformation("Model Saved");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Training Model: ERROR {ex.Message}");
            }

        }

        private void SaveModelAsFile(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            mlContext.Model.Save(model, trainingDataViewSchema, _modelPath);
        }

        public void AddToTrain(Transaction transaction )
        {
            if ( transaction != null ) {

                using (StreamWriter sw = System.IO.File.AppendText(_trainDataPath))
                {
                    sw.WriteLine(transaction.Id + ";" + transaction.Area.ToUpper() + ";" + transaction.Description + ";" + transaction.TxnAmount +";");
                }
                _logger.LogInformation($"Transaction {transaction.Id} added to Train File");
            }
        }


        //Categorize

        public string PredictCategory(string fileNameToPredict)
        {

            //_mlContext = new MLContext(seed: 0);


            //ITransformer loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);

            TransactionMlImportedFileFormat trx = new TransactionMlImportedFileFormat()
            {
                Name = fileNameToPredict
            };

            _predEngine = _mlContext.Model.CreatePredictionEngine<TransactionMlImportedFileFormat, TransactionMlNamePrediction>(loadedModel);

            var prediction = _predEngine.Predict(trx);
            _logger.LogInformation($"{fileNameToPredict} ===> Category Predicted: {prediction.Area} <=== ");
            return prediction.Area;

        }

        public List<Tuple<string, decimal>> PredictCategoryWithScore(string fileNameToPredict)
        {
            //CheckFolderAndFiles();

            //_mlContext = new MLContext(seed: 0);

            var labelBuffer = new VBuffer<ReadOnlyMemory<char>>();


            //ITransformer loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);

            TransactionMlImportedFileFormat trx = new TransactionMlImportedFileFormat()
            {
                Name = fileNameToPredict
            };

            _predEngine = _mlContext.Model.CreatePredictionEngine<TransactionMlImportedFileFormat, TransactionMlNamePrediction>(loadedModel);

            _predEngine.OutputSchema["Score"].Annotations.GetValue("SlotNames", ref labelBuffer);
            var labels = labelBuffer.DenseValues().Select(l => l.ToString()).ToArray();

            var prediction = _predEngine.Predict(trx);


            var index = Array.IndexOf(labels, prediction.Area);
            var score = prediction.Score[index];

            List<Tuple<string, decimal>> predictionResult = new List<Tuple<string, decimal>>();


            var top10scores = labels.ToDictionary(l => l, l => (decimal)prediction.Score[Array.IndexOf(labels, l)]).OrderByDescending(kv => kv.Value).Take(10);

            _logger.LogInformation($"File: {fileNameToPredict}");
            _logger.LogInformation("Top 10 prediction");
            _logger.LogInformation("-------------------------------");
            
            foreach (var item in top10scores)
            {
                _logger.LogInformation($"Category: {item.Key} - Score: {item.Value}");
                predictionResult.Add(new Tuple<string, decimal>(item.Key, item.Value));
            }
            _logger.LogInformation("-------------------------------");
            _logger.LogInformation($"{fileNameToPredict} ====>Category Predicted: {prediction.Area} == with score : {(top10scores.FirstOrDefault().Value * 100)} %");


            return predictionResult;


        }

        //evaluate
        public void EvaluateModel()
        {
            _logger.LogInformation("Start Evaluating Model");
            //_mlContext = new MLContext(seed: 0);

            _trainingDataView = _mlContext.Data.LoadFromTextFile<TransactionMlImportedFileFormat>(_trainDataPath, hasHeader: false, separatorChar: ';');

            DataViewSchema trainingDataViewSchema = _trainingDataView.Schema;

            var pipeline = ProcessData();

            var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);

            var testDataView = _mlContext.Data.LoadFromTextFile<TransactionMlImportedFileFormat>(_testDataPath, hasHeader: false, separatorChar: ';');
            var testMetrics = _mlContext.MulticlassClassification.Evaluate(_trainedModel.Transform(testDataView));

            _logger.LogInformation($"*************************************************************************************************************");
            _logger.LogInformation($"*       Metrics for Multi-class Classification model - Test Data     ");
            _logger.LogInformation($"*------------------------------------------------------------------------------------------------------------");
            _logger.LogInformation($"*       MicroAccuracy:    {testMetrics.MicroAccuracy:0.###}");
            _logger.LogInformation($"*       MacroAccuracy:    {testMetrics.MacroAccuracy:0.###}");
            _logger.LogInformation($"*       LogLoss:          {testMetrics.LogLoss:#.###}");
            _logger.LogInformation($"*       LogLossReduction: {testMetrics.LogLossReduction:#.###}");
            _logger.LogInformation($"*************************************************************************************************************");

        }

        //Other ML
        private IEstimator<ITransformer> ProcessData()
        {
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Area", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Name", outputColumnName: "NameFeaturized"))
                .Append(_mlContext.Transforms.Concatenate("Features", "NameFeaturized"))
                .AppendCacheCheckpoint(_mlContext);

            return pipeline;
        }

        private void CheckFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private void CheckFolderAndFiles()
        {
            CheckFolder(TransactionMlConfig.MlPath);

            if (!File.Exists(_trainDataPath))
            {
                CreateNewTrainFile();
            }

            if (!File.Exists(_modelPath))
            {
                TrainAndSaveModel();
            }
        }

        private void CreateNewTrainFile()
        {
            _logger.LogInformation("Start creating train file");

            File.WriteAllText(_trainDataPath, "");

            var recordsToTrain = _context.Transaction.Where(t => t.IsActive == true && !string.IsNullOrEmpty(t.Area) && t.IsCatConfirmed).ToList();

            using (StreamWriter sw = File.AppendText(_trainDataPath))
            {
                foreach (var item in recordsToTrain)
                {
                    sw.WriteLine(item.Id + ";" + item.Area + ";" + item.Description + ";" + item.TxnAmount + ";");
                }

                sw.Close();
            }

            _logger.LogInformation($"Train file created with {recordsToTrain.Count} records");

        }

        #endregion
    }
}
