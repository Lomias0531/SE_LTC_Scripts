using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

namespace SEScript
{
    class TargetIndicator:API
    {
        bool Checked = false;
        IMyCamera thisCam;
        IMyMotorStator VertRot;
        IMyMotorStator HoriRot;
        void Main()
        {
            if(!Checked)
            {
                CheckComponents();
                return;
            }
        }
        void CheckComponents()
        {
            List<IMyTerminalBlock> terminal = new List<IMyTerminalBlock>();
        }
    }
}
