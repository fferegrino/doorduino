using System;
using Microsoft.SPOT;

namespace Messier16.Parse.DotNetMf.TestConsole
{
    public class Program
    {
        public static void Main()
        {
            ParseClient pc = new ParseClient("[Here goes AppId]", "[Here goes REST api key]");


            pc.SendPushAlertToChanel("hola");
        }
    }
}
