using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ExpressionCalculator.Service.Actors
{
    public class WorkerActor : Actor, IWorkerActor
    {
        public WorkerActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }
    }
}
