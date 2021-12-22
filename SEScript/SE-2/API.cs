using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
    class API
    {
        public static IMyGridTerminalSystem GridTerminalSystem;
        public static IMyGridProgramRuntimeInfo Runtime;
        public static IMyIntergridCommunicationSystem IGC;
        public static IMyGridProgram GridProgram;
        public static IMyTerminalBlock Me;
        public static void Echo(string msg)
        {

        }
    }
}
