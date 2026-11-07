# OasisKits

> **⚠️ Archived – old project, no longer maintained.**
> Written in 2023 for the **Oasis** Unturned servers. It's kept here for reference only. It may not work with current versions of Unturned or RocketMod.

A RocketMod plugin for **Unturned** that adds item **kits**. Players can claim preset bundles of items, XP and an optional vehicle. Cooldowns are saved in **MySQL**, so they still apply after a restart or rejoin.

## What it does

- Kits are defined in the config. Each kit has a name, a list of items with amounts, optional XP, an optional vehicle and a cooldown
- A per-kit cooldown for each player is saved in MySQL
- A global cooldown applies across all kits
- Each kit has its own permission (`kit.<name>` or `kit.*`)
- The database table is created automatically on first load

## Commands & permissions

| Command | Permission | Description |
|---|---|---|
| `/kit <name>` | `OasisKits.kit` + `kit.<name>` | Claim a kit |
| `/kits` | `OasisKits.kits` | List the kits you can use |

## Config

| Key | Default |
|---|---|
| `DatabaseAddress` | `localhost` |
| `DatabaseUsername` | `root` |
| `DatabasePassword` | *(empty)* |
| `DatabaseName` | `unturned` |
| `DatabaseTableName` | `OasisKits` |
| `DatabasePort` | `3306` |
| `GlobalCooldown` | `10` |
| `Kits` | list of `Kit` entries |

The repo only has placeholder defaults. Real database details are set in the server's plugin config.

## Tech

- C# / .NET Framework 4.8
- RocketMod, MySqlConnector
- `lib/` holds the reference assemblies needed to build

## Build

Open `OasisKits.sln` in Visual Studio and build it. Then copy the DLL (and `MySqlConnector.dll`) into `Rocket/Plugins`.

## Part of the Oasis project

[OasisEssential](https://github.com/MikeyBot111/OasisEssential) · [OasisTeleport](https://github.com/MikeyBot111/OasisTeleport) · [OasisKits](https://github.com/MikeyBot111/OasisKits) · [OasisRanks](https://github.com/MikeyBot111/OasisRanks) · [OasisVault](https://github.com/MikeyBot111/OasisVault) · [OasisDiscord](https://github.com/MikeyBot111/OasisDiscord) · [OasisCustomCommands](https://github.com/MikeyBot111/OasisCustomCommands) · [WebsiteOasis](https://github.com/MikeyBot111/WebsiteOasis) · [oasis-server-discord](https://github.com/MikeyBot111/oasis-server-discord)
