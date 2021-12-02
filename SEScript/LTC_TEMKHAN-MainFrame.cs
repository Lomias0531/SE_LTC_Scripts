using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace SEScript
{
    class LTC_TEMKHAN_MainFrame:API
    {
        //T.E.M.K.H.A.N.
        //Tactical Extended Multi-Key Harmonized Antenna Network
        List<TargetStandard> targets;
        List<MissileStandard> missiles;
        bool CheckReady = false;
        IMyUnicastListener listener;
        List<IMyBroadcastListener> broadcastListeners;
        enum TargetType
        {
            HostileObject,
            FriendlyShip,
            LaunchedMissile,
            FriendlyScout,
            FriendlyProbe,
        }
        Program()
        {

        }
        void Main()
        {
            if(!CheckReady)
            {
                CheckComponents();
                return;
            }
            CheckMissileStatus();
            CheckHostileStatus();
            ProcessBroadcastInfo();
        }
        void CheckComponents()
        {
            InitSystem();
        }
        void InitSystem()
        {
            targets = new List<TargetStandard>();
            missiles = new List<MissileStandard>();
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            listener = IGC.UnicastListener;
            broadcastListeners = new List<IMyBroadcastListener>();
            IMyBroadcastListener MissileChannelListener = IGC.RegisterBroadcastListener("MissilesChannel");
            broadcastListeners.Add(MissileChannelListener);
        }
        void CheckMissileStatus()
        {
            for(int i = 0;i<missiles.Count;i++)
            {
                missiles[i].TatgetScanLife -= 1;
                if(missiles[i].TatgetScanLife == 0)
                {
                    missiles.RemoveAt(i);
                    i -= 1;
                    continue;
                }
                IGC.SendUnicastMessage(missiles[i].TargetID, "MissilesChannel", "Missile|Check_Status|");
            }
        }
        IEnumerator StartChannelListening(IMyBroadcastListener listener,string Channel)
        {
            while(listener.HasPendingMessage)
            {
                
            }
        }
        void CheckHostileStatus()
        {
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].TatgetScanLife -= 1;
                if (targets[i].TatgetScanLife == 0)
                {
                    targets.RemoveAt(i);
                    i -= 1;
                }
            }
        }
        void ProcessBroadcastInfo()
        {

        }

        class TargetStandard
        {
            public long TargetID;
            public int TatgetScanLife;
            public Vector3D TargetPos;
            public Vector3D TargetVel;
            public QuaternionD TargetRot;
        }
        class MissileStandard:TargetStandard
        {
            public long LockedTarget;
        }
    }
}
