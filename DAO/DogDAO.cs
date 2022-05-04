﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using TelegramDOGs.Entity;
using System.Data;


namespace TelegramDOGs
{
   
    class DogDAO
    {
        DataBase DB;
        #region Singltone

        private static DogDAO Instens;
        private DogDAO()
        {
            DB = new DataBase();
            DB.OpenConnection();
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
            Random R = new Random();
            if (R.Next(0, 100) < 20)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                Dog dog = new Dog(R.Next(0,999999), "ДворнягаСимпатяга", (byte)R.Next(0, 85), (byte)R.Next(0, 10), R.Next(3, 25));
                
                MySqlCommand command = new MySqlCommand($"INSERT INTO `dogs`(`id`,`age`, `name`, `typedog`, `satiety`, `hp`, `lvl`, `Endurance`, `Agility`, `Intelligence`, `userid`, `multiplier`,`regDoguser`) VALUES ('{dog.id}','{dog.age}','{dog.name}','{dog.TypeDogString}','{dog.satiety}','{dog.HP}','{dog.lvl}','{dog.Endurance}','{dog.Agility}','{dog.Intelligence}','{UserId}',@multiplier,@DataUpdate)", DB.GetConnection());
                command.Parameters.Add("@DataUpdate", MySqlDbType.Date).Value = DateTime.Now;
                command.Parameters.Add("@multiplier", MySqlDbType.Double).Value = dog.multiplier;
                adapter.SelectCommand = command;
                if(command.ExecuteNonQuery() == 1)
                { }    

                return $"найдена собака породи {dog.TypeDogString}";
            }
            else return $"Поиск обернулся нечем";


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
