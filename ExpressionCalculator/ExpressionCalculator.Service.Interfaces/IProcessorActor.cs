using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface IProcessorActor : IActor
    {
        Task ExtractVariables(string correlationId, string expression);
    }
}
