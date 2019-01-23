using Microsoft.ServiceFabric.Actors;
using System.Threading.Tasks;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface IProcessorActor : IActor
    {
        Task ExtractVariables(string correlationId, string expression);
    }
}
