using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Json
{
	public class JsonStorageSetup : FileSetupBase
	{
		public JsonStorageSetup(
			string file,
			bool canCreate = true,
			bool canDelete = true) : base(file, canCreate, canDelete)
		{
		}
	}
}