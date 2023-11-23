using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OasisKits
{
    public class OasisCommandKit : IRocketCommand
    {
        #region Declarations
        public List<string> Permissions => new List<String>() { "OasisKits.kit" };
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "kit";
        public string Syntax => "kit (jméno kitu)";
        public string Help => "Příkaz pro získání kitu.";
        public List<string> Aliases => new List<string>();
        #endregion

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length != 1)
            {
                UnturnedChat.Say(caller, OasisPlugin.Instance.Translations.Instance.Translate("command_kit_invalid_parameter"), UnityEngine.Color.red);
                return;
            }

            Kit kit = OasisPlugin.Instance.Configuration.Instance.Kits.Where(k => k.Name.ToLower() == command[0].ToLower()).FirstOrDefault();
            if (kit == null)
            {
                UnturnedChat.Say(caller, OasisPlugin.Instance.Translations.Instance.Translate("command_kit_not_found"), UnityEngine.Color.red);
                return;
            }

            bool hasPermissions = caller.HasPermission("kit.*") | caller.HasPermission("kit." + kit.Name.ToLower());

            if (!hasPermissions)
            {
                UnturnedChat.Say(caller, OasisPlugin.Instance.Translations.Instance.Translate("command_kit_no_permissions"), UnityEngine.Color.red);
                return;
            }

            KeyValuePair<string, DateTime> globalCooldown = OasisPlugin.GlobalCooldown.Where(k => k.Key == caller.ToString()).FirstOrDefault();
            if (!globalCooldown.Equals(default(KeyValuePair<string, DateTime>)))
            {
                double globalCooldownSeconds = (DateTime.Now - globalCooldown.Value).TotalSeconds;
                if (globalCooldownSeconds < OasisPlugin.Instance.Configuration.Instance.GlobalCooldown)
                {
                    UnturnedChat.Say(caller, OasisPlugin.Instance.Translations.Instance.Translate("command_kit_cooldown_command", (int)(OasisPlugin.Instance.Configuration.Instance.GlobalCooldown - globalCooldownSeconds)), UnityEngine.Color.red);
                    return;
                }
            }

            KeyValuePair<string, DateTime> individualCooldown = OasisPlugin.InvididualCooldown.Where(k => k.Key == (caller.ToString() + kit.Name)).FirstOrDefault();
            if (!individualCooldown.Equals(default(KeyValuePair<string, DateTime>)))
            {
                double individualCooldownSeconds = (DateTime.Now - individualCooldown.Value).TotalSeconds;
                if (individualCooldownSeconds < kit.Cooldown)
                {
                    UnturnedChat.Say(caller, OasisPlugin.Instance.Translations.Instance.Translate("command_kit_cooldown_kit", (int)(kit.Cooldown - individualCooldownSeconds)), UnityEngine.Color.red);
                    return;
                }
            }

            foreach (KitItem item in kit.Items)
            {

                try
                {
                    if (!player.GiveItem(item.ItemId, item.Amount))
                    {
                        Logger.Log(OasisPlugin.Instance.Translations.Instance.Translate("command_kit_failed_giving_item", player.CharacterName, item.ItemId, item.Amount));
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Failed giving item " + item.ItemId + " to player");
                }

            }

            if (kit.XP.HasValue && kit.XP != 0)
            {
                player.Experience += kit.XP.Value;
                UnturnedChat.Say(caller, OasisPlugin.Instance.Translations.Instance.Translate("command_kit_xp", kit.XP.Value, kit.Name), UnityEngine.Color.green);
            }

            if (kit.Vehicle.HasValue)
            {
                try
                {
                    player.GiveVehicle(kit.Vehicle.Value);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Failed giving vehicle " + kit.Vehicle.Value + " to player");
                }
            }

            UnturnedChat.Say(caller, OasisPlugin.Instance.Translations.Instance.Translate("command_kit_success", kit.Name), UnityEngine.Color.green);

            if (OasisPlugin.GlobalCooldown.ContainsKey(caller.ToString()))
            {
                OasisPlugin.GlobalCooldown[caller.ToString()] = DateTime.Now;
            }
            else
            {
                OasisPlugin.GlobalCooldown.Add(caller.ToString(), DateTime.Now);
            }

            if (OasisPlugin.GlobalCooldown.ContainsKey(caller.ToString()))
            {
                OasisPlugin.InvididualCooldown[caller.ToString() + kit.Name] = DateTime.Now;
            }
            else
            {
                OasisPlugin.InvididualCooldown.Add(caller.ToString() + kit.Name, DateTime.Now);
            }
        }
    }

    public class OasisCommandKits : IRocketCommand
    {

        #region Declarations
        public List<string> Permissions => new List<String>() { "OasisKits.kits" };
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "kits";
        public string Syntax => "kits";
        public string Help => "Příkaz pro ukázení všech dostupných kitů.";
        public List<string> Aliases => new List<string>();
        #endregion

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            List<string> availableKits = new List<string>();
            List<Kit> kits = OasisPlugin.Instance.Configuration.Instance.Kits;
            foreach (Kit kit in kits)
            {
                if (caller.HasPermission("kit." + kit.Name.ToLower()))
                {
                    availableKits.Add(kit.Name);
                }
            }

            UnturnedChat.Say(caller, OasisPlugin.Instance.Translations.Instance.Translate("command_kits", String.Join(", ", availableKits.ToArray())), UnityEngine.Color.yellow);
        }
    }
}