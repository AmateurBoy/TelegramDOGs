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
        public float age { get;  set; }
        public string agestring { get; set; }
        public double hungricoificeent { get; set; }
        public uint Dead = 3155760000;
        public double LifeTime { get; set; }
        public DateTime RegDogUser { get; set; }
        public int HP { get; set; }
        public int lvl { get;  set;}
        public double multiplier { get; set; }
        public double satiety { get;  set; }
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
            this.LifeTime = 3110400000;

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
            res += $"Имя: {name}\n";
            res += $"Уровень: {lvl}\n";
            res += $"Сила: {Endurance}\n";
            res += $"Ловкость: {Agility}\n";
            res += $"Интелект: {Intelligence}\n";
            return res;
        }
        public string DogInfo()
        {
            Updatedog();
            string info = "";
            info += $"💡 Id:{id}\n";
            info += $"🐶 Имя:{name}\n";
            info += $"🐕🐩🐕‍Порода:{TypeDogString}\n";
            info += $"⏰ Возраст:{TimeLifeDoginfo(age)}\n";
            info += $"🍽 Сытость:{satiety}\n";
            info += $"🫀 Здоровье:{HP}\n";
            info += $"📈 Уровень:{lvl}\n";
            info += $"🦾 Сила/Ловкость/Интелект:{Endurance}/{Agility}/{Intelligence}\n";
            info += $"Пасивный доход с собаки: {multiplier}/sek";
            info += $"🌝 Предворительная цена: {Prace}";
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
        public bool isDead()
        {
            double resultTotalsek = DateTime.Now.Subtract(RegDogUser).TotalSeconds;
            Updatedog();

            LifeTime = age * 12 * 60 * 24 * 30 * 12;
            LifeTime += 100 * resultTotalsek;
            if (LifeTime >= Dead)
            {
                return true;
            }
            if(isHungru(resultTotalsek))
            {
                return true;
            }
           LifeTime -= age * 12 * 60 * 24 * 30 * 12;
           age += Convert.ToSingle(LifeTime = (((((LifeTime/60)/60)/24)/30)/12));
           return false;
        }
        public bool isHungru(double result)
        {
            satiety -= (0.00289-hungricoificeent)*result;
            if(satiety>0)
            {
                return false;
            }
            return true;
        }
        public string ConvertSecondDataTime(double second)
        {

            double sec = second;
            
            double minuts = 0;
            
            double horse = 0;
            
            double dey = 0;
            
            double month = 0;
            
            double year = 0;
            
            
            for (int i = 0;(int)second>0; i++)
            {
                sec = second;
                if(i==60)
                {
                    i = 0;
                    second -=60;                    
                    minuts += 1;
                    if(minuts==60)
                    {
                        i = 0;
                        minuts -= 60;
                        horse += 1;
                        if(horse==24)
                        {
                            i = 0;
                            horse -= 24;
                            dey += 1;
                            if (dey == 30)
                            {
                                i = 0;
                                dey -= 30;
                                month += 1;
                                if(month==12)
                                {
                                    month -= 12;
                                    i = 0;
                                    year += 1;
                                }
                            }
                        }
                    }
                }
            }
            

            string res = $"Лет:{year} Месяцев:{month} Дней:{dey} Часов:{horse} Минут:{minuts} Секунд:{sec}";
            return res;

        }
        public string TimeLifeDoginfo(double agesekond)
        {
            double test = agesekond * 60 * 60 * 24 * 30 * 12;

            DateTime nullDate =new DateTime();
            
           

            string info = "";
            info += $"{ConvertSecondDataTime(test)}";
            return info;

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
            hungricoificeent = 0.0001 * Agility;
        }
       

    }
}
