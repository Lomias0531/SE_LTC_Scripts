using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace SEScript.LTCProducts
{
    class LTC_GATE : API
    {
        /*
         * G.A.T.E.
         * Grand Accelerate Thrust Engine
         */
        List<IMyGyro> GyroScopes;
        List<IMyPistonBase> Forward;
        List<IMyPistonBase> Backward;
        List<IMyPistonBase> Leftward;
        List<IMyPistonBase> Rightward;
        List<IMyPistonBase> Upward;
        List<IMyPistonBase> Downward;
        List<IMyShipController> LTCShipControl;

        bool CheckReady = false;
        FlightStatus curFlightStatus = FlightStatus.GATEOff;
        /// <summary>
        /// 玩家主控翻转
        /// </summary>
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
        /// <summary>
        /// 玩家主控移动
        /// </summary>
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
        List<LTCGyroControl> gyroControls;

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
            if (!CheckReady)
            {
                CheckComponents();
                return;
            }
            CheckFlightStatus();
            switch (curFlightStatus)
            {
                default:
                    break;
                case FlightStatus.GATEOn:
                    GATEControl();
                    break;
            }
            GATERotate();
        }
        void CheckComponents()
        {
            GridTerminalSystem.GetBlocksOfType(GyroScopes);
            if (GyroScopes.Count == 0)
            {
                Echo("找不到陀螺仪");
                return;
            }
            GridTerminalSystem.GetBlocksOfType(Forward, x => x.CustomName == "Forward");
            GridTerminalSystem.GetBlocksOfType(Backward, x => x.CustomName == "Backward");
            GridTerminalSystem.GetBlocksOfType(Leftward, x => x.CustomName == "Leftward");
            GridTerminalSystem.GetBlocksOfType(Rightward, x => x.CustomName == "Righrward");
            GridTerminalSystem.GetBlocksOfType(Upward, x => x.CustomName == "Upward");
            GridTerminalSystem.GetBlocksOfType(Downward, x => x.CustomName == "Downward");

            GridTerminalSystem.GetBlocksOfType(LTCShipControl);
            if (LTCShipControl.Count == 0)
            {
                Echo("没有船只控制器");
                return;
            }
            InitSystem();
        }
        void InitSystem()
        {
            gyroControls = new List<LTCGyroControl>();
            foreach (var item in GyroScopes)
            {
                LTCGyroControl gyro = new LTCGyroControl(item, LTCShipControl[0]);
                gyroControls.Add(gyro);
            }
            CheckReady = true;
        }
        void CheckFlightStatus()
        {
            if (LTCShipControl[0].GetShipSpeed() > 10)
            {
                curFlightStatus = FlightStatus.GATEOn;
            }
            else
            {
                curFlightStatus = FlightStatus.GATEOff;
            }
        }
        void GATEControl()
        {
            GATEAutoBreak();
            GATEAccelerate();
        }
        void GATEAutoBreak()
        {
            //控制器当前朝向
            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), LTCShipControl[0].WorldMatrix.Forward, LTCShipControl[0].WorldMatrix.Up);
            //控制器当前朝向的速度
            Vector3D actualVel = Vector3D.Transform(LTCShipControl[0].GetShipVelocities().LinearVelocity, matrix);
            if (controlAcl.X == 0)
            {
                GoLeft(actualVel.X > 10);
                GoRight(actualVel.X < -10);
            }
            if (controlAcl.Y == 0)
            {
                GoDown(actualVel.Y > 10);
                GoUp(actualVel.Y < -10);
            }
            if (controlAcl.Z == 0)
            {
                GoBackward(actualVel.Z > 10);
                GoForward(actualVel.Z < -10);
            }
        }
        void GATEAccelerate()
        {
            GoRight(controlAcl.X > 0);
            GoLeft(controlAcl.X < 0);
            GoUp(controlAcl.Y > 0);
            GoDown(controlAcl.Y < 0);
            GoForward(controlAcl.Z > 0);
            GoBackward(controlAcl.Z < 0);
        }
        void GATERotate()
        {
            foreach (var gyro in gyroControls)
            {
                gyro.Rotate(controlRot);
            }
        }
        #region 引擎控制
        void GoForward(bool isOn)
        {
            if (Forward.Count <= 0) return;
            foreach (var item in Forward)
            {
                if (isOn)
                    item.Extend();
                else
                    item.Retract();
            }
        }
        void GoBackward(bool isOn)
        {
            if (Backward.Count <= 0) return;
            foreach (var item in Backward)
            {
                if (isOn)
                    item.Extend();
                else
                    item.Retract();
            }
        }
        void GoLeft(bool isOn)
        {
            if (Leftward.Count <= 0) return;
            foreach (var item in Leftward)
            {
                if (isOn)
                    item.Extend();
                else
                    item.Retract();
            }
        }
        void GoRight(bool isOn)
        {
            if (Rightward.Count <= 0) return;
            foreach (var item in Rightward)
            {
                if (isOn)
                    item.Extend();
                else
                    item.Retract();
            }
        }
        void GoUp(bool isOn)
        {
            if (Upward.Count <= 0) return;
            foreach (var item in Upward)
            {
                if (isOn)
                    item.Extend();
                else
                    item.Retract();
            }
        }
        void GoDown(bool isOn)
        {
            if (Downward.Count <= 0) return;
            foreach (var item in Downward)
            {
                if (isOn)
                    item.Extend();
                else
                    item.Retract();
            }
        }
        #endregion
        #region 陀螺仪控制
        /// <summary>
        /// 将控制器旋转控制转化为陀螺仪自身旋转控制
        /// </summary>
        public class LTCGyroControl
        {
            public IMyGyro thisGyro;
            readonly Base6Directions.Direction UpFacing;
            readonly Base6Directions.Direction LeftFacing;
            readonly Base6Directions.Direction ForwardFacing;

            public LTCGyroControl(IMyGyro gyro,IMyShipController controller)
            {
                thisGyro = gyro;
                UpFacing = gyro.WorldMatrix.GetClosestDirection(controller.WorldMatrix.Up);
                LeftFacing = gyro.WorldMatrix.GetClosestDirection(controller.WorldMatrix.Left);
                ForwardFacing = gyro.WorldMatrix.GetClosestDirection(controller.WorldMatrix.Forward);
                thisGyro.GyroOverride = true;
            }
            public void Rotate(Vector3 rotIndicator)
            {
                switch (LeftFacing)
                {
                    case Base6Directions.Direction.Up:
                        thisGyro.Yaw = rotIndicator.X;
                        break;
                    case Base6Directions.Direction.Down:
                        thisGyro.Yaw = rotIndicator.X * -1;
                        break;
                    case Base6Directions.Direction.Left:
                        thisGyro.Pitch = rotIndicator.X;
                        break;
                    case Base6Directions.Direction.Right:
                        thisGyro.Pitch = rotIndicator.X * -1;
                        break;
                    case Base6Directions.Direction.Forward:
                        thisGyro.Roll = rotIndicator.X;
                        break;
                    case Base6Directions.Direction.Backward:
                        thisGyro.Roll = rotIndicator.X * -1;
                        break;
                }

                switch (UpFacing)
                {
                    case Base6Directions.Direction.Up:
                        thisGyro.Yaw = rotIndicator.Y;
                        break;
                    case Base6Directions.Direction.Down:
                        thisGyro.Yaw = rotIndicator.Y * -1;
                        break;
                    case Base6Directions.Direction.Left:
                        thisGyro.Pitch = rotIndicator.Y;
                        break;
                    case Base6Directions.Direction.Right:
                        thisGyro.Pitch = rotIndicator.Y * -1;
                        break;
                    case Base6Directions.Direction.Forward:
                        thisGyro.Roll = rotIndicator.Y;
                        break;
                    case Base6Directions.Direction.Backward:
                        thisGyro.Roll = rotIndicator.Y * -1;
                        break;
                }
                
                switch(ForwardFacing)
                {
                    case Base6Directions.Direction.Up:
                        thisGyro.Yaw = rotIndicator.Z;
                        break;
                    case Base6Directions.Direction.Down:
                        thisGyro.Yaw = rotIndicator.Z * -1;
                        break;
                    case Base6Directions.Direction.Left:
                        thisGyro.Pitch = rotIndicator.Z;
                        break;
                    case Base6Directions.Direction.Right:
                        thisGyro.Pitch = rotIndicator.Z * -1;
                        break;
                    case Base6Directions.Direction.Forward:
                        thisGyro.Roll = rotIndicator.Z;
                        break;
                    case Base6Directions.Direction.Backward:
                        thisGyro.Roll = rotIndicator.Z * -1;
                        break;
                }
            }
        }
        #endregion
    }
}
