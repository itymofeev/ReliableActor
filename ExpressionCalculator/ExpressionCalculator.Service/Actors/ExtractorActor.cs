using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressionCalculator.Common;
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
    public class ExtractorActor : Actor, IExtractorActor
    {
        private const int LRP_DELAY = 30;
        private readonly IExpressionExtractor _expressionExtractor;

        public ExtractorActor(ActorService actorService, ActorId actorId, IExpressionExtractor expressionExtractor) : base(actorService, actorId)
        {
            _expressionExtractor = expressionExtractor ?? throw new ArgumentNullException(nameof(expressionExtractor));
        }

        public async Task ExtractVariables(string correlationId, string expression)
        {
            await Task.Delay(TimeSpan.FromSeconds(LRP_DELAY));

            var supervisorActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(ISupervisorActor), Constants.APPLICATION_NAME);
            var supervisorActor = ActorProxy.Create<ISupervisorActor>(new ActorId(correlationId), supervisorActorEndpoint);
            var extractionResult = new ExtractionResult
            {
                CorrelationId = correlationId,
                ExtractedVariables = new ExtractedVariables
                {
                    IsFinished = true,
                    Variables = new List<string>(_expressionExtractor.ExtractVariables(expression))
                }
            };

            await supervisorActor.AddExtractedVrailes(extractionResult);
        }
    }
}
