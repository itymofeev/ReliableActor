﻿using System.Runtime.Serialization;

namespace ExpressionCalculator.Common.Dto
{
    [DataContract]
    public class ExtractionResult
    {
        public ExtractionResult()
        {
            CorrelationId = string.Empty;
        }

        [DataMember]
        public string CorrelationId { get; set; }

        [DataMember]
        public ExtractedVariables ExtractedVariables { get; set; }
    }
}
