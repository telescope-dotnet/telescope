using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TeleScope.UI.Cli.Options
{
	/// <summary>
	/// This class provides a routine to put application arguments into properties of an object of type T,
	/// where matching <seealso cref="CliAttribute"/> are applied to the properties.
	/// </summary>
	/// <typeparam name="T">The type of the arguments class.</typeparam>
	public class CliOptionParser<T> where T : new()
	{
		// --fields

		private readonly T options;

		// -- properties

		/// <summary>
		/// Gets or sets the Prefix that is used to identify options.
		/// </summary>
		public string Prefix { get; set; }

		/// <summary>
		/// Gets or sets the format how the strings are parsed into types of the target properties.
		/// </summary>
		public IFormatProvider Format { get; set; }

		// -- constructor

		/// <summary>
		/// The default constructor creates a new instance of the options and sets the prefix to `-`.
		/// The format is set to `InvariantCulture`.
		/// </summary>
		public CliOptionParser()
		{
			options = new T();
			Prefix = "-";
			Format = CultureInfo.InvariantCulture;
		}

		/// <summary>
		/// The constructor calls the default constructor and overrides the <seealso cref="Prefix"/> property.
		/// </summary>
		/// <param name="prefix">The prefix that is used for identifying application arguments in the string array.</param>
		public CliOptionParser(string prefix) : this()
		{
			Prefix = prefix;
		}

		/// <summary>
		/// The constructor calls the default constructor and overrides the <seealso cref="Format"/> property.
		/// </summary>
		/// <param name="format">The format specifies how the strings are parsed into types of the target properties.</param>
		public CliOptionParser(IFormatProvider format) : this()
		{
			Format = format;
		}

		/// <summary>
		/// he constructor calls the default constructor and overrides the <seealso cref="Prefix"/> and <seealso cref="Format"/> properties.
		/// </summary>
		/// <param name="prefix">The prefix that is used for identifying application arguments in the string array.</param>
		/// <param name="format">The format specifies how the strings are parsed into types of the target properties.</param>
		public CliOptionParser(string prefix, IFormatProvider format) : this()
		{
			Prefix = prefix;
			Format = format;
		}

		// -- methods

		/// <summary>
		/// Reads the command line arguments and places them into the properties of the according instance of type T.
		/// </summary>
		/// <param name="args">The cli options of the main method.</param>
		/// <returns>The resulting instance of type T.</returns>
		public T ReadArguments(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				var arg = args[i];

				if (!arg.StartsWith(Prefix))
				{
					continue;
				}

				SetOption(arg, i, args);
			}
			return options;
		}

		// -- helper

		private void SetOption(string arg, int index, string[] args)
		{
			arg = Regex.Replace(arg, @"[^0-9a-zA-Z]+", "");

			foreach (var prop in options.GetType().GetProperties())
			{
				var attr = prop.GetCustomAttributes<CliAttribute>().FirstOrDefault();
				if (attr is null || IsMismatch(attr, arg))
				{
					continue;
				}

				SetProperty(index, args, prop);
			}
		}

		private bool IsMismatch(CliAttribute attr, string arg)
		{
			return !attr.Short.Equals(arg) && !attr.Long.Equals(arg);
		}

		private void SetProperty(int index, string[] args, PropertyInfo prop)
		{
			if (prop.PropertyType == typeof(bool))
			{
				prop.SetValue(options, true);
			}
			else
			{
				var val = args.Length > index + 1 ? args[index + 1] : string.Empty;
				prop.SetValue(options, Convert.ChangeType(val, prop.PropertyType, Format));
			}
		}
	}
}
