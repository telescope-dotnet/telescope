using System.IO;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;

namespace TeleScope.Persistence.Parquet
{
	public class ParquetStorageSetup : FileSetupBase
	{
		// -- properties

		// -- constructors

		public ParquetStorageSetup(string file, WritePermissions permissions = WritePermissions.Create) : base(file, permissions)
		{

		}

		public ParquetStorageSetup(FileInfo fileInfo, WritePermissions permissions = WritePermissions.Create) : base(fileInfo, permissions)
		{

		}
	}
}
