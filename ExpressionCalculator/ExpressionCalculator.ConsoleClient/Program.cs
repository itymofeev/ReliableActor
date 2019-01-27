using System;
using System.Threading.Tasks;
using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExpressionCalculator.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {

            var res = Regex.Replace("(x + max(x1, 5)) / d – sqrt(z) + b * CalculateSalary(\"Ivanov\", -1+x) ", @"(\w+)\((?<args>.*?)\)", m => m.Groups["args"].Value);

            var workerActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(IWorkerActor), "ExpressionCalculator");
            var worker = ActorProxy.Create<IWorkerActor>(ActorId.CreateRandom(), workerActorEndpoint);
            var correlationId = Guid.NewGuid();
            Task.Run(() => worker.StartVariableExtraction(correlationId.ToString(), "xxxx"));
            var test = true;
            while(test) {
                Console.WriteLine("Hit!");
                var result = worker.TryGetExtractedVariables(correlationId.ToString()).Result;
                if (result.IsFinished)
                {
                    test = false;
                    Console.WriteLine($"Result { result }");
                };
            }
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }


        public static string ReplaceCC(Match m)
        {
            return string.Empty;        
        }
    }
}
