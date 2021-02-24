using System.Text;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Json
{
	public class JsonStorageSetup : FileSetupBase
	{
		public Encoding Encoder { get; set; }

		public JsonStorageSetup(
			string file,
			bool canCreate = true,
			bool canDelete = true) : base(file, canCreate, canDelete)
		{
			Encoder = Encoding.UTF8;
		}
	}
}