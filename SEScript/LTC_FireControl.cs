using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace SEScript
{
    class LTC_FireControl:API
    {
        List<IMyProgrammableBlock> CommandBlocks;
        List<IMyProgrammableBlock> Indicators;
        List<IMyProgrammableBlock> MissileLaunchers;
        List<IMyLargeTurretBase> AutoWeapons;

        List<MyDetectedEntityInfo> ShortRangeScanTargets;
        List<MyDetectedEntityInfo> LongRangeScanTargets;
        List<MyDetectedEntityInfo> AllScanTargets;
        List<IMyProgrammableBlock> Turrets;
        List<IMyProgrammableBlock> PointDefenses;
        Queue<string> commandQueue;
        WeaponMode curMode = WeaponMode.Auto;
        bool CheckReady = false;
        Random rnd;
        int ScanCD = 100;
        int ScanCount = 0;
        enum WeaponMode
        {
            Manual,
            Auto,
            Halt,
        }
        void Main(string arg)
        {
            if(!CheckReady)
            {
                CheckComponents();
            }
            ExecuteCommands(arg);
            Echo("Targets: " + AllScanTargets.Count);
            Echo("Turrets: " + Turrets.Count);
            Echo("PointDefenses: " + PointDefenses.Count);
            Echo("Scan: " + ScanCount);
            ScanCount = ScanCount > 0 ? ScanCount -= 1 : 0;
            List<IMyCameraBlock> cams = new List<IMyCameraBlock>();
            GridTerminalSystem.GetBlocksOfType(cams);
            Echo("Cams: " + cams.Count);
            switch(curMode)
            {
                default:
                    {
                        break;
                    }
            }
        }
        void CheckComponents()
        {
            CommandBlocks = new List<IMyProgrammableBlock>();
            Indicators = new List<IMyProgrammableBlock>();
            MissileLaunchers = new List<IMyProgrammableBlock>();
            AutoWeapons = new List<IMyLargeTurretBase>();
            ShortRangeScanTargets = new List<MyDetectedEntityInfo>();
            LongRangeScanTargets = new List<MyDetectedEntityInfo>();
            AllScanTargets = new List<MyDetectedEntityInfo>();
            Turrets = new List<IMyProgrammableBlock>();
            PointDefenses = new List<IMyProgrammableBlock>();
            commandQueue = new Queue<string>();

            GridTerminalSystem.GetBlocksOfType(CommandBlocks);
            if(CommandBlocks.Count == 0)
            {
                Echo("No CommandBlocks Found!");
                return;
            }
            GridTerminalSystem.GetBlocksOfType(AutoWeapons);
            if(AutoWeapons.Count == 0)
            {
                Echo("No auto weapon found!");
                return;
            }
            foreach (IMyProgrammableBlock item in CommandBlocks)
            {
                if(item.CustomName.Contains("LTC_Turret"))
                {
                    Turrets.Add(item);
                    item.TryRun("Default");
                }
                if(item.CustomName.Contains("LTC_PointDefense"))
                {
                    PointDefenses.Add(item);
                    item.TryRun("Default");
                }
                if(item.CustomName.Contains("LTC_Indicator"))
                {
                    item.TryRun("Default");
                    Indicators.Add(item);
                }
            }

            rnd = new Random();

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            CheckReady = true;
            return;
        }
        void AcquireTargets()
        {
            ShortRangeScanTargets.Clear();
            foreach (IMyLargeTurretBase tut in AutoWeapons)
            {
                MyDetectedEntityInfo target = tut.GetTargetedEntity();
                if(target.Relationship == VRage.Game.MyRelationsBetweenPlayerAndBlock.Enemies)
                    ShortRangeScanTargets.Add(target);
            }
            if(AllScanTargets.Count > 0)
            {
                ScanCD = 1;
            }else
            {
                ScanCD = 100;
            }
            LongRangeScan();
            AllScanTargets.Clear();
            foreach (var item in ShortRangeScanTargets)
            {
                AllScanTargets.Add(item);
            }
            foreach (var item in LongRangeScanTargets)
            {
                AllScanTargets.Add(item);
            }
        }
        //长程扫描，由于消耗较多电力，因此设定有扫描期限
        void LongRangeScan()
        {
            if (ScanCount > 0) return;
            LongRangeScanTargets.Clear();
            ScanCount = ScanCD;
            List<IMyCameraBlock> cams = new List<IMyCameraBlock>();
            GridTerminalSystem.GetBlocksOfType(cams);
            
            foreach (var cam in cams)
            {
                cam.EnableRaycast = true;
                for(int i = 0;i<3000;i++)
                {
                    float X = rnd.Next(-9000, 9000) / 100f;
                    float Y = rnd.Next(-9000, 9000) / 100f;
                    MyDetectedEntityInfo tar = cam.Raycast(4000, X, Y);
                    if (!tar.IsEmpty())
                    {
                        if((tar.Relationship == VRage.Game.MyRelationsBetweenPlayerAndBlock.Enemies))
                            LongRangeScanTargets.Add(tar);
                    }
                }
            }
        }
        void ExecuteCommands(string msg)
        {
            //将获取到的命令加入序列
            string command = string.IsNullOrEmpty(msg) ? Me.CustomData : msg;
            if(!string.IsNullOrEmpty(command))
            {
                string[] cmd1 = command.Split('+');
                foreach (string item in cmd1)
                {
                    string[] cmd = item.Split('|');
                    if (cmd[0] == "FireControl" && !commandQueue.Contains(command))
                        commandQueue.Enqueue(command);
                }
            }
            if(commandQueue.Count<=0)
            {
                return;
            }
            AcquireTargets();
            if (AllScanTargets.Count == 0)
            {
                foreach (var item in Turrets)
                {
                    item.CustomData = "Turret|Idle|";
                }
                foreach (var item in PointDefenses)
                {
                    item.CustomData = "PointDefense|Idle|";
                }
            }
            //处理命令
            string curCommand = commandQueue.Dequeue();
            string[] curCmd = curCommand.Split('|');
            switch(curCmd[1])
            {
                default:
                    {
                        break;
                    }
                case "RegisterTurret": //注册炮塔
                    {
                        foreach (IMyProgrammableBlock item in CommandBlocks)
                        {
                            if(item.GetId() == long.Parse(curCmd[2]))
                            {
                                Turrets.Add(item);
                                break;
                            }
                        }
                        break;
                    }
                case "RegisterPointDefense": //注册点防御
                    {
                        foreach (IMyProgrammableBlock item in CommandBlocks)
                        {
                            if (item.GetId() == long.Parse(curCmd[2]))
                            {
                                PointDefenses.Add(item);
                                break;
                            }
                        }
                        break;
                    }
                case "SwitchWeaponMode": //切换开火模式
                    {
                        switch(curCmd[2])
                        {
                            default:
                                {
                                    curMode = WeaponMode.Auto;
                                    break;
                                }
                            case "Manual":
                                {
                                    curMode = WeaponMode.Manual;
                                    break;
                                }
                            case "Halt":
                                {
                                    curMode = WeaponMode.Halt;
                                    break;
                                }
                        }
                        break;
                    }
                case "TurretRequestTarget": //炮塔申请目标
                    {
                        AcquireTargets();
                        if (AllScanTargets.Count == 0)
                        {
                            foreach (var item in Turrets)
                            {
                                if (item.GetId() == long.Parse(curCmd[2]))
                                {
                                    item.CustomData = "Turret|Idle|";
                                }
                            }
                            break;
                        }
                        int index = rnd.Next(0, AllScanTargets.Count - 1);
                        foreach (var item in Turrets)
                        {
                            if(item.GetId() == long.Parse(curCmd[2]))
                            {
                                item.CustomData = "Turret|Target|" + AllScanTargets[index].Position.X + "_" + AllScanTargets[index].Position.Y + "_" + AllScanTargets[index].Position.Z;
                            }
                        }
                        break;
                    }
                case "PointDefenseRequestTarget": //点防御申请目标
                    {
                        AcquireTargets();
                        if (ShortRangeScanTargets.Count == 0)
                        {
                            foreach (var item in PointDefenses)
                            {
                                if (item.GetId() == long.Parse(curCmd[2]))
                                {
                                    item.CustomData = "PointDefense|Idle|";
                                }
                            }
                            break;
                        }
                        int index = rnd.Next(0, ShortRangeScanTargets.Count - 1);
                        foreach (var item in PointDefenses)
                        {
                            if (item.GetId() == long.Parse(curCmd[2]))
                            {
                                item.CustomData = "PointDefense|Target|" + ShortRangeScanTargets[index].Position.X + "_" + ShortRangeScanTargets[index].Position.Y + "_" + ShortRangeScanTargets[index].Position.Z;
                            }
                        }
                        break;
                    }
                case "TurretAimAt": //炮塔锁定目标
                    {
                        foreach(var item in Turrets)
                        {
                            item.CustomData = "Turret|KeepTarget|" + curCmd[2];
                        }
                        break;
                    }
                case "MissileLaunchAt": //导弹发射
                    {
                        GridTerminalSystem.GetBlocksOfType(CommandBlocks);
                        foreach (var item in CommandBlocks)
                        {
                            if (item.CustomName.Contains("LTC_Missile"))
                            {
                                MissileLaunchers.Add(item);
                            }
                        }
                        foreach (var item in MissileLaunchers)
                        {
                            item.CustomData = "Missile|Launch|" + curCmd[2];
                            item.TryRun("Missile|Launch|" + curCmd[2]);
                        }
                        break;
                    }
                case "IndicatorIdle": //目标指示器未选择目标
                    {
                        foreach (var item in Turrets)
                        {
                            item.CustomData = "Turret|Restore";
                        }
                        break;
                    }
            }
            Me.CustomData = "";
        }
    }
}
