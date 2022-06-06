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

        //Not using DI for this simple project.  :(

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
                    , 8000
                    , 11600
                    , 15600
                    , 21600
                    , 36000
                    , 55600
                    , 81200
                    , 113600
                    , 999999999
                },
            };

            PrivateType accessor = new PrivateType(typeof(Core));
            accessor.SetStaticProperty("BexExistsCheckCompleted", true);

            //Bypass init logic
            var bexSettings = new Extended_CE.ModSettings()
            {
                XPCap = true,
                MinXPMultiplier = .1f,
            };

            accessor.SetStaticField("_bexModSettings", bexSettings);



            Core.ModSettings = new ModSettings()
            {
                ShowPilotXpMinDifficultyWorkAround = false,
            };


           

            GetXpCapMinDifficultyTest(settings, 500, 1, 1);
            GetXpCapMinDifficultyTest(settings, 6000,2, .71m);
            GetXpCapMinDifficultyTest(settings, 10800, 3, .22m);
            GetXpCapMinDifficultyTest(settings, 11800, 4, .95m);
            //XP Cap minimum hit.  Bumps to next difficulty
            GetXpCapMinDifficultyTest(settings, 54000, 8, 1m);

            GetXpCapMinDifficultyTest(settings, 120000, 10, .99m);

            //First level
            GetXpCapMinDifficultyTest(settings, 100, 1, 1m);

            GetXpCapMinDifficultyTest(settings, 0, 1, 1m);
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
                UseBexXpCapFix = false
            };

            GetXpCapMinDifficultyTest(settings, 500 , 2, null);
            GetXpCapMinDifficultyTest(settings, 6000, 3, null);
            GetXpCapMinDifficultyTest(settings, 10800, 4, null);
            GetXpCapMinDifficultyTest(settings, 11800, 5, null);
            GetXpCapMinDifficultyTest(settings, 54000, 8, null);
            GetXpCapMinDifficultyTest(settings, 120000, 10, null);
        }


        void GetXpCapMinDifficultyTest(BTExtendedCeSettings settings, int pilotXp, int expectedMinDifficulty,
                        decimal? expectedMaxLeveCapPercentage)
        {
            PilotMinXp pilotMinXp = settings.GetXpCapMinDifficulty(pilotXp);

            Assert.AreEqual(expectedMinDifficulty, pilotMinXp.MinimumContractDifficulty);

            if (expectedMaxLeveCapPercentage.HasValue)
            {
                Assert.AreEqual((double)expectedMaxLeveCapPercentage, (double)pilotMinXp.MaxDifficultyPercentage, .01);
            }
        }


    }
}