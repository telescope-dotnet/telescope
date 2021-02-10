using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TeleScope.Connectors.Http.Abstractions
{
    /// <summary>
    /// Simple Data model class to represent a http endpoint.
    /// </summary>
    public class HttpEndpoint
    {

        // -- fields

        private const string GET = "get";
        private const string POST = "post";
        private const string PUT = "put";
        private const string DELETE = "delete";

        // -- properties

        /// <summary>
        /// Gets or sets the name of the method. Allowed names are Get, Post, Put or Delete.
        /// Case sensitivity is ignored. The property will be used in lower case.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets the complete Url out of base address and request
        /// </summary>
        public Uri Address { get; set; }

        // --constructor

        /// <summary>
        /// The default constructor sets the method type to a GET-Method
        /// </summary>
        public HttpEndpoint()
        {
            MethodName = GET;
        }

        public HttpEndpoint(Uri address) : this()
        {
            Address = address;
        }

        public HttpEndpoint(Uri address, string method) : this()
        {
            Address = address;
            MethodName = method;
        }

        public HttpEndpoint(string baseAddress, string request) : this()
        {
            Address = new Uri(new Uri(baseAddress), request);
        }

        public HttpEndpoint(string baseAddress, string request, string method) : this()
        {
            Address = new Uri(new Uri(baseAddress), request);
            MethodName = method;
        }

        // -- methods

        public void SetRequest(string request, string method = GET)
		{
            var baseUri = RemoveRequest(Address.AbsoluteUri, Address.AbsolutePath);
            Address = new Uri(new Uri(baseUri), request);
            MethodName = method;
        }

        private string RemoveRequest(string uri, string suffix)
        {
            if (uri.EndsWith(suffix))
            {
                return uri.Substring(0, uri.Length - suffix.Length);
            }
            else
            {
                return uri;
            }
        }

        /// <summary>
        /// Returns the HttpMethod class based on the property MethodName
        /// </summary>
        /// <returns>It`s wheater Get, Post, Put or Delete</returns>
        public HttpMethod Method()
        {
            switch (MethodName.ToLower())
            {
                case GET:
                    return HttpMethod.Get;
                case POST:
                    return HttpMethod.Post;
                case PUT:
                    return HttpMethod.Put;
                case DELETE:
                    return HttpMethod.Delete;
                default:
                    return HttpMethod.Get;
            }
        }

        public override string ToString() => $"{MethodName.ToUpper()}: {Address.AbsoluteUri}";
       
    }
}
