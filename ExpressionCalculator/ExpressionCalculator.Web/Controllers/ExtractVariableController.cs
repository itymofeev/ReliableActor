using System;
using System.Threading;
using System.Threading.Tasks;
using ExpressionCalculator.Common;
using ExpressionCalculator.Common.Dto;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.AspNetCore.Cors;
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
        [EnableCors("MyPolicy")]
        public async Task<string> Post([FromForm]string expression)
        {
            var correlationId = Guid.NewGuid().ToString();
            var supervisorActor = MakeSupervisorActor(correlationId);
            await supervisorActor.StartVariableExtraction(correlationId, expression);

            return correlationId;
        }

        [HttpGet("{correlationId}")]
        [EnableCors("MyPolicy")]
        public async Task<ExtractedVariables> Get(string correlationId)
        {
            var supervisorActor = MakeSupervisorActor(correlationId);
            var extractedVariables = await supervisorActor.TryGetExtractedVariables(correlationId);
            if (extractedVariables.IsFinished)
            {
                await DeactivateSupervisorActor(correlationId);
            }

            return extractedVariables;
        }

        private async Task DeactivateSupervisorActor(string correlationId)
        {
            var supervisorActorId = new ActorId(correlationId);
            var myActorServiceProxy = ActorServiceProxy.Create(SupervisorActorEndpoint, supervisorActorId);
            if (myActorServiceProxy != null)
            {
                await myActorServiceProxy.DeleteActorAsync(supervisorActorId, CancellationToken.None);
            }
        }

        private ISupervisorActor MakeSupervisorActor(string correlationId)
        {
            return ActorProxy.Create<ISupervisorActor>(new ActorId(correlationId), SupervisorActorEndpoint);
        }

        private Uri SupervisorActorEndpoint => ActorNameFormat.GetFabricServiceUri(typeof(ISupervisorActor), Constants.APPLICATION_NAME);
    }
}