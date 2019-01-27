using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface IExtractorActor : IActor
    {
        Task ExtractVariables(string correlationId, string expression);
    }
}
