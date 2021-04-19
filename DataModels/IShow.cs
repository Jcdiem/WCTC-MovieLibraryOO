using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.DataModels
{
    class IShow : DbItemI
    {
        public int season { get; private set; }
        public int episode { get; private set; }
        public string[] writers { get; private set; }

        public IShow(int id, string title, int season, int episode, string[] writers) : base (id, title, (int)dbInfoTypes.SHOW)
        {
            this.season = season;
            this.episode = episode;
            this.writers = writers;
        }

        public override string display()
        {
            return "Show: " + title + " -Season: " + season + " -Episode " + episode + " -Writers " + String.Join("|", writers) + " -ID: " + id;
        }

        public override string displayCSV()
        {
            //Have to add string null at beginning so c# returns proper string
            return "" + this.id + ',' + this.title + ',' + this.season + ',' + this.episode + ',' + String.Join("|", this.writers);
        }
    }
}
