using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints.Resources
{
    public static class ResourceRequestDispatcher
    {

        private static Dictionary<string, HttpListener> basePathListeners = new Dictionary<string, HttpListener>();
        private static Dictionary<string, Resource> registeredResources = new Dictionary<string, Resource>();

        public static void Register(Resource resource)
        {
            Uri routeUri = new Uri(resource.Route);
            string basePath = resource.Route.Replace(routeUri.PathAndQuery, "").TrimEnd('/') + '/';
            registeredResources.Add(resource.Route.TrimEnd('/')+'/', resource);

            if (!basePathListeners.ContainsKey(basePath))
                CreateBasePathListener(basePath);
        }

        private static void CreateBasePathListener(string basePath)
        {
            HttpListener httpListener;
            try
            {
                httpListener = new HttpListener();
                httpListener.Prefixes.Add(basePath);
                httpListener.Start();
                TaskFactory tf = new TaskFactory();
                Task listenerTask = tf.StartNew(() =>
                {
                    while (true)
                    {
                        var context = httpListener.GetContext();
                        Task.Factory.StartNew(() => handleRequest(context, httpListener));
                    }
                }
                , TaskCreationOptions.LongRunning);
                basePathListeners.Add(basePath, httpListener);
            }
            catch (HttpListenerException e)
            {
                Console.WriteLine("[LDPDatapoints.Resources.ResourceRequestDispatcher]  FAILED TO START HTTP LISTENER FOR ROUTE {0}, REASON: {1}",
                    basePath,
                    e.Message);
            }
        }

        private static void handleRequest(HttpListenerContext context, HttpListener listener)
        {
            Uri uri = context.Request.Url;

            string requestedUrl = uri.ToString();
            if (uri.Query.Length > 0)
                requestedUrl = requestedUrl.Substring(0, requestedUrl.IndexOf("?"));
            requestedUrl = requestedUrl.TrimEnd('/') + '/';
            if (!registeredResources.ContainsKey(requestedUrl))
            {
                context.Response.StatusCode = 404;
                string returnMessage = "No Resource was found under the requested URL";
                context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(returnMessage), 0, returnMessage.Length);
                context.Response.OutputStream.Flush();
                context.Response.OutputStream.Close();
                return;
            }

            Resource requestedResource = registeredResources[requestedUrl];
            var method = context.Request.HttpMethod;
            if (method == HttpMethod.Get.Method)
                requestedResource.onGet(listener, new HttpEventArgs(context.Request, context.Response));
            else if (method == HttpMethod.Post.Method)
                requestedResource.onPost(listener, new HttpEventArgs(context.Request, context.Response));
            else if (method == HttpMethod.Put.Method)
                requestedResource.onPut(listener, new HttpEventArgs(context.Request, context.Response));
            else if (method == HttpMethod.Options.Method)
                requestedResource.onOptions(listener, new HttpEventArgs(context.Request, context.Response));
        }
    }
}
