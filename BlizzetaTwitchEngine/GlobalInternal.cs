using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blizzeta.Twitch
{
    internal enum GrantType
    {
        password
    }

    internal enum Scope
    {
        user_read,
        user_blocks_edit,
        user_blocks_read,
        user_follows_edit,
        channel_read,
        channel_editor,
        channel_commercial,
        channel_stream,
        channel_subscriptions,
        user_subscriptions,
        channel_check_subscriptions,
        chat_login
    };

    public class Global
    {
        internal const string APIKey = "r8829yrhzewp4gwopt9gh90py0s8p3p";
        internal const string APISecret = "2ef307h9g6wh90dkn5oe75a62em7iwf";
        internal const string OAuthToken = "https://api.twitch.tv/kraken/oauth2/token";
        internal const string OAuth = "https://api.twitch.tv/kraken/oauth2/authorize";
        internal const string Redirect = "http://localhost:5605";
        internal const string AuthorizationFullPermissions = "https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=r8829yrhzewp4gwopt9gh90py0s8p3p&redirect_uri=http://localhost&scope=user_read%20user_blocks_edit%20user_blocks_read%20user_follows_edit%20channel_read%20channel_editor%20channel_commercial%20channel_stream%20channel_subscriptions%20user_subscriptions%20chat_login%20channel_check_subscription";
        internal static string authToken;

        public static string FromBytes ( byte[] b )
        {
            return Encoding.GetEncoding ( 0 ).GetString ( b );
        }
    }
}