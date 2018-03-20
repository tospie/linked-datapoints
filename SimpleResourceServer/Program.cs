using LDPDatapoints;
using LDPDatapoints.Resources;
using System;
using System.Collections.ObjectModel;

namespace SimpleResourceServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ObservableCollection<int> testCollection = new ObservableCollection<int>();
            CollectionResource<ObservableCollection<int>, int> r = new CollectionResource<ObservableCollection<int>, int>(testCollection, "http://localhost:3333/int/");
            testCollection.Add(12);
        }
    }
}
