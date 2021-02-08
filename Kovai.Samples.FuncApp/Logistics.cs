#region Using Directives

using System.Threading.Tasks;
using Kovai.Serverless360.Bam;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

#endregion

namespace Kovai.Samples.FuncApp
{
	public static class Logistics
	{
		[FunctionName("BookingRequest")]
		public static async Task<IActionResult> Run(
				[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
				ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			//Add your own Logger for Info / Sending Notifications etc..
			var logger = new Logger(log);
			ITransactionService service = new ActivityService("ggDoKMKQF0BDDeNYyImfxgvhzOQ72u7fgQUixQguZqf94pwqyUpiTg==", logger);
			var response = await service.StartTransaction(new StartTransactionRequest()
			{
				BusinessProcess = "Ship Any Where Logistics",
				BusinessTransaction = "Booking Request",
				CurrentStage = "Receive",
				PreviousStage = ".",
				IsArchiveEnabled = true,
				MessageBody = "{\"some\":1}",
				MessageHeader = "{\"some\":1}",
			});

			return new OkObjectResult(response);
		}
	}
}
