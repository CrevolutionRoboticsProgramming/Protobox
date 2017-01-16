using System;
using Microsoft.SPOT;

namespace ProtoBoard2017
{
    class OI
    {
        private CTRE.Gamepad controller;
        public enum Axis {left_y = 1, left_x = 0, right_y = 5, right_x = 2 };
        public enum POV {left, right, up, down};
        public enum Button {a = 2, b = 3, x = 1, y = 4};    

        public OI(CTRE.Gamepad controller)
        {
            this.controller = controller;
        }

        public bool GetConnectionStatus()
        {
            if(controller.GetConnectionStatus() == CTRE.UsbDeviceConnection.Connected)
            {
                return true;
            } else {
                return false;
            }
        }

        public float GetAxis(Axis axis)
        {
            return controller.GetAxis((uint)axis);
        }
        
        //TODO: Make work
        public bool GetPOV(POV pov)
        {
            
            switch(pov)
            {
                case POV.left:
                    if(controller.GetAxis(6) > 0)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                    break;
                /*
                case POV.down:
                    if (controller.GetAxis(6) == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                */
                case POV.right:
                    if (controller.GetAxis(6) < 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                /*
                case POV.up:
                    if (controller.GetAxis(6) == -1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                */
                default:
                    return false;
                    break;
            }
        }

        public bool GetButton(Button button)
        {
            if(controller.GetButton((uint)button))
            {
                return true;
            } else {
                return false;
            }
        }

        public float InvertMotorSpeed(float motorSpeed)
        {
            return motorSpeed * -1;
        }
    }
}
