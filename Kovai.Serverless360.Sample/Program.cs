using Kovai.Serverless360.Bam;

namespace Kovai.Serverless360.Sample
{
  class Program
  {
    static void Main()
    {
      var processor = new OrderProcessor(new TransactionService("key", "FunctionAppUrl"));
      processor.ProcessOrders();
    }
  }
}