using System;
using System.Runtime.Serialization;

namespace Kovai.Serverless360.Bam
{
    public class StartTransactionRequest
    {
        /// <summary>
        /// Gets or sets the business process.
        /// </summary>
        /// <value>
        /// The business process.
        /// </value>		
        [DataMember]
        public string BusinessProcess { get; set; }
        /// <summary>
        /// Gets or sets the business transaction.
        /// </summary>
        /// <value>
        /// The business transaction.
        /// </value>
        [DataMember]
        public string Transaction { get; set; }
        /// <summary>
        /// Gets or sets the stage.
        /// </summary>
        /// <value>
        /// The stage.
        /// </value>
        [DataMember]
        public string Stage { get; set; }
        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        /// <value>
        /// The message body.
        /// </value>
        [DataMember]
        public string MessageBody { get; set; }
        /// <summary>
        /// Gets or sets the message header.
        /// </summary>
        /// <value>
        /// The message header.
        /// </value>
        [DataMember]
        public string MessageHeader { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is archive enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is archive enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ArchiveMessage { get; set; }

        /// <summary>
        /// Gets or sets the value to check whether this transaction instance is batched.
        /// </summary>
        /// <value>
        /// The is batched transaction.
        /// </value>
        [DataMember]
        public bool? IsBatchedTransaction { get; set; }

        /// <summary>
        /// Gets or sets the batch identifier.
        /// </summary>
        /// <value>
        /// The batch identifier.
        /// </value>
        [DataMember]
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or sets the value to check whether this stage instance is batched.
        /// </summary>
        /// <value>
        /// The is batched transaction.
        /// </value>
        [DataMember]
        public bool? IsBatchedStage { get; set; }

        /// <summary>
        /// Gets or sets the stage batch identifier.
        /// </summary>
        /// <value>
        /// The batch identifier.
        /// </value>
        [DataMember]
        public string StageBatchId { get; set; }

        /// <summary>
        /// Gets or sets the stage status.
        /// </summary>
        /// <value>
        /// The stage status.
        /// </value>
        [DataMember]
        public StageStatus? StageStatus { get; set; }

        /// <summary>
        /// Gets or sets the is transaction complete.
        /// </summary>
        /// <value>
        /// The is transaction complete.
        /// </value>
        [DataMember]
        public bool? IsTransactionComplete { get; set; }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        /// <value>
        /// The is exception message.
        /// </value>
        [DataMember]
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception code.
        /// </summary>
        /// <value>
        /// The is exception code.
        /// </value>
        [DataMember]
        public string ExceptionCode { get; set; }

        /// <summary>
        /// Gets or sets the Transaction Instance Id.
        /// </summary>
        /// <value>
        /// The is Transaction Instance Id.
        /// </value>
        [DataMember]
        public Guid? TransactionInstanceId { get; set; }


        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <exception cref="InvalidMessageBody"></exception>
        /// <exception cref="InvalidMessageHeader"></exception>
        /// <exception cref="Kovai.Serverless360.Bam.InvalidTransactionException"></exception>
        /// <exception cref="Kovai.Serverless360.Bam.InvalidStageNameException"></exception>
        public void Validate()
        {
            if (BusinessProcess.IsNullOrEmpty()) throw new InvalidBusinessProcessException();
            if (Transaction.IsNullOrEmpty()) throw new InvalidTransactionNameException();
        }
    }

    [DataContract]
    public class FunctionResponse
    {
        /// <summary>
        /// Gets or sets the transaction instance identifier.
        /// </summary>
        /// <value>
        /// The transaction instance identifier.
        /// </value>
        [DataMember]
        public Guid TransactionInstanceId { get; set; }
        /// <summary>
        /// Gets or sets the stage instance identifier.
        /// </summary>
        /// <value>
        /// The stage instance identifier.
        /// </value>
        [DataMember]
        public Guid StageInstanceId { get; set; }
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// Result.
        /// </value>
        [DataMember]
        public string Result { get; set; }
    }

    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets the transaction instance identifier.
        /// </summary>
        /// <value>
        /// The transaction instance identifier.
        /// </value>
        [DataMember]
        public string TransactionInstanceId { get; set; }
        /// <summary>
        /// Gets or sets the stage instance identifier.
        /// </summary>
        /// <value>
        /// The stage instance identifier.
        /// </value>
        [DataMember]
        public string StageInstanceId { get; set; }
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// Result.
        /// </value>
        [DataMember]
        public string Result { get; set; }
    }
}