using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace SEScript
{
    class LTC_TEMKHAN_Server : API
    {
        /*T.E.M.K.H.A.N.
         * Tactical Extended Multi-Key Harmonized Antenna Network
         * 预期实现的功能：
         * ------------第一阶段---------------
         * 1、侦查信息组网：无论用什么方式侦查到的敌方信息均可传递至主机
         * 2、信息同步：主机依据各武器系统锁定的目标同步相关信息，若武器系统当前空闲则分发目标
         * 3、筛查无效目标：一段时间后若主机未能接收到某目标的相关信息，则将其移除目标列表
         * 4、确认导弹状况：定期向导弹发送确认消息，若长时间未回应则将其移除活跃导弹列表
         * ------------第二阶段---------------
         * 5、自动指定侦察机目标位置并发送至侦察机
         * 6、若武器系统未能检测到主机，则自动组建临时网络直至检测到主机为止
         * 7、依据战场形势调整侦察机、战舰的位置和姿态
         */
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
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }
        void Main()
        {
            if (!CheckReady)
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
            for (int i = 0; i < missiles.Count; i++)
            {
                missiles[i].TatgetScanLife -= 1;
                if (missiles[i].TatgetScanLife == 0)
                {
                    missiles.RemoveAt(i);
                    i -= 1;
                    continue;
                }
                IGC.SendUnicastMessage(missiles[i].TargetID, "MissilesChannel", "Missile|Check_Status|");
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
        class MissileStandard : TargetStandard
        {
            public long LockedTarget;
        }

        #region
        //--------------------------------------------------------------------------------------------------------------------------

        //bool CheckComponents = false;
        //IMyTextSurface textDisplay;
        //List<string> txtDisplay = new List<string>();
        //List<IMyBroadcastListener> listeners = new List<IMyBroadcastListener>();
        //IMyUnicastListener uniCaster;

        //Program()
        //{
        //    Runtime.UpdateFrequency = UpdateFrequency.Update1;
        //}

        //void Main(string arg, UpdateType updateType)
        //{
        //    if(!CheckComponents)
        //    {
        //        CheckComponent();
        //        return;
        //    }
        //    foreach (var channel in listeners)
        //    {
        //        if(channel.HasPendingMessage)
        //        {
        //            MyIGCMessage message = channel.AcceptMessage();
        //            DisplayInfo(message.Source + ":" + message.Tag + "/" + message.Data.ToString());
        //        }
        //    }
        //    IGC.SendBroadcastMessage("Channel1", "Hello!",TransmissionDistance.TransmissionDistanceMax);
        //    IGC.SendBroadcastMessage("Channel2", "World!", TransmissionDistance.TransmissionDistanceMax);
        //}
        //void CheckComponent()
        //{
        //    textDisplay = (IMyTextSurface)GridTerminalSystem.GetBlockWithName("Display");
        //    if(txtDisplay == null)
        //    {
        //        Echo("LCD not found");
        //        return;
        //    }
        //    CheckComponents = true;
        //    IMyBroadcastListener Channel1 = IGC.RegisterBroadcastListener("Channel1");
        //    Echo("Registered Channel 1");
        //    IMyBroadcastListener Channel2 = IGC.RegisterBroadcastListener("Channel2");
        //    Echo("Registered Channel 2");
        //    listeners.Add(Channel1);
        //    listeners.Add(Channel2);
        //    uniCaster = IGC.UnicastListener;
        //}
        //void DisplayInfo(string data)
        //{
        //    if(txtDisplay.Count>10)
        //    {
        //        txtDisplay.RemoveAt(0);
        //    }
        //    txtDisplay.Add(data);

        //    string info = "";
        //    foreach (var item in txtDisplay)
        //    {
        //        info += item + "\r\n";
        //    }
        //    textDisplay.WriteText(info);
        //}
        #endregion
    }
}
