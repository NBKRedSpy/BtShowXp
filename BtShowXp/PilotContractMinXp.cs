using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    /// <summary>
    /// The Pilot's XP information for a contract.
    /// </summary>
    public class PilotContractMinXp
    {

        public int MinimumContractDifficulty { get; set; }

        public bool IsPercentage { get; set; }

        public PilotContractMinXp()
        {

        }

        public PilotContractMinXp(PilotMinXp pilotMinXp, int contractDifficulty)
        {
            MinimumContractDifficulty = pilotMinXp.MinimumContractDifficulty;
            IsPercentage = contractDifficulty == MinimumContractDifficulty && pilotMinXp.MaxDifficultyPercentage != 1m;
        }

        public override int GetHashCode()
        {
            int hashCode = 341546945;
            hashCode = hashCode * -1521134295 + MinimumContractDifficulty.GetHashCode();
            hashCode = hashCode * -1521134295 + IsPercentage.GetHashCode();
            
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is PilotContractMinXp xp &&
                   MinimumContractDifficulty == xp.MinimumContractDifficulty &&
                   IsPercentage == xp.IsPercentage;
        }
    }


}
