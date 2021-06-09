using System;
using System.Collections.Generic;
using System.Numerics;
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
         * 当前目标：显示舰船库存状况、电量、航速 - 完成
         * 间隔随机时间卖萌 - 完成
         * 2021/6/6
         * 当前目标：分别显示舰船氧气、氢气储量 - 完成
         * 获取视野中目标
         * 多方块通信
         * UI控制与显示 - 完成
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
        List<IMyGyro> Gyrospheres; //陀螺仪方块
        IMyProgrammableBlock OpheliaMain; //主控程序块

        //IMyCameraBlock TargetIndicator; //目标选择镜头
        //IMyShipController TargetIndController; //目标选择控制
        //IMyMotorBase TargetIndRot_X;

        //--------------用户设定---------------------
        bool isSelfCheckCompleted = false; //自检是否完成

        Dictionary<string, long> invList; //库存
        float powerLevel; //能量百分比

        List<string> opheliaSpeeches; //文字记录
        PowerStatus curPowerStatus; //当前能量情况
        PowerStatus lastPowerStatus; //上次能量情况

        //float GasLevel; //气体情况
        float HydrogenLevel; //氢气储量
        float OxygenLevel; //氧气储量

        //List<MyDetectedEntityInfo> detectedTargets; //发现的目标

        Random rnd; //随机数

        int randomTalkCD; //说话CD
        int talkTick; //说话时间
        OpheliaMood curMood; //当前心情

        //--------------UI控制---------------------
        bool isManualFlight; //当前是否控制推进器
        Vector3 menuPos; //菜单选中位置
        int menuTick = 0;

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
        List<MenuItem> UIMenu_0 = new List<MenuItem>
        {
            new MenuItem()
                {
                    Description = "飞行控制",
                    Command = "MenuNext",
                },
            new MenuItem()
                {
                    Description = "防御模式",
                    Command = "MenuNext",
                },
            new MenuItem()
                {
                    Description = "攻击模式",
                    Command = "MenuNext",
                },
            new MenuItem()
                {
                    Description = "库存信息",
                    Command = "MenuNext",
                },
            new MenuItem()
                {
                    Description = "生产控制",
                    Command = "MenuNext",
                },
            new MenuItem()
                {
                    Description = "人格选项",
                    Command = "MenuNext",
                },
            new MenuItem()
                {
                    Description = "重启系统",
                    Command = "Reset",
                }
        };
        List<List<MenuItem>> UIMenu_1 = new List<List<MenuItem>>
        {
            new List<MenuItem>
            {
                new MenuItem()
                {
                    Description = "恢复手动飞行",
                    Command = "ManualFlight|True",
                },
                new MenuItem()
                {
                    Description = "选择导航终点",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "自动导航",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "前进三",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "停止",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "自动对接",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "启动跳跃",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "返回",
                    Command = "MenuBack",
                },
            },
            new List<MenuItem>
            {
                new MenuItem()
                {
                    Description = "点防御系统", //可选On/Off
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "允许规避机动", //可选On/Off
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "返回",
                    Command = "MenuBack",
                },
            },
            new List<MenuItem>
            {
                new MenuItem()
                {
                    Description = "显示目标列表",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "锁定最近目标",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "锁定最大目标",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "目标详细信息",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "主炮组自动开火", //可选On/Off
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "返回",
                    Command = "MenuBack",
                },
            },
            new List<MenuItem>
            {
                new MenuItem()
                {
                    Description = "显示库存详情",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "显示气体储量",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "显示资源产量",
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "返回",
                    Command = "MenuBack",
                },
            },
            new List<MenuItem>
            {
                new MenuItem()
                {
                    Description =  "自动补充弹药", //可选On/Off
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "自动补充主炮部件", //可选On/Off
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "返回",
                    Command = "MenuBack",
                },
            },
            new List<MenuItem>
            {
                new MenuItem()
                {
                    Description = "人格开启", //可选On/Off
                    Command = "",
                },
                new MenuItem()
                {
                    Description = "返回",
                    Command = "MenuBack",
                },
            },
        };
        void Main(string msg)
        {
            if (!isSelfCheckCompleted)
            {
                SelfCheck();
                return;
            }
            ExecuteCommand(msg);
            DisplayUI();
            DisplayStorage();
            CheckStatus();
            RandomTalk();
        }
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

            OpheliaMain = GridTerminalSystem.GetBlockWithName("LTC_Ophelia_Main") as IMyProgrammableBlock;

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

            Gyrospheres = new List<IMyGyro>();
            GridTerminalSystem.GetBlocksOfType(Gyrospheres);
            if(Gyrospheres.Count == 0)
            {
                StatusCheck += "没有找到陀螺仪，Ophelia动不了啦！";
                isSelfCheckCompleted = false;
            }

            if (!isSelfCheckCompleted)
            {
                ShowOverallMessage(StatusCheck);
                return;
            }

            isSelfCheckCompleted = true;
            menuPos = new Vector3(0, -1, -1);

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
            menuTick -= 1;
            if (menuTick <= 0)
            {
                if (CaptainSeat.MoveIndicator.Z > 0)
                {
                    if (menuPos.Y == -1)
                    {
                        menuPos.X += 1;
                        if (menuPos.X >= UIMenu_0.Count)
                        {
                            menuPos.X = 0;
                        }
                    }
                    else
                    {
                        menuPos.Y += 1;
                        if (menuPos.Y >= UIMenu_1[(int)menuPos.X].Count)
                        {
                            menuPos.Y = 0;
                        }
                    }
                    menuTick = 10;
                }
                if (CaptainSeat.MoveIndicator.Z < 0)
                {
                    if (menuPos.Y == -1)
                    {
                        menuPos.X -= 1;
                        if (menuPos.X < 0)
                        {
                            menuPos.X = UIMenu_0.Count - 1;
                        }
                    }
                    else
                    {
                        menuPos.Y -= 1;
                        if (menuPos.Y < 0)
                        {
                            menuPos.Y = UIMenu_1[(int)menuPos.X].Count - 1;
                        }
                    }
                    menuTick = 10;
                }
                if (CaptainSeat.MoveIndicator.Y > 0)
                {
                    if (menuPos.Y == -1)
                    {
                        OpheliaMain.CustomData = UIMenu_0[(int)menuPos.X].Command;
                    }
                    else
                    {
                        OpheliaMain.CustomData = UIMenu_1[(int)menuPos.X][(int)menuPos.Y].Command;
                    }
                    menuTick = 10;
                }
            }

            string displayUI = "";
            if (!isManualFlight)
            {
                if(menuPos.Y == -1)
                {
                    for(int i = 0;i<UIMenu_0.Count;i++)
                    {
                        if(menuPos.X == i)
                        {
                            displayUI += "=>";
                        }
                        displayUI += UIMenu_0[i].Description + "\r\n";
                    }
                }else
                {
                    for(int i = 0;i<UIMenu_1[(int)menuPos.X].Count;i++)
                    {
                        if(menuPos.Y == i)
                        {
                            displayUI += "=>";
                        }
                        displayUI += UIMenu_1[(int)menuPos.X][i].Description + "\r\n";
                    }
                }
            }
            else
            {
                displayUI = "手动飞行中";
            }
            opheliaUI.WriteText(displayUI);
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

            float Ocap = 0;
            float OCount = 0;
            float Hcap = 0;
            float HCount = 0;
            for (int i = 0; i < GasBlocks.Count; i++)
            {
                Echo(GasBlocks[i].BlockDefinition.SubtypeName);
                if (GasBlocks[i].BlockDefinition.SubtypeName.Contains("Hydrogen"))
                {
                    Hcap += (float)GasBlocks[i].FilledRatio;
                    HCount += 1;
                }else
                {
                    Ocap += (float)GasBlocks[i].FilledRatio;
                    OCount += 1;
                }
            }
            OxygenLevel = (Ocap / OCount) * 100f;
            HydrogenLevel = (Hcap / HCount) * 100f;
            storageMsg += "氧气储量： " + OxygenLevel.ToString("F2") + "%\r\n";
            storageMsg += "氢气储量： " + HydrogenLevel.ToString("F2") + "%\r\n";

            storageMsg += "\r\n存储：\r\n";

            invList = new Dictionary<string, long>();

            for (int i = 0; i < InventoryBlocks.Count; i++)
            {
                if (InventoryBlocks[i].GetInventory().ItemCount > 0)
                {
                    for (int t = 0; t < InventoryBlocks[i].GetInventory().ItemCount; t++)
                    {
                        var inv = InventoryBlocks[i].GetInventory().GetItemAt(t);
                        //Echo(inv.Value.Type.TypeId);
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
            //检测电池能量储备
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

            //检测氧气储备

            //检测氢气储备

            //检测弹药储备
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
        void ExecuteCommand(string msg)
        {
            if(string.IsNullOrEmpty(msg))
            {
                if (string.IsNullOrEmpty(OpheliaMain.CustomData))
                {
                    return;
                }
                msg = OpheliaMain.CustomData;
            }
            string[] cmd = msg.Split('|');
            switch(cmd[0])
            {
                case "Reset":
                    {
                        SelfCheck();
                        break;
                    }
                case "ManualFlight":
                    {
                        switch(cmd[1])
                        {
                            case "True":
                                {
                                    OpheliaSpeaks("恢复手动飞行~舰长要注意安全喔！");
                                    CaptainSeat.ControlThrusters = true;
                                    foreach (IMyGyro gyro in Gyrospheres)
                                    {
                                        gyro.GyroOverride = false;
                                    }
                                    isManualFlight = true;
                                    break;
                                }
                            case "False":
                                {
                                    OpheliaSpeaks("开始自动飞行~接下来请交给我吧！");
                                    menuPos = new Vector3(0, -1, -1);
                                    CaptainSeat.ControlThrusters = false;
                                    foreach (IMyGyro gyro in Gyrospheres)
                                    {
                                        gyro.GyroOverride = true;
                                    }
                                    isManualFlight = false;
                                    break;
                                }
                        }
                        break;
                    }
                case "MenuNext":
                    {
                        menuPos = new Vector3(menuPos.X, 0, -1);
                        break;
                    }
                case "MenuBack":
                    {
                        menuPos = new Vector3(0, -1, -1);
                        break;
                    }
            }
            if(!string.IsNullOrEmpty(OpheliaMain.CustomData))
            {
                OpheliaMain.CustomData = "";
            }
        }
    }
    public class MenuItem
    {
        public string Description;
        public string Command;
    }
}
