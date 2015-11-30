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

namespace BlizzetaZero.Kernel
{
    public class IrcReply
    {
        private string nick, hostmask, command;

        public string Command { get { return command; } }

        public string Hostmask { get { return nick; } }

        public string Nick { get { return nick; } }

        public static void FormatMessage ( IrcMessage message, ConsoleColor colour = ConsoleColor.White, bool encapsulate = false )
        {
            Console.ForegroundColor = colour;
            Console.Write ( "[{0:HH:mm:ss},", DateTime.Now );
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write ( " {0}", message.Prefix );
            Console.ForegroundColor = colour;

            if ( encapsulate )
                Console.WriteLine ( "] -> {0} {{ {1} }}", message.Command, string.Join ( " ", message.Parameters ) );
            else
                Console.WriteLine ( "] -> {0} {1}", message.Command, string.Join ( " ", message.Parameters ) );
            Console.ResetColor ( );
        }

        public static void FormatMessage ( string message, ConsoleColor colour = ConsoleColor.White, bool stripStamp = false )
        {
            Console.ForegroundColor = colour;

            // You can either strip the colour codes, bold codes, italic codes, and even the underline codes... Or we parse with them
            // Find a simple way to get a mapof the colour codes. ## -OR- ##,##

            if ( stripStamp )
                Console.WriteLine ( message );
            else
                Console.WriteLine ( "[{0:HH:mm:ss}] -> {1}", DateTime.Now, message );
            Console.ResetColor ( );
        }

        /// <summary>
        /// Parse Line data
        /// </summary>
        /// <param name="Line">The line of an Irc Reply Message</param>
        /// <returns>A new instance of IrcReply</returns>
        public static IrcReply GetFromLine ( string Line )
        {
            string[] lineData = Line.Split ( new char[] { ' ' }, 4 );

            return GetFromLine ( lineData );
        }

        /// <summary>
        /// Parse Line data
        /// </summary>
        /// <param name="Data">The Array of an Irc Reply Message</param>
        /// <returns>A new instance of IrcReply</returns>
        public static IrcReply GetFromLine ( string[] Data )
        {
            int position = Data[ 0 ].IndexOf ( '!' );
            IrcReply iData = new IrcReply ( );

            if ( position > 1 )
            {
                iData.nick = Data[ 0 ].Substring ( 1, position - 1 );
                iData.hostmask = Data[ 0 ].Substring ( position );
            }
            else
            {
                iData.nick = Data[ 0 ].Substring ( 1 );
                iData.hostmask = string.Empty;
            }
            iData.command = Data[ 1 ];
            return iData;
        }
    }
}