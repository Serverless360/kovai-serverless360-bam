using System;

namespace Kovai.Serverless360.Bam
{

    public class InvalidBusinessProcessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBusinessProcessException"/> class.
        /// </summary>
        public InvalidBusinessProcessException()
            : base($"Business process name is required.")
        {
        }
    }
    public class InvalidTransactionNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTransactionNameException"/> class.
        /// </summary>
        public InvalidTransactionNameException()
            : base("Transaction name is required.")
        {
        }
    }
    public class InvalidStageNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidStageNameException"/> class.
        /// </summary>
        public InvalidStageNameException()
            : base("Stage name is required.")
        {
        }
    }
    public class InvalidTransactionInstanceId : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTransactionInstanceId"/> class.
        /// </summary>
        public InvalidTransactionInstanceId()
        : base("Transaction instance id is required.")
        {
        }
    }
    public class InvalidCorrelationProperties : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCorrelationProperties"/> class.
        /// </summary>
        public InvalidCorrelationProperties()
        : base("Correlation Properties is required.")
        {
        }
    }
}
