namespace TeleScope.Connectors.Plc.Siemens.Extensions
{
	static class IntegerExtensions
	{
		public static int ToBits(this int number)
		{
			return number * 8;
		}
	}
}
