using Rocket.Core.Logging;
using Steamworks;
using System;
using System.Collections.Generic;
using MySqlConnector;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace OasisKits
{
    internal class Database
    {
        private static string CompileArguments(string command, string[] args, string[] argsValues)
        {
            string NewCommand = command;
            int i = 0;
            foreach (string arg in args)
            {
                NewCommand = NewCommand.Replace(arg, argsValues[i]);
                i++;
            }
            return NewCommand;
        }

        private static MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;
            var builder = new MySqlConnectionStringBuilder
            {
                Server = OasisPlugin.Instance.Configuration.Instance.DatabaseAddress,
                UserID = OasisPlugin.Instance.Configuration.Instance.DatabaseUsername,
                Password = OasisPlugin.Instance.Configuration.Instance.DatabasePassword,
                Database = OasisPlugin.Instance.Configuration.Instance.DatabaseName,
                Port = (uint)OasisPlugin.Instance.Configuration.Instance.DatabasePort,
                AllowUserVariables = true
            };

            try
            {
                connection = new MySqlConnection(builder.ConnectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                Logger.LogWarning("MySql Connection Failed");
                Logger.LogException(ex);
            }

            return connection;
        }


        public static void CheckSchema()
        {
            MySqlConnection mySqlConnection = Database.CreateConnection();
            if (mySqlConnection == null)
            {
                Logger.LogWarning("MySql Schema Failed - Connection null");
                return;
            }

            try
            {
                var getCommand = mySqlConnection.CreateCommand();

                string[] args = { "@Tablename" };
                string[] argsValues = { OasisPlugin.Instance.Configuration.Instance.DatabaseTableName };
                string cmd = "SHOW TABLES LIKE '@Tablename'";

                getCommand.CommandText = CompileArguments(cmd, args, argsValues);
                var reader = getCommand.ExecuteReader();
                if (reader.Read().ToString() == "False")
                {
                    reader.Close();
                    var setCommand = mySqlConnection.CreateCommand();

                    string[] args2 = { "@Tablename" };
                    string[] argsValues2 = { OasisPlugin.Instance.Configuration.Instance.DatabaseTableName };
                    string cmd2 = "CREATE TABLE `@Tablename` (`id` int(11) NOT NULL AUTO_INCREMENT,`csteamid` varchar(32) NOT NULL,`cooldown` varchar(11) NOT NULL,`kitname` varchar(32) NOT NULL ,PRIMARY KEY (`id`))";

                    setCommand.CommandText = CompileArguments(cmd2, args2, argsValues2);
                    setCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogWarning("MySql Schema Failed");
                Logger.LogException(ex);
                mySqlConnection.Close();
            }
        }

        internal static bool AddUser(PlayerCooldownComponent playerCooldown)
        {
            MySqlConnection mySqlConnection = Database.CreateConnection();
            if (mySqlConnection == null)
            {
                Logger.LogWarning("MySql AddUser Failed - Connection null");
                return false;
            }
            try
            {

                string[] args = { "@Tablename", "@csteamid", "@timestamp", "@kitname" };
                string[] argsValues = { OasisPlugin.Instance.Configuration.Instance.DatabaseTableName, playerCooldown.PlayerID.ToString(), playerCooldown.Timestamp.ToString(), playerCooldown.KitName };
                string cmd = "INSERT INTO `@Tablename` (`csteamid`,`cooldown`,`kitname`) VALUES('@csteamid', '@timestamp', '@kitname');";

                MySqlCommand command1 = mySqlConnection.CreateCommand();
                command1.CommandText = CompileArguments(cmd, args, argsValues);

                command1.ExecuteNonQuery();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("MySql AddUser Failed");
                Logger.LogException(ex);
                mySqlConnection.Close();
                return false;
            }
        }

        internal static bool DeleteUser(CSteamID PlayerID, string KitName)
        {
            MySqlConnection mySqlConnection = Database.CreateConnection();
            if (mySqlConnection == null)
            {
                Logger.LogWarning("MySql DeleteUser Failed - Connection null");
                return false;
            }
            try
            {

                string[] args = { "@Tablename", "@cSteamId", "@kitName" };
                string[] argsValues = { OasisPlugin.Instance.Configuration.Instance.DatabaseTableName, PlayerID.ToString(), KitName};
                string cmd = "DELETE FROM `@Tablename` WHERE `csteamid` = '@cSteamId' AND `kitname` = '@kitName' LIMIT 1;";

                MySqlCommand command1 = mySqlConnection.CreateCommand();
                command1.CommandText = CompileArguments(cmd, args, argsValues);

                command1.ExecuteNonQuery();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("MySql DeleteUser Failed");
                Logger.LogException(ex);
                mySqlConnection.Close();
                return false;
            }
        }

        internal static PlayerCooldownComponent RetrieveUser(CSteamID PlayerID, string KitName)
        {
            PlayerCooldownComponent ReturnInfo = new PlayerCooldownComponent();

            MySqlConnection mySqlConnection = Database.CreateConnection();
            if (mySqlConnection == null)
            {
                Logger.LogWarning("MySql RetrieveUser Failed - Connection null");
                return new PlayerCooldownComponent();
            }

            try
            {
                int num = 0;

                string[] args = { "@Tablename", "@cSteamId", "@kitName" };
                string[] argsValues = { OasisPlugin.Instance.Configuration.Instance.DatabaseTableName, PlayerID.ToString(), KitName };
                string cmd = "SELECT COUNT(*) FROM `@Tablename` WHERE `csteamid` = '@cSteamId' AND `kitname` = '@kitName';";

                MySqlCommand command1 = mySqlConnection.CreateCommand();
                command1.CommandText = CompileArguments(cmd, args, argsValues);
                var reader = command1.ExecuteReader();
                while (reader.Read())
                {
                    num = reader.GetInt32(0);
                }
                reader.Close();
                if (num == 0)
                {
                    return new PlayerCooldownComponent();
                }

                string[] args2 = { "@Tablename", "@cSteamId", "@kitName" };
                string[] argsValues2 = { OasisPlugin.Instance.Configuration.Instance.DatabaseTableName, PlayerID.ToString(), KitName };
                string cmd2 = "SELECT * FROM `@Tablename` WHERE `csteamid` = '@cSteamId' AND `kitname` = '@kitName' LIMIT 1;";
                MySqlCommand command2 = mySqlConnection.CreateCommand();
                command2.CommandText = CompileArguments(cmd2, args2, argsValues2);
                var reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    ReturnInfo.KitName = reader2.GetString("kitname");
                    ReturnInfo.PlayerID = (CSteamID)ulong.Parse(reader2.GetString("csteamid"));
                    ReturnInfo.Timestamp = int.Parse(reader2.GetString("cooldown"));
                }
                reader2.Close();
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogWarning("MySql RetrieveUser Failed");
                Logger.LogException(ex);
                mySqlConnection.Close();
            }

            return ReturnInfo;
        }
    }
}
