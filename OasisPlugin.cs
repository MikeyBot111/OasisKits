using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System.Collections.Generic;
using System;

namespace OasisKits
{
    public class OasisPlugin : RocketPlugin<OasisConfiguration>
    {
        public static string version = "1.0.0.3";
        public static OasisPlugin Instance;

        public static Dictionary<string, DateTime> GlobalCooldown = new Dictionary<string, DateTime>();

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList
                {
                    { "command_kit_invalid_parameter", "[Oasis] Unknown command, use the command like this /kit <name>" },
                    { "command_kit_not_found", "[Oasis] Kit not found." },
                    { "command_kit_no_permissions", "[Oasis] You do not have permission to use this kit." },
                    { "command_kit_cooldown_command", "[Oasis] You have to wait {0} seconds to use this command again." },
                    { "command_kit_cooldown_kit", "[Oasis] You have to wait {0} seconds to use this kit again." },
                    { "command_kit_failed_giving_item", "[Oasis] Failed to give item to {0} ({1},{2})" },
                    { "command_kit_success", "[Oasis] You have just received the kit {0}!" },
                    { "command_kits", "[Oasis] Available kits: {0}" },
                    { "command_kit_xp", "[Oasis] You have received {0} xp from the kit {1}." }
                };
            }
        }

        protected override void Load()
        {
            OasisPlugin.Instance = this;

            Logger.Log("OasisKits, Version " + OasisPlugin.version);
            Logger.Log("Checking errors... ");
            if (Configuration.Instance.Enabled)
            {
                Database.CheckSchema();
            }
            Logger.Log("No errors have been found!");
        }

        protected override void Unload()
        {
        }
    }
}

