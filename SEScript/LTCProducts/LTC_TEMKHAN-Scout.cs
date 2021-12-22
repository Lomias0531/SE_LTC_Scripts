using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace SEScript
{
    class LTC_TEMKHAN_Scout:API
    {
        List<IMyLargeTurretBase> autoWeapons;
        List<MyDetectedEntityInfo> detectedTargets;
        bool CheckReady = false;
        long ServerAddress = 0;
        int selfCheckTime = 0;
        int SynchTargetTime = 0;
        IMyShipController controller;
        Random rnd;
        List<IMyBroadcastListener> channels;
        IMyUnicastListener unicastChannel;

        bool enableCamScan = false;
        List<IMyCameraBlock> camLens;
        MyDetectedEntityInfo camLock;
        Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        void Main(string arg)
        {
            if(!CheckReady)
            {
                CheckComponents();
                return;
            }
            selfCheckTime += 1;
            if(selfCheckTime > 120)
            {
                CheckComponents();
                selfCheckTime = 0;
            }
            AcquireCamTarget();
            ProcessMessages(arg);

            Echo(ServerAddress.ToString());
            Echo("Sefl check in: " + selfCheckTime.ToString());
            Echo(detectedTargets.Count.ToString());
        }
        void CheckComponents()
        {
            autoWeapons = new List<IMyLargeTurretBase>();
            GridTerminalSystem.GetBlocksOfType(autoWeapons);
            if(autoWeapons.Count == 0)
            {
                Echo("未检测到炮塔");
                CheckReady = false;
                return;
            }
            List<IMyRadioAntenna> antenna = new List<IMyRadioAntenna>();
            GridTerminalSystem.GetBlocksOfType(antenna);
            if(antenna.Count == 0)
            {
                Echo("未检测到天线");
                CheckReady = false;
                return;
            }
            List<IMyShipController> controllers = new List<IMyShipController>();
            GridTerminalSystem.GetBlocksOfType(controllers);
            if(controllers.Count == 0)
            {
                Echo("未检测到控制器");
                CheckReady = false;
                return;
            }
            controller = controllers[0];
            camLens = new List<IMyCameraBlock>();
            GridTerminalSystem.GetBlocksOfType(camLens);
            if(camLens.Count>0)
            {
                foreach (var cam in camLens)
                {
                    cam.EnableRaycast = true;
                }
            }
            InitSystem();
        }
        void InitSystem()
        {
            IGC.SendBroadcastMessage("FriendlyScoutChannel", "RegisterScout", TransmissionDistance.TransmissionDistanceMax);
            detectedTargets = new List<MyDetectedEntityInfo>();
            rnd = new Random((int)Me.EntityId);
            channels = new List<IMyBroadcastListener>();
            IMyBroadcastListener commonChannel = IGC.RegisterBroadcastListener("LTCCommonChannel");
            commonChannel.SetMessageCallback("LTCCommonChannel");
            channels.Add(commonChannel);
            unicastChannel = IGC.UnicastListener;
            CheckReady = true;
        }
        void AcquireCamTarget()
        {
            if(enableCamScan)
            {
                if (camLens.Count == 0) return;
                for (int i = 0; i < camLens.Count; i++)
                {
                    if (camLens[i].CanScan(4000))
                    {
                        camLock = camLens[i].Raycast(4000);
                        break;
                    }
                }
            }
        }
        void SynchInfoToServer()
        {
           
            for(int i = 0;i<3;i++)
            {
                int index = rnd.Next(0, 5);
                IGC.SendBroadcastMessage("FriendlyScoutChannel" + index.ToString(), "SynchSelfInfo|" + Me.CubeGrid.GetPosition().ToString("F3") + "|" + controller.GetShipVelocities().LinearVelocity.ToString("F3"),TransmissionDistance.TransmissionDistanceMax);
            }
        }
        void SynchHostileInfoToServer()
        {
            detectedTargets.Clear();
            for (int i = 0; i < autoWeapons.Count; i++)
            {
                if (autoWeapons[i].HasTarget)
                {
                    detectedTargets.Add(autoWeapons[i].GetTargetedEntity());
                }
            }
            if(!camLock.IsEmpty())
            {
                detectedTargets.Add(camLock);
            }
            if(detectedTargets.Count > 0)
            {
                SynchTargetTime += 1;
                if(SynchTargetTime >= 6)
                {
                    string targetInfo = "";
                    for (int i = 0; i < detectedTargets.Count; i++)
                    {
                        Vector3D vec = new Vector3D(detectedTargets[i].Velocity.X, detectedTargets[i].Velocity.Y, detectedTargets[i].Velocity.Z);
                        targetInfo += detectedTargets[i].EntityId + "/" + detectedTargets[i].Position.ToString("F3") + "/" + vec.ToString("F3") + ",";
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        int index = rnd.Next(0, 10);
                        IGC.SendBroadcastMessage("HostileInfoChannel" + index.ToString(), "UpdateTargetInfo|" + targetInfo, TransmissionDistance.TransmissionDistanceMax);
                    }
                    SynchTargetTime = 0;
                }
            }
            else
            {
                SynchTargetTime = 6;
            }
        }
        void ProcessMessages(string mainArg)
        {
            for(int i = 0;i<channels.Count;i++)
            {
                if(channels[i].HasPendingMessage)
                {
                    MyIGCMessage msg = channels[i].AcceptMessage();
                    if(msg.Data.ToString() == "RequestReply")
                    {
                        SynchInfoToServer();
                        SynchHostileInfoToServer();
                    }
                }
            }
            if(unicastChannel.HasPendingMessage)
            {
                MyIGCMessage msg = unicastChannel.AcceptMessage();
                if(msg.Data.ToString() == "ConfirmedRegister")
                {
                    ServerAddress = msg.Source;
                }
            }
            switch(mainArg)
            {
                case "TriggerCamScan":
                    {
                        enableCamScan = !enableCamScan;
                        break;
                    }
            }
        }
    }
}
