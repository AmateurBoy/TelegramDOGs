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
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Подробнее об игре")});
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
        }
        public static IReplyMarkup GetButtonMyStatus()
        {
            List<List<KeyboardButton>> KeyboardButtonTest = new List<List<KeyboardButton>>();
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Мои Собаки"), new KeyboardButton("Мой Профиль") });
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Главное меню"), new KeyboardButton("Магазин") });
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
        }
        public static IReplyMarkup GetButtonShop()
        {
            List<List<KeyboardButton>> KeyboardButtonTest = new List<List<KeyboardButton>>();
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Купить еды"), new KeyboardButton("Купить єнергию")});
            KeyboardButtonTest.Add(new List<KeyboardButton> { new KeyboardButton("Купить собаку"), new KeyboardButton("Главное меню") });
            var keybord = new ReplyKeyboardMarkup(KeyboardButtonTest);
            keybord.ResizeKeyboard = true;
            return keybord;
        }        
        public static IReplyMarkup GetBuyEnergy()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton("Для покупки") { Text = "Купить", CallbackData= "Buy energy in the store" }) ;
        }
        public static IReplyMarkup GetBuyEat()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton("Для покупки") { Text = "Купить", CallbackData= "Buy eat in the store" }) ;
        }
        public static IReplyMarkup GetBuyDogs(List<Dog> Dogs)
        {
           
             
            List<List<InlineKeyboardButton>> IKB = new List<List<InlineKeyboardButton>>();
            int countRows = 0;

            countRows = Dogs.Count / 2;
            for (int i = 0, j = 0; i < Dogs.Count; i++)
            {

                if (countRows >= i)
                {
                    if (j == Dogs.Count - 1)
                    {
                        IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("Имя собаки") { Text = $"name:{Dogs[j].name} ,lvl:{Dogs[j].lvl}, Prace:{Dogs[j].Prace} ,{Dogs[j].TypeDogString}", CallbackData = $"{j}B" } });
                        break;
                    }
                    if (j >= Dogs.Count)
                    {
                        break;
                    }
                    IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("Имя собаки") { Text = $"name:{Dogs[j].name},lvl:{Dogs[j].lvl}, Prace:{Dogs[j].Prace}, {Dogs[j].TypeDogString}", CallbackData = $"{j}B" }, new InlineKeyboardButton("Имя собаки") { Text = $"name:{Dogs[j+1].name},lvl:{Dogs[j+1].lvl}, Prace:{Dogs[j+1].Prace}, {Dogs[j+1].TypeDogString}", CallbackData = $"{j + 1}B" } });
                    j += 2;
                }


            }
            var keybord = new InlineKeyboardMarkup(IKB);
            
            return keybord;
        }
       
        public static IReplyMarkup GetDogButton(List<Dog>Dogs)
        {
            List<List<InlineKeyboardButton>> IKB = new List<List<InlineKeyboardButton>>();
            int countRows = 0;
           
            countRows = Dogs.Count / 2;
            for (int i = 0,j=0; i < Dogs.Count; i++)
            {
                
                if(countRows>=i)
                { 
                    if(j==Dogs.Count-1)
                    {
                        IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("Имя собаки") { Text = $"{Dogs[j].name}", CallbackData = $"{j}" } });
                        break;
                    }
                    if(j>=Dogs.Count)
                    {
                        break;
                    }
                    IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("Имя собаки") { Text = $"{Dogs[j].name}", CallbackData = $"{ j }" }, new InlineKeyboardButton("Имя собаки") { Text = $"{Dogs[j + 1].name}", CallbackData = $"{j + 1}" } });
                    j += 2;
                }
               
                                
            }
            var keybord = new InlineKeyboardMarkup(IKB);
            return keybord;
           
        }
        public static IReplyMarkup SelectDogStatus(string selectIndexDog)
        {
            string index = selectIndexDog.Remove(1) ;
            List<List<InlineKeyboardButton>> IKB = new List<List<InlineKeyboardButton>>();
            IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("ава") { Text = $"Переименовать Собаку", CallbackData = $"{selectIndexDog}" }, new InlineKeyboardButton("Усыпить :(") { Text = $"Усыпить :(", CallbackData = $"Ded_dog{index}" } });
            IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("віаів") { Text = $"Покормить собаку", CallbackData = $"Eat{index}" }, new InlineKeyboardButton("Продать") { Text = $"Продать(в разработке)", CallbackData = $"13123" } });
            IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("віаів") { Text = $"Воспитывать", CallbackData = $"Training{index}" } });
            var keybord = new InlineKeyboardMarkup(IKB);
            return keybord;
        }
        public static IReplyMarkup TrainingDog(int index)
        {
           
            List<List<InlineKeyboardButton>> IKB = new List<List<InlineKeyboardButton>>();
            IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("ава") { Text = $"Улучшить Силу", CallbackData = $"{index}S" }, new InlineKeyboardButton("Усыпить :(") { Text = $"Улучшить Ловкость", CallbackData = $"{index}L" } });
            IKB.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton("віаів") { Text = $"Улучшить Интелект", CallbackData = $"{index}I" } });
            
            var keybord = new InlineKeyboardMarkup(IKB);
            return keybord;
        }

    }
}
