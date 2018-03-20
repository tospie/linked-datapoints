namespace LDPDatapoints.Resources
{
    public abstract class Resource
    {
        protected string route { get; }
        protected HttpRequestListener RequestListener { get; }

        public Resource(string route)
        {
            this.route = route;
            RequestListener = new HttpRequestListener(route);
            RequestListener.OnGet += onGet;
        }

        protected abstract void onGet(object sender, HttpEventArgs e);
    }
}
