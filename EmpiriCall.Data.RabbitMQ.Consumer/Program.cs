using System;
using System.ServiceProcess;

namespace EmpiriCall.Data.RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ConsumerService();

            if (!Environment.UserInteractive)
            {
                var servicesToRun = new ServiceBase[] { service };
                ServiceBase.Run(servicesToRun);
                return;
            }

            Console.WriteLine("Running EmpiriCall.Data.RabbitMQ as a Console Application");
            Console.WriteLine("For more information, see https://github.com/mgroves/EmpiriCall");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Enter 'R' to run the service, enter anything else to exit.");
            Console.Write("> ");

            var input = Console.ReadLine() ?? "";

            switch (input.ToUpper())
            {
                case "R":
                    service.Start(args);
                    Console.ReadLine();
                    break;
                default:
                    break;
            }
            Console.WriteLine("Exiting...");
        }
    }
}
