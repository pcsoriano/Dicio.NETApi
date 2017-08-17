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
        public SearchResult SearchResult { get; private set; }

        public Dicio()
        {
            _htmlWeb = new HtmlWeb();
            SearchResult = new SearchResult();
        }

        public async Task SearchAsync(string word)
        {
            SearchResult.Word = word ?? throw new ArgumentNullException("word", "Informe a palavra para ser buscada.");

            var meanings = await SearchMeaningsAsync(word);

            SearchResult.Meaning = string.Join(" ", meanings);

            SearchResult.Synonyms = await SearchSynonymsAsync(word);
        }

        public async Task<string> SearchMeaningAsync(string word, string separator = null)
        {
            return string.Join(separator ?? " ", await SearchMeaningsAsync(word));
        }

        public async Task<List<string>> SearchMeaningsAsync(string word)
        {
            if (word == null)
                throw new ArgumentNullException("word", "Informe a palavra para ser buscada.");

            HtmlDocument htmlDocument;

            try
            {
                htmlDocument = await _htmlWeb.LoadFromWebAsync(DICIO_URL + word.PrepareToSearch());
            }
            catch (Exception e)
            {
                throw new Exception("Termo não encontrado. Verifique se a palavra buscada está correta.", e);
            }

            var meaningsHtml = htmlDocument.DocumentNode.Descendants("p")
                .Where(d => d.Attributes.Contains("itemprop") && d.Attributes["itemprop"].Value.Equals("description"))
                .FirstOrDefault();

            if (meaningsHtml is null)
                throw new Exception("Termo não encontrado. Verifique se a palavra buscada está correta.");

            var meanings = meaningsHtml
                .Descendants("span")
                .Where(d => !d.HasAttributes).Select(d => d.InnerText);

            return meanings.ToList();
        }

        public async Task<List<string>> SearchSynonymsAsync(string word)
        {
            if (word == null)
                throw new ArgumentNullException("word", "Informe a palavra para ser buscada.");

            HtmlDocument htmlDocument = null;

            try
            {
                htmlDocument = await _htmlWeb.LoadFromWebAsync(DICIO_URL + word.PrepareToSearch());
            }
            catch (Exception e)
            {
                throw new Exception("Termo não encontrado. Verifique se a palavra buscada está correta.", e);
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
