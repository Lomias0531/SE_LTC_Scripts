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

        float MissileMass = 1869;
        bool launched = false;
        Vector3D TargetPos;
        Vector3D TargetVel;
        float TimeStamp = 0;
        void Main(string arg)
        {
            TimeStamp += 1;
            if (!CheckReady)
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
        }
        void CheckComponents()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
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
                    if(TimeStamp < 5)
                    {
                        foreach (var item in Merge)
                        {
                            item.Enabled = false;
                        }
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
            return;
        }
        void ScanTarget()
        {
            //持续追踪目标
            bool targetAcquired = false;
            foreach (var cam in Scanners)
            {
                if(cam.TimeUntilScan(Vector3D.Distance(TargetPos,cam.GetPosition()) + 100)>0)
                {
                    continue;
                }
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
                break;
            }
            //若目标丢失则随机取1个采样点进行探测
            if(!targetAcquired)
            {
                foreach (var cam in Scanners)
                {
                    if(cam.TimeUntilScan(4000)>0)
                    {
                        continue;
                    }
                    Random rnd = new Random();
                    float offsetX = rnd.Next(-45000, 45000) / 1000;
                    float offsetY = rnd.Next(-45000, 45000) / 1000;
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
        void TrackTarget()
        {
            if(Vector3D.Distance(TargetPos,Me.GetPosition()) < 5f)
            {
                foreach (var item in WarHeads)
                {
                    item.Detonate();
                }
            }
            Echo(TargetPos.ToString("F2"));

            if(TimeStamp < 200)
            {
                return;
            }

            //float estimatedTime = (float)(Vector3D.Distance(TargetPos, Remote.GetPosition()) / Remote.GetShipSpeed());
            //Vector3D VTD = Vector3D.Reject(TargetVel - Remote.GetShipSpeed(), Vector3D.Normalize(TargetPos - Remote.GetPosition())) - Remote.GetNaturalGravity() * estimatedTime * 0.5f;
            //TargetPos += VTD * estimatedTime;
            //MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), Remote.WorldMatrix.Forward, Remote.WorldMatrix.Up);
            //Vector3D posAngle = Vector3D.Normalize(Vector3D.TransformNormal(TargetPos - Remote.GetPosition(), matrix));
            //foreach (var Gyr in Gyros)
            //{
            //    Gyr.SetValue("Pitch", (float)posAngle.Y * -60f);
            //    Gyr.SetValue("Yaw", (float)posAngle.X * -60f);
            //}

            //计算导弹可提供的最大加速度
            float maxThrust = 0;
            foreach (var thr in Thrusts)
            {
                maxThrust += thr.MaxEffectiveThrust;
            }
            double missileAcc = maxThrust / MissileMass;
            //排除不需要的速度
            Vector3D tarN = Vector3D.Normalize(TargetPos - Remote.GetPosition()); //当前导弹位置指向目标位置的单位指向, target normalized
            Vector3D rv = Vector3D.Reject(TargetVel - Remote.GetShipSpeed(), tarN); //相对速度向量排除指向, relative velocity
            //Vector3D ra = Vector3D.Reject(TargetVel, tarN); //相对加速度，可不考虑
            Vector3D ra = Vector3D.Zero; //relative accleration
            //计算不需要的速度
            Vector3D rvN = Vector3D.Normalize(rv); //排除后的相对速度单位指向, relative velocity normalized
            double newlen = Math.Atan2(rv.Length(), 5); //相对速度的大小，通过Atan2限定
            Vector3D newrv = rvN * newlen;
            double GuideRate = 0.3;
            Vector3D rdo = newrv * GuideRate * 60 + ra * 0.5; //侧向加速度
            //计算抵消重力需要的加速度
            Vector3D rd = rdo - Remote.GetNaturalGravity(); //需要抵消掉的加速度
            //double rdl = rd.Length();
            //剩余加速度
            Vector3D rd2 = Vector3D.Reject(rd, tarN); //需要的侧向加速度
            double rd2l = rd2.Length();
            if (missileAcc < rd2l) missileAcc = rd2l;
            double pdl = Math.Sqrt(missileAcc * missileAcc - rd2l * rd2l);
            //剩余加速度方向
            //剩余加速度
            Vector3D pd = tarN * pdl;
            //总加速度
            Vector3D sd = pd + rd2;
            //总加速度方向
            Vector3D nam = Vector3D.Normalize(sd);
            var missileLookAt = MatrixD.CreateLookAt(new Vector3D(), Remote.WorldMatrix.Up, Remote.WorldMatrix.Backward);
            var amToMe = Vector3D.TransformNormal(nam, missileLookAt);
            Echo(nam.ToString("F2"));
            Echo(amToMe.ToString("F2"));

            ////排除不需要的速度
            //Vector3D tarN = Vector3D.Normalize(TargetPos - Remote.GetPosition()); //当前导弹位置指向目标位置的单位指向, target normalized
            //Vector3D rv = Vector3D.Reject(TargetVel - Remote.GetShipSpeed(), tarN); //相对速度向量排除指向, relative velocity
            ////计算不需要的速度
            //Vector3D rvN = Vector3D.Normalize(rv); //排除后的相对速度单位指向, relative velocity normalized
            //Vector3D pd = Vector3D.Normalize(tarN);
            //Vector3D sd = pd + rvN - Remote.GetNaturalGravity();
            //Vector3D nam = Vector3D.Normalize(sd);
            //var missileLookAt = MatrixD.CreateLookAt(new Vector3D(), Remote.WorldMatrix.Up, Remote.WorldMatrix.Backward);
            //var amToMe = Vector3D.TransformNormal(nam, missileLookAt);
            //Echo(nam.ToString("F2"));
            //Echo(amToMe.ToString("F2"));

            foreach (var Gyr in Gyros)
            {
                Gyr.SetValue("Pitch", (float)amToMe.Y * -60f);
                Gyr.SetValue("Yaw", (float)amToMe.X * -60f);
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
                item.ThrustOverridePercentage = 0.1f;
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
            if(string.IsNullOrEmpty(msg))
                Me.CustomData = "";
        }
    }
}
