using ExpressionCalculator.Service.Interfaces;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors;
using System;

namespace ExpressionCalculator.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var workerActorEndpoint = ActorNameFormat.GetFabricServiceUri(typeof(IWorkerActor), "ExpressionCalculator");
            var worker = ActorProxy.Create<IWorkerActor>(ActorId.CreateRandom(), workerActorEndpoint);

            Console.WriteLine("Hello World!");
        }
    }
}
