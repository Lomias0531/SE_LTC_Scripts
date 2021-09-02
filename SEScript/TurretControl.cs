using System;
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
            DeseralizeMsg(msg);
            switch(curStatus)
            {
                case TurretStatus.Aiming:
                    {
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
            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), remote.WorldMatrix.Forward, remote.WorldMatrix.Up);
            Vector3D posAngle = Vector3D.Normalize(Vector3D.TransformNormal(targetPos - remote.GetPosition(), matrix));
            VerticalRot.TargetVelocityRPM = (float)posAngle.Y;
            VerticalRev.TargetVelocityRPM = (float)posAngle.Y * -1;
            HorizontalRot.TargetVelocityRPM = (float)posAngle.X;
        }
        void RestorePos()
        {
            
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
        void DeseralizeMsg(string msg)
        {
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
                        string[] pos = messages[1].Split('_');
                        float x = float.Parse(pos[0]);
                        float y = float.Parse(pos[1]);
                        float z = float.Parse(pos[2]);
                        targetPos = new Vector3D(x, y, z);
                        curStatus = TurretStatus.Aiming;
                        break;
                    }
                case "Fire":
                    {
                        Trigger.Trigger();
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
        }
    }
}
