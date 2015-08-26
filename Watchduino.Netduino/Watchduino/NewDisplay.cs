using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace Watchduino
{
    public class NewDisplay
    {
        OutputPort[] abcd;
        OutputPort[] onoff;

        static bool[][] Numeros = new bool[][]{
            new bool[]{false, false, false , false},    // 0
            new bool[]{false, false, false , true},     // 1
            new bool[]{false, false, true , false},     // 2
            new bool[]{false, false, true , true},      // 3
            new bool[]{false, true, false , false},     // 4
            new bool[]{false, true, false , true},      // 5
            new bool[]{false, true, true , false},      // 6
            new bool[]{false, true, true, true},        // 7
            new bool[]{true, false, false , false},      // 8
            new bool[]{true, false, false , true},      // 9
            };

        public NewDisplay(Cpu.Pin a, Cpu.Pin b, Cpu.Pin c, Cpu.Pin d,
            Cpu.Pin d1, Cpu.Pin d2, Cpu.Pin d3, Cpu.Pin d4)
        {
            abcd = new OutputPort[4];
            abcd[0] = new OutputPort(d, false);
            abcd[1] = new OutputPort(c, false);
            abcd[2] = new OutputPort(b, false);
            abcd[3] = new OutputPort(a, false);

            onoff = new OutputPort[4];
            onoff[0] = new OutputPort(d1, false);
            onoff[1] = new OutputPort(d2, false);
            onoff[2] = new OutputPort(d3, false);
            onoff[3] = new OutputPort(d4, false);
        }

        public void PrintDigit(int digit)
        {
            for (int i = 0; i < Numeros[digit % 10].Length; i++)
            {
                abcd[i].Write(Numeros[digit % 10][i]);
            }
        }

        public void TurnOnDisplay(int p)
        {
            for (int i = 0; i < onoff.Length; i++)
            {
                onoff[i].Write((i + 1) != p);
            }
        }

        public void PrintNumber(int n)
        {
            const int wait = 5;

            TurnOnDisplay(1);
            PrintDigit(n / 1000);
            Thread.Sleep(wait);

            TurnOnDisplay(2);
            PrintDigit(n / 100);
            Thread.Sleep(wait);

            TurnOnDisplay(3);
            PrintDigit(n / 10);
            Thread.Sleep(wait);

            TurnOnDisplay(4);
            PrintDigit(n);
            Thread.Sleep(wait);
        }
    }
}
