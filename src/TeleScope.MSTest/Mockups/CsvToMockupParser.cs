using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.MSTest.Mockups
{
	internal class CsvToMockupParser : IParsable<Mockup>
	{
		public Mockup Parse<Tin>(Tin input, int index = 0, int length = 1)
		{
			string[] fields = input as string[];

			return new Mockup
			{
				Id = int.Parse(fields[0]),
				Name = fields[1],
				Greetings = fields[2],
				Number = double.Parse(fields[3]),
				Timestamp = DateTime.Parse(fields[4])
			};
		}
	}
}
