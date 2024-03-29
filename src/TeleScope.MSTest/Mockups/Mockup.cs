﻿using System;
using System.Linq;

namespace TeleScope.MSTest.Mockups
{
	public class Mockup : IMockable
	{
		// -- static fields

		private static Random random;

		private static Random MockupRandom
		{
			get
			{
				if (random is null)
				{
					random = new Random();
				}
				return random;
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
			Name = this.GetType().Name;
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
