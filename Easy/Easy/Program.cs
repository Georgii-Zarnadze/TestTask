using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    enum Actions //дейсвия
    {
        ActionShot,
        ActionRepair
    }

    class Robot
    {
        private string Name; //название        
        public int Health; //здоровье
        public int Armor; //броня
        private int Damage; //урон

        public Robot(string Name, int Health, int Armor, int Damage) //конструктор
        {
            //привсоение параметров с помощью конструктора
            //глобальные переменные = переменные конструктора 
            this.Name = Name;
            this.Armor = Armor;
            this.Health = Health;
            this.Damage = Damage;
        }

        public void Repair(int PlusArmor) //починка 
        {
            if (PlusArmor > 50) Armor += 50; //нельзя прибавить уровень броня больше 50
            else Armor += PlusArmor; //прибавляем уровень броня
        }

        public int Shot() //выстрел 
        {
            return Damage;
        }

        public void Info() //информация  
        {
            Console.Write("Name: " + Name);
            Console.Write(", hp=" + Health + ", armor=" + Armor);
            Console.Write(", damage=" + Damage + '\n');

        }
    }




    class Program
    {
        static int CheckedValue(string Hint)
        {
            //аргумент - подсказка(Hint)
            //возвращает целое число без ощибки(X)            
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
            //аргумент - действие(A), 2 объекта(Rob1, Rob2)
            //не возвращает значения


            switch (A)
            {
                case Actions.ActionShot:
                    {
                        if (Rob2.Armor >= Rob1.Shot()) Rob2.Armor -= Rob1.Shot();
                        else if (Rob2.Armor == 0) Rob2.Health -= Rob1.Shot();
                        else if (Rob2.Armor < Rob1.Shot())
                        {
                            int MinusHp = Rob1.Shot() - Rob2.Armor;
                            Rob2.Armor = 0;
                            Rob2.Health -= MinusHp;
                        }

                    } break;
                case Actions.ActionRepair:
                    {
                        int PlusArm = CheckedValue("Введите уровень броня которую хотите прибавить(0-50).\n-> ");
                        Rob1.Repair(PlusArm);
                    } break;
            }
        }


        static void Main(string[] args)
        {
            Robot Robot1, Robot2;

            //названия роботов
            Console.Write("Введите название первого робота: ");
            string RobName1 = Console.ReadLine();
            Console.Write("Введите название второго робота: ");
            string RobName2 = Console.ReadLine();
            Console.Clear();

            //параметры роботов
            int H1, A1, D1;
            int H2, A2, D2;
            H1 = CheckedValue("Введите уровень здоровья первого игрока: ");
            A1 = CheckedValue("Введите уровень броня первого игрока: ");
            D1 = CheckedValue("Введите урон от первого игрока: ");
            H2 = CheckedValue("Введите уровень здоровья второго игрока: ");
            A2 = CheckedValue("Введите уровень броня второго игрока: ");
            D2 = CheckedValue("Введите урон от второго игрока: ");

            Robot1 = new Robot(RobName1, H1, A1, D1);
            Robot2 = new Robot(RobName2, H2, A2, D2);


            string WinnersName = " "; //имя победителя            
            bool GameOver = false; //Игра не окончена
            bool FirstPlays = true; //если истина, то действует первый робот, иначе - второй робот.            
            while (!GameOver)
            {
                Console.Clear();
                Robot1.Info();
                Robot2.Info();
                Console.WriteLine("Введите соответствующую цифру команды:\n1 - Выстрел\n2 - Починка");
                Console.Write("Дествует робот ");
                if (FirstPlays) //первый робот 
                {
                    Console.WriteLine(RobName1);
                    byte Command = 0;
                    do
                    {
                        Command = (byte)CheckedValue("Команда -> ");
                    } while (Command > 2);

                    if (Command == 1) Action(Actions.ActionShot, Robot1, Robot2);
                    else Action(Actions.ActionRepair, Robot1, Robot2);

                    if (Robot2.Health <= 0)
                    {
                        WinnersName = RobName1;
                        GameOver = true;
                    }
                    FirstPlays = false;
                }
                else //второй робот
                {
                    Console.WriteLine(RobName2);
                    byte command = 0;
                    do
                    {
                        command = (byte)CheckedValue("Команда -> ");
                    } while (command > 2);

                    if (command == 1) Action(Actions.ActionShot, Robot2, Robot1);
                    else Action(Actions.ActionRepair, Robot2, Robot1);

                    if (Robot1.Health <= 0)
                    {
                        WinnersName = RobName2;
                        GameOver = true;
                    }
                    FirstPlays = true;
                }
            }

            Console.Clear();
            Console.WriteLine("Game over\nWinner is " + WinnersName);


            Console.ReadKey();
        }
    }
}
