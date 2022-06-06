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
        /// If true, will patch the BEX_CE float cast bug.  If the BT_Extended_CE XP Cap patch
        /// has been modified, UseBexXpCapFix will not be used regardless of the setting.
        /// </summary>
        public bool UseBexXpCapFix { get; set; } = true;

        /// <summary>
        /// If true, bumps up the difficulty estimate by .5 to work around
        /// BEX Difficulty effectively using the *previous* XP cap.
        /// </summary>
        public bool ShowPilotXpMinDifficultyWorkAround { get; set; } = false;

        /// <summary>
        /// Controls the XP Level Cap percentage display.
        /// </summary>
        public XpPercentageDisplay XpPercentageDisplay { get; set; } = XpPercentageDisplay.BasedOnPatchStatus;

        public bool DebugOutput { get; set; } = false;

        /// <summary>
        /// If true, writes out the pilot data to the log file.
        /// </summary>
        public bool DebugOutputPilots { get; set; } = false;

        public DifficultyColors DifficultyColors { get; set; }

        public XpCapPercentageColors XpCapPercentageColors { get; set; } = new XpCapPercentageColors();

    }


    public class XpCapPercentageColors
    {
        /// <summary>
        /// Mission will have up to 25% XP
        /// </summary>
        public string Delta25 { get; set; } = "#EE5E43";
        /// <summary>
        /// Mission will have up to 50% XP
        /// </summary>
        public string Delta50 { get; set; } = "#F59338";
        /// <summary>
        /// Mission will have up to 75% XP
        /// </summary>
        public string Delta75 { get; set; } = "#EFC501";
        
        /// <summary>
        /// Mission will have up to 100% XP
        /// </summary>
        public string Delta100 { get; set; } = "#4CFF00";

        /// <summary>
        /// Returns the HTML formatted text for the "XP award percentage" projection text.
        /// </summary>
        /// <param name="maxLeveCapPercentage"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public string GetCapPercentageText(decimal maxLeveCapPercentage)
        {
            string capColor;

            int colorRange = Math.Min(3, (int)(maxLeveCapPercentage * 100) / 25);


            if (maxLeveCapPercentage >= .99m)
            {
                //Default colors
                return "";
            }

            switch (colorRange)
            {
                case 0:
                    capColor = Delta25;
                    break;
                case 1:
                    capColor = Delta50;
                    break;
                case 2:
                    capColor = Delta75;
                    break;
                case 3:
                    capColor = Delta100;
                    break;
                default:
                    throw new ApplicationException($"Unexpected {nameof(colorRange)} value: {colorRange}");
            }

            return $" <color=#FFFFFF>[</color><color={capColor}>{maxLeveCapPercentage:P0}<color=#FFFFFF>]</color></color>";
        }
    }

    /// <summary>
    /// The "best XP use" difficulty colors in order from closest to the difficulty to 
    /// away from.  
    /// The last color will always be the default for any other levels.
    /// </summary>
    public class DifficultyColors : List<string>
    {
        /// <summary>
        /// Returns the UI color for the pilot's level vs the contract.
        /// </summary>
        /// <param name="difficultyDelta">The delta from the contract level. 0 means exact match, 1 is one </param>
        /// <returns></returns>
        public string GetColor(int difficultyDelta)
        {

            return  difficultyDelta == -1 || difficultyDelta >= Count ? this[this.Count - 1] : this[difficultyDelta];
        }
    }
}
