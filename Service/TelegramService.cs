using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Telegram.Bot.Types;
using TelegramDOGs.DAO;

namespace TelegramDOGs.Service
{
    class TelegramService
    {
        private static TelegramService Instens;
        private TelegramService()
        {

        }
        public TelegramService GetInstens()
        {
            if(Instens==null)
            {
                Instens = new TelegramService();
            }
            return Instens;
        }
        public void Temp()
        {
            using(UserDAO userDAO = UserDAO.GetInstens())
            {

            }
        }

        public static string DB_Status(Message message)
        {
            int ID = Convert.ToInt32(message.Chat.Id);
            DataBase DB = new DataBase();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand Command = new MySqlCommand("SELECT * FROM `users` WHERE `id` = @ID", DB.GetConnection());

            Command.Parameters.Add("@ID", MySqlDbType.Int32).Value = ID;
            adapter.SelectCommand = Command;

            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                string t = $"";

                foreach (DataRow item in table.Rows)
                {
                    var Test = item.ItemArray;
                    t = $"Ваше имя: {Test[1]}\nУ вас собак: {Test[2]}\nMoney:{Test[3]}";
                    Console.WriteLine($"Ваше имя: {Test[1]}\nУ вас собак: {Test[2]}\nMoney: {Test[3]}");
                }


                return t;
            }
            else
            {
                Console.WriteLine("Не найдено");
                return "Не найдено";
            }
        }
        public static bool isCreate(string DB_status)
        {
            if (DB_status == "Не найдено")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string Add_DB_Test(Message message)
        {
            return "Я случайно удалил :(";
        }
    }
}
