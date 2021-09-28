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
            SelectedTarget = GetTarget(TargetPos);
            if (!SelectedTarget.IsEmpty())
            {
                TargetPos = SelectedTarget.BoundingBox.Center;
            }
            else
            {
                ResumeIdle();
                return;
            }
            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), controller.WorldMatrix.Forward, controller.WorldMatrix.Up);
            Vector3D posAngle = Vector3D.Normalize(Vector3D.TransformNormal(TargetPos - controller.GetPosition(), matrix));
            Echo(TargetPos.ToString("F2"));
            Echo(posAngle.ToString("F2"));

            HoriRot.TargetVelocityRPM = (float)posAngle.Y * 50;
            VertRot.TargetVelocityRPM = (float)posAngle.X * 50;
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
            TargetPos = SelectedTarget.BoundingBox.Center;
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
            MyDetectedEntityInfo target = new MyDetectedEntityInfo();
            foreach (var camera in camArray)
            {
                if(camera.TimeUntilScan(Range) == 0)
                {
                    target = camera.Raycast(Range);
                    return target;
                }
            }
            return target;
        }
        MyDetectedEntityInfo GetTarget(Vector3D targetPos)
        {
            MyDetectedEntityInfo target = new MyDetectedEntityInfo();
            foreach (var camera in camArray)
            {
                if (camera.TimeUntilScan(Vector3D.Distance(targetPos,camera.GetPosition()) + 100) == 0)
                {
                    target = camera.Raycast(targetPos);
                    return target;
                }
            }
            return target;
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
                            Vector3D hitPos = (Vector3D)SelectedTarget.HitPosition;
                            FireControl.CustomData += "FireControl|TurretAimAt|" + hitPos.X.ToString() + "_" + hitPos.Y.ToString() + "_" + hitPos.Z.ToString() + "_" + SelectedTarget.Velocity.X + "_" + SelectedTarget.Velocity.Y + "_" + SelectedTarget.Velocity.Z + "|+";
                            LockTarget();
                        }
                        break;
                    }
                case "TargetMissile":
                    {
                        SelectTarget();
                        if (!SelectedTarget.IsEmpty())
                        {
                            FireControl.CustomData += "FireControl|MissileLaunchAt|" + SelectedTarget.Position.X.ToString() + "_" + SelectedTarget.Position.Y.ToString() + "_" + SelectedTarget.Position.Z.ToString() + "_" + SelectedTarget.Velocity.X + "_" + SelectedTarget.Velocity.Y + "_" + SelectedTarget.Velocity.Z + "|+";
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
