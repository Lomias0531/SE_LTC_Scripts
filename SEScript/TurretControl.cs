﻿using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRageMath;

namespace SEScript
{
    class TurretControl:API
    {
        IMyMotorStator VerticalRot;
        IMyMotorStator HorizontalRot;
        IMyMotorStator VerticalRev;
        IMyTimerBlock Trigger;
        IMyRemoteControl remote;
        bool CheckReady = false;
        float fireCount = 0;
        float fireCD = 300f;

        Vector3D targetPos;
        TurretStatus curStatus = TurretStatus.Idle;
        enum TurretStatus
        {
            Aiming,
            Idle,
            Manual,
        }
        void Main(string msg)
        {
            if (!CheckReady)
            {
                CheckComponents();
                return;
            }
            fireCount = (fireCount > 0) ? fireCount -= 1 : 0;
            Echo("Running");
            DeseralizeMsg(msg);
            switch(curStatus)
            {
                case TurretStatus.Aiming:
                    {
                        Echo("Targeting " + targetPos.ToString());
                        AimByRotor();
                        break;
                    }
                case TurretStatus.Idle:
                    {
                        RestorePos();
                        break;
                    }
                case TurretStatus.Manual:
                    {
                        MoveByRotor();
                        break;
                    }
            }
        }
        void MoveByRotor()
        {
            HorizontalRot.TargetVelocityRPM = remote.RotationIndicator.Y;
            VerticalRot.TargetVelocityRPM = remote.RotationIndicator.X;
            VerticalRev.TargetVelocityRPM = remote.RotationIndicator.X * -1;
        }
        void AimByRotor()
        {
            LookAtDirection(targetPos, true);
        }
        void RestorePos()
        {
            Vector3D restorePos = GridTerminalSystem.GetBlockWithName("MainControl").GetPosition() + GridTerminalSystem.GetBlockWithName("MainControl").WorldMatrix.Forward * 1000f;
            LookAtDirection(restorePos, false);
            return;
        }
        void LookAtDirection(Vector3D pos,bool isFiring)
        {
            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), remote.WorldMatrix.Forward, remote.WorldMatrix.Up);
            Vector3D posAngle = Vector3D.Normalize(Vector3D.TransformNormal(pos - remote.GetPosition(), matrix));
            double distance = Vector3D.Distance(remote.GetPosition(), pos);
            fireCD = distance > 1500 ? ((float)distance / 1000f) * 300f : 300f;
            if (posAngle.X > -0.01f && posAngle.X < 0.01f && posAngle.Y > -0.01f && posAngle.Y < 0.01f && isFiring)
            {
                Fire();
            }
            VerticalRot.TargetVelocityRPM = (float)posAngle.Y * -5;
            VerticalRev.TargetVelocityRPM = (float)posAngle.Y * 5;
            HorizontalRot.TargetVelocityRPM = (float)posAngle.X * 5;
        }
        void CheckComponents()
        {
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            foreach (var group in groups)
            {
                List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
                group.GetBlocks(terminals);
                if (terminals.Contains(Me as IMyTerminalBlock))
                {
                    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(remotes,blocks => blocks.CustomName == "Remote");
                    if (remotes.Count == 0)
                    {
                        continue;
                    }
                    remote = remotes[0];
                    remote.Direction = Base6Directions.Direction.Forward;
                    List<IMyMotorStator> vet = new List<IMyMotorStator>();
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "VerticalRot");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    VerticalRot = vet[0];
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "VerticalRotRev");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    VerticalRev = vet[0];
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "HorizontalRot");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    HorizontalRot = vet[0];
                    List<IMyTimerBlock> tim = new List<IMyTimerBlock>();
                    group.GetBlocksOfType(tim, blocks => blocks.CustomName == "LTC_Trigger");
                    if(tim.Count == 0)
                    {
                        continue;
                    }
                    Trigger = tim[0];
                }
            }

            if (remote == null)
            {
                Echo("Remote");
                return;
            }
            if (VerticalRot == null)
            {
                Echo("Vertical");
                return;
            }
            if (VerticalRot == null)
            {
                Echo("VerticalRev");
                return;
            }
            if (HorizontalRot == null)
            {
                Echo("Horizontal");
                return;
            }
            if(Trigger == null)
            {
                Echo("Timer");
                return;
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            CheckReady = true;
            return;
        }
        void Fire()
        {
            if (fireCount == 0)
            {
                Trigger.Trigger();
                fireCount = fireCD;
            }
        }
        void DeseralizeMsg(string msg)
        {
            msg = Me.CustomData;
            string[] messages = msg.Split('|');
            if (messages[0] != "Turret") return;
            switch(messages[1])
            {
                default:
                    {
                        break;
                    }
                case "TargetPos":
                    {
                        string[] pos = messages[2].Split('_');
                        Echo(pos[0]);
                        float x = float.Parse(pos[0]);
                        float y = float.Parse(pos[1]);
                        float z = float.Parse(pos[2]);
                        targetPos = new Vector3D(x, y, z);
                        curStatus = TurretStatus.Aiming;
                        break;
                    }
                case "Fire":
                    {
                        Fire();
                        break;
                    }
                case "Idle":
                    {
                        curStatus = TurretStatus.Idle;
                        break;
                    }
                case "Manual":
                    {
                        curStatus = TurretStatus.Manual;
                        remote.IsMainCockpit = true;
                        break;
                    }
            }
            Me.CustomData = "";
        }
    }
}
