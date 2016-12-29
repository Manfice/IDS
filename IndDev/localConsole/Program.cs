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
            var p = new CrudProducts();
            var cat = p.GetProducCategorys();

            foreach (var menu in cat)
            {
                Console.WriteLine($"id: {menu.Id} title: {menu.Title}");
                Console.WriteLine($"{menu.ShotDescription}");
                Console.WriteLine("***********************************************************");
            }

            Console.ReadLine();
        }
    }
}
