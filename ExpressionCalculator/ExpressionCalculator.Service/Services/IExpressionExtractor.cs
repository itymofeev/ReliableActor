using ExpressionCalculator.Common.Dto;
using System.Collections.Generic;

namespace ExpressionCalculator.Service.Services
{
    public interface IExpressionExtractor
    {
        IEnumerable<string> ExtractVariables(string expression);

        string SubstituteVariables(string expression, SubstitutedVariables substitutedVariables);
    }
}
