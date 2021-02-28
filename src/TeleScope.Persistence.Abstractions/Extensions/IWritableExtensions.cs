using System;
using System.Collections.Generic;
using System.IO;

namespace TeleScope.Persistence.Abstractions.Extensions
{
	public static class IWritableExtensions
	{
		// -- properties

		/// <summary>
		/// Validates the permissions to create and delete of the calling instance and returns a true if the process should continue or false if not.
		/// In case that the permissions were violated an InvalidOperationException is thrown.
		/// </summary>
		/// <typeparam name="T">The data type of the data.</typeparam>
		/// <param name="writer">The calling instance.</param>
		/// <param name="data">The data of the calling instance.</param>
		/// <param name="info">The file informations that are used to delete or create files.</param>
		/// <returns></returns>
		public static bool ValidateOrThrow<T>(this IWritable<T> writer, IEnumerable<T> data, FileInfo info)
		{
			if (data == null && info.Exists)
			{
				if (writer.CanDelete)
				{
					info.Delete();
					return false;
				}
				else
				{
					throw new InvalidOperationException($"Not allowed to delete file: {info.FullName}");
				}
			}
			else if (!writer.CanCreate && !info.Exists)
			{
				throw new InvalidOperationException($"Not allowed to create file: {info.FullName}");
			}
			else if (writer.CanCreate && !info.Directory.Exists)
			{
				info.Directory.Create();
			}

			return true;
		}
	}
}
