using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRageMath;

namespace SEScript
{
    class LTC_PlanetArtillery:API
    {
        //Physical variables
        static float shellVelocity = 0;
        Vector3D curGravity
        {
            get
            {
                return artilleryComputer.GetNaturalGravity();
            }
        }
        Vector3D targetPos;
        Vector3D ComputerPos;
        //Controls
        IMyRemoteControl artilleryComputer;
        IMyMotorStator verticalRot;
        IMyMotorStator verticalRotRev;
        IMyMotorStator horizontalRot;
        IMyTimerBlock Trigger;
        float calculateAngle(Vector3D receivedTarget)
        {
            float angle = 0;
            return angle;
        }
        void Main(string arg,UpdateType updateSource)
        {
            if(updateSource == UpdateType.IGC)
            {
                
            }
        }
        void CheckComponents()
        {

        }
    }
}
