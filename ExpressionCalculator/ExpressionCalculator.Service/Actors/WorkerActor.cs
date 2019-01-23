using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data.Collections;

using static ExpressionCalculator.Common.Constants;

namespace ExpressionCalculator.Service.Actors
{
    [StatePersistence(StatePersistence.Persisted)]
    public class WorkerActor : Actor, IWorkerActor
    {
        public WorkerActor(ActorService actorService, ActorId actorId) : base(actorService, actorId) { }

        public async Task AddVariables(KeyValuePair<string, IEnumerable<string>> extractedVariables)
        {
            var extractedVariablesMap =
                await StateManager.GetStateAsync<IDictionary<string, IEnumerable<string>>>(EXTRACTED_VARIABLES_MAP);
            extractedVariablesMap[extractedVariables.Key] = extractedVariables.Value;

            //StateManager.AddOrUpdateStateAsync<IDictionary<string, IEnumerable<string>>>(EXTRACTED_VARIABLES_MAP, extractedVariablesMap);
        }

        public async Task<string> StartVariableExtraction(string expression)
        {
            var correlationId = Guid.NewGuid();

            var processorActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(IProcessorActor), "ExpressionCalculator");
            var processor = ActorProxy.Create<IProcessorActor>(ActorId.CreateRandom(), processorActorEndpoint);
            await Task.Run(() => processor.ExtractVariables(correlationId.ToString(), expression));

            return await Task.FromResult(correlationId.ToString());
        }

        public async Task<IEnumerable<string>> TryGetExtractedVariables(string correlationId)
        {
            var extractedVariablesMap =
                await StateManager.GetStateAsync<IDictionary<string, IEnumerable<string>>>(EXTRACTED_VARIABLES_MAP);

            return extractedVariablesMap == null || !extractedVariablesMap.ContainsKey(correlationId)
                ? Enumerable.Empty<string>()
                : extractedVariablesMap[correlationId];
        }
    }
}
