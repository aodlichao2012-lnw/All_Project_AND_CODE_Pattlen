using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        public  delegate void testdelegate();
        testdelegate Testdelegate = Program;
        static void Main(string[] args)
        {
        }

        private static void Program_handler()
        {
            throw new NotImplementedException();
        }

        public static void test1()
        {

        }
        public static void test2()
        {

        }
    }
}
