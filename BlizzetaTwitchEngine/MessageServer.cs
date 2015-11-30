using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Blizzeta.Twitch
{
    // Establish a listening event on an available port on the server this bot is on Use PHP to
    // attempt to send the command back to the bot
    public class MessageServer
    {
        private int port;
        private TcpListener ls;

        public MessageServer ( int port )
        {
            this.ls = new TcpListener ( IPAddress.Loopback, port );
            this.port = port;
            listen ( );
        }

        private async void listen ( )
        {
            ls.Start ( );
            while ( true )
            {
                try
                {
                    TcpClient c = ls.AcceptTcpClient ( );
                    StreamReader reader = new StreamReader ( c.GetStream ( ) );
                    StreamWriter writer = new StreamWriter ( c.GetStream ( ) );
                    string msg = string.Empty;

                    while ( ( msg = await reader.ReadLineAsync ( ) ) != null )
                    {
                        if ( !string.IsNullOrEmpty ( msg ) )
                            Console.WriteLine ( msg );
                    }
                }
                catch ( Exception ex )
                {
                    Console.WriteLine ( "Error >> {0}", ex.Message );
                }
            }
        }
    }
}