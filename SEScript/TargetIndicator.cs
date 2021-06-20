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
        List<IMyRemoteControl> MissileComputers;
        void Main()
        {
            if (!Checked)
            {
                CheckComponents();
                return;
            }
            ControlCam();
            if(controller.CustomData == "SelectTarget")
            {
                SelectTarget();
            }
        }
        void CheckComponents()
        {
            List<IMyTerminalBlock> terminal = new List<IMyTerminalBlock>();
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            foreach (var group in groups)
            {
                List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
                group.GetBlocks(terminals);
                if (terminals.Contains(this as IMyTerminalBlock))
                {
                    List<IMyCameraBlock> cams = new List<IMyCameraBlock>();
                    group.GetBlocksOfType(cams);
                    if (cams.Count == 0)
                    {
                        Echo("Camera Error");
                        return;
                    }
                    thisCam = cams[0];

                    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(remotes);
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

                MissileComputers = new List<IMyRemoteControl>();
                foreach (var item in terminals)
                {
                    if (item.CustomName == "LTC_MissileComputer")
                    {
                        MissileComputers.Add(item as IMyRemoteControl);
                    }
                }

                if (MissileComputers.Count == 0)
                {
                    Echo("Missile Error");
                    return;
                }

                Checked = true;
                return;
            }
        }
        void ControlCam()
        {
            if (controller.RotationIndicator.X > 0)
            {

            }
            if (controller.RotationIndicator.X < 0)
            {

            }
            if (controller.RotationIndicator.Y > 0)
            {

            }
            if (controller.RotationIndicator.Y < 0)
            {

            }
        }
        void SelectTarget()
        {
            MyDetectedEntityInfo target = thisCam.Raycast(4000);
            if (target.IsEmpty())
            {
                return;
            }
            foreach (var MissileCPU in MissileComputers)
            {
                MissileCPU.CustomData = "TargetPos|" + target.HitPosition.Value.X.ToString() + "_" + target.HitPosition.Value.Y.ToString() + "_" + target.HitPosition.Value.ToString();
            }
        }
    }
}
