using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI;

namespace SEScript
{
    class LTC_TargetIndicator : API
    {
        bool Checked = false;
        List<IMyCameraBlock> camArray;
        IMyMotorStator VertRot;
        IMyMotorStator HoriRot;
        IMyRemoteControl controller;
        IMyProgrammableBlock FireControl;
        MyDetectedEntityInfo SelectedTarget;
        bool Locked = false;
        List<IMyGyro> gyros;
        List<IMySoundBlock> sound;
        Vector3D TargetPos;
        Vector3D TargetOffset;
        Random rnd = new Random();
        int TargetCount = 0;
        void Main(string msg)
        {
            if (!Checked)
            {
                CheckComponents();
                return;
            }
            if (Locked)
            {
                TrackTarget();
            }else
            {
                ControlCam();
            }
            Echo("Running");
            Echo("Target acquired: " + Locked);
            if(Locked)
            {
                foreach (var item in camArray)
                {
                    Echo("Cam" + item.TimeUntilScan(Vector3D.Distance(TargetPos, item.GetPosition()) + 100));
                }
            }
            DeseralizeMsg(msg);
        }
        void CheckComponents()
        {
            FireControl = GridTerminalSystem.GetBlockWithName("LTC_FireControl") as IMyProgrammableBlock;
            List<IMyTerminalBlock> terminal = new List<IMyTerminalBlock>();
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            Echo(groups.Count.ToString());
            foreach (var group in groups)
            {
                List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
                group.GetBlocks(terminals);
                if (terminals.Contains(Me as IMyTerminalBlock))
                {
                    camArray = new List<IMyCameraBlock>();
                    group.GetBlocksOfType(camArray);
                    if (camArray.Count == 0)
                    {
                        Echo("Camera Error");
                        return;
                    }
                    foreach (var item in camArray)
                    {
                        item.EnableRaycast = true;
                    }

                    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(remotes);
                    if (remotes.Count == 0)
                    {
                        Echo("Controller Error");
                        return;
                    }
                    controller = remotes[0];

                    gyros = new List<IMyGyro>();
                    group.GetBlocksOfType(gyros);
                    if(gyros.Count == 0)
                    {
                        Echo("Gyros Error");
                        return;
                    }

                    sound = new List<IMySoundBlock>();
                    group.GetBlocksOfType(sound);
                    if(sound.Count == 0)
                    {
                        Echo("No sound");
                    }

                    foreach (var item in terminals)
                    {
                        if (item.CustomName == "VertRot")
                        {
                            VertRot = item as IMyMotorStator;
                            continue;
                        }
                        if (item.CustomName == "HoriRot")
                        {
                            HoriRot = item as IMyMotorStator;
                            continue;
                        }
                    }
                    if (VertRot == null || HoriRot == null)
                    {
                        Echo("Rotor Error");
                        return;
                    }
                }
            }

            Checked = true;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            return;
        }
        void ControlCam()
        {
            HoriRot.TargetVelocityRPM = controller.RotationIndicator.X * -1;
            VertRot.TargetVelocityRPM = controller.RotationIndicator.Y;

            if (controller.MoveIndicator.Y < 0)
            {
                SelectTarget();
            }
        }
        void TrackTarget()
        {
            Vector3D posmove = Vector3D.TransformNormal(TargetOffset, SelectedTarget.Orientation);
            Vector3D targetPos;
            if(TargetOffset.X == 0 && TargetOffset.Y == 0 && TargetOffset.Z == 0)
            {
                targetPos = (Vector3D)SelectedTarget.HitPosition;
            }else
            {
                targetPos = SelectedTarget.Position + posmove + SelectedTarget.Velocity / 60;
            }

            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), controller.WorldMatrix.Forward, controller.WorldMatrix.Up);
            Vector3D posAngle = Vector3D.Normalize(Vector3D.TransformNormal(targetPos - controller.GetPosition(), matrix));
            Echo(targetPos.ToString("F2"));
            Echo(posAngle.ToString("F2"));
            Echo(TargetOffset.ToString("F2"));

            HoriRot.TargetVelocityRPM = (float)posAngle.Y * 50;
            VertRot.TargetVelocityRPM = (float)posAngle.X * 50;

            SelectedTarget = GetTarget(targetPos);
            if (SelectedTarget.IsEmpty())
            {
                ResumeIdle();
                return; 
            }

            MatrixD TargetLookAtMatrix = MatrixD.CreateLookAt(new Vector3D(), SelectedTarget.Orientation.Forward, SelectedTarget.Orientation.Up);
            Vector3D hitpos = (Vector3D)SelectedTarget.HitPosition;
            //hitpos = hitpos + Vector3D.Normalize(hitpos - (Vector3D)controller.Position) * 2;
            TargetOffset = Vector3D.TransformNormal(hitpos - SelectedTarget.Position, TargetLookAtMatrix);
            TargetPos = (Vector3D)SelectedTarget.HitPosition;

