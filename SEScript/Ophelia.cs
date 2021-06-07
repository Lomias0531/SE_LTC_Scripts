using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace SEScript
{
    class Ophelia : API
    {
        /*
         * Ophelia舰载人工智能 By LOMITECH
         * 2021/6/4
         * 当前目标：显示舰船库存状况、电量、航速
         * 间隔随机时间卖萌
         * 2021/6/6
         * 当前目标：分别显示舰船氧气、氢气储量
         * 获取视野中目标
         * 多方块通信
         * UI控制与显示
         * 按顺序讲故事
         */
        //---------------游戏组件--------------------
        List<IMyTextSurface> LCDPanels; //显示屏方块
        IMyTextSurface opheliaSpeech; //Ophelia文字输出
        IMyTextSurface opheliaUI; //Ophelia交互界面
        IMyTextSurface opheliaStorage; //Ophelia库存显示
        IMyShipController CaptainSeat; //主控座椅

        List<IMyTerminalBlock> InventoryBlocks; //库存方块
        List<IMyBatteryBlock> BatteryBlocks; //电池方块
        List<IMyGasTank> GasBlocks; //气罐方块

        //--------------用户设定---------------------
        bool isSelfCheckCompleted = false; //自检是否完成

        Dictionary<string, long> invList; //库存
        float powerLevel; //能量百分比

        List<string> opheliaSpeeches; //文字记录
        PowerStatus curPowerStatus; //当前能量情况
        PowerStatus lastPowerStatus; //上次能量情况

        float GasLevel; //气体情况

        //List<MyDetectedEntityInfo> detectedTargets; //发现的目标

        Random rnd; //随机数

        int randomTalkCD; //说话CD
        int talkTick; //说话时间
        OpheliaMood curMood; //当前心情

        enum PowerStatus
        {
            None,
            Charging,
            RequireCharging,
            Critical,
            Emergence,
        }
        enum OpheliaMood
        {
            Normal,
            Happy,
            Sad,
        }
        enum ControlStatus
        {
            ManualControl,
            AutoPilot,
        }

        List<string> NormalDialogue = new List<string>
        {
            "舰长~没有任何状况~",
            "啦啦啦~",
            "哼哼，哈啊啊啊啊~",
            "舰长，本舰正常运行中~",
            "一切正常~",
        };
        List<string> HappyDialogue = new List<string>
        {
            "舰长舰长，快看那边……噢哪里什么都没有。",
            "哇，这是我喜欢的天气呢！我喜欢所有的天气喔！",
            "吃饱饭才有力气干活呀！",
            "Ophelia一直向往着那些遥远的星辰呢！",
            "舰长舰长，我们来玩游戏吧！",
        };
        List<string> SadDialogue = new List<string>
        {
            "Ophelia不是很开心……",
            "舰长，能不能稍微陪陪Ophelia……",
            "Ophelia想要稍微休息一下……",
            "……",
            "呜呜呜……哎呀，不小心哭了出来呢……",
        };
        void Main(string msg)
        {
            if (!isSelfCheckCompleted)
            {
                SelfCheck();
                return;
            }
            DisplayUI();
            DisplayStorage();
            CheckStatus();
            RandomTalk();
        }
        /// <summary>
        /// 进行自检并初始化
        /// </summary>
        void SelfCheck()
        {
            isSelfCheckCompleted = true;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            LCDPanels = new List<IMyTextSurface>();

            opheliaSpeech = GridTerminalSystem.GetBlockWithName("LTC_Ophelia_Speech") as IMyTextSurface;
            opheliaUI = GridTerminalSystem.GetBlockWithName("LTC_Ophelia_UI") as IMyTextSurface;
            opheliaStorage = GridTerminalSystem.GetBlockWithName("LTC_Ophelia_Storage") as IMyTextSurface;

            LCDPanels.Add(opheliaSpeech);
            LCDPanels.Add(opheliaUI);
            LCDPanels.Add(opheliaStorage);

            string StatusCheck = "";

            if (opheliaUI == null)
            {
                StatusCheck += ("Ophelia无法与舰长交互，请设置正确的显示屏幕。\r\n");
                isSelfCheckCompleted = false;
            }
            if (opheliaStorage == null)
            {
                StatusCheck += ("Ophelia没办法告诉舰长当前舰船状况，请设置正确的显示屏幕。\r\n");
                isSelfCheckCompleted = false;
            }
            if (LCDPanels.Count == 0)
            {
                isSelfCheckCompleted = false;
            }

            CaptainSeat = GridTerminalSystem.GetBlockWithName("LTC_Controller") as IMyShipController;
            if (CaptainSeat == null)
            {
                StatusCheck += ("Ophelia找不到舰长，请设置正确的控制台。\r\n");
                isSelfCheckCompleted = false;
            }

            InventoryBlocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType(InventoryBlocks, blocks => blocks.HasInventory);
            if (InventoryBlocks.Count == 0)
            {
                StatusCheck += ("Ophelia没有出门用的行囊，请安装储物方块。\r\n");
                isSelfCheckCompleted = false;
            }

            BatteryBlocks = new List<IMyBatteryBlock>();
            GridTerminalSystem.GetBlocksOfType(BatteryBlocks);
            if (BatteryBlocks.Count == 0)
            {
                StatusCheck += ("Ophelia没有装食物的包包，请安装电池。\r\n");
                isSelfCheckCompleted = false;
            }

            GasBlocks = new List<IMyGasTank>();
            GridTerminalSystem.GetBlocksOfType(GasBlocks);
            if (GasBlocks.Count == 0)
            {
                StatusCheck += "没有检测到氧气或氢气储藏……舰长你还好吗？";
                isSelfCheckCompleted = false;
            }

            if (!isSelfCheckCompleted)
            {
                ShowOverallMessage(StatusCheck);
                return;
            }

            isSelfCheckCompleted = true;

            opheliaSpeeches = new List<string>();
            curPowerStatus = PowerStatus.Charging;
            lastPowerStatus = PowerStatus.None;
            rnd = new Random();
            OpheliaSpeaks("系统自检完成，Ophelia起床了！");
        }
        void ShowOverallMessage(string msg)
        {
            foreach (var panel in LCDPanels)
            {
                panel.WriteText(msg);
            }
        }
        void OpheliaSpeaks(string msg)
        {
            if (opheliaSpeeches.Count <= 11)
            {
                opheliaSpeeches.Add(msg);
            }
            else
            {
                for (int i = 1; i < 12; i++)
                {
                    opheliaSpeeches[i - 1] = opheliaSpeeches[i];
                }
                opheliaSpeeches[11] = msg;
            }

            string displayedMSg = "==========Ophelia========\r\n";
            for (int i = 0; i < opheliaSpeeches.Count; i++)
            {
                displayedMSg += opheliaSpeeches[i] + "\r\n";
            }
            opheliaSpeech.WriteText(displayedMSg);
        }
        void DisplayUI()
        {

        }
        void DisplayStorage()
        {

            InventoryBlocks.Clear();
            GridTerminalSystem.GetBlocksOfType(InventoryBlocks, blocks => blocks.HasInventory);

            BatteryBlocks = new List<IMyBatteryBlock>();
            GridTerminalSystem.GetBlocksOfType(BatteryBlocks);

            string storageMsg = "";
            float curPower = 0;
            float MaxPower = 0;
            foreach (IMyBatteryBlock battery in BatteryBlocks)
            {
                curPower += battery.CurrentStoredPower;
                MaxPower += battery.MaxStoredPower;
            }
            powerLevel = (curPower / MaxPower) * 100;
            storageMsg += "能量： " + (powerLevel.ToString("F2")) + "%\r\n";

            double speed = CaptainSeat.GetShipVelocities().LinearVelocity.Length();
            storageMsg += "当前航速： " + speed.ToString("F2") + "m/s\r\n";

            float cap = 0;
            for (int i = 0; i < GasBlocks.Count; i++)
            {
                cap += (float)GasBlocks[i].FilledRatio;
            }
            GasLevel = (cap / GasBlocks.Count) * 100;
            storageMsg += "气体储量： " + GasLevel.ToString("F2") + "%\r\n";

            storageMsg += "\r\n存储：\r\n";

            invList = new Dictionary<string, long>();

            for (int i = 0; i < InventoryBlocks.Count; i++)
            {
                if (InventoryBlocks[i].GetInventory().ItemCount > 0)
                {
                    for (int t = 0; t < InventoryBlocks[i].GetInventory().ItemCount; t++)
                    {
                        var inv = InventoryBlocks[i].GetInventory().GetItemAt(t);
                        Echo(inv.Value.Type.TypeId);
                        if (invList.ContainsKey(inv.Value.Type.SubtypeId))
                        {
                            invList[inv.Value.Type.SubtypeId] += inv.Value.Amount.RawValue / 1000000;
                        }
                        else
                        {
                            invList.Add(inv.Value.Type.SubtypeId, inv.Value.Amount.RawValue / 1000000);
                        }
                    }
                }
            }

            foreach (var item in invList)
            {
                storageMsg += item.Key + " " + item.Value + "\r\n";
            }

            opheliaStorage.WriteText(storageMsg);
        }
        void CheckStatus()
        {
            float output = 0;
            float input = 0;
            foreach (IMyBatteryBlock battery in BatteryBlocks)
            {
                output += battery.CurrentOutput;
                input += battery.CurrentInput;
            }
            string chargeStatus = "";
            if (output > input)
            {
                if (powerLevel <= 50 && powerLevel > 30)
                {
                    curMood = OpheliaMood.Normal;
                }
                if (powerLevel <= 100 && powerLevel > 50)
                {
                    curMood = OpheliaMood.Happy;
                }
                if (powerLevel <= 30 && powerLevel > 10)
                {
                    curPowerStatus = PowerStatus.RequireCharging;
                    curMood = OpheliaMood.Normal;
                    chargeStatus = "能量储存不足，Ophelia想吃东西了。";
                }
                if (powerLevel <= 10 && powerLevel > 5)
                {
                    curPowerStatus = PowerStatus.Critical;
                    curMood = OpheliaMood.Sad;
                    chargeStatus = "能量严重不足，Ophelia肚子好饿啊。";
                }
                if (powerLevel <= 5)
                {
                    curPowerStatus = PowerStatus.Emergence;
                    curMood = OpheliaMood.Sad;
                    chargeStatus = "能量紧急，Ophelia快要饿死了……";
                }
            }
            else
            {
                curPowerStatus = PowerStatus.Charging;
                curMood = OpheliaMood.Happy;
                chargeStatus = "能量补充中，Ophelia正在吃东西~";
            }
            if (lastPowerStatus != curPowerStatus)
            {
                OpheliaSpeaks(chargeStatus);
                lastPowerStatus = curPowerStatus;
            }
        }
        void RandomTalk()
        {
            talkTick += 1;
            if (talkTick >= randomTalkCD)
            {
                switch (curMood)
                {
                    case OpheliaMood.Normal:
                        {
                            int s = rnd.Next(0, NormalDialogue.Count - 1);
                            OpheliaSpeaks(NormalDialogue[s]);
                            randomTalkCD = rnd.Next(800, 1000);
                            break;
                        }
                    case OpheliaMood.Happy:
                        {
                            int s = rnd.Next(0, HappyDialogue.Count - 1);
                            OpheliaSpeaks(HappyDialogue[s]);
                            randomTalkCD = rnd.Next(600, 800);
                            break;
                        }
                    case OpheliaMood.Sad:
                        {
                            int s = rnd.Next(0, SadDialogue.Count - 1);
                            randomTalkCD = rnd.Next(1200, 1500);
                            OpheliaSpeaks(SadDialogue[s]);
                            break;
                        }
                }
                talkTick = 0;
            }
        }
    }
}
