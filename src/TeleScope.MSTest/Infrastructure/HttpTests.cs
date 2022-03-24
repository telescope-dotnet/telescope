using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TeleScope.Connectors.Http;
using TeleScope.Connectors.Http.Abstractions;
using TeleScope.Logging.Extensions;

namespace TeleScope.MSTest.Infrastructure
{
	[TestClass]
	public class HttpTests : TestsBase
	{
		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- Test methods

		[TestMethod]
		public void CancelHttpRequest() 
		{
			
		}

		[TestMethod]
		public async Task CallAsync_GetRequest()
		{
			// arrange
			var request = "/api/users";
			var endpoint = new HttpEndpoint(
				new Uri(new Uri("https://reqres.in"), request),
				HttpMethod.Get);

			endpoint.Request("");
			Assert.IsFalse(endpoint.Address.AbsoluteUri.Contains(request));
			endpoint.Request(request);
			Assert.IsTrue(endpoint.Address.AbsoluteUri.Contains(request));

			var http = GetHttpConnector(endpoint);

			// act
			var result = await http.CallAsync();

			// assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task CallAsync_GraphQl()
		{
			// arrange

			/*
			 * A public GraphQL API with information about continents and countries
			 * https://countries.trevorblades.com/
			 * Found at https://github.com/APIs-guru/graphql-apis
			 */
			var query = "?query={continents{name%20countries{name}}}";
			var endpoint = new HttpEndpoint(
				new Uri(new Uri("https://countries.trevorblades.com/"), query),
				HttpMethod.Get);

			var http = GetHttpConnector(endpoint);

			// act
			var result = await http
				.CallAsync();

			// assert
			log.Trace(result);
			Assert.IsFalse(result.Contains("error"), "No error should be contained in the response message.");
			Assert.IsTrue(result.Contains("continents"), "No continent data received.");
		}

		[TestMethod]
		public async Task GraphQlPostRequest()
		{
			// arrange
			var query = "{ \"query\": \"{ continents {name countries { name }}}\"}";
			var endpoint = new HttpEndpoint(
				new Uri("https://countries.trevorblades.com/"),
				HttpMethod.Post);

			var http = GetHttpConnector(endpoint);

			// act
			var result = await http
				.SetContent(query)
				.CallAsync<Response<Data>>((s) =>
				{
					log.Trace(s);
					return JsonConvert.DeserializeObject<Response<Data>>(s);
				});

			// assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Data.Continents);
			Assert.IsTrue(result.Data.Continents.Count > 0);
		}

		[TestMethod]
		public async Task CallAsync_Multiple_Endpoints_ShouldFail()
		{
			/*
			 * List of public test APIs: https://api.publicapis.org/entries
			 */

			// arrange
			Exception exception = null;
			bool catched = false;
			var endpointOne = new HttpEndpoint(new Uri("https://reqres.in/api/users"));
			var endpointTwo = new HttpEndpoint(new Uri("https://reqres.in/api/users/2"));
			var endpointThree = new HttpEndpoint(new Uri("https://api.publicapis.org/entries"));

			var http = GetHttpConnector(endpointOne);

			// act
			try
			{
				var task1 = http.CallAsync((s) => {
					log.Trace(s);
					return s;
				});

				var task2 = http.Connect(endpointTwo).CallAsync((s) => {
					log.Trace(s);
					return s;
				});

				var task3 = http.Connect(endpointThree).CallAsync((s) => {
					log.Trace(s);
					return s;
				});

				Task.WaitAll(task1, task2, task3);
			}
			catch (Exception ex)
			{
				exception = ex;
				catched = true;
			}
			finally 
			{
				// assert
				Assert.IsTrue(catched);
				Assert.IsTrue(exception is InvalidOperationException,
					$"Wrong Exception type catched. Expected type {nameof(InvalidOperationException)}, but was {exception.GetType()}");
			}
		}

		// -- helper

		private IHttpConnectable GetHttpConnector(HttpEndpoint endpoint)
		{
			var http = new HttpConnector(new HttpClient(), endpoint);
			http.Connected += (o, e) =>
			{
				var message = e.Name;
				log.Info($"Http connection successful: {message}");
			};
			http.Failed += (o, e) =>
			{
				log.Error(e.Exception, $"Http failed: {e.Message}");
			};

			try
			{
				http.Connect();
			}
			catch (WebException wex)
			{
				log.Critical(wex);
			}

			return http;
		}

		// -- inner classes

		class Response<T>
		{
			public T Data { get; set; } = default;
		}

		class Data
		{
			public List<Continent> Continents { get; set; } = default;

			public class Continent
			{
				public string Name { get; set; } = default;

				public List<Country> Countries { get; set; } = default;

				public class Country
				{
					public string Name { get; set; } = default;
				}
			}
		}
	}
}
