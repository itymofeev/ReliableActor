using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ExpressionCalculator.Service.Actors
{
    [StatePersistence(StatePersistence.Volatile)]
    public class WorkerActor : Actor, IWorkerActor
    {
        public WorkerActor(ActorService actorService, ActorId actorId) : base(actorService, actorId) { }

        public async Task StartVariableExtraction(string correlationId, string expression)
        {
            var processorActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(IProcessorActor), "ExpressionCalculator");
            var processor = ActorProxy.Create<IProcessorActor>(ActorId.CreateRandom(), processorActorEndpoint);
            var extractedVariables = await processor.ExtractVariables(correlationId.ToString(), expression);

            await StateManager.AddStateAsync(extractedVariables.Key, extractedVariables.Value);
        }

        public async Task<TestDto> TryGetExtractedVariables(string correlationId)
        {
            var hasExtractedVariables = await StateManager.ContainsStateAsync(correlationId);

            return hasExtractedVariables
                ? await StateManager.GetStateAsync<TestDto>(correlationId)
                : new TestDto(Enumerable.Empty<string>());
        }
    }
}
