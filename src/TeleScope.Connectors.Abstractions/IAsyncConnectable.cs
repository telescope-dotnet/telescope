using System.Threading.Tasks;

namespace TeleScope.Connectors.Abstractions
{
	public interface IAsyncConnectable
	{
		Task ConnectAsync();

		Task DisconnectAsync();
	}
}
