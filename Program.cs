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
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
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
            
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                int idDog = 0;
                //оброботка CallbackQuery
                switch (update.CallbackQuery.Data)
                {
                    case "Buy energy in the store":
                        Console.WriteLine("Buy energy in the store");
                        break;
                    
                    case "0":
                         idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[0].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("0R"));
                        
                        break;

                    case "1":
                        idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[1].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("1R"));

                        break;
                    case "2":
                        idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[2].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("2R"));
                        break;
                    case "3":
                        idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[3].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("3R"));                        
                        break;
                    case "4":
                        idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[4].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("4R"));
                        break;
                    case "5":
                        idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[5].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("5R"));
                        break;
                    case "6":
                        idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[6].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("6R"));
                        break;
                    case "7":
                        idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[7].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("7R"));
                        break;
                    case "8":
                        idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[8].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("8R"));
                        break;
                    case "9":
                        idDog = dogDAO.GetAllDogsUsers((int)update.CallbackQuery.From.Id)[3].id;
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(idDog).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus("9R"));
                        break;
                    case "0R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 0);
                        break;
                    case "1R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 1);
                        break;
                    case "2R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 2);
                        break;
                    case "3R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 3);
                        break;
                    case "4R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 4);
                        break;
                    case "5R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 5);
                        break;
                    case "6R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 6);
                        break;
                    case "7R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 7);
                        break;
                    case "8R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 8);
                        break;
                    case "9R":
                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                        //Добавляем собаку в очередь на переименовку
                        dogDAO.AddQueue((int)update.CallbackQuery.From.Id, 9);

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
