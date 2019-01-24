using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface IWorkerActor : IActor
    {
        Task StartVariableExtraction(string correlationId, string expression);

        Task<IEnumerable<string>> TryGetExtractedVariables(string correlationId);
    }
}
