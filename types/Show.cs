using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.types
{
    class Show : dbItemI
    {
        public int id { get; private set; }
        public string title { get; private set; }
        public int season { get; private set; }
        public int episode { get; private set; }
        public string[] writers { get; private set; }

        public Show(int id, string title, int season, int episode, string[] writers)
        {
            this.id = id;
            this.title = title;
            this.season = season;
            this.episode = episode;
            this.writers = writers;
        }

        string dbItemI.display()
        {
            return "Show: " + title + " -Season: " + season + " -Episode " + episode + " -Writers " + writers + " -ID: " + id;
        }
    }
}
