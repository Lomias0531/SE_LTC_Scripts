using System;
using System.Collections.Generic;
using System.Text;

namespace Sandbox.ModAPI.Ingame
{
    class API
    {
        public static IMyGridTerminalSystem GridTerminalSystem;
        public static IMyGridProgramRuntimeInfo Runtime;
        public static IMyIntergridCommunicationSystem IntergridCommunicationSystem;
        public static IMyGridProgram GridProgram;
        public static void Echo(string msg)
        {

        }
    }
}
