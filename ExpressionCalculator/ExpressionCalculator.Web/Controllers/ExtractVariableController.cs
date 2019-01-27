using System;
using System.Threading;
using System.Threading.Tasks;
using ExpressionCalculator.Common;
using ExpressionCalculator.Common.Dto;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;

namespace ExpressionCalculator.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtractVariableController : ControllerBase
    {
        [HttpPost]
        public async Task<string> Post([FromForm]string expression)
        {
            var correlationId = Guid.NewGuid().ToString();
            var supervisorActor = MakeSupervisorActor(correlationId);
            await supervisorActor.StartVariableExtraction(correlationId, expression);

            return correlationId;
        }

        [HttpGet("{correlationId}")]
        public async Task<ExtractedVariablesDto> Get(string correlationId)
        {
            var supervisorActor = MakeSupervisorActor(correlationId);
            var extractedVariablesDto = await supervisorActor.TryGetExtractedVariables(correlationId);
            if (extractedVariablesDto.IsFinished)
            {
                await DiactivateSupervisorActor(correlationId);
            }

            return extractedVariablesDto;
        }

        private async Task DiactivateSupervisorActor(string correlationId)
        {
            var supervisorActorId = new ActorId(correlationId);
            var myActorServiceProxy = ActorServiceProxy.Create(SupervisorActorEndpoint, supervisorActorId);
            await myActorServiceProxy?.DeleteActorAsync(supervisorActorId, CancellationToken.None);
        }

        private ISupervisorActor MakeSupervisorActor(string correlationId)
        {
            return ActorProxy.Create<ISupervisorActor>(new ActorId(correlationId), SupervisorActorEndpoint);
        }

        private Uri SupervisorActorEndpoint => ActorNameFormat.GetFabricServiceUri(typeof(ISupervisorActor), Constants.APPLICATION_NAME);
    }
}