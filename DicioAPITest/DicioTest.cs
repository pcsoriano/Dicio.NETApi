using DicioAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DicioAPITest
{
    [TestClass]
    public class DicioTest
    {
        [TestMethod]
        public void MeaningTest()
        {
            var dicio = new Dicio();            

            var meaning = dicio.SearchMeaningAsync("teste").Result;

            Assert.IsNotNull(meaning);
        }

        [TestMethod]
        public void MeaningsTest()
        {
            var dicio = new Dicio();

            var meaning = dicio.SearchMeaningsAsync("teste").Result;

            Assert.IsNotNull(meaning);
        }

        [TestMethod]
        public void SynonymsTest()
        {
            var dicio = new Dicio();

            var synonyms = dicio.SearchSynonymsAsync("teste");

            Assert.IsNotNull(synonyms);
        }

        [TestMethod]
        public void SearchResultTest()
        {
            var dicio = new Dicio();

            dicio.SearchAsync("teste");

            Assert.IsNotNull(dicio.SearchResult.Meaning);
            Assert.IsNotNull(dicio.SearchResult.Meanings);
            Assert.IsNotNull(dicio.SearchResult.Synonyms);
        }
    }
}
