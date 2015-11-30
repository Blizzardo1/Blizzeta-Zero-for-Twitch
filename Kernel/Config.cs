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

using BreakerDev.Crypto;
using Nini.Config;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlizzetaZero.Kernel
{
    public class Settings
    {
        private static string _path = Irc.StartupPath;
        private static string configpath = string.Format ( "{0}\\Settings.conf", _path );

        public Irc Irc { get { return _irc; } }

        private Irc _irc;

        private IConfigSource config;

        public Settings ( )
        {
            if ( !File.Exists ( configpath ) )
                File.Create ( configpath ).Close ( );

            config = new IniConfigSource ( configpath );
        }

        public Irc Read ( string server )
        {
            Irc _irc;
            string host, nick, password, srvPass, owner, user, real, channel, key, additional;
            int port;
            ConsoleColor colour;
            RFC1459.ReplyCode lkc;
            IConfig irc = config.Configs[ server ];

            nick = irc.GetString ( "Nick" );
            real = irc.GetString ( "Realname" );
            user = irc.GetString ( "Username" );
            host = irc.GetString ( "Host" );
            srvPass = irc.GetString ( "Server_Password" );
            port = irc.GetInt ( "Port" );
            owner = irc.GetString ( "Owner" );
            password = irc.GetString ( "NickPassword" );
            if ( !string.IsNullOrEmpty ( password ) )
                password = new SymCryptography ( SymCryptography.ServiceProviderEnum.Rijndael ).Decrypt ( password );
            lkc = ( RFC1459.ReplyCode ) irc.GetInt ( "LastKnownCode" );
            colour = ( ConsoleColor ) irc.GetInt ( "Colour" );
            channel = irc.GetString ( "MainChannel" );
            key = irc.GetString ( "MainKey" );
            additional = irc.GetString ( "Channels" );

            Stack<ChannelData> chanBuffer = new Stack<ChannelData> ( );

            foreach ( string ch in additional.Split ( ',' ) )
            {
                string[] chankey = ch.Split ( ':' );
                chanBuffer.Push ( new ChannelData ( ) { Channel = chankey[ 0 ], Key = chankey[ 1 ] } );
            }

            _irc = new Irc ( nick, real, user, password, host, "", port, lkc, colour, channel, key );
            _irc.ChannelBuffer = chanBuffer;

            return _irc;
        }

        public Irc[] Read ( )
        {
            List<Irc> l = new List<Irc> ( );

            ConfigCollection cfgs = null;
            if ( config.Configs.Count > 0 )
                cfgs = config.Configs;
            else
                return null;

            foreach ( IConfig cfg in cfgs )
            {
                l.Add ( Read ( cfg.Name ) );
            }

            return l.ToArray ( );
        }

        public void Save ( Irc irc )
        {
            IConfig server = null; ;
            string host = irc.Host;
            string nick = irc.Nick;
            string user = irc.Username;
            string real = irc.Realname;
            int port = irc.Port;
            string owner = irc.Owner;
            string nspass = irc.NickPass;
            string srvpass = irc.ServerPass;
            Stack<ChannelData> buffer = irc.ChannelBuffer;

            if ( !string.IsNullOrEmpty ( nspass ) )
                nspass = new SymCryptography ( SymCryptography.ServiceProviderEnum.Rijndael ).Encrypt ( nspass );

            // Last known code
            RFC1459.ReplyCode lkc = irc.Code;
            ConsoleColor colour = irc.Colour;
            if ( config.Configs[ irc.Server ] == null )
                server = config.AddConfig ( irc.Server );
            else
                server = config.Configs[ irc.Server ];

            server.Set ( "Host", host );
            server.Set ( "Port", port );
            server.Set ( "Nick", nick );
            server.Set ( "Server_Password", srvpass );
            server.Set ( "Username", user );
            server.Set ( "Realname", real );
            server.Set ( "NickPassword", nspass );
            server.Set ( "Owner", owner );
            server.Set ( "LastKnownCode", ( int ) lkc );
            server.Set ( "Colour", ( int ) colour );
            server.Set ( "MainChannel", "{nokey}" );
            server.Set ( "MainKey", "{nokey}" );

            if ( buffer.Count > 0 )
                server.Set ( "Channels", string.Join ( ",", buffer.ToArray ( ) ) );

            config.Save ( );
        }
    }
}