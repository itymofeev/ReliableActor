using System;
using System.Collections.Generic;

namespace ExpressionCalculator.Common.Dto
{
    public class SubstitutedVariables
    {
        public SubstitutedVariables(IEnumerable<VariableToValueEntry> variablesToValuesMap)
        {
            VariablesToValuesMap = variablesToValuesMap ?? throw new ArgumentNullException(nameof(variablesToValuesMap));
        }

        public IEnumerable<VariableToValueEntry> VariablesToValuesMap { get; private set; }
    }
}