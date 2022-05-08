using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramDOGs
{
    static class BotControlButtons
    {
        public static IReplyMarkup GetButtonMainMenu()
        {
            List<List<KeyboardButton>> KeyboardButtonTest = new List<List<KeyboardButton>>();
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Найти собаку"), new KeyboardButton("Мой Профиль")});
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Магазин"), new KeyboardButton("Лучшие игроки")});
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
        }
        public static IReplyMarkup GetButtonMyStatus()
        {
            List<List<KeyboardButton>> KeyboardButtonTest = new List<List<KeyboardButton>>();
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Мои Собаки"), new KeyboardButton("Мой профиль") });
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Главное меню"), new KeyboardButton("Магазин") });
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
        }
        public static IReplyMarkup GetButtonShop()
        {
            List<List<KeyboardButton>> KeyboardButtonTest = new List<List<KeyboardButton>>();
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Купить еды"), new KeyboardButton("Купить енергию")});
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Купить собаку"), new KeyboardButton("Главное меню") });
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
        }        
        public static IReplyMarkup GetBuy()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton("Для покупки") { Text = "Купить",CallbackData="2"}) ;
        }
        public static IReplyMarkup GetDogButton(List<Dog>Dogs)
        {
            List<List<InlineKeyboardButton>> IKB = new List<List<InlineKeyboardButton>>();
            int countRows = 0;
            countRows = Dogs.Count / 2;
            for (int i = 0; i < Dogs.Count; i++)
            {
                
                if(countRows>=i)
                {                    
                    IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("Имя собаки") { Text = $"{Dogs[i].name}", CallbackData = $"{ i }" } ,new InlineKeyboardButton("Имя собаки") { Text = $"{Dogs[i+1].name}", CallbackData = $"{i+1}" } });
                }
                else
                {
                    IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("Имя собаки") { Text = $"{Dogs[i].name}", CallbackData = $"{i}" } });
                    break;
                }
                                
            }
            var keybord = new InlineKeyboardMarkup(IKB);
            return keybord;
           
        }

    }
}
