using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace SEScript
{
    class LTC_TEMKHAN_Client:API
    {
        Dictionary<long,TargetStandard> DetectedTargets;
        List<IMyLargeTurretBase> AutoWeapons;
        List<IMyAirtightSlideDoor> MissileHatch;
        IMyShipController LTCShipControl;

        bool CheckedReady = false;
        ShipStatus MyStatus = ShipStatus.Idle;
        int MissileLaunchCounter = 0;
        int MissileLaunchFrequency = 240;
        IMyUnicastListener unicastListener;
        List<IMyBroadcastListener> channelListeners;
        int SynchTime = 0;
        int SynchTargetTime = 0;
        Random rnd;
        long ServerAddress = 0;

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
            ProcessBroadcastMessage();
            CheckStatus();
            if (SynchTime > 60)
            {
                if (ServerAddress != 0)
                {
                    SynchInfoToServer();
                }
                else
                {
                    IGC.SendBroadcastMessage("FriendlyShipChannel0", "FriendlyShip|RegisterShip", TransmissionDistance.TransmissionDistanceMax);
                }
                SynchTime = 0;
            }
            switch (MyStatus)
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
            DetectedTargets = new Dictionary<long, TargetStandard>();
            AutoWeapons = new List<IMyLargeTurretBase>();
            MissileHatch = new List<IMyAirtightSlideDoor>();
            unicastListener = IGC.UnicastListener;
            channelListeners = new List<IMyBroadcastListener>();
            IMyBroadcastListener ServerChannel = IGC.RegisterBroadcastListener("FriendlyShipChannel");
            channelListeners.Add(ServerChannel);
            IMyBroadcastListener turretChannel = IGC.RegisterBroadcastListener("ShipArtilleryChannel");
            channelListeners.Add(turretChannel);
            IMyBroadcastListener PointDefenseChannel = IGC.RegisterBroadcastListener("ShipPDChannel");
            channelListeners.Add(PointDefenseChannel);
            rnd = new Random((int)Me.EntityId);

            CheckedReady = true;
        }
        void CheckStatus()
        {
            if(DetectedTargets.Count > 0)
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
                if(channelListeners[i].HasPendingMessage)
                {
                    MyIGCMessage message = channelListeners[i].AcceptMessage();
                    string[] data = message.Data.ToString().Split('|');
                    switch(message.Tag)
                    {
                        case "FriendlyShipChannel":
                            {
                                if(data[0] == "FriendlyShip")
                                {
                                    switch(data[1])
                                    {
                                        case "SynchInfo":
                                            {
                                                string[] targets = data[2].Split(',');
                                                for (int t = 0; t < targets.Length; t++)
                                                {
                                                    if (string.IsNullOrEmpty(targets[t]))
                                                    {
                                                        break;
                                                    }
                                                    string[] info = targets[t].Split('/');
                                                    long targetID = long.Parse(info[0]);
                                                    if (!DetectedTargets.ContainsKey(targetID))
                                                    {
                                                        TargetStandard target = new TargetStandard();
                                                        target.TargetID = targetID;
                                                        DetectedTargets.Add(targetID, target);
                                                    }
                                                    Vector3D.TryParse(info[1], out DetectedTargets[targetID].TargetPos);
                                                    Vector3D.TryParse(info[2], out DetectedTargets[targetID].TargetVel);
                                                }
                                                break;
                                            }
                                    }
                                }
                                break;
                            }
                        case "ShipArtilleryChannel":
                            {
                                break;
                            }
                        case "ShipPDChannel":
                            {
                                break;
                            }
                    }
                }
            }
            if(unicastListener.HasPendingMessage)
            {
                MyIGCMessage message = unicastListener.AcceptMessage();
                switch(message.Tag)
                {

                }
            }
        }
        void GetTargets()
        {
            DetectedTargets.Clear();
            if(AutoWeapons.Count > 0)
            {
                foreach (var item in AutoWeapons)
                {
                    MyDetectedEntityInfo info = item.GetTargetedEntity();
                    if(!DetectedTargets.ContainsKey(info.EntityId))
                    {
                        TargetStandard target = new TargetStandard()
                        {
                            TargetID = info.EntityId,
                            TargetVel = (Vector3D)info.Velocity,
                            TargetPos = info.BoundingBox.Center
                        };
                        DetectedTargets.Add(info.EntityId, target);
                    }
                }
            }

            //向服务器发送自己探测到的目标
            if (DetectedTargets.Count > 0)
            {
                SynchTargetTime += 1;
                if (SynchTargetTime >= 60)
                {
                    string targetInfo = "";
                    foreach (var item in DetectedTargets)
                    {
                        targetInfo += item.Value.TargetID + "/" + item.Value.TargetPos.ToString("F3") + "/" + item.Value.TargetVel.ToString("F3") + ",";
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        int index = rnd.Next(0, 10);
                        IGC.SendBroadcastMessage("HostileInfoChannel" + index.ToString(), "HostileTarget|UpdateTargetInfo|" + targetInfo, TransmissionDistance.TransmissionDistanceMax);
                    }
                    SynchTargetTime = 0;
                }
            }
        }
        void SynchInfoToServer()
        {

            for (int i = 0; i < 3; i++)
            {
                int index = rnd.Next(0, 5);
                IGC.SendBroadcastMessage("FriendlyShipChannel" + index.ToString(), "FriendlyShip|SynchSelfInfo|" + Me.CubeGrid.GetPosition().ToString("F3") + "|" + LTCShipControl.GetShipVelocities().LinearVelocity.ToString("F3"), TransmissionDistance.TransmissionDistanceMax);
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
