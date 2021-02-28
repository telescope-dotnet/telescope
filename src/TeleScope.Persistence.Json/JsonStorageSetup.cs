using System.IO;
using System.Text;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Json
{
	public class JsonStorageSetup : FileSetupBase
	{
		public Encoding Encoder { get; set; }

		public JsonStorageSetup(
			FileInfo fileInfo,
			bool canCreate = true,
			bool canDelete = true) : base(fileInfo, canCreate, canDelete)
		{
			Encoder = Encoding.UTF8;
		}
	}
}