using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;
using MovieLibrary.types;

namespace MovieLibrary.Managers
{
    public abstract class ManagerI
    {

        protected int curLine = 0;
        protected CsvReader csv;
        protected List<DbItemI> dbItemLibrary;
        protected string filePath;
        //Kinda deprecated, kept for error checking purposes
        //Original purpose was to iterate through new entries
        protected int? newEntryStartIndex = null;

        protected ManagerI()
        {
            dbItemLibrary = new List<DbItemI>();
        }


        public abstract void addItem(DbItemI item);
        public abstract void OpenCSV(string filePath);
        public abstract void OpenJSON(string filePath);
        public abstract void writeToJson();

        public void Open(string filePath)
        {
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
            if (newEntryStartIndex != null)
            {
                //Open the file in create mode so we blast away deleted data
                //NOTE: probably not good practice, but CSV Helper is touchy
                CsvWriter csvWrt = new CsvWriter(new StreamWriter(File.Open(filePath, FileMode.CreateNew)), CultureInfo.CurrentCulture);
                csvWrt.WriteRecords(dbItemLibrary);
                newEntryStartIndex = null; //reset the index
            }
            else
            {
                //Prevent trying to save nothing
                throw new InvalidOperationException("Null starting point. New entires must be added before writing to CSV");
            }

        }
    }
}