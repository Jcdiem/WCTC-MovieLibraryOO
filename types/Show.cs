using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.types
{
    class Show : DbItemI
    {
        public int season { get; private set; }
        public int episode { get; private set; }
        public string[] writers { get; private set; }

        public Show(int id, string title, int season, int episode, string[] writers) : base (id, title)
        {
            this.season = season;
            this.episode = episode;
            this.writers = writers;
        }

        public override string display()
        {
            return "Show: " + title + " -Season: " + season + " -Episode " + episode + " -Writers " + writers + " -ID: " + id;
        }
    }
}
