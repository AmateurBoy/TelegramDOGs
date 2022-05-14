using ConsoleTestGI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramDOGs.DAO;

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
           // 
            var message = update.Message;
            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Unknown:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    {
                        
                        
                        if (userDAO.IsAcaunt(update.Message.Chat.Id))
                        {
                            
                            userDAO.UpdateUser(userDAO.GetUserByID((int)message.Chat.Id));
                            if(dogDAO.EditNameDogs(message.Text, (int)message.Chat.Id))
                            {
                                await botClient.SendTextMessageAsync(message.Chat.Id, "Успешное переименование.");
                            }
                            
                            if (message.Photo != null)
                            {
                                await botClient.SendTextMessageAsync(message.Chat.Id, "Фотка зачёт");
                            }
                            switch (message.Text)
                            {
                                case "/start":
                                    await botClient.SendTextMessageAsync(message.Chat, "*Тут приветствие, а так же правиа игры (Должны быть :))");
                                    await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.CreateNewUser(userDAO.CreateNewUser(message.Chat.Id, message.Chat.FirstName))}");
                                    await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.GetUserByID(message.Chat.Id).GetAllStatus()}");

                                    break;
                                case "Правила":
                                    await botClient.SendTextMessageAsync(message.Chat, $"{GameRulesText}", replyMarkup: BotControlButtons.GetButtonMainMenu());
                                    break;
                                case "Мой Профиль":
                                    await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.GetUserByID(message.Chat.Id).GetAllStatus()}", replyMarkup: BotControlButtons.GetButtonMyStatus());
                                    break;
                                case "Мои Собаки":
                                    await botClient.SendTextMessageAsync(message.Chat, $"Ваши собаки:", replyMarkup: BotControlButtons.GetDogButton(userDAO.GetUserByID((int)message.Chat.Id).Dogs));
                                    break;
                                case "Главное меню":
                                    await botClient.SendTextMessageAsync(message.Chat, $"Магазин => Главное меню", replyMarkup: BotControlButtons.GetButtonMainMenu());

                                    await botClient.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
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
                                    await botClient.SendTextMessageAsync(message.Chat, $"{dogDAO.CreatDogRandom(userDAO.GetUserByID(message.Chat.Id))}", replyMarkup: BotControlButtons.GetButtonMainMenu());
                                    break;
                                case "/reg":
                                    await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.CreateNewUser(userDAO.CreateNewUser(message.Chat.Id, message.Chat.FirstName))}", replyMarkup: BotControlButtons.GetButtonMainMenu());
                                    break;
                                case "Лучшие игроки":
                                    await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.Statistic()}", replyMarkup: BotControlButtons.GetButtonMainMenu());
                                    break;
                                case "Магазин":
                                    await botClient.SendTextMessageAsync(message.Chat, $"Ваши деньги:{Math.Round(userDAO.GetUserByID(message.Chat.Id).money)}", replyMarkup: BotControlButtons.GetButtonShop());
                                    break;

                                default:
                                    await botClient.SendTextMessageAsync(message.Chat, $"Кнопочки ван лав");
                                    break;
                            }

                        }
                        else
                        {
                            switch (message.Chat.Type)
                            {
                                case Telegram.Bot.Types.Enums.ChatType.Private:
                                    break;
                                case Telegram.Bot.Types.Enums.ChatType.Group:
                                    await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.CreateNewUser(userDAO.CreateNewUser(message.Chat.Id, message.From.LastName))}");
                                    await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.GetUserByID(message.Chat.Id).GetAllStatus()}");
                                    break;
                                case Telegram.Bot.Types.Enums.ChatType.Channel:
                                    break;
                                case Telegram.Bot.Types.Enums.ChatType.Supergroup:
                                    break;
                                case Telegram.Bot.Types.Enums.ChatType.Sender:
                                    break;
                                default:
                                    break;
                            }

                            await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.CreateNewUser(userDAO.CreateNewUser(message.Chat.Id, message.Chat.FirstName))}");
                                await botClient.SendTextMessageAsync(message.Chat, $"{userDAO.GetUserByID(message.Chat.Id).GetAllStatus()}");

                        }
                       
                        break;
                    }
                case Telegram.Bot.Types.Enums.UpdateType.InlineQuery:                    
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChosenInlineResult:                    
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.CallbackQuery:
                    {
                        userDAO.UpdateUser(userDAO.GetUserByID((int)update.CallbackQuery.From.Id));
                        var user = userDAO.GetUserByID((int)update.CallbackQuery.From.Id);
                        List<Dog> dogs = dogDAO.DogsShop();

                        //оброботка CallbackQuery
                        switch (update.CallbackQuery.Data)
                        {
                            case "Buy energy in the store":

                                if (user.money >= 1000)
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

                        try
                        {
                            for (int i = 0; i < 10; i++)
                            {

                                if (user.countDog - 1 >= i)
                                {
                                    Dog dog = dogDAO.GetAllDogsUser(user.Id)[i];
                                    if (update.CallbackQuery.Data == $"Eat{i}")
                                    {
                                        if (user.eat >= 10)
                                        {

                                            dog.satiety += 10;
                                            user.eat -= 10;
                                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"ГУв-Гав ням ням \nСытость собаки возросла на 10");
                                            dogDAO.UpdataDog(dog);
                                            userDAO.UpdateUser(user);
                                        }
                                        else
                                        {
                                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Недостаточно еды хозяин :( \nМинимум 10");
                                        }
                                        break;
                                    }
                                    //Прокачка собаки
                                    if (update.CallbackQuery.Data == $"Training{i}")
                                    {
                                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"{dog.Doglvl()}", replyMarkup: BotControlButtons.TrainingDog(i));
                                    }
                                    if (update.CallbackQuery.Data == $"{i}S")
                                    {

                                        if (dog.Agility + dog.Endurance + dog.Intelligence < dog.lvl)
                                        {
                                            dog.UpAgility();
                                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"+ 1 к силе.");
                                            dogDAO.UpdataDog(dog);
                                        }
                                        else
                                        {
                                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Недостаточно свободного уровня.");
                                        }


                                    }//Сила
                                    if (update.CallbackQuery.Data == $"{i}L")
                                    {
                                        if (dog.Agility + dog.Endurance + dog.Intelligence < dog.lvl)
                                        {
                                            dog.UpEndurance();
                                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"+ 1 к Ловкости.");
                                            dogDAO.UpdataDog(dog);
                                        }
                                        else
                                        {
                                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Недостаточно свободного уровня.");
                                        }


                                    }//Ловкость
                                    if (update.CallbackQuery.Data == $"{i}I")
                                    {
                                        if (dog.Agility + dog.Endurance + dog.Intelligence < dog.lvl)
                                        {
                                            dog.UpIntelligence();
                                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"+ 1 к Интелекту.");
                                            dogDAO.UpdataDog(dog);
                                        }
                                        else
                                        {
                                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Недостаточно свободного уровня.");
                                        }



                                    }//Интелект
                                }


                                if (update.CallbackQuery.Data == Convert.ToString(i))
                                {
                                    await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, dogDAO.GetDog(dogDAO.GetAllDogsUser((int)update.CallbackQuery.From.Id)[i].id).DogInfo(), replyMarkup: BotControlButtons.SelectDogStatus($"{i}R"));
                                    break;
                                }
                                if (update.CallbackQuery.Data == $"{i}R")
                                {
                                    await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "Введите имя собаке");
                                    //Добавляем собаку в очередь на переименовку
                                    dogDAO.AddQueue((int)update.CallbackQuery.From.Id, i);
                                    break;
                                }//Редактирование собак
                                if (update.CallbackQuery.Data == $"{i}B")
                                {
                                    if (user.countDog < 10)
                                    {
                                        try
                                        {
                                            if (user.money >= dogs[i].lvl * 200)
                                            {
                                                user.money -= dogs[i].lvl * 200;
                                                dogs[i].UserId = user.Id;

                                                dogDAO.AddDog("dogs", dogs[i]);
                                                dogDAO.DelDog("tdogsauction2", dogs[i].id);
                                                await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Покупка успешна,Теперь у вас есть новый друг {dogs[i].name}");
                                                await botClient.DeleteMessageAsync(update.CallbackQuery.From.Id, update.CallbackQuery.Message.MessageId);
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Покупка не удалась кто то уже купил эту собаку :(");
                                            await botClient.DeleteMessageAsync(update.CallbackQuery.From.Id, update.CallbackQuery.Message.MessageId);
                                            break;
                                        }


                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, "У вас максимум собак");
                                    }
                                    break;
                                }//Покупка из магазина                            
                                if (update.CallbackQuery.Data == $"Ded_dog{i}")
                                {
                                    dogDAO.DelDog("dogs", dogDAO.GetAllDogsUser(user.Id)[i].id);
                                    await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Песик уже не с нами :(");
                                    await botClient.DeleteMessageAsync(update.CallbackQuery.From.Id, update.CallbackQuery.Message.MessageId);
                                    
                                    break;
                                }





                            }
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Устаревшая кнопка удалаю...");
                            await botClient.DeleteMessageAsync(update.CallbackQuery.From.Id, update.CallbackQuery.Message.MessageId);
                        }
                        
                        
                        userDAO.UpdateUser(user);
                        break;
                    }                    
                case Telegram.Bot.Types.Enums.UpdateType.EditedMessage:
                    {
                        var edited_message = update.EditedMessage;
                        await botClient.SendTextMessageAsync(edited_message.Chat, "Опа кто то изменил сообщение извени я такое не понимаю...");
                        break;
                    }
                case Telegram.Bot.Types.Enums.UpdateType.ChannelPost:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.EditedChannelPost:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ShippingQuery:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.PreCheckoutQuery:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.Poll:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.PollAnswer:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.MyChatMember:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChatMember:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChatJoinRequest:
                    break;
                default:
                    break;
            }

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
        }

        public static void ClearServer(ITelegramBotClient bot, Update update)
        {
            bot.GetUpdatesAsync(update.Id+10);
            
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
