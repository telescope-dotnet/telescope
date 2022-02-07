namespace TeleScope.Persistence.Abstractions.Handlers
{
	public delegate Tout PersistenceHandler<in Tin, out Tout>(Tin item, int index = 0, int length = 1);
}
