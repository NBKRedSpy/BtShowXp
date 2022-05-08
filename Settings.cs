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
        /// Always show the Pilot's XP summary.  
        /// Spent Xp, Unspent xp, total XP
        /// </summary>
        public bool ShowPilotSummary { get; set; } = false;

        /// <summary>
        /// If true, will export all the pilots when the barricks are opened.
        /// </summary>
        public bool ExportPilots { get; set; } = false;

        /// <summary>
        /// Only export pilots where the XP values do not match expectations.
        /// </summary>
        public bool OnlyExportSyncErrorPilots { get; set; } = false;


        public bool ShowPilotXp { get; set; } = true;

        public bool ShowPilotXpCorruption { get; set; } = true;

        public bool ShowPilotXpMinDifficulty { get; set; } = true;

    }
}
