using BattleTech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    
    /// <summary>
    /// A utility to compute a pilot's skills to the XP amount.
    /// Used to compare expencted spent XP to actual.
    /// </summary>
    public class SkillXpCalc
    {

        /// <summary>
        /// The map of XP to each level.  Level 1 is at the 0 index.
        /// </summary>
        public List<int> LevelExpMap{ get; set; }
        
        
        public SkillXpCalc()
        {
            LevelExpMap = CreateLevelExpMap();
        }

        public int GetXpCost(Pilot pilot)
        {
            int totalXp = 0;

            totalXp += LevelExpMap[pilot.Gunnery - 1];
            totalXp += LevelExpMap[pilot.Piloting - 1];
            totalXp += LevelExpMap[pilot.Guts - 1];
            totalXp += LevelExpMap[pilot.Tactics - 1];

            return totalXp;
        }

        private List<int> CreateLevelExpMap()
        {
            List<int> levelExpMap = new List<int>();

            int totalXp = 0;

            for (int i = 0; i < 10; i++)
            {
                totalXp += i * i * 100;

                levelExpMap.Add(totalXp);
            }

            return levelExpMap;
        }


    }
}
