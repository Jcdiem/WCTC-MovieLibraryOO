using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.Managers
{
    public interface ManagementI
    {
        public abstract String readNextEntries(int numLines);
        public abstract String readAllData();
        public abstract int getEntries();
        public abstract int getCurLine();
        public abstract String addItem();
        public void saveToCsv();
    }
}
