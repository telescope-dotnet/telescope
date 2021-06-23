using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using TeleScope.Connectors.Abstractions;
using TeleScope.Connectors.Abstractions.Events;
using TeleScope.Connectors.Abstractions.Secrets;
using TeleScope.Connectors.Smtp.Abstractions;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace TeleScope.Connectors.Smtp
{
	/// <summary>
	/// This class implements the <seealso cref="ISmtpConnectable"/> interface and
	/// uses only the `System.Net.Mail` namespace internally. 
	/// </summary>
	public class SmtpConnector : ISmtpConnectable
	{
		// -- fields

		private const int RETRY_LIMIT = 3;

		private readonly ILogger<SmtpConnector> log;
		private readonly List<MailMessage> messages;

		private readonly string host;
		private readonly int port;
		private readonly ISecret secret;
		private readonly int retry;

		// --properties

		/// <summary>
		/// Gets the state, if the connection is established or not.
		/// </summary>
		public bool IsConnected { get; private set; }

		// -- events

		/// <summary>
		/// The event is invoked when the <seealso cref="Connect"/> method has finished successfully.
		/// </summary>
		public event ConnectorEventHandler Connected;
		
		/// <summary>
		/// The event is invoked when the <seealso cref="Disconnect"/> method has finished successfully.
		/// </summary>
		public event ConnectorEventHandler Disconnected;

		/// <summary>
		/// The event is invoked when the <seealso cref="Send"/> method has finished successfully.
		/// </summary>
		public event ConnectorCompletedEventHandler Completed;

		/// <summary>
		/// The event is invoked when the <seealso cref="Send"/> method has finished with a failure.
		/// </summary>
		public event ConnectorFailedEventHandler Failed;

		// -- constructors

		/// <summary>
		/// The default constructor gets the SMTP host configuration injected.
		/// </summary>
		/// <param name="host">The name of the host or server.</param>
		/// <param name="port">The port where the SMTP protocol is accessible at the host.</param>
		/// <param name="secret">The user credentials to get access at the host.</param>
		/// <param name="retry">The number of retries if sending returns an error. The default value is `3`.</param>
		public SmtpConnector(string host, int port, ISecret secret, int retry = RETRY_LIMIT)
		{

			log = LoggingProvider.CreateLogger<SmtpConnector>();
			messages = new List<MailMessage>();

			this.host = host ?? throw new ArgumentNullException(nameof(host));
			this.port = port;
			this.secret = secret ?? throw new ArgumentNullException(nameof(secret));
			this.retry = retry;
		}

		// -- methods

		/// <summary>
		/// Clears the internal collection of emails and
		/// invokes the <seealso cref="Connected"/> event.
		/// </summary>
		/// <returns>The calling instance.</returns>
		public ISmtpConnectable Connect()
		{
			messages.Clear();
			IsConnected = true;
			Connected?.Invoke(this, new ConnectorEventArgs(host));
			return this;
		}

		IConnectable IConnectable.Connect()
		{
			return this.Connect();
		}

		/// <summary>
		/// Clears the internal collection of emails and
		/// invokes the <seealso cref="Disconnected"/> event.
		/// </summary>
		/// <returns>The calling instance.</returns>
		public ISmtpConnectable Disconnect()
		{
			messages.Clear();
			IsConnected = false;
			Disconnected?.Invoke(this, new ConnectorEventArgs(host));
			return this;
		}

		IConnectable IConnectable.Disconnect()
		{
			return this.Disconnect();
		}

		/// <summary>
		/// Adds a new message to an internal collection of emails
		/// with the essetial properties, needed to build a new email object.
		/// </summary>
		/// <param name="from">The email address of the sender.</param>
		/// <param name="to">The email address of the receiver.</param>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="body">The message of the email.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		public ISmtpConnectable NewMessage(string from, string to, string subject, string body)
		{
			return NewMessage(from, new string[] { to }, subject, body);
		}

		/// <summary>
		/// Adds a new message to an internal collection of emails
		/// with the essetial properties, needed to build a new email object.
		/// </summary>
		/// <param name="from">The email address of the sender.</param>
		/// <param name="to">The email addresses of the receivers.</param>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="body">The message of the email.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		public ISmtpConnectable NewMessage(string from, string[] to, string subject, string body)
		{
			var msg = new MailMessage();
			var adresses = JoinAddresses(to);

			msg.From = new MailAddress(from);
			msg.To.Add(adresses);
			msg.Subject = subject;
			msg.Body = body;
			messages.Add(msg);

			return this;
		}

		/// <summary>
		/// Adds a cc (carbon body) address to the last email 
		/// that was created by the last call of `NewMessage`.
		/// </summary>
		/// <param name="to">The email address of the receiver.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		public ISmtpConnectable CarbonCopy(string to)
		{
			return CarbonCopy(new string[] { to });
		}

		/// <summary>
		/// Adds a cc (carbon body) address to the last email 
		/// that was created by the last call of `NewMessage`.
		/// </summary>
		/// <param name="to">The email addresses of the receivers.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		public ISmtpConnectable CarbonCopy(string[] to)
		{
			var addresses = JoinAddresses(to);
			messages.Last()
				.CC.Add(addresses);
			return this;
		}

		/// <summary>
		/// Adds a bcc (blind carbon body) address to the last email 
		/// that was created by the last call of `NewMessage`.
		/// </summary>
		/// <param name="to">The email address of the receiver.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		public ISmtpConnectable BlindCarbonCopy(string to)
		{
			return BlindCarbonCopy(new string[] { to });
		}

		/// <summary>
		/// Adds a bcc (blind carbon body) address to the last email 
		/// that was created by the last call of `NewMessage`.
		/// </summary>
		/// <param name="to">The email addresses of the receivers.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		public ISmtpConnectable BlindCarbonCopy(string[] to)
		{
			var addresses = JoinAddresses(to);
			messages.Last()
				.Bcc.Add(addresses);
			return this;
		}

		/// <summary>
		/// Adds a high priority flag to the latest email.
		/// </summary>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		public ISmtpConnectable HighPriority()
		{
			messages.Last().Priority = MailPriority.High;
			return this;
		}

		/// <summary>
		/// Adds a low priority flag to the latest email.
		/// </summary>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		public ISmtpConnectable LowPriority()
		{
			messages.Last().Priority = MailPriority.Low;
			return this;
		}

		/// <summary>
		/// This method should attach a file to the latest email.
		/// </summary>
		/// <param name="file">The file info object that will be attached.</param>
		/// <param name="mimeType">The mime type of the file.</param>
		/// <returns>Returns the calling instance to enable chaining method calls.</returns>
		public ISmtpConnectable Attach(FileInfo file, string mimeType = MediaTypeNames.Text.Plain)
		{
			if (file == null)
			{
				throw new ArgumentNullException(nameof(file));
			}

			var filename = file.FullName;
			var attachment = new Attachment(filename, mimeType);
			messages.Last().Attachments.Add(attachment);
			log.Debug($"The file '{filename}' was added as attachment to the last email.");
			return this;
		}

		/// <summary>
		/// Sends all the created and configured emails at once and 
		/// clears the inner stack.
		/// </summary>
		/// <returns>Returns a result triple that contains the total amount of sent emails, 
		/// the sucessful and the failed ones.</returns>
		public (int total, int success, int failed) Send()
		{
			var result = (0, 0, 0);
			var client = GetClient();

			try
			{
				result = SendAllMessages(client);
				Completed?.Invoke(this, new ConnectorCompletedEventArgs(host, result));
			}
			catch (ArgumentOutOfRangeException ex)
			{
				log.Critical(ex);
				Failed?.Invoke(this, new ConnectorFailedEventArgs(ex, host));
			}
			finally
			{
				if (client != null)
				{
					client.Dispose();
				}
			}

			return result;
		}

		// -- private helper 

		private string JoinAddresses(string[] addresses)
		{
			return addresses.Length == 1 ? addresses[0] : string.Join(",", addresses);
		}

		private (int total, int success, int failed) SendAllMessages(SmtpClient client)
		{
			(int total, int success, int failed) result = (0, 0, 0);

			result.total = messages.Count;
			int limit = retry;
			int attempt = 1;

			while (messages.Count > 0 && limit > 0)
			{
				var mail = messages.First();
				try
				{
					client.Send(mail);
					messages.Remove(mail);
					result.success++;
				}
				catch (Exception ex) when (handled(ex))
				{
					log.Error(ex, $"Attempt #{attempt} failed sending an email via {client.Host}.");
					Failed?.Invoke(this, new ConnectorFailedEventArgs(ex, host));
					result.failed++;
					limit--;
					attempt++;
				}
			}

			return result;

			// local function

			bool handled(Exception e)
			{
				return
					e is ArgumentNullException ||
					e is InvalidOperationException ||
					e is ObjectDisposedException ||
					e is SmtpException ||
					e is SmtpFailedRecipientsException;
			}
		}

		private SmtpClient GetClient()
		{
			return new SmtpClient(host, port)
			{
				UseDefaultCredentials = false,
				EnableSsl = true,
				Credentials = new NetworkCredential(secret.Name, secret.Password)
			};
		}
	}
}
