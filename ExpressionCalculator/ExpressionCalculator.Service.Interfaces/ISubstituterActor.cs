using System.Threading.Tasks;
using ExpressionCalculator.Common.Dto;
using Microsoft.ServiceFabric.Actors;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface ISubstituterActor : IActor
    {
        Task<string> SubstituteVariables(string expression, SubstitutedVariables substitutedVariables);
    }
}
