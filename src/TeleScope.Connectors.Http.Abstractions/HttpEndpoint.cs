﻿using System;
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

		/// <summary>
		/// Gets the http method is used by the endpoint.
		/// </summary>
		public HttpMethod MethodType { get; private set; }

		// -- constructor

		/// <summary>
		/// The default constructor sets the property `Address` with the given parameter and 
		/// uses the http method `GET` as `MethodType`.
		/// </summary>
		/// <param name="address">The full adress of the http endpoint.</param>
		public HttpEndpoint(Uri address)
		{
			Address = address;
			MethodType = HttpMethod.Get;
		}

		/// <summary>
		/// Sets the properties `Address` and `MethodType` with the given parameters.
		/// </summary>
		/// <param name="address">The full adress of the http endpoint.</param>
		/// <param name="method">The http method.</param>
		public HttpEndpoint(Uri address, HttpMethod method) : this(address)
		{
			MethodType = method;
		}

		/// <summary>
		/// Sets the properties `Address` and `MethodType` with the given parameters.
		/// The `Address` is separated by the two parameters `baseAddress` and `request`.
		/// </summary>
		/// <param name="baseAddress">The base adress of the http endpoint.</param>
		/// <param name="request">The request part of the http endpoint.</param>
		/// <param name="method">The http method.</param>
		public HttpEndpoint(string baseAddress, string request, HttpMethod method) : 
			this(new Uri(new Uri(baseAddress), request), method)
		{

		}

		// -- methods

		/// <summary>
		/// This method may be overridden and updates the request part of the http endpoint. The base url will not change.
		/// </summary>
		/// <param name="request">The request part of the http endpoint.</param>
		/// <returns>The calling instance.</returns>
		public virtual HttpEndpoint Request(string request)
		{
			var port = (!Address.IsDefaultPort ? $":{Address.Port}" : "");
			var baseUri = $"{Address.Scheme}://{Address.Host}{port}";
			Address = new Uri(new Uri(baseUri), request);
			return this;
		}

		/// <summary>
		/// This method may be overridden and updates the http method of the http endpoint. The `Address` property will not change.
		/// </summary>
		/// <param name="method">The http method.</param>
		/// <returns></returns>
		public virtual HttpEndpoint Method(HttpMethod method)
		{
			MethodType = method ?? throw new ArgumentNullException(nameof(method));
			return this;
		}

		/// <summary>
		/// Overrides the ToString method and returns the method type and the absolute url of the http endpoint.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"{MethodType.Method}: {Address.AbsoluteUri}";

	}
}
