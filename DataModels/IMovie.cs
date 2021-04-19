using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.DataModels
{
    class IMovie : DbItemI
    {
        public IMovie(string[] genres, int id, string title) : base(id, title, (int)DbItemI.dbInfoTypes.MOVIE)
        {
            this.genres = genres ?? throw new ArgumentNullException(nameof(genres));
        }

        public string[] genres { get; private set; }

        public override string display()
        {
            return "Movie: " + title + " -Genres: " + String.Join("|", genres) + " -ID: " + id;
        }

        public override string displayCSV()
        {
            //Have to add string null at beginning so c# returns proper string
            return "" + id + ',' + title + ',' + String.Join("|", genres);
        }
    }
}
