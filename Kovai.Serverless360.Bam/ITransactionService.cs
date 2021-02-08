using System.Threading.Tasks;

namespace Kovai.Serverless360.Bam
{
  public interface ITransactionService
  {
    Task<FunctionResponse> StartTransaction(StartTransactionRequest activityRequest);
    Task<FunctionResponse> CheckPoint(CheckPointRequest activityRequest);
    Task<bool> CorrelationCheckPoint(CorrelationCheckPointRequest activityRequest);
  }
}
