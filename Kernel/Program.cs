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

using Blizzeta.Twitch;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace BlizzetaZero.Kernel
{
    /*
     * I will be the very best, Like no one ever was.
     * To catch them is my real test.
     * To train them is my cause.
     * I will travel across the land, searching far and wide.
     * Each pokémon, to understand the power that's inside.
     * ->   Pokémon! It's you and me, I know it's my destiny!
     * ->   Pokémon, Oh, you're my best friend, in a world we must defend.
     * ->   Pokémon, a heart so true, our courage will pull us through!
     * ->   You teach me and i'll teach you, Pokémon!
     * ->   Gotta catch 'em all!
     * ->   POKÉMON!
     */

    public class Program
    {
        private static Settings _settings;

        public static Settings settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        private static void irc_OnMotd ( string data )
        {
            IrcReply.FormatMessage ( data, ConsoleColor.Yellow );
        }

        private static async void CarpeDiem ( )
        {
            Console.WriteLine ( "Creating new Process" );
            using ( Process p = new Process ( )
            {
                StartInfo = new ProcessStartInfo ( )
                {
                    FileName = "bztais.exe",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false
                }
            } )
            {
                Console.WriteLine ( "Starting {0} with {1}, {2}, {3}, and {4}",
                    p.StartInfo.FileName,
                    p.StartInfo.RedirectStandardError ? "Errors redirected" : "Errors not redirected",
                    p.StartInfo.RedirectStandardInput ? "Input redirected" : "Input not redirected",
                    p.StartInfo.RedirectStandardOutput ? "Output redirected" : "Output not redirected",
                    p.StartInfo.UseShellExecute ? "Shell Execute On" : "Shell Execute Off"
                    );
                p.Start ( );

                StreamReader output = p.StandardOutput;
                while ( !p.HasExited )
                {
                    string msg = "";
                    while ( !string.IsNullOrEmpty ( msg = await output.ReadLineAsync ( ) ) )
                        Console.WriteLine ( msg );
                }
                Console.WriteLine ( "Program has Quit" );
                Auth.Authenticate ( );
            }
        }

        private static bool SanityCheck ( )
        {
            if ( !File.Exists ( "Streamers.xml" ) )
            {
                Streamer.CreateStreamerDatabase ( "blizzardothegreat" );
                Console.WriteLine ( "Warning! -> This is a soft check!" );
            }

            return true;
        }

        private static void Main ( string[] args )
        {
            if ( !SanityCheck ( ) )
            {
                Console.WriteLine ( "The bot has failed to complete its sanity check...\r\nPlease press any key to exit the Bot..." );
                Console.ReadKey ( true );
                return;
            }

            // Start the timer on ground zero 
            global::BlizzetaZero.Kernel.Global.BeginUptime ( );

            // Auth.Authenticate(); Thread t = new Thread ( new ThreadStart ( LaunchGamePanel ) );
            // t.Start ( );
            Console.Title = Global.Title;
            Console.WindowWidth += 60;

            _settings = new Settings ( );
            CoreCommands.IncludeBuiltInCommands ( );
            SetCharSet ( );
            CarpeDiem ( );

            Irc irc = new Irc ( Global.Nick,
                Global.Realname,
                Global.Username,
                "",
                Global.IrcServer,
                Global.ServerPassword,
                Global.Port,
                RFC1459.ReplyCode.ERR_UNKNOWNMODE,
                ConsoleColor.Green,
                "#blizzardothegreat",
                "",
                "blizzardothegreat",
                "#blizzetabot",
                "" );
            try
            {
                irc.OnMotd += irc_OnMotd;
                irc.Connect ( irc.Host );
            }
            catch ( Exception e )
            {
                Console.WriteLine ( e.Message );
            }
        }

        private static void i_OnMotd ( string data )
        {
            irc_OnMotd ( data );
        }

        private static void SetCharSet ( )
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine ( "Output encoding now changed to {0}", Console.OutputEncoding.EncodingName );
        }
    }
}