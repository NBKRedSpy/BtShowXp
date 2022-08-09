using BattleTech;
using BattleTech.UI;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp.Patches
{

    
    /// <summary>
    /// The after action XP adjustment.
    /// This is a full replacement for the BEX CE's XP Cap to fix a float bug.
    /// </summary>
    [HarmonyPatch(typeof(AAR_UnitStatusWidget), "FillInPilotData")]
    public static class DifficultyXPCapFix
    {

        /// <summary>
        /// Will be true if BEX is installed and the patch target matched the expected code.
        /// </summary>
        public static bool? PatchInstalled { get; private set; }

        public static bool Prepare()
        {
            if (PatchInstalled.HasValue)
            {
                return PatchInstalled.Value;
            }
            else
            {
                PatchInstalled = Core.IsBexInstalled && BexXpCapPatchUtility.UseDifficutlyXpPatch(); 
                return PatchInstalled.Value;
            }
        }

        /// <summary>
        /// Patches the Extended_CE DifficultyXP cap functionality.
        /// Corrects the issue where the max XP cap for a pilot always gives minimum XP instead of a fractional value.
        /// This is a one line change of moving a float cast.
        /// </summary>
        private static void Prefix(AAR_UnitStatusWidget __instance, ref int xpEarned, SimGameState ___simState, UnitResult ___UnitData, Contract ___contract)
        {
            //Note - tried to use a transpile but it was too difficult to debug with the .net Framework and 
            //  mono CodeInstruction differences. (plays tiny violin)

            if (___simState.Constants.Story.MaximumDebt < 21)
                return;
            bool isClan = ___contract.Override.targetTeam.FactionValue.IsClan;
            int totalXp = ___UnitData.pilot.TotalXP;
            int unspentXp = ___UnitData.pilot.UnspentXP;
            int index = Math.Min(9, Math.Max(0, ___contract.Override.finalDifficulty - 1));
            int xpDifficultyCap = Core.BTExtendedCeSettings.XPDifficultyCaps[index];
            int num1 = xpDifficultyCap;
            if (index > 0)
                num1 = Core.BTExtendedCeSettings.XPDifficultyCaps[index - 1];
            float val2 = (float)xpEarned;
            if (!isClan)
            {
                if (totalXp >= xpDifficultyCap)
                    val2 *= Core.BTExtendedCeSettings.MinXPMultiplier;
                else if (totalXp > num1)
                {
                    int num2 = xpDifficultyCap - totalXp;
                    int num3 = xpDifficultyCap - num1;

                    //The one line of code that needs to change
                    //val2 *= Math.Max(Core.BexModSettings.MinXPMultiplier, (float)(num2 / num3));
                    val2 *= Math.Max(Core.BTExtendedCeSettings.MinXPMultiplier, ((float)num2 / num3));
                }
            }
            if (___simState.Constants.Story.MaximumDebt >= 42 && (double)totalXp + (double)val2 >= (double)Core.BTExtendedCeSettings.XPMax && Core.BTExtendedCeSettings.XPCap)
            {
                val2 = (float)Math.Min(xpEarned, Core.BTExtendedCeSettings.XPMax - totalXp);
                    ___UnitData.pilot.StatCollection.Set<int>("ExperienceUnspent", Math.Max(0, Core.BTExtendedCeSettings.XPMax - totalXp));
            }
            else
                    ___UnitData.pilot.StatCollection.Set<int>("ExperienceUnspent", unspentXp - (xpEarned - (int)val2));
            xpEarned = Math.Max(0, (int)val2);

            return;
        }
    }
}