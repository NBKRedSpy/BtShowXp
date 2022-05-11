using BattleTech;
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
        public static Contract Contract { get; set; }


        /// <summary>
        /// Get the contract if the pilot list is in the Lance pre-mission configuration screen.
        /// </summary>
        /// <param name="__instance"></param>
        public static void Prefix(SGBarracksRosterList __instance)
        {
            LanceConfiguratorPanel lanceConfiguratorPanel = null;

            if((lanceConfiguratorPanel = __instance.parentDropTarget as LanceConfiguratorPanel) != null)
            {
                Contract = lanceConfiguratorPanel.activeContract;
            }
            else
            {
                Contract = null;
            }
        }
        
    }
}
