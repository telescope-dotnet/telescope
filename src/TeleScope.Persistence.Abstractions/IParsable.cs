namespace TeleScope.Persistence.Abstractions
{
	public interface IParsable<out Tout>
	{
		Tout Parse<Tin>(Tin input, int total = 1);
	}
}