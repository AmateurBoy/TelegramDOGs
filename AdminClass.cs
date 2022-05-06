using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Telegram.Bot.Types;
using TelegramDOGs.Entity;

namespace TelegramDOGs
{
    static class AdminClass
    {
        static DataBase DB = new DataBase();
        public static void AddUserPanel(Message message)
        {
            if(IsFind((int)message.Chat.Id))
            {
                MySqlCommand Command = new MySqlCommand($"INSERT INTO `adminpanel`(`Id`, `Name`) VALUES(@ID,@UserName)", DB.GetConnection());
                Command.Parameters.Add("@ID", MySqlDbType.Int32).Value = message.Chat.Id;
                Command.Parameters.Add("@UserName", MySqlDbType.VarChar).Value = message.Chat.FirstName;
                if (Command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("Пользовтель добавлен в панель быстрого доступа");
                }
            }
            else
            {
                Console.WriteLine("Уже есть в базе данных");
            }
            
        }
        public static Dictionary<int, string> GetAllUserPanel()
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand Command = new MySqlCommand($"SELECT * FROM `adminpanel`", DB.GetConnection());
            adapter.SelectCommand = Command;
            Dictionary<int, string> Diction = new Dictionary<int, string>();
            foreach (DataRow Rows in table.Rows)
            {
                Diction.Add((int)Rows.ItemArray[0], (string)Rows.ItemArray[1]);
            }
            return Diction;
        }
        public static Dictionary<int, string> GetUserPanel(int id)
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand Command = new MySqlCommand($"SELECT * FROM `adminpanel` WHERE `id`={id}", DB.GetConnection());
            adapter.SelectCommand = Command;
            Dictionary<int, string> Diction = new Dictionary<int, string>();
            foreach (DataRow Rows in table.Rows)
            {
                Diction.Add((int)Rows.ItemArray[0],(string)Rows.ItemArray[1]);
            }
            return Diction;
        }
        public static bool IsFind(int id)
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand Command = new MySqlCommand($"SELECT * FROM `adminpanel` WHERE `id`={id}", DB.GetConnection());
            adapter.SelectCommand = Command;
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void Chat()
        {
            
        }
    }
}
