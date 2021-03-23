using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.types
{
    class Video : DbItemI
    {
        
        public string format { get; private set; }
        public int length { get; private set; }
        public int[] regions { get; private set; }

        public Video(int id, string title, string format, int length, int[] regions) : base (id, title, (int)DbItemI.dbInfoTypes.SHOW)
        {
            this.format = format;
            this.length = length;
            this.regions = regions;
        }

        public override string display()
        {
            throw new NotImplementedException();
        }
    }
}
