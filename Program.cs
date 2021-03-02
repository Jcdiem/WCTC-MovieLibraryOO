using System;
using Microsoft.Extensions.Logging;
using System.Globalization;
using NLog;
using MovieLibrary.Managers;

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
           try
           {
                Console.WriteLine("Please give the path to the file or the file name if in the current directory.");
                MovieManager mvMng = new MovieManager(Console.ReadLine(),CultureInfo.CurrentCulture);
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
                            Console.WriteLine(mvMng.addItem(title,genres) + "\n");
                            Console.WriteLine("Press enter to continue");
                            Console.ReadLine();
                        }
                        if (input == 2)
                        {
                            Console.WriteLine("Writing to CSV file...");
                            mvMng.saveToCsv();
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
                        if (mvMng.getEntires() <= mvMng.getCurLine())
                        {
                            Console.WriteLine("END OF DATA");
                            done = true;
                        }
                        else if (input == 2)
                        {
                            Console.WriteLine(mvMng.readNextEntries(10));
                        }
                        else if (input == 1)
                        {
                            Console.WriteLine(mvMng.readAllData());
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
