using System;
using System.Threading.Tasks;
using ExpressionCalculator.Common.Dto;
using ExpressionCalculator.Service.Interfaces;
using ExpressionCalculator.Service.Services;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ExpressionCalculator.Service.Actors
{
    [StatePersistence(StatePersistence.None)]
    public class SubstituterActor : Actor, ISubstituterActor
    {
        private readonly IExpressionExtractor _expressionExtractor;

        public SubstituterActor(ActorService actorService, ActorId actorId, IExpressionExtractor expressionExtractor) : base(actorService, actorId)
        {
            _expressionExtractor = expressionExtractor ?? throw new ArgumentNullException(nameof(_expressionExtractor));
        }

        public Task<string> SubstituteVariables(string expression, SubstitutedVariables substitutedVariables)
        {
            return Task.FromResult(_expressionExtractor.SubstituteVariables(expression, substitutedVariables));
        }
    }
}
