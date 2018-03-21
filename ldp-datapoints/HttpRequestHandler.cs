using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        public event EventHandler<HttpEventArgs> OnPut;
        public event EventHandler<HttpEventArgs> OnPost;

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
            while (true)
            {
                var context = httpListener.GetContext();
                Task.Factory.StartNew(() => handleRequest(context));
            }
        }

        private void handleRequest(HttpListenerContext context)
        {
            var eventArgs = new HttpEventArgs(context.Request, context.Response);
            var method = context.Request.HttpMethod;
            try
            {
                if (method == HttpMethod.Get.Method)
                {
                    OnGet?.Invoke(this, eventArgs);
                }
                else if (method == HttpMethod.Post.Method)
                {
                    OnPost?.Invoke(this, eventArgs);
                }
                else if (method == HttpMethod.Put.Method)
                {
                    OnPut?.Invoke(this, eventArgs);
                }
            }
            catch (NotImplementedException)
            {
                context.Response.Abort();
            }
        }
    }
}
