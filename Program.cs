using System;
using System.Collections.Generic;
using System.Linq;



namespace BusTime
{
    class Program
    {
        static void Main(string[] args)
        {
            // создаем список автобусов, для дальнейшего заполнения и 
            // передачи его в марщрутный лист
            List<BusStop> busStops = new List<BusStop>();
            BusStop busStop;
            // создаем 10 остановок с номерами от 1 до 10
            for (int i = 1; i < 11; i++)
            {
                busStop = new BusStop(i);
                busStops.Add(busStop);
            
            }
           
            // создаем маршрутный лист(карту)
            Maps maps = new Maps(busStops);
            // демонстрируем его содержимое, чтобы убедиться, что создание остановок прошло успешно
            maps.show();
            Console.WriteLine();
            // создаем объект класса Bus и передаем в него маршрутный лист
            Bus bus = new Bus(maps);
            // запускаем имитацию одного полного рейса автобуса
            bus.Route();
            
            Console.ReadLine();






        }


    }

    // класс человек, который имеет только пункт назначения
    public class People 
    {
        public int DesiredBusStop;


        public People(int nameBusStop)
        {
            DesiredBusStop = nameBusStop;
        }


    }

    // класс остановка - содержит в себе название(номер остановки), два списка, которые представляют 
    //собой две очереди в одну и в другую сторону движения
    public class BusStop
    {
        public int NameBusStop;
        public List<People> TrueWay;
        public List<People> FalseWay;
        // конструктор, который получает уже готовую информацию об очередях и номере остановки
        public BusStop(int NameBusStop, List<People> TrueWay, List<People> FalseWay)
        {
            this.NameBusStop = NameBusStop;
            this.TrueWay = TrueWay;
            this.FalseWay = FalseWay;

        }
        // конструктор, который имеет только номер остановки
        public BusStop(int NameBusStop)
        {
            this.NameBusStop = NameBusStop;
            TrueWay = new List<People>();
            FalseWay = new List<People>();
            
            GeneratePeople();

        }
        // метод для генерации людей на остановке, по умолчанию создает 15 объектов
        public void GeneratePeople(int maxCountGeneratePeople = 15)
        {   // в данную переменную заносится номер остановки, куда держит путь человек
            int numBusStop;
            People people;
            Random randomNum = new Random();
            // в цикле случайным образом генерируем остановку, куда держит путь человек
            // исключая ту, на которой он находится в данный момент
            for (int i = 0; i < maxCountGeneratePeople; i++)
            {
                do
                {

                    numBusStop = randomNum.Next(1, 10);

                } while (numBusStop == NameBusStop);
                
                people = new People(numBusStop);
                // распределяем людей в очереди в соответствующую сторону
                if (people.DesiredBusStop < NameBusStop)
                    FalseWay.Add(people);
                else
                    TrueWay.Add(people);
            }

        }
        

    }
    // класс карта - маршрутный лист содержит только список остановок
    public class Maps
    {
        public List<BusStop> BusStop;
        public Maps(List<BusStop> ListStations)
        {
            BusStop = ListStations;
            
        }
        // вспомогательный метод для демонстрации заполненности маршрутных веток
        public void show()
        {
            foreach (var a in BusStop)
            {
                Console.WriteLine("На остановке " + a.NameBusStop + " стоит в одну сторону " + a.TrueWay.Count + " и в другую сторону " 
                    + a.FalseWay.Count + " человек");
            }
        
        }

    }
    public class Bus
    {   // максимальная вместимость автобуса
        public const int Сapacity = 30;
        // количество людей в автобусе в конкретный момент поездки
        public int CountOfPeople;
        // список пассажиров автобуса
        public List<People> Peoples;
        // маршрут автобуса
        public Maps Maps;
        // переменная, которая хранит bool переменную, о том в каком направление едет автобус
        public bool Way;
        // конструктор класса автобус, который принимает единственный параметр маршрутный лист
        public Bus(Maps maps) 
        {
            Peoples = new List<People>();
            Maps = maps; 
        }
        public void Route()
        {   // номера остановок на маршруте
            int countBusStop = 1;
            // устанавливаем переменную в значение True, что означает - автобус выходит на маршрут
            Way = true;
            //  в этом цикле мы по очереди перебираем остановки из маршрутного листа, имитируя передвижение между ними
            foreach (BusStop busStop in Maps.BusStop)
            {   // в каждой части условного оператора реализованы три основные точки для автобуса: стартовая точка, конечная и путь между ними
                if (countBusStop == 1)
                {
                    Console.WriteLine("Автобус стартует с остановки: " + busStop.NameBusStop);
                    LoadingPeople(busStop);
                    countBusStop++;

                }
                else if (countBusStop == Maps.BusStop.Count)
                {
                    Console.WriteLine("Автобус прибыл на конечную остановку: " + busStop.NameBusStop);
                    DropPeople();
                    Way = false;
                    

                    
                }
                else
                {
                    Console.WriteLine("Автобус прибыл на остановку: " + busStop.NameBusStop);
                    DropPeople(busStop);
                    LoadingPeople(busStop);
                    countBusStop++;

                }
                busStop.GeneratePeople(3);

            }
            // разворачиваем список остановок
            // чтобы не создавать еще один лист с остановками, т.к. они дублируются в обе стороны
            Maps.BusStop.Reverse();

            foreach (BusStop busStop in Maps.BusStop)
            {
                if (countBusStop == Maps.BusStop.Count)
                {
                    Console.WriteLine();
                    Console.WriteLine("Автобус стартует с остановки: " + busStop.NameBusStop);
                    LoadingPeople(busStop);
                    countBusStop--;

                }
                else if (countBusStop == 1)
                {
                    Console.WriteLine("Автобус прибыл на конечную остановку " + busStop.NameBusStop +", рейс завершен.");
                    DropPeople();
                    Way = false;
                    
                }
                else
                {
                    
                    Console.WriteLine("Автобус прибыл на остановку: " + busStop.NameBusStop);
                    DropPeople(busStop);
                    LoadingPeople(busStop);
                    countBusStop--;
                }
                // после каждого переезда добавляем людей в очереди
                busStop.GeneratePeople(3);
            }
            // производим разворот списка в очередной раз для дальнейшей исправной работы
            Maps.BusStop.Reverse();


        }

