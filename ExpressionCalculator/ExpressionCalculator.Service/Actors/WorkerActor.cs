using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data.Collections;

using static ExpressionCalculator.Common.Constants;

namespace ExpressionCalculator.Service.Actors
{
    public class WorkerActor : Actor, IWorkerActor, IRemindable
    {
        public WorkerActor(ActorService actorService, ActorId actorId) : base(actorService, actorId) { }


        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            throw new NotImplementedException();
        }

        public async Task<string> StartVariableExtraction(string expression)
        {
            var correlationId = Guid.NewGuid();

            await Task.Run(() => { });
            // lunch ProcessorActor here.

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
