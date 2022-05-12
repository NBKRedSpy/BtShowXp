﻿using BattleTech;
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
        public static void Postfix( Pilot ___pilot, LocalizableText ___callsign)
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
                    pilotText.Append($"<color=#ff0000>{skillTotalDelta:N0}</color> ");
                }
            }


            if (Core.BTExtendedCeSettings.IsCapEnabled && Core.ModSettings.ShowPilotXpMinDifficulty)
            {
                decimal minXPCapDifficulty = Core.BTExtendedCeSettings.GetXpCapMinDifficulty(___pilot.TotalXP);

                bool showPilotIsUnderXpCap = false;

                int difficutly = -1;
                Contract contract = GetLanceConfigurationContract.Contract;

                //If this is the lance prep screen, check if the pilot is under the XP cap.
                if (contract != null)
                {
                    ///The contract difficutly shown in the UI, which does not show any secret difficulty increases 
                    ///for the mission.
                    ///
                    ///Should always have an override, but being safe since original code tests for null override sometimes.
                    difficutly = contract.Override?.GetUIDifficulty() ?? contract.Difficulty;

                    if (difficutly / 2m >= minXPCapDifficulty)
                    {
                        showPilotIsUnderXpCap = true;   
                    }
                }

                if (showPilotIsUnderXpCap)
                {
                    pilotText.Append($"<color=#4CFF00>Diff: {minXPCapDifficulty} </color>");
                }
                else
                {
                    pilotText.Append($"Diff: {minXPCapDifficulty}");
                }
            }

            if(pilotText.Length != 0)
            {
                ___callsign.SetText(pilotText.ToString().TrimEnd());
            }
            
        }

    }
}
