using ConsoleTestGI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Data;

namespace TelegramDOGs.DAO
{
    class QueueRenamingDogsDAO
    {
        DataBase DB;
        #region Singleton
        private static QueueRenamingDogsDAO Instens;
        private QueueRenamingDogsDAO()
        {
            try
            {
                
                DB.OpenConnection();
            }
            catch
            {
                Console.WriteLine("Создать UserDAO не удалось попытка повториться через 20 секунд...");
                ProccesServec procces = ProccesServec.GetProccesAPI();
                procces.StartServer();
                Thread.Sleep(20000);
                DB = new DataBase();
                DB.OpenConnection();

            }
        }
        ~QueueRenamingDogsDAO()
        {
            DB.CloseConnection();
        }
        public static QueueRenamingDogsDAO GetInstens()
        {
            if (Instens == null)
            {
                Instens = new QueueRenamingDogsDAO();
            }
            return Instens;
        }
        #endregion
       
    }
}
