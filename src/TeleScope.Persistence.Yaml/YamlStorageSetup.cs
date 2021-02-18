using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Yaml
{
	public class YamlStorageSetup : FileSetupBase
	{
		public YamlStorageSetup(
			string file, 
			bool canCreate = true,
			bool canDelete = false) : base(file, canCreate, canDelete)
		{
		}
	}
}