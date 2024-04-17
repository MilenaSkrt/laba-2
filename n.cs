using System;

namespace lab2
{
    public abstract class BaseList
    {
        //абстрактные методы, которое реализованы в классах-наследниках
        protected int count = 0;
        public int Count { get { return count; } }
        public abstract void Add(int a);
        public abstract void Insert(int a, int pos);
        public abstract void Delete(int pos);
        public abstract void Clear();

        public abstract int this[int i] { get; set; }
        public void Print() //
        {
            for (int i = 0; i < Count; i++)
            {
                Console.Write(this[i]);
            }
            Console.WriteLine();
        }
        public void AssignTo(BaseList dest)
        {
            dest.Assign(this);
        }//из объекта в то что надо
        //абстрактный метод для копирования элементов списка 
        protected abstract BaseList EmptyClone(); //создает пустышки для Clone() //
        public BaseList Clone()
        {
            BaseList clone_list = EmptyClone();
            clone_list.Assign(this);
            return clone_list;
        }
        //пузырьковая сортировка
        public virtual void Sort()
        {
            // внешний цикл проходит по всем элементам списка за исключением последнего
            for (int i = 0; i < Count - 1; i++)
            {
                // внутренний цикл проходит по всем элементам списка, исключая уже отсортированные
                for (int j = 0; j < Count - i - 1; j++)
                {
                    // если текущий элемент больше следующего, меняем их местами
                    if (this[j] > this[j + 1])
                    {
                        int temp = this[j];
                        this[j] = this[j + 1];
                        this[j + 1] = temp;
                    }
                }
            }
        }
        public virtual bool Equals(BaseList list)  //
        {
            // Проверяем, что другой список не равен null и что тип объекта является производным от BaseList
            if (list == null || !(list is BaseList))
            {
                return false;
            }

            // Приведение объекта к типу BaseList для сравнения
            BaseList otherList = (BaseList)list;

            // Сравниваем количество элементов
            if (this.Count != otherList.Count)
            {
                return false;
            }

            // Построение логики сравнения элементов списка
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] != otherList[i])
                {
                    return false;
                }
            }
            return true;
        }
        public void Assign(BaseList source)  //
        {
            // очищаем текущий список, чтобы избежать дублирования элементов 
            Clear();
            // проверка источника на null
            if (source == null)  // Если источник пуст или null, просто завершаем выполнение метода
            {
                return;
            }
            // проходим по элементам источника и добавляем их в текущий список
            for (int i = 0; i < source.Count; i++)
            {
                // добавляем каждый элемент из источника в текущий список
                Add(source[i]);
            }
        }
    }

    public class ArrList : BaseList
    {
        int[] buf; // массив для хранения элементов 
                   //int count;
        int Length;//
        // Переопределение свойства для получения количества элементов в списке


        public ArrList()
        {
            Length = 2;
            buf = new int[Length]; // задаем нач. размер
            count = 0;

        }

        void Expand()
        {
            Length *= 2;
            int[] newbuf = new int[Length];
            Array.Copy(buf, newbuf, count);
            buf = newbuf;
        }
        // метод для добавления элементов в список
        public override void Add(int a)
        {
            if (count >= Length)
            {
                Expand();
            }
            buf[count] = a;
            count++;
        }

        // метод для добавления элемента на определенную позицию
        public override void Insert(int a, int pos)
        {
            if (pos < 0 || pos > base.count)
            {
                return; // позиция вне диапазона списка
            }
            if (buf.Length == base.count)
            {
                Array.Resize(ref buf, buf.Length * 2);
            }
            for (var i = base.count; i > pos; i--)
            {
                buf[i] = buf[i - 1];
            }
            buf[pos] = a;
            base.count++;
        }

        // метод для удаления элемента с определенной позиции
        public override void Delete(int pos)
        {
            if (pos < 0 || pos >= base.count)
            {
                return; //проверка позиции
            }
            //сдвигаем элементы влево для удаления
            for (int i = pos; i < base.count - 1; i++)
            {
                buf[i] = buf[i + 1];
            }
            //уменьшаем кол-во элементов
            base.count--;
        }

        // метод очистки списка
        public override void Clear()
        {
            buf = new int[6];
            base.count = 0; //сбрасываем кол-во элементов
        }

        //индексатор для доступа к элементам списка
        public override int this[int i]
        {
            get
            {
                if (i >= base.count || i < 0)
                {
                    throw new ArgumentOutOfRangeException("Element is out of range");
                }
                return buf[i]; //возращаем элемент списка по указанному индексу
            }

            set
            {
                if (i >= base.count || i < 0)
                {
                    //throw new ArgumentOutOfRangeException("Element is out of range");
                    return;
                }
                buf[i] = value; //присваиваем новое значение элементу списка по указанному индексу
            }
        }
        // public override void Print()
        // {
        //     Console.WriteLine($"Динамический массив:");
        //     for (int i = 0; i < count; i++)
        //     {
        //         Console.Write(buf[i] + " ");
        //     }
        //     Console.WriteLine();
        // }

        // переопределение метода EmptyClone для создания пустой копии текущего списка
        protected override BaseList EmptyClone()
        {
            // создание нового объекта ArrList в качестве пустой копии списка.
            return new ArrList();
        }

        // переопределение метода isEqual для сравнения текущего списка с другим списком
        // public override bool Equals(BaseList list)
        // {
        //      // проверка на null и разную длину списков
        //     if (list == null || Count != list.Count)
        //     {
        //         return false;
        //     }

        //     // Проверка на тип списка
        //     if (list is ArrList)
        //     {
        //         // приведение типа для доступа к методам ArrList
        //         ArrList comparedList = (ArrList)list;

        //         // Сравниваем элементы текущего списка с элементами сравниваемого списка
        //         for (int i = 0; i < Count; i++)
        //         {
        //             if (this[i] != comparedList[i])
        //             {
        //                 return false;
        //             }
        //         }

        //         return true;
        //     }
        //     else
        //     {
        //         // В случае, если тип списка не совпадает, невозможно выполнить сравнение
        //         Console.WriteLine("Невозможно выполнить сравнение: типы списков не совпадают.");
        //         return false;
        //     }
        // }
    }
    //цепной список
    public class ChainList : BaseList
    {
        private class Node
        {
            public int Data; //данные в узле
            public Node Next; // ссылка на след. элемент

            //конструктор для создания узла с заданными нач. условиями 
            public Node(int data)
            {
                Data = data;
                Next = null; //нач. условие 
            }
        }
        private Node head; //голова списка 1 узел
        //private int count; //кол-во узлов в списке

        //сам цепной список
        public ChainList()
        {
            head = null; //создание пустого списка
            count = 0;
        }

        //метод для добавления узлов
        public override void Add(int data)
        {
            Node newNode = new Node(data); //создаем новый список
            if (head == null)
            {
                head = newNode;//создание 1-ого узла, если его нет (newNode становится головой списка)
            }
            else
            {
                Node current = head; // current - голова списка
                while (current.Next != null) //идем к последнему узлу
                {
                    current = current.Next;
                }
                current.Next = newNode; //добавляем новый узел в конец
            }
            count++;
        }

        //метод для добавления узла на указанную позицию
        public override void Insert(int data, int pos)
        {
            if (pos < 0 || pos > count)
            {
                return; // проверка позиции
            }

            Node newNode = new Node(data); // создаем новый узел с указынными данными
            if (pos == 0) //вставка в начало
            {
                //делаем его головным
                newNode.Next = head;
                head = newNode;
            }
            else //вставка в середину или в конец списка
            {
                //находим узел, который находится до позиции вставки
                Node current = head;
                for (int i = 0; i < pos - 1; i++)
                {
                    current = current.Next;
                }
                newNode.Next = current.Next; // вставляем новый узел между current и current.Next
                current.Next = newNode;
            }
            count++;
        }

        //метод для удаления узла с указанной позиции
        public override void Delete(int pos)
        {
            if (pos < 0 || pos > count)
            {
                return; //проверка позиции
            }
            if (pos == 0)
            {
                head = head.Next; //удаление головы списка
            }
            else // удаление из середины или конца списка
            {
                //находим узел, который находится до позиции удаления
                Node current = head;
                for (int i = 0; i < pos - 1; i++)
                {
                    current = current.Next;
                }
                // проверяем, не является ли current.Next последним элементом
                if (current.Next != null)
                {
                    // если удаляем последний элемент, то устанавливаем Next предпоследнего элемента в null
                    if (current.Next.Next == null)
                    {
                        current.Next = null;
                    }
                    else
                    {
                        current.Next = current.Next.Next; //пропускаем узел, который хотим удалить 
                    }
                }
            }
            count--;
        }

        //метод для очистки 
        public override void Clear()
        {
            head = null; //удаляем ссылку на голову списка
            count = 0;
        }
        // получение кол-ва элементов списка

        //индексатор для доступа к узлам списка
        public override int this[int i]
        {
            get
            {
                //начинаем с головного узла
                Node current = head;
                for (int j = 0; j < i; j++)
                {
                    // переходим к след. узлу, пока не достигнем нужного индекса
                    if (current != null)
                    {
                        current = current.Next;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Index out of range");
                    }
                }
                //возвращаем данные узла по указанному индексу
                return current.Data;
            }
            set
            {
                //начинаем с головного узла
                Node current = head;
                for (int j = 0; j < i; j++)
                {
                    //переходим к след. узлу, пока не достигнем нужного индекса
                    if (current != null)
                    {
                        current = current.Next;
                    }
                    else
                    {
                        //throw new ArgumentOutOfRangeException("Index out of range");
                        return;
                    }
                }
                //проверяем, что узел существует
                if (current != null)
                {
                    current.Data = value; //присваиваем новое значение
                }
                else
                {
                    //throw new ArgumentOutOfRangeException("Index out of range");
                    return;
                }
            }
        }
        // public override void Print()
        // {
        //     Console.WriteLine($"Цепной список:");
        //     Node current = head;
        //     while (current != null)
        //     {
        //         Console.Write(current.Data + " ");
        //         current = current.Next;
        //     }
        //     Console.WriteLine();
        // }

        protected override BaseList EmptyClone()
        {
            return new ChainList(); //создаем пустышку как ChainList
        }
        // сортировка пузырьком 
        public override void Sort()
        {
            bool swapped; // флаг, указывающий, была ли произведена перестановка элементов во время итерации
            Node current; // текущий узел
            Node last = null; // последний узел, на который указывает current

            if (head == null)
                return; // если список пуст, сортировка не требуется

            do
            {
                swapped = false; // переинициализация флага на каждой итерации
                current = head; // устанавливаем текущий узел в голову списка


                while (current.Next != last)
                {
                    // если значение текущего узла больше значения следующего узла, меняем их местами
                    if (current.Data > current.Next.Data)
                    {
                        int temp = current.Data;
                        current.Data = current.Next.Data;
                        current.Next.Data = temp;
                        swapped = true; // устанавливаем флаг перестановки в true
                    }
                    current = current.Next; // переходим к следующему узлу
                }
                last = current; // обновляем последний узел
            } while (swapped); // продолжаем итерации, пока есть перестановки
        }
        // // то же самое только здесь приведение к ChainList
        // public override bool Equals(BaseList list)
        // {
        //     if (list == null || Count != list.Count)
        //     {
        //         return false;
        //     }

        //     // Проверка на тип списка
        //     if (list is ChainList)
        //     {
        //         ChainList comparedList = (ChainList)list;

        //         // Сравниваем элементы текущего списка с элементами сравниваемого списка
        //         for (int i = 0; i < Count; i++)
        //         {
        //             if (this[i] != comparedList[i])
        //             {
        //                 return false;
        //             }
        //         }

        //         return true;
        //     }
        //     else
        //     {
        //         // В случае, если тип списка не совпадает, невозможно выполнить сравнение
        //         Console.WriteLine("Невозможно выполнить сравнение: типы списков не совпадают.");
        //         return false;
        //     }
        // }
    }

    class Tester
    {
        private static Random random = new Random();
        public static void Test()
        {
            ArrList list1 = new ArrList();
            ChainList list2 = new ChainList();

            for (int i = 0; i < 10000; i++)
            {
                int operation = random.Next(6); //выбор операции
                int value = random.Next(1000); // случ. знач.
                int index = random.Next(1, 100); // случ. индекс

                try
                {
                    switch (operation)
                    {
                        case 0:
                            list1.Add(value);
                            list2.Add(value);
                            break;
                        case 1:
                            if (list1.Count > 0 && list2.Count > 0)
                            {
                                list1.Insert(value, index);
                                list2.Insert(value, index);
                            }
                            break;
                        case 2:
                            if (list1.Count > 0 && list2.Count > 0)
                            {
                                list1.Delete(index);
                                list2.Delete(index);
                            }
                            break;
                        case 3:
                            //list1.Clear();
                            //list2.Clear();
                            break;
                        case 4:
                            if (list1.Count > 0 && list2.Count > 0)
                            {
                                list1[index] = value;
                                list2[index] = value;
                            }
                            break;
                        case 5:
                            list1.Sort();
                            list2.Sort();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка: {e.Message}");
                    break;
                }
            }
            Console.WriteLine($"Проверка на одинаковость: ");
            Console.WriteLine(list1.Count);
            Console.WriteLine(list2.Count);
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    Console.WriteLine($"Листы разные");
                }
                else
                {
                    Console.WriteLine($"Листы одинаковые");
                }
            }
        }
    }

    class Programm
    {
        public static void Main(string[] args)
        {

            Console.WriteLine($"Динамический список");
            Console.WriteLine();
            BaseList dynamicArray = new ArrList();
            dynamicArray.Add(1);
            dynamicArray.Add(2);
            dynamicArray.Add(3);
            dynamicArray.Add(4);
            dynamicArray.Add(5);
            Console.WriteLine($"После добавления: ");
            dynamicArray.Print(); //1 2 3 4 5
            dynamicArray.Insert(9, 2);
            Console.WriteLine($"После вставки: ");
            dynamicArray.Print();  //1 2 9 3 4 5
            dynamicArray.Delete(1);
            Console.WriteLine($"После удаления: ");
            dynamicArray.Print(); //1 9 3 4 5
            dynamicArray.Clear();
            Console.WriteLine($"После очистки: ");
            dynamicArray.Print(); //пустая строка
            dynamicArray.Add(1);
            dynamicArray.Add(2);
            Console.WriteLine($"Добавила два элемента");
            dynamicArray.Print(); //1 2
            dynamicArray[0] = 9;
            int value = dynamicArray[0];
            Console.WriteLine($"Изменила первый элемент на 9");
            dynamicArray.Print(); // 9 2
            Console.WriteLine($"Значение 1-ого элемента: {value}"); //9
            int currentCount = dynamicArray.Count;
            Console.WriteLine($"Текущее кол-во элементов: {currentCount}"); //2

            Console.WriteLine();
            Console.WriteLine($"Связной список");
            Console.WriteLine();
            BaseList list = new ChainList();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Console.WriteLine($"После добавления: ");
            list.Print(); // 1 2 3
            list.Insert(6, 2);
            Console.WriteLine($"После вставки: ");
            list.Print(); //1 2 6
            list.Delete(1);
            Console.WriteLine($"После удаления: ");
            list.Print(); // 1 6
            list.Clear();
            Console.WriteLine($"После очистки: ");
            list.Print(); // пустая строка
            list.Add(1);
            list.Add(2);
            Console.WriteLine($"Добавила два элемента");
            list.Print(); // 1 2
            list[1] = 45;
            int valueOfChainList = list[1];
            Console.WriteLine($"Изменила второй элемент на 45");
            list.Print();// 1 45
            Console.WriteLine($"Значение 2-ого элемента: {valueOfChainList}"); //45
            int currentCountOfChainList = list.Count;
            Console.WriteLine($"Текущее кол-во элементов: {currentCountOfChainList}"); //2

            Console.WriteLine();

            Console.WriteLine($"ПРО BASELIST ");
            Console.WriteLine();
            Console.WriteLine($"ПРО DYNAMICARRAY");
            BaseList newarrList = new ArrList();
            newarrList.Add(40);
            newarrList.Add(25);
            newarrList.Add(30);
            Console.WriteLine($"После добавления");
            newarrList.Print(); //40 25 30
            newarrList.Sort();
            Console.WriteLine($"После сортировки");
            newarrList.Print(); //25 30 40
            dynamicArray.Assign(newarrList);
            Console.WriteLine($"После назначения:");
            dynamicArray.Print(); // 25 30 40
            BaseList clonedList = (ArrList)dynamicArray.Clone();
            Console.WriteLine($"Клонированный лист:");
            clonedList.Print(); // 25 30 40
            Console.WriteLine($"Равны ли листы? " + dynamicArray.Equals(newarrList)); // True
            dynamicArray.Clear();
            Console.WriteLine($"После очистки:");
            dynamicArray.Print(); // пустая строка

            Console.WriteLine($"ТУТ ПРО CHAINLIST:");
            BaseList newarrChainList = new ChainList();
            newarrChainList.Add(40);
            newarrChainList.Add(25);
            newarrChainList.Add(30);
            Console.WriteLine($"После добавления");
            newarrChainList.Print(); //40 25 30
            newarrChainList.Sort();
            Console.WriteLine($"После сортировки");
            newarrChainList.Print(); //25 30 40
            list.Assign(newarrChainList);
            Console.WriteLine($"После назначения:");
            list.Print(); //25 30 40
            BaseList clonedChainList = (ChainList)list.Clone();
            Console.WriteLine($"Клонированный лист:");
            clonedChainList.Print(); //25 30 40
            Console.WriteLine("Равны ли листы? " + list.Equals(newarrChainList)); // True
            list.Clear();
            Console.WriteLine($"После очистки:");
            list.Print(); // пустая строка

            Console.WriteLine($"***ТЕСТИРОВАНИЕ***");
            Console.WriteLine();
            Tester.Test();
        }
    }
}
