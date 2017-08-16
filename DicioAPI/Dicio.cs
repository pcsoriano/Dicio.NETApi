using System.Linq;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace DicioAPI
{
    public class Dicio
    {
        private const string DICIO_URL = "https://www.dicio.com.br/";
        private HtmlWeb _htmlWeb;

        public Dicio()
        {
            _htmlWeb = new HtmlWeb();

        }

        public async Task<string> Meaning(string word, string separator = null)
        {
            return string.Join(separator ?? " ", await Meanings(word));
        }

        public async Task<List<string>> Meanings(string word)
        {
            if (word == null)
                throw new ArgumentNullException("word", "Search word must be informed.");

            HtmlDocument htmlDocument;

            try
            {
                htmlDocument = await _htmlWeb.LoadFromWebAsync(DICIO_URL + word.PrepareToSearch());
            }
            catch (Exception e)
            {
                throw new Exception("Nothing found. Verify if the search term is correct.", e);
            }

            var meaningsHtml = htmlDocument.DocumentNode.Descendants("p")
                .Where(d => d.Attributes.Contains("itemprop") && d.Attributes["itemprop"].Value.Equals("description"))
                .FirstOrDefault();

            if (meaningsHtml is null)
                throw new Exception("Nothing found. Verify if the search term is correct.");

            var meanings = meaningsHtml
                .Descendants("span")
                .Where(d => !d.HasAttributes).Select(d => d.InnerText);

            return meanings.ToList();
        }

        public async Task<List<string>> Synonyms(string word)
        {
            if (word == null)
                throw new ArgumentNullException("word", "Search word must be informed.");

            HtmlDocument htmlDocument = null;

            try
            {
                htmlDocument = await _htmlWeb.LoadFromWebAsync(DICIO_URL + word.PrepareToSearch());
            }
            catch (Exception e)
            {
                throw new Exception("Nothing found. Verify if the search term is correct.", e);
            }            

            var synonymsHtml = htmlDocument.DocumentNode.Descendants("p")
                .Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value.Equals("adicional sinonimos"))
                .FirstOrDefault();

            if (synonymsHtml is null)
                return new List<string>();

            var synonyms = synonymsHtml.Descendants("a")
                .Select(a => a.InnerText);

            return synonyms.ToList();
        }
    }
}
