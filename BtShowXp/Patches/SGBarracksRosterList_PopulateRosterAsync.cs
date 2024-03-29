﻿using BattleTech;
using BattleTech.UI;
using Harmony;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp.Patches
{


    [HarmonyPatch(typeof(SGBarracksRosterList), nameof(SGBarracksRosterList.PopulateRosterAsync))]
    public static class GetLanceConfigurationContract
    {
        /// <summary>
        /// Is set if the list is in the Lance configuration screen and has a contract (which it always should).
        /// The value will be null for all other parent screens.
        /// </summary>
        /// <remarks>This is used by other patches as it is the only place it is available</remarks>
        public static Contract Contract { get; set; }

        /// <summary>
        /// Used to color the best matching pilot XP for the mission.  The top
        /// PilotXp levels ordered by percentage, then by min contract difficulty.
        /// Only contains the top levels to uniquely color.
        /// </summary>
        /// <remarks>Used by the Pilot XP text patch</remarks>
        public static List<PilotContractMinXp> PilotContractMinXpTopEntries { get; set; }

        /// <summary>
        /// Get the contract if the pilot list is in the Lance pre-mission configuration screen.
        /// </summary>
        /// <param name="__instance"></param>
        public static void Prefix(SGBarracksRosterList __instance, List<Pilot> pilotList)
        {
            try
            {
                LanceConfiguratorPanel lanceConfiguratorPanel = null;

                if ((lanceConfiguratorPanel = __instance.parentDropTarget as LanceConfiguratorPanel) != null)
                {
                    Contract = lanceConfiguratorPanel.activeContract;

                    if (Core.IsBexInstalled && Core.ModSettings.ShowPilotXpMinDifficulty)
                    {
                        //emulate 5 star mission is clan, else use the difficulty that is displayed to the user.
                        int uiDifficulty = Contract.TargetFactionIsClan() ? Core.ClanDifficultyEmulation : Contract.GetUiDifficulty();

                        //Get the colors for the top pilot's minimum difficulties.  Used to color difficulty text relative to
                        //  the most experienced pilot that is under the contract's difficulty.
                        PilotContractMinXpTopEntries = GetTopPilotXpLevels(pilotList, uiDifficulty, Core.ModSettings.DifficultyColors.Count);
                    }
                }
                else
                {
                    Contract = null;
                }

                
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pilotList"></param>
        /// <param name="uiDifficulty"></param>
        /// <param name="count">The number of items to return.  Should be the number of colors available.</param>
        /// <returns></returns>
        private static List<PilotContractMinXp>  GetTopPilotXpLevels(List<Pilot> pilotList, int uiDifficulty, int count)
        {


            //percentage descending

            //Order:
            //  Highest without percentage
            //  The remaining:  percentage descending, difficulty
            List<PilotContractMinXp> pilotXpList = pilotList
                .Select(pilot =>
                {
                    PilotMinXp pilotMinXp = Core.BTExtendedCeSettings.GetXpCapMinDifficulty(pilot.TotalXP);

                    return new PilotContractMinXp(pilotMinXp, uiDifficulty);
                })
                .Where(x => x.MinimumContractDifficulty <= uiDifficulty)
                .Distinct()
                .ToList();

            if(pilotXpList.Count  == 0)
            {
                //All levels are lower
                return new List<PilotContractMinXp>();
            }
            else
            {
                int highestDifficultyLevel = pilotXpList.Max(x => x.MinimumContractDifficulty);

                return pilotXpList
                    .OrderByDescending(x => x.MinimumContractDifficulty == highestDifficultyLevel)
                    .OrderByDescending(x => x.IsPercentage)
                    .ThenByDescending(x => x.MinimumContractDifficulty)
                    .Take(count)
                    .ToList();
            }
        }
    }
}
