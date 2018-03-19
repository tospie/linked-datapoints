using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints
{
    class HttpRequestListener
    {
        private HttpListener httpListener;

        public event EventHandler OnGet;

        public HttpRequestListener(string path)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(path);
            httpListener.Start();
            TaskFactory tf = new TaskFactory();
            tf.StartNew(listen, TaskCreationOptions.LongRunning);
        }

        private void listen()
        {
            while (httpListener.IsListening)
            {
                httpListener.GetContextAsync().ContinueWith((task) =>
                {
                    handleRequest(task);
                });
            }
        }

        private async void handleRequest(Task<HttpListenerContext> task)
        {
            HttpListenerContext context = await task;
        }
    }
}
