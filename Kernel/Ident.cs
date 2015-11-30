/*‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾*\
|*  Copyright (C) 2014  Blizzeta Software                                   *|
|*                                                                          *|
|*  This program is free software: you can redistribute it and/or modify    *|
|*  it under the terms of the GNU General Public License as published by    *|
|*  the Free Software Foundation, either version 3 of the License, or       *|
|*  (at your option) any later version.                                     *|
|*                                                                          *|
|*  This program is distributed in the hope that it will be useful,         *|
|*  but WITHOUT ANY WARRANTY; without even the implied warranty of          *|
|*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *|
|*  GNU General Public License for more details.                            *|
|*                                                                          *|
|*  You should have received a copy of the GNU General Public License       *|
|*  along with this program.  If not, see <http://www.gnu.org/licenses/>.   *|
\*__________________________________________________________________________*/

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace BlizzetaZero.Kernel
{
    public class Ident
    {
        private readonly TcpListener _listener;
        private readonly string _userId;

        public const int Port = 113;

        public Ident ( string userId )
        {
            _userId = userId;
            _listener = new TcpListener ( IPAddress.Any, Port );
        }

        public void InitServer ( )
        {
            try
            {
                _listener.Start ( );
                IrcReply.FormatMessage ( "Blizzeta Ident Server Initialized", ConsoleColor.Green );
                TcpClient client = _listener.AcceptTcpClient ( );
                IrcReply.FormatMessage ( string.Format ( "Got a connection! {0}",
                    client.Client.LocalEndPoint.AddressFamily ), ConsoleColor.Gray );
                _listener.Stop ( );

                IrcReply.FormatMessage ( "Ident Server Stopped... Parsing Data...", ConsoleColor.DarkMagenta );
                var reader = new StreamReader ( client.GetStream ( ) );
                var writer = new StreamWriter ( client.GetStream ( ) );
                string s = reader.ReadLine ( );
                string rplFmt = string.Format ( "{0} : USERID: UNIX : {1}", s, _userId );
                IrcReply.FormatMessage ( string.Format ( "Fetched Ident! Ident is {0}. Sending a reply now...", s ),
                    ConsoleColor.Yellow );
                Console.WriteLine ( "Reply: {0}", rplFmt );

                writer.SendMessage ( rplFmt );
                IrcReply.FormatMessage ( "Sent!", ConsoleColor.Magenta );
                IrcReply.FormatMessage ( "Disconnecting from {{ Ident }}", ConsoleColor.DarkMagenta );

                // Destroy
                client.Close ( );
            }
            catch ( SocketException se )
            {
                IrcReply.FormatMessage ( se.Message, ConsoleColor.Red, true );
            }
            catch ( Exception ex )
            {
                IrcReply.FormatMessage ( ex.Message, ConsoleColor.Red, true );
            }
        }
    }
}