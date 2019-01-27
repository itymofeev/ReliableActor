using System;
using System.Threading;
using System.Threading.Tasks;
using ExpressionCalculator.Common.Dto;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors.Runtime;

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
            var worker = MakeWorkerActor(correlationId);
            await worker.StartVariableExtraction(correlationId, expression);

            return correlationId;
        }

        [HttpGet("{correlationId}")]
        public async Task<ExtractedVariablesDto> Get(string correlationId)
        {
            var worker = MakeWorkerActor(correlationId);
            var extractedVariablesDto = await worker.TryGetExtractedVariables(correlationId);
            if (extractedVariablesDto.IsFinished)
            {
                var supervisorActorId = new ActorId(correlationId);
                var actorService = ActorNameFormat.GetFabricServiceUri(typeof(IWorkerActor), "ExpressionCalculator");
                var myActorServiceProxy = ActorServiceProxy.Create(actorService, supervisorActorId);
                if (myActorServiceProxy != null)
                {
                    await myActorServiceProxy.DeleteActorAsync(supervisorActorId, CancellationToken.None);
                }
            }

            return extractedVariablesDto;
        }

        private IWorkerActor MakeWorkerActor(string correlationId)
        {
            var workerActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(IWorkerActor), "ExpressionCalculator");
            return ActorProxy.Create<IWorkerActor>(new ActorId(correlationId), workerActorEndpoint);
        }
    }
}