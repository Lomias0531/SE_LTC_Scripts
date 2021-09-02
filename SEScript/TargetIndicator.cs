using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

namespace SEScript
{
    class TargetIndicator : API
    {
        bool Checked = false;
        IMyCameraBlock thisCam;
        IMyMotorStator VertRot;
        IMyMotorStator HoriRot;
        IMyRemoteControl controller;
        List<IMyProgrammableBlock> TurretControls;
        void Main(string msg)
        {
            if (!Checked)
            {
                CheckComponents();
                return;
            }
            ControlCam();
            DeseralizeMsg(msg);
        }
        void CheckComponents()
        {
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
                    Echo("EEE");
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

            TurretControls = new List<IMyProgrammableBlock>();
            GridTerminalSystem.GetBlocksOfType(TurretControls);
            if (TurretControls.Count == 0)
            {
                Echo("Turret Error");
                return;
            }

            Checked = true;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            return;
        }
        void ControlCam()
        {
            HoriRot.TargetVelocityRPM = controller.RotationIndicator.X * -1;
            VertRot.TargetVelocityRPM = controller.RotationIndicator.Y;

            SelectTarget();

            if(controller.MoveIndicator.Y>0)
            {
                FireCommand();
            }
        }
        void SelectTarget()
        {
            Me.CustomData = "Indicator|SelectTarget";
            Vector3D destination = (Vector3D)thisCam.Position + (Vector3D)thisCam.WorldMatrix.Forward * 4000;
            MyDetectedEntityInfo target = thisCam.Raycast(destination);
            if (target.IsEmpty())
            {
                Echo("No target");
                return;
            }

            Echo("Target Acquired");
            foreach (var turretControl in TurretControls)
            {
                turretControl.CustomData = "Turret|TargetPos|" + target.HitPosition.Value.X.ToString() + "_" + target.HitPosition.Value.Y.ToString() + "_" + target.HitPosition.Value.ToString();
            }
        }
        void FireCommand()
        {
            foreach (var turretControl in TurretControls)
            {
                turretControl.CustomData = "Turret|Fire|111";
            }
        }
        void DeseralizeMsg(string msg)
        {
            msg = Me.CustomData;
            string[] message = msg.Split('|');
            if (message[0] != "Indicator") return;
            switch(message[1])
            {
                default:
                    {
                        return;
                    }
                case "Control":
                    {
                        controller.IsMainCockpit = true;
                        break;
                    }
                case "SelectTarget":
                    {
                        SelectTarget();
                        break;
                    }
            }
            //Me.CustomData = "";
        }
    }
}
