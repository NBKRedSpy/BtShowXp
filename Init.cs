using Harmony;
using System;
using System.Collections.Generic;
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
            ModSettings modSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<ModSettings>(settingsJSON);
            Core.ModSettings = modSettings;

            var harmony = HarmonyInstance.Create("io.github.nbk_redspy.BtShowXp");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Logger.Log("Init was run");
        }
    }
}
