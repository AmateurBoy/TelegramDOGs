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
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("/finddog"), new KeyboardButton("/status") });
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Купить собаку"), new KeyboardButton("Бой") });
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
        }
        public static IReplyMarkup GetButton()
        {
            List<List<KeyboardButton>> KeyboardButtonTest = new List<List<KeyboardButton>>();
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("/finddog"), new KeyboardButton("/status") });
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Купить собаку"), new KeyboardButton("Бой") });
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
        }
    }
}
