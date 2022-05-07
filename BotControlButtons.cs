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
        public static IReplyMarkup GetButtonShop()
        {
            List<List<KeyboardButton>> KeyboardButtonTest = new List<List<KeyboardButton>>();
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Купить еды"), new KeyboardButton("Купить енергию")});
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Купить собаку"), new KeyboardButton("Главное меню") });
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
        }
    }
}
