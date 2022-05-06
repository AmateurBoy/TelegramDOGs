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
        public int UserId { get;  set; }
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
        
        public Dog(int id, string name, int age, int lvl , int satiety)
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.lvl = lvl;
            this.satiety = satiety;
            this.Intelligence = 50;
            SetTypeDogRandom();
            Updatedog();
            
            
        }
        public Dog()
        {
            this.Intelligence = 50;
            SetTypeDogRandom();
            Updatedog();
            
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
        public void LvlUp()
        {
            if(lvl<=50)
            {
                this.lvl += 1;
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
            }
        }
        public void Updatedog()
        {
            multiplier = 0.001 * Intelligence;
        }

    }
}
