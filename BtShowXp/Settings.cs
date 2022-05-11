using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    public class ModSettings
    {
        /// <summary>
        /// If true, will compute the XP cost of the pilot's skills and display an error
        /// if the pilot's actual spent XP doesn't match.
        /// </summary>
        public bool ShowSkillSyncError { get; set; } = false;

        /// <summary>
        /// Displays the pilot's total XP (spent and unspent)
        /// </summary>
        public bool ShowPilotXp { get; set; } = true;

        /// <summary>
        /// If true and the pilot's spent XP does match the skills cost, shows the difference.
        /// </summary>
        public bool ShowPilotXpCorruption { get; set; } = true;
        
        /// <summary>
        /// If true, shows the minimum difficulty for the pilot to get full XP when 
        /// using the BEX XP Cap
        /// </summary>
        public bool ShowPilotXpMinDifficulty { get; set; } = true;

        /// <summary>
        /// If true, bumps up the difficulty estimate by .5 to work around
        /// BEX Difficulty effectively using the *previous* XP cap.
        /// </summary>
        public bool ShowPilotXpMinDifficultyWorkAround { get; set; } = true;



    }
}
