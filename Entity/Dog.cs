using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace TelegramDOGs
{
    enum TypeDogs { LabradorRetrievers, GermanShepherds, Bulldogs,Mops, Poodles, Rottweilers, GoldenRetrievers, };
    
    class Dog
    {
        
        Random R = new Random();
        public long UserId { get;  set; }
        public int id { get;  set; }
        public string name { get;  set; }
        public int age { get;  set; }
        public DateTime RegDogUser { get; set; }
        public int HP { get; set; }
        public int lvl { get;  set;}
        public double multiplier { get; set; }
        public int satiety { get;  set; }
        public string TypeDogString { get;  set; }
        public int TypeDog { get;  set; }
        public int Endurance { get;  set; }
        public int Agility { get;  set; }
        public int Intelligence { get;  set; }
        public int Prace { get; set; }

        public Dog(int id, string name, int age, int lvl, TypeDogs type , int satiety )
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.lvl = lvl;
            this.satiety = satiety;
            this.TypeDogString = Convert.ToString(type);
            Updatedog();
            this.Prace = lvl * 200;


        }
        public Dog(int id, string name, int age, int lvl , int satiety)
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.lvl = lvl;
            this.satiety = satiety;            
            SetTypeDogRandom();
            Updatedog();
            this.Prace = lvl * 200;

        }
        public Dog()
        {            
            SetTypeDogRandom();
            Updatedog();
            this.Prace = lvl * 200;
        }
        public string Doglvl()
        {
            string res = "";
            res += $"Имя:{name}";
            res += $"Уровень:{lvl}";
            res += $"Сила:{Endurance}";
            res += $"Ловкость:{Agility}";
            res += $"Интелект:{Intelligence}";
            return res;
        }
        public string DogInfo()
        {
            string info = "";
            info += $"Id:{id}\n";
            info += $"Имя:{name}\n";
            info += $"Порода:{TypeDogString}\n";
            info += $"Возраст:{age}\n";
            info += $"Сытость:{satiety}\n";
            info += $"Здоровье:{HP}\n";
            info += $"Уровень:{lvl}\n";
            info += $"Сила/Ловкость/Интелект:{Endurance}/{Agility}/{Intelligence}\n";
            info += $"Предворительная цена: {Prace}";
            return info;

        }
        public void SetTypeDogRandom()
        {
            TypeDogString = Convert.ToString((TypeDogs)R.Next(0,6));
        }
        public void SetTypeDog(TypeDogs e)
        {
            this.TypeDogString = Convert.ToString(e);
        }
        public int CountFreeLvL()
        {
            return lvl - (Endurance + Agility + Intelligence);
        }
        public void LvlUp(int count)
        {
            if(lvl<=1000)
            {
                this.lvl += count;
            }           
            else
            {
                this.lvl = lvl; 
            }
        }
        public void AgeUp()
        {
            this.age += 1;
        }
        public void UpEndurance()
        {
            if(Endurance+Agility+Intelligence<lvl)
            {
                Endurance += 1;
            }
        }
        public void UpAgility()
        {
            if (Endurance + Agility + Intelligence < lvl)
            {
                Agility += 1;
            }
        }
        public void UpIntelligence()
        {
            if (Endurance + Agility + Intelligence < lvl)
            {
                Intelligence += 1;
                Updatedog();
            }
        }
        public void Updatedog()
        {
            multiplier = 0.001 * Intelligence;
        }

    }
}
