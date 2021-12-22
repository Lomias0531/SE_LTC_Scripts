using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace SEScript
{
    class MoveScript:API
    {
        IMyMotorStator VerticalRot;
        IMyMotorStator HorizontalRot;
        IMyMotorStator VerticalRev;
        IMyRemoteControl remote;
        IMyTimerBlock Trigger;
        bool CheckReady = false;
        void Main()
        {
            if(!CheckReady)
            {
                CheckComponents();
                return;
            }
            Echo("EEE");
            MoveByRotor();
        }
        void MoveByRotor()
        {
            HorizontalRot.TargetVelocityRPM = remote.RotationIndicator.Y;
            VerticalRot.TargetVelocityRPM = remote.RotationIndicator.X;
            VerticalRev.TargetVelocityRPM = remote.RotationIndicator.X * -1;
        }
        void CheckComponents()
        {
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            Echo(groups.Count.ToString());
            foreach (var group in groups)
            {
                List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
                group.GetBlocks(terminals);
                Echo(group.Name);
                if (terminals.Contains(Me as IMyTerminalBlock))
                {
                    Echo("ERE");
                    //remote = GridTerminalSystem.GetBlockWithName("LTC_TurretRemote") as IMyRemoteControl;
                    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
                    group.GetBlocksOfType(remotes, blocks => blocks.CustomName == "Remote");
                    if (remotes.Count == 0)
                    {
                        continue;
                    }
                    remote = remotes[0];
                    //VerticalRot = GridTerminalSystem.GetBlockWithName("VerticalRot") as IMyMotorStator;
                    List<IMyMotorStator> vet = new List<IMyMotorStator>();
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "VerticalRot");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    VerticalRot = vet[0];
                    //VerticalRev = GridTerminalSystem.GetBlockWithName("VerticalRotRev") as IMyMotorStator;
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "VerticalRotRev");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    VerticalRev = vet[0];
                    //HorizontalRot = GridTerminalSystem.GetBlockWithName("HorizontalRot") as IMyMotorStator;
                    group.GetBlocksOfType(vet, blocks => blocks.CustomName == "HorizontalRot");
                    if (vet.Count == 0)
                    {
                        continue;
                    }
                    HorizontalRot = vet[0];
                    //Trigger = GridTerminalSystem.GetBlockWithName("LTC_Trigger") as IMyTimerBlock;
                    List<IMyTimerBlock> tim = new List<IMyTimerBlock>();
                    group.GetBlocksOfType(tim, blocks => blocks.CustomName == "LTC_Trigger");
                    if (tim.Count == 0)
                    {
                        continue;
                    }
                    Trigger = tim[0];
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
            if (Trigger == null)
            {
                Echo("Timer");
                return;
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            CheckReady = true;
            return;
        }
    }
}
