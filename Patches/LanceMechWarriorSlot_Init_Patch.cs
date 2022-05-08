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

            StringBuilder pilotText = new StringBuilder();

            if (Core.ModSettings.ShowPilotXp)
            {
                pilotText.Append($"{___callsign.text} [{___pilot.TotalXP:#,#.##} XP] ");
            }

            if (Core.ModSettings.ShowPilotXpCorruption)
            {
                //Formatted text for the XP corruption
                if (skillTotalDelta > 0)
                {
                    pilotText.Append($"XP Mismatch: {skillTotalDelta:N0} ");
                }
            }


            if (Core.BTExtendedCeSettings.IsCapEnabled && Core.ModSettings.ShowPilotXpMinDifficulty)
            {
                //Add max difficulty
                pilotText.Append($"Diff: {Core.BTExtendedCeSettings.GetXpCapMinDifficulty(___pilot.TotalXP)} ");

                //Debug
                //Logger.Log(___pilot.Callsign);
            }

            if(pilotText.Length != 0)
            {
                ___callsign.SetText(pilotText.ToString().TrimEnd());
            }
            
        }

    }
}
