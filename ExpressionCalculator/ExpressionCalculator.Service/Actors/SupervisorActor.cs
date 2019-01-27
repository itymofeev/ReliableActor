using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressionCalculator.Common;
using ExpressionCalculator.Common.Dto;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ExpressionCalculator.Service.Actors
{
    [StatePersistence(StatePersistence.Volatile)]
    public class SupervisorActor : Actor, ISupervisorActor
    {
        public SupervisorActor(ActorService actorService, ActorId actorId) : base(actorService, actorId) { }

        public async Task AddExtractedVrailes(ExtractionResult extractionResult)
        {
            await StateManager.AddStateAsync(extractionResult.CorrelationId, extractionResult.ExtractedVariables);
        }

        public Task StartVariableExtraction(string correlationId, string expression)
        {
            var extractorActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(IExtractorActor), Constants.APPLICATION_NAME);
            var extractorActor = ActorProxy.Create<IExtractorActor>(ActorId.CreateRandom(), extractorActorEndpoint);
            Task.Run(() => extractorActor.ExtractVariables(correlationId, expression));

            return Task.CompletedTask;
        }

        public async Task<string> SubstituteVariables(string expression, SubstitutedVariables substitutedVariables)
        {
            var substituterActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(ISubstituterActor), Constants.APPLICATION_NAME);
            var substituterActor = ActorProxy.Create<ISubstituterActor>(ActorId.CreateRandom(), substituterActorEndpoint);

            return await substituterActor.SubstituteVariables(expression, substitutedVariables);
        }

        public async Task<ExtractedVariables> TryGetExtractedVariables(string correlationId)
        {
            var hasExtractedVariables = await StateManager.ContainsStateAsync(correlationId);

            return hasExtractedVariables
                ? await StateManager.GetStateAsync<ExtractedVariables>(correlationId)
                : new ExtractedVariables { IsFinished = false, Variables = new List<string>() };
        }
    }
}
