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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BlizzetaZero.Kernel
{
    using Scripts;

    public struct User
    {
        public bool banned;
        public string name;
        public string permission;

        private static string userdb = string.Empty;
        private static XDocument xDoc;

        public static User GetUser ( Streamer streamer, string name )
        {
            Load ( streamer.config );
            return GetUserProfile ( name );
        }

        public Permissions GetPermission ( )
        {
            User user = GetUserProfile ( name );
            if ( !user.banned )
                return ( Permissions ) Enum.Parse ( typeof ( Permissions ), permission );

            return Permissions.Guest;
        }

        public static void Load ( string dbLoc )
        {
            userdb = string.Format ( "{0}\\users.xml", dbLoc );
            xDoc = XDocument.Load ( userdb );
        }

        public static XElement GetUserXmlProfile ( string name, bool falseAdmin = false, Irc i = null )
        {
            XDocument xdc = xDoc;
            XElement xel = xdc.Descendants ( "Users" ).Descendants ( "User" ).Where ( ( x ) => x.Attribute ( "name" ).Value == name ).FirstOrDefault ( );

            if ( falseAdmin )
                i.SendMessage ( name, "You do not have permission to manipulate another user, only yourself." );

            return xel;
        }

        public static IEnumerable<User> ReadUsers ( string configDir )
        {
            Load ( configDir );
            List<User> users = new List<User> ( );

            IEnumerable<XElement> xList = from x in xDoc.Descendants ( "Users" ).Descendants ( "User" ) select x;

            foreach ( XElement xel in xList )
            {
                users.Add ( User.GetUserProfile ( xel.Attribute ( "name" ).Value ) );
            }

            return users.ToArray ( );
        }

        public static User GetUserProfile ( string nick )
        {
            XElement xel = GetUserXmlProfile ( nick );
            return new User ( )
            {
                name = xel.Attribute ( "name" ).Value,
                permission = xel.Attribute ( "permission" ).Value,
                banned = bool.Parse ( xel.Attribute ( "banned" ).Value ),

                //token = Token.Validate ( xel.Attribute ( "token" ).Value )
            };
        }

        public void modifyUser ( Permissions Permission, bool Banned )
        {
            XElement xNode = GetUserXmlProfile ( name );

            if ( xNode == null )
                throw new NullReferenceException ( string.Format ( "User {0} not found!", name ) );

            xNode.SetAttributeValue ( "permission", Permission );
            xNode.SetAttributeValue ( "banned", Banned );

            XAttribute[] xAttr = xNode.Attributes ( ).ToArray ( );

            Console.WriteLine ( "Name: {0}, Permission: {1}, Banned: {2}", xAttr[ 0 ].Value, xAttr[ 1 ].Value, xAttr[ 2 ].Value );
            xDoc.Save ( userdb );
            return;
        }
    }

    public struct Streamer
    {
        public string name;
        public string token;
        public bool banned;
        public string config;
        public int CommandLimit;
        public bool CommandsLimited;
        public User[] users;

        private static XDocument xDoc;
        private const string db = "Streamers.xml";

        private Streamer ( string Name )
        {
            name = string.Empty;
            token = string.Empty;
            banned = false;
            config = string.Empty;
            CommandLimit = 100;
            CommandsLimited = true;
            users = null;
            xDoc = GetDB ( );
        }

        public void AddCommand ( string Command, string Message )
        {
            xDoc = XDocument.Load ( string.Format ( "{0}\\config.xml", config ) );
            XElement xel = new XElement ( "Command" );
            XAttribute xat = new XAttribute ( "name", Command );
            XAttribute xms = new XAttribute ( "message", Message );
            xel.Add ( xat, xms );
            xDoc.Element ( "Config/Commands" ).Add ( xel );
            Save ( );
        }

        public static Streamer GetProfile ( string name )
        {
            Streamer str = new Streamer ( );
            XElement xel = str.GetXmlProfile ( name );

            return str.ToStreamer ( xel );
        }

        private Streamer ToStreamer ( XElement xel )
        {
            this.name = xel.Attribute ( "name" ).Value;
            this.token = xel.Attribute ( "token" ).Value;
            this.banned = bool.Parse ( xel.Attribute ( "banned" ).Value );
            this.config = xel.Attribute ( "config" ).Value;
            this.users = User.ReadUsers ( config ).ToArray ( );
            return this;
        }

        public XElement GetXmlProfile ( string name )
        {
            XDocument xdc = GetDB ( );
            XElement xel = xdc.Descendants ( "Streamers" ).Descendants ( "Streamer" ).Where ( ( x ) => x.Attribute ( "name" ).Value == name ).FirstOrDefault ( );

            return xel;
        }

        public static void CreateStreamerDatabase ( string defaultUser )
        {
            xDoc = new XDocument ( new XDeclaration ( "1.0", "utf-8", "yes" ) );
            XElement root = new XElement ( "Streamers" );
            xDoc.Add ( root );
            createProfile ( defaultUser, true, true );
            Save ( );
            CreateDefaultStreamerFiles ( GetProfile ( defaultUser ) );
        }

        public static XDocument GetDB ( )
        {
            return XDocument.Load ( db );
        }

        public static void Save ( )
        {
            xDoc.Save ( db );
        }

        public static Streamer[] GetStreamers ( )
        {
            xDoc = GetDB ( );
            List<Streamer> streamers = new List<Streamer> ( );
            foreach ( XElement x in xDoc.Element ( "Streamers" ).Elements ( ) )
            {
                streamers.Add ( GetProfile ( x.Attribute ( "name" ).Value ) );
            }

            return streamers.ToArray ( );
        }

        public static bool StreamerExists ( string name )
        {
            // Logic to check for existing user in XML DB 
            Streamer s = GetProfile ( name );
            return string.IsNullOrEmpty ( s.name );
        }

        private static void createProfile ( string name, bool staff, bool defaultUser = false )
        {
            //Irc.Format ( "<Accounts> Formatting and adding {0}", ConsoleColor.Green, name );
            string streamDir = string.Empty;

            if ( staff )
            {
                Console.WriteLine ( "Using Staff Directory" );
                streamDir = string.Format ( "Streamers\\Staff\\{0}", name );
            }
            else
            {
                Console.WriteLine ( "Using Normal Directory" );
                streamDir = string.Format ( "Streamers\\Normal\\{0}", name );
            }

            if ( !defaultUser && StreamerExists ( name ) )
            {
                Console.WriteLine ( "User {0} has already been registered!", name );
                return;
            }

            Directory.CreateDirectory ( streamDir );
            XAttribute xName = new XAttribute ( "name", name );
            XAttribute xToken = new XAttribute ( "token", string.Empty );
            XAttribute xBanned = new XAttribute ( "banned", false );
            XAttribute xConfig = new XAttribute ( "config", streamDir );
            XElement xUser = new XElement ( "Streamer" );

            xUser.Add ( xName, xToken, xBanned, xConfig );

            XElement xUsers = xDoc.Element ( "Streamers" );
            xUsers.Add ( xUser );

            XDocument xUsersDB = new XDocument ( new XDeclaration ( "1.0", "utf-8", "yes" ) );
            XElement xUserDB = new XElement ( "User" );
            User sUser = new User ( ) { name = name, banned = false, permission = Permissions.Administrator.ToString ( ) };
            xUserDB.SetAttributeValue ( "name", sUser.name );
            xUserDB.SetAttributeValue ( "banned", sUser.banned.ToString ( ) );
            xUserDB.SetAttributeValue ( "permission", sUser.permission );

            xUsersDB.Add ( xUserDB );
            xUsersDB.Save ( string.Format ( "{0}\\users.xml", streamDir ) );
        }

        private static void CreateDefaultStreamerFiles ( Streamer streamer )
        {
            XDocument xConfig = new XDocument ( new XDeclaration ( "1.0", "utf-8", "yes" ) );

            XElement xRoot = new XElement ( "Config" );
            XElement xCommands = new XElement ( "Commands" );
            XElement xBans = new XElement ( "Bans" );

            XElement xFiles = new XElement ( "Files" );
            XElement xFile = new XElement ( "Config" );
            xFile.SetAttributeValue ( "path", string.Format ( "{0}\\users.xml", streamer.config ) );

            xFiles.Add ( xFile );
            xRoot.SetAttributeValue ( "owner", streamer.name );
            xRoot.Add ( xCommands, xBans, xFiles );
            xConfig.Add ( xRoot );
            xConfig.Save ( string.Format ( "{0}\\config.xml", streamer.config ) );
        }

        public static void CreateProfile ( string name, bool staff = false )
        {
            xDoc = GetDB ( );
            createProfile ( name, staff );
            Save ( );
            CreateDefaultStreamerFiles ( GetProfile ( name ) );
        }

        // Modify Token, banned, and permanent 
        public void Modify ( string Token, bool Banned, bool Permanent )
        {
            xDoc = GetDB ( );

            XElement xNode = GetXmlProfile ( name );

            if ( xNode == null )
                throw new NullReferenceException ( string.Format ( "User {0} not found!", name ) );

            xNode.SetAttributeValue ( "token", Token );
            xNode.SetAttributeValue ( "banned", banned );
            xNode.SetAttributeValue ( "permanent", Permanent );

            Save ( );
            return;
        }
    }

    #region Core Commands

    public class CoreCommands
    {
        private static Dictionary<string, Func<Irc, string, string, object[], int>> commands = new Dictionary<string, Func<Irc, string, string, object[], int>> ( );

        private static bool ignoreBan = false, ignoreSay = false, ignoreKick = false, ignoreEval = false;

        private static int overridekey = 0;

        private static string streamersDB = Irc.StartupPath + "\\Streamers.xml";

        public static Dictionary<string, Func<Irc, string, string, object[], int>> Commands { get { return commands; } }

        public static string StreamersDB { get { return streamersDB; } }

        // Must be set 
        public static Irc irc { get; set; }

        public static void AddCommand ( string command, Func<Irc, string, string, object[], int> function )
        {
            IrcReply.FormatMessage ( string.Format ( "Including {0}", command ), ConsoleColor.Green );
            commands.Add ( command, function );
        }

        public static string CheckS ( string name )
        {
            return name.EndsWith ( "s" ) ? name + "'" : name + "'s";
        }

        public static void Execute ( string command, Irc irc, Channel chan, string callee, object[] args )
        {
            Func<Irc, string, string, object[], int> func = commands[ command ];
            if ( chan != null )
                func.Invoke ( irc, chan.Name, callee, args );
            else
                func.Invoke ( irc, string.Empty, callee, args );
        }

        private static ScriptContext context = null;
        private static ScriptEngine engine = null;
        private static List<string> src = null;

        private static string[] specials = {
                                     "I'm sorry {0}, but I'm afraid I can't let you do that.",
                                     "No, {0}, you may not do that.",
                                     "OI! Mind yer damn Business! Sheesh, {0} can be nosey sometimes..."
                                           };

        private static string SpecialMessage ( string nick )
        {
            Random r = new Random ( );
            return string.Format ( specials[ r.Next ( 0, specials.GetUpperBound ( 0 ) ) ], nick );
        }

        private static void SendMessage ( Irc irc, string channel, string nick, string message, params object[] args )
        {
            if ( string.IsNullOrEmpty ( channel ) )
                irc.SendMessage ( nick, string.Format ( message, args ) );
            else
                irc.GetChannel ( channel ).SendMessage ( string.Format ( message, args ) );
        }

        public static void IncludeBuiltInCommands ( )
        {
            #region check

            AddCommand ( "check", ( i, c, n, o ) =>
            {
                Channel chan = i.GetChannel ( c );
                Permissions p = User.GetUser ( chan.Streamer, n ).GetPermission ( );
                Dictionary<string, Func<int>> subcmd = new Dictionary<string, Func<int>> ( );
                subcmd.Add ( "permission", ( ) =>
                {
                    // TODO: Add permissions check 
                    i.GetChannel ( c ).SendMessage ( string.Format ( "{0}, Your Permissions are \"{1}\"", n, p ) );
                    return 0;
                } );

                subcmd.Add ( "time", ( ) =>
                {
                    // TODO: Add configuration for callee 
                    DateTime dt = DateTime.Now;
                    Irc.Format ( "Current Time: {0:dddd MMMM dd, yyyy} at {0:HH:mm:ss}", ConsoleColor.Yellow, dt );
                    i.GetChannel ( c ).SendMessage ( string.Format ( "{0} time is {1:dddd MMMM dd, yyyy} at {1:HH:mm:ss}", CheckS ( i.Owner ), dt ) );
                    return 0;
                } );
                subcmd.Add ( "uptime", ( ) =>
                {
                    Irc.Format ( "Uptime: {0}", ConsoleColor.DarkGreen );
                    i.GetChannel ( c ).SendMessage ( string.Format ( "{0}, I've been up for {1} {2}, {3} {4}, {5} {6}, and {7} {8}" ) );
                    return 0;
                } );

                try
                {
                    subcmd[ o[ 0 ] as string ].Invoke ( );
                }
                catch ( Exception ex )
                {
                    i.SendMessage ( c, "Invalid command" );
                }

                return 0;
            } );

            #endregion check

            #region clear

            AddCommand ( "clear", ( i, c, n, o ) =>
            {
                Channel chan = i.GetChannel ( c );
                Permissions p = User.GetUser ( chan.Streamer, n ).GetPermission ( );
                if ( p <= Permissions.Root )
                    Console.Clear ( );
                else
                    i.SendMessage ( n, "Let me clean that nose of yours... in other words... \"MIND YOUR DAMN BUSINESS!\" o_o" );
                return 0;
            } );

            #endregion clear

            #region get

            AddCommand ( "get", ( i, c, n, o ) =>
            {
                Permissions p = User.GetUserProfile ( n ).GetPermission ( );
                Dictionary<string, Func<int>> subcmd = new Dictionary<string, Func<int>> ( );

                subcmd.Add ( "say", ( ) =>
                {
                    i.SendMessage ( n, ignoreSay ? "I'm ignoring say." : "I'm not ignoring say." );
                    return 0;
                } );

                if ( p <= Permissions.Operator )
                    subcmd[ o[ 0 ] as string ].Invoke ( );
                else
                    i.GetChannel ( c ).SendMessage ( SpecialMessage ( n ) );
                return 0;
            } );

            #endregion get

            #region set

            AddCommand ( "set", ( i, c, n, o ) =>
            {
                Permissions p = User.GetUserProfile ( n ).GetPermission ( );
                Dictionary<string, Func<int>> subcmd = new Dictionary<string, Func<int>> ( );

                subcmd.Add ( "say", ( ) =>
                {
                    string val = ( o[ 1 ] as string ).ToLower ( );
                    if ( val == "on" || val == "true" )
                    {
                        ignoreSay = false;
                    }
                    else if ( val == "off" || val == "false" )
                    {
                        ignoreSay = true;
                    }
                    return 0;
                } );

                return 0;
            } );

            #endregion set

            #region help

            AddCommand ( "help", ( i, c, n, o ) =>
            {
                Permissions p = User.GetUserProfile ( n ).GetPermission ( );

                // + help [-mpuv] [command] [subcommand] 
                /*
                 *  Options:
                 *      -m | --more         : More Command
                 *      -p | --permissions  : Permissions Command
                 *      -u | --usage        : Usage Command
                 *      -v | --version      : Version Command
                 */

                return 0;
            } );

            #endregion help

            #region join

            AddCommand ( "join", ( i, c, n, o ) =>
            {
                Channel chan = i.GetChannel ( c );
                Permissions p = User.GetUser ( chan.Streamer, n ).GetPermission ( );
                if ( p <= Permissions.Administrator )
                {
                    bool chanExists = i.Channels.Exists ( new Predicate<Channel> ( ( ch ) =>
                    {
                        return ch.Name == o[ 0 ] as string;
                    } ) );

                    if ( !chanExists )
                    {
                        if ( o.Length > 1 )
                        {
                            i.Join ( o[ 0 ] as string, o[ 1 ] as string );
                            return 0;
                        }
                        else if ( o.Length > 0 )
                        {
                            i.Join ( o[ 0 ] as string, string.Empty );
                            return 0;
                        }
                        else
                            i.SendMessage ( c, "I can't join without a channel name" );
                    }
                    else
                    {
                        i.SendMessage ( c, string.Format ( "I'm already in {0}!", o[ 0 ] as string ) );
                    }
                }
                else i.SendMessage ( n, SpecialMessage ( n ) );
                return -1;
            } );

            #endregion join

            #region list

            AddCommand ( "list", ( i, c, n, o ) =>
            {
                Channel chan = i.GetChannel ( c );
                Permissions p = User.GetUser ( chan.Streamer, n ).GetPermission ( );

                if ( p <= Permissions.Administrator )
                {
                    Channel[] chans = i.Channels.ToArray ( );
                    i.SendMessage ( n, "I am in these channels:" );
                    foreach ( Channel channel in chans )
                    {
                        i.SendMessage ( n, string.Format ( channel.Name ) );
                    }
                }
                else
                    i.GetChannel ( c ).SendMessage ( SpecialMessage ( n ) );
                return 0;
            } );

            #endregion list

            #region Topic

            AddCommand ( "topic", ( i, c, n, o ) =>
            {
                Channel chan = i.GetChannel ( c );
                Permissions p = User.GetUser ( chan.Streamer, n ).GetPermission ( );
                if ( p <= Permissions.Operator )
                {
                    string msg = string.Join ( " ", o as string[], 1, o.Length - 1 );
                }

                return 0;
            } );

            #endregion Topic

            #region Playlist

            AddCommand ( "playlist", ( i, c, n, o ) =>
            {
                Channel chan = i.GetChannel ( c );
                Permissions p = User.GetUser ( chan.Streamer, n ).GetPermission ( );
                if ( p <= Permissions.Administrator )
                    i.GetChannel ( c ).SendMessage ( string.Format ( "{0} playlist: http://playlist.blizzeta.net", CheckS ( i.Owner ) ) );
                return 0;
            } );

            #endregion Playlist

            #region Game

            AddCommand ( "game", ( i, c, n, o ) =>
            {
                Channel chan = i.GetChannel ( c );
                Permissions p = User.GetUser ( chan.Streamer, n ).GetPermission ( );
                if ( p <= Permissions.User )
                {
                }
                return 0;
            } );

            #endregion Game

            #region account

            AddCommand ( "acc", ( i, c, n, o ) =>
            {
                Channel chan = i.GetChannel ( c );
                Permissions p = User.GetUser ( chan.Streamer, n ).GetPermission ( );

                Streamer streamer = Streamer.GetProfile ( c.Replace ( "#", "" ) );
                string userdb = string.Format ( "{0}\\users.xml" );
                XDocument xDoc = XDocument.Load ( userdb );
                i.SendNotice ( n, string.Format ( "Please see {0} or a mod for assistance.", streamer.name ) );

                #region depcode WORK ON THIS

                Dictionary<string, Func<int>> subcmd = new Dictionary<string, Func<int>> ( );

                subcmd.Add ( "streamer", ( ) =>
                {
                    Dictionary<string, Func<int>> subsubcmd = new Dictionary<string, Func<int>> ( );

                    subsubcmd.Add ( "register", new Func<int> ( ( ) =>
                    {
                        string channel = c.Replace ( "#", "" );
                        string botnick = Streamer.GetProfile ( i.Nick ).name;
                        if ( p <= Permissions.Administrator && channel.Equals ( botnick ) )
                        {
                            string nc = o[ 3 ] as string;
                            Streamer.CreateProfile ( nc );
                            i.GetChannel ( c ).SendMessage ( string.Format ( "Streamer \"{0}\" has registered!", nc ) );
                            i.Join ( string.Format ( "#{0}", nc ) );
                        }
                        else
                        {
                            i.GetChannel ( c ).SendMessage ( string.Format ( "{0}, I can only register streamers on my profile and with the approval of an Administrator", n ) );
                        }
                        return 0;
                    } ) );

                    subsubcmd[ o[ 2 ] as string ].Invoke ( );

                    return 0;
                } );

                subcmd.Add ( "register", ( ) =>
                {
                    string uname = ( ( o.Length > 1 ? ( string.IsNullOrEmpty ( o[ 1 ] as string ) ? n : o[ 1 ] as string ) : n ) );
                    Irc.Format ( "<Accounts> Formatting and adding {0}", ConsoleColor.Green, uname );

                    User u = chan.Streamer.users.Single ( ( U ) =>
                    {
                        bool b = ( U.name == uname );

                        if ( b )
                        {
                            chan.SendMessage ( string.Format ( "{0} is already registered! " ) );
                        }

                        return b;
                    } );

                    XAttribute xName = new XAttribute ( "name", o.Length > 1 ? o[ 1 ] as string : n );
                    XAttribute xPerm = new XAttribute ( "permission", ( p <= Permissions.Root ?
                        ( o.Length > 2 ?
                            Enum.GetName ( typeof ( Permissions ), o[ 2 ] as string )
                            : Enum.GetName ( typeof ( Permissions ), Permissions.User ) )
                        : Enum.GetName ( typeof ( Permissions ), Permissions.User ) ) );
                    XAttribute xBanned = new XAttribute ( "banned", false );

                    //XAttribute xToken = new XAttribute ( "token",
                    //    Token.Generate ( xName.Value, c, ( Permissions ) Enum.Parse ( typeof ( Permissions ), xPerm.Value ) ).TokenData );
                    XElement xUser = new XElement ( "User" );
                    xUser.Add ( xName, xPerm, xBanned );//, xToken );

                    XElement xUsers = xDoc.Element ( "Users" );
                    xUsers.Add ( xUser );

                    xDoc.Save ( userdb );
                    i.GetChannel ( c ).SendMessage ( string.Format ( "Welcome {0}!", uname ) );

                    return 0;
                } );

                subcmd.Add ( "ban", ( ) =>
                {
                    if ( p <= Permissions.Operator )
                    {
                        XElement xel = xDoc.Descendants ( "Users" )
                            .Descendants ( "User" )
                            .Where ( ( x ) => x.Attribute ( "name" )
                                .Value == o[ 1 ] as string ).FirstOrDefault ( );

                        xel.SetAttributeValue ( "banned", true );

                        xDoc.Save ( userdb );
                    }
                    return 0;
                } );

                subcmd.Add ( "unban", ( ) =>
                {
                    if ( p <= Permissions.Operator )
                    {
                        XElement xel = xDoc.Descendants ( "Users" )
                            .Descendants ( "User" )
                            .Where ( ( x ) => x.Attribute ( "name" )
                                .Value == o[ 1 ] as string ).FirstOrDefault ( );

                        xel.SetAttributeValue ( "banned", false );

                        xDoc.Save ( userdb );
                    }
                    return 0;
                } );

                // 0 1 2 promote user permission 
                subcmd.Add ( "promote", ( ) =>
                {
                    Dictionary<string, Func<int>> perms = new Dictionary<string, Func<int>> ( );
                    XElement xel = xDoc.Descendants ( "Users" )
                        .Descendants ( "User" )
                        .Where ( ( x ) => x.Attribute ( "name" )
                            .Value == o[ 1 ] as string ).FirstOrDefault ( );
                    perms.Add ( "administrator", new Func<int> ( ( ) =>
                    {
                        if ( p == Permissions.Root )
                        {
                            xel.SetAttributeValue ( "permission", Permissions.Administrator );

                            xDoc.Save ( userdb );
                        }
                        else
                            i.SendMessage ( n, "I'm sorry, Only Root Users can set Admin on other users." );
                        return 0;
                    } ) );

                    perms.Add ( "operator", new Func<int> ( ( ) =>
                    {
                        if ( p <= Permissions.Administrator )
                        {
                            xDoc.Descendants ( "Users" )
                                .Descendants ( "User" )
                                .Where ( ( x ) => x.Attribute ( "name" )
                                    .Value == o[ 1 ] as string ).FirstOrDefault ( );

                            xel.SetAttributeValue ( "permission", Permissions.Operator );

                            xDoc.Save ( userdb );
                        }
                        else
                            i.SendMessage ( n, "I'm sorry, Only Administrative Users can set Operator on other users." );

                        return 0;
                    } ) );

                    try
                    {
                        if ( p <= Permissions.Administrator )
                        {
                            perms[ o[ 2 ] as string ].Invoke ( );
                        }
                    }
                    catch ( Exception )
                    {
                        i.SendNotice ( n, "+acc promote <user> <permission>" );
                    }
                    return 0;
                } );

                subcmd.Add ( "demote", ( ) =>
                {
                    if ( p <= Permissions.Administrator )
                    {
                        XElement xel = xDoc.Descendants ( "Users" )
                            .Descendants ( "User" )
                            .Where ( ( x ) => x.Attribute ( "name" )
                                .Value == o[ 1 ] as string ).FirstOrDefault ( );
                        Permissions perm = ( Permissions ) Enum.Parse ( typeof ( Permissions ), xel.Attribute ( "permission" ).Value );

                        // We check to see if an Administrator is trying to revoke Administrator or
                        // Root. If Administrator < Operator <-- TRUE If Administrator <
                        // Administrator <-- FALSE If Administrator < Root <-- FALSE If Root < Root
                        // <-- FALSE

                        if ( xel.Attribute ( "name" ).Value == i.Owner )
                        {
                            i.SendNotice ( n, string.Format ( "God-Mode: De-Ranking {0}", xel.Attribute ( "name" ).Value ) );
                            xel.SetAttributeValue ( "permission", Permissions.User );
                        }
                        else if ( p < perm )
                        {
                            xel.SetAttributeValue ( "permission", Permissions.User );
                        }
                        else
                        {
                            i.SendNotice ( n, "You cannot De-Rank either yourself, a higher user, or someone on the same rank as yourself." );
                        }

                        xDoc.Save ( streamer.config );
                    }
                    return 0;
                } );

                subcmd.Add ( "bye", ( ) =>
                {
                    XElement xel = o.Length < 2 ? User.GetUserXmlProfile ( n ) : ( p <= Scripts.Permissions.Administrator ? User.GetUserXmlProfile ( o[ 1 ] as string ) : User.GetUserXmlProfile ( n, true, i ) );
                    string name = xel.Attribute ( "name" ).Value;

                    Irc.Format ( "Selected {0}!", ConsoleColor.DarkGreen, name );

                    if ( p <= Permissions.Administrator )
                        i.GetChannel ( c ).SendMessage ( string.Format ( "Account {0} has been deregistered by an Administrator!", name ) );
                    else
                        i.GetChannel ( c ).SendMessage ( string.Format ( "Sorry to see you go {0}! :(", name ) );

                    xel.Remove ( );

                    xDoc.Save ( userdb );

                    return 0;
                } );

                try
                {
                    if ( System.IO.File.Exists ( userdb ) )
                    {
                        try
                        {
                            subcmd[ o[ 0 ] as string ].Invoke ( );
                        }
                        catch ( Exception ex )
                        {
                            i.SendMessage ( c, "Invalid command" );
                        }
                    }
                    else
                        i.GetChannel ( c ).SendMessage ( string.Format ( "There is no Database. Use +createdb to send an Access code to {0} for a new File.", i.Owner ) );
                }
                catch ( Exception ex )
                {
                    i.GetChannel ( c ).SendMessage ( ex.Message );
                    Console.WriteLine ( ex );
                }

                #endregion depcode WORK ON THIS

                return 0;
            } );

            #endregion account

            #region part

            AddCommand ( "part", ( i, c, n, o ) =>
            {
                Permissions p = User.GetUserProfile ( n ).GetPermission ( );
                if ( p <= Permissions.Administrator )
                {
                    string[] format = o as string[];
                    string channel = format[ 0 ];
                    i.Part ( channel, string.Join ( " ", format, 1, format.Length - 1 ) );
                }
                else
                    i.GetChannel ( c ).SendMessage ( SpecialMessage ( n ) );
                return 0;
            } );

            #endregion part

            #region quit

            AddCommand ( "quit", ( i, c, n, o ) =>
            {
                Permissions p = User.GetUserProfile ( n ).GetPermission ( );
                if ( p <= Permissions.Administrator )
                    i.Disconnect ( string.Join ( " ", o as string[] ) );
                else
                    i.GetChannel ( c ).SendMessage ( SpecialMessage ( n ) );
                return 0;
            } );

            #endregion quit

            #region raw

            AddCommand ( "raw", ( i, c, n, o ) =>
            {
                Permissions p = User.GetUserProfile ( n ).GetPermission ( );
                if ( p <= Permissions.Root )
                    i.Raw ( string.Join ( " ", o ) );
                else
                    SendMessage ( i, c, n, SpecialMessage ( n ) );
                return 0;
            } );

            #endregion raw

            #region reboot

            AddCommand ( "reboot", ( i, c, n, o ) =>
            {
                Permissions p = User.GetUserProfile ( n ).GetPermission ( );

                if ( p <= Permissions.Administrator )
                {
                    System.Diagnostics.Process pr = new System.Diagnostics.Process ( )
                    {
                        StartInfo = new System.Diagnostics.ProcessStartInfo ( )
                        {
                            FileName = System.Reflection.Assembly.GetExecutingAssembly ( ).GetName ( ).Name + ".exe"
                        }
                    };
                    i.Disconnect ( "Rebooting!" );
                    pr.Start ( );
                    Environment.Exit ( 0 );
                }
                else
                    i.SendMessage ( n, "You may not reboot me. Only an Administrator or the Owner may do so." );
                return 0;
            } );

            #endregion reboot

            #region say

            AddCommand ( "say", ( i, c, n, o ) =>
            {
                Permissions p = User.GetUserProfile ( n ).GetPermission ( );
                if ( p <= Permissions.User )
                {
                    if ( !ignoreSay )
                        SendMessage ( i, c, n, string.Join ( " ", o as string[] ) );
                }
                else
                    SendMessage ( i, c, n, SpecialMessage ( n ) );

                return 0;
            } );

            #endregion say

            #region Uptime

            AddCommand ( "uptime", ( i, c, n, o ) =>
            {
                i.GetChannel ( c ).SendMessage ( string.Format ( "I have been online for {0}", Global.FormatUptime ( ) ) );
                return 0;
            } );

            #endregion Uptime

            #region version

            AddCommand ( "version", ( i, c, n, o ) =>
            {
                // No Permissions needed 
                Dictionary<string, Func<int>> arguments = new Dictionary<string, Func<int>> ( );
                arguments.Add ( "core", new Func<int> ( ( ) =>
                {
                    i.GetChannel ( c ).SendMessage ( string.Format ( "{0}; Core Version {1}", Global.Title, Global.Core ) );
                    return 0;
                } ) );

                arguments.Add ( "scripts", new Func<int> ( ( ) =>
                {
                    i.GetChannel ( c ).SendMessage ( string.Format ( "{0}; Scripts Version {1}", Global.Title, Global.Scripts ) );
                    return 0;
                } ) );

                try
                {
                    arguments[ o[ 0 ] as string ].Invoke ( );
                }
                catch
                {
                    i.GetChannel ( c ).SendMessage ( string.Format ( "{0}; For more information, see \"core\" and \"scripts\"", Global.Title ) );
                }
                return 0;
            } );

            #endregion version
        }

        public static void ReleaseCommand ( string command )
        {
            IrcReply.FormatMessage ( string.Format ( "Excluding {0}", command ), ConsoleColor.DarkRed );
            commands.Remove ( command );
        }
    }

    #endregion Core Commands

    #region Token Data

    /**
    public class Token
    {
        private static string code = "blizzetazero70iamopensourcemadebyblizzardo1dontabuse(c)2014blizzardo1&blizzetatechnologiesltd.";
        private DateTime creation;
        private DateTime expiration;
        private string nick;
        private string channel;
        private Permissions permission;
        private string tokendata;

        public string TokenData { get { return tokendata; } }

        private Token ( string nick, string channel, Permissions permission )
        {
            creation = DateTime.UtcNow;
            expiration = creation.AddMonths ( 1 );
            this.nick = nick;
            this.channel = channel;
            this.permission = permission;
        }

        public static Token Generate ( string nick, string channel, Permissions permission )
        {
            Token t = new Token ( nick, channel, permission );
            string tok = string.Format ( "{0};{1};{2};{3};{4}", t.nick, t.channel, t.permission, t.creation, t.expiration );
            string enc = ProductKey.KeyCipher.Encrypt ( tok, code );
            t.tokendata = Convert.ToBase64String ( Encoding.Unicode.GetBytes ( enc ) );
            return t;
        }

        public static Token Validate ( string Key )
        {
            byte[] bKey = Convert.FromBase64String ( Key );
            string nKey = Encoding.Unicode.GetString ( bKey );
            nKey = ProductKey.KeyCipher.Decrypt ( nKey, code );
            string[] data = nKey.Split ( ';' );
            Token tok = new Token (
                data[ 0 ],
                data[ 1 ],
                ( Permissions ) Enum.Parse ( typeof ( Permissions ), data[ 2 ] ) )
                {
                    creation = DateTime.Parse ( data[ 3 ] ),
                    expiration = DateTime.Parse ( data[ 4 ] )
                };
            if ( tok.expiration <= DateTime.Now )
                return null;
            else
                return tok;
        }
    }*/

    public class ProductKey
    {
        private string productID;

        public string ProductID { get { return productID; } }

        public static bool ActivateKey ( string key, out string[] decrypted )
        {
            string[] data = KeyCipher.Decrypt ( key, "blizzetazero70iamopensourcemadebyblizzardo1dontabuse(c)2014blizzardo1" ).Split ( ':' );
            if ( data.Length == 3 )
            {
                IrcReply.FormatMessage ( string.Format ( "Nick: {0}\r\nChannel: {1}\r\nDate: {2:dddd MMMM dd, yyyy} at {2:HH:mm:ss}", data[ 0 ], data[ 1 ], DateTime.FromBinary ( long.Parse ( data[ 2 ] ) ) ), ConsoleColor.DarkGray, true );
                decrypted = data;
                return true;
            }

            decrypted = null;
            return false;
        }

        public static ProductKey GenerateProductKey ( string nick, string channel )
        {
            DateTime dt = DateTime.Now;
            string data = string.Format ( "{0}:{1}:{2}", nick, channel, dt.ToBinary ( ) );

            return new ProductKey ( ) { productID = KeyCipher.Encrypt ( data, "blizzetazero70iamopensourcemadebyblizzardo1dontabuse(c)2014blizzardo1" ) };
        }

        public static class KeyCipher
        {
            // This constant string is used as a "salt" value for the PasswordDeriveBytes function
            // calls. This size of the IV (in bytes) must = (keysize / 8). Default keysize is 256,
            // so the IV must be 32 bytes long. Using a 16 character string here gives us 32 bytes
            // when converted to a byte array.
            private const string initVector = "tu89geji340t89u2";

            // This constant is used to determine the keysize of the encryption algorithm. 
            private const int keysize = 256;

            public static string Decrypt ( string cipherText, string passPhrase )
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes ( initVector );
                byte[] cipherTextBytes = Convert.FromBase64String ( cipherText );
                PasswordDeriveBytes password = new PasswordDeriveBytes ( passPhrase, null );
                byte[] keyBytes = password.GetBytes ( keysize / 8 );
                RijndaelManaged symmetricKey = new RijndaelManaged ( );
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor ( keyBytes, initVectorBytes );
                MemoryStream memoryStream = new MemoryStream ( cipherTextBytes );
                CryptoStream cryptoStream = new CryptoStream ( memoryStream, decryptor, CryptoStreamMode.Read );
                byte[] plainTextBytes = new byte[ cipherTextBytes.Length ];
                int decryptedByteCount = cryptoStream.Read ( plainTextBytes, 0, plainTextBytes.Length );
                memoryStream.Close ( );
                cryptoStream.Close ( );
                return Encoding.UTF8.GetString ( plainTextBytes, 0, decryptedByteCount );
            }

            public static string Encrypt ( string plainText, string passPhrase )
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes ( initVector );
                byte[] plainTextBytes = Encoding.UTF8.GetBytes ( plainText );
                PasswordDeriveBytes password = new PasswordDeriveBytes ( passPhrase, null );
                byte[] keyBytes = password.GetBytes ( keysize / 8 );
                RijndaelManaged symmetricKey = new RijndaelManaged ( );
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor ( keyBytes, initVectorBytes );
                MemoryStream memoryStream = new MemoryStream ( );
                CryptoStream cryptoStream = new CryptoStream ( memoryStream, encryptor, CryptoStreamMode.Write );
                cryptoStream.Write ( plainTextBytes, 0, plainTextBytes.Length );
                cryptoStream.FlushFinalBlock ( );
                byte[] cipherTextBytes = memoryStream.ToArray ( );
                memoryStream.Close ( );
                cryptoStream.Close ( );
                return Convert.ToBase64String ( cipherTextBytes );
            }
        }
    }

    #endregion Token Data
}