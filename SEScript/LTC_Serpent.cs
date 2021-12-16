using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI;
using VRageMath;

namespace SEScript
{
    class LTC_Serpent:API
    {
        List<IMyCameraBlock> Scanners;
        IMyRemoteControl Remote;
        List<IMyWarhead> WarHeads;
        List<IMyGyro> Gyros;
        List<IMyShipMergeBlock> Merge;
        List<IMyThrust> Thrusts;
        IMyTimerBlock spark;
        bool CheckReady = false;

        //float MissileMass = 1328;
        float MissileMass = 1328;
        bool launched = false;
        Vector3D TargetPos;
        //Vector3D TargetVel;
        float TimeStamp = 0;
        int BreakThroughCount = 0;
        Vector3D BreakThroughOffset;
        double YawPrev;
        double PitchPrev;
        Random rnd = new Random();
        Vector3D MissilePosPev;
        Vector3D TargetPosPev;
        int GuideCount = 100;
        int EvadeDistance = 800;

        IMyUnicastListener unicastListener;
        void Main(string arg)
        {
            Echo(TimeStamp.ToString());
            TimeStamp += 1;
            if (!CheckReady)
            {
                CheckComponents();
                return;
            }
            ExecuteCmd(arg);
            if(launched)
            {
                Echo("TARGET LOCKED");
                //ScanTarget();
                TrackTarget();
            }else
            {
                Echo("STAND BY");
            }
        }
        void CheckComponents()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            Merge = new List<IMyShipMergeBlock>();
            GridTerminalSystem.GetBlocksOfType(Merge, item => item.CustomName.Contains("MissileMerge"));
            foreach (var item in Merge)
            {
                item.Enabled = false;
            }

            if (TimeStamp < 10) return;

            Scanners = new List<IMyCameraBlock>();
            GridTerminalSystem.GetBlocksOfType(Scanners);
            if (Scanners.Count == 0)
            {
                Echo("Cams");
                return;
            }
            List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
            GridTerminalSystem.GetBlocksOfType(remotes);
            if (remotes.Count == 0)
            {
                Echo("Remote");
                return;
            }
            Remote = remotes[0];
            WarHeads = new List<IMyWarhead>();
            GridTerminalSystem.GetBlocksOfType(WarHeads);
            if (WarHeads.Count == 0)
            {
                Echo("Warheads");
                return;
            }
            Gyros = new List<IMyGyro>();
            GridTerminalSystem.GetBlocksOfType(Gyros);
            if (Gyros.Count == 0)
            {
                Echo("Gyros");
                return;
            }
            foreach (var item in Gyros)
            {
                item.GyroOverride = true;
            }
            Thrusts = new List<IMyThrust>();
            GridTerminalSystem.GetBlocksOfType(Thrusts);
            if (Thrusts.Count == 0)
            {
                Echo("Thrust");
                return;
            }
            spark = (IMyTimerBlock)GridTerminalSystem.GetBlockWithName("TriggerSpark");

            unicastListener = IGC.UnicastListener;
            CheckReady = true;
            return;
        }
        
