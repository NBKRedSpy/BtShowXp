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

    [HarmonyPatch(typeof(SGBarracksWidget), "OpenBarracks")]
    public static class SGBarracksWidget_OpenBarracks_Patch
    {

        private const string LogFileName = "PilotExport.txt";

        public static void Prefix(List<Pilot> pilotRoster, Pilot selectedPilot = null)
        {

            if(pilotRoster == null)
            {
                return;
            }

            var output = new
            {
                Date = DateTime.Now,
                Pilots = new List<PilotData>()
            };


            if (Core.ModSettings.ExportPilots)
            {
                foreach (var pilot in pilotRoster)
                {

                    PilotData pilotData = new PilotData(pilot.Callsign, pilot.SpentXP, pilot.UnspentXP, pilot.TotalXP,
                        pilot.Gunnery, pilot.Piloting, pilot.Guts, pilot.Tactics);

                    pilotData.SetXpDelta(pilot);

                    if(Core.ModSettings.OnlyExportSyncErrorPilots == false || (Core.ModSettings.OnlyExportSyncErrorPilots == true && pilotData.XpDisagree))
                    {
                        output.Pilots.Add(pilotData);
                    }
                }
            }

            if(output.Pilots.Count != 0)
            {
                Logger.LogJson(output, LogFileName);
            }
        }

        private class PilotData
        {
            public string CallSign { get; set; }
            public int SpentXP { get; set; }

            /// <summary>
            /// Will be nonzero if the Spent XP doesn't match the skill's XP cost.
            /// </summary>
            public int SpentXpDelta { get; set; }

            /// <summary>
            /// Will be nonzero if the Total XP doesn't match the skill's XP cost.
            /// </summary>
            public int TotalXpDelta { get; set; }
            public int UnspentXP { get; set; }
            public int TotalXP { get; set; }
            public int Gunnery { get; set; }
            public int Piloting { get; set; }
            public int Guts { get; set; }
            public int Tactics { get; set; }

            /// <summary>
            /// True if the pilot's spent, unspent, and/or Total XP don't match expectation.
            /// </summary>
            public bool XpDisagree { get; set; }

            public PilotData(){}

            public PilotData(string callSign, int spentXP, int unspentXP, int totalXP, int gunnery, int piloting, int guts, int tactics)
            {
                CallSign = callSign;
                SpentXP = spentXP;
                UnspentXP = unspentXP;
                TotalXP = totalXP;
                Gunnery = gunnery;
                Piloting = piloting;
                Guts = guts;
                Tactics = tactics;
            }

            public void SetXpDelta(Pilot pilot)
            {
                int skillXpCost = Core.SkillXpCalc.GetXpCost(pilot);

                if(skillXpCost != SpentXP)
                {
                    SpentXpDelta = SpentXP - skillXpCost;
                    XpDisagree = true;
                }
                    

                if(SpentXP + UnspentXP != TotalXP)
                {
                    TotalXpDelta = TotalXP - (skillXpCost + UnspentXP);
                    XpDisagree = true;
                }
            }
        }

    }
}
