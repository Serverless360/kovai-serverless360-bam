using Kovai.Serverless360.Bam;
using Newtonsoft.Json;
using System;
using System.Xml;

namespace Kovai.Serverless360.Sample
{
    public class OrderProcessor
    {
        private readonly ITransactionService _service;
        private readonly string _businessProcess;

        public OrderProcessor(ITransactionService service)
        {
            _service = service;
            _businessProcess = "OrderProcessing";
        }

        public void ProcessOrders()
        {
            var businessTransaction = "ValidateOrders";
            string messageBody = "{\"orderId\":1,\"productCode\":\"PR01\",\"Quantity\":1,\"IsValid\":true}";

            /*
            XML data

            string messageBody = "<Valid>false</Valid>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(messageBody); 

            */

            var startTransactionResponse = _service.StartTransaction(new StartTransactionRequest
            {
                BusinessProcess = _businessProcess,
                Transaction = businessTransaction,
                Stage = "getOrders",
                MessageBody = messageBody,
                MessageHeader = "{\"source\":\"website\"}",
                StageStatus = StageStatus.Success
            }).Result;

            if (startTransactionResponse.IsValid())
            {
                _service.CheckPoint(new CheckPointRequest()
                {
                    Stage = "segregateOrders",
                    TransactionInstanceId = startTransactionResponse.TransactionInstanceId,
                    StageStatus = StageStatus.Success,
                    MessageBody = messageBody,
                    MessageHeader = "{\"source\":\"website\"}"
                }).GetAwaiter().GetResult();

                //bool.Parse(doc.SelectSingleNode("Valid").InnerText)

                if (!(bool)JsonConvert.DeserializeObject<dynamic>(messageBody).IsValid)
                {
                    _service.CheckPoint(new CheckPointRequest()
                    {
                        Stage = "acceptOrders",
                        TransactionInstanceId = startTransactionResponse.TransactionInstanceId,
                        StageStatus = StageStatus.Success,
                        MessageBody = messageBody,
                        MessageHeader = "{\"source\":\"website\"}",
                        IsTransactionComplete = true
                    }).GetAwaiter().GetResult();
                }
                else
                {
                    _service.CheckPoint(new CheckPointRequest()
                    {
                        Stage = "declineOrders",
                        TransactionInstanceId = startTransactionResponse.TransactionInstanceId,
                        StageStatus = StageStatus.Failure,
                        MessageBody = messageBody,
                        MessageHeader = "{\"source\":\"website\"}",
                        IsTransactionComplete = true
                    }).GetAwaiter().GetResult();
                }
            }
            else
            {
                Console.WriteLine("We ran into an error, We suggest you to check your API key and Function app URL or it may be due to an network error");
            }
        }
    }
}