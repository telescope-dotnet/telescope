using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TeleScope.Connectors.Abstractions
{
	public interface IAsyncConnectable
	{
        Task ConnectAsync();

        Task DisconnectAsync();
    }
}
