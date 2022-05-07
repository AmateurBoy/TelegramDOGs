using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramDOGs.Entity
{
    class User
    {
        public User()
        {
                       
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double money { get; set; }
        public int EnergyUser { get; set; }
        public int eat { get; set; }
        public int rating { get; set; }
        public DateTime DataUpdate { get; set; }
        public int countDog { get; set; }
        public List<Dog> Dogs { get; set; }
        
        public string GetAllStatus()
        {
            countDog = Dogs.Count;
            string result="";
            int count = 0;
            result += $"🆔 Ваш ID:{Convert.ToString(this.Id)}\n";
            result += $"👶 Ваше имя:{Convert.ToString(this.Name)}\n";
            result += $"💸 Ваши деньги:{Convert.ToString(this.money)}\n";
            result += $"⚡ Ваша енергия:{Convert.ToString(this.EnergyUser)}\n";
            result += $"🍎 Ваш запас еды:{Convert.ToString(this.eat)}\n";
            result += $"📈 Ваш рейтинг:{Convert.ToString(this.rating)}\n";
            result += $"🐶 Всего собак:{Convert.ToString(this.countDog)}\n";
            result += $"🐶 Ваши собаки: ";
            if(Dogs!=null)
            {
                foreach (var item in Dogs)
                {
                    count++;
                    if(count>10)
                    {
                        result += "\n и остальные...";
                        break;
                    }
                    else
                    {
                        result += $"{item.name}, ";
                    }
                    
                }
            }
             
            return result;
        }
    }
}
