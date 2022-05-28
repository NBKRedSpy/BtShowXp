using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    /// <summary>
    /// How to display the percentage XP projected to be given.
    /// </summary>
    public enum XpPercentageDisplay
    {
        /// <summary>
        /// Always show, regardless of other settings.
        /// </summary>
        Always = 1, 

        /// <summary>
        /// Never show
        /// </summary>
        Off, 

        /// <summary>
        /// Will be shown if the BEX visual workaround is disabled and the BEX XP Cap bug fix is enabled.
        /// enabled.
        /// </summary>
        BasedOnPatchStatus,
    }
}
