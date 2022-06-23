using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Ce = Extended_CE;

namespace BtShowXp
{
    public static class Core
    {

        /// <summary>
        /// For clan missions, treat the difficulty as if it is a 5 star.
        /// Required since BEX resets the contract scale, but they should always be treated as a 5 star.
        /// </summary>
        public const int ClanDifficultyEmulation = 10;

        public static ModSettings ModSettings { get; set; }

        public static SkillXpCalc SkillXpCalc { get; set; } = new SkillXpCalc();

        public static BTExtendedCeSettings BTExtendedCeSettings { get; set; } = new BTExtendedCeSettings();

        /// <summary>
        /// True the check for BEX availability has been completed.
        /// </summary>
        private static bool BexExistsCheckCompleted { get; set; }

        private static Extended_CE.ModSettings _bexModSettings = null;

        public static Ce.ModSettings BexModSettings
        {
            get { 
                    if(BexExistsCheckCompleted == false)
                    {

                        BexExistsCheckCompleted = true;

                        FieldInfo bexSettingsInfo = AccessTools.Field(typeof(Ce.Core), "Settings");

                        _bexModSettings = (Ce.ModSettings)bexSettingsInfo.GetValue(null);
                    }
                return _bexModSettings; 
            }

            set { _bexModSettings = value; }
        }
    }
}
