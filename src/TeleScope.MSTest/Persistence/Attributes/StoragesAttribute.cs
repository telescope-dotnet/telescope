﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.MSTest.Mockups;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Csv;
using TeleScope.Persistence.Json;
using TeleScope.Persistence.Parquet;
using TeleScope.Persistence.Yaml;

namespace TeleScope.MSTest.Persistence.Attributes
{
	class StoragesAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var list = new List<object[]>();

            // add object array as parameters for TestMethods
            list.Add(BuildJson());
            list.Add(BuildYaml());
            list.Add(BuildCsv());
            list.Add(BuildParquet());
            return list;
        }

        // -- helper 

        private string GetFile(string extension)
		{
            return Path.Combine("App_Data", $"data.{extension}");
        }

        private object[] BuildJson()
		{
            var ext = "json";
            var file = GetFile(ext);
            var json = new JsonStorage<Mockup>(file, true, true);
            return new object[] { 
                ext,
                file,
                json as IReadable<Mockup>,
                json as IWritable<Mockup>,
            };
		}

        private object[] BuildYaml()
        {
            var ext = "yml";
            var file = GetFile(ext);
            var yaml = new YamlStorage<Mockup>(file, true, true);
            return new object[] {
                ext,
                file,
                yaml as IReadable<Mockup>,
                yaml as IWritable<Mockup>,
            };
        }

        private object[] BuildCsv()
        {
            var ext = "csv";
            var file = GetFile(ext);
            var setup = new CsvStorageSetup(
                csvFileInfo: new FileInfo(file),
                startRow: 2,
                header: "This is my awesome\r\nHEADER"
            );

            var csv = new CsvStorage<Mockup>(
                setup, new CsvToMockupParser(), new MockupToCsvParser());
            return new object[] {
                ext,
                file,
                csv as IReadable<Mockup>,
                csv as IWritable<Mockup>,
            };
        }

        private object[] BuildParquet()
        {
            var ext = "parquet";
            var file = GetFile(ext);

            var parquet = new ParquetStorage<Mockup>(file, true, true);
            return new object[] {
                ext,
                file,
                parquet as IReadable<Mockup>,
                parquet as IWritable<Mockup>,
            };
        }

        // copy this method for other TestDataSource implementations
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data != null)
            {
                return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", methodInfo.Name, string.Join(",", data));
            }
            return null;
        }
    }
}
