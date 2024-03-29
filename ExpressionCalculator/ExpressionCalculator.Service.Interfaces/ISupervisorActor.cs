﻿using System.Threading.Tasks;
using ExpressionCalculator.Common.Dto;
using Microsoft.ServiceFabric.Actors;

namespace ExpressionCalculator.Service.Interfaces
{
    public interface ISupervisorActor : IActor
    {
        Task StartVariableExtraction(string correlationId, string expression);

        Task AddExtractedVrailes(ExtractionResult extractionResult);

        Task<ExtractedVariables> TryGetExtractedVariables(string correlationId);

        Task<string> SubstituteVariables(string expression, SubstitutedVariables substitutedVariables);
    }
}
