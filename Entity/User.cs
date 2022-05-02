using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramDOGs.Entity
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Dog> Dogs { get; set; }

    }
}
