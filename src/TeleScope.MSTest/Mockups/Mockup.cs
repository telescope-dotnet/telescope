using System;
using System.Linq;

namespace TeleScope.MSTest.Mockups
{
	public class Mockup
	{
		// -- static fields

		private static Random _random;

		private static Random MockupRandom
		{
			get
			{
				if (_random == null)
				{
					_random = new Random();
				}
				return _random;
			}
		}

		// -- instance fields & properties

		public int Id { get; set; }
		public string Name { get; set; }

		public string Greetings { get; set; }

		public double Number { get; set; }

		public DateTime Timestamp { get; set; }

		// -- constructor

		public Mockup()
		{
			Greetings = "Hello Mockup";
			Timestamp = DateTime.Now;
		}

		// -- static methods

		public static Mockup Random(int id = 0)
		{
			return new Mockup
			{
				Id = id,
				Name = $"Mockup no. {id}",
				Greetings = $"Hello from {id}",
				Number = MockupRandom.NextDouble()
			}; 
		}

		public static Mockup[] RandomArray(int length)
		{
			return Enumerable
				.Range(0, length)
				.Select(i => Random(i))
				.ToArray();
		}
	}
}
