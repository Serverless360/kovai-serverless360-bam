using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kovai.Serverless360.Bam
{
    [DataContract]
    internal class MessageContent
    {
        [DataMember]
        public string MessageBody { get; set; }
        [DataMember]
        public string MessageHeader { get; set; }
        [DataMember]
        public List<Property> Property { get; set; }
    }
    internal class Property
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
