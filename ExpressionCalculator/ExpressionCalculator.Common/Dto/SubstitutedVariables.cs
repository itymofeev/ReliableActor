using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExpressionCalculator.Common.Dto
{
    [DataContract]
    public class SubstitutedVariables
    {
        public SubstitutedVariables()
        {
            VariablesToValuesMap = new List<VariableToValueEntry>();
        }

        [DataMember]
        public IList<VariableToValueEntry> VariablesToValuesMap { get; set; }
    }
}