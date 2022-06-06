using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{

    /// <summary>
    /// The pilot's minimum XP to get XP beyond the BEX XP Cap
    /// </summary>
    public class PilotMinXp 
    {

        /// <summary>
        /// The minimum contract difficulty to get more than the minimum XP Cap
        /// </summary>
        public int MinimumContractDifficulty { get; set; }

        /// <summary>
        /// When the contract matches the minimum contract difficulty, indicates the percentage of XP the pilot will receive.
        /// </summary>
        public decimal MaxDifficultyPercentage { get; set; }

        public PilotMinXp()
        {

        }
        public PilotMinXp(int difficulty, decimal maxDifficultyPercentage)
        {
            MinimumContractDifficulty = difficulty;
            MaxDifficultyPercentage = maxDifficultyPercentage;
        }


        /// <summary>
        /// Returns true if the current contract level would result in a percentage of XP for the pilot.
        /// </summary>
        /// <param name="contractDifficulty"></param>
        /// <returns></returns>
        public bool IsDifficultyPercentage(int contractDifficulty)
        {
            return contractDifficulty == MinimumContractDifficulty && MaxDifficultyPercentage != 1m;
        }

    }


    /// <summary>
    /// Orders by no percentage, then level.
    /// </summary>
    public class PilotMinXpPercentCompararer : Comparer<PilotMinXp>
    {
        public int ContractDifficulty { get; set; }

        public PilotMinXpPercentCompararer(int contractDifficulty)
        {
            ContractDifficulty = contractDifficulty;
        }

        public PilotMinXpPercentCompararer()
        {

        }

        public override int Compare(PilotMinXp x, PilotMinXp y)
        {
            int result;

            result = x.IsDifficultyPercentage(ContractDifficulty).CompareTo(y.IsDifficultyPercentage(ContractDifficulty));
            if (result != 0) return result * -1;

            result = x.MinimumContractDifficulty.CompareTo(y.MinimumContractDifficulty); if (result != 0) return result;

            return 0;
        }
    }


}
