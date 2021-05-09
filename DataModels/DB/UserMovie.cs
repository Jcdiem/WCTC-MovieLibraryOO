using System;
using System.Collections.Generic;

#nullable disable

namespace MovieLibrary.DataModels.DB
{
    public partial class UserMovie
    {
        public long Id { get; set; }
        public long Rating { get; set; }
        public DateTime RatedAt { get; set; }
        public long? UserId { get; set; }
        public long? MovieId { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual User User { get; set; }
    }
}
