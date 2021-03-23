using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.types
{
    class Movie : DbItemI
    {
        public Movie(string[] genres, int id, string title) : base(id, title, (int)DbItemI.dbInfoTypes.MOVIE)
        {
            this.genres = genres ?? throw new ArgumentNullException(nameof(genres));
        }

        public string[] genres { get; private set; }

        public override string display()
        {
            return "Movie: " + title + " -Genres: " + genres.ToString() + " -ID: " + id;
        }
    }
}
