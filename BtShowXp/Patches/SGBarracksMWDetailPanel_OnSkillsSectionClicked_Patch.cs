using BattleTech;
using BattleTech.UI;
using Harmony;
using HBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace BtShowXp.Patches
{

    /// <summary>
    /// Reset Corrupt XP functionality.
    /// </summary>
    [HarmonyPatch(typeof(SGBarracksMWDetailPanel), "OnSkillsSectionClicked", MethodType.Normal)]
    public static class SGBarracksMWDetailPanel_OnSkillsSectionClicked_Patch
    {
        private static readonly UIColorRef Backfill = LazySingletonBehavior<UIManager>.Instance.UILookAndColorConstants.PopupBackfill;

        public static bool Prefix(SGBarracksMWDetailPanel __instance, Pilot ___curPilot)
        {


            try
            {

                //Skip if the action is not ctrl+click
                //IsMethodInStack is a workaround as there is a HBS ui element that forces the pointer up event to fire multiple times.
                //  This is an effective workaround without mucking up the event collection.
                if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl)))
                {
                    return true;
                }

                if(Utils.IsMethodInStack("ForceRadioSetSelection"))
                {
                    return true;
                }


                int expectedXpSpent = Core.SkillXpCalc.GetXpCost(___curPilot);

                if (expectedXpSpent == ___curPilot.SpentXP)
                {
                    GenericPopupBuilder.Create("Reset Unspent XP",
                        "The pilot's Unspent XP is already correct.")
                        .AddButton("Ok")
                        .CancelOnEscape()
                        .AddFader(new UIColorRef?(Backfill))
                        .Render();

                    return false;
                }
                else
                {
                    var popup = GenericPopupBuilder.Create("Reset Unspent XP",
                        "This will reset the SpentXP for the pilot to the cost of the skill selected")
                        .AddButton("Cancel")
                        .AddButton("Reset <br>UnspentXP", (Action)(() => SetSpentXp(__instance, ___curPilot, expectedXpSpent)))
                        .CancelOnEscape()
                        .SetAlwaysOnTop()
                        .AddFader(new UIColorRef?(Backfill))
                        .Render();
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex.ToString());

            }

            return false;
        }


        /// <summary>
        /// Sets the pilot's spent XP to the skill xp cost provided.
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="pilot"></param>
        /// <param name="newSpentXpAmount"></param>
        public static void SetSpentXp(SGBarracksMWDetailPanel __instance, Pilot pilot, int newSpentXpAmount)
        {

            try
            {
                Logger.Log($"Retraining {pilot.Callsign} from {pilot.SpentXP} to {newSpentXpAmount}");

                pilot.StatCollection.ModifyStat<int>("SpentXpFix", 0, "ExperienceSpent", StatCollection.StatOperation.Set, (int)newSpentXpAmount, -1, true);
                pilot.pilotDef.SetSpentExperience(pilot.StatCollection.GetValue<int>("ExperienceSpent"));

                ResetBarracksUi(__instance, pilot);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error resetting Spent XP: ${ex}");
            }

        }


        /// <summary>
        /// Forces the parent barracks screen to reset.
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="pilot"></param>
        private static void ResetBarracksUi(SGBarracksMWDetailPanel __instance, Pilot pilot)
        {

            SGBarracksWidget barracks;


            //barracks.Reset(pilot);

            FieldInfo barracksField = AccessTools.Field(typeof(SGBarracksMWDetailPanel), "barracks");

            barracks = (SGBarracksWidget)barracksField.GetValue(__instance);

            MethodInfo resetMethodInfo = AccessTools.Method(typeof(SGBarracksWidget), "Reset",new [] { typeof(Pilot) });

            resetMethodInfo.Invoke(barracks, new [] { pilot });
        }

    }
}
