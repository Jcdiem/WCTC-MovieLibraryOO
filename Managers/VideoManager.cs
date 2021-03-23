using CsvHelper;
using MovieLibrary.types;
using Newtonsoft.Json;
using System;
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
                        convertRegionsToInt(record.regions)
                        ));
                }
            }
        }

        public override void OpenJSON(string filePath)
        {
            throw new NotImplementedException();
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

    }
}
