using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using CTRE;

/*/=============================================
 * _____ Code for the Crevolution ProtoBox _____
 * =============================================
 * 
 * ~ Jason Deal and Martin Smooger 
 * 
 *Proto Box Uses :
 * Testing motors on ProtoTypes
 * Code logic testing 
 * Teach new Programmers close looping and Vision Processing 
 * Can be configured for a quick robot
 * 
 *Proto Box Features :
 * Crevo Cards:
 *  Main "Mother Board" Card (Holds Hero, Arduino, and Raspberry PI) 
 *  Talon SRX Card (3 Talons per card)
 *  Sensor Card (Digital and Analog) 
 *  Pneumatics Card
 *  
 * Close Looping on manipluators 
 *  
 *Proto Box Equitment :
 *  Hero Development Board
 *  Power Distribution Panel 
 *  Pneumattics Control Module 
 *  Voltage Regulator Module 
 *  Talon SRX 
 *  SRX Magnetic Encoder
 *  Talon SRX Analog Breakout Board 
 *  Gadgeteer Display Module
 *  Gadgeteer Driver Module 
 *  Gadgeteer Breakout Module 
 *    
 * Non Cross the road electronics Componets: 
 *  Arduino Uno 
 *  Raspberry PI 2 
 *  LCD Touch Screen 
 *  Cooling fan 
/*/

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

            int motorSpeed = 0;
            int motorStoredSpeed = 700;

            float rMotorSpeed = 0f;
            float rMotorStoreSpeed = .7f;

            bool isPressed = false;
            byte tData;
            
            int tMotorSpeed;
            int counter;

            while(!arduino.Status())
            {
                Debug.Print("Waiting for arduino");
            }
            
            while (true)
            {
                if(controller.GetButton(OI.Button.a) && !isPressed)
                {
                    motorSpeed += 50;
                    rMotorSpeed += .05f;
                } else if (controller.GetButton(OI.Button.b) && !isPressed)
                {
                    motorSpeed -= 50;
                    rMotorSpeed -= .05f;
                }

                if(controller.GetButton(OI.Button.x) && !isPressed)
                {
                    motorSpeed += 10;
                    rMotorSpeed += .01f;
                } else if (controller.GetButton(OI.Button.y) && !isPressed)
                {
                    motorSpeed -= 10;
                    rMotorSpeed -= .01f;
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
                    rMotorSpeed = 0;
                } else if (controller.GetAxis(OI.Axis.left_y) == 1)
                {
                    motorSpeed = motorStoredSpeed;
                    rMotorSpeed = rMotorStoreSpeed;
                }

                tMotorSpeed = motorSpeed / 10;

                tData = (byte)(tMotorSpeed - (tMotorSpeed % 10)); //Outputs 100 place of motor speed
                tData += (byte)(tMotorSpeed - tData);

                if (controller.GetAxis(OI.Axis.right_y) == -1 && (counter % 1000) == 0)
                {
                    arduino.sendCommand('U', 'P', tData, talon.GetOutputVoltage(), (byte)talon.GetOutputCurrent());
                } else if (controller.GetAxis(OI.Axis.right_y) == 1 && (counter % 1000) == 0)
                {
                    arduino.sendCommand('D', 'O', tData, talon.GetOutputVoltage(), (byte)talon.GetOutputCurrent());
                } else if ((counter % 1000) == 0)
                {
                    arduino.sendCommand('0', '0', tData, talon.GetOutputVoltage(), (byte)talon.GetOutputCurrent());
                }

                talon.Set(rMotorSpeed);
                talon2.Set(rMotorSpeed);
                CTRE.Watchdog.Feed();
                Debug.Print("Motor Speed = " + motorSpeed);
                Debug.Print("tData = " + tData);
                counter++;
                
            }
        }
    }
}
