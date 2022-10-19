using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Kovai.Serverless360.Bam.Tests")]
namespace Kovai.Serverless360.Bam
{
    /// <summary>
    /// Provides a base class to send events to Serverless360 APIs.
    /// </summary>
    /// <seealso cref="Kovai.Serverless360.Bam.ITransactionService" />
    public class TransactionService : ITransactionService
    {
        private readonly string _key;
        private readonly string _url;
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="url">The URL.</param>
        public TransactionService(string key, string url)
        {
            _key = key;
            _url = url;
            _client = new HttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="client">The client.</param>
        internal TransactionService(string key, HttpClient client)
        {
            _key = key;
            _url = Constants.FunctionUrlPattern;
            _client = client;
        }

        /// <summary>
        /// Starts the transaction.
        /// </summary>
        /// <param name="transactionRequest">The transaction request.</param>
        /// <returns></returns>
        public async Task<FunctionResponse> StartTransaction(StartTransactionRequest transactionRequest)
        {
            var result = new FunctionResponse();
            try
            {
                transactionRequest.Validate();

                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.BusinessProcess, transactionRequest.BusinessProcess);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.Transaction, transactionRequest.Transaction);
                if (!string.IsNullOrEmpty(transactionRequest.Stage))
                    _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.Stage, transactionRequest.Stage);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ArchiveMessage, Convert.ToString(transactionRequest.ArchiveMessage));
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.StageStatus, transactionRequest.StageStatus?.ToString());
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ExceptionMessage, transactionRequest.ExceptionMessage);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ExceptionCode, transactionRequest.ExceptionCode);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.IsBatchedTransaction, transactionRequest.IsBatchedTransaction != null ? Convert.ToString(transactionRequest.IsBatchedTransaction) : null);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.BatchId, transactionRequest.BatchId);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.IsBatchedStage, transactionRequest.IsBatchedStage != null ? Convert.ToString(transactionRequest.IsBatchedStage) : null);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.StageBatchId, transactionRequest.StageBatchId);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.IsTransactionComplete, transactionRequest.IsTransactionComplete != null ? Convert.ToString(transactionRequest.IsTransactionComplete) : null);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ExecutedAt, "custom");
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.Key, _key);

                if (transactionRequest.MessageHeader == null)
                    transactionRequest.MessageHeader = "{\"Content-Type\":\"application/json\"}";
                if (transactionRequest.MessageBody == null)
                    transactionRequest.MessageBody = "{}";

                var header = transactionRequest.MessageHeader.DeSerialize<Dictionary<string, string>>();
                if (header != null)
                    header["Content-Type"] = "application/json";
                var body = new MessageContent
                {
                    MessageBody = transactionRequest.MessageBody,
                    MessageHeader = header?.Serialize<Object>()
                };


                var uri = $"{_url}/api/{Constants.Operations.StartTransaction}";
                var data = body.Serialize();
                var response = await _client.PostAsync(uri, new StringContent(data, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsJsonAsync<ApiResponse>();
                    result = new FunctionResponse
                    {
                        TransactionInstanceId = Guid.Parse(responseContent.TransactionInstanceId),
                        StageInstanceId = string.IsNullOrEmpty(responseContent.StageInstanceId) ? Guid.Empty : Guid.Parse(responseContent.StageInstanceId),
                        Result = responseContent.Result
                    };
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Check point Transaction.
        /// </summary>
        /// <param name="checkPointRequest">The check point request.</param>
        /// <returns></returns>
        public async Task<FunctionResponse> CheckPoint(CheckPointRequest checkPointRequest)
        {
            var result = new FunctionResponse();
            try
            {
                checkPointRequest.Validate();

                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.TransactionInstanceId, checkPointRequest.TransactionInstanceId.ToString());
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.Stage, checkPointRequest.Stage);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.StageInstanceId, checkPointRequest.StageInstanceId.ToString());
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ArchiveMessage, Convert.ToString(checkPointRequest.ArchiveMessage));
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.StageStatus, checkPointRequest.StageStatus?.ToString());
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ExceptionMessage, checkPointRequest.ExceptionMessage);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ExceptionCode, checkPointRequest.ExceptionCode);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.IsBatchedStage, checkPointRequest.IsBatchedStage != null ? Convert.ToString(checkPointRequest.IsBatchedStage) : null);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.IsTransactionComplete, checkPointRequest.IsTransactionComplete != null ? Convert.ToString(checkPointRequest.IsTransactionComplete) : null);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ExecutedAt, "custom");
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.Key, _key);


                if (checkPointRequest.MessageHeader == null)
                    checkPointRequest.MessageHeader = "{\"Content-Type\":\"application/json\"}";
                if (checkPointRequest.MessageBody == null)
                    checkPointRequest.MessageBody = "{}";

                var header = checkPointRequest.MessageHeader.DeSerialize<Dictionary<string, object>>();
                if (header != null)
                    header["Content-Type"] = "application/json";
                var body = new MessageContent
                {
                    MessageBody = checkPointRequest.MessageBody,
                    MessageHeader = header?.Serialize()
                };

                var uri = $"{_url}/api/{Constants.Operations.CheckPoint}";

                var data = body.Serialize();
                var response = await _client.PostAsync(uri, new StringContent(data, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsJsonAsync<FunctionResponse>();
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Correlation Check point Transaction.
        /// </summary>
        /// <param name="correlationcheckPointRequest">The correlation check point request.</param>
        /// <returns></returns>
        public async Task<bool> CorrelationCheckPoint(CorrelationCheckPointRequest correlationCheckPointRequest)
        {
            try
            {
                correlationCheckPointRequest.Validate();

                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.Stage, correlationCheckPointRequest.Stage);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ArchiveMessage, Convert.ToString(correlationCheckPointRequest.ArchiveMessage));
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.StageStatus, correlationCheckPointRequest.StageStatus?.ToString());
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ExceptionMessage, correlationCheckPointRequest.ExceptionMessage);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ExceptionCode, correlationCheckPointRequest.ExceptionCode);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.IsBatchedTransaction, correlationCheckPointRequest.IsBatchedTransaction != null ? Convert.ToString(correlationCheckPointRequest.IsBatchedTransaction) : null);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.IsBatchedStage, correlationCheckPointRequest.IsBatchedStage != null ? Convert.ToString(correlationCheckPointRequest.IsBatchedStage) : null);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.BatchId, correlationCheckPointRequest.BatchId);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.StageBatchId, correlationCheckPointRequest.StageBatchId);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.IsTransactionComplete, correlationCheckPointRequest.IsTransactionComplete != null ? Convert.ToString(correlationCheckPointRequest.IsTransactionComplete) : null);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.BusinessProcess, correlationCheckPointRequest.BusinessProcess);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.Transaction, correlationCheckPointRequest.Transaction);
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.IgnoreNotFound, Convert.ToString(correlationCheckPointRequest.IgnoreNotFound));
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.ExecutedAt, "custom");
                _client.DefaultRequestHeaders.AddOrReplace(Constants.Headers.Key, _key);


                if (correlationCheckPointRequest.MessageHeader == null)
                    correlationCheckPointRequest.MessageHeader = "{\"Content-Type\":\"application/json\"}";
                if (correlationCheckPointRequest.MessageBody == null)
                    correlationCheckPointRequest.MessageBody = "{}";
                if (correlationCheckPointRequest.CorrelationProperties == null)
                    correlationCheckPointRequest.CorrelationProperties = new Dictionary<string, object>();

                var header = correlationCheckPointRequest.MessageHeader.DeSerialize<Dictionary<string, object>>();
                if (header != null)
                    header["Content-Type"] = "application/json";
                var body = new MessageContent
                {
                    MessageBody = correlationCheckPointRequest.MessageBody,
                    MessageHeader = header?.Serialize(),
                    Property = correlationCheckPointRequest.CorrelationProperties.Select(c => new Property() { Name = c.Key, Value = c.Value }).ToList()
                };

                var uri = $"{_url}/api/{Constants.Operations.CorrelationCheckPoint}";

                var data = body.Serialize();
                var content = await _client.PostAsync(uri, new StringContent(data, Encoding.UTF8, "application/json"));
                return content.IsSuccessStatusCode;
            }
            catch
            {
            }

            return false;
        }

    }
}