using System;
using System.Collections.Generic;

#nullable disable

namespace MovieLibrary.DataModels.DB
{
    public partial class Occupation
    {
        public Occupation()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
