using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace Remibit.PriceServices
{
    static class Program
    {
        private const string ListeningOn = "http://localhost:8088/";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var appHost = new AppHost();
            var listeningOn = appHost.AppSettings.GetString("service:listeningOn");
            listeningOn = listeningOn ?? "http://*:1088/";
            //Allow you to debug your Windows Service while you're deleloping it. 
            #if DEBUG
            Console.WriteLine("Running WinServiceAppHost in Console mode");
            try
            {
                appHost.Init();
                appHost.Start(listeningOn);
                Process.Start(listeningOn);
                Console.WriteLine("Press <CTRL>+C to stop.");
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}: {1}", ex.GetType().Name, ex.Message);
                throw;
            }
            finally
            {
                appHost.Stop();
            }

            Console.WriteLine("WinServiceAppHost has finished");

            #else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new PriceServices(appHost,listeningOn)
            };
            ServiceBase.Run(ServicesToRun);
            #endif

            Console.ReadLine();
        }
    }
}
