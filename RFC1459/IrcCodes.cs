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

namespace BlizzetaZero.RFC1459
{
    public enum Colors : int
    {
        White = 0,
        Black = 1,
        Blue = 2,
        Green = 3,
        Red = 4,
        Maroon = 5,
        Purple = 6,
        Orange = 7,
        Yellow = 8,
        LightGreen = 9,
        DarkCyan = 10,
        Cyan = 11,
        Royale = 12,
        Magenta = 13,
        DarkGray = 14,
        Gray = 15
    };

    public enum ReceiveType : int
    {
        Login,
        Info,
        Motd,
        Name,
        Who,
        List,
        BanList,
        Topic,
        TopicChange,
        WhoIs,
        WhoWas,
        NickChange,
        UserMode,
        ChannelMode,
        ChannelModeChange,
        ErrorMessage,
        Unknown,
        Error,
        CtcpRequest,
        CtcpReply,
        Invite,
        Join,
        Kick,
        Part,
        UserModeChange,
        Quit,
        QueryMessage,
        QueryNotice,
        QueryAction,
        ChannelAction,
        ChannelNotice,
        ChannelMessage,
    }

    public enum ReplyCode : int
    {
        ERR_UNKNOWNCODE = 000,
        ERR_NOSUCHNICK = 401,           // "<nickname> :No such nick/channel"
        ERR_NOSUCHSERVER = 402,         // "<server name> :No such server"
        ERR_NOSUCHCHANNEL = 403,        // "<channel name> :No such channel"
        ERR_CANNOTSENDTOCHAN = 404,     // "<channel name> :Cannot send to channel"
        ERR_TOOMANYCHANNELS = 405,      // "<channel name> :You have joined too many channels"
        ERR_WASNOSUCHNICK = 406,        // "<nickname> :There was no such nickname"
        ERR_TOOMANYTARGETS = 407,       // "<target> :Duplicate recipients. No message delivered"
        ERR_NOORIGIN = 409,             // ":No origin specified"
        ERR_NORECIPIENT = 411,          // ":No recipient given (<command>)"
        ERR_NOTEXTTOSEND = 412,         // ":No text to send"
        ERR_NOTOPLEVEL = 413,           // "<mask> :No toplevel domain specified"
        ERR_WILDTOPLEVEL = 414,         // "<mask> :Wildcard in toplevel domain"
        ERR_UNKNOWNCOMMAND = 421,       // "<command> :Unknown command"
        ERR_NOMOTD = 422,               // ":MOTD File is missing"
        ERR_NOADMININFO = 423,          // "<server> :No administrative info available"
        ERR_FILEERROR = 424,            // ":File error doing <file op> on <file>"
        ERR_NONICKNAMEGIVEN = 431,      // ":No nickname given"
        ERR_ERRONEUSNICKNAME = 432,     // "<nick> :Erroneus nickname"
        ERR_NICKNAMEINUSE = 433,        // "<nick> :Nickname is already in use"
        ERR_NICKCOLLISION = 436,        // "<nick> :Nickname collision KILL"
        ERR_USERNOTINCHANNEL = 441,     // "<nick> <channel> :They aren't on that channel"
        ERR_NOTONCHANNEL = 442,         // "<channel> :You're not on that channel"
        ERR_USERONCHANNEL = 443,        // "<user> <channel> :is already on channel"
        ERR_NOLOGIN = 444,              // "<user> :User not logged in"
        ERR_SUMMONDISABLED = 445,       // ":SUMMON has been disabled"
        ERR_USERSDISABLED = 446,        // ":USERS has been disabled"
        ERR_NOTREGISTERED = 451,        // ":You have not registered"
        ERR_NEEDMOREPARAMS = 461,       // "<command> :Not enough parameters"
        ERR_ALREADYREGISTERED = 462,    // ":You may not reregister"
        ERR_NOPERMFORHOST = 463,        // ":Your host isn't among the privileged"
        ERR_PASSWDMISMATCH = 464,       // ":Password incorrect"
        ERR_YOUREBANNEDCREEP = 465,     // ":You are banned from this server"
        ERR_KEYSET = 467,               // "<channel> :Channel key already set"
        ERR_CHANNELISFULL = 471,        // "<channel> :Cannot join channel (+l)"
        ERR_UNKNOWNMODE = 472,          // "<char> :is unknown mode char to me"
        ERR_INVITEONLYCHAN = 473,       // "<channel> :Cannot join channel (+i)"
        ERR_BANNEDFROMCHAN = 474,       // "<channel> :Cannot join channel (+b)"
        ERR_BADCHANNELKEY = 475,        // "<channel> :Cannot join channel (+k)"
        ERR_NOPRIVILEGES = 481,         // ":Permission Denied- You're not an IRC operator"
        ERR_CHANOPRIVSNEEDED = 482,     // "<channel> :You're not channel operator"
        ERR_CANTKILLSERVER = 483,       // ":You cant kill a server!"
        ERR_NOOPERHOST = 491,           // ":No O-lines for your host"
        ERR_UMODEUNKNOWNFLAG = 501,     // ":Unknown MODE flag"
        ERR_USERSDONTMATCH = 502,       // ":Cant change mode for other users"

