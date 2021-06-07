using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace SEScript
{
    class Program:ThisAPI
    {
        List<IMyTextSurface> LCDPanels;
        static void Main(string[] args)
        {
            
        }
        void GetBlocks()
        {
            GridTerminalSystem.GetBlocksOfType(LCDPanels);
        }
    }
}
