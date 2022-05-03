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
        DogDAO dogs = DogDAO.GetInstens();
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
        
        public User CreateNewUser(int id, string name)
        {
            User user = new User();
            user.Id = id;
            user.Name = name;
            return user;
        }
        public string CreateNewUser(User user)
        {
            string result;
            if (IsAcaunt(Convert.ToInt64(user.Id))==false)
            {
                MySqlCommand Command = new MySqlCommand($"INSERT INTO `users` (`id`, `name`, `countDogs`, `money`, `EnergeUser`, `eat`, `rating`, `DateUpdate`) VALUES (@ID, @UserName,@countDogs,@money,@EnergeUser,@Eat,@rating,@DataUpdate)", DB.GetConnection());
                Command.Parameters.Add("@ID", MySqlDbType.Int32).Value = user.Id;
                Command.Parameters.Add("@UserName", MySqlDbType.VarChar).Value = user.Name;
                Command.Parameters.Add("@countDogs", MySqlDbType.Int32).Value = user.countDog;
                Command.Parameters.Add("@money", MySqlDbType.Int32).Value = user.money;
                Command.Parameters.Add("@EnergeUser", MySqlDbType.Int32).Value = user.EnergyUser;
                Command.Parameters.Add("@Eat", MySqlDbType.Int32).Value = user.eat;
                Command.Parameters.Add("@rating", MySqlDbType.Int32).Value = user.rating;
                Command.Parameters.Add("@DataUpdate", MySqlDbType.Date).Value = user.DataUpdate;

                if (Command.ExecuteNonQuery() == 1)
                {

                    Console.WriteLine("Регестрация успешна");
                    return result = "Регестрация успешна";
                }
                else
                {
                    Console.WriteLine("С регестрацией что то не то");
                    return result = "С регестрацией что то не то";
                }
            }
            else
                return result = "Акаунт уже зарегестрирован.";
                
            
            
        }
        public void DeleteUser(User user)
        {

        }
        public void UpdateUser(User user)
        {
            
        }
        public User GetUserByID(int ID)
        {
            
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand Command = new MySqlCommand($"SELECT * FROM `users` WHERE `id`={ID}", DB.GetConnection());
                adapter.SelectCommand = Command;
                adapter.Fill(table);
                if(table.Rows.Count>0)
                {
                    
                }
                User user = new User();
                foreach (DataRow item in table.Rows)
                {
                    user = InitializationUser(item.ItemArray);
                    
                }
                return user;

            




        }
        private bool IsAcaunt(long ID)
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand Command = new MySqlCommand($"SELECT * FROM `users` WHERE `id`={ID}", DB.GetConnection());
            adapter.SelectCommand = Command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<User> GetAllUsers()
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand Command = new MySqlCommand($"SELECT * FROM `users`", DB.GetConnection());
            adapter.SelectCommand=Command;
            adapter.Fill(table);
            List<User> Users = new List<User>();
            foreach (DataRow Rows in table.Rows)
            {                
                Users.Add(InitializationUser(Rows.ItemArray));
            }
            return Users;
            
        }
        public User InitializationUser(object[] data)
        {
                User user = new User();
                var item = data;
                user.Id = Convert.ToInt32(item[0]);
                user.Name = (string)item[1];
                user.countDog = (int)item[2];
                user.money = (int)item[3];
                user.EnergyUser = (int)item[4];
                user.eat = (int)item[5];
                user.rating = (int)item[6];
                user.DataUpdate = DateTime.Now;
                user.Dogs = dogs.GetDogs(user.Id);
            return user;
        }


    }
}
