using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TelegramDOGs.Entity;
using MySql.Data;

namespace TelegramDOGs.DAO
{
class UserDAO  //DAO - Data Access Object -> Объект Доступа к Данным (к Юзеру)
{
    DogDAO dogs = DogDAO.GetInstens();
    #region Singleton
    private static UserDAO Instens;
    private UserDAO()
    {
        DB = new DataBase();
        DB.OpenConnection();
    }
     ~UserDAO()
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
            Command.Parameters.Add("@DataUpdate", MySqlDbType.DateTime).Value = DateTime.Now;

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
            
         
         double multsuma = 0;
            if(user.Dogs!=null)
            {
                foreach (Dog item in user.Dogs)
                {
                    multsuma += item.multiplier; 
                }

                double dohod =  multsuma * DateTime.Now.Subtract(user.DataUpdate).Seconds;
                user.money +=  (multsuma * DateTime.Now.Subtract(user.DataUpdate).Seconds);
                
                Console.WriteLine($"Dohod {dohod}/{DateTime.Now.Subtract(user.DataUpdate).Seconds}");
                
            }        
            string ConvertorSQL()
            {
                string S = string.Format("{0:d2}-{1:d2}-{2:d2} {3:d2}:{4:d2}:{5:d2}",DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);
                return S;
            }
           
            MySqlCommand command = new MySqlCommand($"UPDATE `users` SET `money`=@money,`countDogs`=@countDogs,`DateUpdate`=@DataUpdate WHERE `id`=@id", DB.GetConnection());
            command.Parameters.Add("@DataUpdate", MySqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@money", MySqlDbType.Double).Value = user.money;
            command.Parameters.Add("@countDogs", MySqlDbType.Int32).Value = user.Dogs.Count;
            command.Parameters.Add("@id", MySqlDbType.Int32).Value =user.Id;


            if (command.ExecuteNonQuery() == 1)
            {
                Console.WriteLine("Обновлено");
            }


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
            user.money = (double)item[3];
            user.EnergyUser = (int)item[4];
            user.eat = (int)item[5];
            user.rating = (int)item[6];
            user.DataUpdate = (DateTime)item[7];
            user.Dogs = dogs.GetAllDogsUsers(user.Id);
        return user;
    }


}
}
