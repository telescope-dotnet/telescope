using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeleScope.Connectors.Abstractions;

namespace TeleScope.Connectors.Http.Abstractions
{
	/// <summary>
	/// This interface provides extended methods, based on the `IConnectable` interface to build http connections. 
	/// </summary>
	public interface IHttpConnectable : IConnectable, IDisposable
	{
		/// <summary>
		/// Tests the connection to the given endpoint and stores the parameter internally. 
		/// The http client must be ready-to-use before calling this method.
		/// </summary>
		/// <param name="endpoint">The endpoint configuration.</param>
		/// <returns>The calling instance.</returns>
		IHttpConnectable Connect(HttpEndpoint endpoint);

		/// <summary>
		/// Tests the connection with the given http client and endpoint and stores both parameters internally.
		/// </summary>
		/// <param name="client">The http client that will be used by the connector.</param>
		/// <param name="endpoint">The endpoint configuration.</param>
		/// <returns>The calling instance.</returns>
		IHttpConnectable Connect(HttpClient client, HttpEndpoint endpoint);

		IHttpConnectable WithCaching();

		IHttpConnectable AddCancelToken(CancellationToken cancelToken);

		/// <summary>
		/// Updates the request part of the http endpoint configuration.
		/// </summary>
		/// <param name="request">The request part of the url.</param>
		/// <param name="method">Optional: The method type of the call.</param>
		/// <returns>The calling instance.</returns>
		IHttpConnectable SetRequest(string request, HttpMethod method = null);

		/// <summary>
		/// Adds an http header to the next request as simple pair of name and value.
		/// </summary>
		/// <param name="name">The name of the header information.</param>
		/// <param name="value">The value of the header information.</param>
		/// <returns>The calling instance.</returns>
		IHttpConnectable AddHeader(string name, string value);

		/// <summary>
		/// Sets the payload of the http request which must be represented as json compliant string.
		/// </summary>
		/// <param name="jsonContent">The payload for the next http request.</param>
		/// <returns>The calling instance.</returns>
		IHttpConnectable SetContent(string jsonContent);

		/// <summary>
		/// Sets the payload of the next http request.
		/// </summary>
		/// <param name="content">The payload as string.</param>
		/// <param name="encoding">The encoding to format the string before serialization.</param>
		/// <param name="mediatype">The media type of the payload.</param>
		/// <returns>The calling instance.</returns>
		IHttpConnectable SetContent(string content, Encoding encoding, string mediatype);

		/// <summary>
		/// Performs the http request asynchronously that is defined by the http endpoint and optional parameters.
		/// </summary>
		/// <typeparam name="T">The generic returned type T.</typeparam>
		/// <param name="convert">The function converts the response body into the generic type T.</param>
		/// <returns>The executing task whereby the result of the task is of type T.</returns>
		Task<T> CallAsync<T>(Func<string, T> convert);

		/// <summary>
		/// Performs the http request asynchronously that is defined by the http endpoint and optional parameters.
		/// </summary>
		/// <returns>The executing task whereby the result is the raw string of the response body.</returns>
		Task<string> CallAsync();

		IHttpConnectable CancelCall();
	}
}
