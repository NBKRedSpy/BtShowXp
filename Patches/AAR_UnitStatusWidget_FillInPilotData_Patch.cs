using BattleTech;
using BattleTech.UI;
using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp.Patches
{
    [HarmonyPatch(typeof(AAR_UnitStatusWidget), "FillInPilotData")]
    public static class AAR_UnitStatusWidget_FillInPilotData_Patch
    {
        public static void Prefix(
            AAR_UnitStatusWidget __instance,
            ref int xpEarned,
            SimGameState ___simState,
            UnitResult ___UnitData,
            Contract ___contract)
        {
            LogPilot(___UnitData.pilot, ___contract);
        }

        private static void LogPilot(Pilot pilot, Contract contract)
        {

            if(Core.ModSettings.ShowSkillSyncError)
            {
                int spentXp = Core.SkillXpCalc.GetXpCost(pilot);
                if(spentXp != pilot.SpentXP)
                {
                    {
                        var message = new
                        {
                            Time = DateTime.Now,
                            CallSign = pilot.Callsign,
                            Error = "Pilot SpentXP mismatch.",
                            Actual = pilot.SpentXP,
                            Expected = spentXp,
                        };


                        Logger.LogJson(message);
                    }
                    
                    int totalXpExpected = spentXp + pilot.UnspentXP;

                    if(totalXpExpected != pilot.TotalXP)
                    {
                        var message = new
                        {
                            Time = DateTime.Now,
                            CallSign = pilot.Callsign,
                            Error = "Total XP mismatch.",
                            Actual = pilot.TotalXP,
                            Expected = totalXpExpected,
                        };

                        Logger.LogJson(message);
                    }
                }
            }
            
            if(Core.ModSettings.ShowPilotSummary)
            {
                var message = new
                {
                    Time = DateTime.Now,
                    CallSign = pilot.Callsign,
                    Spent = pilot.SpentXP,
                    Unspent = pilot.UnspentXP,
                    TotalXp = pilot.TotalXP,
                    ContractFinalDifficulty = contract.Override.finalDifficulty,
                };


                Logger.LogJson(message);
            }
        }
    }
}