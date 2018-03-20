using LDPDatapoints;
using LDPDatapoints.Resources;
using LDPDatapoints.Subscriptions;
using System;
using System.Collections.ObjectModel;

namespace SimpleResourceServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            WebsocketSubscription wsSubscription = new WebsocketSubscription("ws://stuff");
            WebHookSubscription whSubscription = new WebHookSubscription("http://test");
            ValueResource<int> testResource = new ValueResource<int>(3, "http://localhost:12345/int/");
            ObservableCollection<int> c = new ObservableCollection<int>();
            CollectionResource<ObservableCollection<int>, int> r = new CollectionResource<ObservableCollection<int>, int> (c, "http://localhost:12345/c/");

            Resource<bool> test2 = new Resource<bool>(true, "http://127.0.0.1:12345/bool/");
            r.Subscribe(whSubscription);
            r.Subscribe(wsSubscription);
            r.Value.Add(12);
            Console.WriteLine("Eingabe beendet Programm...");
            Console.ReadKey();
        }
    }
}
