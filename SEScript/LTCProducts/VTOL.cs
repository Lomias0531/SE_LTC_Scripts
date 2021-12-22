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

            for(int i = 0;i<2;i++)
            {
                MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), VTOLDir[i].WorldMatrix.Forward, VTOLDir[i].WorldMatrix.Up);
                Vector3D curVelDirToSelf = Vector3D.TransformNormal(MainControl.GetShipVelocities().LinearVelocity, matrix);
                VTOLRot[i].TargetVelocityRPM = (float)(curVelDirToSelf.Y * 50f);
            }
            foreach (var item in VTOLThrust)
            {
                item.ThrustOverridePercentage = (float)MainControl.GetShipSpeed() * 5f;
            }
        }
        void ManualControl()
        {
            Vector3D playerControl = MainControl.MoveIndicator;

            if (playerControl.Z != 0 || playerControl.Y != 0)
            {
                foreach (var thrust in VTOLThrust)
                {
                    thrust.ThrustOverridePercentage = 1f;
                }
                for (int i = 0; i < 2; i++)
                {
                    Vector3D GDir = VTOLDir[i].GetNaturalGravity();
                    Vector3D tarPos = VTOLDir[i].GetPosition() + new Vector3D(0, GDir.Y * playerControl.Y * -1, GDir.Z * playerControl.Z);
                    tarPos = Vector3D.Reject(tarPos, VTOLDir[i].WorldMatrix.Left);
                    MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), VTOLDir[i].WorldMatrix.Forward, VTOLDir[i].WorldMatrix.Up);
                    Vector3D angle = Vector3D.TransformNormal(VTOLDir[i].GetPosition() - tarPos, matrix);
                    VTOLRot[i].TargetVelocityRPM = (float)angle.Z * 50f * (i == 0 ? -1 : 1);
                }
            }
        }
    }
}
