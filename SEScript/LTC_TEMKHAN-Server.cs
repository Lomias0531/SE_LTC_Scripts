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
        List<long> activeFriendlyIndex;
        Dictionary<long, TargetStandard> activeTargets;
        Dictionary<long, MissileStandard> activeMissiles;
        Dictionary<long, TargetStandard> activeFriendly;
        bool CheckReady = false;
        IMyUnicastListener listener;
        List<IMyBroadcastListener> channelListeners;
        Random rnd;
        List<string> DisplayMessage;
        IMyTextSurface MessageBoard;
        IMyTextPanel Radar;
        CurrentStatus curStatus = CurrentStatus.Offline;
        List<long> itemRemoval;
        CommandProcessor processor;
        int CheckCount = 0;
        enum TargetType
        {
            HostileObject,
            FriendlyShip,
            LaunchedMissile,
            FriendlyScout,

        }
        enum CurrentStatus
        {
            Online,
            Offline,
        }
        Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }
        void Main(string arg,UpdateType updateType)
        {
            if (!CheckReady)
            {
                CheckComponents();
                return;
            }
            CheckCount += 1;
            if(CheckCount>=10)
            {
                processor.QueueAction(CheckMissileStatus, activeMissilesIndex.Count);
                //CheckMissileStatus();
                processor.QueueAction(CheckHostileStatus, activeTargetsIndex.Count);
                //CheckHostileStatus();
                //CheckFriendlyStatus();
                processor.QueueAction(CheckFriendlyStatus, activeFriendlyIndex.Count);
                CheckCount = 0;
            }
            processor.QueueAction(ProcessBroadcastInfo, channelListeners.Count);
            //ProcessBroadcastInfo();
            processor.QueueAction(AsyncInfo, activeFriendlyIndex.Count + activeFriendlyIndex.Count);
            //AsyncInfo();
            processor.QueueAction(RemoveItems, itemRemoval.Count);
            processor.QueueAction(DisplayMessages, DisplayMessage.Count);
            processor.Update();
            //DisplayMessages();
        }
        void CheckComponents()
        {
            List<IMyRadioAntenna> antenna = new List<IMyRadioAntenna>();
            GridTerminalSystem.GetBlocksOfType(antenna);
            if(antenna.Count>0)
            {
                curStatus = CurrentStatus.Online;
            }
            MessageBoard = ((IMyProgrammableBlock)Me).GetSurface(0);
            Radar = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("LTC_Radar");
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
            activeFriendlyIndex = new List<long>();
            activeFriendly = new Dictionary<long, TargetStandard>();
            itemRemoval = new List<long>();
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
            for(int i = 0;i<5;i++)
            {
                IMyBroadcastListener channel = IGC.RegisterBroadcastListener("FriendlyScoutChannel" + i.ToString());
                channelListeners.Add(channel);
            }
            for (int i = 0; i < 5; i++)
            {
                IMyBroadcastListener channel = IGC.RegisterBroadcastListener("FriendlyShipChannel" + i.ToString());
                channelListeners.Add(channel);
            }
            rnd = new Random((int)Me.EntityId);
            DisplayMessage = new List<string>();
            itemRemoval = new List<long>();
            ShowMessage("System online");
            processor = new CommandProcessor();
            processor.InitProcessor();
            CheckReady = true;
        }
        /// <summary>
        /// 检查所有导弹状态
        /// </summary>
        void CheckMissileStatus()
        {
            itemRemoval.Clear();
            for (int i = 0; i < activeMissilesIndex.Count; i++)
            {
                activeMissiles[activeMissilesIndex[i]].TargetScanLife -= 1;

                if (activeMissiles[activeMissilesIndex[i]].TargetScanLife <= 0)
                {
                    itemRemoval.Add(activeMissilesIndex[i]);
                    if (i > 0)
                        i -= 1;
                    else
                        i = 0;
                        continue;
                }
            }
        }
        /// <summary>
        /// 检查所有敌方目标状态
        /// </summary>
        void CheckHostileStatus()
        {
            itemRemoval.Clear();
            for (int i = 0; i <activeTargetsIndex.Count; i++)
            {
                activeTargets[activeTargetsIndex[i]].TargetScanLife -= 1;
                if (activeTargets[activeTargetsIndex[i]].TargetScanLife <= 0)
                {
                    itemRemoval.Add(activeTargetsIndex[i]);
                    i += 1;
                }
            }
        }
        void CheckFriendlyStatus()
        {
            itemRemoval.Clear();
            for(int i = 0;i<activeFriendlyIndex.Count;i++)
            {
                activeFriendly[activeFriendlyIndex[i]].AsyncTime -= 1;
                if(activeFriendly[activeFriendlyIndex[i]].AsyncTime <= 0)
                {
                    itemRemoval.Add(activeFriendlyIndex[i]);
                    if (i > 0)
                        i--;
                    else
                        i = 0;
                        continue;
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
                    Echo(message.Data.ToString());
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
                                            activeMissiles[message.Source].TargetScanLife = 12;
                                            ShowMessage("导弹" + message.Source.ToString() + "确认状态");
                                        }
                                        break;
                                    }
                                //case "FinalFarewell"://导弹发送引爆消息，从导弹列表中移除
                                //    {
                                //        if(activeMissiles.ContainsKey(message.Source))
                                //        {
                                //            activeMissiles.Remove(message.Source);
                                //        }
                                //        if(activeMissilesIndex.Contains(message.Source))
                                //        {
                                //            activeMissilesIndex.Remove(message.Source);
                                //        }
                                //        ShowMessage("导弹" + message.Source.ToString() + "被引爆");
                                //        break;
                                //    }
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
                                        missile.TargetScanLife = 12;
                                        missile.AsyncTime = 3;
                                        missile.type = TargetType.LaunchedMissile;
                                        int targetIndex = rnd.Next(0, activeTargetsIndex.Count);
                                        missile.LockedTarget = activeTargetsIndex[targetIndex];
                                        activeMissiles.Add(message.Source, missile);
                                        activeMissilesIndex.Add(message.Source);
                                        ShowMessage("导弹" + message.Source.ToString() + "已升空");
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
                                case "UpdateTargetInfo"://接收来自友方舰船发来的敌方消息
                                    {
                                        string[] targets = data[2].Split(',');
                                        for(int t = 0;t<targets.Length;t++)
                                        {
                                            if(string.IsNullOrEmpty(targets[t]))
                                            {
                                                break;
                                            }    
                                            string[] info = targets[t].Split('/');
                                            long targetID = long.Parse(info[0]);
                                            if (!activeTargetsIndex.Contains(targetID))
                                            {
                                                activeTargetsIndex.Add(targetID);
                                            }
                                            if (!activeTargets.ContainsKey(targetID))
                                            {
                                                TargetStandard target = new TargetStandard();
                                                target.type = TargetType.HostileObject;
                                                target.TargetID = targetID;
                                                activeTargets.Add(targetID, target);
                                            }
                                            activeTargets[targetID].TargetScanLife = 12;
                                            Vector3D.TryParse(info[1], out activeTargets[targetID].TargetPos);
                                            Vector3D.TryParse(info[2], out activeTargets[targetID].TargetVel);
                                            //activeTargets[targetID].TargetRot = new QuaternionD(float.Parse(info[3]), float.Parse(info[4]), float.Parse(info[5]), float.Parse(info[6]));
                                        }
                                        ShowMessage("接收到来自" + message.Source.ToString() + "的敌方数据");
                                        break;
                                    }
                            }
                        }
                    }else if(message.Tag.Contains("FriendlyScoutChannel"))
                    {
                        if(data[0] == "FriendlyScout")
                        {
                            switch(data[1])
                            {
                                case "RegisterScout":
                                    {
                                        if (!activeFriendlyIndex.Contains(message.Source))
                                        {
                                            activeFriendlyIndex.Add(message.Source);
                                        }
                                        if (!activeFriendly.ContainsKey(message.Source))
                                        {
                                            TargetStandard target = new TargetStandard();
                                            target.type = TargetType.FriendlyScout;
                                            target.TargetID = message.Source;
                                            target.AsyncTime = 12;
                                            activeFriendly.Add(message.Source, target);
                                        }
                                        IGC.SendUnicastMessage(message.Source, "FriendlyScoutChannel", "ConfirmedRegister");
                                        ShowMessage("友方侦察机" + message.Source.ToString() + "已连接");
                                        break;
                                    }
                                case "AsyncSelfInfo":
                                    {
                                        if(activeFriendly.ContainsKey(message.Source))
                                        {
                                            activeFriendly[message.Source].AsyncTime = 120;
                                        }else
                                        {
                                            if (!activeFriendlyIndex.Contains(message.Source))
                                            {
                                                activeFriendlyIndex.Add(message.Source);
                                            }
                                            if (!activeFriendly.ContainsKey(message.Source))
                                            {
                                                TargetStandard target = new TargetStandard();
                                                target.type = TargetType.FriendlyScout;
                                                target.TargetID = message.Source;
                                                target.AsyncTime = 12;
                                                activeFriendly.Add(message.Source, target);
                                            }
                                            ShowMessage("友方侦察机" + message.Source.ToString() + "已连接");
                                        }
                                        Vector3D.TryParse(data[2], out activeFriendly[message.Source].TargetPos);
                                        Vector3D.TryParse(data[3], out activeFriendly[message.Source].TargetVel);
                                        ShowMessage("友方侦察机" + message.Source.ToString() + "同步单位信息");
                                        break;
                                    }
                            }
                        }
                        return;
                    }else if(message.Tag.Contains("FriendlyShipChannel"))
                    {
                        if (data[0] == "FriendlyShip")
                        {
                            switch (data[1])
                            {
                                case "RegisterShip":
                                    {
                                        if (!activeFriendlyIndex.Contains(message.Source))
                                        {
                                            activeFriendlyIndex.Add(message.Source);
                                        }
                                        if (!activeFriendly.ContainsKey(message.Source))
                                        {
                                            TargetStandard target = new TargetStandard();
                                            target.type = TargetType.FriendlyShip;
                                            target.TargetID = message.Source;
                                            target.AsyncTime = 6;
                                            activeFriendly.Add(message.Source, target);
                                        }
                                        ShowMessage("友方舰船" + message.Source.ToString() + "已连接");
                                        break;
                                    }
                                case "AsyncSelfInfo":
                                    {
                                        if (activeFriendly.ContainsKey(message.Source))
                                        {
                                            Vector3D.TryParse(data[2], out activeFriendly[message.Source].TargetPos);
                                            Vector3D.TryParse(data[3], out activeFriendly[message.Source].TargetVel);
                                            activeFriendly[message.Source].AsyncTime = 60;
                                            ShowMessage("友方舰船" + message.Source.ToString() + "同步单位信息");
                                        }
                                        break;
                                    }
                            }
                        }
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

                if (activeMissiles[activeMissilesIndex[i]].AsyncTime <= 0)
                {
                    ShowMessage("向导弹" + activeMissilesIndex[i].ToString() + "同步消息");
                    string targetInfo;

                    if (activeTargets.Count == 0)
                    {
                        for (int d = 0; d < 3; d++)
                        {
                            int index = rnd.Next(0, 10);
                            IGC.SendUnicastMessage(activeMissilesIndex[i], "MissilesChannel" + index.ToString(), "Missile|StatusIdle");
                        }
                    }else
                    {
                        if (!activeTargets.ContainsKey(activeMissiles[activeMissilesIndex[i]].LockedTarget))
                        {
                            int targetIndex = rnd.Next(0, activeTargetsIndex.Count);
                            activeMissiles[activeMissilesIndex[i]].LockedTarget = activeTargetsIndex[targetIndex];
                        }

                        targetInfo = activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetPos.ToString() + "|" + activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetVel.ToString();
                        for (int d = 0; d < 3; d++)
                        {
                            int index = rnd.Next(0, 10);
                            IGC.SendUnicastMessage(activeMissilesIndex[i], "MissilesChannel" + index.ToString(), "Missile|AsyncTargetInfo|" + targetInfo);
                        }
                    }
                    activeMissiles[activeMissilesIndex[i]].AsyncTime = 3;
                }
            }
        }
        void ShowMessage(string msg)
        {
            if(MessageBoard != null)
            {
                if (DisplayMessage.Count >= 10)
                {
                    DisplayMessage.RemoveAt(0);
                }
                DisplayMessage.Add(msg);                
            }
        }
        void DisplayMessages()
        {
            string info = "T.E.M.K.H.A.N.    " + curStatus.ToString().ToUpper() + "\r\n=====================\r\n";
            info += "已激活导弹：" + activeMissilesIndex.Count.ToString() + "\r\n";
            info += "已联网的友方单位：" + activeFriendlyIndex.Count.ToString() + "\r\n";
            info += "已发现的敌方目标：" + activeTargetsIndex.Count.ToString() + "\r\n";
            info += "=====================\r\n";
            for (int i = 0; i < DisplayMessage.Count; i++)
            {
                info += DisplayMessage[i] + "\r\n";
            }
            MessageBoard.WriteText(info);
        }
        void RemoveItems()
        {
            for(int i = 0;i<itemRemoval.Count;i++)
            {
                if(activeFriendlyIndex.Contains(itemRemoval[i]))
                {
                    activeFriendlyIndex.Remove(itemRemoval[i]);
                    activeFriendly.Remove(itemRemoval[i]);
                    ShowMessage("己方单位" + itemRemoval[i].ToString() + "已失联");
                    continue;
                }
                if (activeMissilesIndex.Contains(itemRemoval[i]))
                {
                    activeMissilesIndex.Remove(itemRemoval[i]);
                    activeMissiles.Remove(itemRemoval[i]);
                    ShowMessage("导弹" + itemRemoval[i].ToString() + "已销毁");
                    continue;
                }
                if (activeTargetsIndex.Contains(itemRemoval[i]))
                {
                    activeTargetsIndex.Remove(itemRemoval[i]);
                    activeTargets.Remove(itemRemoval[i]);
                    ShowMessage("敌方目标" + itemRemoval[i].ToString() + "已摧毁");
                    continue;
                }
            }
            itemRemoval.Clear();
        }
        #region drawRadarMap
        void DrawRadarMap()
        {

        }
        #endregion
        class TargetStandard
        {
            public long TargetID;
            public int TargetScanLife;
            public Vector3D TargetPos;
            public Vector3D TargetVel;
            public TargetType type;
            public int AsyncTime;
            //public QuaternionD TargetRot;
        }
        class MissileStandard : TargetStandard
        {
            public long LockedTarget;
        }
        class CommandProcessor
        {
            List<LTCCommand> queuedActions;
            List<LTCCommand> actionDispose;
            int processedTime = 0;
            public void InitProcessor()
            {
                queuedActions = new List<LTCCommand>();
                actionDispose = new List<LTCCommand>();
            }
            public void Update()
            {
                processedTime = 0;
                foreach (var command in queuedActions)
                {
                    if (processedTime + command.runCheck >= 50)
                    {
                        break;
                    }
                    command.ExeCommand();
                    processedTime += command.runCheck;
                    actionDispose.Add(command);
                }
                queuedActions.RemoveAll((x) => actionDispose.Contains(x));
            }
            public void QueueAction(Action action,int check)
            {
                LTCCommand cmd = new LTCCommand(action,check);
                queuedActions.Add(cmd);
            }
        }
        class LTCCommand
        {
            public Action action;
            public int runCheck = 1;
            public LTCCommand(Action _action,int _runCheck)
            {
                action = _action;
                runCheck = _runCheck;
            }
            public void ExeCommand()
            {
                action.Invoke();
            }
        }    
    }
}
