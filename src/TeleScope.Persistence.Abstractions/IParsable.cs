namespace TeleScope.Persistence.Abstractions
{
	public interface IParsable<Tout>
	{
		Tout Parse<Tin>(Tin input);
	}
}