using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramDOGs.DAO;

namespace TelegramDOGs.Service
{
    class TelegramService
    {
        private static TelegramService Instens;
        static ITelegramBotClient bot = new TelegramBotClient("5384438845:AAG6qrDzwcni1Lk8bBIkXAPCJ2-D7YVG6j0");
        private TelegramService()
        {

        }

        public TelegramService GetInstens()
        {
            if(Instens==null)
            {
                Instens = new TelegramService();
            }
            return Instens;
        }

       

    }
}
