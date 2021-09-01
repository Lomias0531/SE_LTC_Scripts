﻿using Sandbox.ModAPI.Ingame;
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
        IMyMotorStator VerticalRev;
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
            HorizontalRot.TargetVelocityRPM = remote.RotationIndicator.Y;
            VerticalRot.TargetVelocityRPM = remote.RotationIndicator.X;
            VerticalRev.TargetVelocityRPM = remote.RotationIndicator.X * -1;
        }
        void CheckComponents()
        {
            IMyBlockGroup thisGroup;
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
            VerticalRev = GridTerminalSystem.GetBlockWithName("VerticalRotRev") as IMyMotorStator;
            if (VerticalRot == null)
            {
                Echo("VerticalRev");
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
