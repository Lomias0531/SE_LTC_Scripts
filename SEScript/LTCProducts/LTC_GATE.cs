using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace SEScript.LTCProducts
{
    class LTC_GATE:API
    {
        /*
         * G.A.T.E.
         * Grand Accelerate Thrust Engine
         */
        List<IMyGyro> GyroScopes;
        List<IMyPistonBase> Forward;
        List<IMyPistonBase> Backward;
        List<IMyPistonBase> Leftward;
        List<IMyPistonBase> Righrward;
        List<IMyPistonBase> Upward;
        List<IMyPistonBase> Downward;
        List<IMyShipController> LTCShipControl;

        bool CheckReady = false;
        FlightStatus curFlightStatus = FlightStatus.GATEOff;
        Vector3D controlRot
        {
            get
            {
                float x = 0;
                float y = 0;
                float z = 0;
                foreach (var control in LTCShipControl)
                {
                    if (control.RollIndicator != 0) z = control.RollIndicator;
                    if (control.RotationIndicator.X != 0) x = control.RotationIndicator.X;
                    if (control.RotationIndicator.Y != 0) y = control.RotationIndicator.Y;
                }
                return new Vector3D(x, y, z);
            }
        }
        Vector3D controlAcl
        {
            get
            {
                float x = 0;
                float y = 0;
                float z = 0;
                foreach (var control in LTCShipControl)
                {
                    if (control.MoveIndicator.X != 0) x = control.MoveIndicator.X;
                    if (control.MoveIndicator.Y != 0) y = control.MoveIndicator.Y;
                    if (control.MoveIndicator.Z != 0) z = control.MoveIndicator.Z;
                }
                return new Vector3D(x, y, z);
            }
        }

        enum FlightStatus
        {
            GATEOn,
            GATEOff,
        }
        Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }
        void Main()
        {
            if(!CheckReady)
            {
                CheckComponents();
                return;
            }
            CheckFlightStatus();
            switch(curFlightStatus)
            {
                default:
                    break;
                case FlightStatus.GATEOn:
                    break;
            }
        }
        void CheckComponents()
        {
            GridTerminalSystem.GetBlocksOfType(GyroScopes);
            if(GyroScopes.Count == 0)
            {
                Echo("找不到陀螺仪");
                return;
            }
            GridTerminalSystem.GetBlocksOfType(Forward, x => x.CustomName == "Forward");
            GridTerminalSystem.GetBlocksOfType(Backward, x => x.CustomName == "Backward");
            GridTerminalSystem.GetBlocksOfType(Leftward, x => x.CustomName == "Leftward");
            GridTerminalSystem.GetBlocksOfType(Righrward, x => x.CustomName == "Righrward");
            GridTerminalSystem.GetBlocksOfType(Upward, x => x.CustomName == "Upward");
            GridTerminalSystem.GetBlocksOfType(Downward, x => x.CustomName == "Downward");

            GridTerminalSystem.GetBlocksOfType(LTCShipControl);
            if(LTCShipControl.Count == 0)
            {
                Echo("没有船只控制器");
                return;
            }
            InitSystem();
        }
        void InitSystem()
        {

            CheckReady = true;
        }
        void CheckFlightStatus()
        {
            if(LTCShipControl[0].GetShipSpeed()>10)
            {
                curFlightStatus = FlightStatus.GATEOn;
            }else
            {
                curFlightStatus = FlightStatus.GATEOff;
            }
        }
        void GATEAutoBreak()
        {
            if(controlAcl.X == 0)
            {
                if(LTCShipControl[0].GetShipVelocities().LinearVelocity.X>0)
                {

                }
            }
            if(controlAcl.Y == 0)
            {

            }
            if(controlAcl.Z == 0)
            {

            }
        }
        void GATEAccelerate()
        {

        }
        void GATERotate()
        {

        }
    }
}
