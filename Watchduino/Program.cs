using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace Watchduino
{
    public class Program
    {

        static bool on;
        public static void Main()
        {
            var led = new OutputPort(Pins.ONBOARD_LED, false);
            var inputPort = new InterruptPort(Pins.GPIO_PIN_D7, true, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeBoth);

            inputPort.OnInterrupt +=inputPort_OnInterrupt;
            

            while(true)
            {
                led.Write(on);
            }


        }

        private static void inputPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            //Debug.Print("data 1" + data1 + " data2 " + data2);
            on = data2 == 1;
        }

    }
}
