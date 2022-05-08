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
            ModSettings modSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<ModSettings>(settingsJSON);
            Core.ModSettings = modSettings;

            var harmony = HarmonyInstance.Create("io.github.nbk_redspy.BtShowXp");
            harmony.PatchAll(Assembly.GetExecutingAssembly());


            //Try to load BTExtended CE settings if available.
            try
            {
                Core.BTExtendedCeSettings = LoadBTExtendedCeSettings();
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());

                Core.BTExtendedCeSettings = BTExtendedCeSettings.BexNotFound();
            }
        }

        public static BTExtendedCeSettings LoadBTExtendedCeSettings()
        {


            string settingsFilePath = Path.Combine(Utils.GetModPath(), @"..\BT_Extended_CE\mod.json");
            
            //Get the BEX settings
            if (File.Exists(settingsFilePath))
            {
                //Please don't shame me for not using a DTO :)
                BTXSettingsSettingsJson btxSettings = JsonConvert.DeserializeObject<BTXSettingsSettingsJson>(File.ReadAllText(settingsFilePath));


                BTExtendedCeSettings settings = btxSettings.Settings;
                settings.BTExtendedCeSettingsFound = true;
                return settings;

            }
            else
            {
                return BTExtendedCeSettings.BexNotFound();

            }
        }


        public class BTXSettingsSettingsJson
        {
            public BTExtendedCeSettings Settings { get; set; }
        }
    }
}
