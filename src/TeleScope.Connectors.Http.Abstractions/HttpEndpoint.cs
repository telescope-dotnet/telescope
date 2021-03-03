using System;
using System.Net.Http;

namespace TeleScope.Connectors.Http.Abstractions
{
	/// <summary>
	/// This class represents an editable http endpoint,
	/// where updates on the request part may be made without changing the base URL.
	/// </summary>
	public class HttpEndpoint
	{
		// -- properties

		/// <summary>
		/// Gets the complete Url that has the base address and request part.
		/// </summary>
		public Uri Address { get; private set; }

		public HttpMethod MethodType { get; private set; }

		// -- constructor


		public HttpEndpoint(Uri address)
		{
			Address = address;
		}

		public HttpEndpoint(Uri address, HttpMethod method) : this(address)
		{
			MethodType = method;
		}

		public HttpEndpoint(string baseAddress, string request, HttpMethod method) : 
			this(new Uri(new Uri(baseAddress), request), method)
		{

		}

		// -- methods

		public virtual HttpEndpoint Request(string request)
		{
			var port = (!Address.IsDefaultPort ? $":{Address.Port}" : "");
			var baseUri = $"{Address.Scheme}://{Address.Host}{port}";
			Address = new Uri(new Uri(baseUri), request);
			return this;
		}

		public virtual HttpEndpoint Method(HttpMethod method)
		{
			MethodType = method ?? throw new ArgumentNullException(nameof(method));
			return this;
		}

		public override string ToString() => $"{MethodType.Method}: {Address.AbsoluteUri}";

	}
}
