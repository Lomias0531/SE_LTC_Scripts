using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace SEScript
{
    class VTOL : API
    {
        IMyShipController LTC_Controller;
        List<IMyThrust> LTC_Thrusters;
        List<IMyTextSurface> LTC_Displays;
        List<IMyMotorRotor> LTC_Rot_Vertical;
        List<IMyMotorRotor> LTC_Rot_Horizontal;

        bool isSelfCheckComplete = false;
        void Main()
        {
            if(!isSelfCheckComplete)
            {
                SelfCheck();
                return;
            }
            DisplayInfo();
        }
        void SelfCheck()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            LTC_Displays = new List<IMyTextSurface>();
            GridTerminalSystem.GetBlocksOfType(LTC_Displays);
            if(LTC_Displays.Count == 0)
            {
                Echo("No Displays Found.");
                return;
            }
            LTC_Controller = GridTerminalSystem.GetBlockWithName("LTC_Controller") as IMyShipController;
            if (LTC_Controller == null)
            {
                Echo("No Controller Found.");
                return;
            }
            Echo(LTC_Controller.CustomName);
            LTC_Thrusters = new List<IMyThrust>();
            GridTerminalSystem.GetBlocksOfType(LTC_Thrusters);
            if(LTC_Thrusters.Count == 0)
            {
                Echo("No Thruster Found.");
                return;
            }
            LTC_Rot_Vertical = new List<IMyMotorRotor>();
            GridTerminalSystem.GetBlocksOfType(LTC_Rot_Vertical, block => block.Base.CustomName.Contains("LTC_Vertical"));
            if(LTC_Rot_Vertical.Count == 0)
            {
                Echo("No Vertical Rotor Found.");
                return;
            }
            LTC_Rot_Horizontal = new List<IMyMotorRotor>();
            GridTerminalSystem.GetBlocksOfType(LTC_Rot_Horizontal, block => block.Base.CustomName.Contains("LTC_Horizontal"));
            if (LTC_Rot_Horizontal.Count == 0)
            {
                Echo("No Horizontal Rotor Found.");
                return;
            }
        }
        void DisplayInfo()
        {
            string info = "EEE";
            foreach (IMyMotorRotor item in LTC_Rot_Vertical)
            {
                info += item.DisplayName + " - " + item.Orientation.ToString() + "\r\n";
            }
            foreach (IMyThrust thu in LTC_Thrusters)
            {
                info += thu.GridThrustDirection.ToString();
            }
            ShowInfo(info);
        }
        void ShowInfo(string msg)
        {
            foreach (IMyTextSurface lcd in LTC_Displays)
            {
                lcd.WriteText(msg);
            }
        }
    }
}
