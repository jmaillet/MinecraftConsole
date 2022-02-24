using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CraftWrap
{


    public class SocketListener
    {
        private const string SOCKET_ADDR = @"d:\temp.socket";

        public SocketListener()
        {
            ClientSocket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
        }

        public Socket ClientSocket { get; }

        public async Task ListenAsync(CancellationToken cancellationToken)
        {
            if (File.Exists(SOCKET_ADDR))
            {
                File.Delete(SOCKET_ADDR);
            }
            var localEndPoint = new UnixDomainSocketEndPoint(SOCKET_ADDR);

            // Create a unix domain socket.  
            var listener = new Socket(localEndPoint.AddressFamily,
                SocketType.Stream, ProtocolType.Unspecified);
            listener.Bind(localEndPoint);
            listener.Listen(1);
            // Start an asynchronous socket to listen for connections.  
            Console.WriteLine("Waiting for a connection...");
            _ = await listener.AcceptAsync(ClientSocket,cancellationToken).ConfigureAwait(false);
           
           
        }
    }

   
}
