using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine
{
    public class Result
    {
        public string URL;          // Stores the URL of the result
        public string title;        // Stores the title of the result
        public string metadesc;     // Stores a description of the result's contents

        private int id;

        // Constructor for sites where the metadescription was inaccessible
        public Result(int _id, string _title, string _URL)
        {
            URL = _URL;
            title = _title;
            id = _id;
        }

        // Overload constructor for sites where the metadescription was extracted successfully
        public Result(int _id, string _title, string _URL, string _metadesc)
        {
            URL = _URL;
            title = _title;
            metadesc = _metadesc;
            id = _id;
        }

        public int GetId() { return (id); }
    }
}
