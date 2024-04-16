using System;

namespace lab2
{
    public abstract class BaseList
    {
        protected int count = 0;
        public int Count { get { return count; } }
        public abstract void Add(int a);
        public abstract void Insert(int a, int pos);
        public abstract void Delete(int pos);
        public abstract void Clear();

        public abstract int this[int i] { get; set; }

        public void Print()
        {
            for (int i = 0; i < Count; i++)
            {
                Console.Write(this[i] + " ");
            }
            Console.WriteLine();
        }

        public void Assign(BaseList dest)
        {
            dest.Assign(this);
        }

        protected abstract BaseList EmptyClone();

        public BaseList Clone()
        {
            BaseList clone_list = EmptyClone();
            clone_list.Assign(this);
            return clone_list;
        }

        public virtual void Sort()
        {
            for (int i = 0; i < Count - 1; i++)
            {
                for (int j = 0; j < Count - i - 1; j++)
                {
                    if (this[j] > this[j + 1])
                    {
                        int temp = this[j];
                        this[j] = this[j + 1];
                        this[j + 1] = temp;
                    }
                }
            }
        }

        public virtual bool Equals(BaseList list)
        {
            if (list == null || !(list is BaseList))
            {
                return false;
            }

            BaseList otherList = (BaseList)list;

            if (this.Count != otherList.Count)
            {
                return false;
            }

            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] != otherList[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class ArrList : BaseList
    {
        int[] buf;
        int Length;

        public ArrList()
        {
            Length = 2;
            buf = new int[Length];
            count = 0;
        }

        void Expand()
        {
            Length *= 2;
            int[] newbuf = new int[Length];
            Array.Copy(buf, newbuf, count);
            buf = newbuf;
        }

        public override void Add(int a)
        {
            if (count == Length)
            {
                Expand();
            }
            buf[count] = a;
            count++;
        }

        public override void Insert(int a, int pos)
        {
            if (pos < 0 || pos > count)
            {
                return;
            }
            if (buf.Length == count)
            {
                Array.Resize(ref buf, buf.Length * 2);
            }
            for (var i = count; i > pos; i--)
            {
                buf[i] = buf[i - 1];
            }
            buf[pos] = a;
            count++;
        }

        public override void Delete(int pos)
        {
            if (pos < 0 || pos >= count)
            {
                return;
            }
            for (int i = pos; i < count - 1; i++)
            {
                buf[i] = buf[i + 1];
            }
            count--;
        }

        public override void Clear()
        {
            buf = new int[2];
            count = 0;
        }

        public override int this[int i]
        {
            get
            {
                if (i >= count || i < 0)
                {
                    throw new ArgumentOutOfRangeException("Element is out of range");
                }
                return buf[i];
            }

            set
            {
                if (i >= count || i < 0)
                {
                    return;
                }
                buf[i] = value;
            }
        }

        protected override BaseList EmptyClone()
        {
            return new ArrList();
        }
    }

    public class ChainList : BaseList
    {
        private class Node
        {
            public int Data;
            public Node Next;

            public Node(int data)
            {
                Data = data;
                Next = null;
            }
        }

        private Node head;

        public ChainList()
        {
            head = null;
            count = 0;
        }

        public override void Add(int data)
        {
            Node newNode = new Node(data);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
            count++;
        }

        public override void Insert(int data, int pos)
        {
            if (pos < 0 || pos > count)
            {
                return;
            }

            Node newNode = new Node(data);
            if (pos == 0)
            {
                newNode.Next = head;
                head = newNode;
            }
            else
            {
                Node current = head;
                for (int i = 0; i < pos - 1; i++)
                {
                    current = current.Next;
                }
                newNode.Next = current.Next;
                current.Next = newNode;
            }
            count++;
        }

        public override void Delete(int pos)
        {
            if (pos < 0 || pos > count)
            {
                return;
            }
            if (pos == 0)
            {
                head = head.Next;
            }
            else
            {
                Node current = head;
                for (int i = 0; i < pos - 1; i++)
                {
                    current = current.Next;
                }
                if (current.Next != null)
                {
                    if (current.Next.Next == null)
                    {
                        current.Next = null;
                    }
                    else
                    {
                        current.Next = current.Next.Next;
                    }
                }
            }
            count--;
        }

        public override void Clear()
        {
            head = null;
            count = 0;
        }

        public override int this[int i]
        {
            get
            {
                Node current = head;
                for (int j = 0; j < i; j++)
                {
                    if (current != null)
                    {
                        current = current.Next;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Index out of range");
                    }
                }
                return current.Data;
            }
            set
            {
                Node current = head;
                for (int j = 0; j < i; j++)
                {
                    if (current != null)
                    {
                        current = current.Next;
                    }
                    else
                    {
                        return;
                    }
                }
                if (current != null)
                {
                    current.Data = value;
                }
                else
                {
                    return;
                }
            }
        }

        protected override BaseList EmptyClone()
        {
            return new ChainList();
        }
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
                int operation = random.Next(6);
                int value = random.Next(1000);
                int index = random.Next(1, 100);

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
                    Console.WriteLine($"Error: {e.Message}");
                    break;
                }
            }
            Console.WriteLine("Checking for equality: ");
            Console.WriteLine(list1.Count);
            Console.WriteLine(list2.Count);
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    Console.WriteLine("Lists are different");
                }
                else
                {
                    Console.WriteLine("Lists are the same");
                }
            }
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Tester.Test();
        }
    }
}
