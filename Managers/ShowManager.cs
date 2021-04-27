using CsvHelper;
using MovieLibrary.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


namespace MovieLibrary.Managers
{
    class ShowManager : ManagerI
    {
        public ShowManager() : base() { }
        public override void addItem(DbItemI inShow)
        {
            if (inShow is IShow)
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

                    base.dbItemLibrary.Add(new IShow(
                        Convert.ToInt32(record.id),
                        record.title,
                        Convert.ToInt32(record.season),
                        Convert.ToInt32(record.episode),
                        record.writers.Split("|")
                        ));
                }
            }
        }

        public override void writeToJson()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (StreamWriter sw = new StreamWriter(@"showLibrary.json"))
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
                var tempList = JsonConvert.DeserializeObject<List<IShow>>(json);
                base.dbItemLibrary.AddRange(tempList);
            }
        }

        public override void OpenSQL(MovieContext db)
        {
            throw new NotImplementedException();
        }
    }
}
