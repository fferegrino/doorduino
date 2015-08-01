using System;
using System.Net;
using System.Diagnostics;
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

        static bool open;
        static InterruptPort DoorSwitch;
        static Stopwatch stopWatch;
        static int seconds;
        static NewDisplay display;
        static bool wasOpen;

        public static void Main()
        {

            var DoorSwitch = new InterruptPort(Pins.GPIO_PIN_D0, true, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeBoth);
            stopWatch = Stopwatch.StartNew(); // new Stopwatch();

            display = new NewDisplay(Pins.GPIO_PIN_D1,
                                                Pins.GPIO_PIN_D2,
                                                Pins.GPIO_PIN_D3,
                Pins.GPIO_PIN_D4, Pins.GPIO_PIN_D5, Pins.GPIO_PIN_D6, Pins.GPIO_PIN_D7, Pins.GPIO_PIN_D8);



            Thread dsp = new Thread(PrintDisplay);
            dsp.Start();


            wasOpen = DoorSwitch.Read();
            if (wasOpen)
            {
                stopWatch.Start();
            }
            DoorSwitch.OnInterrupt += DoorSwitch_OnInterrupt;
            //int  i = 0;
            //while (i < 1000)
            //{
            //    i++;
            //    seconds++;
            //    Thread.Sleep(500);
            //}

            Thread.Sleep(Timeout.Infinite);
        }


        public static void PrintDisplay()
        {
            while (true)
            {
                display.PrintNumber(seconds);
            }
        }

        private static void DoorSwitch_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            open = data2 == 1;
            if (!open)
            {
                Debug.Print("Opened");
                seconds = 0;
                stopWatch.Start();
            }
            else
            {
                Debug.Print("Closed");
                if (!wasOpen)
                {
                    stopWatch.Stop();
                }
                seconds = (int)stopWatch.ElapsedMilliseconds;
                Debug.Print("Tiempo abierto: " + seconds);
                stopWatch.Reset();
                wasOpen = false;
            }
        }

    }
}
