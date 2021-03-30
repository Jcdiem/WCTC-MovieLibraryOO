using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.types
{
    public abstract class DbItemI
    {
        protected DbItemI(int id, string title, int type)
        {
            this.id = id;
            this.title = title;
            this.type = type;
        }
        public enum dbInfoTypes : int
        {
            //Dummy types
            UNIVERSAL = -2,
            DEBUG = -1,
            //Real Types
            MOVIE = 0,
            SHOW = 1,
            VIDEO = 2,            
        }
        public int id { get; private set; }
        public int type { get; private set; }
        public string title { get; private set; }
        public abstract string display();
        public abstract string displayCSV();
    }
}
