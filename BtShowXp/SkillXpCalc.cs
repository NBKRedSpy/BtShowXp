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
        public XPTotalCosts LevelExpMap { get; set; } = new XPTotalCosts();
        

        public int GetXpCost(Pilot pilot)
        {
            int totalXp = 0;

            totalXp += LevelExpMap[pilot.Gunnery - 1];
            totalXp += LevelExpMap[pilot.Piloting - 1];
            totalXp += LevelExpMap[pilot.Guts - 1];
            totalXp += LevelExpMap[pilot.Tactics - 1];

            return totalXp;
        }

    }
}
