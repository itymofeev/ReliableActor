using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public string SubstituteVariables(string expression, IDictionary<string, string> variableValueMap)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (variableValueMap == null)
            {
                throw new ArgumentNullException(nameof(variableValueMap));
            }
            var stringConsts = Regex.Matches(expression, "(?<stringConsts>\".*?\")")
                                           .Select((x, i) => new { StringConst = x.Groups["stringConsts"].Value, Index = i });
            var stringConstReplacedExp = stringConsts.Aggregate(expression, (e, m) => Regex.Replace(e, m.StringConst, $"##StringConst_{m.Index}##"));
            var substitutedExp = variableValueMap.Aggregate(stringConstReplacedExp, (e, vvm) => Regex.Replace(e, $@"{vvm.Key}(?!\(|\w|\d)", vvm.Value));
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
