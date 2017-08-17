using System.Collections.Generic;

namespace DicioAPI
{
    public class SearchResult
    {
        internal SearchResult()
        {            
            Synonyms = new List<string>();
            Meanings = new List<string>();
        }

        public string Word { get; internal set; }
        public string Meaning { get; internal set; }
        public List<string> Meanings { get; set; }
        public List<string> Synonyms { get; internal set; }
    }
}
