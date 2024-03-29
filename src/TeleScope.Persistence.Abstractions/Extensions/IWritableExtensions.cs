﻿using System;
using System.Collections.Generic;
using System.IO;
using TeleScope.Persistence.Abstractions.Enumerations;

namespace TeleScope.Persistence.Abstractions.Extensions
{
	/// <summary>
	/// This extension class extends the <seealso cref="IWritable{T}"/> interface with common functions.
	/// It provides shared logic for implementations across the persistence layer. 
	/// </summary>
	public static class IWritableExtensions
	{
		// -- properties

		/// <summary>
		/// Validates the permissions to create and delete of the calling instance and returns a true if the process should continue or false if not.
		/// In case that the permissions were violated an <seealso cref="InvalidOperationException"/> is thrown.
		/// </summary>
		/// <typeparam name="T">The data type of the data.</typeparam>
		/// <param name="writer">The calling instance.</param>
		/// <param name="data">The data of the calling instance.</param>
		/// <param name="info">The file informations that are used to delete or create files.</param>
		/// <returns></returns>
		public static bool ValidateOrThrow<T>(this IWritable<T> writer, IEnumerable<T> data, FileInfo info)
		{
			if (data is null && info.Exists)
			{
				// deleting data...
				if (writer.HasPermission(WritePermissions.Delete))
				{
					info.Delete();
					return false;
				}
				else
				{
					throw new InvalidOperationException($"Not allowed to delete file: {info.FullName}");
				}
			}
			else if (!writer.HasPermission(WritePermissions.Create) && !info.Exists)
			{
				// fail to create a file...
				throw new InvalidOperationException($"Not allowed to create file: {info.FullName}");
			}
			else if (writer.HasPermission(WritePermissions.Create) && !info.Directory.Exists)
			{
				// creating folder...
				info.Directory.Create();
			}

			return true;
		}
	}
}
