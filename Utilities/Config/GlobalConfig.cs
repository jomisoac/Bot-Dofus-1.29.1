using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Utilities.Config
{
    internal class GlobalConfig
    {
        private static List<AccountConfig> accounts_List;
        private static readonly string accounts_List_Path = Path.Combine(Directory.GetCurrentDirectory(), "accounts.bot");
        public static bool show_debug_messages { get; set; }
        public static string loginIP = "34.251.172.139";
        public static short loginPort = 443;

        static GlobalConfig()
        {
            accounts_List = new List<AccountConfig>();
            show_debug_messages = false;
        }

        public static void Load_All_Accounts()
        {
            if (File.Exists(accounts_List_Path))
            {
                accounts_List.Clear();
                using (BinaryReader br = new BinaryReader(File.Open(accounts_List_Path, FileMode.Open)))
                {
                    int accountCount = br.ReadInt32();
                    for (int i = 0; i < accountCount; i++)
                    {
                        accounts_List.Add(AccountConfig.LoadAcccount(br));
                    }
                    show_debug_messages = br.ReadBoolean();
                    loginIP = br.ReadString();
                    loginPort = br.ReadInt16();
                }
            }
            else
            {
                return;
            }
        }

        public static void SaveConfig()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(accounts_List_Path, FileMode.Create)))
            {
                bw.Write(accounts_List.Count);
                accounts_List.ForEach(a => a.SaveAccount(bw));
                bw.Write(show_debug_messages);
                bw.Write(loginIP);
                bw.Write(loginPort);
            }
        }

        public static void AddAccount(string accountUsername, string accountPassword, string server, string characterName) => accounts_List.Add(new AccountConfig(accountUsername, accountPassword, server, characterName));
        public static void DeleteAccount(int accountIndex) => accounts_List.RemoveAt(accountIndex);
        public static AccountConfig Get_Account(string accountUsername) => accounts_List.FirstOrDefault(account => account.accountUsername == accountUsername);
        public static AccountConfig Get_Account(int accountIndex) => accounts_List.ElementAt(accountIndex);
        public static List<AccountConfig> Get_Accounts_List() => accounts_List;
    }
}
