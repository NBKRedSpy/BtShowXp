using BattleTech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp.Patches
{
    public static class Extensions
    {

        /// <summary>
        /// Returns the contract that is actually displayed to the user.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static int GetUiDifficulty(this Contract contract)
        {
            return contract.Override?.GetUIDifficulty() ?? contract.Difficulty;
        }
    }
}
