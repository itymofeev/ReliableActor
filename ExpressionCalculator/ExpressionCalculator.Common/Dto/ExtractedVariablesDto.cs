using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExpressionCalculator.Common.Dto
{
    [DataContract]
    public class ExtractedVariablesDto
    {
        public ExtractedVariablesDto()
        {
            Variables = new List<string>();
        }

        [DataMember]
        public bool IsFinished { get; set; }

        [DataMember]
        public IList<string> Variables { get; set; }
    }
}
