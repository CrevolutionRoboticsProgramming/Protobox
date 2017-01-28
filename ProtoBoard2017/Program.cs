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
            TalonSrx talon2 = new TalonSrx(2);
            OI controller = new OI(new Gamepad(new UsbHostDevice()));
            float motorSpeed = .70f;
            float motorStoredSpeed = motorSpeed;
            bool isPressed = false;
            while (true)
            {
                if(controller.GetButton(OI.Button.a) && !isPressed)
                {
                    motorSpeed += 0.05f;
                } else if (controller.GetButton(OI.Button.b) && !isPressed)
                {
                    motorSpeed -= 0.05f;
                }

                if(controller.GetButton(OI.Button.x) && !isPressed)
                {
                    motorSpeed += 0.01f;
                } else if (controller.GetButton(OI.Button.y) && !isPressed)
                {
                    motorSpeed -= 0.01f;
                }

                if (controller.GetButton(OI.Button.a) || controller.GetButton(OI.Button.b) || controller.GetButton(OI.Button.x) || controller.GetButton(OI.Button.y))
                {
                    isPressed = true;
                }
                else
                {
                    isPressed = false;
                }

                if (controller.GetAxis(OI.Axis.left_y) == -1)
                {
                    motorSpeed = 0;
                } else if (controller.GetAxis(OI.Axis.left_y) == 1)
                {
                    motorSpeed = motorStoredSpeed;
                }

                talon.Set(motorSpeed);
                talon2.Set(motorSpeed);
                CTRE.Watchdog.Feed();
                Debug.Print("Motor Speed = " + motorSpeed);

            }
        }
    }
}
