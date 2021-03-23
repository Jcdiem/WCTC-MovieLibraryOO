using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using CsvHelper;
using System.Linq;
using MovieLibrary.types;

namespace MovieLibrary
{
    public class Manager
    {
        private int curLine = 0;
        private CsvReader csv;
        private List<DbItemI> dbItemLibrary;
        private string filePath;
        public enum itemType
        {
            MOVIE = 0,
            SHOW = 1,
            VIDEO= 2,
        }
        //Kinda deprecated, kept for error checking purposes
        //Original purpose was to iterate through new entries
        private int? newEntryStartIndex = null;     

        public Manager(string file, CultureInfo culture, int type)
        {
            using StreamReader fileReader = File.OpenText(file);
            using var csv = new CsvReader(fileReader, culture);
            switch (type)
            {
                case (int)itemType.MOVIE:
                    dbItemLibrary = csv.GetRecords<>();
                    break;               
            }

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

        public int getEntries()
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

        public string addItem(DbItemI item)
        {
            if (!alreadyExists(item.title)) //Don't add the item if it already exists
            {
                if (newEntryStartIndex == null) newEntryStartIndex = dbItemLibrary.Count; //Set the new starting point
                dbItemLibrary.Add(item);
                return "Movie added to temporary list. Make sure to write changes.";
            }
            else return "Item already exists.";

        }

        public void saveToCsv()
        {
            if (newEntryStartIndex != null)
            {
                //Open the file in append mode so we don't overwrite it
                CsvWriter csvWrt = new CsvWriter(new StreamWriter(File.Open(filePath, FileMode.Append)), CultureInfo.CurrentCulture);
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
