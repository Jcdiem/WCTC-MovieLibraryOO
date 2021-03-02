using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.types
{
    class Video : dbItemI
    {
        public int id { get; private set;}
        public string title { get; private set; }
        public string format { get; private set; }
        public int length { get; private set; }
        public int[] regions { get; private set; }

        public Video(int id, string title, string format, int length, int[] regions)
        {
            this.id = id;
            this.title = title;
            this.format = format;
            this.length = length;
            this.regions = regions;
        }

        string dbItemI.display()
        {
            throw new NotImplementedException();
        }
    }
}
