using BreakerDev.Imports32;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Blizzeta.Twitch
{
    public class Auth
    {
        private static MessageServer _server;

        public static void SetToken ( string token )
        {
            Global.authToken = token;
        }

        public static void Blast ( string text, params object[] args )
        {
            Imports.User32.DisplayMessage ( IntPtr.Zero, string.Format ( text, args ), null, Imports.User32.MessageBoxOptions.IconError | Imports.User32.MessageBoxOptions.Ok );
        }

        public static bool Authenticate ( )
        {
            TwitchPassword pt = auth ( );
            if ( pt == null )
            {
                Imports.User32.DisplayMessage ( IntPtr.Zero, "Couldn't Authenticate!", null, Imports.User32.MessageBoxOptions.Ok | Imports.User32.MessageBoxOptions.IconError );
                return false;
            }
            else
            {
                Console.WriteLine ( "AccessCode: {0}", pt.access_token );
                SetToken ( pt.access_token );
                return true;
            }
        }

        public static bool Authenticate ( string Token )
        {
            // Example Token: qia35stw0yxsll4kjsx085o9liavwyc 
            return false;
        }

        public static async Task<string> AuthorizeUser ( )
        {
            _server = new MessageServer ( 5605 );
            return await Task<string>.FromResult ( ParseAuthorization ( ) );
        }

        private static string ParseAuthorization ( )
        {
            var builder = new UriBuilder ( Global.OAuth );
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString ( builder.Query );
            query[ "response_type" ] = "token";
            query[ "client_id" ] = Global.APIKey;
            query[ "redirect_uri" ] = Global.Redirect;
            query[ "scope" ] = "user_read user_blocks_edit user_blocks_read user_follows_edit channel_read channel_editor channel_commercial channel_stream channel_subscriptions user_subscriptions chat_login channel_check_subscription";

            builder.Query = query.ToString ( );
            return builder.ToString ( ).Replace ( "+", "%20" );
        }

        private static TwitchPassword auth ( )
        {
            try
            {
                /*
                    https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=r8829yrhzewp4gwopt9gh90py0s8p3p&redirect_uri=http://integrationservices.blizzeta.net&scope=user_read%20user_blocks_edit%20user_blocks_read%20user_follows_edit%20channel_read%20channel_editor%20channel_commercial%20channel_stream%20channel_subscriptions%20user_subscriptions%20chat_login%20channel_check_subscription
                */

                //Process.Start ( Global.AuthorizationFullPermissions );
                string url = ParseAuthorization ( );

                X509Certificate2 cert = DevDefined.OAuth.Tests.TestCertificates.OAuthTestCertificate ( );

                OAuthConsumerContext context = new OAuthConsumerContext
                {
                    ConsumerKey = "api.twitch.tv",
                    SignatureMethod = SignatureMethod.RsaSha1,
                    Key = cert.PrivateKey
                };

                IOAuthSession session = new OAuthSession ( context, null, url, null ).WithQueryParameters ( new { scope = Global.Redirect } );
                IToken request = session.GetRequestToken ( );
                string authorize = session.GetUserAuthorizationUrlForToken ( request, Global.Redirect );
                IToken access = session.ExchangeRequestTokenForAccessToken ( request );
                string response = session.Request ( ).Get ( ).ForUrl ( Global.AuthorizationFullPermissions ).ToString ( );

                Console.WriteLine ( response );
                TwitchPassword pt = JsonConvert.DeserializeObject<TwitchPassword> ( response );
                return pt;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( "Error: {0}", ex.Message );
                if ( ex.InnerException != null )
                {
                    Console.WriteLine ( "Inner Error: {0}", ex.InnerException.Message );
                }
            }
            return null;
        }
    }
}