        RPL_UNKNOWNCODE = 000,
        RPL_WELCOME = 001,          // "Welcome to the Internet Relay Network <nick>!<user>@<host>"
        RPL_YOURHOST = 002,         // "Your host is <servername>, running version <ver>"
        RPL_CREATED = 003,          // "This server was created <date>"
        RPL_MYINFO = 004,           // "<servername> <version> <available user modes> <available channel modes>"
        RPL_BOUNCE = 005,           // "Try server <server name>, port <port number>"
        RPL_TRACELINK = 200,        // "Link <version & debug level> <destination> <next server>"
        RPL_TRACECONNECTING = 201,  // "Try. <class> <server>"
        RPL_TRACEANDSHAKE = 202,    // "H.S. <class> <server>"
        RPL_TRACEUNKNOWN = 203,     // "???? <class> [<client IP address in dot form>]"
        RPL_TRACEOPERATOR = 204,    // "Oper <class> <nick>"
        RPL_TRACEUSER = 205,        // "User <class> <nick>"
        RPL_TRACESERVER = 206,      // "Serv <class> <int>S <int>C <server> <nick!user|*!*>@<host|server>"
        RPL_TRACENEWTYPE = 208,     // "<newtype> 0 <client name>"
        RPL_STATSLINKINFO = 211,    // "<linkname> <sendq> <sent messages> <sent bytes> <received messages> <received bytes> <time open>"
        RPL_STATSCOMMANDS = 212,    // "<command> <count>"
        RPL_STATSCLINE = 213,       // "C <host> * <name> <port> <class>"
        RPL_STATSNLINE = 214,       // "N <host> * <name> <port> <class>"
        RPL_STATSILINE = 215,       // "I <host> * <host> <port> <class>"
        RPL_STATSKLINE = 216,       // "K <host> * <username> <port> <class>"
        RPL_STATSYLINE = 218,       // "Y <class> <ping frequency> <connect frequency> <max sendq>"
        RPL_ENDOFSTATS = 219,       // "<stats letter> :End of /STATS report"
        RPL_UMODEIS = 221,          // "<user mode string>"
        RPL_STASLLINE = 241,        // "L <hostmask> * <servername> <maxdepth>"
        RPL_STATSUPTIME = 242,      // ":Server Up %d days %d:%02d:%02d"
        RPL_STATSOLINE = 243,       // "O <hostmask> * <name>"
        RPL_STATSHLINE = 244,       // "H <hostmask> * <servername>"
        RPL_LUSERCLIENT = 251,      // ":There are <integer> users and <integer> invisible on <integer> servers"
        RPL_LUSEROP = 252,          // "<integer> :operator(s) online"
        RPL_LUSERUNKNOWN = 253,     // "<integer> :unknown connection(s)"
        RPL_LUSERCHANNELS = 254,    // "<integer> :channels formed"
        RPL_LUSERME = 255,          // ":I have <integer> clients and <integer> servers"
        RPL_ADMINME = 256,          // "<server> :Administrative info"
        RPL_ADMINLOC1 = 257,        // ":<admin info>"
        RPL_ADMINLOC2 = 258,        // ":<admin info>"
        RPL_ADMINEMAIL = 259,       // ":<admin info>"
        RPL_TRACELOG = 261,         // "File <logfile> <debug level>"

