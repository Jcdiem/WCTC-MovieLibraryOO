using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using MovieLibrary.DataModels;
using System.Linq;

namespace MovieLibrary.Managers
{
    abstract class ManagerI
    {

        protected int curLine = 0;
        //protected CsvReader csv;
        protected List<DbItemI> dbItemLibrary;
        protected string filePath;

        protected ManagerI()
        {
            dbItemLibrary = new List<DbItemI>();
        }


        public abstract void addItem(DbItemI item);
        public abstract void OpenCSV(string filePath);
        public abstract void OpenJSON(string filePath);
        public abstract void OpenSQL(dotnetfinalDbContext db);
        public abstract void writeToJson();
        
        
        
        
        public string searchByTitle(string searchTitle)
        {
            if (dbItemLibrary.Any())
            {
                try
                {
                    var searchResult = dbItemLibrary.Where(item => item.title.Contains(searchTitle));
                    string outStr = "Found " + searchResult.Count() + " Items! \n";
                    foreach (var item in searchResult) outStr += item.display() + "\n";
                    return outStr;
                }
                catch (System.InvalidOperationException)
                {
                    return "Item not found!";
                }

            }
            else throw new Exception("You cannot search before adding a database!");
        }

        public void Open(string filePath)
        {
            this.filePath = filePath;
            //Check file type
            if (filePath.Contains(".json"))
            {
                OpenJSON(filePath);
            }
            else if (filePath.Contains(".csv"))
            {
                OpenCSV(filePath);
            }
            else throw new Exception("Improper file type given to Open() method");
        }
        public string readNextEntries(int numLines)
        {
            string outStr = "";
            int max = numLines + curLine;
            for (int i = curLine; i < dbItemLibrary.Count && i < max; i++)
            {
                outStr += dbItemLibrary[i].display() + "\n";
            }
            curLine = curLine + (numLines);
            return outStr;
        }

        public string readAllData()
        {
            string outStr = "";
            foreach (var dbItem in dbItemLibrary)
            {
                outStr += dbItem.display() + "\n";
            }

            return outStr;
        }

        public int getEntryCount()
        {
            return dbItemLibrary.Count;
        }

        public int getCurLine()
        {
            return curLine;
        }

        private bool alreadyExists(string title)
        {
            foreach (DbItemI dbItem in dbItemLibrary)
            {
                if (dbItem.title == title) return true;
            }
            return false;
        }

        public void saveToCsv()
        {
            if (dbItemLibrary.Any())
            {
                //Remove any prior extensions
                string outPath = "";
                foreach (string pathPart in this.filePath.Split('.'))
                {
                    if(!(pathPart.ToLower().Contains("json") || pathPart.ToLower().Contains("csv")))
                    {
                        outPath += pathPart + ".";
                    }
                }
                //Create a csv extension
                outPath += "csv";

                //Use custom CSV Writer because CSV helper documentation is terrible
                //---Not as cool as following fancy standards, but I can actually know how this works at a glance.
                
                //Set up the lines to be written, with an extra space for headers
                string[] linesOut = new string[dbItemLibrary.Count+1];

                //Get the header
                linesOut[0] = File.ReadLines(filePath).First();
                //Add all data, offset by 1 to keep in line with database
                for (int i = 1; i < linesOut.Length; i++) linesOut[i] = dbItemLibrary[i - 1].displayCSV();
                //Write out the data, overwriting if previous file
                File.WriteAllLines(outPath,linesOut);

            }
            else
            {
                //Prevent trying to save null db
                throw new InvalidOperationException("Null database. Entries must exist before writing to CSV file");
            }

        }
    }
}