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

using System.Linq;
using System.Text.RegularExpressions;

namespace BlizzetaZero.RFC1459
{
    public class IrcCommands
    {
        /**/
        public static readonly string ircregex = @"^(:(?<prefix>\S+) )?(?<command>\S+)( (?!:)(?<params>.+?))?( :(?<trail>.+))?$";
        public static Regex ActionRegex = new Regex ( "^:.*? PRIVMSG (.).* :" + "\x1" + "ACTION .*" + "\x1" + "$", RegexOptions.Compiled );
        public static Regex CtcpReplyRegex = new Regex ( "^:.*? NOTICE .* :" + "\x1" + ".*" + "\x1" + "$", RegexOptions.Compiled );
        public static Regex CtcpRequestRegex = new Regex ( "^:.*? PRIVMSG .* :" + "\x1" + ".*" + "\x1" + "$", RegexOptions.Compiled );
        public static Regex ErrorRegex = new Regex ( "^ERROR :.*", RegexOptions.Compiled );
        public static Regex InviteRegex = new Regex ( "^:.*? INVITE .* .*$", RegexOptions.Compiled );
        public static Regex JoinRegex = new Regex ( "^:.*? JOIN .*$", RegexOptions.Compiled );
        public static Regex KickRegex = new Regex ( "^:.*? KICK .* .*$", RegexOptions.Compiled );
        public static Regex MessageRegex = new Regex ( "^:.*? PRIVMSG (.).* :.*$", RegexOptions.Compiled );
        public static Regex ModeRegex = new Regex ( "^:.*? MODE (.*) .*$", RegexOptions.Compiled );
        public static Regex NickRegex = new Regex ( "^:.*? NICK .*$", RegexOptions.Compiled );
        public static Regex NoticeRegex = new Regex ( "^:.*? NOTICE (.).* :.*$", RegexOptions.Compiled );
        public static Regex PartRegex = new Regex ( "^:.*? PART .*$", RegexOptions.Compiled );
        public static Regex PingRegex = new Regex ( "^PING :.*", RegexOptions.Compiled );
        public static Regex QuitRegex = new Regex ( "^:.*? QUIT :.*$", RegexOptions.Compiled );
        public static Regex ReplyCodeRegex = new Regex ( "^:[^ ]+? ([0-9]{3}) .+$", RegexOptions.Compiled );
        public static Regex TopicRegex = new Regex ( "^:.*? TOPIC .* :.*$", RegexOptions.Compiled );
        /*Registration*/

        public static string Action ( string channel, string actionmessage )
        {
            return string.Format ( "PRIVMSG {0} :\x01" + "ACTION {1}\x01", channel, actionmessage );
        }

        public static string Admin ( )
        {
            return "ADMIN";
        }

        public static string Admin ( string server )
        {
            return string.Format ( "ADMIN {0}", server );
        }

        public static string Away ( string message )
        {
            return string.Format ( "AWAY {0}", message );
        }

        public static string Ban ( string channel, string nick )
        {
            return string.Format ( "MODE {0} +b {1}", channel, nick );
        }

        public static string Ctcp ( string receiver, string message )
        {
            return string.Format ( "CTCP {0} :{1}", receiver, message );
        }

        public static string Info ( )
        {
            return "INFO";
        }

        public static string Info ( string server )
        {
            return string.Format ( "INFO {0}", server );
        }

        public static string Invite ( string nick, string channel )
        {
            return string.Format ( "INVITE {0} {1}", nick, channel );
        }

        public static string Ison ( string nick, params string[] nicks )
        {
            return string.Format ( "ISON {0} {1}", nick, string.Join ( " ", nicks ) );
        }

        public static string Join ( string channel, string password )
        {
            return string.Format ( "JOIN {0} {1}", channel, password );
        }

        public static string Join ( string[] channels, string[] passwords )
        {
            return string.Format ( "JOIN {0} {1}", string.Join ( ",", channels ), string.Join ( ",", passwords ) );
        }

        public static string Kick ( string channel, string nick, string reason )
        {
            return string.Format ( "KICK {0} {1} :{2}", channel, nick, reason );
        }

        public static string Kill ( string nick, string reason )
        {
            return string.Format ( "KILL {0} {1}", nick, reason );
        }

        public static string Links ( )
        {
            return "LINKS";
        }

        public static string Links ( string remoteServer )
        {
            return string.Format ( "LINKS {0}", remoteServer );
        }

        public static string Links ( string remoteServer, string server_mask )
        {
            return string.Format ( "LINKS {0} {1}", remoteServer, server_mask );
        }

        public static string Lusers ( )
        {
            return "LUSERS";
        }

        public static string Lusers ( string mask )
        {
            return string.Format ( "LUSERS {0}", mask );
        }

        public static string Lusers ( string mask, string target )
        {
            return string.Format ( "LUSERS {0} {1}", mask, target );
        }

