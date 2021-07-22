using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleScope.MSTest.UI
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	class FooAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the minimum permission level.
		/// </summary>
		public int Bar { get; set; }
	}
}
