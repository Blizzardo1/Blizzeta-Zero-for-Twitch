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
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace BlizzetaZero.Kernel
{
    public class Global
    {
        public const string Appname = "Blizzeta Codenamed Zero";
        public const string Core = "3.0";
        public const string Scripts = "2.0";
        public const string Nick = "BlizzetaBot";
        public const string Username = "Blizzeta";
        public const string Realname = "Blizzeta Zero";
        public const string IrcServer = "irc.twitch.tv";
        public const int Port = 6667;
        public const string ServerPassword = "oauth:xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        public const string MainChannel = "#blizzardothegreat"; // Main channel
        public const string DebugChannel = "#blizzetadbg"; // This channel doesn't really get utilized, so it can be removed, and debug set to false

        public const string UrlLoc = "https://github.com/Blizzardo1/BlizzetaZero";
        public const string Version = "7.0β";
        public static string DefaultQuit = string.Format ( "{0} {1}; Source Code: {2}", Appname, Version, UrlLoc );
        public static string Title = string.Format ( "{0} Version {1}", Appname, Version );

        private static TimeSpan uptime;

        public static TimeSpan Uptime { get { return uptime; } }

        public static void BeginUptime ( )
        {
            uptime = new TimeSpan ( );
            Task.Run ( ( ) =>
            {
                while ( true )
                {
                    Console.Title = string.Format ( "{0} Uptime: {1:c}", Appname, uptime );
                    uptime = uptime.Add ( new TimeSpan ( 0, 0, 0, 1, 0 ) );
                    Thread.Sleep ( 1000 );
                }
            } );
        }

        public static string FormatUptime ( )
        {
            // Are you gonna fuck up on me again!? Come on Visual Studio, you don't stand a fucking chance...

            string Days = string.Format ( "{0} {1}", uptime.Days, uptime.Days > 1 || uptime.Days < 1 ? "days" : "day" );
            string Hours = string.Format ( "{0} {1}", uptime.Hours, uptime.Hours > 1 || uptime.Hours < 1 ? "hours" : "hour" );
            string Minutes = string.Format ( "{0} {1}", uptime.Minutes, uptime.Minutes > 1 || uptime.Minutes < 1 ? "minutes" : "minute" );
            string Seconds = string.Format ( "{0} {1}", uptime.Seconds, uptime.Seconds > 1 || uptime.Seconds < 1 ? "seconds" : "second" );
            return ( uptime.Minutes < 1 ?
                string.Format ( "{0}", Seconds )
                : ( uptime.Hours < 1 ?
                string.Format ( "{0} and {1}", Minutes, Seconds ) : (
                uptime.Days < 1 ?
                string.Format ( "{0}, {1}, and {2}", Hours, Minutes, Seconds )
                :
                string.Format ( "{0}, {1}, {2}, and {3}", Days, Hours, Minutes, Seconds )
                )
                ) );
        }

        public static string GenerateHash ( string fileName )
        {
            string hashText = "";
            string hexValue = "";

            byte[] fileData = File.ReadAllBytes ( fileName );
            byte[] hashData = SHA1.Create ( ).ComputeHash ( fileData ); // SHA1 or MD5

            foreach ( byte b in hashData )
            {
                hexValue = b.ToString ( "X" ).ToLower ( ); // Lowercase for compatibility on case-sensitive systems
                hashText += ( hexValue.Length == 1 ? "0" : "" ) + hexValue;
            }

            return hashText;
        }

        public static DateTime UnixTimeStampToDateTime ( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime ( 1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc );
            dtDateTime = dtDateTime.AddSeconds ( unixTimeStamp ).ToLocalTime ( );
            return dtDateTime;
        }
    }
}