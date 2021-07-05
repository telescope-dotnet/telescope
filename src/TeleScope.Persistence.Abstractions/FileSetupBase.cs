﻿using System;
using System.IO;

namespace TeleScope.Persistence.Abstractions
{
	/// <summary>
	/// This abstract base class provides properties and 
	/// a default constructor signature for concrete file setup classes.
	/// </summary>
	public abstract class FileSetupBase
	{
		// -- fields

		/// <summary>
		/// The default behavior for file storage implementations, if it is allowed to create files or not.
		/// </summary>
		protected const bool DEFAULT_CAN_CREATE = true;

		/// <summary>
		/// The default behavior for file storage implementations, if it is allowed to delete files or not.
		/// </summary>
		protected const bool DEFAULT_CAN_DELETE = true;

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
		/// Gets the information, if the setup provides the ability to create files. 
		/// </summary>
		public bool CanCreate { get; private set; }

		/// <summary>
		/// Gets the information, if the setup provides the ability to delete files. 
		/// </summary>
		public bool CanDelete { get; private set; }

		// -- constructors

		/// <summary>
		/// The default constructor sets the file info propterties and <seealso cref="CanCreate"/> and <seealso cref="CanDelete"/>. 
		/// </summary>
		/// <param name="fileInfo">The information about the file that will get accessed by a file storage.</param>
		/// <param name="canCreate">Sets the information, if the setup provides the ability to create files.</param>
		/// <param name="canDelete">Sets the information, if the setup provides the ability to delete files.</param>
		protected FileSetupBase(
			FileInfo fileInfo,
			bool canCreate = DEFAULT_CAN_CREATE,
			bool canDelete = DEFAULT_CAN_DELETE)
		{
			SetFileInfo(info);
			CanCreate = canCreate;
			CanDelete = canDelete;
		}

		// -- methods

		/// <summary>
		/// Gets the reference to the FileInfo object, that was given to the constructor. 
		/// </summary>
		/// <returns></returns>
		public FileInfo GetFileInfo()
		{
			return info;
		}

		/// <summary>
		/// Sets or updates the reference to the FileInfo object. 
		/// </summary>
		/// <param name="fileInfo">The new FileInfo object.</param>
		public void SetFileInfo(FileInfo fileInfo)
		{
			info = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
		}
	}
}
