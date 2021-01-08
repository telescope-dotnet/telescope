using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Csv
{
	public class CsvStorage : IReadable, IWritable
	{

		// -- fields

		private ILogger _log;
		private CsvStorageSetup _setup;

		private DataTable _table;

		// -- properties

		public bool CanCreate => throw new NotImplementedException();

		public bool CanDelete => throw new NotImplementedException();



		public CsvStorage(CsvStorageSetup setup)
		{
			_log = LoggingProvider.CreateLogger<CsvStorage>();
			_setup = setup;

		}

		public IReadable Read()
		{

			try
			{
				_table = new DataTable();
				_table.TableName = _setup.Filename;

				string[] lines = File.ReadAllLines(_setup.File);

				string[] fields;

				// TODO: find a solution for files without header

				/*
				 * prepare table
				 */
				_table.Columns.AddRange(
					lines[0]		// read header at line 0, this must be present currently
					.ToLower()
					.Split(new char[] { _setup.Separator })
					.Select(s => new DataColumn(s))
					.ToArray());


				/*
				 * read all rows
				 */
				DataRow row;
				for (int i = 1; i < lines.GetLength(0); i++)
				{
					fields = lines[i].Split(new char[] { _setup.Separator });
					row = _table.NewRow();
					for (int f = 0; f < fields.Length; f++)
					{
						row[f] = fields[f];

					}
					_table.Rows.Add(row);
				}

				_log.Trace($"csv import successfull for '{_setup.Filename}'");
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{

			}
			
			return this;
		}

		public T As<T>()
		{
			throw new NotImplementedException();
		}

		public void Write<T>(T data)
		{
			throw new NotImplementedException();
		}

		
	}
}
