using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace SEScript
{
    class LTC_GuidedMissile:API
    {
        int guideCountDown = 0;
        IMyRemoteControl CPU;
        bool CheckReady = false;
        bool targetAcquired = false;
        List<IMyThrust> MainThu;
        List<IMyGyro> gyro;
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
                if (cmd[0] != "TargetPos")
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
                foreach (var item in gyro)
                {
                    item.GyroOverride = true;
                }
                foreach (var Thu in MainThu)
                {
                    Thu.ThrustOverridePercentage = 1f;
                }
            }
            else
            {
                guideCountDown += 1;
                if (guideCountDown >= 100)
                {
                    LTC_GyroLookAt(CPU as IMyEntity, TargetPos, gyro);
                }
            }
        }
        void CheckComponent()
        {
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            foreach (var group in groups)
            {
                List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
                group.GetBlocks(terminals);
                if(terminals.Contains(this as IMyTerminalBlock))
                {
                    List<IMyRemoteControl> Remote = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(Remote);
                    if (Remote.Count == 0)
                    {
                        Echo("Remote Error");
                        continue;
                    }
                    CPU = Remote[0];

                    MainThu = new List<IMyThrust>();
                    group.GetBlocksOfType(MainThu, blocks => blocks.Name == "Backwards");
                    foreach (var item in MainThu)
                    {
                        Echo(item.GridThrustDirection.ToString());
                    }
                    if (MainThu.Count == 0)
                    {
                        Echo("Thrust Error");
                        continue;
                    }
                    gyro = new List<IMyGyro>();

                    group.GetBlocksOfType(gyro);
                    if (gyro == null)
                    {
                        Echo("Gyro Error");
                        continue;
                    }

                    Runtime.UpdateFrequency = UpdateFrequency.Update1;
                    CheckReady = true;
                    return;
                }
            }
        }
        void LTC_GyroLookAt(IMyEntity block,Vector3D pos,List<IMyGyro> gyro)
        {
            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), block.WorldMatrix.Forward, block.WorldMatrix.Up);
            Vector3D posAngle = Vector3D.Normalize(Vector3D.TransformNormal(pos - block.GetPosition(), matrix));
            Echo(posAngle.ToString());
            foreach (var Gyr in gyro)
            {
                Gyr.SetValue("Pitch", (float)posAngle.Y * 60f);
                Gyr.SetValue("Yaw", (float)posAngle.X * 60f);
            }
        }

        //List<IMyBlockGroup> groups;
        //IMyMotorStator HorizontalRot;
        //IMyMotorStator VecticalRot;
        //void LTC_RotLookAt(IMyEntity block,Vector3D pos,IMyMotorStator HoriRot,IMyMotorStator VectRot)
        //{
        //    GridTerminalSystem.GetBlockGroups(groups);
        //    foreach (var group in groups)
        //    {
        //        List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
        //        group.GetBlocks(terminals);
        //        if(terminals.Contains(block as IMyTerminalBlock))
        //        {
        //            foreach (var item in terminals)
        //            {
        //                if(item.CustomName == "Horizontal")
        //                {
        //                    HorizontalRot = item as IMyMotorStator;
        //                    continue;
        //                }
        //                if(item.CustomName == "Vertical")
        //                {
        //                    VecticalRot = item as IMyMotorStator;
        //                    continue;
        //                }
        //            }
        //            break;
        //        }
        //    }
        //}
    }
}
