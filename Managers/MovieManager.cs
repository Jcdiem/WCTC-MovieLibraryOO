using CsvHelper;
using MovieLibrary.types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public override void writeToJson()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (StreamWriter sw = new StreamWriter(@"movieLibrary.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, base.dbItemLibrary);
            }
        }
        public override void OpenJSON(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                var tempList = JsonConvert.DeserializeObject<List<Movie>>(json);
                base.dbItemLibrary.AddRange(tempList);
            }
        }
    }
}
