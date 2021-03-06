﻿using System;
using Microsoft.Extensions.Logging;
using System.Globalization;
using MovieLibrary.DataModels;
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
                bool universalDb = false;
                bool userDb = false;
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
                    case (int)DbItemI.dbInfoTypes.UNIVERSAL:
                        //Ensure read only!
                        manager = null;
                        universalDb = true;                        
                        break;
                    case (int)DbItemI.dbInfoTypes.USER:
                        manager = new Managers.MovieManager();
                        userDb = true;
                        break;
                    default:
                        manager = null;
                        Console.WriteLine("ERROR IN CREATING MANAGER :: IMPROPER TYPE");
                        throw new Exception("Unvalidated user input for data type caused crash");
                }

                bool useSQL = false;
                if (userDb) useSQL = true;
                else if (manager is Managers.MovieManager)
                {
                    Console.WriteLine("Would you like to use the SQL Database? (Y/n)");
                    if (!(Console.ReadLine().ToLower().Equals('n'))) useSQL = true;
                }                              
                
                //User actions built on movie SQL database
                if(userDb && manager is Managers.MovieManager)
                {
                    dotnetfinalDbContext db = new dotnetfinalDbContext();
                    manager.OpenSQL(db);
                    uix.handleUserDatabase(db);
                }
                //Movie SQL Database
                else if (useSQL && manager is Managers.MovieManager)
                {
                    dotnetfinalDbContext db = new dotnetfinalDbContext();
                    manager.OpenSQL(db);
                    uix.handleMediaSqlOperation(db);
                    //==Managed Actions==
                    //Search for movie
                    //Add movie
                    //Update a movie
                    //Delete a movie
                }                
                //Any item that doesn't use SQL or universal
                else if (!universalDb)
                {
                    Console.WriteLine("Please give the path to the file or the file name if in the current directory. \n" +
                    " Example files that came with this project are: \n" +
                    "shows.csv | videos.csv | movies.csv || movieLibrary.json | showLibrary.json | videoLibrary.json");
                    manager.Open(Console.ReadLine());
                    uix.handleDbOperation(manager);
                }
                //Universal item
                else
                {
                    Utilities.Searcher dbSearch = null;

                    bool doneLoadingData = false;
                    while (!doneLoadingData)
                    {
                        string[] paths = uix.getSearchPathArray();
                        dbSearch = new Utilities.Searcher(paths[(int)DbItemI.dbInfoTypes.MOVIE], paths[(int)DbItemI.dbInfoTypes.SHOW], paths[(int)DbItemI.dbInfoTypes.VIDEO]);
                        //Try to open the databases
                        try
                        {
                            dbSearch.openDatabases();
                            doneLoadingData = !doneLoadingData;
                        }
                        catch (FormatException e)
                        {
                            //Reset if formatted wrong
                            Console.WriteLine(e.Message + " \n Please try again.");
                        }                        
                    }
                    //This shouldn't happen, but I had to set dbSearch to null to appease the compile gods
                    //Better safe than sorry
                    if (dbSearch == null) throw new AccessViolationException("TRIED TO READ NULL SEARCH DATABASE");
                    
                    uix.searchUniversal(dbSearch);
                }
                Console.WriteLine("Goodbye! :)");
                             
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
