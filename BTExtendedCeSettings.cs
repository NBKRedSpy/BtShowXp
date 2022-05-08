using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    public class BTExtendedCeSettings
    {
        public bool BTExtendedCeSettingsFound { get; set; }

        public List<int> XPDifficultyCaps { get; set; }

        public bool XPCap { get; set; }

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

            //xpCap not found (-1) is implied to be .5 skulls

            //Debug
            //int xpCapIndex = XPDifficultyCaps.FindIndex(x => !(pilotXp >= x));
            //int xpCapIndex = XPDifficultyCaps.FindIndex(x => !(pilotXp <= x));
            //int xpCapIndex = XPDifficultyCaps.FindLastIndex(x => (pilotXp >= x));
            int xpCapIndex = Math.Max(0, XPDifficultyCaps.FindIndex(x => pilotXp <= x));

            //Debug
            //Logger.Log($"Index: {xpCapIndex} XP: {XPDifficultyCaps[xpCapIndex]} Pilot XP: {pilotXp}");


            //decimal skullDifficulty = ((xpCapIndex +1) / 2m);
            decimal skullDifficulty = (xpCapIndex /2m + .5m);
            return skullDifficulty;
        }
    }


}
