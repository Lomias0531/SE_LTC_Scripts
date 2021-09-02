using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEScript
{
    class ComponentStarter:API
    {
        void Main()
        {
            List<IMyProgrammableBlock> controls = new List<IMyProgrammableBlock>();
            GridTerminalSystem.GetBlocksOfType(controls);
            foreach (var control in controls)
            {
                control.TryRun("");
            }
        }
    }
}
