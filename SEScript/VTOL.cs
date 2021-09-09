using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace SEScript
{
    class VTOL : API
    {
        List<IMyMotorStator> VTOLRot;
        List<IMyRemoteControl> VTOLDir;
        List<IMyThrust> VTOLThrust;
        IMyShipController MainControl;

        bool Checked = false;
        void Main()
        {
            if(!Checked)
            {
                CheckComponents();
                return;
            }
            AutoBreak();
            ManualControl();
            CalculateRot();
        }
        void CheckComponents()
        {
            VTOLDir = new List<IMyRemoteControl>();
            VTOLRot = new List<IMyMotorStator>();
            VTOLThrust = new List<IMyThrust>();
            GridTerminalSystem.GetBlocksOfType(VTOLThrust, blocks => blocks.CustomName.Contains("VTOL"));
            if(VTOLThrust.Count == 0)
            {
                Echo("Thrust");
                return;
            }
            GridTerminalSystem.GetBlocksOfType(VTOLDir, blocks => blocks.CustomName.Contains("VTOL"));
            if(VTOLDir.Count == 0)
            {
                Echo("Remote");
                return;
            }
            GridTerminalSystem.GetBlocksOfType(VTOLRot, blocks => blocks.CustomName.Contains("VTOL"));
            if(VTOLRot.Count == 0)
            {
                Echo("Rotor");
                return;
            }
            MainControl = GridTerminalSystem.GetBlockWithName("LTC_MainControl") as IMyShipController;
            if(MainControl == null)
            {
                Echo("Controller");
                return;
            }

            Checked = true;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            return;
        }
        void AutoBreak()
        {

        }
        void ManualControl()
        {

        }
        void CalculateRot()
        {

        }
    }
}
