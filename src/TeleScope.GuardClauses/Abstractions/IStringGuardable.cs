namespace TeleScope.GuardClauses.Abstractions
{
	public interface IStringGuardable
	{
		string IsNotNullOrEmpty(string input, string paramName = null, string message = null);
		string IsNotNullOrWhiteSpace(string input, string paramName = null, string message = null);
	}
}
