using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace SEScript
{
    class LTC_TEMKHAN_Server : API
    {
        /* T.E.M.K.H.A.N.
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
         * 7、依据战场形势以集群算法调整侦察机、战舰的位置和姿态
         * 8、增强抗干扰能力（跳频等）
         */
        List<long> activeTargetsIndex;
        List<long> activeMissilesIndex;
        Dictionary<long, TargetStandard> activeTargets;
        Dictionary<long, MissileStandard> activeMissiles;
        bool CheckReady = false;
        IMyUnicastListener listener;
        List<IMyBroadcastListener> channelListeners;
        Random rnd;
        enum TargetType
        {
            HostileObject,
            FriendlyShip,
            LaunchedMissile,
            FriendlyScout,

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
            AsyncInfo();
        }
        void CheckComponents()
        {
            InitSystem();
        }
        /// <summary>
        /// 系统初始化
        /// </summary>
        void InitSystem()
        {
            activeTargetsIndex = new List<long>();
            activeTargets = new Dictionary<long, TargetStandard>();
            activeMissilesIndex = new List<long>();
            activeMissiles = new Dictionary<long, MissileStandard>();
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            listener = IGC.UnicastListener;
            channelListeners = new List<IMyBroadcastListener>();
            //为导弹分配10个频道，每次向导弹发送消息以及导弹发送消息均采用随机3个频道以避免信息丢失
            for(int i = 0;i<10;i++)
            {
                IMyBroadcastListener channel = IGC.RegisterBroadcastListener("MissilesChannel" + i.ToString());
                channelListeners.Add(channel);
            }
            //为敌方信息分配10个频道，每次更新敌方信息均采用3个随机频道以避免信息丢失
            for (int i = 0; i < 10; i++)
            {
                IMyBroadcastListener channel = IGC.RegisterBroadcastListener("HostileInfoChannel" + i.ToString());
                channelListeners.Add(channel);
            }
            rnd = new Random((int)Me.EntityId);
        }
        /// <summary>
        /// 检查所有导弹状态
        /// </summary>
        void CheckMissileStatus()
        {
            for (int i = 0; i < activeMissilesIndex.Count; i++)
            {
                activeMissiles[activeMissilesIndex[i]].TargetScanLife -= 1;
                if(activeMissiles[activeMissilesIndex[i]].TargetScanLife == 20)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        int index = rnd.Next(0, 10);
                        IGC.SendUnicastMessage(activeMissiles[activeMissilesIndex[i]].TargetID, "MissilesChannel" + index.ToString(), "Missile|Check_Status|");
                    }
                }
                if (activeMissiles[activeMissilesIndex[i]].TargetScanLife == 0)
                {
                    activeMissiles.Remove(activeMissilesIndex[i]);
                    activeMissilesIndex.RemoveAt(i);
                    i -= 1;
                    continue;
                }
            }
        }
        /// <summary>
        /// 检查所有敌方目标状态
        /// </summary>
        void CheckHostileStatus()
        {
            for (int i = 0; i < activeTargetsIndex.Count; i++)
            {
                activeTargets[activeTargetsIndex[i]].TargetScanLife -= 1;
                if (activeTargets[activeTargetsIndex[i]].TargetScanLife == 0)
                {
                    activeTargets.Remove(activeTargetsIndex[i]);
                    activeMissilesIndex.RemoveAt(i);
                    i -= 1;
                }
            }
        }
        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        void ProcessBroadcastInfo()
        {
            for(int i = 0;i<channelListeners.Count;i++)
            {
                if(channelListeners[i].HasPendingMessage)
                {
                    MyIGCMessage message = channelListeners[i].AcceptMessage();
                    string[] data = message.Data.ToString().Split('|');
                    if (message.Tag.Contains("MissilesChannel"))
                    {
                        if(data[0] == "Missile")
                        {
                            switch(data[1])
                            {
                                default:
                                    {
                                        break;
                                    }
                                case "ConfirmStatus"://导弹回复状态确认消息，刷新导弹确认时间
                                    {
                                        if(activeMissiles.ContainsKey(message.Source))
                                        {
                                            activeMissiles[message.Source].TargetScanLife = 60;
                                        }
                                        break;
                                    }
                                case "FinalFarewell"://导弹发送引爆消息，从导弹列表中移除
                                    {
                                        if(activeMissiles.ContainsKey(message.Source))
                                        {
                                            activeMissiles.Remove(message.Source);
                                        }
                                        if(activeMissilesIndex.Contains(message.Source))
                                        {
                                            activeMissilesIndex.Remove(message.Source);
                                        }
                                        break;
                                    }
                                case "LaunchConfirmed"://导弹发送发射消息，检查列表中是否有相同ID的导弹，并新增项目
                                    {
                                        if (activeMissiles.ContainsKey(message.Source))
                                        {
                                            activeMissiles.Remove(message.Source);
                                        }
                                        if (activeMissilesIndex.Contains(message.Source))
                                        {
                                            activeMissilesIndex.Remove(message.Source);
                                        }
                                        MissileStandard missile = new MissileStandard();
                                        missile.TargetID = message.Source;
                                        missile.TargetScanLife = 60;
                                        missile.AsyncTime = 10;
                                        missile.LockedTarget = long.Parse(data[2]);
                                        activeMissiles.Add(message.Source, missile);
                                        activeMissilesIndex.Add(message.Source);
                                        break;
                                    }
                            }
                        }
                    }else if(message.Tag.Contains("HostileInfoChannel"))
                    {
                        if(data[0] == "HostileTarget")
                        {
                            switch(data[1])
                            {
                                default:
                                    {
                                        break;
                                    }
                                case "UpdateTargetInfo":
                                    {
                                        long targetID = long.Parse(data[2]);
                                        if (!activeTargetsIndex.Contains(targetID))
                                        {
                                            activeTargetsIndex.Add(targetID);
                                        }
                                        if(!activeTargets.ContainsKey(targetID))
                                        {
                                            TargetStandard target = new TargetStandard();
                                            target.TargetID = targetID;
                                            target.TargetScanLife = 40;
                                            Vector3D.TryParse(data[3],out target.TargetPos);
                                            Vector3D.TryParse(data[4], out target.TargetVel);
                                            target.TargetRot = new QuaternionD(float.Parse(data[5]), float.Parse(data[6]), float.Parse(data[7]), float.Parse(data[8]));
                                            activeTargets.Add(targetID, target);
                                        }else
                                        {
                                            activeTargets[message.Source].TargetScanLife = 40;
                                            Vector3D.TryParse(data[3], out activeTargets[message.Source].TargetPos);
                                            Vector3D.TryParse(data[4], out activeTargets[message.Source].TargetVel);
                                            activeTargets[message.Source].TargetRot = new QuaternionD(float.Parse(data[5]), float.Parse(data[6]), float.Parse(data[7]), float.Parse(data[8]));
                                        }
                                        break;
                                    }
                            }
                        }
                    }else if(message.Tag.Contains("FriendlyScoutChannel"))
                    {
                        return;
                    }else if(message.Tag.Contains("FriendlyShipChannel"))
                    {
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 向导弹或友方舰船同步消息
        /// </summary>
        void AsyncInfo()
        {
            for(int i = 0;i<activeMissilesIndex.Count;i++)
            {
                activeMissiles[activeMissilesIndex[i]].AsyncTime -= 1;

                if (activeMissiles[activeMissilesIndex[i]].AsyncTime == 0)
                {
                    string targetInfo;
                    if (activeTargets.ContainsKey(activeMissiles[activeMissilesIndex[i]].LockedTarget))
                    {
                        targetInfo = activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetPos.ToString() + "|" + activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetVel.ToString() + "|";
                        targetInfo += activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetRot.X.ToString() + "|" + activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetRot.Y.ToString() + "|" 
                                    + activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetRot.Z.ToString() + "|" + activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetRot.W.ToString();
                        for (int d = 0; d < 3; d++)
                        {
                            int index = rnd.Next(0, 10);
                            IGC.SendUnicastMessage(activeMissilesIndex[i], "MissilesChannel" + index.ToString(), "Missile|AsyncTargetInfo|" + targetInfo);
                        }
                    }
                    else
                    {
                        for (int d = 0; d < 3; d++)
                        {
                            int index = rnd.Next(0, 10);
                            IGC.SendUnicastMessage(activeMissilesIndex[i], "MissilesChannel" + index.ToString(), "Missile|SelfDestruct|");
                        }
                        activeMissiles.Remove(activeMissilesIndex[i]);
                        activeMissilesIndex.RemoveAt(i);
                        i--;
                    }
                }

                activeMissiles[activeMissilesIndex[i]].AsyncTime = 10;
            }
        }

        class TargetStandard
        {
            public long TargetID;
            public int TargetScanLife;
            public Vector3D TargetPos;
            public Vector3D TargetVel;
            public QuaternionD TargetRot;
        }
        class MissileStandard : TargetStandard
        {
            public long LockedTarget;
            public int AsyncTime;
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