        RPL_AWAY = 301,             // "<nick> :<away message>"
        RPL_USERHOST = 302,         // ":[<reply>{<space><reply>}]"
        RPL_ISON = 303,             // ":[<nick> {<space><nick>}]"
        RPL_UNAWAY = 305,           // ":You are no longer marked as being away"
        RPL_NOWAWAY = 306,          // ":You have been marked as being away"
        RPL_WHOISUSER = 311,        // "<nick> <user> <host> * :<real name>"
        RPL_WHOISSERVER = 312,      // "<nick> <server> :<server info>"
        RPL_WHOISOPERATOR = 313,    // "<nick> :is an IRC operator"
        RPL_WHOWASUSER = 314,       // "<nick> <user> <host> * :<real name>"
        RPL_ENDOFWHO = 315,         // "<name> :End of /WHO list"
        RPL_WHOISIDLE = 317,        // "<nick> <integer> :seconds idle"
        RPL_ENDOFWHOIS = 318,       // "<nick> :End of /WHOIS list"
        RPL_WHOISCHANNELS = 319,    // "<nick> :{[@|+]<channel><space>}"
        RPL_LISTSTART = 321,        // "Channel :Users  Name"
        RPL_LIST = 322,             // "<channel> <# visible> :<topic>"
        RPL_LISTEND = 323,          // ":End of /LIST"
        RPL_CHANNELMODEIS = 324,    // "<channel> <mode> <mode params>"
        RPL_NOTOPIC = 331,          // "<channel> :No topic is set"
        RPL_TOPIC = 332,            // "<channel> :<topic>"
        RPL_INVITING = 341,         // "<channel> <nick>"
        RPL_SUMMONING = 342,        // "<user> :Summoning user to IRC"
        RPL_VERSION = 351,          // "<version>.<debuglevel> <server> :<comments>"
        RPL_WHOREPLY = 352,         // "<channel> <user> <host> <server> <nick> <H|G>[*][@|+] :<hopcount> <real name>"
        RPL_NAMESREPLY = 353,        // "<channel> :[[@|+]<nick> [[@|+]<nick> [...]]]"
        RPL_LINKS = 364,            // "<mask> <server> :<hopcount> <server info>"
        RPL_ENDOFLINKS = 365,       // "<mask> :End of /LINKS list"
        RPL_ENDOFNAMES = 366,       // "<channel> :End of /NAMES list"
        RPL_BANLIST = 367,          // "<channel> <banid>"
        RPL_ENDOFBANLIST = 368,     // "<channel> :End of channel ban list"
        RPL_ENDOFWHOWAS = 369,      // "<nick> :End of WHOWAS"
        RPL_INFO = 371,             // ":<string>"
        RPL_MOTD = 372,             // ":- <text>"
        RPL_ENDOFINFO = 374,        // ":End of /INFO list"
        RPL_MOTDSTART = 375,        // ":- <server> Message of the day - "
        RPL_ENDOFMOTD = 376,        // ":End of /MOTD command"
        RPL_YOUREOPER = 381,        // ":You are now an IRC operator"
        RPL_REHASHING = 382,        // "<config file> :Rehashing"
        RPL_TIME = 391,             // "<server> :<string showing server's local time>"
        RPL_USERSSTART = 392,       // ":UserID   Terminal  Host"
        RPL_USERS = 393,            // ":%-8s %-9s %-8s"
        RPL_ENDOFUSERS = 394,       // ":End of users"
        RPL_NOUSERS = 395,          // ":Nobody logged in"
    }

    public class Constants
    {
        public const char Bold = '\x2';
        public const char Color = '\x3';
        public const char CtcpChar = '\x1';
        public const char CtcpQuoteChar = '\x20';
        public const char Normal = '\xf';
        public const char Reverse = '\x16';
        public const char Underline = '\x1f';
    }
}