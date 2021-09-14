using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI;
using VRageMath;

namespace SEScript
{
    class LTC_Serphant:API
    {
        List<IMyCameraBlock> Scanners;
        IMyRemoteControl Remote;
        List<IMyWarhead> WarHeads;
        List<IMyGyro> Gyros;
        List<IMyShipMergeBlock> Merge;
        List<IMyThrust> Thrusts;
        bool CheckReady = false;

        float MissileMass = 0;
        bool launched = false;
        Vector3D TargetPos;
        Vector3D TargetVel;
        float TimeStamp = 0;
        void Main(string arg)
        {
            if(!CheckReady)
            {
                CheckComponents();
                return;
            }
            ExecuteCmd(arg);
            if(launched)
            {
                ScanTarget();
                TrackTarget();
            }
            TimeStamp += 1;
            if(TimeStamp == 100)
            {
                foreach (var item in Merge)
                {
                    item.Enabled = true;
                }
            }
        }
        void CheckComponents()
        {
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            foreach (var group in groups)
            {
                List<IMyProgrammableBlock> terminals = new List<IMyProgrammableBlock>();
                group.GetBlocksOfType(terminals);
                if(terminals.Contains(Me as IMyProgrammableBlock))
                {
                    Merge = new List<IMyShipMergeBlock>();
                    group.GetBlocksOfType(Merge);
                    if (Merge.Count == 0)
                    {
                        Echo("Merge");
                        return;
                    }
                    Scanners = new List<IMyCameraBlock>();
                    group.GetBlocksOfType(Scanners);
                    if(Scanners.Count == 0)
                    {
                        Echo("Cams");
                        return;
                    }
                    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(remotes);
                    if(remotes.Count == 0)
                    {
                        Echo("Remote");
                        return;
                    }
                    Remote = remotes[0];
                    WarHeads = new List<IMyWarhead>();
                    group.GetBlocksOfType(WarHeads);
                    if(WarHeads.Count == 0)
                    {
                        Echo("Warheads");
                        return;
                    }
                    Gyros = new List<IMyGyro>();
                    group.GetBlocksOfType(Gyros);
                    if(Gyros.Count == 0)
                    {
                        Echo("Gyros");
                        return;
                    }
                    foreach (var item in Gyros)
                    {
                        item.GyroOverride = true;
                    }
                    Thrusts = new List<IMyThrust>();
                    group.GetBlocksOfType(Thrusts);
                    if(Thrusts.Count == 0)
                    {
                        Echo("Thrust");
                        return;
                    }
                }
            }

            CheckReady = true;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            return;
        }
        void ScanTarget()
        {
            //持续追踪目标
            bool targetAcquired = false;
            foreach (var cam in Scanners)
            {
                MyDetectedEntityInfo target = cam.Raycast(TargetPos);
                if (!target.IsEmpty())
                {
                    if(target.Relationship == VRage.Game.MyRelationsBetweenPlayerAndBlock.Enemies)
                    {
                        Echo("Target locking");
                        TargetPos = target.BoundingBox.Center;
                        TargetVel = target.Velocity;
                        targetAcquired = true;
                    }
                }
            }
            //若目标丢失则每帧随机取30个采样点进行探测
            if(!targetAcquired)
            {
                foreach (var cam in Scanners)
                {
                    for(int i = 0;i<30;i++)
                    {
                        Random rnd = new Random();
                        float offsetX = rnd.Next(-90000, 90000) / 1000;
                        float offsetY = rnd.Next(-90000, 90000) / 1000;
                        MyDetectedEntityInfo target = cam.Raycast(4000, offsetX, offsetY);
                        if (!target.IsEmpty())
                        {
                            if (target.Relationship == VRage.Game.MyRelationsBetweenPlayerAndBlock.Enemies)
                            {
                                Echo("Target acquired");
                                TargetPos = target.BoundingBox.Center;
                                TargetVel = target.Velocity;
                                targetAcquired = true;
                            }
                        }
                    }
                }
            }
        }
        void TrackTarget()
        {
            if(Vector3D.Distance(TargetPos,Me.GetPosition()) < 5f)
            {
                foreach (var item in WarHeads)
                {
                    item.Detonate();
                }
            }
            float estimatedTime = (float)(Vector3D.Distance(TargetPos, Remote.GetPosition()) / Remote.GetShipSpeed());
            Vector3D VTD = Vector3D.Reject(TargetVel - Remote.GetShipSpeed(),TargetPos - Remote.GetPosition()) - Remote.GetNaturalGravity() * estimatedTime * 0.5f;
            TargetPos += VTD * estimatedTime;
            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), Remote.WorldMatrix.Forward, Remote.WorldMatrix.Up);
            Vector3D posAngle = Vector3D.Normalize(Vector3D.TransformNormal(TargetPos - Remote.GetPosition(), matrix));
            foreach (var Gyr in Gyros)
            {
                Gyr.SetValue("Pitch", (float)posAngle.Y * -60f);
                Gyr.SetValue("Yaw", (float)posAngle.X * -60f);
            }
        }
        void Launch()
        {
            foreach (var item in Merge)
            {
                item.Enabled = false;
            } 
            launched = true;
            foreach (var item in WarHeads)
            {
                item.IsArmed = true;
            }
            foreach (var item in Thrusts)
            {
                item.ThrustOverridePercentage = 1f;
            }
            TimeStamp = 0;
        }
        void ExecuteCmd(string msg)
        {
            string cmd = string.IsNullOrEmpty(msg) ? Me.CustomData : msg;
            string[] cmds = cmd.Split('|');
            if (cmds[0] != "Missile") return;
            if (launched) return;
            switch(cmds[1])
            {
                case "Launch":
                    {
                        string[] pos = cmds[2].Split('_');
                        float x = float.Parse(pos[0]);
                        float y = float.Parse(pos[1]);
                        float z = float.Parse(pos[2]);
                        TargetPos = new Vector3D(x, y, z);
                        Launch();
                        break;
                    }
            }
        }
    }
}
