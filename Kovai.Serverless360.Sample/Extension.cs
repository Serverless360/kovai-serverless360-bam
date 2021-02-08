using System;
using Kovai.Serverless360.Bam;

namespace Kovai.Serverless360.Sample
{
	public static class Extension
	{
		public static bool IsValid(this FunctionResponse activityResponse)
		{
			if (activityResponse.TransactionInstanceId != Guid.Empty && activityResponse.StageInstanceId != Guid.Empty)
				return true;
			return false;
		}
	}
}
