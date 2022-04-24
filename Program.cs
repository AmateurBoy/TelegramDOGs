using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace TelegramDOGs
{
    class Program
    {
            
            static ITelegramBotClient bot = new TelegramBotClient("5384438845:AAG6qrDzwcni1Lk8bBIkXAPCJ2-D7YVG6j0");
            
            public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Некоторые действия
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                   
                    var message = update.Message;
                    
                    if (message.Text.ToLower() == "/start")
                    {
                        
                        
                        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                        return;
                    }
                    else if(message.Text.ToLower() == "/status")
                    {
                        
                        await botClient.SendTextMessageAsync(message.Chat, $"{DB_Status(message)}");
                    }
                    else if(message.Text.ToLower() == "/reg")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, $"{Add_DB_Test(message)}");
                        
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat, $" ID:{message.Chat.Id} UserName: {message.Chat.FirstName} text: {message.Text}");

                    }



            }
                if(update.Type==Telegram.Bot.Types.Enums.UpdateType.EditedMessage)
                {
                    var edited_message = update.EditedMessage;
                    await botClient.SendTextMessageAsync(edited_message.Chat, "Опа кто то изменил сообщение извени я такое не понимаю...");
                }
            }
            public static async Task AdminMessage(ITelegramBotClient botClient)
            {
                Console.WriteLine("Введите ID Пользователя:");
                long ID = long.Parse(Console.ReadLine());
                Console.WriteLine($"Введите сообщение которое нужно оптавить юзеру");
                string Text = Console.ReadLine();
                await botClient.SendTextMessageAsync(ID,Text);
            }
            public static string DB_Status(Message message)
        {
            int ID =Convert.ToInt32(message.Chat.Id);
            DataBase DB = new DataBase();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand Command = new MySqlCommand("SELECT * FROM `users` WHERE `id` = @ID",DB.GetConnection());
            Command.Parameters.Add("@ID",MySqlDbType.Int32).Value=ID;
            adapter.SelectCommand = Command;
            adapter.Fill(table);
            if(table.Rows.Count>0)
            {
                Console.WriteLine($"Найден пользователь ID: {ID}");
                string t = $"Найден пользователь ID: {ID}";
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
                if(DB_status== "Не найдено")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public static string Add_DB_Test(Message message)
        {
            if(isCreate(DB_Status(message)))
            {
                DataBase DB = new DataBase();

                MySqlCommand Command = new MySqlCommand($"INSERT INTO `users` (`id`, `name`, `countDogs`) VALUES (@ID, @UserName, @CountDogs)", DB.GetConnection());
                Command.Parameters.Add("@ID", MySqlDbType.Int32).Value = Convert.ToInt32(message.Chat.Id);
                Command.Parameters.Add("@UserName", MySqlDbType.VarChar).Value = message.Chat.FirstName;
                Command.Parameters.Add("@CountDogs", MySqlDbType.Int32).Value = 0;
                DB.OpenConnection();
                if (Command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("Регестрация успешна");
                }
                else
                {

                }
                DB.CloseConnection();
                return "Акаунт зарегестрирован.";
            }
            else
            {
                return "Акаунт уже есть...";
            }
            
            
        }
            public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                // Некоторые действия
                 Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            }
        static void Main(string[] args)
        {
                Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { }, // receive all update types
                };
                bot.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken
                );


            bool ServerActiv = true;  
            while (ServerActiv)
            {
                ConsoleKeyInfo key;
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.End)
                {
                    Console.WriteLine("AdminMessageActiv");
                    AdminMessage(bot);

                }
                if(key.Key == ConsoleKey.Home)
                {
                    Console.WriteLine("Сервер закрыт завершение Администратором");
                    ServerActiv = false;
                    break;
                }    
            }
               
                


        }
    }
}
