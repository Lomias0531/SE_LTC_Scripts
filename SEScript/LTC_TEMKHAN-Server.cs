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
        IMyUnicastListener uniListener;
        List<IMyBroadcastListener> channelListeners;
        Random rnd;
        Queue<string> DisplayMessage;
        IMyTextSurface MessageBoard;
        IMyTextPanel Radar;
        CurrentStatus curStatus = CurrentStatus.Offline;
        List<long> itemRemoval;

        SequencedCommand SynchInfoCommands;
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
            double deltaTime = Runtime.TimeSinceLastRun.TotalSeconds;
            SynchInfoCommands.Commence(deltaTime);
            ProcessBroadcastInfo();
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
            uniListener = IGC.UnicastListener;
            uniListener.SetMessageCallback();
            channelListeners = new List<IMyBroadcastListener>();
            //为导弹分配10个频道，每次向导弹发送消息以及导弹发送消息均采用随机3个频道以避免信息丢失
            for(int i = 0;i<10;i++)
            {
                IMyBroadcastListener channel = IGC.RegisterBroadcastListener("MissilesChannel" + i.ToString());
                channel.SetMessageCallback("MissilesChannel" + i.ToString());
                channelListeners.Add(channel);
            }
            //为敌方信息分配10个频道，每次更新敌方信息均采用3个随机频道以避免信息丢失
            for (int i = 0; i < 10; i++)
            {
                IMyBroadcastListener channel = IGC.RegisterBroadcastListener("HostileInfoChannel" + i.ToString());
                channel.SetMessageCallback("HostileInfoChannel" + i.ToString());
                channelListeners.Add(channel);
            }
            for(int i = 0;i<5;i++)
            {
                IMyBroadcastListener channel = IGC.RegisterBroadcastListener("FriendlyScoutChannel" + i.ToString());
                channel.SetMessageCallback("FriendlyScoutChannel" + i.ToString());
                channelListeners.Add(channel);
            }
            for (int i = 0; i < 5; i++)
            {
                IMyBroadcastListener channel = IGC.RegisterBroadcastListener("FriendlyShipChannel" + i.ToString());
                channel.SetMessageCallback("FriendlyShipChannel" + i.ToString());
                channelListeners.Add(channel);
            }
            rnd = new Random((int)Me.EntityId);
            DisplayMessage = new Queue<string>();
            itemRemoval = new List<long>();
            ShowMessage("System online");

            //消息同步命令序列
            SynchInfoCommands = new SequencedCommand();
            SynchInfoCommands.AddCommand(RemoveItems,0.2);//清空字典
            SynchInfoCommands.AddCommand(RequestReply,0.1);//要求回复
            SynchInfoCommands.AddCommand(SynchInfo,0.5);//返回消息
            SynchInfoCommands.AddCommand(DisplayMessages,0.1);//显示信息

            CheckReady = true;
        }
        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        void ProcessBroadcastInfo()
        {
            for(int i = 0;i<channelListeners.Count;i++)
            {
                while(channelListeners[i].HasPendingMessage)
                {
                    MyIGCMessage message = channelListeners[i].AcceptMessage();
                    string[] data = message.Data.ToString().Split('|');
                    if (message.Tag.Contains("MissilesChannel"))
                    {
                        switch (data[0])
                        {
                            default:
                                {
                                    break;
                                }
                            case "ConfirmStatus"://导弹回复状态确认消息，刷新导弹确认时间
                                {
                                    if (activeMissiles.ContainsKey(message.Source))
                                    {
                                        ShowMessage("导弹" + message.Source.ToString() + "确认状态");
                                    }else
                                    {
                                        activeMissilesIndex.Add(message.Source);
                                        MissileStandard missile = new MissileStandard();
                                        activeMissiles.Add(message.Source, missile);
                                    }
                                    Vector3D.TryParse(data[1], out activeMissiles[message.Source].TargetPos);
                                    long.TryParse(data[2], out activeMissiles[message.Source].LockedTarget);
                                    break;
                                }
                            case "LaunchConfirmed"://导弹发送发射消息，检查列表中是否有相同ID的导弹，并新增项目
                                {
                                    MissileStandard missile = new MissileStandard();
                                    missile.TargetID = message.Source;
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
                    else if(message.Tag.Contains("HostileInfoChannel"))
                    {
                        switch (data[0])
                        {
                            default:
                                {
                                    break;
                                }
                            case "UpdateTargetInfo"://接收来自友方舰船发来的敌方消息
                                {
                                    string[] targets = data[1].Split(',');
                                    for (int t = 0; t < targets.Length; t++)
                                    {
                                        if (string.IsNullOrEmpty(targets[t]))
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
                                        Vector3D.TryParse(info[1], out activeTargets[targetID].TargetPos);
                                        Vector3D.TryParse(info[2], out activeTargets[targetID].TargetVel);
                                        //activeTargets[targetID].TargetRot = new QuaternionD(float.Parse(info[3]), float.Parse(info[4]), float.Parse(info[5]), float.Parse(info[6]));
                                    }
                                    ShowMessage("接收到来自" + message.Source.ToString() + "的敌方数据");
                                    break;
                                }
                        }
                    }
                    else if(message.Tag.Contains("FriendlyScoutChannel"))
                    {
                        switch (data[0])
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
                                        activeFriendly.Add(message.Source, target);
                                    }
                                    IGC.SendUnicastMessage(message.Source, "FriendlyScoutChannel", "ConfirmedRegister");
                                    ShowMessage("友方侦察机" + message.Source.ToString() + "已连接");
                                    break;
                                }
                            case "SynchSelfInfo":
                                {
                                    if (!activeFriendly.ContainsKey(message.Source))
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
                                            activeFriendly.Add(message.Source, target);
                                        }
                                    }
                                    Vector3D.TryParse(data[1], out activeFriendly[message.Source].TargetPos);
                                    Vector3D.TryParse(data[2], out activeFriendly[message.Source].TargetVel);
                                    ShowMessage("友方侦察机" + message.Source.ToString() + "同步单位信息");
                                    break;
                                }
                        }
                        return;
                    }else if(message.Tag.Contains("FriendlyShipChannel"))
                    {
                        switch (data[0])
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
                                        activeFriendly.Add(message.Source, target);
                                    }
                                    IGC.SendUnicastMessage(message.Source, "FriendlyShipChannel", "ConfirmedRegister");
                                    ShowMessage("友方舰船" + message.Source.ToString() + "已连接");
                                    break;
                                }
                            case "SynchSelfInfo":
                                {
                                    if (activeFriendly.ContainsKey(message.Source))
                                    {
                                        Vector3D.TryParse(data[1], out activeFriendly[message.Source].TargetPos);
                                        Vector3D.TryParse(data[2], out activeFriendly[message.Source].TargetVel);
                                        ShowMessage("友方舰船" + message.Source.ToString() + "同步单位信息");
                                    }
                                    break;
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
        void SynchInfo()
        {
            //向导弹同步信息
            for(int i = 0;i<activeMissilesIndex.Count;i++)
            {
                ShowMessage("向导弹" + activeMissilesIndex[i].ToString() + "同步消息");
                string MissileTargetInfo;

                if (activeTargets.Count == 0)
                {
                    IGC.SendUnicastMessage(activeMissilesIndex[i], "MissilesChannel", "StatusIdle");
                }
                else
                {
                    if (!activeTargets.ContainsKey(activeMissiles[activeMissilesIndex[i]].LockedTarget))
                    {
                        int targetIndex = rnd.Next(0, activeTargetsIndex.Count);
                        activeMissiles[activeMissilesIndex[i]].LockedTarget = activeTargetsIndex[targetIndex];
                    }

                    MissileTargetInfo = activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetPos.ToString() + "|" + activeTargets[activeMissiles[activeMissilesIndex[i]].LockedTarget].TargetVel.ToString() + "|" + activeMissiles[activeMissilesIndex[i]].LockedTarget.ToString();
                    IGC.SendUnicastMessage(activeMissilesIndex[i], "MissilesChannel", "SynchTargetInfo|" + MissileTargetInfo);
                }
            }
            //向友方舰船同步信息
            ShowMessage("向友方舰船广播信息");
            string targetInfo = "SynchInfo|";
            for(int i = 0;i<activeTargetsIndex.Count;i++)
            {
                targetInfo += activeTargets[activeTargetsIndex[i]].TargetID + "/" + activeTargets[activeTargetsIndex[i]].TargetPos + "/" + activeTargets[activeTargetsIndex[i]].TargetVel + ",";
            }
            IGC.SendBroadcastMessage("FriendlyShipChannel", targetInfo, TransmissionDistance.TransmissionDistanceMax);
        }
        /// <summary>
        /// 向己方发送要求回复消息
        /// </summary>
        void RequestReply()
        {
            IGC.SendBroadcastMessage("LTCCommonChannel", "RequestReply");
        }
        void ShowMessage(string msg)
        {
            if(MessageBoard != null)
            {
                if (DisplayMessage.Count >= 5)
                {
                    DisplayMessage.Dequeue();
                }
                DisplayMessage.Enqueue(msg);                
            }
        }
        void DisplayMessages()
        {
            string info = "T.E.M.K.H.A.N.    " + curStatus.ToString().ToUpper() + "\r\n=====================\r\n";
            info += "已激活导弹：" + activeMissilesIndex.Count.ToString() + "\r\n";
            info += "已联网的友方单位：" + activeFriendlyIndex.Count.ToString() + "\r\n";
            info += "已发现的敌方目标：" + activeTargetsIndex.Count.ToString() + "\r\n";
            info += "=====================\r\n";
            foreach (var item in DisplayMessage)
            {
                info += item + "\r\n";
            }
            MessageBoard.WriteText(info);
        }
        void RemoveItems()
        {
            //activeFriendly.Clear();
            activeFriendlyIndex.Clear();
            //activeMissiles.Clear();
            activeMissilesIndex.Clear();
            //activeTargets.Clear();
            activeTargetsIndex.Clear();
        }
        #region drawRadarMap
        void DrawRadarMap()
        {
            if (Radar == null) return;
        }
        void DrawAxis()
        {

        }
        void DrawTargets()
        {

        }
        void DrawFriendly()
        {

        }
        void DrawMissiles()
        {

        }
        #endregion
        class TargetStandard
        {
            public long TargetID;
            public Vector3D TargetPos;
            public Vector3D TargetVel;
            public TargetType type;
        }
        class MissileStandard : TargetStandard
        {
            public long LockedTarget;
        }
        /*命令处理
         * 2021/12/17
         * 尝试通过以帧数计数的方式来执行命令的方法失败了，原因有可能是从集合中移除元素会消耗较长时间。
         * 猜想：移除指令也会遍历集合？采用RemoveAll进行尝试
         * 仅对作为索引的List进行变动，实际储存只增不减
         * 另外，将命令执行改为以时间计数的方式
         * 此处借鉴WHIP大佬的设计，赞美他！
         * 结果不行，重新设计
        */
        //class CommandProcessor
        //{
        //    Queue<LTCCommand> queuedCommands;
        //    public void InitProcessor()
        //    {
        //        queuedCommands = new Queue<LTCCommand>();
        //    }
        //    public void Update()
        //    {
        //        double timeElapsed = Runtime.TimeSinceLastRun.TotalSeconds;

        //        foreach (LTCCommand cmd in queuedCommands)
        //        {
        //            cmd.ExeCommand(timeElapsed);
        //        }
        //    }
        //    public void QueueAction(Action action,int frequency)
        //    {
        //        LTCCommand cmd = new LTCCommand(action, frequency);
        //        queuedCommands.Enqueue(cmd);
        //    }
        //}
        //class LTCCommand
        //{
        //    public Action action;
        //    readonly double runFrequency = 1;
        //    double TimeSinceLastRun = 0;
        //    public LTCCommand(Action _action,int frequency)
        //    {
        //        action = _action;
        //        runFrequency = 1f / frequency;
        //    }
        //    public void ExeCommand(double timeElapsed)
        //    {
        //        TimeSinceLastRun += timeElapsed;
        //        if(TimeSinceLastRun >= runFrequency)
        //        {
        //            action.Invoke();
        //            TimeSinceLastRun = 0;
        //        }
        //    }
        //}    
        /// <summary>
        /// 序列执行命令
        /// 按照顺序和间隔时间执行序列中的命令，以获得更多的缓冲时间
        /// </summary>
        class SequencedCommand
        {
            int counter = 0;
            List<SeqSingleCmd> sequencedAction;
            double timeElapsed = 0;
            public SequencedCommand()
            {
                counter = 0;
                sequencedAction = new List<SeqSingleCmd>();
                timeElapsed = 0;
            }
            public void Commence(double yield)
            {
                if(sequencedAction.Count>0)
                {
                    timeElapsed += yield;
                    if(timeElapsed >= sequencedAction[counter].yieldTime)
                    {
                        sequencedAction[counter].command.Invoke();
                        counter += 1;
                        counter = counter >= sequencedAction.Count ? 0 : counter;
                        timeElapsed = 0;
                    }
                }
            }
            public void AddCommand(Action cmd,double yieldTime)
            {
                SeqSingleCmd command = new SeqSingleCmd(cmd, yieldTime);
                sequencedAction.Add(command);
            }
        }
        class SeqSingleCmd
        {
            public Action command;
            public double yieldTime;
            public SeqSingleCmd(Action cmd,double _yield)
            {
                command = cmd;
                yieldTime = _yield;
            }
        }
    }
}
