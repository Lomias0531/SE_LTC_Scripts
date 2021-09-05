using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace SEScript
{
    class LTC_PointDefense:API
    {
        IMyMotorStator HorizontalRot;
        IMyMotorStator VerticalRot;
        IMyRemoteControl Remote;
        List<IMySmallGatlingGun> weapons;
        IMyProgrammableBlock FireControl;
        IMySensorBlock IFF;

        bool CheckComponents = false;
        int FireLength = 0;
        Vector3D TargetPos;
        Vector2D TargetOffset;
        PointStatus curStatus = PointStatus.Idle;
        Random rnd;
        int AimingLag = 0;
        enum PointStatus
        {
            Idle,
            Firing,
        }
        void Main(string arg)
        {
            if(!CheckComponents)
            {
                CheckComponent();
                return;
            }
            EcecuteCommands(arg);
            switch(curStatus)
            {
                case PointStatus.Firing:
                    {
                        Echo("Firing");
                        AimByRotor();
                        break;
                    }
                case PointStatus.Idle:
                    {
                        Echo("Idle");
                        RestorePos();
                        FireControl.CustomData += "FireControl|PointDefenseRequestTarget|" + Me.GetId() + "|+";
                        break;
                    }
            }
        }
        void CheckComponent()
        {
            FireControl = GridTerminalSystem.GetBlockWithName("LTC_FireControl") as IMyProgrammableBlock;
            if(FireControl == null)
            {
                Echo("No fireControl module found!");
                return;
            }
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            foreach (var group in groups)
            {
                List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
                group.GetBlocks(terminals);
                if (terminals.Contains(Me as IMyTerminalBlock))
                {
                    weapons = new List<IMySmallGatlingGun>();
                    group.GetBlocksOfType(weapons);
                    if(weapons.Count == 0)
                    {
                        continue;
                    }

                    List<IMyMotorStator> rotors = new List<IMyMotorStator>();
                    group.GetBlocksOfType(rotors);
                    foreach (var item in rotors)
                    {
                        if(item.CustomName == "VerticalRot")
                        {
                            VerticalRot = item;
                        }
                        if(item.CustomName == "HorizontalRot")
                        {
                            HorizontalRot = item;
                        }
                    }
                    if(VerticalRot == null || HorizontalRot == null)
                    {
                        continue;
                    }

                    List<IMyRemoteControl> rem = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(rem);
                    if(rem.Count == 0)
                    {
                        continue;
                    }
                    Remote = rem[0];

                    List<IMySensorBlock> cam = new List<IMySensorBlock>();
                    group.GetBlocksOfType(cam);
                    if (cam.Count == 0)
                    {
                        continue;
                    }
                    IFF = cam[0];
                }
            }
            if(weapons.Count == 0)
            {
                Echo("No weapons!");
                return;
            }
            if (HorizontalRot == null || VerticalRot == null)
            {
                Echo("Rotor error");
                return;
            }
            if(Remote == null)
            {
                Echo("No remote found");
                return;
            }
            if (IFF == null)
            {
                Echo("Camera");
                return;
            }

            CheckComponents = true;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            rnd = new Random();
            return;
        }
        void RestorePos()
        {
            Vector3D restorePos = HorizontalRot.GetPosition() + HorizontalRot.WorldMatrix.Forward * 1000f;
            LookAtDirection(restorePos, false);
            return;
        }
        void AimByRotor()
        {
            LookAtDirection(TargetPos, true);
        }
        void LookAtDirection(Vector3D pos, bool isFiring)
        {
            AimingLag += 1;
            Echo(AimingLag.ToString());
            if (AimingLag > 50)
            {
                FireControl.CustomData += "FireControl|PointDefenseRequestTarget|" + Me.GetId() + "|+";
                AimingLag = 0;
            }
            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), Remote.WorldMatrix.Forward, Remote.WorldMatrix.Up);
            Vector3D posAngle = Vector3D.Normalize(Vector3D.TransformNormal(pos - Remote.GetPosition(), matrix));
            if(isFiring)
            {
                int offsetX = rnd.Next(-10, 10);
                int offsetY = rnd.Next(-10, 10);
                posAngle.X += (float)(offsetX / 50f);
                posAngle.Y += (float)(offsetY / 50f);
            }
            if (posAngle.X > -0.1f && posAngle.X < 0.1f && posAngle.Y > -0.1f && posAngle.Y < 0.1f && isFiring)
            {
                Fire();
            }
            VerticalRot.TargetVelocityRPM = (float)posAngle.Y * 50;
            HorizontalRot.TargetVelocityRPM = (float)posAngle.X * 50;
        }
        void Fire()
        {
            List<MyDetectedEntityInfo> detects = new List<MyDetectedEntityInfo>();
            IFF.DetectedEntities(detects);
            foreach (var item in detects)
            {
                if(item.Relationship == VRage.Game.MyRelationsBetweenPlayerAndBlock.Owner)
                {
                    FireControl.CustomData += "FireControl|PointDefenseRequestTarget|" + Me.GetId() + "|+";
                    AimingLag = 0;
                    return;
                }
            }
            foreach (IMySmallGatlingGun gun in weapons)
            {
                gun.ApplyAction("ShootOnce");
            }
        }
        void EcecuteCommands(string cmd)
        {
            string command = string.IsNullOrEmpty(cmd) ? Me.CustomData : cmd;
            string[] cmd1 = command.Split('|');
            if(cmd1[0] != "PointDefense")
            {
                return;
            }
            switch(cmd1[1])
            {
                default:
                    {
                        break;
                    }
                case "Idle":
                    {
                        curStatus = PointStatus.Idle;
                        break;
                    }
                case "Target":
                    {
                        string[] pos = cmd1[2].Split('_');
                        float x = float.Parse(pos[0]);
                        float y = float.Parse(pos[1]);
                        float z = float.Parse(pos[2]);
                        TargetPos = new Vector3D(x, y, z);
                        curStatus = PointStatus.Firing;
                        break;
                    }
            }
            Me.CustomData = "";
        }
    }
}
