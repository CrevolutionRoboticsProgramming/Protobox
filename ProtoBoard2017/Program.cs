﻿using System;
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
            Arduino arduino = new Arduino(CTRE.HERO.IO.Port1.UART, 9600);
            TalonSrx talon = new TalonSrx(1);
            TalonSrx talon2 = new TalonSrx(2);
            OI controller = new OI(new Gamepad(new UsbHostDevice()));
            int motorSpeed = 700;
            int motorStoredSpeed = motorSpeed;
            bool isPressed = false;
            byte tData;
            
            int tMotorSpeed;
            
            while (true)
            {
                if(controller.GetButton(OI.Button.a) && !isPressed)
                {
                    motorSpeed += 50;
                } else if (controller.GetButton(OI.Button.b) && !isPressed)
                {
                    motorSpeed -= 50;
                }

                if(controller.GetButton(OI.Button.x) && !isPressed)
                {
                    motorSpeed += 10;
                } else if (controller.GetButton(OI.Button.y) && !isPressed)
                {
                    motorSpeed -= 10;
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

                tMotorSpeed = motorSpeed / 10;

                tData = (byte)(tMotorSpeed - (tMotorSpeed % 10)); //Outputs 100 place of motor speed
                tData += (byte)(tMotorSpeed - tData);

                if (controller.GetAxis(OI.Axis.right_y) == -1)
                {
                    arduino.sendCommand('U', 'P', tData, talon.GetOutputVoltage(), (byte)talon.GetOutputCurrent());
                } else if (controller.GetAxis(OI.Axis.right_y) == 1)
                {
                    arduino.sendCommand('D', 'O', tData, talon.GetOutputVoltage(), (byte)talon.GetOutputCurrent());
                } else
                {
                    arduino.sendCommand('0', '0', tData, talon.GetOutputVoltage(), (byte)talon.GetOutputCurrent());
                }

                talon.Set(motorSpeed / 100);
                talon2.Set(motorSpeed / 100);
                CTRE.Watchdog.Feed();
                Debug.Print("Motor Speed = " + motorSpeed);
                Debug.Print("tData = " + tData);

                
            }
        }
    }
}