        public static string Mode ( string channel, string mode )
        {
            return string.Format ( "MODE {0} {1}", channel, mode );
        }

        public static string Names ( string channel )
        {
            return string.Format ( "NAMES {0}", channel );
        }

        public static string Nick ( string newNick )
        {
            return string.Format ( "NICK {0}", newNick );
        }

        public static string Notice ( string receiver, string message )
        {
            return string.Format ( "NOTICE {0} :{1}", receiver, message );
        }

        public static string Oper ( string username, string password )
        {
            return string.Format ( "OPER {0} {1}", username, password );
        }

        public static bool ParseReply ( string input, out string prefix, out string command, out string[] parameters )
        {
            string trailing = null;
            prefix = command = string.Empty;
            parameters = new string[] { };
            Regex r = new Regex ( ircregex, RegexOptions.Compiled | RegexOptions.ExplicitCapture );
            Match match = r.Match ( input );

            if ( match.Success )
            {
                prefix = match.Groups[ "prefix" ].Value;
                command = match.Groups[ "command" ].Value;
                parameters = match.Groups[ "params" ].Value.Split ( ' ' );
                trailing = match.Groups[ "trail" ].Value;

                if ( !string.IsNullOrEmpty ( trailing ) )
                    parameters = parameters.Concat ( new string[] { trailing } ).ToArray ( );
                return true;
            }

            return false;
        }

        public static string Part ( string channel, string reason )
        {
            return string.Format ( "PART {0} {1}", channel, reason );
        }

        public static string Pass ( string password )
        {
            return string.Format ( "PASS {0}", password );
        }

        public static string Ping ( string server )
        {
            return string.Format ( "PING {0}", server );
        }

        public static string Pong ( string server )
        {
            return string.Format ( "PONG {0}", server );
        }

        public static string Privmsg ( string receiver, string message )
        {
            return string.Format ( "PRIVMSG {0} :{1}", receiver, message );
        }

        public static string Quit ( string reason )
        {
            return string.Format ( "QUIT {0}", reason );
        }

        public static string Quote ( string text, string arguments )
        {
            return string.Format ( "QUOTE {0} {1}", text, arguments );
        }

        public static string Raw ( string text, string arguments )
        {
            return Quote ( text, arguments );
        }

        public static string Squery ( string serviceName, string message )
        {
            return string.Format ( "SQUERY {0} {1}", serviceName, message );
        }

        public static string Stats ( string query )
        {
            return string.Format ( "STATS {0}", query );
        }

        public static string Stats ( string query, string server )
        {
            return string.Format ( "STATS {0} {1}", query, server );
        }

        public static string Time ( )
        {
            return "TIME";
        }

        public static string Time ( string server )
        {
            return string.Format ( "TIME {0}", server );
        }

        public static string Topic ( string channel )
        {
            return string.Format ( "TOPIC {0}", channel );
        }

        public static string Topic ( string channel, string topic )
        {
            return string.Format ( "TOPIC {0} {1}", channel, topic );
        }

        public static string Trace ( )
        {
            return "TRACE";
        }

        public static string Trace ( string server )
        {
            return string.Format ( "TRACE {0}", server );
        }

        public static string Umode ( string mode )
        {
            return string.Format ( "UMODE {0}", mode );
        }

        public static string Umode2 ( string mode )
        {
            return string.Format ( "UMODE2 {0}", mode );
        }

        public static string Unban ( string channel, string nick )
        {
            return string.Format ( "MODE {0} -b {1}", channel, nick );
        }

        public static string User ( string username, int mode, string realname )
        {
            return string.Format ( "USER {0} {1} * :{2}", username, mode, realname );
        }

        /*Oper*/
        /*Client Operations*/
        /*Channel*/
        /*Non-Channel*/

        public static string Users ( )
        {
            return "USERS";
        }

        public static string Users ( string server )
        {
            return string.Format ( "USERS {0}", server );
        }

        public static string Version ( string server )
        {
            return string.Format ( "VERSION {0}", server );
        }

        public static string Who ( string name, bool checkForOperator )
        {
            return string.Format ( checkForOperator ? "WHO {0} o" : "WHO {0}", name );
        }

        public static string Whois ( string nick )
        {
            return string.Format ( "WHOIS {0}", nick );
        }

        public static string Whois ( string server, string nick )
        {
            return string.Format ( "WHOIS {0} {1}", server, nick );
        }

        public static string Whowas ( string nick )
        {
            return string.Format ( "WHOWAS {0}", nick );
        }

        public static string Whowas ( string nick, int count )
        {
            return string.Format ( "WHOWAS {0} {1}", nick, count );
        }

        public static string Whowas ( string nick, int count, string server )
        {
            return string.Format ( "WHOWAS {0} {1} {2}", nick, count, server );
        }

        /*Server*/
    }
}