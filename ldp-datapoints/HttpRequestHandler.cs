/*
Copyright 2018 T.Spieldenner, DFKI GmbH

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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
        public event EventHandler<HttpEventArgs> OnOptions;

        public HttpRequestListener(string path)
        {
            try
            {
                httpListener = new HttpListener();
                httpListener.Prefixes.Add(path);
                httpListener.Start();
            }
            catch (HttpListenerException e)
            {
                Console.WriteLine("[LDPDatapoints.HttpRequestHandler]  FAILED TO START HTTP LISTENER FOR ROUTE {0}, REASON: {1}", path, e.Message);
            }

            TaskFactory tf = new TaskFactory();
            Task listenerTask = tf.StartNew(listen, TaskCreationOptions.LongRunning);
            listenerTask.ContinueWith(t =>
            {
                if (t.IsCanceled || t.IsFaulted)
                {
                    foreach (Exception e in t.Exception.InnerExceptions)
                    {
                        Console.WriteLine("[LDPDatapoints.HttpRequestHandler]  HTTP LISTENER FAILED WITH EXCEPTION: {0}\n{1}", e.Message, e.StackTrace);
                    }
                }
            }
            );
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
                else if (method == HttpMethod.Options.Method)
                {
                    OnOptions?.Invoke(this, eventArgs);
                }
            }
            catch (NotImplementedException)
            {
                context.Response.Abort();
            }
        }
    }
}
