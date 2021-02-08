using Kovai.Serverless360.Bam;
using Microsoft.Extensions.Logging;

namespace Kovai.Samples.FuncApp
{
	public class Logger :  IBamActivityLogger
	{
		private readonly ILogger _logger;

		public Logger(ILogger logger)
		{
			_logger = logger;
		}
		public void Debug(string message)
		{
			_logger.LogDebug(message);
		}

		public void Info(string message)
		{
			_logger.LogInformation(message);
		}

		public void Warning(string message)
		{
			_logger.LogWarning(message);
		}

		public void Error(string message)
		{
			_logger.LogError(message);
		}

		public void Fatal(string message)
		{
			throw new System.NotImplementedException();
		}
	}
}
