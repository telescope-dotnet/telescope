using System.IO;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;

namespace TeleScope.Persistence.Parquet
{
	/// <summary>
	/// This storage setup extends <seealso cref="FileSetupBase"/> 
	/// to specify parameters for an access to Parquet files. 
	/// </summary>
	public class ParquetStorageSetup : FileSetupBase
	{
		// -- properties

		// -- constructors

		/// <summary>
		/// The constructor calls the according base class constructor and 
		/// leaves the default settings of the property.
		/// </summary>
		/// <param name="file">The file represented as string, the storage is attached to.</param>
		/// <param name="permissions">The permission flag to access the file.</param>
		public ParquetStorageSetup(string file, 
			WritePermissions permissions = WritePermissions.Create) 
			: base(file, permissions)
		{

		}

		/// <summary>
		/// The constructor calls the according base class constructor and 
		/// leaves the default settings of the property. 
		/// </summary>
		/// <param name="fileInfo">The file information, the storage is attached to.</param>
		/// <param name="permissions">The permission flag to access the file.</param>
		public ParquetStorageSetup(FileInfo fileInfo, 
			WritePermissions permissions = WritePermissions.Create) 
			: base(fileInfo, permissions)
		{

		}
	}
}
