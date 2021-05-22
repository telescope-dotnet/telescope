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

		public bool IsConnected { get; private set; }

		// -- events

		public event ConnectorEventHandler Connected;
		public event ConnectorEventHandler Disconnected;
		public event ConnectorCompletedEventHandler Completed;
		public event ConnectorFailedEventHandler Failed;

		// -- constructors

		public SmtpConnector(string host, int port, ISecret secret, int retry = RETRY_LIMIT)
		{
			log = LoggingProvider.CreateLogger<SmtpConnector>();
			messages = new List<MailMessage>();

			this.host = host;
			this.port = port;
			this.secret = secret;
			this.retry = retry;
		}

		// -- methods

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

		public ISmtpConnectable Disconnect()
		{
			IsConnected = false;
			Disconnected?.Invoke(this, new ConnectorEventArgs(host));
			return this;
		}

		IConnectable IConnectable.Disconnect()
		{
			return this.Disconnect();
		}

		public ISmtpConnectable NewMessage(string from, string to, string subject, string body)
		{
			return NewMessage(from, new string[] { to }, subject, body);
		}

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

		public ISmtpConnectable CarbonCopy(string to)
		{
			return CarbonCopy(new string[] { to });
		}

		public ISmtpConnectable CarbonCopy(string[] to)
		{
			var addresses = JoinAddresses(to);
			messages.Last()
				.CC.Add(addresses);
			return this;
		}

		public ISmtpConnectable BlindCarbonCopy(string to)
		{
			return BlindCarbonCopy(new string[] { to });
		}

		public ISmtpConnectable BlindCarbonCopy(string[] to)
		{
			var addresses = JoinAddresses(to);
			messages.Last()
				.Bcc.Add(addresses);
			return this;
		}

		public ISmtpConnectable HighPriority()
		{
			messages.Last().Priority = MailPriority.High;
			return this;
		}

		public ISmtpConnectable LowPriority()
		{
			messages.Last().Priority = MailPriority.Low;
			return this;
		}

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

		public (int total, int success, int failed) Send()
		{
			var result = (0, 0, 0);
			var client = default(SmtpClient);

			try
			{
				result = SendAllMessages(out client);
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

		private (int total, int success, int failed) SendAllMessages(out SmtpClient client)
		{
			(int total, int success, int failed) result = (0, 0, 0);
			client = GetClient();

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
					result.success ++;
				}
				catch (Exception ex) when (handled(ex))
				{
					log.Error(ex, $"Attempt #{attempt} failed sending an email via {client.Host}.");
					Failed?.Invoke(this, new ConnectorFailedEventArgs(ex, host));
					result.failed ++;
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
