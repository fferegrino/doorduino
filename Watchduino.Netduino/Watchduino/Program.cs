using System;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Messier16.Parse.DotNetMf;

namespace Watchduino
{
    public class Program
    {
        static ParseClient pc;
        public static void Main()
        {
            // wait for DHCP-allocated IP address and turn
            while (IPAddress.GetDefaultLocalAddress() == IPAddress.Any);

            pc = new ParseClient("[Here goes your Parse Application ID]", "[Here goes your Parse REST API Key]");

            var doorPort = new InputPort(Pins.GPIO_PIN_D0, true, Port.ResistorMode.PullDown); 

            bool lastStatus = doorPort.Read();
            bool currentStatus;
            while (true)
            {
                currentStatus = doorPort.Read();
                if (currentStatus != lastStatus)
                {
                    pc.SendPushToChannel((currentStatus ? "Someone closed the door" : "Someone opened the door"));
                }
                lastStatus = currentStatus;
            }
            #region Interrupt port buggy implementation

            //var doorPort = new InterruptPort(Pins.GPIO_PIN_D0, true, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeBoth);
            //doorPort.OnInterrupt += DoorSwitch_OnInterrupt;
            //Thread.Sleep(Timeout.Infinite);

            #endregion
        }

        //private static void DoorSwitch_OnInterrupt(uint data1, uint data2, DateTime time)
        //{

        //    bool closed = data2 == 1;
        //    //Debug.Print((closed ? "Cerrado" : "Abierto"));
        //    //pc.SendPushToChannel((closed ? "Cerrado" : "Abierto"));
        //    Debug.Print((closed ? "Cerrado" : "Abierto"));
        //}

    }
}
