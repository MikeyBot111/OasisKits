using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System.Collections.Generic;
using System;

namespace OasisKits
{
    public class OasisPlugin : RocketPlugin<OasisConfiguration>
    {
        public static string version = "1.0.0.1";
        public static OasisPlugin Instance;

        public static Dictionary<string, DateTime> GlobalCooldown = new Dictionary<string, DateTime>();
        public static Dictionary<string, DateTime> InvididualCooldown = new Dictionary<string, DateTime>();

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList
                {
                    {"command_kit_invalid_parameter","[Oasis] Neznámý příkaz, použí příkaz takto /kit <name>"},
                    {"command_kit_not_found","[Oasis] Kit nebyl nalezen."},
                    {"command_kit_no_permissions","[Oasis] Nemáš oprávnění na použití tohoto kitu."},
                    {"command_kit_cooldown_command","[Oasis] Musíš počkat ještě {0} sekund aby si mohl tento příkaz použít znovu."},
                    {"command_kit_cooldown_kit","[Oasis] Musíš počkat ještě {0} sekund aby si mohl tento kit použít znovu."},
                    {"command_kit_failed_giving_item","[Oasis] Nepodařilo se dát item do {0} ({1},{2})"},
                    {"command_kit_success","[Oasis] Právě jsi dostal kit {0}!" },
                    {"command_kits","[Oasis] Dostupné kity: {0}" },
                    {"command_kit_xp","[Oasis] Dostal jsi {0} xp z kitu {1}." }
                };
            }
        }

        protected override void Load()
        {
            OasisPlugin.Instance = this;

            Logger.Log("OasisKits, Version " + OasisPlugin.version);
            Logger.Log("Checking errors... ");
            Logger.Log("No errors have been found!");
        }

        protected override void Unload()
        {
        }
    }
}

