using LDPDatapoints;
using System;

namespace SimpleResourceServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Resource<int> testResource = new Resource<int>(3, "http://localhost:12345/int/");
            Resource<bool> test2 = new Resource<bool>(true, "http://127.0.0.1:12345/bool/");
            Console.WriteLine("Eingabe beendet Programm...");
            Console.ReadKey();
        }
    }
}