        void TrackTarget()
        {
            if(Vector3D.Distance(TargetPos, Gyros[0].CubeGrid.GetPosition()) < 5f)
            {
                foreach (var item in WarHeads)
                {
                    item.Detonate();
                }
            }

            if(TimeStamp < GuideCount)
            {
                return;
            }
            if(TimeStamp == GuideCount)
            {
                for (int i = 0; i < 3; i++)
                {
                    int index = rnd.Next(0, 10);
                    IGC.SendBroadcastMessage("MissilesChannel" + index.ToString(), "Missile|LaunchConfirmed", TransmissionDistance.TransmissionDistanceMax);
                }
            }

            //计算可提供的最大加速度
            float maxThrust = 0;
            foreach (var thr in Thrusts)
            {
                maxThrust += thr.MaxEffectiveThrust;
            }
            double missileAcc = maxThrust / MissileMass;
            ////排除不需要的速度
            //Vector3D tarN = Vector3D.Normalize(TargetPos - Remote.CubeGrid.WorldVolume.Center); //当前位置指向目标位置的单位指向
            //Vector3D rv = Vector3D.Reject((TargetVel - Remote.GetShipVelocities().LinearVelocity) / 60, tarN); //相对速度向量排除指向
            //                                                                        //Vector3D ra = Vector3D.Reject(TargetVel, tarN); //相对加速度，可不考虑
            //Vector3D ra = Vector3D.Zero;
            ////计算不需要的速度
            ////Vector3D rvN = Vector3D.Normalize(rv); //排除后的相对速度单位指向
            ////double newlen = Math.Atan2(rv.Length(), 5); //相对速度的大小，通过Atan2限定
            ////Vector3D newrv = rvN * newlen;
            //Vector3D newrv = rv;
            //double GuideRate = 0.3;
            //Vector3D rdo = newrv * GuideRate * 60 + ra * 0.5; //侧向加速度
            //                                                  //计算抵消重力需要的加速度
            //Vector3D rd = rdo - Remote.GetNaturalGravity(); //需要抵消掉的加速度
            //                                                //double rdl = rd.Length();
            //                                                //剩余加速度
            //Vector3D rd2 = Vector3D.Reject(rd, tarN); //需要的侧向加速度
            //double rd2l = rd2.Length();
            //if (missileAcc < rd2l) missileAcc = rd2l;
            //double pdl = Math.Sqrt(missileAcc * missileAcc - rd2l * rd2l);
            ////剩余加速度方向
            //Vector3D pdN = Vector3D.Normalize(TargetPos - Remote.CubeGrid.WorldVolume.Center);
            ////剩余加速度
            //Vector3D pd = pdN * pdl;
            ////总加速度
            //Vector3D sd = pd + rd2;
            ////总加速度方向
            //Vector3D nam = Vector3D.Normalize(sd);
            //var missileLookAt = MatrixD.CreateLookAt(new Vector3D(), Remote.WorldMatrix.Up, Remote.WorldMatrix.Backward);
            //var amToMe = Vector3D.TransformNormal(nam, missileLookAt);

            //Vector3D posAngle = amToMe;


            Vector3D MissilePosition = Gyros[0].CubeGrid.WorldVolume.Center;
            Vector3D MissilePositionPrev = MissilePosPev;
            Vector3D MissileVelocity = (MissilePosition - MissilePositionPrev) * 60;

            Vector3D TargetPosition = TargetPos;
            Vector3D TargetPositionPrev = TargetPosPev;
            Vector3D TargetVelocity = (TargetPosition - TargetPosPev) * 60;

            //Uses RdavNav Navigation APN Guidance System
            //-----------------------------------------------

            //Setup LOS rates and PN system
            Vector3D LOS_Old = Vector3D.Normalize(TargetPositionPrev - MissilePositionPrev);
            Vector3D LOS_New = Vector3D.Normalize(TargetPosition - MissilePosition);
            Vector3D Rel_Vel = Vector3D.Normalize(TargetVelocity - MissileVelocity);

            //And Assigners
            Vector3D am = new Vector3D(1, 0, 0); 
            double LOS_Rate; 
            Vector3D LOS_Delta;
            Vector3D MissileForwards = Thrusts[0].WorldMatrix.Backward;

            //Vector/Rotation Rates
            if (LOS_Old.Length() == 0)
            { LOS_Delta = new Vector3D(0, 0, 0); LOS_Rate = 0.0; }
            else
            { LOS_Delta = LOS_New - LOS_Old; LOS_Rate = LOS_Delta.Length() * 60; }

            //-----------------------------------------------

            //Closing Velocity
            double Vclosing = (TargetVelocity - MissileVelocity).Length();

            //If Under Gravity Use Gravitational Accel
            Vector3D GravityComp = -Remote.GetNaturalGravity();

            //Calculate the final lateral acceleration
            Vector3D LateralDirection = Vector3D.Normalize(Vector3D.Cross(Vector3D.Cross(Rel_Vel, LOS_New), Rel_Vel));
            Vector3D LateralAccelerationComponent = LateralDirection * 3 * LOS_Rate * Vclosing + LOS_Delta * 9.8 * (0.5 * 3); //Eases Onto Target Collision LOS_Delta * 9.8 * (0.5 * Gain)

            //If Impossible Solution (ie maxes turn rate) Use Drift Cancelling For Minimum T
            double OversteerReqt = (LateralAccelerationComponent).Length() / missileAcc;
            if (OversteerReqt > 0.98)
            {
                LateralAccelerationComponent = missileAcc * Vector3D.Normalize(LateralAccelerationComponent + (OversteerReqt * Vector3D.Normalize(-MissileVelocity)) * 40);
            }

            //Calculates And Applies Thrust In Correct Direction (Performs own inequality check)
            double ThrustPower = Vector_Projection_Scalar(MissileForwards, Vector3D.Normalize(LateralAccelerationComponent)); //TESTTESTTEST
            //ThrustPower = This_Missile.IsLargeGrid ? MathHelper.Clamp(ThrustPower, 0.9, 1) : ThrustPower;

            ThrustPower = MathHelper.Clamp(ThrustPower, 0.4, 1); //for improved thrust performance on the get-go
            foreach (IMyThrust thruster in Thrusts)
            {
                if (thruster.ThrustOverride != (thruster.MaxThrust * ThrustPower)) //12 increment inequality to help conserve on performance
                { thruster.ThrustOverride = (float)(thruster.MaxThrust * ThrustPower); }
            }

            //Calculates Remaining Force Component And Adds Along LOS
            double RejectedAccel = Math.Sqrt(missileAcc * missileAcc - LateralAccelerationComponent.LengthSquared()); //Accel has to be determined whichever way you slice it
            if (double.IsNaN(RejectedAccel)) { RejectedAccel = 0; }
            LateralAccelerationComponent = LateralAccelerationComponent + LOS_New * RejectedAccel;

            //-----------------------------------------------

            //Guides To Target Using Gyros
            am = Vector3D.Normalize(LateralAccelerationComponent + GravityComp);

            //Commence breakthrough action
            var distance = (TargetPos - Remote.CubeGrid.WorldVolume.Center).Length();
            if (distance <= EvadeDistance)
            {
                Echo("Evading");
                BreakThroughCount = BreakThroughCount > 0 ? BreakThroughCount -= 1 : 0;
                if (BreakThroughCount == 0)
                {
                    BreakThroughCount = rnd.Next(300, 600);
                    var offsetScale = (distance - 500) > 0 ? (distance - 500) / 300 : 0;
                    BreakThroughOffset = new Vector3D(rnd.Next(-30, 30) * offsetScale, rnd.Next(-30, 30) * offsetScale, 0);
                }
                Vector3D offset3D = new Vector3D(BreakThroughOffset.X / 180 * Math.PI, BreakThroughOffset.Y / 180 * Math.PI, BreakThroughOffset.Z / 180 * Math.PI);
                Quaternion offset = new Quaternion((float)(Math.Cos(offset3D.Y * 0.5f) * Math.Sin(offset3D.X * 0.5f) * Math.Cos(offset3D.Z * 0.5f) + Math.Sin(offset3D.Y * 0.5f) * Math.Cos(offset3D.X * 0.5f) * Math.Sin(offset3D.Z * 0.5f)),
                                                (float)(Math.Cos(offset3D.Y * 0.5f) * Math.Cos(offset3D.X * 0.5f) * Math.Sin(offset3D.Z * 0.5f) - Math.Sin(offset3D.Y * 0.5f) * Math.Sin(offset3D.X * 0.5f) * Math.Cos(offset3D.Z * 0.5f)),
                                                (float)(Math.Sin(offset3D.Y * 0.5f) * Math.Cos(offset3D.X * 0.5f) * Math.Cos(offset3D.Z * 0.5f) - Math.Cos(offset3D.Y * 0.5f) * Math.Sin(offset3D.X * 0.5f) * Math.Sin(offset3D.Z * 0.5f)),
                                                (float)(Math.Cos(offset3D.Y * 0.5f) * Math.Cos(offset3D.X * 0.5f) * Math.Cos(offset3D.Z * 0.5f) + Math.Sin(offset3D.Y * 0.5f) * Math.Sin(offset3D.X * 0.5f) * Math.Sin(offset3D.Z * 0.5f)));
                am = Vector3D.Transform(am, offset);
            }

            Echo("Target: " + TargetPos);

            double Yaw; double Pitch;
            GyroTurn6(am, 18, 0.3, Remote, Gyros[0], YawPrev, PitchPrev, out Pitch, out Yaw);

            YawPrev = Yaw;
            PitchPrev = Pitch;
            MissilePosPev = MissilePosition;
            TargetPosPev = TargetPos;
        }
        public static double Vector_Projection_Scalar(Vector3D IN, Vector3D Axis_norm)
        {
            double OUT = 0;
            OUT = Vector3D.Dot(IN, Axis_norm);
            if (OUT == double.NaN)
            { OUT = 0; }
            return OUT;
        }
        void GyroTurn6(Vector3D TARGETVECTOR, double GAIN, double DAMPINGGAIN, IMyTerminalBlock REF, IMyGyro GYRO, double YawPrev, double PitchPrev, out double NewPitch, out double NewYaw)
        {
            //Pre Setting Factors
            NewYaw = 0;
            NewPitch = 0;

            //Retrieving Forwards And Up
            Vector3D ShipUp = REF.WorldMatrix.Up;
            Vector3D ShipForward = REF.WorldMatrix.Backward; //Backward for thrusters

            //Create And Use Inverse Quatinion                   
            Quaternion Quat_Two = Quaternion.CreateFromForwardUp(ShipForward, ShipUp);
            var InvQuat = Quaternion.Inverse(Quat_Two);

            Vector3D DirectionVector = TARGETVECTOR; //RealWorld Target Vector
            Vector3D RCReferenceFrameVector = Vector3D.Transform(DirectionVector, InvQuat); //Target Vector In Terms Of RC Block

            //Convert To Local Azimuth And Elevation
            double ShipForwardAzimuth = 0; double ShipForwardElevation = 0;
            Vector3D.GetAzimuthAndElevation(RCReferenceFrameVector, out ShipForwardAzimuth, out ShipForwardElevation);

            //Post Setting Factors
            NewYaw = ShipForwardAzimuth;
            NewPitch = ShipForwardElevation;

            //Applies Some PID Damping
            ShipForwardAzimuth = ShipForwardAzimuth + DAMPINGGAIN * ((ShipForwardAzimuth - YawPrev) * 60 );
            ShipForwardElevation = ShipForwardElevation + DAMPINGGAIN * ((ShipForwardElevation - PitchPrev) * 60 );

            //Does Some Rotations To Provide For any Gyro-Orientation
            var REF_Matrix = MatrixD.CreateWorld(REF.GetPosition(), (Vector3)ShipForward, (Vector3)ShipUp).GetOrientation();
            var Vector = Vector3.Transform((new Vector3D(ShipForwardElevation, ShipForwardAzimuth, 0)), REF_Matrix); //Converts To World
            var TRANS_VECT = Vector3.Transform(Vector, Matrix.Transpose(GYRO.WorldMatrix.GetOrientation()));  //Converts To Gyro Local

            //Logic Checks for NaN's
            if (double.IsNaN(TRANS_VECT.X) || double.IsNaN(TRANS_VECT.Y) || double.IsNaN(TRANS_VECT.Z))
            { return; }

            //Applies To Scenario
            GYRO.Pitch = -(float)MathHelper.Clamp((-TRANS_VECT.X) * GAIN, -1000, 1000);
            GYRO.Yaw = -(float)MathHelper.Clamp(((-TRANS_VECT.Y)) * GAIN, -1000, 1000);
            GYRO.Roll = -(float)MathHelper.Clamp(((-TRANS_VECT.Z)) * GAIN, -1000, 1000);
            GYRO.GyroOverride = true;
        }
        void Launch()
        {
            foreach (var item in Merge)
            {
                item.Enabled = false;
            }
            launched = true;
            foreach (var item in WarHeads)
            {
                item.IsArmed = true;
                item.DetonationTime = 50;
                item.StartCountdown();
            }
            foreach (var item in Thrusts)
            {
                item.ThrustOverridePercentage = 1f;
            }
            TimeStamp = 0;
            GuideCount = rnd.Next(0, 10) * 10 + 100;
            if (spark != null)
                spark.Trigger();
        }
        void ExecuteCmd(string msg)
        {
            if (unicastListener.HasPendingMessage)
            {
                //if (TimeStamp < GuideCount) return;
                MyIGCMessage message = unicastListener.AcceptMessage();
                if (message.Tag.Contains("MissilesChannel"))
                {
                    string[] data = message.Data.ToString().Split('|');
                    if (data[0] == "Missile")
                    {
                        switch (data[1])
                        {
                            case "SynchTargetInfo":
                                {
                                    Vector3D pos = new Vector3D();
                                    Vector3D.TryParse(data[2], out pos);
                                    if (pos != null)
                                        TargetPos = pos;
                                    for (int i = 0; i < 3; i++)
                                    {
                                        int index = rnd.Next(0, 10);
                                        IGC.SendBroadcastMessage("MissilesChannel" + index.ToString(), "Missile|ConfirmStatus|" + Me.WorldVolume.Center.ToString(), TransmissionDistance.TransmissionDistanceMax);
                                    }
                                    break;
                                }
                            case "SelfDestruct":
                                {
                                    foreach (var item in WarHeads)
                                    {
                                        item.Detonate();
                                    }
                                    break;
                                }
                            case "StatusIdle":
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        int index = rnd.Next(0, 10);
                                        IGC.SendBroadcastMessage("MissilesChannel" + index.ToString(), "Missile|ConfirmStatus|" + Me.WorldVolume.Center.ToString(), TransmissionDistance.TransmissionDistanceMax);
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            string cmd = string.IsNullOrEmpty(msg) ? Me.CustomData : msg;
            string[] cmds = cmd.Split('|');
            if (cmds[0] != "Missile") return;
            if (launched) return;
            switch(cmds[1])
            {
                case "Launch":
                    {
                        string[] tar = cmds[2].Split(',');
                        Vector3D.TryParse(tar[0], out TargetPos);
                        Launch();
                        break;
                    }
            }
            if(string.IsNullOrEmpty(msg))
                Me.CustomData = "";           
        }
    }
}