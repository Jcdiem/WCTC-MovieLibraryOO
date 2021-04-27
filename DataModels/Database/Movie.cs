using System;
using System.Collections.Generic;

namespace MovieLibrary.DataModels.Database
{
    class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Release_Date { get; set; }


        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }
    }
}
