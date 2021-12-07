using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRageMath;

namespace SEScript
{
    class LTC_TurretControl:API
    {
        IMyMotorStator VerticalRot;
        IMyMotorStator HorizontalRot;
        IMyMotorStator VerticalRev;
        IMyTimerBlock TriggerBlock;
        IMyRemoteControl remote;
        List<IMyPistonBase> pistons = new List<IMyPistonBase>();
        IMyMotorBase Trigger;
        IMySmallGatlingGun gun;
        IMyCameraBlock IFF;
        List<IMyShipWelder> wld = new List<IMyShipWelder>();
        IMyProgrammableBlock FireControl;

        bool CheckReady = false; //组件初始化
        //float fireCount = 0; //开火计数
        //float fireCD = 300f; //开火冷却，避免因为下一发炮弹刷新掉前一发

        float reloadTime = 0f; //装弹计数
        //float reloadLength = 200f; //装弹冷却

        int AimingLag = 0; //开火阻碍倒计时，若长时间无法击发则重新申请目标

        Vector3D targetPos;
        Vector3D targetVel;
        float shellSpeed = 459.88f; //弹速
        TurretStatus curStatus = TurretStatus.Idle; //炮塔索敌状态
        ReloadStatus curReload = ReloadStatus.Ready; //装弹状态
        Quaternion aimOffset = new Quaternion(); //炮口方向修正
        enum TurretStatus
        {
            Aiming,
            Idle,
            Manual,
            Auto,
        }
        enum ReloadStatus
        {
            Fired,
            Extending,
            Attached,
            Retracting,
            Ready,
        }
        void Main(string msg)
        {
            if (!CheckReady)
            {
                CheckComponents();
                return;
            }
            //fireCount = (fireCount > 0) ? fireCount -= 1 : 0;
            Echo("Running");
            Reload();
            //Echo("Fire: " + fireCount.ToString() + "/" + fireCD.ToString());
            Echo("Reload: " + curReload.ToString());
            DeseralizeMsg(msg);
            switch(curStatus)
            {
                case TurretStatus.Aiming:
                    {
                        Echo("Aiming");
                        Echo("Targeting " + targetPos.ToString());
                        AimByRotor();
                        break;
                    }
                case TurretStatus.Idle:
                    {
                        Echo("Idle");
                        RestorePos();
                        FireControl.CustomData += "FireControl|TurretRequestTarget|" + Me.GetId() + "|+";
                        break;
                    }
                case TurretStatus.Manual:
                    {
                        Echo("Manual");
                        MoveByRotor();
                        break;
                    }
                case TurretStatus.Auto:
                    {
                        Echo("Auto");
                        AimByRotor();
                        break;
                    }
            }
        }
        /// <summary>
        /// 炮塔鼠标控制
        /// </summary>
        void MoveByRotor()
        {
            HorizontalRot.TargetVelocityRPM = remote.RotationIndicator.Y;
            VerticalRot.TargetVelocityRPM = remote.RotationIndicator.X;
            VerticalRev.TargetVelocityRPM = remote.RotationIndicator.X * -1;
        }
        /// <summary>
        /// 炮塔瞄准
        /// </summary>
        void AimByRotor()
        {
            LookAtDirection(CalculateCollisionPos(), true);
        }
        /// <summary>
        /// 借鉴于MEA的预瞄算法
        /// </summary>
        /// <returns></returns>
        Vector3D CalculateCollisionPos()
        {
            Vector3D hitPos;
            double hitTime;
            hitTime = Vector3D.Distance(Me.GetPosition(), targetPos) / shellSpeed;
            hitPos = targetPos + (targetVel - remote.GetNaturalGravity() * hitTime / 2 - remote.GetShipVelocities().LinearVelocity) * hitTime;
            for(int i = 0;i<6;i++)
            {
                hitTime = Vector3D.Distance(Me.GetPosition(), hitPos) / shellSpeed;
                hitPos = targetPos + (targetVel - remote.GetNaturalGravity() * hitTime / 2 - remote.GetShipVelocities().LinearVelocity) * hitTime;
            }
            return hitPos;
        }
        /// <summary>
        /// 恢复位置
        /// </summary>
        void RestorePos()
        {
            Vector3D restorePos = HorizontalRot.GetPosition() + HorizontalRot.WorldMatrix.Forward * 1000f;
            LookAtDirection(restorePos, false);
            return;
        }
        /// <summary>
        /// 转向位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="isFiring"></param>
        void LookAtDirection(Vector3D pos,bool isFiring)
        {
            //if (reloadTime < 10) return;
            Echo("Pos: " + pos.ToString());
            Echo("Aim: " + AimingLag);
            AimingLag += 1;
            if(curStatus != TurretStatus.Aiming)
            {
                FireControl.CustomData += "FireControl|TurretRequestTarget|" + Me.GetId() + "|+";
                AimingLag = 0;
                return;
            }           

            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), remote.WorldMatrix.Forward, remote.WorldMatrix.Up);
            Vector3D posAngle = Vector3D.Normalize(Vector3D.TransformNormal(Vector3D.Transform(pos - remote.GetPosition(),aimOffset), matrix));
            
