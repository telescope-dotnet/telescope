using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
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
		/*
		 * List of public test APIs: https://api.publicapis.org/entries
		 * A public GraphQL API with information about continents and countries
		 * https://countries.trevorblades.com/ found at https://github.com/APIs-guru/graphql-apis
		 */

		// -- fields

		private static bool skip;

		/// <summary>
		/// Initialize method that runs once for the entire class.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			/*
			 * add the "local.runsettings" as global runsettings file, if context has no data.
			 */
			skip = bool.Parse(GetProperty(context, SKIP_HTTP_TESTS));
		}


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
		public async Task CallAsync_GetRequest()
		{
			if (skip)
			{
				Console.WriteLine($"Skipping {nameof(HttpTests)} test...");
				return;
			}

			// arrange
			var request = "/api/users";
			var endpoint = new HttpEndpoint(
				new Uri(new Uri("https://reqres.in"), request),
				HttpMethod.Get);

			endpoint.SetRequest("");
			Assert.IsFalse(endpoint.Address.AbsoluteUri.Contains(request));
			endpoint.SetRequest(request);
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
			if (skip)
			{
				Console.WriteLine($"Skipping {nameof(HttpTests)} test...");
				return;
			}

			// arrange
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
			if (skip)
			{
				Console.WriteLine($"Skipping {nameof(HttpTests)} test...");
				return;
			}

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
			if (skip)
			{
				Console.WriteLine($"Skipping {nameof(HttpTests)} test...");
				return;
			}

			// arrange
			Exception exception = null;
			bool catched = false;
			var endpointOne = new HttpEndpoint(new Uri("https://reqres.in/api/users"));
			var endpointTwo = new HttpEndpoint(new Uri("https://reqres.in/api/users/2"));
			var endpointThree = new HttpEndpoint(new Uri("https://countries.trevorblades.com/"));

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
					$"Wrong Exception type catched. Expected type {nameof(InvalidOperationException)}, but was {exception.GetType()}.");
			}
		}

		[TestMethod]
		public async Task CallAsync_Multiple_Endpoints()
		{
			if (skip)
			{
				Console.WriteLine($"Skipping {nameof(HttpTests)} test...");
				return;
			}

			// arrange
			var endpointOne = new HttpEndpoint(new Uri("https://reqres.in/api/users/1"));
			var endpointTwo = new HttpEndpoint(new Uri("https://reqres.in/api/users/2"));
			var endpointThree = new HttpEndpoint(new Uri("https://countries.trevorblades.com/"));

			var resultOne = string.Empty;
			var resultTwo = string.Empty;
			var resultThree = string.Empty;

			Exception exception = null;
			bool catched = false;

			var http = GetHttpConnector(endpointOne);

			// act
			try
			{
				var task1 = http.SetRequest(endpointOne).CallAsync((s) => {
					log.Trace(s);
					return s;
				});

				var task2 = http.SetRequest(endpointTwo).CallAsync((s) => {
					log.Trace(s);
					return s;
				});

				var task3 = http.SetRequest(endpointThree).CallAsync((s) => {
					log.Trace(s);
					return s;
				});

				await Task.WhenAll(task1, task2, task3);

				resultOne = task1.Result;
				resultTwo = task2.Result;
				resultThree = task3.Result;
			}
			catch (Exception ex)
			{
				exception = ex;
				catched = true;
			}
			finally
			{
				// assert
				Assert.IsFalse(catched);
				Assert.IsTrue(exception is null, "Exception should be null.");
				Assert.IsFalse(string.IsNullOrEmpty(resultOne));
				Assert.IsFalse(string.IsNullOrEmpty(resultTwo));
				Assert.IsFalse(string.IsNullOrEmpty(resultTwo));
				Assert.AreNotEqual(resultOne, resultTwo);
				Assert.AreNotEqual(resultOne, resultThree);
				Assert.AreNotEqual(resultTwo, resultThree);
			}
		}

		[TestMethod]
		public async Task CallAsync_WithCaching() 
		{
			if (skip)
			{
				Console.WriteLine($"Skipping {nameof(HttpTests)} test...");
				return;
			}

			// arrange
			var endpoint = new HttpEndpoint(new Uri("https://countries.trevorblades.com/"));
			var http = GetHttpConnector(endpoint);
			
			long millisOne = 0;
			long millisTwo = 0;

			// act
			http.WithCaching(2, 4);

			var watch = Stopwatch.StartNew();
			var resultOne = await http.CallAsync((s) =>
			{
				log.Trace(s);
				watch.Stop();
				millisOne = watch.ElapsedMilliseconds;
				return s;
			});

			watch.Restart();
			var resultTwo = await http.CallAsync((s) =>
			{
				log.Trace(s);
				watch.Stop();
				millisTwo = watch.ElapsedMilliseconds;
				return s;
			});

			// assert
			Assert.IsTrue(millisOne > 0);
			Assert.IsTrue(millisTwo > 0);
			Assert.IsTrue((millisOne / 2) > millisTwo);
			Assert.IsTrue(millisTwo < 100);
			Assert.AreEqual(resultOne, resultTwo);
		}

		[TestMethod]
		public void CallAsync_With_CancellationToken()
		{
			if (skip)
			{
				Console.WriteLine($"Skipping {nameof(HttpTests)} test...");
				return;
			}

			// arrange
			var response = string.Empty;
			var taskResult = string.Empty;
			var tokenSource = new CancellationTokenSource();

			var endpoint = new HttpEndpoint(new Uri("https://api.publicapis.org/entries"));
			var http = GetHttpConnector(endpoint);
			http.Completed += (o, e) =>
			{
				response = e.Response.ToString();
			};			

			// act
			var task = http.AddCancelToken(tokenSource.Token).CallAsync((s) =>
			{
				log.Trace(s);
				return s;
			});

			tokenSource.Cancel();
			taskResult = task.Result;


			// assert
			Assert.IsTrue(string.IsNullOrEmpty(taskResult));
			Assert.IsTrue(!string.IsNullOrEmpty(response));
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
