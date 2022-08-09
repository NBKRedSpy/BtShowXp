using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    public static class HarmonyInit
    {
        public static void Init(string directory, string settingsJSON)
        {
            try
            {            
                ModSettings modSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<ModSettings>(settingsJSON);

                //Default for colors since JSON will reuse existing collections
                if(modSettings.DifficultyColors.Count == 0)
                {
                    modSettings.DifficultyColors.AddRange(new string[] {
                        "#4CFF00",
                        "#FFDA38",
                        "#FF4242",
                        "#8EC6FF",
                        "#D582E0"
                    });
                }

                Core.ModSettings = modSettings;


                //Try to load BTExtended CE settings if available.
                try
                {
                    Core.BTExtendedCeSettings = BTExtendedCeSettings.LoadBexCeSettings();
                }
                catch (Exception e)
                {

                    Logger.Log("Load exception");
                    Logger.Log(e.ToString());
                    Core.BTExtendedCeSettings = BTExtendedCeSettings.BexNotFound();
                }

                if (Core.ModSettings.DebugOutput)
                {
                    Logger.LogDebug($"------------ {DateTime.Now}");
                }

                var harmony = HarmonyInstance.Create("io.github.nbk_redspy.BtShowXp");
                harmony.PatchAll(Assembly.GetExecutingAssembly());

            }

            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                throw;
            }
        }
       
    }
}
