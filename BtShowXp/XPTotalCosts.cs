using BattleTech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    public class XPTotalCosts
    {


        private List<int> _xpTotalCost;

        /// <summary>
        /// The sum of the total amount of XP required to get to a specific level.
        /// </summary>
        public List<int> XPtotalCost
        {
            get {
                //run init late as the simulation may not be running yet.
                Init();

                return _xpTotalCost;
            }
        }

        public int this[int index]
        {
            get {
                return XPtotalCost[index];
            }
        }

        private void Init()
        {

            if (_xpTotalCost != null) return;

            var sim = UnityGameInstance.BattleTechGame.Simulation;

            if (sim == null)
            {
                throw new ApplicationException("Simulation is null");
            }

            var totalCostList = new List<int>();

            int sum = 0;

            for (int i = 0; i < 10; i++)
            {
                int xpCost = sim.GetLevelCost(i);

                sum += xpCost;

                totalCostList.Add(sum);
            }

            _xpTotalCost = totalCostList;

        }
    }
}
