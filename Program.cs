﻿using System;
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

        public static ProccesServec proccesAPI = ProccesServec.GetProccesAPI();
        
        public static UserDAO userDAO = UserDAO.GetInstens();
        public static DogDAO dogDAO = DogDAO.GetInstens();
        public static DataBase DB = new DataBase();

        public static bool ButtonActiv = false;
        static ITelegramBotClient bot = new TelegramBotClient("5384438845:AAG6qrDzwcni1Lk8bBIkXAPCJ2-D7YVG6j0");

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            // Некоторые действия
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            /*
            if (AdminClass.ActivChat == false)
            {
                Console.WriteLine($"Id:{update.Message.Chat.Id} Name:{update.Message.Chat.FirstName} " +
                 $"\nText:{update.Message.Text}");
                userDAO.UpdateUser(userDAO.GetUserByID((int)message.Chat.Id));
            }
            else
            {
                if(update.Message.Chat.Id==AdminClass.IDactivChat)
                {
                    Console.WriteLine($"Id:{update.Message.Chat.Id} Name:{update.Message.Chat.FirstName} " +
                    $"\nText:{update.Message.Text}");
                    userDAO.UpdateUser(userDAO.GetUserByID((int)message.Chat.Id));
                }
                
            }
               */
            
            if(message != null)
            {
                if (userDAO.IsAcaunt((int)message.Chat.Id))
                {
                    userDAO.UpdateUser(userDAO.GetUserByID((int)message.Chat.Id));
                    switch (update.Type)
                    {
                        case Telegram.Bot.Types.Enums.UpdateType.Message:
                            {
                                if (message.Photo != null)
                                {
                                    await botClient.SendTextMessageAsync(message.Chat.Id, "Фотка зачёт");
                                }
                                switch (message.Text)
                                {
                                    case "/start":
                                        await botClient.SendTextMessageAsync(message.Chat, "*Тут приветствие, а так же правиа игры (Должны быть :))");
                                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.CreateNewUser(userDAO.CreateNewUser((int)message.Chat.Id, message.Chat.FirstName))}");
                                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.GetUserByID((int)message.Chat.Id).GetAllStatus()}");

                                        break;
                                    case "Мой Профиль":
                                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.GetUserByID((int)message.Chat.Id).GetAllStatus()}", replyMarkup: BotControlButtons.GetButtonMyStatus());
                                        break;
                                    case "Мои Собаки":
                                        await botClient.SendTextMessageAsync(message.Chat, $"Ваши собаки:", replyMarkup: BotControlButtons.GetDogButton(userDAO.GetUserByID((int)message.Chat.Id).Dogs));
                                        break;                                    
                                    case "Главное меню":
                                        await botClient.SendTextMessageAsync(message.Chat, $"Магазин => Главное меню", replyMarkup: GetButton());
                                        break;
                                    case "Купить еды":
                                        break;
                                    case "Купить енергию":
                                        await botClient.SendTextMessageAsync(message.Chat, $"Купить енергию", replyMarkup: BotControlButtons.GetBuy());
                                        break;
                                    case "Купить собаку":
                                        break;
                                    case "Найти собаку":
                                        await botClient.SendTextMessageAsync(message.Chat, $"{dogDAO.CreatDogRandom(userDAO.GetUserByID((int)message.Chat.Id))}", replyMarkup: GetButton());
                                        break;
                                    case "/reg":
                                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.CreateNewUser(userDAO.CreateNewUser((int)message.Chat.Id, message.Chat.FirstName))}", replyMarkup: GetButton());
                                        break;
                                    case "Лучшие игроки":
                                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.Statistic()}", replyMarkup: GetButton());
                                        break;
                                    case "Магазин":
                                        await botClient.SendTextMessageAsync(message.Chat, $"В Разработке", replyMarkup: BotControlButtons.GetButtonShop());
                                        break;
                                    default:
                                        await botClient.SendTextMessageAsync(message.Chat, $" ID:{message.Chat.Id} UserName: {message.Chat.FirstName} text: {message.Text}");
                                        break;
                                }
                                break;
                            }
                        case Telegram.Bot.Types.Enums.UpdateType.EditedMessage:
                        {
                            var edited_message = update.EditedMessage;
                            await botClient.SendTextMessageAsync(edited_message.Chat, "Опа кто то изменил сообщение извени я такое не понимаю...");
                            break;
                        }
                    }
                }
                else
                {
                    if (update.Message.Chat.Id > 0)
                    {
                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.CreateNewUser(userDAO.CreateNewUser((int)message.Chat.Id, message.Chat.FirstName))}");
                        await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.GetUserByID((int)message.Chat.Id).GetAllStatus()}");
                    }
                }
            }
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                //оброботка CallbackQuery
                switch (update.CallbackQuery.Data)
                {
                    case"0":
                        Console.WriteLine("Член нажат");
                        break;
                    case "1":
                        Console.WriteLine("Член нажат");
                        break;
                    case "2":
                        Console.WriteLine("Член нажат");
                        break;
                    case "3":
                        Console.WriteLine("Член нажат");
                        break;
                    case "4":
                        Console.WriteLine("Член нажат");
                        break;
                    case "5":
                        Console.WriteLine("Член нажат");
                        break;
                    case "6":
                        Console.WriteLine("Член нажат");
                        break;
                    case "7":
                        Console.WriteLine("Член нажат");
                        break;
                    case "8":
                        Console.WriteLine("Член нажат");
                        break;
                    case "9":
                        Console.WriteLine("Член нажат");
                        break;

                }
            }
        }

        private static IReplyMarkup GetButton()
        {
            List<List<KeyboardButton>> KeyboardButtonTest = new List<List<KeyboardButton>>();
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Найти собаку"),new KeyboardButton("Мой Профиль")});
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Магазин"), new KeyboardButton("Лучшие игроки") });
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
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
                    Console.WriteLine("AdminMessageActiv" +
                        "" +
                        "" +
                        "" +
                        "=========================================");
                    AdminClass.AdminChat(bot);
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
