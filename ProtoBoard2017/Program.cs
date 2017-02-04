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

            System.IO.Ports.SerialPort uart = new System.IO.Ports.SerialPort(CTRE.HERO.IO.Port1.UART, 9600);
            byte[] data = new byte[7];
            byte tData = 0;

            uart.Open();
            
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
                tData = (byte)(motorSpeed * 100);
                if(motorSpeed > 1)
                {
                    motorSpeed = 1;
                    tData = (byte)(motorSpeed * 100);
                } else if (motorSpeed < -1)
                {
                    motorSpeed = -1;
                }

                if (motorSpeed < 0)
                {
                    tData = (byte)(motorSpeed * -1);
                }
                uart.WriteByte(tData);

                talon.Set(motorSpeed);
                talon2.Set(motorSpeed);
                CTRE.Watchdog.Feed();
                Debug.Print("Motor Speed = " + motorSpeed);

            }
        }
    }
}
