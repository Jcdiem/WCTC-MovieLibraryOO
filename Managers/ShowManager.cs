﻿using CsvHelper;
using MovieLibrary.types;
using Newtonsoft.Json;
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
                        Convert.ToInt32(record.id),
                        record.title,
                        Convert.ToInt32(record.season),
                        Convert.ToInt32(record.episode),
                        record.writers.Split(", ")
                        ));
                }
            }
        }

        public override void OpenJSON(string filePath)
        {
            throw new NotImplementedException();
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
    }
}
