using System;
using Microsoft.Extensions.Logging;
using System.Globalization;
using MovieLibrary.types;
using NLog;

namespace MovieLibrary
{
     public class Runner
     {
         private readonly ILogger<Runner> _logger;

         public Runner(ILogger<Runner> logger)
         {
             _logger = logger;
         }

         public void DoAction(string name)
         {
             _logger.LogDebug(20, "Doing hard work! {Action}", name);
         }
     }

    class Program
    {
        static void Main(string[] args)
        {
           var logger = LogManager.GetCurrentClassLogger();
           ui.InterfaceManager uix = new ui.InterfaceManager();
           try
           {                               
                Managers.ManagerI manager;
                switch (uix.getDbItemType())
                {
                    case (int)DbItemI.dbInfoTypes.MOVIE:
                        manager = new Managers.MovieManager();                                                
                        break;
                    case (int)DbItemI.dbInfoTypes.SHOW:
                        manager = new Managers.ShowManager();
                        break;
                    case (int)DbItemI.dbInfoTypes.VIDEO:
                        manager = new Managers.VideoManager();
                        break;
                    default:
                        manager = null;
                        Console.WriteLine("ERROR IN CREATING MANAGER :: IMPROPER TYPE");
                        throw new Exception("Unvalidated user input for data type caused crash");
                }
                Console.WriteLine("Please give the path to the file or the file name if in the current directory.");
                manager.OpenCSV(Console.ReadLine());
                Console.WriteLine("Enter a 1 for data adding and a 2 for data reading.");
                uix.handleDbOperation(Convert.ToInt32(Console.ReadLine()),manager);
                
           }
           catch (Exception ex)
           {
              // NLog: catch any exception and log it.
              logger.Error(ex, "Stopped program because of exception");
              throw;
           }
           finally
           {
              // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
              LogManager.Shutdown();
           }
        }
    }
}
