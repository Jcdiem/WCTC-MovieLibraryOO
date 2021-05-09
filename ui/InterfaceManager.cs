using System;
using System.Linq;
using MovieLibrary.DataModels.DB;
using MovieLibrary.DataModels;

namespace MovieLibrary.ui
{
    class InterfaceManager
    {
        private enum fileTypes
        {
            JSON = 1,
            CSV = 2,
            SQL = 3,
        }

        public void handleSqlOperation(dotnetfinalDbContext db)
        {
            bool done = false;
            while (!done) {
                Console.Write("What action would you like to take? \n 1. Search for an item \n 2. Add an item \n 3. Update an item \n 4. Delete an item \n Action: ");
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1: //Search
                        Movie movie = searchForMovie(db);
                        printMovieDetails(movie);
                        break;
                    case 2: //Add
                        bool adding = true;
                        while (adding)
                        {
                            Console.Write("Title: ");
                            string title = Console.ReadLine();
                            if (title != null && title.Length > 0)
                            {
                                


                                Console.WriteLine("Release Date (yyyy-mm-dd): ");
                                string date = Console.ReadLine();
                                if (date.Length != 10)
                                {
                                    //0 is year, 1 is month, 2 is day
                                    string[] dateFormat = date.Split('-');
                                    DateTime releaseDate = new DateTime(
                                        Convert.ToInt32(dateFormat[0]), //Year
                                        Convert.ToInt32(dateFormat[1]), //Month
                                        Convert.ToInt32(dateFormat[2]));//Day

                                    //Create the movie
                                    Movie thisMovie = new Movie { Title = title, ReleaseDate = releaseDate };

                                    //Find the Genres allowed
                                    string allowedGenres = "";
                                    foreach (var genre in db.Genres)
                                    {
                                        allowedGenres += "" + genre.Name + ", ";
                                    }

                                    //Ask what genres to use
                                    Console.WriteLine("Please choose genres for movie (You can choose more than one by using this format: Comedy|Romance|Action) \n" +
                                        "Avaialable genres: \n " + allowedGenres);
                                    string[] chosenGenres = Console.ReadLine().Split('|');
                                    MovieGenre[] movieGenresUsed = new MovieGenre[chosenGenres.Length];

                                    //Add the genres to the movie                                    
                                    foreach (string genre in chosenGenres)
                                    {
                                        Genre tempGenre = db.Genres.Where(tG => tG.Name.ToLower().Equals(genre.ToLower())).FirstOrDefault();
                                        if (tempGenre != null)
                                        {
                                            MovieGenre tempMovieGenre = new MovieGenre() { Movie = thisMovie, Genre = tempGenre };
                                            thisMovie.MovieGenres.Add(tempMovieGenre);
                                            movieGenresUsed.Append(tempMovieGenre);
                                        }
                                        else Console.WriteLine("Error in your genre " + genre + ", skipping.");
                                    }

                                    //Check to make sure everything is ok to user
                                    Console.WriteLine("Are the above notices acceptable? (Y/n)");

                                    if (!(Console.ReadLine().ToLower().Equals('n')))
                                    {
                                        //Actually add the movie
                                        db.Movies.Add(thisMovie);
                                        //Add the movie genres
                                        foreach (var movieGenre in movieGenresUsed) db.MovieGenres.Add(movieGenre);
                                        //Save changes
                                        db.SaveChanges();

                                        adding = false;
                                    }
                                    else Console.WriteLine("Repeating...");

                                }
                                else Console.WriteLine("Error in date format, please try again.");
                                                            
                            }
                            else
                            {
                                Console.WriteLine("Error in name format, please try again.");
                            }
                                                       
                        }
                        break;
                    case 3: //Update
                        bool updating = true;
                        while (updating)
                        {
                            Movie movieToUpdate = searchForMovie(db);
                            Console.WriteLine(movieToUpdate.Title + " selected...");
                            printMovieDetails(movieToUpdate);
                            Console.WriteLine("What would you like to update? \n 1. Title \n 2. Release date \n 3. Genres");
                            switch (Convert.ToInt32(Console.ReadLine()))
                            {
                                case 1: //Title
                                    Console.WriteLine("What would you like the new title to be?");
                                    break;
                                case 2: //Release Date
                                    Console.WriteLine("What would you like the new date to be? (yyyy-mm-dd)");
                                    break;
                                case 3: //Genres
                                    break;
                            
                            }
                            if (!Console.ReadLine().ToLower().Equals('n')) updating = false;
                        }
                        break;
                    case 4: //Delete
                        bool deleting = true;
                        while (deleting)
                        {
                            Movie movieToDelete = searchForMovie(db);
                            //Delete movie
                            if (!Console.ReadLine().ToLower().Equals('n')) deleting = false;
                        }
                        break;
                }
            }
            
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
                    if(manager is Managers.MovieManager)
                    {
                        Console.WriteLine("What type of file do you want? \n 1. JSON \n 2. CSV \n 3. SQL");
                        switch (Convert.ToInt32(Console.ReadLine()))
                        {
                            case (int)fileTypes.JSON:
                                fileType = (int)fileTypes.JSON;
                                break;
                            case (int)fileTypes.CSV:
                                fileType = (int)fileTypes.CSV;
                                break;
                            case (int)fileTypes.SQL:
                                fileType = (int)fileTypes.SQL;
                                break;
                            default:
                                Console.WriteLine("Try again, with the correct numbers this time");
                                break;
                        }
                        if (fileType != 0) break;
                    }
                    else
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
                                Console.WriteLine("Try again, with the correct numbers this time");
                                break;
                        }
                        if (fileType != 0) break;
                    }
                    
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
                            manager.addItem(new IMovie(genres, manager.getEntryCount(), title));
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
                            manager.addItem(new IShow(manager.getEntryCount(),title,season,episode,writers));
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
                            manager.addItem(new IVideo(manager.getEntryCount(), title, format, length, regionInt));
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

        private Movie searchForMovie(dotnetfinalDbContext db)
        {
            while (true)
            {
                Console.WriteLine("Enter the title part you would like to search by (Spaces are included in search)");
                Movie possibleMovie = db.Movies.Where(m => m.Title.Contains(Console.ReadLine())).FirstOrDefault();
                Console.WriteLine("Is " + possibleMovie.Title + " the movie you were looking for? (Y/n)");
                if (!Console.ReadLine().ToLower().Equals('n')) return possibleMovie;
                else Console.WriteLine("Restartng, please change your search");
            }
        }
        private void printMovieDetails(Movie movie)
        {
            Console.WriteLine("Printing full details about movie.");
            string genres = "";
            foreach (var genre in movie.MovieGenres) genres += "" + genre.Genre.Name;
            Console.WriteLine("Title: " + movie.Title + "\n Date Released: " + movie.ReleaseDate.ToString() + "\n Genres: " + genres);
        }
    }    
}
