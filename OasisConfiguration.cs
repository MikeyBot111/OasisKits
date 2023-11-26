﻿using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Rocket.Unturned.Player;
using Steamworks;

namespace OasisKits
{
    public class OasisConfiguration : IRocketPluginConfiguration
    {
        public string DatabaseAddress;
        public string DatabaseUsername;
        public string DatabasePassword;
        public string DatabaseName;
        public string DatabaseTableName;
        public int DatabasePort;
        public bool Enabled;

        [XmlArrayItem(ElementName = "Kit")]
        public List<Kit> Kits;
        public int GlobalCooldown;

        public void LoadDefaults()
        {
            DatabaseAddress = "localhost";
            DatabaseUsername = "root";
            DatabasePassword = "";
            DatabaseName = "unturned";
            DatabaseTableName = "OasisKits";
            DatabasePort = 3306;
            Enabled = true;

            GlobalCooldown = 10;
            Kits = new List<Kit>() {
                new Kit() { Cooldown = 10, Name = "Survival", XP = 0,Items = new List<KitItem>() { new KitItem(245, 1), new KitItem(81, 2), new KitItem(16, 1) }},
                new Kit() { Cooldown = 10, Name = "Brute Force", XP = 0, Vehicle = 57,Items = new List<KitItem>() { new KitItem(112, 1), new KitItem(113, 3), new KitItem(254, 3) }},
            };
        }
    }

    public class Kit
    {
        public Kit() { }

        public string Name;
        public uint? XP = null;
        public ushort? Vehicle = null;

        [XmlArrayItem(ElementName = "Item")]
        public List<KitItem> Items;

        public int? Cooldown = null;
    }

    public class KitItem
    {

        public KitItem() { }

        public KitItem(ushort itemId, byte amount)
        {
            ItemId = itemId;
            Amount = amount;
        }

        [XmlAttribute("id")]
        public ushort ItemId;

        [XmlAttribute("amount")]
        public byte Amount;
    }

    public class PlayerCooldownComponent : UnturnedPlayerComponent
    {
        public int Timestamp = 0;
        public string KitName = null;
        public CSteamID PlayerID;
    }
}
