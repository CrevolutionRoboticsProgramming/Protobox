using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using CTRE;

namespace ProtoBoard2017
{
    public class Program
    {
        public static void Main()
        {
            TalonSrx talon = new TalonSrx(1);
            OI controller = new OI(new Gamepad(new UsbHostDevice()));
            float motorSpeed;
            while (controller.GetConnectionStatus())
            {
                motorSpeed = controller.GetAxis(OI.Axis.left_x);
                motorSpeed = controller.InvertMotorSpeed(motorSpeed);
                talon.Set(motorSpeed);
                CTRE.Watchdog.Feed();
                Debug.Print("Power: " + talon.Get().ToString());
            }
        }
    }
}
