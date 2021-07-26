using System.IO;
using System.Net.Mime;
using TeleScope.Connectors.Abstractions;

namespace TeleScope.Connectors.Smtp.Abstractions
{
	/// <summary>
	/// This interface provides extended methods in addition to the <seealso cref="IConnectable"/> interface.
	/// It supprots SMTP connections to send emails with a minimal interface. 
	/// </summary>
	public interface ISmtpConnectable : IConnectable
	{
		// -- methods

		/// <summary>
		/// This method should add a new message to an internal collection 
		/// with only specific properties, needed to build a new email object. 
		/// Sender and receivers should be known by the implementing instace beforehand.
		/// </summary>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="body">The message of the email.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable NewMessage(string subject, string body);

		/// <summary>
		/// This method should add a new message to an internal collection 
		/// with the essetial properties, needed to build a new email object.
		/// </summary>
		/// <param name="from">The email address of the sender.</param>
		/// <param name="to">The email address of the receiver.</param>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="body">The message of the email.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable NewMessage(string from, string to, string subject, string body);

		/// <summary>
		/// This method should add a new message to an internal collection 
		/// with the essetial properties needed to build an email object.
		/// </summary>
		/// <param name="from">The email address of the sender.</param>
		/// <param name="to">The email addresses of the receivers.</param>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="body">The message of the email.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable NewMessage(string from, string[] to, string subject, string body);

		/// <summary>
		/// This method should add a cc (carbon body) address to the last email 
		/// that was created by the last call of `NewMessage`.
		/// </summary>
		/// <param name="to">The email address of the receiver.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable CarbonCopy(string to);

		/// <summary>
		/// This method should add a cc (carbon body) address to the last email 
		/// that was created by the last call of `NewMessage`.
		/// </summary>
		/// <param name="to">The email addresses of the receivers.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable CarbonCopy(string[] to);

		/// <summary>
		/// This method should add a bcc (blind carbon body) address to the last email 
		/// that was created by the last call of `NewMessage`.
		/// </summary>
		/// <param name="to">The email address of the receiver.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable BlindCarbonCopy(string to);

		/// <summary>
		/// This method should add a bcc (blind carbon body) address to the last email 
		/// that was created by the last call of `NewMessage`.
		/// </summary>
		/// <param name="to">The email addresses of the receivers.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable BlindCarbonCopy(string[] to);

		/// <summary>
		/// This method should add a high priority flag to the latest email.
		/// </summary>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable HighPriority();

		/// <summary>
		/// This method should add a low priority flag to the latest email.
		/// </summary>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable LowPriority();

		/// <summary>
		/// This method should attach a file to the latest email.
		/// </summary>
		/// <param name="file">The file info object that will be attached.</param>
		/// <param name="mimeType">The mime type of the file.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		ISmtpConnectable Attach(FileInfo file, string mimeType = MediaTypeNames.Text.Plain);

		/// <summary>
		/// This method should send all the created and configured emails at once and 
		/// clear its inner stack.
		/// </summary>
		/// <returns>Returns a result triple that contains the total amount of sent emails, 
		/// the sucessful and the failed ones.</returns>
		(int total, int success, int failed) Send();
	}
}
