using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.MyMethod();
        }
    }


    class A : I
    {
        public void M_A()
        {
            Console.WriteLine("M_A");
        }

        public void M_B()
        {
            Console.WriteLine("M_B");
        }

        public void M_C()
        {
            Console.WriteLine("M_C");
        }

        public void MyMethod()
        {
            Console.WriteLine("WTF???????????");
        }
    }


    interface I
    {
        void M_A();
        void M_B();
        void M_C();
    }
}
