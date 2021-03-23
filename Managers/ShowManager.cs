using CsvHelper;
using MovieLibrary.types;
using System;
using System.IO;


namespace MovieLibrary.Managers
{
    class ShowManager : ManagerI
    {
        public ShowManager() : base() { }
        public override void addItem(DbItemI inShow)
        {
            if (inShow is Show)
            {
                base.dbItemLibrary.Add(inShow);
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
                    base.dbItemLibrary.Add(new Show(
                        (int)record.id,
                        record.title,
                        (int)record.season,
                        (int)record.episode,
                        record.writers.split(", ")
                        ));
                }
            }
        }

        public override void OpenJSON(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
}
