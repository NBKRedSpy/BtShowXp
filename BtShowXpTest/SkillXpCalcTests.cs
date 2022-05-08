using Microsoft.VisualStudio.TestTools.UnitTesting;
using BtShowXp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;

namespace BtShowXp.Tests
{
    [TestClass()]
    public class SkillXpCalcTests
    {
        [TestMethod()]
        public void GetXpCostTest()
        {

            SkillXpTest(100 * 4, 2, 2, 2, 2);

            SkillXpTest(0, 1, 1, 1, 1);
            SkillXpTest(100, 2, 1, 1, 1);
            SkillXpTest(500, 3, 1, 1, 1);
            SkillXpTest(1400, 4, 1, 1, 1);
            SkillXpTest(3000, 5, 1, 1, 1);
            SkillXpTest(5500, 6, 1, 1, 1);
            SkillXpTest(9100, 7, 1, 1, 1);
            SkillXpTest(14000, 8, 1, 1, 1);
            SkillXpTest(20400, 9, 1, 1, 1);
            SkillXpTest(28500, 10, 1, 1, 1);

            SkillXpTest(28500 * 4, 10, 10, 10, 10);

            SkillXpTest(17700, 7, 6, 2, 5);


        }

        private void SkillXpTest(int expected, int gunSkill, int pilotSkill, int gutsSkill, int tacticsSkill)
        {
            Pilot pilot = CreatePilot(gunSkill, pilotSkill, gutsSkill, tacticsSkill);

            SkillXpCalc skillXpCalc = new SkillXpCalc();
            int actual = skillXpCalc.GetXpCost(pilot);
            Assert.AreEqual(expected, actual);
        }


        public Pilot CreatePilot(int gunSkill, int pilotSkill, int gutsSkill, int tacticsSkill)
        {
            Pilot pilot = new Pilot(new PilotDef(), "9999", false);

            pilot.StatCollection.Set("Piloting", pilotSkill);
            pilot.StatCollection.Set("Gunnery", gunSkill);
            pilot.StatCollection.Set("Guts", gutsSkill);
            pilot.StatCollection.Set("Tactics", tacticsSkill);

            return pilot;
        }
    }
}