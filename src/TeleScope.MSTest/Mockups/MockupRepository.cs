using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.MSTest.Mockups
{
	internal class MockupRepository
	{
		public List<IMockable> Mockups { get; set; }

		public MockupRepository()
		{
			Mockups = new List<IMockable>();
		}
	}
}
