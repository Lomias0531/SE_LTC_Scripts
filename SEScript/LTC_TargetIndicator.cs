using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

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
        IMyProgrammableBlock FireControl;
        MyDetectedEntityInfo SelectedTarget;
        void Main(string msg)
        {
            if (!Checked)
            {
                CheckComponents();
                return;
            }
            ControlCam();
            Echo("Running");
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

            controlCD = controlCD < 1 ? 0 : controlCD -= 1;
            if(controlCD == 0)
            {
                if (controller.MoveIndicator.Y < 0)
                {
                    SelectTarget();
                    controlCD = 3;
                }
            }
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
            string msg1 = "";
            if(string.IsNullOrEmpty(msg))
            {
                msg1 = Me.CustomData;
            }else
            {
                msg1 = msg; 
            }
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
                                controlCD = 3;
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
                                controlCD = 3;
                            }
                        }
                        break;
                    }
            }
            if(string.IsNullOrEmpty(msg))
                Me.CustomData = "";
        }
    }
}
