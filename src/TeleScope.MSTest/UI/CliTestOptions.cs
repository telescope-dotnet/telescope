using TeleScope.UI.Cli.Options;

namespace TeleScope.MSTest.UI
{
	class CliTestOptions
	{
		public CliTestOptions()
		{
			Path = string.Empty;
		}

		[Cli(Long = "verbose", Short = "v")]
		public bool Verbose { get; set; }

		[Cli(Long = "path", Short = "p")]
		public string Path { get; set; }

		[Cli(Long = "on", Short = "o")]
		public bool OnOff { get; set; }

		[Cli(Long = "counter", Short = "c")]
		public int Counter { get; set; }

		[Cli(Long = "floating", Short = "f")]
		public float Floating { get; set; }

		[Cli(Long = "double", Short = "d")]
		public double Double { get; set; }
	}
}
