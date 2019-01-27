using System.Threading.Tasks;
using ExpressionCalculator.Common.Dto;
using Microsoft.ServiceFabric.Actors;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface IWorkerActor : IActor
    {
        Task StartVariableExtraction(string correlationId, string expression);

        Task<ExtractedVariablesDto> TryGetExtractedVariables(string correlationId);
    }
}
