using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using System.Collections.Generic;
namespace TelegramDOGs
{
    class Program
    {
            public static List<USER> userList = new List<USER>();
            static ITelegramBotClient bot = new TelegramBotClient("5384438845:AAG6qrDzwcni1Lk8bBIkXAPCJ2-D7YVG6j0");
            
            public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Некоторые действия
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {

                    var message = update.Message;
                    USER user = new USER(message.Chat.Username, message.Chat.Id);
                    if (message.Text.ToLower() == "/start")
                    {
                        
                        
                        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                        return;
                    }
                   
                    
                    await botClient.SendTextMessageAsync(message.Chat, $" ID:{message.Chat.Id} UserName: {message.Chat.FirstName} text: {message.Text}");
                    

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
           
            public static string AdminMessageControl()
            {
                Console.WriteLine("Веди сообщение лоху");
                string t = Console.ReadLine();
                return t;
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
                
            
                
            while (true)
            {
                ConsoleKeyInfo key;
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.End)
                {
                    Console.WriteLine("AdminMessageActiv");
                    AdminMessage(bot);

                }
            }
               
                Console.ReadLine();


        }
    }
}
