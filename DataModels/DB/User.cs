using System;
using System.Collections.Generic;

#nullable disable

namespace MovieLibrary.DataModels.DB
{
    public partial class User
    {
        public User()
        {
            UserMovies = new HashSet<UserMovie>();
        }

        public long Id { get; set; }
        public long Age { get; set; }
        public string Gender { get; set; }
        public string ZipCode { get; set; }
        public long? OccupationId { get; set; }

        public virtual Occupation Occupation { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }
    }
}
