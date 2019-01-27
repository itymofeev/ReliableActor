using System;
using System.Collections.Generic;

namespace ExpressionCalculator.Common.Dto
{
    [Serializable]
    public class ExtractedVariablesDto
    {
        public ExtractedVariablesDto(bool isFinished, IEnumerable<string> variables)
        {
            Variables = variables ?? throw new ArgumentNullException(nameof(variables));
            IsFinished = isFinished;
        }

        public bool IsFinished { get; private set; }

        public IEnumerable<string> Variables { get; private set; }
    }
}
