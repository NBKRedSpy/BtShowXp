using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    public class BTExtendedCeSettings
    {
        public bool BTExtendedCeSettingsFound { get; set; } = false;

        public List<int> XPDifficultyCaps { get; set; } = new List<int>();

        public bool XPCap { get; set; } = false;

        public bool IsCapEnabled => XPCap && BTExtendedCeSettingsFound;

        public static BTExtendedCeSettings BexNotFound()
        {
            return new BTExtendedCeSettings()
            {
                BTExtendedCeSettingsFound = false
            };
        }

        /// <summary>
        /// Returns the minimum contract difficulty that the pilot will still get full XP
        /// </summary>
        /// <returns></returns>
        public decimal GetXpCapMinDifficulty(int pilotXp)
        {

            int xpCapIndex = Math.Max(0, XPDifficultyCaps.FindIndex(x => pilotXp <= x));

            decimal skullDifficulty = (xpCapIndex /2m + .5m);

            //Bug in BEX where the XP ratio is alwasy the XP min.  The ratio is supposed to be computed
            //  if the Pilot's XP is between the mission XP cap and the previous cap level.
            if (Core.ModSettings.ShowPilotXpMinDifficultyWorkAround && skullDifficulty < 5)
            {
                skullDifficulty += .5m;
            }

            return skullDifficulty;
        }
    }


}
