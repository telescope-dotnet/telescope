﻿using System;
using System.IO;
using System.Text;

namespace TeleScope.Persistence.Abstractions
{
	public abstract class FileSetupBase
	{
		// -- fields

		protected const bool DEFAULT_CAN_CREATE = true;
		protected const bool DEFAULT_CAN_DELETE = true;

		private readonly FileInfo _info;

		// -- properties

		public string File => _info.FullName;

		public string Filename => _info.Name;

		public string Extension => _info.Extension;

		public string Location => _info.Directory.FullName;

		public bool CanCreate { get; private set; }

		public bool CanDelete { get; private set; }

		// -- constructors

		protected FileSetupBase(
			FileInfo fileInfo,
			bool canCreate = DEFAULT_CAN_CREATE,
			bool canDelete = DEFAULT_CAN_DELETE)
		{
			_info = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
			CanCreate = canCreate;
			CanDelete = canDelete;
		}

		// -- methods

		public FileInfo GetFileInfo()
		{
			return _info;
		}
	}
}
