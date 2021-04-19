using System.Collections.Generic;

namespace MovieLibrary.DataModels.Database
{
    class Genre
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
