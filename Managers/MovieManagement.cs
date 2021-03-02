using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using CsvHelper;
using System.Linq;
using MovieLibrary.types;

namespace MovieLibrary.Managers
{
    
    public class MovieManager : ManagementI
    {
        private int curLine = 0;
        private readonly CsvReader csv;
        private List<Movie> movieLibrary;
        private String filePath;
        //Kinda deprecated, kept for error checking purposes
        //Original purpose was to iterate through new entries
        private int? newEntryStartIndex = null;

        public MovieManager(String file, CultureInfo culture)
        {
            StreamReader sr = File.OpenText(file);
            filePath = file;
            csv = new CsvReader(sr, culture);
            movieLibrary = csv.GetRecords<Movie>().ToList();
            sr.Close();
        }

        public String readNextEntries(int numLines)
        {
            String outStr = "";
            int max = numLines + curLine;
            for(int i = curLine; i < movieLibrary.Count && i < max; i++)
            {
                outStr += movieLibrary[i].display() + "\n";
            }
            curLine = curLine + (numLines);
            return outStr;
        }

        public String readAllData()
        {
            String outStr = "";
            foreach (var movie in movieLibrary)
            {
                outStr += movie.display() + "\n";
            }

            return outStr;
        }

        public int getEntries()
        {
            return movieLibrary.Count;
        }

        public int getCurLine()
        {
            return curLine;
        }

        private bool alreadyExists(String title)
        {
            foreach(Movie movie in movieLibrary)
            {
                if (movie.title == title) return true;
            }
            return false;
        }

        public String addItem(String title, String[] genres)
        {
            if (!alreadyExists(title)) //Don't add the movie if it already exists
            {
                if (newEntryStartIndex == null) newEntryStartIndex = movieLibrary.Count; //Set the new starting point
                movieLibrary.Add(new Movie(movieLibrary.Last().id + 1, title, genres));                
                return "Movie added to temporary list. Make sure to write changes.";
            }
            else return "Item already exists.";

        }

        public void saveToCsv()
        {
            if (newEntryStartIndex != null) {                
                //Open the file in append mode so we don't overwrite it
                CsvWriter csvWrt = new CsvWriter(new StreamWriter(File.Open(filePath, FileMode.Append)), CultureInfo.CurrentCulture);
                csvWrt.WriteRecords(movieLibrary);
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