using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace TelegramDOGs
{
    enum TypeDogs { LabradorRetrievers, GermanShepherds };
    
    class Dog
    {
        
        Random R = new Random();
        
        public int id { get; private set; }
        public string name { get; private set; }
        public byte age { get; private set; }
       
        public byte lvl { get; private set;}
        
        public int satiety { get; private set; }
        public string TypeDogString { get; private set; }
        public byte TypeDog { get; private set; }
        public byte Endurance { get; private set; }
        public byte Agility { get; private set; }
        public byte Intelligence { get; private set; }
        public Dog(int id, string name, byte age, byte lvl , int satiety)
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.lvl = lvl;
            this.satiety = satiety;
        }

        public void SetTypeDogRandom()
        {
            TypeDogString = Convert.ToString((TypeDogs)R.Next(0,1));
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

    }
}
