using System.Collections.Generic;

namespace ExpressionCalculator.Common.Dto
{
    public class SubstitutedVariablesRequest
    {
        public SubstitutedVariablesRequest(IEnumerable<VariableToValueEntry> variablesToValuesMap, string expression)
        {
            VariablesToValuesMap = variablesToValuesMap;
            Expression = expression;
        }

        public IEnumerable<VariableToValueEntry> VariablesToValuesMap { get; private set; }

        public string Expression { get; private set; }
    }
}
