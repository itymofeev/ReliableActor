using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressionCalculator.Common.Dto;
using Microsoft.ServiceFabric.Actors;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface IProcessorActor : IActor
    {
        Task<KeyValuePair<string, ExtractedVariablesDto>> ExtractVariables(string correlationId, string expression);
    }
}
