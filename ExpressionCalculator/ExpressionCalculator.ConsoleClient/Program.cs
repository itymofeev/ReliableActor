using System;
using System.Threading.Tasks;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors;
using System.Linq;

namespace ExpressionCalculator.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var workerActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(IWorkerActor), "ExpressionCalculator");
            var worker = ActorProxy.Create<IWorkerActor>(ActorId.CreateRandom(), workerActorEndpoint);
            var correlationId = Guid.NewGuid();
            Task.Run(() => worker.StartVariableExtraction(correlationId.ToString(), "xxxx"));
            var test = true;
            while(test) {
                Console.WriteLine("Hit!");
                var result = worker.TryGetExtractedVariables(correlationId.ToString()).Result;
                if (result.ExtractedVariables.Any())
                {
                    test = false;
                    Console.WriteLine($"Result { result }");
                };
            }
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
