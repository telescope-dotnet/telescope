using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TeleScope.Connectors.Plc.Siemens
{
	/// <summary>
	/// This static class contains all SIEMENS S7 result or error codes and
	/// makes them accessible as integer or as a string via the GetString method. 
	/// </summary>
	public static class S7Results
	{
		// -- fields

		/*
		 * ATTENTION: Do not add any other public constant fields except known error codes! 
		 */
		public const int NoError = 0x00000000;
		public const int TCPSocketCreation = 0x00000001;
		public const int TCPConnectionTimeout = 0x00000002;
		public const int TCPConnectionFailed = 0x00000003;
		public const int TCPReceiveTimeout = 0x00000004;
		public const int TCPDataReceive = 0x00000005;
		public const int TCPSendTimeout = 0x00000006;
		public const int TCPDataSend = 0x00000007;
		public const int TCPConnectionReset = 0x00000008;
		public const int TCPNotConnected = 0x00000009;
		public const int TCPUnreachableHost = 0x00002751;
		public const int IsoConnect = 0x00010000;
		public const int IsoInvalidPDU = 0x00030000;
		public const int IsoInvalidDataSize = 0x00040000;
		public const int CliNegotiatingPDU = 0x00100000;
		public const int CliInvalidParams = 0x00200000;
		public const int CliJobPending = 0x00300000;
		public const int CliTooManyItems = 0x00400000;
		public const int CliInvalidWordLen = 0x00500000;
		public const int CliPartialDataWritten = 0x00600000;
		public const int CliSizeOverPDU = 0x00700000;
		public const int CliInvalidPlcAnswer = 0x00800000;
		public const int CliAddressOutOfRange = 0x00900000;
		public const int CliInvalidTransportSize = 0x00A00000;
		public const int CliWriteDataSizeMismatch = 0x00B00000;
		public const int CliItemNotAvailable = 0x00C00000;
		public const int CliInvalidValue = 0x00D00000;
		public const int CliCannotStartPLC = 0x00E00000;
		public const int CliAlreadyRun = 0x00F00000;
		public const int CliCannotStopPLC = 0x01000000;
		public const int CliCannotCopyRamToRom = 0x01100000;
		public const int CliCannotCompress = 0x01200000;
		public const int CliAlreadyStop = 0x01300000;
		public const int CliFunNotAvailable = 0x01400000;
		public const int CliUploadSequenceFailed = 0x01500000;
		public const int CliInvalidDataSizeRecvd = 0x01600000;
		public const int CliInvalidBlockType = 0x01700000;
		public const int CliInvalidBlockNumber = 0x01800000;
		public const int CliInvalidBlockSize = 0x01900000;
		public const int CliNeedPassword = 0x01D00000;
		public const int CliInvalidPassword = 0x01E00000;
		public const int CliNoPasswordToSetOrClear = 0x01F00000;
		public const int CliJobTimeout = 0x02000000;
		public const int CliPartialDataRead = 0x02100000;
		public const int CliBufferTooSmall = 0x02200000;
		public const int CliFunctionRefused = 0x02300000;
		public const int CliDestroying = 0x02400000;
		public const int CliInvalidParamNumber = 0x02500000;
		public const int CliCannotChangeParam = 0x02600000;
		public const int CliFunctionNotImplemented = 0x02700000;

		private static List<FieldInfo> fields;

		// -- methods

		public static string GetString(int code)
		{
			var field = ReflectConstants().FirstOrDefault(f => (int)f.GetRawConstantValue() == code);
			return field != null ? field.Name : $"Unknown error code ({code}).";
		}

		// -- helper

		private static List<FieldInfo> ReflectConstants()
		{
			if (fields == null)
			{
				var type = typeof(S7Results);
				fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
					.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
			}

			return fields;
		}
	}
}
