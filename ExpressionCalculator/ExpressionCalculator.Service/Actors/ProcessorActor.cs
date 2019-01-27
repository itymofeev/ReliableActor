using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExpressionCalculator.Common.Dto;
using ExpressionCalculator.Service.Interfaces;
using ExpressionCalculator.Service.Services;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ExpressionCalculator.Service.Actors
{
    [StatePersistence(StatePersistence.None)]
    public class ProcessorActor : Actor, IProcessorActor
    {
        private readonly IExpressionExtractor _expressionExtractor;

        public ProcessorActor(ActorService actorService, ActorId actorId, IExpressionExtractor expressionExtractor) : base(actorService, actorId)
        {
            _expressionExtractor = expressionExtractor ?? throw new ArgumentNullException(nameof(expressionExtractor));
        }

        public async Task ExtractVariables(string correlationId, string expression)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));

            var workerActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(IWorkerActor), "ExpressionCalculator");
            var worker = ActorProxy.Create<IWorkerActor>(new ActorId(correlationId), workerActorEndpoint);
            var extractionResult = new ExtractionResult
            {
                CorrelationId = correlationId,
                ExtractedVariables = new ExtractedVariablesDto
                {
                    IsFinished = true,
                    Variables = new List<string>(_expressionExtractor.ExtractVariables(expression))
                }
            };

            await worker.AddExtractedVrailes(extractionResult);
        }
    }
}
