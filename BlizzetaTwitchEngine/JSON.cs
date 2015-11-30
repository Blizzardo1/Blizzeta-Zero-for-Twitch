using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blizzeta.Twitch
{
    public class TwitchPassword
    {
        public List<string> scope { get; set; }
        public string access_token { get; set; }
    }

    public class TwitchError
    {
        public string message { get; set; }
        public int status { get; set; }
        public string error { get; set; }
    }
}
