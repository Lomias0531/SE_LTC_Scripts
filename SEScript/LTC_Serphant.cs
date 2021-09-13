using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
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
        IMyShipMergeBlock Merge;
        List<IMyThrust> Thrusts;
        bool CheckReady = false;

        float MissileMass = 0;
        bool launched = false;
        Vector3D TargetPos;
        Vector3D TargetVel;
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
                    Thrusts = new List<IMyThrust>();
                    group.GetBlocksOfType(Thrusts);
                    if(Thrusts.Count == 0)
                    {
                        Echo("Thrust");
                        return;
                    }
                    List<IMyShipMergeBlock> merges = new List<IMyShipMergeBlock>();
                    group.GetBlocksOfType(merges);
                    if(merges.Count == 0)
                    {
                        Echo("Merge");
                        return;
                    }
                    Merge = merges[0];
                }
            }

            CheckReady = true;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            return;
        }
        void ScanTarget()
        {
            foreach (var cam in Scanners)
            {
                MyDetectedEntityInfo target = cam.Raycast(TargetPos);
                if (!target.IsEmpty())
                {
                    TargetPos = target.BoundingBox.Center;
                    TargetVel = target.Velocity;
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
        }
        void Launch()
        {
            Merge.Enabled = false;
            launched = true;
            foreach (var item in WarHeads)
            {
                item.IsArmed = true;
            }
        }
        void ExecuteCmd(string msg)
        {
            string cmd = string.IsNullOrEmpty(msg) ? Me.CustomData : msg;
            string[] cmds = cmd.Split('|');
            if (cmds[0] != "Missile") return;
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