            Vector3D hitPos = (Vector3D)SelectedTarget.HitPosition;

            TargetCount = TargetCount > 20 ? 0 : TargetCount += 1;

            if(TargetCount == 0)
            {
                FireControl.CustomData += "FireControl|TurretAimAt|" + hitPos.ToString() + "," + SelectedTarget.Velocity.ToString() + "|+";
                FireControl.CustomData += "FireControl|MissileLaunchAt|" + hitPos.ToString() + "," + SelectedTarget.Velocity.ToString() + "|+";
            }
        }
        void LockTarget()
        {
            if(sound.Count > 0)
            {
                foreach (var item in sound)
                {
                    item.SelectedSound = "发现敌人";
                    item.Play();
                }
            }
            TargetPos = (Vector3D)SelectedTarget.HitPosition;
            //TargetPos = SelectedTarget.BoundingBox.Center;
            Locked = true;
            foreach (var gyro in gyros)
            {
                gyro.GyroOverride = true;
            }
        }
        void ResumeIdle()
        {
            Echo("EEEE");
            if (sound.Count > 0)
            {
                foreach (var item in sound)
                {
                    item.SelectedSound = "目标完成";
                    item.Play();
                }
            }
            Locked = false;
            foreach (var gyro in gyros)
            {
                gyro.GyroOverride = false;
            }
            TargetOffset = new Vector3D();
            Echo("No target");
            FireControl.CustomData += "FireControl|IndicatorIdle|+";
        }
        void SelectTarget()
        {
            SelectedTarget = GetTarget(4000);
            if (SelectedTarget.IsEmpty())
            {
                Echo("No target");
                FireControl.CustomData += "FireControl|IndicatorIdle|+";
                return;
            }
        }
        MyDetectedEntityInfo GetTarget(int Range)
        {
            foreach (var camera in camArray)
            {
                if(camera.CanScan(Range))
                {
                    MyDetectedEntityInfo target = camera.Raycast(Range);
                    return target;
                }
            }
            return new MyDetectedEntityInfo();
        }
        MyDetectedEntityInfo GetTarget(Vector3D targetPos)
        {
            foreach (var camera in camArray)
            {
                if (camera.CanScan(targetPos))
                {
                    MyDetectedEntityInfo target = camera.Raycast(targetPos);
                    return target;
                }
            }
            return new MyDetectedEntityInfo();
        }
        void DeseralizeMsg(string msg)
        {
            string msg1 = string.IsNullOrEmpty(msg) ? Me.CustomData : msg;
            string[] message = msg1.Split('|');
            if (message[0] != "Indicator") return;
            switch(message[1])
            {
                default:
                    {
                        return;
                    }
                case "TargetTurret":
                    {
                        SelectTarget();
                        if (!SelectedTarget.IsEmpty())
                        {
                            FireControl.CustomData += "FireControl|TurretAimAt|" + ((Vector3D)SelectedTarget.HitPosition).ToString() + "," + ((Vector3D)SelectedTarget.Velocity).ToString() + "|+";
                            LockTarget();
                        }
                        break;
                    }
                case "TargetMissile":
                    {
                        SelectTarget();
                        if (!SelectedTarget.IsEmpty())
                        {
                            string TargetPack = "";
                            double targetXL = SelectedTarget.BoundingBox.Size.X * 0.7;
                            double targetYL = SelectedTarget.BoundingBox.Size.Y * 0.7;
                            double targetZL = SelectedTarget.BoundingBox.Size.Z * 0.7;
                            for (int i = 0;i<10;i++)
                            {
                                double x = (rnd.NextDouble() - 0.5) * targetXL;
                                double y = (rnd.NextDouble() - 0.5) * targetYL;
                                double z = (rnd.NextDouble() - 0.5) * targetZL;
                                Vector3D offset = Vector3D.TransformNormal(new Vector3D(x, y, z), SelectedTarget.Orientation);
                                Vector3D missileOffset = new Vector3D(SelectedTarget.Position.X + offset.X, SelectedTarget.Position.Y + offset.Y, SelectedTarget.Position.Z + offset.Z);
                                TargetPack += missileOffset.ToString() + ",";
                            }
                            FireControl.CustomData += "FireControl|MissileLaunchAt|" + TargetPack + "," + ((Vector3D)SelectedTarget.Velocity).ToString() + "|+";
                            LockTarget();
                        }
                        break;
                    }
                case "ResumeManual":
                    {
                        ResumeIdle();
                        break;
                    }
            }
            if(string.IsNullOrEmpty(msg))
                Me.CustomData = "";
        }
    }
}
