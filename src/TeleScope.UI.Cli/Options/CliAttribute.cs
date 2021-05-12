using System;

namespace TeleScope.UI.Cli.Options
{
	/// <summary>
	/// Attribute class to add names for command line options.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class CliAttribute : Attribute
	{
		// -- properties

		/// <summary>
		/// Gets or sets the short term of the command line option.
		/// Do not use any special characters, pre- or suffixes.
		/// </summary>
		public string Short { get; set; }

		/// <summary>
		/// Gets or sets the long term of the command line option.
		/// Do not use any special characters, pre- or suffixes.
		/// </summary>
		public string Long { get; set; }

		// -- constructors

		/// <summary>
		/// The default empty constructor.
		/// </summary>
		public CliAttribute()
		{

		}

		/// <summary>
		/// The constructor sets the properties Short and Long of the attribute. 
		/// </summary>
		/// <param name="shortTerm"></param>
		/// <param name="longTerm"></param>
		public CliAttribute(string shortTerm, string longTerm) : this()
		{
			Short = shortTerm;
			Long = longTerm;
		}		
	}
}
