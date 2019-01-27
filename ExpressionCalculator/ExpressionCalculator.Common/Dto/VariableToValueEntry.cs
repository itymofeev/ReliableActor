using System;

namespace ExpressionCalculator.Common.Dto
{
    public class VariableToValueEntry
    {
        public VariableToValueEntry(string name, string value)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Value = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentNullException(nameof(value));
        }

        public string Name { get; private set; }

        public string Value { get; private set; }
    }
}
