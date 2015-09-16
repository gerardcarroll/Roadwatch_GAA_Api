using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcApplication2.Controllers;

namespace MvcApplication2.Tests
{
    [TestClass]
    public class TestArticleController
    {
        [TestMethod]
        public void GetListArticlesCount()
        {
            var controller = new ArticleController();

            var result = controller.GetArticles();
            Assert.AreEqual(25, result.Count());
        }
    }
}
