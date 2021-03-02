using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.types
{
    class Movie : dbItemI
    {
        public int id { get; private set; }
        public string title { get; private set; }
        public string[] genres { get; private set; }

        public Movie(int id, string title, string[] genres)
        {
            this.id = id;
            this.title = title;
            this.genres = genres;
        }

        public string display()
        {
            return "Movie: " + title + " -Genres: " + genres.ToString() + " -ID: " + id;
        }
    }
}
