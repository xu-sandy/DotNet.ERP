using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.JPush;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = IJPushFactory.Create();
            Console.WriteLine(result);
        }
    }
}
