using System;
using Microsoft.SPOT;
using System.IO.Ports;

namespace ProtoBoard2017
{
    class Arduino
    {
        private SerialPort uart;

        public Arduino(string uartPort, int baudRate)
        {
            uart = new SerialPort(uartPort, baudRate);
            uart.Open();
        }
        
        public void sendCommand(char a, char b, byte pOut, float vOut, byte cOut)
        {
            byte vA = (byte)vOut;
            byte vB = (byte)((vOut - vA) * 100);

            byte[] values = new byte[6];

            values[0] = (byte)a;
            values[1] = (byte)b;
            values[2] = pOut;
            values[3] = vA;
            values[4] = vB;
            values[5] = cOut;
            
            uart.Write(values, 0, 6);
        }
    }
}
