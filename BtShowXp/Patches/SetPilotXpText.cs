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
                Contract contract = GetLanceConfigurationContract.Contract;
                if (contract == null) return;   //Just in case.  Contract should always be set.


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
                        pilotText.Append($"<color=#ff0000>{skillTotalDelta:N0}</color> ");
                    }
                }
                

                if (Core.BTExtendedCeSettings.IsCapEnabled && Core.ModSettings.ShowPilotXpMinDifficulty)
                {
                    string displayText;

                    //Clan gets full XP
                    if (contract.Override.targetTeam.FactionValue.IsClan)
                    {
                        displayText = $"<color=#4CFF00>Clan</color>";
                    }
                    else
                    {
                        int minXPCapDifficulty;     //Minimum contract difficulty to get more than minimum XP
                        decimal maxLevelCapPercentage;   //The amount of XP at the pilot's max XP cap.

                        Core.BTExtendedCeSettings.GetXpCapMinDifficulty(___pilot.TotalXP, out minXPCapDifficulty,
                            out maxLevelCapPercentage);

                        if (Core.ModSettings.DebugOutput)
                        {
                            XpDebugOutput(___pilot, ___callsign, skillTotalDelta, contract, minXPCapDifficulty, maxLevelCapPercentage);
                        }



                        //The difficulty in display "skull" format.
                        decimal skullMinXPCapDifficulty = minXPCapDifficulty / 2m;

                        string capPercentageText = "";

                        ///The contract difficutly shown in the UI, which does not show any secret difficulty increases 
                        ///for the mission.
                        ///
                        ///Should always have an override, but being safe since original BT code tests for null override sometimes.
                        int contractDifficutly = contract.Override?.GetUIDifficulty() ?? contract.Difficulty;

                        if (ShowXpPercentage() && contractDifficutly == minXPCapDifficulty && maxLevelCapPercentage <= .999m)
                        {
                            capPercentageText = GetCapPercentageText(maxLevelCapPercentage);
                        }


                        if (contractDifficutly >= minXPCapDifficulty)
                        {
                            displayText = ($"<color=#4CFF00>Diff: {skullMinXPCapDifficulty}</color>{capPercentageText}");
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
                    ___callsign.SetText(pilotText.ToString().TrimEnd());
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


        /// <summary>
        /// Returns the HTML formatted text for the "XP award percentage" projection text.
        /// </summary>
        /// <param name="maxLeveCapPercentage"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private static string GetCapPercentageText(decimal maxLeveCapPercentage)
        {
            string capPercentageText;
            string capColor;

            int colorRange = Math.Min(3, (int)(maxLeveCapPercentage * 100) / 25);

            switch (colorRange)
            {
                case 0:
                    capColor = "#EE5E43";
                    break;
                case 1:
                    capColor = "#F59338";
                    break;
                case 2:
                    capColor = "#EFC501";
                    break;
                case 3:
                    capColor = "#4CFF00";
                    break;
                default:
                    throw new ApplicationException($"Unexpected {nameof(colorRange)} value: {colorRange}");
            }

            capPercentageText = maxLeveCapPercentage >= .99m ? "" :
                $" <color=#FFFFFF>[</color><color={capColor}>{maxLeveCapPercentage:P0}<color=#FFFFFF>]</color></color>";
            return capPercentageText;
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
