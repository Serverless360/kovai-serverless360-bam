using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kovai.Serverless360.Bam
{
  [DataContract]
  public class CorrelationCheckPointRequest
  {
    /// <summary>
    /// Gets or sets the businessProcess.
    /// </summary>
    /// <value>
    /// The current businessProcess.
    /// </value>
    [DataMember]
    public string BusinessProcess { get; set; }
    /// <summary>
    /// Gets or sets the transaction.
    /// </summary>
    /// <value>
    /// The current transaction.
    /// </value>
    [DataMember]
    public string Transaction { get; set; }
    /// <summary>
    /// Gets or sets the stage.
    /// </summary>
    /// <value>
    /// The current stage.
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
    /// Gets or sets the correlation properties.
    /// </summary>
    /// <value>
    /// The correlation properties.
    /// </value>
    [DataMember]
    public Dictionary<string, object> CorrelationProperties { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is archive enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is archive enabled; otherwise, <c>false</c>.
    /// </value>
    [DataMember]
    public bool ArchiveMessage { get; set; }

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
    /// Gets or sets the Ignore Not Found.
    /// </summary>
    /// <value>
    /// The is Ignore Not Found.
    /// </value>
    [DataMember]
    public bool IgnoreNotFound { get; set; }

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
    /// Validates this instance.
    /// </summary>
    /// <exception cref="InvalidMessageBody"></exception>
    /// <exception cref="InvalidMessageHeader"></exception>
    /// <exception cref="InvalidTransactionInstanceId"></exception>
    /// <exception cref="InvalidStageInstanceId"></exception>
    /// <exception cref="InvalidTransactionException">
    /// </exception>
    /// <exception cref="InvalidStageNameException"></exception>
    public void Validate()
    {
      if (CorrelationProperties == null)
        throw new InvalidCorrelationProperties();
      if (Stage.IsNullOrEmpty())
        throw new InvalidStageNameException();
    }
  }
}