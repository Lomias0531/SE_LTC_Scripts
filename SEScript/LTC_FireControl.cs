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

        List<MyDetectedEntityInfo> TargetDic;
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
            Echo("Targets: " + TargetDic.Count);
            Echo("Turrets: " + Turrets.Count);
            Echo("PointDefenses: " + PointDefenses.Count);
            Echo("Scan: " + ScanCount);
            ScanCount = ScanCount > 0 ? ScanCount -= 1 : 0;
            List<IMyCameraBlock> cams = new List<IMyCameraBlock>();
            GridTerminalSystem.GetBlocksOfType(cams);
            Echo("Cams: " + cams.Count);
        }
        void CheckComponents()
        {
            CommandBlocks = new List<IMyProgrammableBlock>();
            Indicators = new List<IMyProgrammableBlock>();
            MissileLaunchers = new List<IMyProgrammableBlock>();
            AutoWeapons = new List<IMyLargeTurretBase>();
            TargetDic = new List<MyDetectedEntityInfo>();
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
            TargetDic.Clear();
            foreach (IMyLargeTurretBase tut in AutoWeapons)
            {
                MyDetectedEntityInfo target = tut.GetTargetedEntity();
                if(target.Relationship == VRage.Game.MyRelationsBetweenPlayerAndBlock.Enemies)
                    TargetDic.Add(target);
            }
            if(TargetDic.Count > 0)
            {
                ScanCD = 10;
            }else
            {
                ScanCD = 100;
            }
            CamScan();
        }
        void CamScan()
        {
            if (ScanCount > 0) return;
            ScanCount = ScanCD;
            List<IMyCameraBlock> cams = new List<IMyCameraBlock>();
            GridTerminalSystem.GetBlocksOfType(cams);
            
            foreach (var cam in cams)
            {
                cam.EnableRaycast = true;
                for(float offsetX = -90; offsetX <= 90; offsetX+=3)
                {
                    for(float offsetY = -90;offsetY<=90;offsetY+=3)
                    {
                        MyDetectedEntityInfo tar = cam.Raycast(4000, offsetX, offsetY);
                        if(!tar.IsEmpty())
                        {
                            TargetDic.Add(tar);
                        }
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
            if (TargetDic.Count == 0)
            {
                foreach (var item in Turrets)
                {
                    item.CustomData = "Turret|Idle|";
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
                        if (TargetDic.Count == 0)
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
                        int index = rnd.Next(0, TargetDic.Count - 1);
                        foreach (var item in Turrets)
                        {
                            if(item.GetId() == long.Parse(curCmd[2]))
                            {
                                item.CustomData = "Turret|Target|" + TargetDic[index].Position.X + "_" + TargetDic[index].Position.Y + "_" + TargetDic[index].Position.Z;
                            }
                        }
                        break;
                    }
                case "PointDefenseRequestTarget": //点防御申请目标
                    {
                        AcquireTargets();
                        if (TargetDic.Count == 0)
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
                        int index = rnd.Next(0, TargetDic.Count - 1);
                        foreach (var item in PointDefenses)
                        {
                            if (item.GetId() == long.Parse(curCmd[2]))
                            {
                                item.CustomData = "PointDefense|Target|" + TargetDic[index].Position.X + "_" + TargetDic[index].Position.Y + "_" + TargetDic[index].Position.Z;
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
