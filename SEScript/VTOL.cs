using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace SEScript
{
    class VTOL : API
    {
        List<IMyMotorStator> VTOLRot;
        List<IMyRemoteControl> VTOLDir;
        List<IMyThrust> VTOLThrust;
        IMyShipController MainControl;

        bool Checked = false;
        void Main()
        {
            if(!Checked)
            {
                CheckComponents();
                return;
            }
            AutoBreak();
            ManualControl();
        }
        void CheckComponents()
        {
            VTOLDir = new List<IMyRemoteControl>();
            VTOLRot = new List<IMyMotorStator>();
            VTOLThrust = new List<IMyThrust>();
            GridTerminalSystem.GetBlocksOfType(VTOLThrust, blocks => blocks.CustomName.Contains("VTOL"));
            if(VTOLThrust.Count == 0)
            {
                Echo("Thrust");
                return;
            }
            GridTerminalSystem.GetBlocksOfType(VTOLDir, blocks => blocks.CustomName.Contains("VTOL"));
            if(VTOLDir.Count == 0)
            {
                Echo("Remote");
                return;
            }
            GridTerminalSystem.GetBlocksOfType(VTOLRot, blocks => blocks.CustomName.Contains("VTOL"));
            if(VTOLRot.Count == 0)
            {
                Echo("Rotor");
                return;
            }
            MainControl = GridTerminalSystem.GetBlockWithName("LTC_MainControl") as IMyShipController;
            if(MainControl == null)
            {
                Echo("Controller");
                return;
            }

            Checked = true;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            return;
        }
        void AutoBreak()
        {
            if (MainControl.MoveIndicator.Y != 0 || MainControl.MoveIndicator.Z != 0) return;
            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), MainControl.WorldMatrix.Forward, MainControl.WorldMatrix.Up);
            Vector3D curVelDirToSelf = Vector3D.TransformNormal(MainControl.GetShipVelocities().LinearVelocity * -1, matrix);
            for(int i = 0;i<2;i++)
            {
                VTOLRot[i].TargetVelocityRPM = (float)(curVelDirToSelf.Y * 50f);
            }
            foreach (var item in VTOLThrust)
            {
                item.ThrustOverridePercentage = (float)MainControl.GetShipSpeed();
            }
        }
        void ManualControl()
        {
            Vector3D MoveDir = MainControl.MoveIndicator;
            for(int i = 0;i<2;i++)
            {
                Vector3D targetPos = VTOLRot[i].GetPosition() + new Vector3D(0, MoveDir.Y * 30, MoveDir.Z * 30);
                MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), VTOLDir[i].WorldMatrix.Forward, VTOLDir[i].WorldMatrix.Up);
                Vector3D moveDir = Vector3D.TransformNormal(targetPos - VTOLRot[i].GetPosition(), matrix);
                VTOLRot[i].TargetVelocityRPM = (float)(moveDir.Y * 50f);
            }
            foreach (var item in VTOLThrust)
            {
                item.ThrustOverridePercentage = 1f;
            }
        }
    }
}
