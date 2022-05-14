using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using TelegramDOGs.Entity;
using System.Data;
using System.Threading;
using ConsoleTestGI;
using TelegramDOGs.DAO;

namespace TelegramDOGs
{
   
    class DogDAO
    {
        enum NameDogShop { Дарман, Цезарь, Одиссей, Эмир, Эйс, Локи, Спарки, Олли, Валли, Бєтмен, Альберт, Нора, Дино, Джек, Оливер, Молли, Витти , Тайни , Эрнест , Честер , Монро , Тото };
        
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
                ProccesServec procces = ProccesServec.GetProccesAPI();
                procces.StartServer();
                Thread.Sleep(20000);
                DB = new DataBase();
                DB.OpenConnection();
            }
            
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
        #region QueueDogEdit
        public void AddQueue(int idUser, int idDog)
        {
            MySqlCommand command = new MySqlCommand($"INSERT INTO `renamequeue`(`idDog`, `idUser`) VALUES (@idDog,@idUser)", DB.GetConnection());
            command.Parameters.Add("@idDog", MySqlDbType.Int32).Value = idDog;
            command.Parameters.Add("@idUser", MySqlDbType.Int32).Value = idUser;
            if (command.ExecuteNonQuery() == 1)
            {
                Console.WriteLine("Удачно добавлено в очередь");
            }

        }
        public void DelQueue(int idDog)
        {
            MySqlCommand command = new MySqlCommand($" DELETE FROM `renamequeue` WHERE `idDog`={idDog}", DB.GetConnection());
            if (command.ExecuteNonQuery() == 1)
            {
                Console.WriteLine("Удаление из очереди успешно");
            }
        }
        public int idDog(int userId)
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"SELECT `idDog` FROM `renamequeue` WHERE `idUser`=@idUser", DB.GetConnection());
            command.Parameters.Add("@idUser", MySqlDbType.Int32).Value = userId;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                int idDOg = -1;
                idDOg = (int)table.Rows[0].ItemArray[0];
                return idDOg;
            }
            else
            {
                return -1;
            }

        }
        public bool EditNameDogs(string NewName, int userId)
        {
            int indexDog = idDog(userId);
            if (indexDog >= 0)
            {
                GetAllDogsUser(userId)[indexDog].name = NewName;
                UpdataDogName(NewName, GetAllDogsUser(userId)[indexDog].id);
                DelQueue(indexDog);
                Console.WriteLine("Успешное переиминование> " + NewName);
                return true;
            }
            return false;
        }
        #endregion
        public void DelDog(string nameDB, int idDog)
        {
            MySqlCommand command = new MySqlCommand($"DELETE FROM `{nameDB}` WHERE `id`={idDog}", DB.GetConnection());
            if(command.ExecuteNonQuery()==1)
            {
                Console.WriteLine($"Удалена собака id:{idDog} из баззи данних {nameDB}");
            }
        }
        public void AddDog(string nameDB,Dog dog)
        {
            MySqlCommand Command = new MySqlCommand($"INSERT INTO `{nameDB}`(`id`, `age`, `name`, `typedog`, `satiety`, `hp`, `lvl`, `Endurance`, `Agility`, `Intelligence`, `userid`, `multiplier`, `regDoguser`)" +
                        $" VALUES (@id,@age,@name,@typedog,@satiety,@hp,@lvl,@Endurance,@Agility,@Intelligence,@userid,@multiplier,@DataUpdate)", DB.GetConnection());

            Command.Parameters.Add("@id", MySqlDbType.Int32).Value = dog.id;
            Command.Parameters.Add("@age", MySqlDbType.Float).Value = dog.age;
            Command.Parameters.Add("@name", MySqlDbType.VarChar).Value = dog.name;
            Command.Parameters.Add("@typedog", MySqlDbType.VarChar).Value = dog.TypeDogString;
            Command.Parameters.Add("@satiety", MySqlDbType.Int32).Value = dog.satiety;
            Command.Parameters.Add("@hp", MySqlDbType.Int32).Value = dog.HP;
            Command.Parameters.Add("@Endurance", MySqlDbType.Int32).Value = dog.Endurance;
            Command.Parameters.Add("@Agility", MySqlDbType.Int32).Value = dog.Agility;
            Command.Parameters.Add("@Intelligence", MySqlDbType.Int32).Value = dog.Intelligence;
            Command.Parameters.Add("@userid", MySqlDbType.Int64).Value = dog.UserId;
            Command.Parameters.Add("@lvl", MySqlDbType.Int32).Value = dog.lvl;
            Command.Parameters.Add("@multiplier", MySqlDbType.Double).Value = dog.multiplier;
            Command.Parameters.Add("@DataUpdate", MySqlDbType.DateTime).Value = DateTime.Now;

            if (Command.ExecuteNonQuery() == 1)
            {
                Console.WriteLine($"Успешное добавление в Базу данных:{nameDB}");
            }
        }
        public string CreatDogRandom(User user)
         {
            

            int Energy = 0;
            int countDog = 0;
            Random R = new Random();
            MySqlDataAdapter Adapter = new MySqlDataAdapter();
            MySqlCommand CommandEnergySelect = new MySqlCommand($"SELECT `countDogs`,`EnergeUser` FROM `users` WHERE `id`={user.Id}", DB.GetConnection());
           
            DataTable table = new DataTable();            
            Adapter.SelectCommand = CommandEnergySelect;
            Adapter.Fill(table);
            if (table.Rows.Count>0)
            {
                foreach (DataRow item in table.Rows)
                {
                    countDog = (int)item.ItemArray[0];
                    Energy = (int)item.ItemArray[1];
                    
                }
                if(countDog==10)
                {
                    return "У вас максимальный лимит, свора из 10 собак";
                }
                else
                {
                    if (Energy >= 10)
                    {
                        MySqlCommand CommandEnergyUpdate = new MySqlCommand($"UPDATE `users` SET `EnergeUser`=@SetEnergy WHERE `id`={user.Id}", DB.GetConnection());
                        Energy -= 10;
                        CommandEnergyUpdate.Parameters.Add("@SetEnergy", MySqlDbType.Double).Value = Energy;
                        Adapter.SelectCommand = CommandEnergyUpdate;
                        if (CommandEnergyUpdate.ExecuteNonQuery() == 1)
                        {
                            if (R.Next(0, 100) < 20)
                            {
                                MySqlDataAdapter adapter = new MySqlDataAdapter();
                                Dog dog = new Dog(R.Next(0, 999999), "ДворнягаСимпатяга", (byte)R.Next(0, 85), (byte)R.Next(0, 20), R.Next(3, 25));

                                MySqlCommand command = new MySqlCommand($"INSERT INTO `dogs`(`id`,`age`, `name`, `typedog`, `satiety`, `hp`, `lvl`, `Endurance`, `Agility`, `Intelligence`, `userid`, `multiplier`,`regDoguser`) VALUES ('{dog.id}','{dog.age}','{dog.name}','{dog.TypeDogString}','{dog.satiety}','{dog.HP}','{dog.lvl}','{dog.Endurance}','{dog.Agility}','{dog.Intelligence}','{user.Id}',@multiplier,@DataUpdate)", DB.GetConnection());
                                command.Parameters.Add("@DataUpdate", MySqlDbType.DateTime).Value = DateTime.Now;
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
                
                
                
            }
            return "Ошибочка";

        }
        public List<Dog> DogsShop()
        {
            Random R = new Random();
            List<Dog> DogsShopListLocal = new List<Dog>();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM `tdogsauction2`", DB.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if(table.Rows.Count>=1)
            {
                Console.WriteLine("Собак достаточно для продажи");
                foreach (DataRow item in table.Rows)
                {
                    DogsShopListLocal.Add(IncealizatorDog(item.ItemArray));
                }
            }
            else
            {
                for (int i = 0; i < 10-table.Rows.Count; i++)
                {
                    Dog dog = new Dog(R.Next(0, 999999),Convert.ToString((NameDogShop)R.Next(0,21)),0,R.Next(0,100),(TypeDogs)R.Next(0,6),R.Next(50,100));
                    MySqlCommand Command = new MySqlCommand($"INSERT INTO `tdogsauction2`(`id`, `age`, `name`, `typedog`, `satiety`, `hp`, `lvl`, `Endurance`, `Agility`, `Intelligence`, `userid`, `multiplier`, `regDoguser`)" +
                        $" VALUES (@id,@age,@name,@typedog,@satiety,@hp,@lvl,@Endurance,@Agility,@Intelligence,@userid,@multiplier,@DataUpdate)", DB.GetConnection());
                    
                    Command.Parameters.Add("@id", MySqlDbType.Int32).Value = dog.id;
                    Command.Parameters.Add("@age", MySqlDbType.Int32).Value = dog.age;
                    Command.Parameters.Add("@name", MySqlDbType.VarChar).Value = dog.name;
                    Command.Parameters.Add("@typedog", MySqlDbType.VarChar).Value = dog.TypeDogString;
                    Command.Parameters.Add("@satiety", MySqlDbType.Int32).Value = dog.satiety;
                    Command.Parameters.Add("@hp", MySqlDbType.Int32).Value = dog.HP;
                    Command.Parameters.Add("@Endurance", MySqlDbType.Int32).Value = dog.Endurance;
                    Command.Parameters.Add("@Agility", MySqlDbType.Int32).Value = dog.Agility;
                    Command.Parameters.Add("@Intelligence", MySqlDbType.Int32).Value = dog.Intelligence;
                    Command.Parameters.Add("@userid", MySqlDbType.Int64).Value = dog.UserId;
                    Command.Parameters.Add("@lvl", MySqlDbType.Int32).Value = dog.lvl;
                    Command.Parameters.Add("@multiplier", MySqlDbType.Double).Value = dog.multiplier;
                    Command.Parameters.Add("@DataUpdate", MySqlDbType.Date).Value = DateTime.Now;

                    if (Command.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Успешное добавление в магазин собаки");
                    }
                    
                }
                
                MySqlCommand Sommand = new MySqlCommand($"SELECT * FROM `tdogsauction2`", DB.GetConnection());
                adapter.SelectCommand = Sommand;
                adapter.Fill(table);
                foreach (DataRow item in table.Rows)
                {
                    DogsShopListLocal.Add(IncealizatorDog(item.ItemArray));
                }


            }            
            return DogsShopListLocal;
        }
        public List<Dog> GetAllDogsUser(long userId)
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
        public async void UpdataDog(Dog dog)
        {
            if(dog.isDead()==false)
            {
                Console.WriteLine(dog.TimeLifeDoginfo(dog.age));
                MySqlCommand command = new MySqlCommand($"UPDATE `dogs` SET `age`=@age, `satiety`=@satiety, `hp`={dog.HP}, `lvl`={dog.lvl}, `Endurance`={dog.Endurance}, `Agility`={dog.Agility}, `Intelligence`={dog.Intelligence}, `userid`={dog.UserId},`regDoguser`=@DataUpdate WHERE `id`={dog.id}", DB.GetConnection());
                command.Parameters.Add("@DataUpdate", MySqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.Add("@age", MySqlDbType.Float).Value = dog.age;
                command.Parameters.Add("@satiety", MySqlDbType.Double).Value = dog.satiety;
                if (command.ExecuteNonQuery() == 1)
                {
                    if (AdminClass.ActivChat == false)
                        Console.WriteLine("Собака обновлена");
                }
            }
            else
            {
                MySqlCommand command = new MySqlCommand($"DELETE FROM `dogs` WHERE `id`={dog.id}", DB.GetConnection());
                if (command.ExecuteNonQuery() == 1)
                {
                    if (AdminClass.ActivChat == false)
                        Console.WriteLine("Собака умерла и удалина из базы данных");
                }
            }

        }
        public async void UpdataDogName(string name,int dogId)
        {
            
            
            MySqlCommand command = new MySqlCommand($"UPDATE `dogs` SET `name`=@name WHERE `id`={dogId}", DB.GetConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
           
            if (command.ExecuteNonQuery() == 1)
            {
                if (AdminClass.ActivChat == false)
                    Console.WriteLine("Имя обновлено");
            }
        }
        
        public Dog IncealizatorDog(object[] obj)
        {
            Dog dog = new Dog();
            dog.id = (int)obj[0];
            dog.age = (float)obj[1];
            dog.name = (string)obj[2];
            dog.TypeDogString = (string)obj[3];
            dog.satiety = (int)obj[4];
            dog.HP = (int)obj[5];
            dog.lvl = (int)obj[6];
            dog.Endurance = (int)obj[7];
            dog.Agility = (int)obj[8];
            dog.Intelligence = (int)obj[9];
            dog.UserId = (long)obj[10];
            dog.multiplier = (double)obj[11];
            dog.RegDogUser = (DateTime)obj[12];
            dog.Prace = dog.lvl * 200;
            return dog;
        }
    }
}
