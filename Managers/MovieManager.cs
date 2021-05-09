using CsvHelper;
using MovieLibrary.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieLibrary.Managers
{
    class MovieManager : ManagerI
    {
        public MovieManager() : base() { }
        public override void addItem(DbItemI inMovie)
        {
            if (inMovie is IMovie)
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
                    base.dbItemLibrary.Add(new IMovie(
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
                var tempList = JsonConvert.DeserializeObject<List<IMovie>>(json);
                base.dbItemLibrary.AddRange(tempList);
            }
        }

        public override void OpenSQL(dotnetfinalDbContext db)
        {
            //Console.WriteLine("example MovieGenre: " + JsonConvert.SerializeObject(db.MovieGenres.First()));
            //MovieContext db = new MovieContext();
            Console.WriteLine(" movie count " + db.Movies.Count());
            foreach (DataModels.DB.Movie loopItem in db.Movies)
            {

                //==DEBUG==               
                Console.WriteLine("Json of cur object: \n" + JsonConvert.SerializeObject(loopItem));
                List <String> tGenres;
                tGenres = new List<string> { };
                int id = Convert.ToInt32(loopItem.Id);
                string title = loopItem.Title;

                //Get the genres
                foreach (DataModels.DB.MovieGenre loopGenre in loopItem.MovieGenres)
                {
                    tGenres.Add(loopGenre.Genre.Name);
                }

                //Check for nulls
                if (id == 0) throw new ArgumentNullException("ID for movie " + loopItem.Title + " returned null");
                else if (title is null || title == "") throw new ArgumentNullException("Title for movie  with id " + id + " returned null");
                else if (tGenres is null) throw new ArgumentNullException("Genres for movie " + loopItem.Title + " returned null");
                

                //Create temp db item
                DbItemI thisMovie = new IMovie(
                    tGenres.ToArray(),
                    id,
                    title);
                    //dbItem.MovieGenres.Select(g => g.Genre.Name).ToArray(), //object dbItem child
                    //System.Convert.ToInt32(dbItem.Id), //long id -> int
                    //dbItem.Title); //string title

                //Add it
                base.dbItemLibrary.Add(thisMovie);
            }

        }
    }
}
