using System;
using System.IO;
using TeleScope.Persistence.Abstractions.Enumerations;

namespace TeleScope.Persistence.Abstractions
{
	/// <summary>
	/// This abstract base class provides properties and 
	/// a default constructor signature for concrete file setup classes.
	/// </summary>
	public abstract class FileSetupBase
	{
		// -- fields

		private FileInfo info;

		// -- properties

		/// <summary>
		/// Gets the complete filenmane with path and the extension of the file. 
		/// </summary>
		public string File => info.FullName;

		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		public string Filename => info.Name;

		/// <summary>
		/// Gets the extension or type of the file.
		/// </summary>
		public string Extension => info.Extension;

		/// <summary>
		/// Gets the complete path, where the file is located. 
		/// </summary>
		public string Location => info.Directory.FullName;

		/// <summary>
		/// Gets the information if the file info instance exists or not.
		/// </summary>
		public bool Exists => info.Exists;

		/// <summary>
		/// Gets the flags of permissions how files may be treated. 
		/// </summary>
		public WritePermissions Permissions { get; set; }

		// -- constructors

		/// <summary>
		/// The constructor sets the file propterties and the <see cref="WritePermissions"/>. 
		/// </summary>
		/// <param name="file">The file that will get accessed by a file storage.</param>
		/// <param name="permissions">The information about the write permissions.</param>
		protected FileSetupBase(string file, 
			WritePermissions permissions = WritePermissions.Create | WritePermissions.Delete) 
			: this(permissions)
		{
			SetFile(new FileInfo(file));
		}

		/// <summary>
		/// The constructor sets the file propterties and the <see cref="WritePermissions"/>.
		/// </summary>
		/// <param name="fileInfo">The information about the file that will get accessed by a file storage.</param>
		/// <param name="permissions">The information about the write permissions.</param>
		protected FileSetupBase(FileInfo fileInfo, 
			WritePermissions permissions = WritePermissions.Create | WritePermissions.Delete) 
			: this(permissions)
		{
			SetFile(fileInfo);
		}

		private FileSetupBase(WritePermissions permissions)
		{
			Permissions = permissions;
		}

		// -- methods

		/// <summary>
		/// Gets the reference to the FileInfo object, that was given to the constructor. 
		/// </summary>
		/// <returns></returns>
		public FileInfo Info()
		{
			return info;
		}

		/// <summary>
		/// Sets or updates the reference to the FileInfo object. 
		/// </summary>
		/// <param name="fileInfo">The new FileInfo object.</param>
		public void SetFile(FileInfo fileInfo)
		{
			_ = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
			info = fileInfo;
		}
	}
}
