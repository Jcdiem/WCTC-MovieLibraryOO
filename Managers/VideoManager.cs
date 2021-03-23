using CsvHelper;
using MovieLibrary.types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MovieLibrary.Managers
{
    class VideoManager : ManagerI
    {
        public VideoManager() : base() { }
        public override void addItem(DbItemI inShow)
        {
            if (inShow is Video)
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
                    base.dbItemLibrary.Add(new Video(
                        Convert.ToInt32(record.id), 
                        record.title,
                        record.format,
                        Convert.ToInt32(record.length),
                        convertRegionsToInt(record.regions.Split('|'))
                        ));
                }
            }
        }

        private int[] convertRegionsToInt(string[] regionInfoStrArray)
        {
            return Array.ConvertAll(regionInfoStrArray, e => int.Parse(e));
        }

        public override void writeToJson()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (StreamWriter sw = new StreamWriter(@"videoLibrary.json"))
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
                var tempList = JsonConvert.DeserializeObject<List<Show>>(json);
                base.dbItemLibrary.AddRange(tempList);
            }
        }

    }
}
