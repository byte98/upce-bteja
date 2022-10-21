using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW01
{
    class Program
    {
        static void Main(string[] args)
        {
            Interpreter interpreter = new Interpreter();
            String input = "";
            do
            {
                input = Console.ReadLine();
                double result = interpreter.Execute(input);
                Console.WriteLine(String.Format(">>> {0:0.0#}", result));
            }
            while (input.Length > 0);
        }
    }
}
