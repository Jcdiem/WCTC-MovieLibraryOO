using CsvHelper;
using MovieLibrary.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieLibrary.Utilities
{
    public class Searcher
    {
        //TODO: Store this in a different place so that it only has to be updated in 1 area when new item types are created
        private string[] dbPaths = new string[3];
        private List<List<DbItemI>> itemDb = new List<List<DbItemI>>();


        public Searcher(string moviePath, string showPath, string videoPath)
        {
            //Scream in pain when we get nulls from the naughty devs (myself)
            this.dbPaths[(int)DbItemI.dbInfoTypes.MOVIE] = moviePath ?? throw new ArgumentNullException(nameof(moviePath));
            this.dbPaths[(int)DbItemI.dbInfoTypes.SHOW] = showPath ?? throw new ArgumentNullException(nameof(showPath));
            this.dbPaths[(int)DbItemI.dbInfoTypes.VIDEO] = videoPath ?? throw new ArgumentNullException(nameof(videoPath));
        }

        public string searchDatabasesForTitle(string searchTitle)
        {
            if (!itemDb.Any()) throw new AccessViolationException("Tried to search db info without populating!");
            string outStr = "";
            int runningCount = 0;
            //Use iterated for loop to echo item type (extra credit)
            for(int i = 0; i < itemDb.Count(); i++)
            {
                try
                {
                    var searchResult = itemDb[i].Where(item => item.title.ToLower().Contains(searchTitle.ToLower()));
                    runningCount += searchResult.Count();
                    //TODO: Implement a better methodology than nested loops
                    foreach (var item in searchResult) outStr += item.display() + "\n";
                    outStr += "--- End results from " + (DbItemI.dbInfoTypes)i + " database ---\n";                      
                }
                catch (System.InvalidOperationException)
                {
                    return "Item not found!";
                }

            }
            return outStr + " Found " + runningCount + " entries!";
        }

        public void openDatabases()
        {
            //Use standard for loop so we can check which item type is being added using the dbItemType enum 
            for(int i = 0; i < dbPaths.Length; i++)
            {   
                //Check file type
                if (dbPaths[i].Contains(".json"))
                {
                    //Open the database with proper type
                    OpenJSON(dbPaths[i], i);
                }
                else if (dbPaths[i].Contains(".csv"))
                {
                    //Open the database with proper type
                    OpenCSV(dbPaths[i], i);
                }
                else throw new FormatException("Improper file format given for path " + dbPaths[i]);
            }
        }

        private void OpenJSON(string filePath, int dataType)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                //Create a new list to be used for converting from derived class to base class
                List<DbItemI> conversionList = new List<DbItemI>();
                switch (dataType)
                {
                    //Convert the specific, derived type to a the base type
                    case (int) DbItemI.dbInfoTypes.MOVIE:
                        conversionList.AddRange(JsonConvert.DeserializeObject<List<IMovie>>(json));
                        break;
                    case (int) DbItemI.dbInfoTypes.SHOW:
                        conversionList.AddRange(JsonConvert.DeserializeObject<List<IShow>>(json));
                        break;
                    case (int) DbItemI.dbInfoTypes.VIDEO:
                        conversionList.AddRange(JsonConvert.DeserializeObject<List<IVideo>>(json));
                        break;
                    default:
                        throw new ArgumentException("OpenJson given incorrect db item enum type of " + dataType);
                }
                //Add the items
                itemDb.Add(conversionList);                             
            }
        }

        private void OpenCSV(string filePath, int dataType)
        {            
            using (StreamReader fileReader = File.OpenText(filePath))
            using (var csv = new CsvReader(fileReader, System.Globalization.CultureInfo.CurrentCulture))
            {
                var stringDB = csv.GetRecords<dynamic>();
                List<DbItemI> csvList = new List<DbItemI>();
                switch (dataType)
                {
                    case (int)DbItemI.dbInfoTypes.MOVIE:
                        foreach (var record in stringDB)
                        {
                            csvList.Add(new IMovie(
                                record.genres.Split('|'),
                                Convert.ToInt32(record.id),
                                record.title));
                        }
                        break;
                    case (int)DbItemI.dbInfoTypes.SHOW:
                        foreach (var record in stringDB)
                        {
                           csvList.Add(new IShow(
                                Convert.ToInt32(record.id),
                                record.title,
                                Convert.ToInt32(record.season),
                                Convert.ToInt32(record.episode),
                                record.writers.Split("|")
                                ));
                        }
                        break;
                    case (int)DbItemI.dbInfoTypes.VIDEO:
                        foreach (var record in stringDB)
                        {
                            csvList.Add(new IVideo(
                                Convert.ToInt32(record.id),
                                record.title,
                                record.format,
                                Convert.ToInt32(record.length),
                                Managers.VideoManager.convertRegionsToInt(record.regions.Split('|'))
                                ));
                        }
                        break;
                    default:
                        throw new ArgumentException("OpenJson given incorrect db item enum type of " + dataType);
                }
                itemDb.Add(csvList);
            }
        }
    }
}
