using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface IProcessorActor : IActor
    {
        Task<KeyValuePair<string, TestDto>> ExtractVariables(string correlationId, string expression);
    }
}
