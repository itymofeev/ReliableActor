using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExpressionCalculator.Common.Dto;

namespace ExpressionCalculator.Service.Services
{
    public class ExpressionExtractor : IExpressionExtractor
    {
        private readonly IEnumerable<Func<string, string>> sanitizeFuncs = new Func<string, string>[]
        {
            StripStringConstantsExpressions,
            StripFunctionNameExpressions
        };

        public IEnumerable<string> ExtractVariables(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }
            var sanitizedExpression = sanitizeFuncs.Aggregate(expression, (e, func) => func(e));
            var matches = Regex.Matches(sanitizedExpression, @"(?<vars>(_|@)*[a-zA-Z]+(\d|_)*)");

            return matches.Select(x => x.Groups["vars"].Value)
                          .Distinct();
        }

        public string SubstituteVariables(string expression, SubstitutedVariables substitutedVariables)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }
            var variablesToValuesMap = substitutedVariables?.VariablesToValuesMap ?? throw new ArgumentNullException(nameof(substitutedVariables));
            var stringConsts = Regex.Matches(expression, "(?<stringConsts>\".*?\")")
                                           .Select((x, i) => new { StringConst = x.Groups["stringConsts"].Value, Index = i });
            var stringConstReplacedExp = stringConsts.Aggregate(expression, (e, m) => Regex.Replace(e, m.StringConst, $"##StringConst_{m.Index}##"));
            var substitutedExp = variablesToValuesMap.Aggregate(stringConstReplacedExp, (e, vvm) => Regex.Replace(e, $@"{vvm.Name}(?!\(|\w|\d)", vvm.Value));
            var reconstructedExp = stringConsts.Aggregate(substitutedExp, (e, m) => Regex.Replace(e, $"##StringConst_{m.Index}##", m.StringConst));

            return reconstructedExp;
        }

        private static string StripStringConstantsExpressions(string sourceExpression)
        {
            return Regex.Replace(sourceExpression, "\"(.*?)\"", string.Empty);
        }

        private static string StripFunctionNameExpressions(string sourceExpression)
        {
            return Regex.Replace(sourceExpression, @"(\w+)\((?<args>.*?)\)", m => m.Groups["args"].Value);
        }
    }
}
