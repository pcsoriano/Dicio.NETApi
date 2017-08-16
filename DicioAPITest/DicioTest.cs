using DicioAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DicioAPITest
{
    [TestClass]
    public class DicioTest
    {
        [TestMethod]
        public async void MeaningTest()
        {
            var dicio = new Dicio();

            var meaning = await dicio.Meaning("teste");

            Assert.IsNotNull(meaning);
        }
    }
}
