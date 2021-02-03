namespace TeleScope.Persistence.Abstractions
{
	public interface IParsable<out Tout>
	{
		Tout Parse<Tin>(Tin input, int index = 0, int length = 1);
	}
}