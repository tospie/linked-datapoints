using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints
{
    public class HttpEventArgs : EventArgs
    {
        public HttpListenerRequest request;
        public HttpListenerResponse response;
        public HttpEventArgs(HttpListenerRequest request, HttpListenerResponse response)
        {
            this.request = request;
            this.response = response;
        }
    }

    public class HttpRequestListener
    {
        private HttpListener httpListener;

        public event EventHandler<HttpEventArgs> OnGet;

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
            OnGet?.Invoke(this, new HttpEventArgs(context.Request, context.Response));
        }
    }
}
