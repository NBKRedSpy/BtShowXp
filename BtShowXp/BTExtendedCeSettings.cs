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
        /// <param name="pilotXp">The pilot's current total XP</param>
        /// <param name="minDifficulty">The minimum difficulty required to not force the XP minimum</param>
        /// <param name="maxLevelCapPercentage">The percentage of XP the pilot would recieve it at the pilot's max
        /// XP Cap level.</param>
        public void GetXpCapMinDifficulty(int pilotXp, out int minDifficulty, out decimal maxLevelCapPercentage)
        {
            int xpCapIndex = Math.Max(0, XPDifficultyCaps.FindIndex(x => pilotXp <= x));

            minDifficulty = (xpCapIndex  + 1);

            //Bug in BEX where the XP ratio is alwasy the XP min.  The ratio is supposed to be computed
            //  if the Pilot's XP is between the mission XP cap and the previous cap level.
            if (Core.ModSettings.ShowPilotXpMinDifficultyWorkAround && minDifficulty < 10)
            {
                minDifficulty += 1;
            }

            //Get the percentage of the pilot's XP award if the difficulty is at the pilot's last XP Cap Level.
            //Eg, pilot has 10,000 XP and the max cap for the level is 20,000, the pilot would get 50% XP.

            if(Core.ModSettings.ShowPilotXpMinDifficultyWorkAround || xpCapIndex == 0)
            {
                //without the fix, will never get a percentage of XP
                maxLevelCapPercentage = 1;
            }
            else
            {

                int currentLevelCapXp = XPDifficultyCaps[xpCapIndex];
                int previousLevelCapXp = XPDifficultyCaps[xpCapIndex - 1];

                int currentXpCapRange = currentLevelCapXp - previousLevelCapXp;
                int pilotXpCapDelta = currentLevelCapXp - pilotXp;

                maxLevelCapPercentage = (decimal)pilotXpCapDelta / currentXpCapRange;
                
                if(maxLevelCapPercentage <= (decimal) Core.BexModSettings.MinXPMultiplier)
                {
                    //next level is required to get more than minimum.
                    minDifficulty++;
                    maxLevelCapPercentage = 1;
                }
            }
       }
    }


}