            double distance = Vector3D.Distance(remote.GetPosition(), pos);
            //fireCD = distance > 1500 ? ((float)distance / 1000f) * 300f : 300f;
            if (posAngle.X > -0.001f && posAngle.X < 0.001f && posAngle.Y > -0.001f && posAngle.Y < 0.001f && isFiring)
            {
                Fire();
            }
            VerticalRot.TargetVelocityRPM = (float)posAngle.Y * -50;
            VerticalRev.TargetVelocityRPM = (float)posAngle.Y * 50;
            HorizontalRot.TargetVelocityRPM = (float)posAngle.X * 50;
        }
        /// <summary>
        /// 获取组件
        /// </summary>
        void CheckComponents()
        {
            FireControl = GridTerminalSystem.GetBlockWithName("LTC_FireControl") as IMyProgrammableBlock;
            FireControl.CustomData += "FireControl|RegisterTurret" + Me.GetId() + "|+";
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            foreach (var group in groups)
            {
                List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
                group.GetBlocks(terminals);
                if (terminals.Contains(Me as IMyTerminalBlock))
                {
                    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(remotes,blocks => blocks.CustomName == "Remote");
                    if (remotes.Count == 0)
                    {
                        continue;
                    }
                    remote = remotes[0];
                    remote.Direction = Base6Directions.Direction.Forward;
                    List<IMyMotorStator> vet = new List<IMyMotorStator>();
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "VerticalRot");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    VerticalRot = vet[0];
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "VerticalRotRev");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    VerticalRev = vet[0];
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "HorizontalRot");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    HorizontalRot = vet[0];
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "Trigger");
                    if(vet.Count == 0)
                    {
                        continue;
                    }
                    Trigger = vet[0];
                    List<IMyTimerBlock> tim = new List<IMyTimerBlock>();
                    group.GetBlocksOfType(tim, blocks => blocks.CustomName == "LTC_Trigger");
                    if (tim.Count == 0)
                    {
                        continue;
                    }
                    TriggerBlock = tim[0];
                    group.GetBlocksOfType(pistons);
                    if(pistons.Count == 0)
                    {
                        continue;
                    }
                    List<IMySmallGatlingGun> gat = new List<IMySmallGatlingGun>();
                    group.GetBlocksOfType(gat);
                    if(gat.Count == 0)
                    {
                        continue;
                    }
                    gun = gat[0];
                    List<IMyCameraBlock> cam = new List<IMyCameraBlock>();
                    group.GetBlocksOfType(cam);
                    if(cam.Count == 0)
                    {
                        continue;
                    }
                    IFF = cam[0];
                    IFF.EnableRaycast = true;
                    group.GetBlocksOfType(wld);
                    if(wld.Count == 0)
                    {
                        continue;
                    }
                }
            }

            if (remote == null)
            {
                Echo("Remote");
                return;
            }
            if (VerticalRot == null)
            {
                Echo("Vertical");
                return;
            }
            if (VerticalRot == null)
            {
                Echo("VerticalRev");
                return;
            }
            if (HorizontalRot == null)
            {
                Echo("Horizontal");
                return;
            }
            if(pistons.Count == 0)
            {
                Echo("Piston");
                return;
            }
            if(Trigger == null)
            {
                Echo("Trigger");
                return;
            }
            if(gun == null)
            {
                Echo("Gun");
                return;
            }
            if(IFF == null)
            {
                Echo("Camera");
                return;
            }
            if(wld.Count == 0)
            {
                Echo("Welder");
                return;
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            CheckReady = true;
            return;
        }
        /// <summary>
        /// 装弹进程
        /// </summary>
        void Reload()
        {
            switch (curReload)
            {
                case ReloadStatus.Fired:
                    {
                        //if (GetPistionStatus() != 0) return;
                        foreach (var pis in pistons)
                        {
                            pis.Extend();
                        }
                        curReload = ReloadStatus.Extending;
                        reloadTime = 0;
                        break;
                    }
                case ReloadStatus.Extending:
                    {
                        Trigger.Detach();
                        Trigger.Attach();
                        if (Trigger.IsAttached)
                        {
                            curReload = ReloadStatus.Attached;
                        }
                        break;
                    }
                case ReloadStatus.Attached:
                    {
                        foreach (var pis in pistons)
                        {
                            pis.Retract();
                        }
                        foreach (var welder in wld)
                        {
                            welder.Enabled = true;
                        }
                        curReload = ReloadStatus.Retracting;
                        reloadTime = 0;
                        break;
                    }
                case ReloadStatus.Retracting:
                    {
                        if (GetPistionStatus() == 0)
                        {
                            reloadTime += 1;
                        }
                        if(reloadTime > 30)
                        {
                            curReload = ReloadStatus.Ready;
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// 获取活塞状态
        /// </summary>
        /// <returns></returns>
        int GetPistionStatus()
        {
            int Reversed = 1;
            int MoveCount = 0;
            foreach (var pis in pistons)
            {
                Echo(pis.Status.ToString());
                switch(pis.Status)
                {
                    case PistonStatus.Extended:
                        {
                            Reversed = 2;
                            MoveCount += 1;
                            break;
                        }
                    case PistonStatus.Retracted:
                        {
                            Reversed = 0;
                            MoveCount += 1;
                            break;
                        }
                }
            }
            Echo(MoveCount.ToString() + "/" + pistons.Count + " - " + Reversed.ToString());
            if(MoveCount>=pistons.Count-2)
            {
                return Reversed;
            }
            return 1;
        }
        /// <summary>
        /// 射击
        /// </summary>
        void Fire()
        {
            MyDetectedEntityInfo target = IFF.Raycast(100);
            if (target.Relationship == VRage.Game.MyRelationsBetweenPlayerAndBlock.Owner)
            {
                FireControl.CustomData += "FireControl|TurretRequestTarget|" + Me.GetId() + "|+";
                return;
            }
            if (curReload == ReloadStatus.Ready)
            {
                TriggerBlock.Trigger();
                //fireCount = fireCD;
                foreach (var welder in wld)
                {
                    welder.Enabled = false;
                }
                AimingLag = 0;
                curReload = ReloadStatus.Fired;
            }
            else
            {
                return;
            }
            if(curStatus == TurretStatus.Auto || curStatus == TurretStatus.Idle)
            {
                curStatus = TurretStatus.Idle;
                FireControl.CustomData += "FireControl|TurretRequestTarget|" + Me.GetId() + "|+";
            }
        }
        /// <summary>
        /// 分析指令
        /// </summary>
        /// <param name="msg"></param>
        void DeseralizeMsg(string msg)
        {
            if(string.IsNullOrEmpty(msg))
                msg = Me.CustomData;
            string[] messages = msg.Split('|');
            if (messages[0] != "Turret") return;
            switch(messages[1])
            {
                default:
                    {
                        break;
                    }
                case "KeepTarget":
                    {
                        string[] pos = messages[2].Split(',');
                        Echo(pos[0]);
                        Vector3D.TryParse(pos[0], out targetPos);
                        Vector3D.TryParse(pos[1], out targetVel);
                        curStatus = TurretStatus.Aiming;
                        break;
                    }
                case "Idle":
                    {
                        if (curStatus != TurretStatus.Aiming)
                            curStatus = TurretStatus.Idle;
                        break;
                    }
                case "Manual":
                    {
                        curStatus = TurretStatus.Manual;
                        remote.IsMainCockpit = true;
                        break;
                    }
                case "Target":
                    {
                        string[] pos = messages[2].Split(',');
                        Echo(pos[0]);
                        Vector3D.TryParse(pos[0], out targetPos);
                        Vector3D.TryParse(pos[1], out targetVel);
                        curStatus = TurretStatus.Auto;
                        break;
                    }
                case "Restore":
                    {
                        curStatus = TurretStatus.Idle;
                        break;
                    }
                case "Fire":
                    {
                        Fire();
                        break;
                    }
            }
            if(string.IsNullOrEmpty(msg))
                Me.CustomData = "";
        }
    }
}
