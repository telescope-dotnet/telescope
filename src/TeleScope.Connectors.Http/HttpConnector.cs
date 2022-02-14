﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeleScope.Connectors.Abstractions;
using TeleScope.Connectors.Abstractions.Events;
using TeleScope.Connectors.Http.Abstractions;
using TeleScope.Connectors.Http.Caching;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace TeleScope.Connectors.Http
{
	/// <summary>
	/// This class implements the `IHttpConnectable` interface and uses the standard microsoft http client.
	/// </summary>
	public class HttpConnector : IHttpConnectable
	{
		// -- fields

		/// <summary>
		/// The constant timeout defines how long the http client will wait after a request until a response needs to arrive. 
		/// </summary>
		public const int TIMEOUT = 10000;

		private readonly ILogger<HttpConnector> log;

		private bool isDisposed;

		private HttpClient client;
		private HttpEndpoint endpoint;
		private StringContent content;

		private CancellationToken cancelToken;

		private ICacheable<string> cache;
		private bool enableCaching;

		// -- events

		/// <summary>
		/// The event is invoked when the `Connect` method has finished successfully.
		/// </summary>
		public event ConnectorEventHandler Connected;
		/// <summary>
		/// The event is invoked when the `Disconnect` method has finished successfully.
		/// </summary>
		public event ConnectorEventHandler Disconnected;
		/// <summary>
		/// The event is invoked when a type specific method or action has finished successfully.
		/// </summary>
		public event ConnectorCompletedEventHandler Completed;
		/// <summary>
		/// The event is invoked when any method or action has finished with a failure.
		/// </summary>
		public event ConnectorFailedEventHandler Failed;

		// -- properties

		/// <summary>
		/// Gets the state, if the connection is established or not.
		/// </summary>
		public bool IsConnected { get; private set; }

		// -- constructurs

		/// <summary>
		/// The default empty constructor binds a logger for internal usage.
		/// </summary>
		public HttpConnector()
		{
			log = LoggingProvider.CreateLogger<HttpConnector>();
			cancelToken = CancellationToken.None;

			cache = new HttpEndpointCache();

			Completed = OnCompleted;
		}

		/// <summary>
		/// Saves the properties and calls the empty default constructor.
		/// </summary>
		/// <param name="client">The http client to perform requests.</param>
		/// <param name="endpoint">The endpoint configuration executed by the client.</param>
		public HttpConnector(HttpClient client, HttpEndpoint endpoint) : this()
		{
			this.client = client;
			this.endpoint = endpoint;
		}

		// -- Finalizer

		~HttpConnector()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed)
			{
				return;

			}
			if (disposing)
			{
				// dispose managed resources
				client?.Dispose();
				content?.Dispose();
			}

			// dispose unmanaged resources and finish
			isDisposed = true;
		}

		// -- methods

		/// <summary>
		/// Tests the connection to the given endpoint and stores the parameter internally. 
		/// The http client must be ready-to-use before calling this method.
		/// </summary>
		/// <param name="endpoint">The endpoint configuration.</param>
		/// <returns>The calling instance.</returns>
		public IHttpConnectable Connect(HttpEndpoint endpoint)
		{
			return Connect(client, endpoint);
		}

		/// <summary>
		/// Tests the connection with the given http client and endpoint and stores both parameters internally.
		/// </summary>
		/// <param name="client">The http client that will be used by the connector.</param>
		/// <param name="endpoint">The endpoint configuration.</param>
		/// <returns>The calling instance.</returns>
		public IHttpConnectable Connect(HttpClient client, HttpEndpoint endpoint)
		{
			this.client = client;
			this.endpoint = endpoint;
			return Connect();
		}

		/// <summary>
		/// Tests the connection with the internal http client and endpoint.
		/// </summary>
		/// <returns>The calling instance.</returns>
		public IHttpConnectable Connect()
		{
			if (!Validate())
			{
				return this;
			}

			client.BaseAddress = endpoint.Address;

			try
			{

				using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Head, client.BaseAddress)).Result)
				{
					if (response.StatusCode == HttpStatusCode.OK)
					{
						IsConnected = true;
					}
					else
					{
						throw new WebException(response.ReasonPhrase);
					}
				}
			}
			catch (WebException wex)
			{
				log.Trace(wex, $"Http connection failed for: '{client.BaseAddress.AbsoluteUri}'.");
				Failed?.Invoke(this, new ConnectorFailedEventArgs(wex, client.BaseAddress.AbsoluteUri));
			}

			Connected?.Invoke(this, new ConnectorEventArgs(endpoint.Address.AbsoluteUri));
			return this;
		}

		IConnectable IConnectable.Connect()
		{
			return this.Connect();
		}

		/// <summary>
		/// Disposes the http client and deletes the internal fields.
		/// </summary>
		/// <returns>The calling instance.</returns>
		public IConnectable Disconnect()
		{
			var address = endpoint.Address.AbsoluteUri;

			client.Dispose();
			endpoint = null;
			content = null;
			IsConnected = false;

			Disconnected?.Invoke(this, new ConnectorEventArgs(address));
			return this;
		}

		public IHttpConnectable WithCaching()
		{
			enableCaching = true;
			return this;
		}

		public IHttpConnectable AddCancelToken(CancellationToken token)
		{
			cancelToken = token;
			return this;
		}

		/// <summary>
		/// Updates the request part of the http endpoint configuration.
		/// </summary>
		/// <param name="request">The request part of the url.</param>
		/// <param name="method">Optional: The method type of the call.</param>
		/// <returns>The calling instance.</returns>
		public IHttpConnectable SetRequest(string request, HttpMethod method = null)
		{
			if (!Validate())
			{
				return this;
			}

			endpoint.Request(request).Method(method);
			return this;
		}

		/// <summary>
		/// Adds an http header to the next request as simple pair of name and value.
		/// </summary>
		/// <param name="name">The name of the header information.</param>
		/// <param name="value">The value of the header information.</param>
		/// <returns>The calling instance.</returns>
		public IHttpConnectable AddHeader(string name, string value)
		{
			if (client is null)
			{
				return this;
			}

			client.DefaultRequestHeaders.Add(name, value);

			return this;
		}

		/// <summary>
		/// Sets the payload of the http request which must be represented as json compliant string.
		/// </summary>
		/// <param name="jsonContent">The payload for the next http request.</param>
		/// <returns>The calling instance.</returns>
		public IHttpConnectable SetContent(string jsonContent)
		{
			return SetContent(jsonContent, Encoding.UTF8, "application/json");
		}

		/// <summary>
		/// Sets the payload of the next http request.
		/// </summary>
		/// <param name="content">The payload as string.</param>
		/// <param name="encoding">The encoding to format the string before serialization.</param>
		/// <param name="mediatype">The media type of the payload.</param>
		/// <returns>The calling instance.</returns>
		public IHttpConnectable SetContent(string content, Encoding encoding, string mediatype)
		{
			this.content = new StringContent(content, encoding, mediatype);
			return this;
		}


		/// <summary>
		/// Performs the http request asynchronously that is defined by the http endpoint and optional parameters.
		/// </summary>
		/// <typeparam name="T">The generic returned type T.</typeparam>
		/// <param name="convert">The function converts the response body into the generic type T.</param>
		/// <returns>The executing task whereby the result of the task is of type T.</returns>
		public async Task<T> CallAsync<T>(Func<string, T> convert)
		{
			string response = await CallAsync();
			return convert(response);
		}

		/// <summary>
		/// Performs the http request asynchronously that is defined by the http endpoint and optional parameters.
		/// </summary>
		/// <returns>The executing task whereby the result is the raw string of the response body.</returns>
		public async Task<string> CallAsync()
		{
			var request = new HttpRequestMessage
			{
				Method = endpoint.MethodType,
				RequestUri = endpoint.Address,
				Content = content
			};

			if (enableCaching)
			{
				return await Task.Run(() =>
				{
					return cache.GetOrInvoke(endpoint.ToString(), call);
				});
			}
			else
			{
				return await Task.Run(() =>
				{
					return call();
				});
			}

			// -- local function

			string call()
			{
				log.Trace($"Calling via http '{endpoint}'.");

				var response = client.Send(request, cancelToken);
				var result = response.Content.ReadAsStringAsync().Result;

				Completed?.Invoke(this, new ConnectorCompletedEventArgs(response.ReasonPhrase, response));

				return result;
			}
		}

		public IHttpConnectable CancelCall()
		{
			client?.CancelPendingRequests();
			return this;
		}

		// -- helper

		private void OnCompleted(object sender, ConnectorCompletedEventArgs e)
		{
			enableCaching = false;
		}

		private bool Validate()
		{
			var err = "The http conncetor is not ready.";
			if (client is null)
			{
				log.Error($"{err} The client is null.");
				return false;
			}

			if (endpoint is null)
			{
				log.Error($"{err} The endpoint configuration is null.");
				return false;
			}

			return true;
		}


	}
}