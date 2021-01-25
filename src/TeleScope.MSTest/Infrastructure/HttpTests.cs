using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
		public async Task RequestResponse()
		{
			// arrange
			var endpoint = new HttpEndpoint
			{
				Address = new Uri(new Uri("https://reqres.in"), "/api/users"),
				MethodName = "get"
			};
			var http = GetHttpConnector(endpoint);


			// act
			var result = await http.CallAsync();

			// assert
			Assert.IsNotNull(result);
		}

		// -- helper

		private IHttpConnectable GetHttpConnector(HttpEndpoint endpoint)
		{
			var http = new HttpConnector(new HttpClient(), endpoint);
			http.Connected += (o, e) =>
			{
				var message = e.Name;
				_log.Info($"Http connection successful: {message}");
				Assert.IsTrue(http.IsConnected, "The http connection should be okay");
			};
			http.Failed += (o, e) =>
			{
				_log.Error(e.Exception, $"Http failed: {e.Message}");
				Assert.Fail(e.Message);
			};

			var connected = http.Connect().IsConnected;
			Assert.IsTrue(connected, "The connection should be good");

			return http;
		}


	}
}
