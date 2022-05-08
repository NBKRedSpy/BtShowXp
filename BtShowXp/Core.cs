using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    public static class Core
    {
        public static ModSettings ModSettings { get; set; }

        public static SkillXpCalc SkillXpCalc { get; set; } = new SkillXpCalc();

        public static BTExtendedCeSettings BTExtendedCeSettings { get; set; } = new BTExtendedCeSettings(); 

    }
}
