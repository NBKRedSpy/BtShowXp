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
        /// XP Cap level.</param>
        public PilotMinXp GetXpCapMinDifficulty(int pilotXp)
        {
            int xpCapIndex = Math.Max(0, XPDifficultyCaps.FindIndex(x => pilotXp <= x));

            PilotMinXp pilotMinXp = new PilotMinXp();


            pilotMinXp.MinimumContractDifficulty = (xpCapIndex  + 1);

            //Bug in BEX where the XP ratio is always the XP min.  The ratio is supposed to be computed
            //  if the Pilot's XP is between the mission XP cap and the previous cap level.
            if (Core.ModSettings.ShowPilotXpMinDifficultyWorkAround && pilotMinXp.MinimumContractDifficulty < 10)
            {
                pilotMinXp.MinimumContractDifficulty += 1;
            }

            //Get the percentage of the pilot's XP award if the difficulty is at the pilot's last XP Cap Level.
            //Eg, pilot has 10,000 XP and the max cap for the level is 20,000, the pilot would get 50% XP.

            if(Core.ModSettings.ShowPilotXpMinDifficultyWorkAround || xpCapIndex == 0)
            {
                //without the fix, will never get a percentage of XP

                pilotMinXp.MaxDifficultyPercentage = 1;
            }
            else
            {
                //Compute the percentage of XP a pilot gets when a mission matches the XP level.
                int currentLevelCapXp = XPDifficultyCaps[xpCapIndex];
                int previousLevelCapXp = XPDifficultyCaps[xpCapIndex - 1];

                int currentXpCapRange = currentLevelCapXp - previousLevelCapXp;
                int pilotXpCapDelta = currentLevelCapXp - pilotXp;

                pilotMinXp.MaxDifficultyPercentage = (decimal)pilotXpCapDelta / currentXpCapRange;
                
                if(pilotMinXp.MaxDifficultyPercentage <= (decimal) Core.BexModSettings.MinXPMultiplier)
                {
                    //next level is required to get more than minimum.
                    pilotMinXp.MinimumContractDifficulty++;
                    pilotMinXp.MaxDifficultyPercentage = 1;
                }
            }

            return pilotMinXp;
        }
    }


}
