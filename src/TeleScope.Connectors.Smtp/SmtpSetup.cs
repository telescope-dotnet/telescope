using TeleScope.Connectors.Abstractions.Secrets;

namespace TeleScope.Connectors.Smtp
{
	/// <summary>
	/// This class encapsulates the settings for the <seealso cref="SmtpConnector"/> type.
	/// </summary>
	public class SmtpSetup
	{
		/// <summary>
		/// The default retry limit is `3` for attempts to send messages via Smtp.
		/// </summary>
		public const int RETRY_LIMIT = 3;

		// -- properties

		/// <summary>
		/// Gets or sets the host for the smtp connection.
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// Gets or set the sending email address.
		/// </summary>
		public string Sender { get; set; }

		/// <summary>
		/// Gets or sets the port for the smtp connection.
		/// 
		/// **Please note the default ports:**
		/// * Port 25 (non-secure) - this is the default port (often times blocked by your ISP - Internet Service Provider)
		/// * Port 26 (non-secure) - use port 26 if port 25 is not working and is blocked by your ISP
		/// * Port 465 (secure - SSL) - this is to be used to send email via SMTP securely over SSL
		/// * Port 587 (secure - TLS) - this is to be used to send email via SMTP securely over TLS
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		/// Gets or sets all receivers that are addressed by sending an email.
		/// </summary>
		public string[] Receivers { get; set; }

		/// <summary>
		/// Gets or sets the connection information (e.g. name and passwort) of type <seealso cref="ISecret"/>.
		/// </summary>
		public ISecret Credentials { get; set; }

		/// <summary>
		/// Gets or sets the limit of retry attempts for sending an email.
		/// The default retry limit is `3`.
		/// </summary>
		public int RetryLimit { get; set; }

		// -- constructor

		/// <summary>
		/// The default empty constructor sets the retry limit to its default value.
		/// </summary>
		public SmtpSetup()
		{
			RetryLimit = RETRY_LIMIT;
		}
	}
}
