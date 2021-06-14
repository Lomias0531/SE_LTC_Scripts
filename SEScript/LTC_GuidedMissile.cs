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
        IMyGyro gyro;
        Vector3D TargetPos;
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
                TargetPos = new Vector3D(x, y, z);
                Echo(TargetPos.ToString());
                CPU.Direction = Base6Directions.Direction.Forward;
                targetAcquired = true;
                gyro.GyroOverride = true;
                //MainThu.ThrustOverridePercentage = 1f;
            }
            else
            {
                guideCountDown += 1;
                if (guideCountDown >= 100)
                {
                    Vector3D ang = Vector3D.Normalize(TargetPos - CPU.GetPosition()) - CPU.WorldMatrix.Forward;
                    Echo(CPU.WorldMatrix.Forward.ToString());
                    Echo(ang.ToString());
                    if(ang.Z>0)
                    {
                        gyro.Yaw = -30f * ((float)ang.Z);
                    }
                    if(ang.Z<0)
                    {
                        gyro.Yaw = 30f * ((float)ang.Z);
                    }
                    if(ang.Y>0)
                    {
                        gyro.Pitch = -30f * ((float)(ang.Y));
                    }
                    if(ang.Y<0)
                    {
                        gyro.Pitch = 30f * ((float)(ang.Y));
                    }
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
            gyro = GridTerminalSystem.GetBlockWithName("MissileGyro") as IMyGyro;
            if(gyro == null)
            {
                return;
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            CheckReady = true;
        }
    }
}
