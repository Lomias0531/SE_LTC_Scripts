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
        int asyncTime = 0;
        int asyncTargetTime = 0;
        IMyShipController controller;
        Random rnd;
        List<IMyBroadcastListener> channels;
        IMyUnicastListener unicastChannel;
        Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        void Main()
        {
            if(!CheckReady)
            {
                CheckComponents();
                return;
            }
            selfCheckTime += 1;
            if(selfCheckTime > 12)
            {
                CheckComponents();
                selfCheckTime = 0;
            }
            asyncTime += 1;
            ProcessMessages();
            if (asyncTime > 6)
            {
                if(ServerAddress != 0)
                {
                    AsyncInfoToServer();
                }else
                {
                    IGC.SendBroadcastMessage("FriendlyScoutChannel0", "FriendlyScout|RegisterScout", TransmissionDistance.TransmissionDistanceMax);
                }
                asyncTime = 0;
            }
            AsyncHostileInfoToServer();
            Echo(ServerAddress.ToString());
            Echo("Async time: " + asyncTime.ToString());
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
            InitSystem();
        }
        void InitSystem()
        {
            IGC.SendBroadcastMessage("FriendlyScoutChannel", "FriendlyScout|RegisterScout", TransmissionDistance.TransmissionDistanceMax);
            detectedTargets = new List<MyDetectedEntityInfo>();
            rnd = new Random((int)Me.EntityId);
            channels = new List<IMyBroadcastListener>();
            for(int i = 0;i<5;i++)
            {
                IMyBroadcastListener channel = IGC.RegisterBroadcastListener("FriendlyScoutChannel" + i.ToString());
                channels.Add(channel);
            }
            unicastChannel = IGC.UnicastListener;
            CheckReady = true;
        }
        void AsyncInfoToServer()
        {
           
            for(int i = 0;i<3;i++)
            {
                int index = rnd.Next(0, 5);
                IGC.SendBroadcastMessage("FriendlyScoutChannel" + index.ToString(), "FriendlyScout|AsyncSelfInfo|" + Me.CubeGrid.GetPosition().ToString("F3") + "|" + controller.GetShipVelocities().LinearVelocity.ToString("F3"),TransmissionDistance.TransmissionDistanceMax);
            }
        }
        void AsyncHostileInfoToServer()
        {
            detectedTargets.Clear();
            for (int i = 0; i < autoWeapons.Count; i++)
            {
                if (autoWeapons[i].HasTarget)
                {
                    detectedTargets.Add(autoWeapons[i].GetTargetedEntity());
                }
            }
            if(detectedTargets.Count > 0)
            {
                asyncTargetTime += 1;
                if(asyncTargetTime >= 6)
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
                        IGC.SendBroadcastMessage("HostileInfoChannel" + index.ToString(), "HostileTarget|UpdateTargetInfo|" + targetInfo, TransmissionDistance.TransmissionDistanceMax);
                    }
                    asyncTargetTime = 0;
                }
            }
            else
            {
                asyncTargetTime = 6;
            }
        }
        void ProcessMessages()
        {
            for(int i = 0;i<channels.Count;i++)
            {

            }
            if(unicastChannel.HasPendingMessage)
            {
                MyIGCMessage msg = unicastChannel.AcceptMessage();
                if(msg.Data.ToString() == "ConfirmedRegister")
                {
                    ServerAddress = msg.Source;
                }
            }
        }
    }
}
