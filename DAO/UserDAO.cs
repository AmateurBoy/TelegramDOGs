using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TelegramDOGs.Entity;

namespace TelegramDOGs.DAO
{
    class UserDAO : IDisposable //DAO - Data Access Object -> Объект Доступа к Данным (к Юзеру)
    {
        #region Singleton
        private static UserDAO Instens;
        private UserDAO()
        {
            DB = new DataBase();
            DB.OpenConnection();
        }
        public void Dispose()
        {
            DB.CloseConnection();
        }
        public static UserDAO GetInstens()
        {
            if (Instens == null)
            {
                Instens = new UserDAO();
            }
            return Instens;
        }
        #endregion
        private DataBase DB;
        private Dictionary<int, User> UsersCash = new Dictionary<int, User>();
        private bool CashValid;
        
        public void CreateNewUser(int id, string name)
        {
            User user = new User();
            user.Id = id;
            user.Name = name;
        }
        public void CreateNewUser(User user)
        {
            MySqlCommand Command = new MySqlCommand($"INSERT INTO `users` (`id`, `name`, `countDogs`) VALUES (@ID, @UserName, @CountDogs)", DB.GetConnection());
            Command.Parameters.Add("@ID", MySqlDbType.Int32).Value = user.Id;
            Command.Parameters.Add("@UserName", MySqlDbType.VarChar).Value =user.Name;
            Command.Parameters.Add("@CountDogs", MySqlDbType.Int32).Value = 0;
            
            if (Command.ExecuteNonQuery() == 1)
            {
                UsersCash.Add(user.Id, user);
                Console.WriteLine("Регестрация успешна");
            }
            else
            {

            }
            
        }

        public void DeleteUser(User user)
        {

        }
        public void UpdateUser(User user)
        {
            MySqlCommand Command = new MySqlCommand($"UPDATE INTO `users` (`id`, `name`, `countDogs`) VALUES (@ID, @UserName, @CountDogs) ", DB.GetConnection());
        }
        public void GetUserByID(int ID)
        {

        }
        public List<User> GetAllUsers()
        {
            
        }
        
        
    }
}
