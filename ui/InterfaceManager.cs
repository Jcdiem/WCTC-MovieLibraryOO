using MovieLibrary.types;
using System;

namespace MovieLibrary.ui
{
    public class InterfaceManager
    {
        private enum fileTypes
        {
            JSON = 1,
            CSV = 2,
        }


        public void handleDbOperation(Managers.ManagerI manager)
        {
            Console.WriteLine("Enter a 1 for data adding, a 2 for data reading, and a 3 for data searching.");
            int operationType = Convert.ToInt32(Console.ReadLine());
            int fileType = 0;
            //Write data
            if (operationType == 1)
            {
                while (true)
                {
                    Console.WriteLine("What type of file do you want? \n 1. JSON \n 2. CSV");
                    switch (Convert.ToInt32(Console.ReadLine()))
                    {
                        case (int)fileTypes.JSON:
                            fileType = (int)fileTypes.JSON;
                            break;
                        case (int)fileTypes.CSV:
                            fileType = (int)fileTypes.CSV;
                            break;
                        default:
                            Console.WriteLine("Try again, with numbers this time");
                            break;
                    }
                    if (fileType != 0) break;
                }               
                bool done = false;
                while (!done)
                {
                    Console.WriteLine("---WRITE TO DATABASE---");
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
                        if(fileType == (int)fileTypes.CSV)
                        {
                            Console.WriteLine("Writing to CSV file...");
                            manager.saveToCsv();
                            Console.WriteLine("Done.");
                        }
                        else if(fileType == (int)fileTypes.JSON)
                        {
                            Console.WriteLine("Writing to JSON file...");
                            manager.writeToJson();
                            Console.WriteLine("Done.");
                        }
                        
                        done = !done; //Exit the loop for adding movies
                    }

                }

            }
            //Read data
            else if (operationType == 2)
            {
                Console.WriteLine("---READ DATABASE---");
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
            //Data searching
            else if (operationType == 3)
            {
                while (true)
                {
                    Console.WriteLine("Enter the title of the item you would like to search for: ");
                    Console.WriteLine(manager.searchByTitle(Console.ReadLine()));
                    Console.WriteLine("Is this what you were lookin for? Y/N");
                    if (Console.ReadLine().ToUpper() == "Y") break;
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
                Console.WriteLine("Please enter the number for the data type: \n 1. Movie \n 2. Show \n 3. Video \n 4. All Types (Read Only) ");
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
                    case 4:
                        Console.WriteLine("Selected Universal Data Reading");
                        dbItemType = (int)DbItemI.dbInfoTypes.UNIVERSAL;
                        break;
                    default:
                        Console.WriteLine("Improper selection, try again.");
                        break;
                }
            }
            return dbItemType;
        }
        public string[] getSearchPathArray()
        {
            //TODO: Define a 'global' var for amount of real data types supported
            //------Do not just use count, dummy vars will be counted
            string[] outArr = new string[3];
            Console.WriteLine("Please enter the path/name of your movie database: \n" +
                "--Examples included are movies.csv and movieLibrary.json");
            outArr[(int)DbItemI.dbInfoTypes.MOVIE] = Console.ReadLine();
            Console.WriteLine("Please enter the path/name of your show database: \n" +
                "--Examples included are shows.csv and showLibrary.json");
            outArr[(int)DbItemI.dbInfoTypes.SHOW] = Console.ReadLine();
            Console.WriteLine("Please enter the path/name of your movie database: \n" +
                "--Examples included are videos.csv and videoLibrary.json");
            outArr[(int)DbItemI.dbInfoTypes.VIDEO] = Console.ReadLine();
            return outArr;
        }
        public void searchUniversal(Utilities.Searcher search)
        {
            while (true)
            {
                Console.WriteLine("Enter the part of a title you would like to search: ");
                Console.WriteLine(search.searchDatabasesForTitle(Console.ReadLine()));
                Console.WriteLine("Search again? (Y/n)");
                if (Console.ReadLine().ToUpper() == "N") break;
            }
            Console.WriteLine("Goodbye!");
        }
    }    
}
