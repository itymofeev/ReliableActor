using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ExpressionCalculator.Service.Actors
{
    [StatePersistence(StatePersistence.Volatile)]
    public class ProcessorActor : Actor, IProcessorActor
    {
        public ProcessorActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }

        public async Task ExtractVariables(string correlationId, string expression)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));
            var workerActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(IWorkerActor), "ExpressionCalculator");
            var worker = ActorProxy.Create<IWorkerActor>(ActorId.CreateRandom(), workerActorEndpoint);

            await worker.AddVariables(KeyValuePair.Create<string, IEnumerable<string>>(correlationId, new[] { "X1", "X2" }));
        }
    }
}
