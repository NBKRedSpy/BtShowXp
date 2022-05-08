using Microsoft.VisualStudio.TestTools.UnitTesting;
using BtShowXp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp.Tests
{
    [TestClass()]
    public class BTExtendedCeSettingsTests
    {
        [TestMethod()]
        public void GetXpCapMinDifficultyTest()
        {
            BTExtendedCeSettings settings;

            settings = new BTExtendedCeSettings()
            {
                BTExtendedCeSettingsFound = true,
                XPCap = true,
                XPDifficultyCaps = new List<int>
                {
                    5200
                    ,8000
                    ,11600
                    ,15600
                    ,21600
                    ,36000
                    ,55600
                    ,81200
                    ,113600
                    ,999999999
                }
            };

            Core.ModSettings = new ModSettings()
            {
                ShowPilotXpMinDifficultyWorkAround = false,
            };


            decimal expected;
            decimal actual;

            expected = .5m;
            actual = settings.GetXpCapMinDifficulty(500);

            Assert.AreEqual(expected, actual);


            expected = 1m;
            actual = settings.GetXpCapMinDifficulty(6000);

            Assert.AreEqual(expected, actual);

            expected = 1.5m;
            actual = settings.GetXpCapMinDifficulty(10800);

            Assert.AreEqual(expected, actual);


            expected = 2m;
            actual = settings.GetXpCapMinDifficulty(11800);

            Assert.AreEqual(expected, actual);


            expected = 3.5m;
            actual = settings.GetXpCapMinDifficulty(54000);

            Assert.AreEqual(expected, actual);


            expected = 5m;
            actual = settings.GetXpCapMinDifficulty(120000);

            Assert.AreEqual(expected, actual);


        }

        [TestMethod()]
        public void GetXpCapMinDifficultyWorkAroundTest()
        {
            BTExtendedCeSettings settings;

            settings = new BTExtendedCeSettings()
            {
                BTExtendedCeSettingsFound = true,
                XPCap = true,
                XPDifficultyCaps = new List<int>
                {
                    5200
                    ,8000
                    ,11600
                    ,15600
                    ,21600
                    ,36000
                    ,55600
                    ,81200
                    ,113600
                    ,999999999
                }
            };

            Core.ModSettings = new ModSettings()
            {
                ShowPilotXpMinDifficultyWorkAround = true,
            };


            decimal expected;
            decimal actual;

            expected = 1m;
            actual = settings.GetXpCapMinDifficulty(500);

            Assert.AreEqual(expected, actual);


            expected = 1.5m;
            actual = settings.GetXpCapMinDifficulty(6000);

            Assert.AreEqual(expected, actual);

            expected = 2m;
            actual = settings.GetXpCapMinDifficulty(10800);

            Assert.AreEqual(expected, actual);


            expected = 2.5m;
            actual = settings.GetXpCapMinDifficulty(11800);

            Assert.AreEqual(expected, actual);


            expected = 4m;
            actual = settings.GetXpCapMinDifficulty(54000);

            Assert.AreEqual(expected, actual);


            expected = 5m;
            actual = settings.GetXpCapMinDifficulty(120000);

            Assert.AreEqual(expected, actual);
        }




    }
}