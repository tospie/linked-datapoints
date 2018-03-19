using LDPDatapoints;
using System;

namespace SimpleResourceServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Resource<OInt, int> testResource = new Resource<OInt, int>();
        }
    }
}
