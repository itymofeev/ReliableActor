using System;
using System.Collections.Generic;

namespace ExpressionCalculator.Common.Dto
{
    public class SubstitutedVariablesRequest
    {
        public SubstitutedVariablesRequest(IEnumerable<VariableToValueEntry> variablesToValuesMap, string expression)
        {
            VariablesToValuesMap = variablesToValuesMap ?? throw new ArgumentNullException(nameof(variablesToValuesMap));
            Expression = !string.IsNullOrWhiteSpace(expression) ? expression : throw new ArgumentNullException(nameof(expression));
        }

        public IEnumerable<VariableToValueEntry> VariablesToValuesMap { get; }

        public string Expression { get; }
    }
}