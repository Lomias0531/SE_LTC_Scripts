﻿using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRageMath;

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
        int ScanOffsetX;
        int ScanOffsetY;
        List<Vector3D> MissileTargets;
        int displayCount = 0;
        int missileCount = 0;
        enum WeaponMode
        {
            Manual,
            Auto,
            Halt,
        }
        List<VRage.Game.MyRelationsBetweenPlayerAndBlock> TargetFilter = new List<VRage.Game.MyRelationsBetweenPlayerAndBlock>() { VRage.Game.MyRelationsBetweenPlayerAndBlock.Enemies};
        Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            (Me as IMyProgrammableBlock).TryRun("Default");
        }
        void Main(string arg)
        {
            if(!CheckReady)
            {
                CheckComponents();
            }
            DisplayInfo();
            ExecuteCommands(arg);
            switch(curMode)
            {
                default:
                    {
                        break;
                    }
            }
        }
        void DisplayInfo()
        {
            displayCount = displayCount > 120 ? 0 : displayCount += 1;
            if(displayCount > 60)
            {
                if(AllScanTargets.Count>0)
                {
                    Echo("=====COMBATING=====");
                }
                else
                {
                    Echo("=====SCANNING======");
                }
            }else
            {
                Echo(" ");
            }
            Echo("Targets: " + AllScanTargets.Count);
            Echo("Turrets: " + Turrets.Count);
            Echo("PointDefenses: " + PointDefenses.Count);
            Echo("Cams: " + GetAvailableScanner().Count);
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
        //摄像头扫描不能逐帧进行，这里需要注意
        void AcquireTargets()
        {
            ShortRangeScanTargets.Clear();
            foreach (IMyLargeTurretBase tut in AutoWeapons)
            {
                MyDetectedEntityInfo target = tut.GetTargetedEntity();
                if(TargetFilter.Contains(target.Relationship) || target.Type == MyDetectedEntityType.Missile)
                    ShortRangeScanTargets.Add(target);
            }
            if (AllScanTargets.Count > 0)
            {
                LongRangeDetailedScan();
            }
            else
            {
                LongRangeSimpleScan();
            }
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
        //长程简单扫描，由于消耗较多电力，因此设定有扫描期限
        void LongRangeSimpleScan()
        {
            LongRangeScanTargets.Clear();
            
            foreach (var cam in GetAvailableScanner())
            {
                cam.EnableRaycast = true;
                for(int i = 0;i<10;i++)
                {
                    float X = (float)(rnd.Next(-45000, 45000) / 1000f);
                    float Y = (float)(rnd.Next(-45000, 45000) / 1000f);
                    MyDetectedEntityInfo tar = cam.Raycast(4000, X, Y);
                    if (!tar.IsEmpty())
                    {
                        if(TargetFilter.Contains(tar.Relationship))
                            LongRangeScanTargets.Add(tar);
                    }
                }
            }
        }
        //长程详细扫描，在之前检测到有敌对目标时进行，将持续性消耗大量电力
        void LongRangeDetailedScan()
        {
            ScanOffsetX += 1;
            List<IMyCameraBlock> scanners = GetAvailableScanner();
            if (ScanOffsetX>20)
            {
                ScanOffsetX = 0;
                ScanOffsetY += 1;
                if(ScanOffsetY > 20)
                {
                    ScanOffsetY = 0;
                    for (int i = LongRangeScanTargets.Count - 1; i >= 0; i--)
                    {
                        bool foundThis = false;
                        for (int tx = -10; tx <= 10; tx++)
                        {
                            for (int ty = -10; ty <= 10; ty++)
                            {
                                for(int tz = -10;tz <= 10; tz++)
                                {
                                    foreach (var cam in scanners)
                                    {
                                        MyDetectedEntityInfo detect = cam.Raycast(LongRangeScanTargets[i].Position + new Vector3D(LongRangeScanTargets[i].BoundingBox.Size.X * tx * 0.1, LongRangeScanTargets[i].BoundingBox.Size.Y * ty * 0.1, LongRangeScanTargets[i].BoundingBox.Size.Z * tz * 0.1));
                                        if (!detect.IsEmpty())
                                        {
                                            foundThis = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (!foundThis)
                        {
                            LongRangeScanTargets.RemoveAt(i);
                        }
                    }
                }
            }

            foreach (var cam in scanners)
            {
                cam.EnableRaycast = true;
                for(float offsetx = -45; offsetx < 45; offsetx += 3)
                {
                    for(float offsety = -45; offsety<45;offsety +=3)
                    {
                        MyDetectedEntityInfo tar = cam.Raycast(4000, offsetx + ScanOffsetX * 0.15f, offsety + ScanOffsetY * 0.15f);
                        if(!tar.IsEmpty())
                        {
                            if((tar.Relationship == VRage.Game.MyRelationsBetweenPlayerAndBlock.Enemies) && !LongRangeScanTargets.Contains(tar))
                            {
                                LongRangeScanTargets.Add(tar);
                            }
                        }
                    }
                }
            }
        }
        List<IMyCameraBlock> GetAvailableScanner()
        {
            List<IMyCameraBlock> cams = new List<IMyCameraBlock>();
            GridTerminalSystem.GetBlocksOfType(cams, blocks => blocks.CustomName.Contains("Scanner"));
            return cams;
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
            for(int t = 0;t<5;t++)
            {
                if (commandQueue.Count <= 0)
                {
                    return;
                }
                string curCommand = commandQueue.Dequeue();
                string[] curCmd = curCommand.Split('|');
                switch (curCmd[1])
                {
                    default:
                        {
                            break;
                        }
                    case "RegisterTurret": //注册炮塔
                        {
                            foreach (IMyProgrammableBlock item in CommandBlocks)
                            {
                                if (item.GetId() == long.Parse(curCmd[2]))
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
                            switch (curCmd[2])
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
                                if (item.GetId() == long.Parse(curCmd[2]))
                                {
                                    item.CustomData = "Turret|Target|" + AllScanTargets[index].Position.ToString() + "," + AllScanTargets[index].Velocity.ToString();
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
                                    item.CustomData = "PointDefense|Target|" + ShortRangeScanTargets[index].Position.ToString();
                                }
                            }
                            break;
                        }
                    case "TurretAimAt": //炮塔锁定目标
                        {
                            foreach (var item in Turrets)
                            {
                                item.CustomData = "Turret|KeepTarget|" + curCmd[2];
                            }

                            //List<IMyCameraBlock> scanners = GetAvailableScanner();
                            //Vector3D ScanPos = new Vector3D();
                            //string[] target = curCmd[2].Split(',');
                            //Vector3D.TryParse(target[0], out ScanPos);
                            //for (int s = 0; s < 10; s++)
                            //{
                            //    foreach (var cam in scanners)
                            //    {
                            //        MyDetectedEntityInfo detect = cam.Raycast(ScanPos);
                            //        if (!detect.IsEmpty())
                            //        {
                            //            if (!LongRangeScanTargets.Contains(detect))
                            //            {
                            //                LongRangeScanTargets.Add(detect);
                            //            }
                            //            break;
                            //        }
                            //    }
                            //}
                            break;
                        }
                    case "MissileLaunchAt": //导弹发射
                        {
                            CommandBlocks.Clear();
                            GridTerminalSystem.GetBlocksOfType(CommandBlocks);
                            MissileLaunchers.Clear();
                            MissileTargets = new List<Vector3D>();
                            string[] targets = curCmd[2].Split(',');
                            foreach (var target in targets)
                            {
                                Vector3D pos = new Vector3D();
                                Vector3D.TryParse(target, out pos);
                                MissileTargets.Add(pos);
                            }
                            foreach (var item in CommandBlocks)
                            {
                                if (item.CustomName.Contains("LTC_Missile"))
                                {
                                    MissileLaunchers.Add(item);
                                }
                            }
                            foreach (var item in MissileLaunchers)
                            {
                                int index = rnd.Next(0, MissileTargets.Count - 2);
                                item.CustomData = "Missile|Launch|" + MissileTargets[index].ToString();
                                item.TryRun("Missile|Launch|" + MissileTargets[index].ToString());
                            }

                            //List<IMyCameraBlock> scanners = GetAvailableScanner();
                            //Vector3D ScanPos = new Vector3D();
                            //Vector3D.TryParse(curCmd[2], out ScanPos);
                            //for (int s = 0; s < 10; s++)
                            //{
                            //    foreach (var cam in scanners)
                            //    {
                            //        MyDetectedEntityInfo detect = cam.Raycast(ScanPos);
                            //        if (!detect.IsEmpty())
                            //        {
                            //            if (!LongRangeScanTargets.Contains(detect))
                            //            {
                            //                LongRangeScanTargets.Add(detect);
                            //            }
                            //            break;
                            //        }
                            //    }
                            //}
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
                    case "TargetFilter":
                        {
                            switch (curCmd[2])
                            {
                                case "Enemies":
                                    {
                                        TargetFilter.Clear();
                                        TargetFilter.Add(VRage.Game.MyRelationsBetweenPlayerAndBlock.Enemies);
                                        break;
                                    }
                                case "All":
                                    {
                                        TargetFilter.Clear();
                                        TargetFilter.Add(VRage.Game.MyRelationsBetweenPlayerAndBlock.Enemies);
                                        TargetFilter.Add(VRage.Game.MyRelationsBetweenPlayerAndBlock.Neutral);
                                        TargetFilter.Add(VRage.Game.MyRelationsBetweenPlayerAndBlock.NoOwnership);
                                        break;
                                    }
                            }
                            break;
                        }
                }
                if(string.IsNullOrEmpty(msg))
                    Me.CustomData = "";
            }            
        }
        string SerializeTarget(MyDetectedEntityInfo target)
        {
            string result = "{";
            result += target.EntityId.ToString() + ",";
            result += target.Name + ",";
            result += ((int)target.Type).ToString() + ",";
            result += ((Vector3D)target.HitPosition).ToString() + ",";
            result += ",";
            result += ((Vector3D)target.Velocity).ToString() + ",";
            result += ((int)target.Relationship).ToString() + ",";
            result += "}";
            return result;
        }
        MyDetectedEntityInfo DeserializeTarget(string target)
        {
            MyDetectedEntityInfo result = new MyDetectedEntityInfo();
            return result;
        }
    }
}
