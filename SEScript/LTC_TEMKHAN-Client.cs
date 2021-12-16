using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace SEScript
{
    class LTC_TEMKHAN_Client:API
    {
        List<TargetStandard> NetworkTargets;
        List<MyDetectedEntityInfo> DetectedTargets;
        List<IMyLargeTurretBase> AutoWeapons;
        List<IMyAirtightSlideDoor> MissileHatch;
        IMyShipController LTCShipControl;

        bool CheckedReady = false;
        ShipStatus MyStatus = ShipStatus.Idle;
        int MissileLaunchCounter = 0;
        int MissileLaunchFrequency = 240;
        IMyUnicastListener unicastListener;
        List<IMyBroadcastListener> channelListeners;

        enum ShipStatus
        {
            Idle,
            Patrolling,
            Combating,
        }
        /// <summary>
        /// 获取或设置导弹舱门开关状态
        /// </summary>
        float MissileHatchOpen
        {
            get
            {
                if(MissileHatch.Count>0)
                {
                    return MissileHatch[0].OpenRatio;
                }else
                    return 0;
            }
            set
            {
                if (value == 1f)
                {
                    foreach (var hatch in MissileHatch)
                    {
                        hatch.OpenDoor();
                    }
                }
                else
                {
                    foreach (var hatch in MissileHatch)
                    {
                        hatch.CloseDoor();
                    }
                }
            }
        }

        Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        void Main(string arg,UpdateType updateType)
        {
            if(!CheckedReady)
            {
                CheckComponents();
                return;
            }
            GetTargets();
            CheckStatus();
            ProcessBroadcastMessage();
            switch(MyStatus)
            {
                case ShipStatus.Idle:
                    {
                        break;
                    }
                case ShipStatus.Combating:
                    {
                        Combat();
                        break;
                    }
                case ShipStatus.Patrolling:
                    {
                        break;
                    }
            }
        }
        void CheckComponents()
        {
            InitSystem();
        }
        void InitSystem()
        {
            NetworkTargets = new List<TargetStandard>();
            DetectedTargets = new List<MyDetectedEntityInfo>();
            AutoWeapons = new List<IMyLargeTurretBase>();
            MissileHatch = new List<IMyAirtightSlideDoor>();
            unicastListener = IGC.UnicastListener;
            channelListeners = new List<IMyBroadcastListener>();

            CheckedReady = true;
        }
        void CheckStatus()
        {
            if(NetworkTargets.Count + DetectedTargets.Count > 0)
            {
                if(MyStatus != ShipStatus.Combating)
                {
                    MissileHatchOpen = 1;
                }
                MyStatus = ShipStatus.Combating;
            }else
            {
                if(MyStatus != ShipStatus.Idle)
                {
                    MissileHatchOpen = 0;
                }
                MyStatus = ShipStatus.Idle;
            }
            MissileLaunchCounter = MissileLaunchCounter < MissileLaunchFrequency ? MissileLaunchCounter += 1 : MissileLaunchFrequency;
        }
        void ProcessBroadcastMessage()
        {
            for(int i = 0;i<channelListeners.Count;i++)
            {

            }
        }
        void GetTargets()
        {
            if(AutoWeapons.Count > 0)
            {
                foreach (var item in AutoWeapons)
                {

                }
            }
        }
        void Combat()
        {
            LaunchMissiles();
            FocusLaserBeam();
        }
        void LaunchMissiles()
        {
            if (MissileHatchOpen != 1f) return;
            if (MissileLaunchCounter != MissileLaunchFrequency) return;

            //结构内广播
            IGC.SendBroadcastMessage("MissilesChannel", "Missile|ConfirmLaunch", TransmissionDistance.ConnectedConstructs);

            MissileLaunchCounter = 0;
        }
        void FocusLaserBeam()
        {
            //TODO：聚焦激光武器
        }
        class TargetStandard
        {
            public long TargetID;
            public Vector3D TargetPos;
            public Vector3D TargetVel;
        }
    }
}
