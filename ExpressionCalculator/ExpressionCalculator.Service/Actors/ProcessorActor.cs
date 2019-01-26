using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public async Task<KeyValuePair<string, TestDto>> ExtractVariables(string correlationId, string expression)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));

            return KeyValuePair.Create(correlationId, new TestDto(_expressionExtractor.ExtractVariables(expression)));
        }
    }
}
