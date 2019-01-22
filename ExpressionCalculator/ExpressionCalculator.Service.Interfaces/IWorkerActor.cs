using Microsoft.ServiceFabric.Actors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface IWorkerActor : IActor
    {
        Task<string> StartVariableExtraction(string expression);

        Task<IEnumerable<string>> TryGetExtractedVariables(string correlationId);
    }
}
