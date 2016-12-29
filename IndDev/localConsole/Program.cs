using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using IndDev.Domain.Context;

namespace localConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            while (Console.ReadLine()!="x")
            {
                Console.Write("Введите число: ");
                var x = double.Parse(Console.ReadLine());
                if (x%1 == 0)
                {
                    Console.WriteLine("Целое");
                }
                else
                {
                    Console.WriteLine("Не Целое");
                }


            }
        }
    }
}
