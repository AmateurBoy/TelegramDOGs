using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramDOGs
{
    class USER
    {
        public USER(string Name,long ID)
        {
            UserName = Name;
            this.ID = ID;
        }
        private string UserName { get; set; }
        private long ID { get; set; }
        public string GetUserName()
        {
            return UserName;
        }
        public string GetID()
        {
            return Convert.ToString(ID);
        }
    }
}
