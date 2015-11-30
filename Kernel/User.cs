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

namespace BlizzetaZero.Kernel
{
    /*public class User
    {
        private string nick, host, username;

        /// <summary>
        ///
        /// </summary>
        /// <param name="hostmask"></param>
        public User ( string hostmask )
        {
            string[] hm = hostmask.Split ( '!' );
            string[] uh = hm[ 1 ].Split ( '@' );
            this.nick = hm[ 0 ];
            this.username = uh[ 0 ];
            this.host = uh[ 1 ];
        }

        public string Host { get { return host; } }

        public string Nick { get { return nick; } }

        public string Username { get { return username; } }

        public static User GetUser ( Irc server, string nick )
        {
            WhoInfo wi = WhoInfo.GetUser ( server, nick );

            return new User ( wi.ToString ( ) );
        }
    }*/
}