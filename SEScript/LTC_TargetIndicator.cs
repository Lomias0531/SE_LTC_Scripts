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
        IMyCameraBlock thisCam;
        IMyMotorStator VertRot;
        IMyMotorStator HoriRot;
        IMyRemoteControl controller;
        int controlCD = 0;
        int controlERE = 100;
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
            controlCD = controlCD < 1 ? 0 : controlCD -= 1;
            if (Locked)
            {
                TrackTarget();
            }else
            {
                ControlCam();
            }
            Echo("Running");
            Echo("Target acquired: " + Locked);
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
                    List<IMyCameraBlock> cams = new List<IMyCameraBlock>();
                    group.GetBlocksOfType(cams);
                    Echo("Cam " + cams.Count.ToString());
                    if (cams.Count == 0)
                    {
                        Echo("Camera Error");
                        return;
                    }
                    thisCam = cams[0];
                    thisCam.EnableRaycast = true;

                    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(remotes);
                    Echo("Controller " + remotes.Count.ToString());
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

            if(controlCD == 0)
            {
                if (controller.MoveIndicator.Y < 0)
                {
                    SelectTarget();
                    controlCD = controlERE;
                }
            }
        }
        void TrackTarget()
        {
            if (controlCD == 0)
            {
                controlCD = controlERE;
                SelectedTarget = thisCam.Raycast(4000);
                if(!SelectedTarget.IsEmpty())
                {
                    TargetPos = SelectedTarget.BoundingBox.Center;                    
                }
                else
                {
                    ResumeIdle();
                    return;
                }
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
            SelectedTarget = thisCam.Raycast(4000);
            if (SelectedTarget.IsEmpty())
            {
                Echo("No target");
                FireControl.CustomData += "FireControl|IndicatorIdle|+";
                return;
            }
        }
        void DeseralizeMsg(string msg)
        {
            string msg1 = string.IsNullOrEmpty(msg) ? Me.CustomData : msg;
            controlCD = controlCD < 1 ? 0 : controlCD -= 1;
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
                        if(controlCD == 0)
                        {
                            SelectTarget();
                            if(!SelectedTarget.IsEmpty())
                            {
                                Vector3D hitPos = (Vector3D)SelectedTarget.HitPosition;
                                FireControl.CustomData += "FireControl|TurretAimAt|" + hitPos.X.ToString() + "_" + hitPos.Y.ToString() + "_" + hitPos.Z.ToString() + "_" + SelectedTarget.Velocity.X + "_" + SelectedTarget.Velocity.Y + "_" + SelectedTarget.Velocity.Z + "|+";
                                controlCD = controlERE;
                                LockTarget();
                            }
                        }
                        break;
                    }
                case "TargetMissile":
                    {
                        if (controlCD == 0)
                        {
                            SelectTarget();
                            if(!SelectedTarget.IsEmpty())
                            {
                                FireControl.CustomData += "FireControl|MissileLaunchAt|" + SelectedTarget.Position.X.ToString() + "_" + SelectedTarget.Position.Y.ToString() + "_" + SelectedTarget.Position.Z.ToString() + "_" + SelectedTarget.Velocity.X + "_" + SelectedTarget.Velocity.Y + "_" + SelectedTarget.Velocity.Z + "|+";
                                controlCD = controlERE;
                                LockTarget();
                            }
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
