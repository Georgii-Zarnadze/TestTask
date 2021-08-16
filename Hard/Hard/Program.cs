using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    enum Actions //дейсвия
    {
        ActionShot = 1,
        ActionRepair,
        BuyBullets
    }

    interface IRobot
    {
        //Свойства
        bool GiveUp { get; }
        bool MyMove { get; }
        string RobotName { get; }
        int ArmorAndHP { get; }
        int HP { get; }

        //методы
        void BuyBullets(int X);
        void Shot(Robot robot);
        void Repair();
        int GenerateDamageNumber();
        void Info();
    }

    interface IMyRobot : IRobot
    {
        void Command(Robot robot1, Robot robot2);
    }

    interface ICompRobot : IRobot
    {
        void CompCommand(Robot robot1, Robot robot2);
    }


    class Robot : IMyRobot, ICompRobot
    {
        //поля
        public string Name; //название        
        public int Health; //здоровье
        public int Armor; //броня
        private int Damage; //урон при попадании не критического выстрела
        public int QuantityBullet; //количество патрона
        public int QuantityRepair; //количество починки
        public int Money; //деньги
        private int MaxHp; //максимальное здоровье
        public bool MyMoveField; //ход робота
        public bool GiveUpField; //сдаться

        //свойства
        public bool GiveUp
        {
            get { return GiveUpField; }
        }

        public bool MyMove
        {
            get { return MyMoveField; }
        }

        public string RobotName
        {
            get { return Name; }
        }

        public int ArmorAndHP
        {
            get { return (Armor + Health); }
        }

        public int HP
        {
            get { return Health; }
        }

        //конструкторы
        public Robot(string Name, int Health, int Armor, int Damage, int QuantityBullet, int QuantityRepair, int Money)
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
            GiveUpField = false; //если есть деньги или пуля не сдаваться
        }

        //методы
        private int CheckedValue(string Hint, int MinN, int MaxN)
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
            } while (X < MinN || X > MaxN);
            Console.Clear();
            return X;
        }


        // реализация интерфейса


        #region Члены IRobot

        public void BuyBullets(int X)
        {
            if (X * 50 > Money)
            {
                X = Money / 50;
                Money %= 50;
            }
            else Money -= 50 * X;
            QuantityBullet += X;
            MyMoveField = true;
        }

        public void Shot(Robot RobotEnemy)
        {
            if (QuantityBullet == 0)
            {
                Console.WriteLine("У Вас нет пуля, надо купить.\nНажмите Enter, чтобы купить ->");
                Console.ReadKey();
                BuyBullets(1);
                System.Threading.Thread.Sleep(350); //Задержка
                MyMoveField = true;
            }
            else
            {
                QuantityBullet--;
                int GenRandN = GenerateDamageNumber();

                if (RobotEnemy.Armor >= GenRandN) RobotEnemy.Armor -= GenRandN;
                else if (RobotEnemy.Armor == 0) RobotEnemy.Health -= GenRandN;
                else
                {
                    int MinusHP = GenRandN - RobotEnemy.Armor;
                    RobotEnemy.Armor = 0;
                    RobotEnemy.Health -= MinusHP;
                }

                MyMoveField = false;
            }
        }

        public void Repair()
        {
            if (QuantityRepair == 0)
            {
                Console.WriteLine("Вы не можете починить");
                System.Threading.Thread.Sleep(1250); //Задержка
            }
            else
            {
                QuantityRepair--;
                Console.WriteLine("Починка робота " + Name + ".");
                System.Threading.Thread.Sleep(950); //Задержка
                if (Health + 50 >= MaxHp) Health = MaxHp;
                else Health += 50;
            }
            MyMoveField = true;
        }

        public int GenerateDamageNumber()
        {
            Random RandomNumber = new Random();
            int RandN = RandomNumber.Next(100);
            if (RandN < 10)
            {
                Console.WriteLine("Критический выстрел!");
                System.Threading.Thread.Sleep(2150); //Задержка
                return (Damage * 6 / 5);
            }
            else if (RandN < 30 && RandN >= 10)
            {
                Console.WriteLine("Промах!");
                System.Threading.Thread.Sleep(1520); //Задержка
                return 0;
            }
            else
            {
                Console.WriteLine("Попал!");
                System.Threading.Thread.Sleep(1520); //Задержка
                return Damage;
            }

        }

        public void Info()
        {
            Console.Write("Name: " + Name);
            Console.Write(", HP=" + Health + ", Armor=" + Armor + ", Damage=" + Damage);
            Console.WriteLine(", Bullets=" + QuantityBullet + ", $=" + Money + ", Repair=" + QuantityRepair);
        }

        #endregion

        #region Члены IMyRobot

        public void Command(Robot robot1, Robot robot2)
        {
            if (QuantityBullet == 0 && Money < 50)
            {
                Console.WriteLine("Сдается робот " + Name);
                System.Threading.Thread.Sleep(1350); //Задержка
                GiveUpField = true;
                return;
            }
            Console.WriteLine("1 - Огонь\n2 - Починка\n3 - Купить патроны");
            Console.WriteLine("Ваш ход!");
            System.Threading.Thread.Sleep(350); //Задержка
            Actions C = (Actions)CheckedValue("Введите команду ->", 1, 3);
            switch (C)
            {
                case Actions.ActionRepair: Repair(); break;
                case Actions.ActionShot: Shot(robot2); break;
                case Actions.BuyBullets:
                    {
                        int X = CheckedValue("Сколько пулей хотите купить? ->", 0, 9999);
                        BuyBullets(X);
                    }; break;
            }
        }

        #endregion

        #region Члены ICompRobot

        public void CompCommand(Robot robot1, Robot robot2)
        {
            Console.WriteLine("Ход Компютера!");
            System.Threading.Thread.Sleep(1111); //Задержка
            if (QuantityBullet == 1 && Money < 50 && Health < MaxHp) Repair();

            if (QuantityBullet == 0 && Money < 50)
            {
                Console.WriteLine("Сдается робот " + Name + ".");
                System.Threading.Thread.Sleep(1350); //Задержка
                GiveUpField = true;
            }
            else
            {
                if (Health < 50 && QuantityRepair > 0)
                {
                    Repair();
                    Console.WriteLine("Противник починил робот.");
                    System.Threading.Thread.Sleep(1350); //Задержка
                }
                if (QuantityBullet == 0)
                {
                    int X = Money / 50;
                    Money %= 50;
                    QuantityBullet += X;
                    Console.WriteLine("Противник купил " + X + " пулей.");
                    System.Threading.Thread.Sleep(1350); //Задержка
                }
                Console.WriteLine("Противник стреляет...");
                System.Threading.Thread.Sleep(1111); //Задержка
                Shot(robot2);
            }
        }

        #endregion
    }





    class Program
    {

        static void Main(string[] args)
        {
            bool GameOver = false;

            //имя, здоровье, броня, урон, патроны, починка, деньги 
            IMyRobot MRobot = new Robot("UserRobot", 100, 100, 10, 5, 0, 301); //робот пользователя
            ICompRobot CRobot = new Robot("Computer", 100, 100, 10, 5, 0, 301); //робот-компьютер    

            while (!GameOver)
            {
                Console.Clear();
                MRobot.Info();
                CRobot.Info();
                do
                {
                    MRobot.Command(MRobot as Robot, CRobot as Robot);
                } while (MRobot.MyMove);

                if (CRobot.HP > 0)
                {
                    Console.Clear();
                    MRobot.Info();
                    CRobot.Info();
                    CRobot.CompCommand(CRobot as Robot, MRobot as Robot);
                    System.Threading.Thread.Sleep(1354); //Задержка
                }


                //Закончить игру при следующих условиях
                if (CRobot.HP < 1) //Если противник умер
                {
                    Console.Clear();
                    Console.WriteLine("Робот " + CRobot.RobotName + " умер!\nВы выиграли!");
                    GameOver = true;
                }
                if (MRobot.HP < 1)
                {
                    Console.Clear();
                    Console.WriteLine("Робот " + MRobot.RobotName + " умер!\nВы проиграли!");
                    GameOver = true;
                }

                if (MRobot.GiveUp == CRobot.GiveUp && CRobot.GiveUp == true)
                {
                    if (MRobot.ArmorAndHP == CRobot.ArmorAndHP)
                    {
                        Console.Clear();
                        Console.WriteLine("No Winner!");
                    }
                    else if (MRobot.ArmorAndHP > CRobot.ArmorAndHP)
                    {
                        Console.Clear();
                        Console.WriteLine("Winner is " + MRobot.RobotName + ".");
                        Console.WriteLine("Вы выиграли!");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Winner is " + CRobot.RobotName + ".");
                        Console.WriteLine("Вы проиграли!");
                    }
                    GameOver = true;
                }
            }



            Console.ReadKey();
        }
    }
}
