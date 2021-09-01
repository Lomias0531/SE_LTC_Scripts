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
        void Main(string msg)
        {
            if (!CheckReady)
            {
                CheckComponents();
                return;
            }
            DeseralizeMsg(msg);
            MoveByRotor();
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
        void CheckComponents()
        {
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            foreach (var group in groups)
            {
                List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
                group.GetBlocks(terminals);
                if (terminals.Contains(this as IMyTerminalBlock))
                {
                    //remote = GridTerminalSystem.GetBlockWithName("LTC_TurretRemote") as IMyRemoteControl;
                    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(remotes);
                    if (remotes.Count == 0)
                    {
                        continue;
                    }
                    remote = remotes[0];
                    //VerticalRot = GridTerminalSystem.GetBlockWithName("VerticalRot") as IMyMotorStator;
                    List<IMyMotorStator> vet = new List<IMyMotorStator>();
                    group.GetBlocksOfType(vet, blocks => blocks.Name == "VerticalRot");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    VerticalRot = vet[0];
                    //VerticalRev = GridTerminalSystem.GetBlockWithName("VerticalRotRev") as IMyMotorStator;
                    group.GetBlocksOfType(vet, blocks => blocks.Name == "VerticalRotRev");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    VerticalRev = vet[0];
                    //HorizontalRot = GridTerminalSystem.GetBlockWithName("HorizontalRot") as IMyMotorStator;
                    group.GetBlocksOfType(vet, blocks => blocks.Name == "HorizontalRot");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    HorizontalRot = vet[0];
                    //Trigger = GridTerminalSystem.GetBlockWithName("LTC_Trigger") as IMyTimerBlock;
                    List<IMyTimerBlock> tim = new List<IMyTimerBlock>();
                    group.GetBlocksOfType(tim, blocks => blocks.Name == "LTC_Trigger");
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
            string[] messages = msg.Split("|");
            switch(messages[0])
            {
                default:
                    {
                        return;
                    }
                case "TargetPos":
                    {
                        string[] pos = messages[1].Split("_");
                        float x = float.Parse(pos[0]);
                        float y = float.Parse(pos[1]);
                        float z = float.Parse(pos[2]);
                        targetPos = new Vector3D(x, y, z);
                        return;
                    }
                case "Fire":
                    {
                        Trigger.Trigger();
                        return;
                    }
            }
        }
    }
}
