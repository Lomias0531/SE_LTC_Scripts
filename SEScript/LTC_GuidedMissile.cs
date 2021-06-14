using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace SEScript
{
    class LTC_GuidedMissile:API
    {
        int guideCountDown = 0;
        IMyRemoteControl CPU;
        bool CheckReady = false;
        bool targetAcquired = false;
        IMyThrust MainThu;
        void Main(string arg)
        {
            if(!CheckReady)
            {
                CheckComponent();
                return;
            }
            if (!targetAcquired)
            {
                string[] cmd = arg.Split('|');
                if (cmd[0] != "MissileTarget")
                {
                    return;
                }
                string[] targetPos = cmd[1].Split('_');
                Echo(cmd[1]);
                float x = float.Parse(targetPos[0]);
                float y = float.Parse(targetPos[1]);
                float z = float.Parse(targetPos[2]);
                Vector3D Target = new Vector3D(x, y, z);
                CPU.Direction = Base6Directions.Direction.Forward;
                CPU.AddWaypoint(Target, "Target");
                targetAcquired = true;
                MainThu.ThrustOverridePercentage = 1f;
            }
            else
            {
                guideCountDown += 1;
                if (guideCountDown >= 100 && guideCountDown < 250)
                {
                    CPU.SetAutoPilotEnabled(true);
                    CPU.FlightMode = FlightMode.OneWay;
                    MainThu.ThrustOverridePercentage = 0f;
                }
                if(guideCountDown >= 250)
                {
                    CPU.SetAutoPilotEnabled(false);
                    MainThu.ThrustOverridePercentage = 1f;
                }
            }
        }
        void CheckComponent()
        {
            List<IMyRemoteControl> Remote = new List<IMyRemoteControl>();
            GridTerminalSystem.GetBlocksOfType(Remote);
            if (Remote.Count == 0)
            {
                return;
            }
            CPU = Remote[0];

            MainThu = GridTerminalSystem.GetBlockWithName("Forward") as IMyThrust;
            if(MainThu == null)
            {
                Echo("EEE");
                return;
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            CheckReady = true;
        }
    }
}
