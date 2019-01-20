using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ExpressionCalculator.Service.Actors
{
    public class ProcessorActor : Actor, IProcessorActor
    {
        public ProcessorActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }
    }
}
