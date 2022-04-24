using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
namespace TelegramDOGs
{
    class DataBase
    {
        MySqlConnection Connector = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=telegrambotdogsdb"); 
        public void OpenConnection()
        {
            if(Connector.State == System.Data.ConnectionState.Closed)
            {
                Connector.Open();
            }
        }
        public void CloseConnection()
        {
            if (Connector.State == System.Data.ConnectionState.Open)
            {
                Connector.Close();
            }
        }
        public MySqlConnection GetConnection()
        {
            return Connector;
        }
    }
}
