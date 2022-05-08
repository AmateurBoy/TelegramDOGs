using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TelegramDOGs.Entity;
using MySql.Data;
using System.Threading;

using ConsoleTestGI;

namespace TelegramDOGs.DAO
{
class UserDAO  //DAO - Data Access Object -> Объект Доступа к Данным (к Юзеру)
{
    DogDAO dogs = DogDAO.GetInstens();
    #region Singleton
    private static UserDAO Instens;
    private UserDAO()
    {
        try
        {
            DB = new DataBase();
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

    public User CreateNewUser(int id, string name)
    {
        User user = new User();
        user.Id = id;
        user.Name = name;
        return user;
    }
    public string CreateNewUser(User user)
    {
        
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
                
                return "Регестрация успешна";
            }
            else
            {
                Console.WriteLine("С регестрацией что то не то");
                return "С регестрацией что то не то";
            }
        }
        else
            return "Акаунт уже зарегестрирован.";
    }
    public void DeleteUser(User user)
    {

    }
    public void UpdateUser(User user)
      {
            
         if(user.Id!=0)
            {
                double multsuma = 0;
                if (user.Dogs != null)
                {
                    foreach (Dog item in user.Dogs)
                    {
                        multsuma += item.multiplier;
                    }

                    double dohod = multsuma * DateTime.Now.Subtract(user.DataUpdate).TotalSeconds;
                    user.money += (multsuma * DateTime.Now.Subtract(user.DataUpdate).TotalSeconds);
                    if((int)DateTime.Now.Subtract(user.DataUpdate).TotalSeconds/60>=100)
                    {
                        user.EnergyUser = 100;
                    }
                    else
                    {
                        if(user.EnergyUser<100)
                        user.EnergyUser += (int)DateTime.Now.Subtract(user.DataUpdate).TotalSeconds / 60;
                    }

                }


                MySqlCommand command = new MySqlCommand($"UPDATE `users` SET `money`=@money,`countDogs`=@countDogs,`DateUpdate`=@DataUpdate,`EnergeUser`=@Energy WHERE `id`=@id", DB.GetConnection());
                command.Parameters.Add("@DataUpdate", MySqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.Add("@money", MySqlDbType.Double).Value = user.money;
                command.Parameters.Add("@countDogs", MySqlDbType.Int32).Value = user.Dogs.Count;
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = user.Id;
                command.Parameters.Add("@Energy", MySqlDbType.Int32).Value = user.EnergyUser;


                if (command.ExecuteNonQuery() == 1)
                {
                    if (AdminClass.ActivChat == false)
                        Console.WriteLine("Обновлено");
                }

            }


        }
    public string Statistic()
    {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand Command = new MySqlCommand($"SELECT * FROM `users` ORDER BY `users`.`money` DESC", DB.GetConnection());
            adapter.SelectCommand = Command;
            adapter.Fill(table);
            string result = "";
            int count = 0;
            string Winer(int i)
            {
                switch(i)
                {
                    case 1:
                        return "🥇";
                        break;
                    case 2:
                        return "🥈";
                        break;
                    case 3:
                        return "🥉";
                        break;
                    default:
                        return "🗿";
                        break;
                }
                
            }
            foreach (DataRow item in table.Rows)
            {
                count++;
                if(count>5)
                {
                    break;
                }
                result += $"{Winer(count)} {count}. Имя: {item.ItemArray[1]}." +
                    $"\nDogs: {item.ItemArray[2]}" +
                    $"\nMoney:{Convert.ToDouble(item.ItemArray[3])}\n\n";
            }
            return result;
        }
    public User GetUserByID(int ID)
    {
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter(); 
        MySqlCommand Command = new MySqlCommand($"SELECT * FROM `users` WHERE `id`={ID}", DB.GetConnection());
        adapter.SelectCommand = Command;
        adapter.Fill(table);
        User user = new User();
        user.Id = ID;
        user.Name = "Ноу нейм";
        if (table.Rows.Count>0)
        {
                foreach (DataRow item in table.Rows)
                {
                    user = InitializationUser(item.ItemArray);
                }
                return user;
            }
        else
        {
                CreateNewUser(user);
                return GetUserByID(user.Id);
        }
    }
    public bool IsAcaunt(long ID)
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
