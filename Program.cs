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
        static string GameRulesText = "";
        
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
                    dogDAO.EditNameDogs(message.Text,(int)message.Chat.Id);
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
                            case "Правила":
                                await botClient.SendTextMessageAsync(message.Chat, $"{GameRulesText}", replyMarkup: BotControlButtons.GetButtonMainMenu());
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
                                await botClient.SendTextMessageAsync(message.Chat, $"Цена 50 еды за 1000 монет", replyMarkup: BotControlButtons.GetBuyEat());
                                break;
                            case "Купить єнергию":
                                await botClient.SendTextMessageAsync(message.Chat, $"Цена 10 єнергии за 1000 монет", replyMarkup: BotControlButtons.GetBuyEnergy());
                                break;
                            case "Купить собаку":
                                await botClient.SendTextMessageAsync(message.Chat, $"Список собак для покупки(У всех возраст=0):(Переименовать и прокачать можно в Моем Профеле)", replyMarkup: BotControlButtons.GetBuyDogs(dogDAO.DogsShop()));
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
                                await botClient.SendTextMessageAsync(message.Chat, $"Ваши деньги:{Math.Round(userDAO.GetUserByID((int)message.Chat.Id).money)}", replyMarkup: BotControlButtons.GetButtonShop());
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
            
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                int idDog = 0;
                userDAO.UpdateUser(userDAO.GetUserByID((int)update.CallbackQuery.From.Id));
                var user = userDAO.GetUserByID((int)update.CallbackQuery.From.Id);

                //оброботка CallbackQuery
                switch (update.CallbackQuery.Data)
                {   
                    case"Buy energy in the store":
                        
                        if(user.money>=1000)
                        {
                            user.money -= 1000;
                            user.EnergyUser += 10;
                            userDAO.UpdateUser(user);
                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Успешная покупка\nОстаток:💰 {Math.Round(user.money)}\nЄнергии:⚡ {user.EnergyUser}");
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Недостаточно денег");
                        }
                        break;
                    case "Buy eat in the store":
                        
                        if (user.money >= 1000)
                        {
                            user.money -= 1000;
                            user.eat += 50;
                            userDAO.UpdateUser(user);
                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Успешная покупка\nОстаток:💰 {Math.Round(user.money)}\n🍎 Ваш запас еды: {user.eat}");
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Недостаточно денег");
                        }
                        break;
                }

                for (int i = 0; i < 10; i++)
                {
                    if(update.CallbackQuery.Data == Convert.ToString(i))
                    {
                        idDog = dogDAO.GetAllDogsUser((int)update.CallbackQuery.From.Id)[i].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus($"{i}R"));
                        break;
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    if (update.CallbackQuery.Data == $"{i}R")
                    {
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, i);
                        break;
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    List<Dog> dogs = dogDAO.DogsShop();
                    if (update.CallbackQuery.Data == $"{i}B")
                    {
                        if(user.countDog<10)
                        {
                                if(user.money >= dogs[i].lvl * 200)
                                {
                                    user.money -= dogs[i].lvl * 200;
                                    dogs[i].UserId = user.Id;
                                    dogDAO.AddDog("dogs", dogs[i]);
                                    dogDAO.DelDog("tdogsauction2", dogs[i].id);
                                    await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Покупка успешна,Теперь у вас есть новый друг {dogs[i].name}");

                                }
                                
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "У вас максимум собак");
                        }
                        break;
                    }
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
