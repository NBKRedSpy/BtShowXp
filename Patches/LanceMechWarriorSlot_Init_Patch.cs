using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp.Patches
{


    [HarmonyPatch(typeof(SGBarracksRosterSlot), nameof(SGBarracksRosterSlot.Refresh))]
    public class LanceMechWarriorSlot_Init_Patch
    {
        public static void Postfix(Pilot ___pilot, LocalizableText ___callsign)
        {

            //Computed actual XP cost for the skills on the pilot.
            int skillXpCost = Core.SkillXpCalc.GetXpCost(___pilot);

            //Difference between expected and actual XP (corruption check)
            int skillTotalDelta = ___pilot.TotalXP - (skillXpCost + ___pilot.UnspentXP);

            //Formatted text for the XP corruption
            string disagreeText = "";

            if (skillTotalDelta > 0)
            {
                disagreeText = $" XP Mismatch: {skillTotalDelta:N0}";
            }

            ___callsign.SetText($"{___callsign.text} [{___pilot.TotalXP:#,#.##} XP]{disagreeText}");
        }

    }
}
