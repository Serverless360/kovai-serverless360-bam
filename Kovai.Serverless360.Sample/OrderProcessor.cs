using Kovai.Serverless360.Bam;
using Newtonsoft.Json;

namespace Kovai.Serverless360.Sample
{
  public class OrderProcessor
  {
    private readonly ITransactionService _service;
    private readonly string _businessProcess;

    public OrderProcessor(ITransactionService service)
    {
      _service = service;
      _businessProcess = "Order Processing";
    }

    public void ProcessOrders()
    {
      var businessTransaction = "Process Orders";
      string messageBody = "{\"orderId\":1,\"productCode\":\"PR01\",\"Quantity\":1,\"IsValid\":true}";

      var startTransactionResponse = _service.StartTransaction(new StartTransactionRequest
      {
        BusinessProcess = _businessProcess,
        Transaction = businessTransaction,
        Stage = "Get Order",
        MessageBody = messageBody,
        MessageHeader = "{\"source\":\"website\"}",
        StageStatus = StageStatus.Success
      }).Result;

      if (startTransactionResponse.IsValid())
      {
        _service.CheckPoint(new CheckPointRequest()
        {
          Stage = "Validate Order",
          TransactionInstanceId = startTransactionResponse.TransactionInstanceId,
          StageStatus = StageStatus.Success,
          MessageBody = messageBody,
          MessageHeader = "{\"source\":\"website\"}"
        }).GetAwaiter().GetResult();
      }
      if (!(bool)JsonConvert.DeserializeObject<dynamic>(messageBody).IsValid)
      {
        _service.CheckPoint(new CheckPointRequest()
        {
          Stage = "Decline Order",
          TransactionInstanceId = startTransactionResponse.TransactionInstanceId,
          StageStatus = StageStatus.Failure,
          MessageBody = messageBody,
          MessageHeader = "{\"source\":\"website\"}",
          ExceptionCode = "500",
          ExceptionMessage = "Invalid Order",
          IsTransactionComplete = true
        }).GetAwaiter().GetResult();
      }
      else
      {
        var _receiveResponse = _service.CheckPoint(new CheckPointRequest()
        {
          Stage = "Accept Order",
          TransactionInstanceId = startTransactionResponse.TransactionInstanceId,
          StageStatus = StageStatus.InProgress,
          MessageBody = messageBody,
          MessageHeader = "{\"source\":\"website\"}"
        }).GetAwaiter().GetResult();

        _service.CheckPoint(new CheckPointRequest()
        {
          StageInstanceId = _receiveResponse.StageInstanceId,
          TransactionInstanceId = startTransactionResponse.TransactionInstanceId,
          StageStatus = StageStatus.Success,
          MessageBody = messageBody,
          MessageHeader = "{\"source\":\"website\"}"
        }).GetAwaiter().GetResult();

        _service.CheckPoint(new CheckPointRequest()
        {
          Stage = "Process Order",
          TransactionInstanceId = startTransactionResponse.TransactionInstanceId,
          StageStatus = StageStatus.Success,
          MessageBody = messageBody,
          MessageHeader = "{\"source\":\"website\"}"
        }).GetAwaiter().GetResult();

        _service.CheckPoint(new CheckPointRequest()
        {
          Stage = "Mark order as complete",
          TransactionInstanceId = startTransactionResponse.TransactionInstanceId,
          StageStatus = StageStatus.Success,
          MessageBody = messageBody,
          MessageHeader = "{\"source\":\"website\"}",
          IsTransactionComplete = true
        }).GetAwaiter().GetResult();

      }
    }
  }
}