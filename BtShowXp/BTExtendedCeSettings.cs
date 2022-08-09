using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    public class BTExtendedCeSettings
    {
        /// <summary>
        /// True if BEX was detected and the settings were loaded.
        /// </summary>
        public bool BTExtendedCeInstalled { get; set; } = false;

        public List<int> XPDifficultyCaps { get; set; } = new List<int>();

        public bool XPCap { get; set; } = false;

        public float MinXPMultiplier { get; set; }

        public int XPMax { get; set; }


        public static BTExtendedCeSettings BexNotFound()
        {
            return new BTExtendedCeSettings()
            {
                BTExtendedCeInstalled = false
            };
        }


        /// <summary>
        /// Returns the BEX CE specific settings directly from the in process BEX settings.
        /// </summary>
        /// <returns></returns>
        public static BTExtendedCeSettings LoadBexCeSettings()
        {
            try
            {
                Type CeCoreType = AccessTools.TypeByName("Extended_CE.Core");

                if (CeCoreType != null)
                {
                    FieldInfo bexSettingsInfo = AccessTools.Field(CeCoreType, "Settings");
                    object bexCeSettings = bexSettingsInfo.GetValue(null);
                    Type ceSettingsType = bexCeSettings.GetType();


                    //This mod's copy of the BEX CE settings.
                    BTExtendedCeSettings ceSettings = new BTExtendedCeSettings();

                    ceSettings.BTExtendedCeInstalled = true;
                    ceSettings.XPDifficultyCaps = new List<int>((int[])AccessTools.Field(ceSettingsType, "XPDifficultyCaps").GetValue(bexCeSettings));
                    ceSettings.XPCap = (bool)AccessTools.Field(ceSettingsType, "XPCap").GetValue(bexCeSettings);
                    ceSettings.MinXPMultiplier = (float) AccessTools.Field(ceSettingsType, "MinXPMultiplier").GetValue(bexCeSettings);
                    ceSettings.XPMax = (int)AccessTools.Field(ceSettingsType, "XPMax").GetValue(bexCeSettings);

                    return ceSettings;
                }
                else
                {
                    return BexNotFound();
                }
            }
            catch (Exception)
            {
                Logger.Log("Unable to load BEX settings");
                throw;
            }
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
                
                if(pilotMinXp.MaxDifficultyPercentage <= (decimal) this.MinXPMultiplier)
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
