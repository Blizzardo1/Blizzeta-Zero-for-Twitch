using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blizzeta.Twitch
{
    public class ConsoleInterop
    {
        public static void Error ( string message, params object[] args )
        {
            Console.WriteLine ( "[Error] {0}", string.Format ( message, args ) );
            Debug.WriteLine ( "[Error] {0}", string.Format ( message, args ) );
        }

        public static void Info ( string message, params object[] args )
        {
            Console.WriteLine ( "[Info] {0}", string.Format ( message, args ) );
            Debug.WriteLine ( "[Info] {0}", string.Format ( message, args ) );
        }

        public static void Warning ( string message, params object[] args )
        {
            Console.WriteLine ( "[Warning] {0}", string.Format ( message, args ) );
            Debug.WriteLine ( "[Warning] {0}", string.Format ( message, args ) );
        }
    }
}