using System.IO;
using System.Text;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Yaml
{
	public class YamlStorageSetup : FileSetupBase
	{
		public Encoding Encoder { get; set; }

		public YamlStorageSetup(
			FileInfo fileInfo, 
			bool canCreate = true,
			bool canDelete = false) : base(fileInfo, canCreate, canDelete)
		{

			Encoder = Encoding.UTF8;
		}
	}
}