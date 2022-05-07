using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using TelegramDOGs.Entity;
using System.Data;
using System.Threading;
using ConsoleTestGI;

namespace TelegramDOGs
{
   
    class DogDAO
    {
        DataBase DB;
        #region Singltone

        private static DogDAO Instens;
        private DogDAO()
        {
            try
            {
                DB = new DataBase();
                DB.OpenConnection();
            }
            catch
            {
                Console.WriteLine("Не удалось создать DogDAO отсутсвует подключение к базеданних повторная попытка через 20 секунд...");
                ProccesAPI procces = ProccesAPI.GetProccesAPI();
                procces.StartServer();
                Thread.Sleep(20000);
                DB = new DataBase();
                DB.OpenConnection();
            }
            
        }
        public void Dispose()
        {
            DB.CloseConnection();
        }
        public static DogDAO GetInstens()
        {
            if (Instens == null)
            {
                Instens = new DogDAO();
            }
            return Instens;
        }
        #endregion
        public string CreatDogRandom(int UserId)
         {
            int Energy = 0;
            Random R = new Random();
            MySqlDataAdapter Adapter = new MySqlDataAdapter();
            MySqlCommand CommandEnergySelect = new MySqlCommand($"SELECT `EnergeUser` FROM `users` WHERE `id`={UserId}", DB.GetConnection());
           
            DataTable table = new DataTable();            
            Adapter.SelectCommand = CommandEnergySelect;
            Adapter.Fill(table);
            if (table.Rows.Count>0)
            {
                foreach (DataRow item in table.Rows)
                {
                    Energy = (int)item.ItemArray[0];

                }
                if (Energy >= 10)
                {
                    MySqlCommand CommandEnergyUpdate = new MySqlCommand($"UPDATE `users` SET `EnergeUser`=@SetEnergy WHERE `id`={UserId}", DB.GetConnection());
                    Energy -= 10;
                    CommandEnergyUpdate.Parameters.Add("@SetEnergy", MySqlDbType.Double).Value = Energy;
                    Adapter.SelectCommand = CommandEnergyUpdate;
                    if (CommandEnergyUpdate.ExecuteNonQuery() == 1)
                    {
                        if (R.Next(0, 100) < 20)
                        {
                            MySqlDataAdapter adapter = new MySqlDataAdapter();
                            Dog dog = new Dog(R.Next(0, 999999), "ДворнягаСимпатяга", (byte)R.Next(0, 85), (byte)R.Next(0, 10), R.Next(3, 25));

                            MySqlCommand command = new MySqlCommand($"INSERT INTO `dogs`(`id`,`age`, `name`, `typedog`, `satiety`, `hp`, `lvl`, `Endurance`, `Agility`, `Intelligence`, `userid`, `multiplier`,`regDoguser`) VALUES ('{dog.id}','{dog.age}','{dog.name}','{dog.TypeDogString}','{dog.satiety}','{dog.HP}','{dog.lvl}','{dog.Endurance}','{dog.Agility}','{dog.Intelligence}','{UserId}',@multiplier,@DataUpdate)", DB.GetConnection());
                            command.Parameters.Add("@DataUpdate", MySqlDbType.Date).Value = DateTime.Now;
                            command.Parameters.Add("@multiplier", MySqlDbType.Double).Value = dog.multiplier;
                            adapter.SelectCommand = command;
                            if (command.ExecuteNonQuery() == 1)
                            { }

                            return $"найдена собака породы {dog.TypeDogString}";
                        }
                        else return $"Поиск обернулся ничем";
                    }
                    return "";
                }
                else
                {
                    return "Недостаточно енергии...";
                }
                
            }
            return "Ошибочка";

        }
        public List<Dog> GetAllDogsUsers(int userId)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM `dogs` WHERE `userid`={userId}", DB.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            List<Dog> Dogs = new List<Dog>();
            foreach (DataRow item in table.Rows)
            {
                Dogs.Add(IncealizatorDog(item.ItemArray));
            }
            return Dogs;
        }
        public Dog GetDog(int idDog)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM `dogs` WHERE `id`={idDog}", DB.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            Dog dog = new Dog();
            foreach (DataRow item in table.Rows)
            {
                dog = IncealizatorDog(item.ItemArray);
            }
            return dog;
        }
        public Dog IncealizatorDog(object[] obj)
        {
            Dog dog = new Dog();
            dog.id = (int)obj[0];
            dog.age = (int)obj[1];
            dog.name = (string)obj[2];
            dog.TypeDogString = (string)obj[3];
            dog.satiety = (int)obj[4];
            dog.HP = (int)obj[5];
            dog.lvl = (int)obj[6];
            dog.Endurance = (int)obj[7];
            dog.Agility = (int)obj[8];
            dog.Intelligence = (int)obj[9];
            dog.UserId = (int)obj[10];
            dog.multiplier = (double)obj[11];
            dog.RegDogUser = (DateTime)obj[12];
            return dog;
        }
    }
}
