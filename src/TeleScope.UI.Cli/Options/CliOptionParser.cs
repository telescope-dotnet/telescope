using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TeleScope.UI.Cli.Options
{
	public class CliOptionParser<T> where T : new()
	{
		// --fields

		private readonly T _options;

		// -- properties

		/// <summary>
		/// Gets or sets the Prefix that is used to identify options.
		/// </summary>
		public string Prefix { get; set; }

		public IFormatProvider Format { get; set; }

		// -- constructor

		public CliOptionParser()
		{
			_options = new T();
			Prefix = "-";
			Format = CultureInfo.InvariantCulture;
		}

		public CliOptionParser(string prefix) : this()
		{
			Prefix = prefix;
		}

		public CliOptionParser(IFormatProvider format) : this()
		{
			Format = format;
		}

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
			return _options;
		}

		private void SetOption(string arg, int index, string[] args)
		{
			arg = Regex.Replace(arg, @"[^0-9a-zA-Z]+", "");

			foreach (var prop in _options.GetType().GetProperties())
			{
				var attr = prop.GetCustomAttributes<CliAttribute>().First();

				if (!attr.Short.Equals(arg) && !attr.Long.Equals(arg))
				{
					continue;
				}

				if (prop.PropertyType == typeof(bool))
				{
					prop.SetValue(_options, true);
				}
				else
				{
					var val = args.Length > index + 1 ? args[index + 1] : string.Empty;
					prop.SetValue(_options, Convert.ChangeType(val, prop.PropertyType, Format));
				}
			}
		}
	}
}
