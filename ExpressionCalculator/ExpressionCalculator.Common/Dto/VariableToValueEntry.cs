using System.Runtime.Serialization;

namespace ExpressionCalculator.Common.Dto
{
    [DataContract]
    public class VariableToValueEntry
    {
        public VariableToValueEntry()
        {
            Name = string.Empty;
            Value = string.Empty;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
