using System;
using System.Collections.Generic;
using ExpressionCalculator.Service.Services;
using Xunit;
using ExpressionCalculator.Common.Dto;

namespace ExpressionCalculator.Test
{
    public class ExpressionExtractorTest
    {
        private IExpressionExtractor _expressionExtractor = new ExpressionExtractor();

        [Theory]
        [InlineData("\"Only constans\"+ 45.1*8", new string[0])]
        [InlineData("!@#$%^&*()_+~", new string[0])]
        [InlineData("x()", new string[0])]
        [InlineData("y", new[] { "y" })]
        [InlineData("\"First constant\"+ abc -(\"Another constant\")", new[] { "abc" })]
        [InlineData("(x + 2)*(x - 2)", new[] { "x" })]
        [InlineData("func() + \"This is my foo variable\" * (foo + 34)", new[] { "foo" })]
        [InlineData("_test*test_ + 12*@test - fun() + @test_123", new[] { "_test", "test_", "@test", "@test_123" })]
        [InlineData("(x + max(x1, 5)) / d – sqrt(z) + b * CalculateSalary(\"Ivanov\", -1+x) ", new[] { "x", "x1", "d", "z", "b" })]
        public void ExtractVariables_ValidTargetExpression_ExtractedVariables(string targetExpression, IEnumerable<string> expected)
        {
            Assert.Equal(expected, _expressionExtractor.ExtractVariables(targetExpression));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void ExtractVariables_InvalidTargetExpression_ThrowExcpetion(string targetExpression)
        {
            Assert.Throws<ArgumentNullException>(() => { _expressionExtractor.ExtractVariables(targetExpression); });
        }

        [Theory]
        [MemberData(nameof(GenerateInvalidSubstituteVariablesData))]
        public void SubstituteVariables_InvalidArgs_SubstitutedExpression(SubstitutedVariables substitutedVariables, string expression)
        {
            Assert.Throws<ArgumentNullException>(() => { _expressionExtractor.SubstituteVariables(expression, substitutedVariables); });
        }

        [Theory]
        [MemberData(nameof(GenerateValidSubstituteVariablesData))]
        public void SubstituteVariables_ValidArgs_SubstitutedExpression(SubstitutedVariables substitutedVariables, string expression, string expected)
        {
            Assert.Equal(expected, _expressionExtractor.SubstituteVariables(expression, substitutedVariables));
        }

        public static IEnumerable<object[]> GenerateValidSubstituteVariablesData() => new[]
        {
            new object[]
            {
                new SubstitutedVariables(new[] { new VariableToValueEntry("x1", "34") }),
                "func() + (\"Test\") *56 - x1",
                "func() + (\"Test\") *56 - 34"
            },
            new object[]
            {
                new SubstitutedVariables(new[] { new VariableToValueEntry("x1", "42") }),
                "34.5 + x1() + (\"The same x1 in string\") - 42*x1",
                "34.5 + x1() + (\"The same x1 in string\") - 42*42"
            },
            new object[]
            {
                new SubstitutedVariables(new[] { new VariableToValueEntry("_xr", "test") }),
                "\"First string const\" + (\"Second string const\") - 45%_xr",
                "\"First string const\" + (\"Second string const\") - 45%test"
            },
            new object[]
            {
                new SubstitutedVariables(new[]
                {
                    new VariableToValueEntry("x", "123"),
                    new VariableToValueEntry("x1", "\"aaa\""),
                    new VariableToValueEntry("d", "3.14"),
                    new VariableToValueEntry("z", "5"),
                    new VariableToValueEntry("b", "42")
                }),
                "(x + max(x1, 5)) / d – sqrt(z) + b * CalculateSalary(\"Ivanov\", -1+x) ",
                "(123 + max(\"aaa\", 5)) / 3.14 – sqrt(5) + 42 * CalculateSalary(\"Ivanov\", -1+123) "
            }
        };

        public static IEnumerable<object[]> GenerateInvalidSubstituteVariablesData() => new[]
        {
            new object[] { null, "45 + x" },
            new object[] { new SubstitutedVariables(new[] { new VariableToValueEntry("x1", "34") }), string.Empty },
            new object[] { new SubstitutedVariables(new[] { new VariableToValueEntry("x1", "34") }), null},
            new object[] { new SubstitutedVariables(new[] { new VariableToValueEntry("x1", "34") }), "     " }
        };
    }
}