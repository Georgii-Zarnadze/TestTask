using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    enum Actions //дейсвия
    {
        ActionShot,
        ActionRepair,
        BuyBullets
    }

    class Robot
    {
        public string Name; //название        
        public int Health; //здоровье
        public int Armor; //броня
        private int Damage; //урон
        public int QuantityBullet; //количество патрона
        public int QuantityRepair; //количество починки
        public int Money; //деньги
        private int MaxHp; //максимальное здоровье
        public bool GiveUp; //сдаться

        public Robot(string Name, int Health, int Armor, int Damage, int QuantityBullet, int QuantityRepair, int Money) //конструктор
        {
            //привсоение параметров с помощью конструктора
            //глобальные переменные = переменные конструктора 
            this.Name = Name;
            this.Armor = Armor;
            this.Health = Health;
            this.Damage = Damage;
            this.QuantityBullet = QuantityBullet;
            this.QuantityRepair = QuantityRepair;
            this.Money = Money;
            MaxHp = Health;
            GiveUp = false; //если есть деньги или пуля не сдаваться
        }

        public void BuyBullets()
        {
            if (Money < 50)
            {
                Console.WriteLine("Нет достоточных денег!");
                System.Threading.Thread.Sleep(1250); //Задержка
            }
            else
            {
                int PlusBullet = 0;
                do //Обработка ошибок переменной
                {
                    try
                    {
                        Console.Write("Сколько пулей вы хотите купить? ->");
                        PlusBullet = Convert.ToInt16(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        PlusBullet = 0;
                    }
                } while (PlusBullet < 0);

                if (PlusBullet > (Money / 50))
                {
                    QuantityBullet += (Money / 50);
                    Money %= 50;
                }
                else
                {
                    Money -= (PlusBullet * 50);
                    QuantityBullet += PlusBullet;
                }
            }
        }
        public void BuyBulletsForComp() //метод покупки для компьютера
        {
            int AddBullets = Money / 50;
            Money %= 50;
            QuantityBullet += AddBullets;
            Console.WriteLine("Противник купил " + AddBullets + " пулей");
            System.Threading.Thread.Sleep(3250); //Задержка

        }

        public int Shot() //выстрел 
        {
            if (QuantityBullet == 0)
            {
                Console.WriteLine("У Вас нет пуля нажмите Enter, чтобы купить");
                Console.ReadKey();
                BuyBullets();
                return 0;
            }
            else
            {
                QuantityBullet--;
                Random RandomNumber = new Random();
                int RandN = RandomNumber.Next(100);
                if (RandN < 10) //вероятность=0.1
                {
                    Console.WriteLine("Критический выстрел!");
                    System.Threading.Thread.Sleep(2150); //Задержка
                    int Damage20 = Damage * 6 / 5;
                    return Damage20;
                }
                else if (RandN < 30 && RandN >= 10) //вероятность=0.2
                {
                    Console.WriteLine("Промах!");
                    System.Threading.Thread.Sleep(1250); //Задержка
                    return 0;
                }
                else
                {
                    Console.WriteLine("Попал!"); // вероятность=0.7
                    System.Threading.Thread.Sleep(1250); //Задержка
                    return Damage;
                }
            }
        }

        public void Repair() //починка 
        {
            if (QuantityRepair > 0)
            {
                Console.WriteLine("Починка робота " + Name);
                System.Threading.Thread.Sleep(1250); //Задержка
                QuantityRepair--;
                if ((Health + 50) > MaxHp) Health = MaxHp; //нельзя прибавить уровень здоровья больше максимального
                else Health += 50; //прибавляем уровень здоровья
            }
            else Console.WriteLine("Невозможно починить бельше!\n Количество починки закончились!");
        }


        public void Info() //информация  
        {
            Console.Write("Name: " + Name);
            Console.Write(", HP=" + Health + ", Armor=" + Armor + ", Damage=" + Damage);
            Console.WriteLine(", Bullets=" + QuantityBullet + ", $=" + Money + ", Repair=" + QuantityRepair);

        }
    }




    class Program
    {
        static int CheckedValue(string Hint)
        {
            int X = 0;
            do //Обработка ошибок переменной
            {
                try
                {
                    Console.Write(Hint);
                    X = Convert.ToInt16(Console.ReadLine());
                }
                catch (Exception)
                {
                    X = 0;
                }
            } while (X <= 0);
            Console.Clear();
            return X;
        }

        static void Action(Actions A, Robot Rob1, Robot Rob2) //действие 
        {

            switch (A)
            {
                case Actions.ActionShot:
                    {
                        int Damage = Rob1.Shot();
                        if (Rob2.Armor >= Damage) Rob2.Armor -= Damage;
                        else if (Rob2.Armor == 0) Rob2.Health -= Damage;
                        else if (Rob2.Armor < Damage)
                        {
                            int MinusHp = Damage - Rob2.Armor;
                            Rob2.Armor = 0;
                            Rob2.Health -= MinusHp;
                        }

                    } break;
                case Actions.ActionRepair:
                    {
                        Rob1.Repair();
                    } break;
                case Actions.BuyBullets:
                    {
                        Rob1.BuyBullets();
                    } break;
            }
        }


        static void Main(string[] args)
        {
            Robot Robot1, Robot2;

            //имя, здоровье, броня, урон, патроны, починка, деньги 
            Robot1 = new Robot("User", 100, 100, 10, 2, 3, 100);
            Robot2 = new Robot("Comp", 100, 100, 10, 2, 3, 104);


            string WinnersName = " "; //имя победителя            
            bool GameOver = false; //игра не окончена
            bool FirstPlays = true; //если истина, то действует первый робот, иначе - второй робот.            
            while (!GameOver)
            {

                Console.Clear(); //очистка консоли
                Robot1.Info();
                Robot2.Info();
                Console.WriteLine("Введите соответствующую цифру команды:\n1 - Огонь\n2 - Починка\n3 - Покупка пулей");
                Console.Write("Дествует робот ");
                if (FirstPlays) //робот - пользователь
                {
                    Console.WriteLine(Robot1.Name);
                    byte Command = 0;
                    do
                    {
                        Command = (byte)CheckedValue("Команда -> ");
                    } while (Command > 3);

                    if (Command == 1 && Robot1.QuantityBullet != 0)
                    {
                        Action(Actions.ActionShot, Robot1, Robot2);
                        FirstPlays = false;
                    }
                    else if (Command == 1 && Robot1.QuantityBullet == 0)
                    {
                        Console.WriteLine("Патроны закончились надо купить!");
                        System.Threading.Thread.Sleep(250); //Задержка
                        Action(Actions.BuyBullets, Robot1, Robot2);
                        if (Robot1.QuantityBullet > 0)
                        {
                            Console.WriteLine("Нажмите Enter, чтобы выстрелить!");
                            Console.ReadKey();
                            Action(Actions.ActionShot, Robot1, Robot2);
                            FirstPlays = false;
                        }
                        else Console.WriteLine("У вас нет ни денег и ни патронов!");
                    }
                    else if (Command == 2) Action(Actions.ActionRepair, Robot1, Robot2);
                    else Action(Actions.BuyBullets, Robot1, Robot2);


                    if (Robot2.Health <= 0)
                    {
                        WinnersName = Robot1.Name;
                        GameOver = true;
                    }
                    if (Robot1.Money < 50 && Robot1.QuantityBullet == 0) Robot1.GiveUp = true;
                }
                else //робот - компьютер
                {
                    Console.WriteLine(Robot2.Name + "\n");


                    //ИИ робота
                    if (Robot2.Health < 50 && Robot2.QuantityRepair > 0) Action(Actions.ActionRepair, Robot2, Robot1);
                    if (Robot2.QuantityBullet == 0) Robot2.BuyBulletsForComp();
                    if (Robot2.QuantityBullet > 0) Action(Actions.ActionShot, Robot2, Robot1);
                    else Console.WriteLine("Противник пропускает ход!");
                    System.Threading.Thread.Sleep(3350); //Задержка

                    FirstPlays = true;



                    if (Robot1.Health <= 0)
                    {
                        WinnersName = Robot2.Name;
                        GameOver = true;
                    }
                    if (Robot2.Money < 50 && Robot2.QuantityBullet == 0) Robot2.GiveUp = true;
                }

                if (Robot2.GiveUp == Robot1.GiveUp && Robot2.GiveUp == true)
                {
                    if ((Robot2.Armor + Robot2.Health) == (Robot1.Health + Robot1.Armor)) WinnersName = "Nobody";
                    else if ((Robot2.Armor + Robot2.Health) > (Robot1.Health + Robot1.Armor)) WinnersName = Robot2.Name;
                    else if ((Robot2.Armor + Robot2.Health) < (Robot1.Health + Robot1.Armor)) WinnersName = Robot1.Name;
                    GameOver = true;
                }
            }

            Console.Clear();
            Console.WriteLine("Game over\nWinner is " + WinnersName);


            Console.ReadKey();
        }
    }
}
