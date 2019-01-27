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
    public class SubstituteVariableController : ControllerBase
    {
        [HttpPut]
        public async Task<string> Put(string expression, [FromBody]SubstitutedVariables substitutedVariables)
        {
            var supervisorActorEndpoint =  ActorNameFormat.GetFabricServiceUri(typeof(ISupervisorActor), Constants.APPLICATION_NAME);
            var supervisorActor = ActorProxy.Create<ISupervisorActor>(ActorId.CreateRandom(), supervisorActorEndpoint);

            return await supervisorActor.SubstituteVariables(expression, substitutedVariables);
        }
    }
}