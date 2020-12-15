﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeleScope.Connectors.Abstractions;
using TeleScope.Connectors.Abstractions.Secrets;

namespace TeleScope.Connectors.Http.Abstractions
{
	public interface IHttpConnectable : IConnectable
	{
		IHttpConnectable Connect(HttpEndpoint endpoint);

		IHttpConnectable Connect(HttpClient client, HttpEndpoint endpoint);

		IHttpConnectable AddHeader(string name, string value);

		IHttpConnectable SetContent(string jsonContent);

		IHttpConnectable SetContent(string content, Encoding encoding, string mediatype);

		Task<string> CallAsync();

		Task<T> CallAsync<T>(Func<string, T> convert);
	}
}