        // метод для высадки пассажиров на их остановке
        public void DropPeople(BusStop busStop)
        {
            // в данной переменной хранится кол-во вышедших из автобуса людей
            short countDropPeople = 0;
            // проходим циклом по списку пассажиров, если пассажир ехал до этой остановки, высаживаем его
            foreach (var people in Peoples.ToList())
            {
                if (busStop.NameBusStop == people.DesiredBusStop)
                {
                    Peoples.Remove(people);
                    countDropPeople++;
                }
            }
            CountOfPeople -= countDropPeople;
            Console.WriteLine("Из автобуса вышло: " + countDropPeople);
        }
        // перегрузка метода DropPeople(BusStop busStop), метод используется для выгрузки пассажиров на конечной остановке
        private void DropPeople()
        {
            Peoples.Clear();
            CountOfPeople = 0;
            Console.WriteLine("Из автобуса вышли все люди");
        }

        // метод для погрузки пассажиров
        private void LoadingPeople(BusStop busStop)
        {   // кол-во людей, севших в автобус
            short countLoadingPeople = 0;
            // определяем в какую сторону движется автобус
            // переходим к соответствующей ветке IF
            if (Way == true)
            {
                // проходимся по списку людей на остановке 
                foreach (People peopleOnTheStreat in busStop.TrueWay.ToList())
                {   // если вместимость автобуса позволяет, то сажаем человека на автобус, в противном случае выходим из цикла
                    if (CountOfPeople < Сapacity)
                    {
                        Peoples.Add(peopleOnTheStreat);
                        CountOfPeople++;
                        // удаляем пассажира из очереди на автобус
                        busStop.TrueWay.Remove(peopleOnTheStreat);
                        countLoadingPeople++;
                    }
                    else 
                        break;
                         
                }

            }
            else
            {

                foreach (var peopleOnTheStreat in busStop.FalseWay.ToList())
                {
                    if (CountOfPeople < Сapacity)
                    {
                        Peoples.Add(peopleOnTheStreat);
                        CountOfPeople++;
                        busStop.FalseWay.Remove(peopleOnTheStreat);
                        countLoadingPeople++;
                    }
                    else 
                        break;
                    
                }

            }


            Console.WriteLine("В автомус село: " + countLoadingPeople);

        }






    }






}
