using BattleTech;
using BattleTech.UI;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ce = Extended_CE;

namespace BtShowXp.Patches
{

    /// <summary>
    /// The after action XP adjustment.
    /// This is a full replacement for the BEX CE's XP Cap to fix a float bug.
    /// </summary>
    [HarmonyPatch]
    public static class BexDifficultyXPCapDisable
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            try
            {
                return new MethodBase[] { BexXpCapPatchUtility.GetBexXpCapMethod() };
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                throw;
            }
        }

        public static bool Prepare()
        {
            return BexXpCapPatchUtility.UseDifficutlyXpPatch();
        }
        private static bool Prefix()
        {
            return false;
        }
    }
}