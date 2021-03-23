using MovieLibrary.types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.ui
{
    public class InterfaceManager
    {


        public void handleDbOperation(int operationType, Managers.ManagerI manager)
        {
            //Write data
            if (operationType == 1)
            {
                bool done = false;
                while (!done)
                {
                    Console.WriteLine("---WRITE TO MOVIE DATABASE---");
                    Console.WriteLine("Enter 1 for adding a new item and 2 for saving changes to the file and close.");
                    var input = Convert.ToInt32(Console.ReadLine());
                    if (input == 1)
                    {                       
                        Console.WriteLine("Please enter the title below");
                        String title = Console.ReadLine();
                        //Begin asking for specific info
                        if(manager is Managers.MovieManager)
                        {
                            Console.WriteLine("Please enter the gneres seperated by |");
                            Console.WriteLine("Example: \'Action|Comedy|Romance\'");
                            string[] genres = Console.ReadLine().Split("|");
                            Console.WriteLine("Adding movie...");
                            manager.addItem(new Movie(genres, manager.getEntryCount(), title));
                            Console.WriteLine("Done.");
                        }
                        else if(manager is Managers.ShowManager)
                        {
                            Console.WriteLine("Please enter the season number");
                            int season = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Please enter the episode number ");
                            int episode = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Please enter the writers, separated by comma and space \n" +
                                "Example: Steve, Dave, Carl");
                            string[] writers = Console.ReadLine().Split(", ");
                            Console.WriteLine("OK! Adding show...");
                            manager.addItem(new Show(manager.getEntryCount(),title,season,episode,writers));
                            Console.WriteLine("Finished adding.");
                        }
                        else if (manager is Managers.VideoManager)
                        {
                            Console.WriteLine("One type of format (this copy's type)");
                            string format = Console.ReadLine();
                            Console.WriteLine("Length, in minutes");
                            int length = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Regions, comma sepperated integers without space \n " +
                                "Example: 0,1,2,3,4,5");
                            string[] regionStr = Console.ReadLine().Split(",");
                            Console.WriteLine("OK! Adding video...");
                            int[] regionInt = Array.ConvertAll(regionStr, e => int.Parse(e));
                            manager.addItem(new Video(manager.getEntryCount(), title, format, length, regionInt));
                            Console.WriteLine("Finished adding.");
                        }
                        
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
            //Read data
            else if (operationType == 2)
            {
                Console.WriteLine("---READ MOVIE DATABASE---");
                bool done = false;
                while (!done)
                {
                    Console.WriteLine("Enter 1 for all data once and 2 for another 10 lines of data.");
                    var input = Convert.ToInt32(Console.ReadLine());
                    if (manager.getEntryCount() <= manager.getCurLine())
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
        public int getDbItemType()
        {
            int dbItemType = -1;
            while (dbItemType == -1)
            {
                Console.WriteLine("Please enter the number for the data type: \n 1. Movie \n 2. Show \n 3. Video ");
                var tInput = Convert.ToInt32(Console.ReadLine());
                switch (tInput)
                {
                    case 1:
                        Console.WriteLine("Selected 'Movie'");
                        dbItemType = (int)DbItemI.dbInfoTypes.MOVIE;
                        break;
                    case 2:
                        Console.WriteLine("Selected 'Show'");
                        dbItemType = (int)DbItemI.dbInfoTypes.SHOW;
                        break;
                    case 3:
                        Console.WriteLine("Selected 'Video'");
                        dbItemType = (int)DbItemI.dbInfoTypes.VIDEO;
                        break;
                    default:
                        Console.WriteLine("Improper selection, try again.");
                        break;
                }
            }
            return dbItemType;
        }
    }
}
