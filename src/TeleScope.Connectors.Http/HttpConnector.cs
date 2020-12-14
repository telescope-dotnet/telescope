﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeleScope.Connectors.Abstractions;
using TeleScope.Connectors.Abstractions.Events;
using TeleScope.Connectors.Abstractions.Secrets;
using TeleScope.Connectors.Http.Abstractions;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace TeleScope.Connectors.Http
{
	public class HttpConnector : IHttpConnectable
	{
		// -- fields

		public const int TIMEOUT = 10000;

		private ILogger _log;
		private HttpClient _client;
		private HttpEndpoint _endpoint;
		private StringContent _content;

		// -- events

		public event ConnectorEventHandler Connected;
		public event ConnectorEventHandler Disconnected;
		public event ConnectorCompletedEventHandler Completed;
		public event ConnectorFailedEventHandler Failed;


		// -- properties

		public bool IsConnected { get; private set; }

		// -- constructurs

		public HttpConnector()
		{
			_log = LoggingProvider.CreateLogger<HttpConnector>();
		}

		public HttpConnector(HttpClient client, HttpEndpoint endpoint) : this()
		{
			_client = client;
			_endpoint = endpoint;
		}

		// -- methods

		IConnectable IConnectable.Connect()
		{
			throw new NotImplementedException();
		}

		public IHttpConnectable Connect()
		{
			_client.BaseAddress = _endpoint.Address;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_client.BaseAddress);
			request.Timeout = TIMEOUT;
			request.Method = "HEAD";

			try
			{
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					if (response.StatusCode == HttpStatusCode.OK)
					{
						IsConnected = true;
					}
					else
					{
						throw new WebException(response.StatusDescription);
					}
				}
			}
			catch (WebException wex)
			{
				Failed?.Invoke(this, new ConnectorFailedEventArgs(wex, _client.BaseAddress.AbsoluteUri));
			}

			Connected?.Invoke(this, new ConnectorEventArgs(_endpoint.Address.AbsoluteUri));
			return this;
		}

		public IHttpConnectable Connect(HttpClient client, HttpEndpoint endpoint)
		{
			_client = client;
			_endpoint = endpoint;
			return Connect();
		}

		public IConnectable Disconnect()
		{
			var address = _endpoint.Address.AbsoluteUri;

			_client.Dispose();
			_endpoint = null;
			_content = null;
			IsConnected = false;

			Disconnected?.Invoke(this, new ConnectorEventArgs(address));
			return this;
		}

		public IHttpConnectable AddHeader(string name, string value)
		{
			if (_client == null)
			{
				return this;
			}

			_client.DefaultRequestHeaders.Add(name, value);

			return this;
		}

		public IHttpConnectable SetContent(string jsonContent)
		{
			return SetContent(jsonContent, Encoding.UTF8, "application/json");
		}

		public IHttpConnectable SetContent(string content, Encoding encoding, string mediatype)
		{
			_content = new StringContent(content, encoding, mediatype);
			return this;
		}

		public async Task<string> CallAsync()
		{
			var request = new HttpRequestMessage
			{
				Method = _endpoint.Method(),
				RequestUri = _endpoint.Address,
				Content = _content
			};

			_log.Trace($"Calling via http '{_endpoint}'.");

			var response = await _client.SendAsync(request);
			var result = await response.Content.ReadAsStringAsync();

			Completed?.Invoke(this, new ConnectorCompletedEventArgs(response.ReasonPhrase, response));
			return result;
		}

		public async Task<T> CallAsync<T>(Func<string, T> convert)
		{
			string response = await CallAsync();
			return convert(response);
		}

	}
}
