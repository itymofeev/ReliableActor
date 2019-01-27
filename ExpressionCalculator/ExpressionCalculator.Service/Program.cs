using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using ExpressionCalculator.Service.Actors;
using ExpressionCalculator.Service.Services;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ExpressionCalculator.Service
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // This line registers an Actor Service to host your actor class with the Service Fabric runtime.
                // The contents of your ServiceManifest.xml and ApplicationManifest.xml files
                // are automatically populated when you build this project.
                // For more information, see https://aka.ms/servicefabricactorsplatform

                ActorRuntime.RegisterActorAsync<SupervisorActor>(
                   (context, actorType) => new ActorService(context, actorType)).GetAwaiter().GetResult();

                ActorRuntime.RegisterActorAsync<ExtractorActor>(RegisterProcessorActor).GetAwaiter().GetResult();

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static ActorService RegisterProcessorActor(StatefulServiceContext context, ActorTypeInformation actorType)
        {
            return new ActorService(context, actorType, (s, i) => new ExtractorActor(s, i, new ExpressionExtractor()));
        }
    }
}
