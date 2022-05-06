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
using Telegram.Bot.Types.ReplyMarkups;
using System.Diagnostics;
using ConsoleTestGI;
using TelegramDOGs.DAO;
using TelegramDOGs.Service;

namespace TelegramDOGs
{
    class Program
    {

        public static ProccesAPI proccesAPI = ProccesAPI.GetProccesAPI();
        public static UserDAO userDAO = UserDAO.GetInstens();
        public static DogDAO dogDAO = DogDAO.GetInstens();
        public static DataBase DB = new DataBase();

        public static bool ButtonActiv = false;
        static ITelegramBotClient bot = new TelegramBotClient("5384438845:AAG6qrDzwcni1Lk8bBIkXAPCJ2-D7YVG6j0");
            
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                
                // Некоторые действия
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    var message = update.Message;
                    userDAO.UpdateUser(userDAO.GetUserByID((int)message.Chat.Id));
                if (ButtonActiv==false)
                {
                   
                    
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Кнопки активейт", replyMarkup: GetButton());
                    ButtonActiv = true;
                }
                    
                    if(message.Text != null)
                    {
                    
                    if (message.Text.ToLower() == "/start")
                    {


                        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.CreateNewUser(userDAO.CreateNewUser((int)message.Chat.Id, message.Chat.FirstName))}");
                        return;
                    }
                    else if (Equals( message.Text.ToLower(), "/finddog"))
                    {

                        await botClient.SendTextMessageAsync(message.Chat, $"{dogDAO.CreatDogRandom((int)message.Chat.Id)}");
                        
                    }
                    else if (message.Text.ToLower() == "/status")
                    {

                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.GetUserByID((int)message.Chat.Id).GetAllStatus()}");
                        
                    }
                    else if (message.Text.ToLower() == "/reg")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.CreateNewUser(userDAO.CreateNewUser((int)message.Chat.Id, message.Chat.FirstName))}");

                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat, $" ID:{message.Chat.Id} UserName: {message.Chat.FirstName} text: {message.Text}");

                    }
                    
                }
                    if(message.Photo != null)
                    {
                    
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Фотка зачёт");
                    }

                   
                }
               
                
                if(update.Type==Telegram.Bot.Types.Enums.UpdateType.EditedMessage)
                {
                    var edited_message = update.EditedMessage;
                    await botClient.SendTextMessageAsync(edited_message.Chat, "Опа кто то изменил сообщение извени я такое не понимаю...");
                    
                }
            }

        private static IReplyMarkup GetButton()
        {
            List<List<KeyboardButton>> KeyboardButtonTest = new List<List<KeyboardButton>>();
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("/finddog"),new KeyboardButton("/status")});
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Купить собаку"), new KeyboardButton("Бой") });
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);            
            return keybord;
        }
        
        public static async Task AdminMessage(ITelegramBotClient botClient)
            {
                Console.WriteLine("Введите ID Пользователя:");
                long ID = long.Parse(Console.ReadLine());
                Console.WriteLine($"Введите сообщение которое нужно оптавить юзеру");
                string Text = Console.ReadLine();
                await botClient.SendTextMessageAsync(ID,Text);
            }        

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                
                 Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            }
        static void Main(string[] args)
         {
               proccesAPI.StartServer();
                
                Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
                
                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { }, 
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
                    proccesAPI.StatusServer();                    
                } 
                if(key.Key == ConsoleKey.PageUp)
                {
                    proccesAPI.StopServer();
                    Console.WriteLine("Отключение TelegramBot");
                    ServerActiv = false;
                    break;
                }
                if(key.Key == ConsoleKey.PageDown)
                {
                    proccesAPI.StopServer();
                }
                 if(key.Key==ConsoleKey.Delete)
                {
                    DB.OpenConnection();
                    string idadmina = "683008996";
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    MySqlCommand command = new MySqlCommand($"DELETE FROM `users` WHERE `id`={idadmina}", DB.GetConnection());
                    adapter.SelectCommand = command;
                    if(command.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Админ удален");
                    }
                    DB.CloseConnection();
                    
                }
            }
               
                


        }
    }
}
