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
                var input = Convert.ToInt32(Console.ReadLine());
                if(input == 1)
                {                    
                    bool done = false;
                    while (!done)
                    {
                        Console.WriteLine("---WRITE TO MOVIE DATABASE---");
                        Console.WriteLine("Enter 1 for adding a new item and 2 for saving changes to the file and close.");
                        input = Convert.ToInt32(Console.ReadLine());
                        if (input == 1)
                        {
                            Console.WriteLine("Please enter the movie title below");
                            String title = Console.ReadLine();
                            Console.WriteLine("Please enter the gneres seperated by |");
                            Console.WriteLine("Example: \'Action|Comedy|Romance\'");
                            String genres = Console.ReadLine();
                            Console.WriteLine("Adding movie...");
                            Console.WriteLine(manager.addItem(title,genres) + "\n");
                            Console.WriteLine("Press enter to continue");
                            Console.ReadLine();
                        }
                        if (input == 2)
                        {
                            Console.WriteLine("Writing to CSV file...");
                            manager.saveToCsv();
                            Console.WriteLine("Done.");
                            done = !done; //Exit the loop for adding movies
                        }
                        
                    }
                    
                }
                else if(input == 2)
                {
                    Console.WriteLine("---READ MOVIE DATABASE---");
                    bool done = false;
                    while (!done)
                    {
                        Console.WriteLine("Enter 1 for all data once and 2 for another 10 lines of data.");
                        input = Convert.ToInt32(Console.ReadLine());
                        if (manager.getEntires() <= manager.getCurLine())
                        {
                            Console.WriteLine("END OF DATA");
                            done = true;
                        }
                        else if (input == 2)
                        {
                            Console.WriteLine(manager.readNextEntries(10));
                        }
                        else if (input == 1)
                        {
                            Console.WriteLine(manager.readAllData());
                            done = true;
                        }
                    }
                }                
                Console.WriteLine("Press enter to leave :)");
                Console.ReadLine();
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
