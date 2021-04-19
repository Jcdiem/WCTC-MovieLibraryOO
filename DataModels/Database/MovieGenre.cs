using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.DataModels.Database
{
    class MovieGenre
    {
        public int Id { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
