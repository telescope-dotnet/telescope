using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.MSTest.Mockups
{
	internal class MockupToCsvParser : IParsable<string[]>
	{
		public string[] Parse<Tin>(Tin input, int index = 0, int length = 1)
		{
			var mockup = input as Mockup;

			return new string[] {
				mockup.Id.ToString(),
				mockup.Name,
				mockup.Greetings,
				mockup.Number.ToString(),
				mockup.Timestamp.ToString(),
			};
		}
	}
}
