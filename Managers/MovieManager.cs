using CsvHelper;
using MovieLibrary.types;
using System;
using System.ComponentModel;
using System.IO;

namespace MovieLibrary.Managers
{
    class MovieManager : ManagerI
    {
        public MovieManager() : base() { }
        public override void addItem(DbItemI inMovie)
        {
            if (inMovie is Movie)
            {
                base.dbItemLibrary.Add(inMovie);
            }
            //TODO: Elaborate exception
            else throw new Exception();           
        }

        public override void OpenCSV(string filePath)
        {
            using (StreamReader fileReader = File.OpenText(filePath))
            using (var csv = new CsvReader(fileReader, System.Globalization.CultureInfo.CurrentCulture))
            {
                var stringDB = csv.GetRecords<dynamic>();
                
                foreach (var record in stringDB)
                {
                    base.dbItemLibrary.Add(new Movie(
                        record.genres.Split('|'),
                        Convert.ToInt32(record.id),
                        record.title));
                }
            }         
        }

        public override void OpenJSON(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
