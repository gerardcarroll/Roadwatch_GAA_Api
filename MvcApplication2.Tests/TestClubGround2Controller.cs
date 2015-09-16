using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcApplication2.Controllers;

namespace MvcApplication2.Tests
{
    [TestClass]
    public class TestClubGround2Controller
    {
        [TestMethod]
        public void GetSligoClubGroundsCount()
        {
            var controller = new ClubGround2Controller();
            var result = controller.GetClubGroundByCounty("sligo");
            Assert.AreEqual(26, result.Count);
        }

        [TestMethod]
        public void GetFirstSligoClubGroundName()
        {
            var controller = new ClubGround2Controller();
            var result = controller.GetClubGroundByCounty("sligo");
            Assert.AreEqual("Ballisodare GFC", result[0].Club_Ground);
        }

        [TestMethod]
        public void GetClubGroundCountZero()
        {
            var controller = new ClubGround2Controller();
            var result = controller.GetClubGroundByCounty("123");
            Assert.AreEqual(0, result.Count);
        }

    }
}
