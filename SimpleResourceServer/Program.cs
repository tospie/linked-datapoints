using LDPDatapoints;
using System;

namespace SimpleResourceServer
{
    class OInt : IObservable<int>
    {
        public IDisposable Subscribe(IObserver<int> observer)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Resource<OInt> testResource = new Resource<OInt>();
        }
    }
}
