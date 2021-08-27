using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace SEScript
{
    class MoveScript:API
    {
        IMyMotorStator VerticalRot;
        IMyMotorStator HorizontalRot;
        IMyRemoteControl remote;
        bool CheckReady = false;
        void Main()
        {
            if(!CheckReady)
            {
                CheckComponents();
                return;
            }
            MoveByRotor();
        }
        void MoveByRotor()
        {
            VerticalRot.TargetVelocityRPM = 0;
            HorizontalRot.TargetVelocityRPM = 0;
            if(remote.RotationIndicator.X>0)
            {
                HorizontalRot.TargetVelocityRPM = 4 * remote.RotationIndicator.X;
            }
            if(remote.RotationIndicator.X<0)
            {
                HorizontalRot.TargetVelocityRPM = -4 * remote.RotationIndicator.X;
            }
            if(remote.RotationIndicator.Y>0)
            {
                VerticalRot.TargetVelocityRPM = 2 * remote.RotationIndicator.Y;
            }
            if(remote.RotationIndicator.Y<0)
            {
                VerticalRot.TargetVelocityRPM = -2 * remote.RotationIndicator.Y;
            }
        }
        void CheckComponents()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            remote = GridTerminalSystem.GetBlockWithName("Remote") as IMyRemoteControl;
            if(remote == null)
            {
                Echo("Remote");
                return;
            }
            VerticalRot = GridTerminalSystem.GetBlockWithName("VerticalRot") as IMyMotorStator;
            if(VerticalRot == null)
            {
                Echo("Vertical");
                return;
            }
            HorizontalRot = GridTerminalSystem.GetBlockWithName("HorizontalRot") as IMyMotorStator;
            if(HorizontalRot == null)
            {
                Echo("Horizontal");
                return;
            }
            CheckReady = true;
            return;
            
        }
    }
}
