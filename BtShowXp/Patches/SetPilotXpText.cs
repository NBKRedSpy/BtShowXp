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

    /// <summary>
    /// Changes the pilot's Call sign text to have the callsign, total XP, any corrupt XP 
    /// and the mission difficulty for full XP when running BEX XP Caps
    /// </summary>
    [HarmonyPatch(typeof(SGBarracksRosterSlot), nameof(SGBarracksRosterSlot.Refresh))]
    public class SetPilotXpText
    {

        /// <summary>
        /// True if the debug option is enabled and the XP total List was exported.
        /// </summary>
        private static bool DebugOutputtedXpList { get; set; }



        public static void Postfix( Pilot ___pilot, LocalizableText ___callsign)
        {

            try
            {

                //Computed actual XP cost for the skills on the pilot.
                int skillXpCost = Core.SkillXpCalc.GetXpCost(___pilot);

                //Difference between expected and actual XP (corruption check)
                int skillTotalDelta = ___pilot.TotalXP - (skillXpCost + ___pilot.UnspentXP);

                StringBuilder pilotText = new StringBuilder();

                if (Core.ModSettings.ShowPilotXp)
                {

                    decimal xpInThousands = (___pilot.TotalXP / 100) / 10m;

                    pilotText.Append($" [{xpInThousands}K XP] ");
                }

                if (Core.ModSettings.ShowPilotXpCorruption)
                {
                    //Formatted text for the XP corruption
                    if (skillTotalDelta > 0)
                    {
                        pilotText.Append($"<color=#ff0000>{skillTotalDelta:N0}</color> ");
                    }
                }

                Contract contract = GetLanceConfigurationContract.Contract;

                //Contract screen only.
                if (contract != null && Core.BTExtendedCeSettings.IsCapEnabled && Core.ModSettings.ShowPilotXpMinDifficulty)
                {
                    string displayText;

                    PilotMinXp pilotMinXp = Core.BTExtendedCeSettings.GetXpCapMinDifficulty(___pilot.TotalXP);

                    if (Core.ModSettings.DebugOutputPilots)
                    {
                        XpDebugOutput(___pilot, ___callsign, skillTotalDelta, contract, pilotMinXp.MinimumContractDifficulty,
                            pilotMinXp.MaxDifficultyPercentage);
                    }

                    //Clan gets full XP
                    if (contract.Override.targetTeam.FactionValue.IsClan)
                    {
                        displayText = $"<color=#4CFF00>Clan</color>";
                    }
                    else
                    {
                        //The difficulty in display "skull" format.
                        decimal skullMinXPCapDifficulty = pilotMinXp.MinimumContractDifficulty / 2m;

                        string capPercentageText = "";

                        ///The contract difficulty shown in the UI, which does not show any secret difficulty increases 
                        ///for the mission.
                        ///
                        ///Should always have an override, but being safe since original BT code tests for null override sometimes.
                        int contractDifficulty = contract.GetUiDifficulty();

                        if (ShowXpPercentage() && contractDifficulty == pilotMinXp.MinimumContractDifficulty && pilotMinXp.MaxDifficultyPercentage <= .999m)
                        {
                            capPercentageText = Core.ModSettings.XpCapPercentageColors.GetCapPercentageText(pilotMinXp.MaxDifficultyPercentage);

                        }

                        if (contractDifficulty >= pilotMinXp.MinimumContractDifficulty)
                        {

                            PilotContractMinXp pilotContractMinXp = new PilotContractMinXp(pilotMinXp, contractDifficulty);

                            int topColorIndex = GetLanceConfigurationContract.PilotContractMinXpTopEntries.FindIndex(x => x.Equals(pilotContractMinXp));

                            string color = Core.ModSettings.DifficultyColors.GetColor(topColorIndex);
                            displayText = ($"<color={color}>Diff: {skullMinXPCapDifficulty}</color>{capPercentageText}");

                        }
                        else
                        {
                            displayText = ($"Diff: {skullMinXPCapDifficulty}");
                        }
                    }

                    pilotText.Append(displayText);
                }
                
                if (pilotText.Length != 0)
                {
                    string displayText = pilotText.ToString().Trim();
                    ___callsign.SetText("{0} {1}", ___callsign.text,displayText);
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }
            
        }

        /// <summary>
        /// Returns true if the BEX XPCap percentage should be shown
        /// </summary>
        /// <returns></returns>
        private static bool ShowXpPercentage()
        {
            XpPercentageDisplay xpPercentageDisplay = Core.ModSettings.XpPercentageDisplay;


            switch (xpPercentageDisplay)
            {
                case XpPercentageDisplay.Always:
                    return true;
                case XpPercentageDisplay.Off:
                    return false;
                case XpPercentageDisplay.BasedOnPatchStatus:
                    return Core.ModSettings.ShowPilotXpMinDifficultyWorkAround == false && DifficultyXPCapFix.PatchInstalled == true;
                default:
                    throw new ArgumentException($"Unexpected value {xpPercentageDisplay}", nameof(xpPercentageDisplay));
            }
        }

        private static void XpDebugOutput(Pilot ___pilot, LocalizableText ___callsign, int skillTotalDelta, Contract contract, int minXPCapDifficulty, decimal maxLeveCapPercentage)
        {
            //Only the Skill XP total costs table.
            if (DebugOutputtedXpList == false)
            {

                DebugOutputtedXpList = true;

                Logger.LogJson(new
                {
                    Core.SkillXpCalc.LevelExpMap,
                });
            }

            Logger.LogJson(new
            {
                CallSign = ___callsign.text,
                Date = DateTime.Now,
                minXPCapDifficulty,
                maxLeveCapPercentage,
                UIDifficulty = contract.Override?.GetUIDifficulty(),
                contract.Difficulty,
                ___pilot.TotalXP,
                OverrideFinalDifficulty = contract.Override.finalDifficulty,
                skillTotalDelta,
            });
        }
    }
}
