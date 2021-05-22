using System.IO;
using System.Net.Mime;
using TeleScope.Connectors.Abstractions;

namespace TeleScope.Connectors.Smtp.Abstractions
{
	public interface ISmtpConnectable : IConnectable
	{
		// -- methods

		ISmtpConnectable NewMessage(string from, string to, string subject, string body);

		ISmtpConnectable NewMessage(string from, string[] to, string subject, string body);

		ISmtpConnectable CarbonCopy(string to);

		ISmtpConnectable CarbonCopy(string[] to);

		ISmtpConnectable BlindCarbonCopy(string to);

		ISmtpConnectable BlindCarbonCopy(string[] to);

		ISmtpConnectable HighPriority();

		ISmtpConnectable LowPriority();

		ISmtpConnectable Attach(FileInfo file, string mimeType = MediaTypeNames.Text.Plain);

		(int total, int success, int failed) Send();
	}
}
