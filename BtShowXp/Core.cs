using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public static BTExtendedCeSettings BTExtendedCeSettings { get; set; } = BTExtendedCeSettings.BexNotFound();


        public static bool IsBexInstalled => BTExtendedCeSettings.BTExtendedCeInstalled;




    }
}
