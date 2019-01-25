using System;
using System.Collections.Generic;

namespace ExpressionCalculator.Service.Interfaces
{
    [Serializable]
    public class TestDto
    {
        public TestDto(IEnumerable<string> extractedVariables)
        {
            ExtractedVariables = extractedVariables ?? throw new ArgumentNullException(nameof(extractedVariables));
        }

        public IEnumerable<string> ExtractedVariables { get; private set; }
    }
}