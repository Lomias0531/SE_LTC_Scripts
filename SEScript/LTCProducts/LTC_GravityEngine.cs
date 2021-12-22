using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI;
using VRageMath;

namespace SEScript
{
    class LTC_GravityEngine : API
    {
        List<IMyGravityGeneratorBase> Forward;
        List<IMyGravityGeneratorBase> Back;
        List<IMyGravityGeneratorBase> Left;
        List<IMyGravityGeneratorBase> Right;
        List<IMyGravityGeneratorBase> Up;
        List<IMyGravityGeneratorBase> Down;
        List<IMyGyro> Gyros;
        IMyShipController MainController;

        bool ComponentsCheck = false;
        void Main()
        {
            if(!ComponentsCheck)
            {
                CheckComponents();
            }
            ControlEngine();
        }
        void CheckComponents()
        {
            Forward = new List<IMyGravityGeneratorBase>();
            Back = new List<IMyGravityGeneratorBase>();
            Left = new List<IMyGravityGeneratorBase>();
            Right = new List<IMyGravityGeneratorBase>();
            Up = new List<IMyGravityGeneratorBase>();
            Down = new List<IMyGravityGeneratorBase>();
            Gyros = new List<IMyGyro>();
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);
            foreach (var group in groups)
            {
                List<IMyTerminalBlock> terminals = new List<IMyTerminalBlock>();
                group.GetBlocks(terminals);
                if (terminals.Contains(Me as IMyTerminalBlock))
                {
                    MainController = GridTerminalSystem.GetBlockWithName("LTC_MainControl") as IMyShipController;
                    GridTerminalSystem.GetBlocksOfType(Gyros);
                    if(Gyros.Count == 0)
                    {
                        Echo("Gyro");
                        continue;
                    }
                    foreach (var gyro in Gyros)
                    {
                        gyro.GyroOverride = true;
                    }
                    List<IMyGravityGeneratorBase> grav = new List<IMyGravityGeneratorBase>();
                    group.GetBlocksOfType(grav);
                    if(grav.Count == 0)
                    {
                        Echo("Gravity");
                        continue;
                    }
                    foreach (IMyGravityGeneratorBase gGenerator in grav)
                    {
                        Base6Directions.Direction gyForward = gGenerator.WorldMatrix.GetClosestDirection(MainController.WorldMatrix.Forward);
                        Base6Directions.Direction gyLeft = gGenerator.WorldMatrix.GetClosestDirection(MainController.WorldMatrix.Left);
                        Base6Directions.Direction gyUp = gGenerator.WorldMatrix.GetClosestDirection(MainController.WorldMatrix.Up);
                        switch (gyForward)
                        { case Base6Directions.Direction.Down:Forward.Add(gGenerator); break; case Base6Directions.Direction.Up: Back.Add(gGenerator); break; }
                        switch (gyLeft)
                        { case Base6Directions.Direction.Down: Left.Add(gGenerator); break; case Base6Directions.Direction.Up: Right.Add(gGenerator); break; }
                        switch (gyUp)
                        { case Base6Directions.Direction.Down: Up.Add(gGenerator); break; case Base6Directions.Direction.Up: Down.Add(gGenerator); break; }
                    }
                }
            }
            if (MainController == null)
            {
                Echo("Controller");
                return;
            }
            if (Gyros.Count == 0)
            {
                Echo("Gyros");
                return;
            }
            if (Forward.Count == 0)
            {
                Echo("Forward");
                return;
            }
            if(Back.Count == 0)
            {
                Echo("Back");
                return;
            }
            if(Left.Count == 0)
            {
                Echo("Left");
                return;
            }
            if(Right.Count == 0)
            {
                Echo("Right");
                return;
            }    
            if(Up.Count == 0)
            {
                Echo("Up");
                return;
            }
            if(Down.Count == 0)
            {
                Echo("Down");
                return;
            }

            ComponentsCheck = true;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            return;
        }
        void ControlEngine()
        {
            AutoBreak();
            Move();
            Rotate();
        }
        void AutoBreak()
        {
            MatrixD matrix = MatrixD.CreateLookAt(new Vector3D(), MainController.WorldMatrix.Forward, MainController.WorldMatrix.Up);
            Vector3D angle = Vector3D.TransformNormal(MainController.GetShipVelocities().LinearVelocity, matrix);
            Echo(MainController.GetShipVelocities().LinearVelocity.ToString());
            foreach (IMyGravityGeneratorBase item in Left)
            {
                item.GravityAcceleration = 98f * (float)angle.X;
            }
            foreach (IMyGravityGeneratorBase item in Right)
            {
                item.GravityAcceleration = -98f * (float)angle.X;
            }
            foreach (IMyGravityGeneratorBase item in Down)
            {
                item.GravityAcceleration = 98f * (float)angle.Y;
            }
            foreach (IMyGravityGeneratorBase item in Up)
            {
                item.GravityAcceleration = -98f * (float)angle.Y;
            }
            foreach (IMyGravityGeneratorBase item in Back)
            {
                item.GravityAcceleration = -98f * (float)angle.Z;
            }
            foreach (IMyGravityGeneratorBase item in Forward)
            {
                item.GravityAcceleration = 98f * (float)angle.Z;
            }
        }
        void Move()
        {
            Vector3D controllerMove = MainController.MoveIndicator;
            if(controllerMove.X!= 0)
            {
                foreach (IMyGravityGeneratorBase item in Left)
                {
                    item.GravityAcceleration = -9.8f * (float)controllerMove.X;
                }
                foreach (IMyGravityGeneratorBase item in Right)
                {
                    item.GravityAcceleration = 9.8f * (float)controllerMove.X;
                }
            }
            if(controllerMove.Y != 0)
            {
                foreach (IMyGravityGeneratorBase item in Up)
                {
                    item.GravityAcceleration = 9.8f * (float)controllerMove.Y;
                }
                foreach (IMyGravityGeneratorBase item in Down)
                {
                    item.GravityAcceleration = -9.8f * (float)controllerMove.Y;
                }
            }
            if(controllerMove.Z!=0)
            {
                foreach (IMyGravityGeneratorBase item in Forward)
                {
                    item.GravityAcceleration = -9.8f * (float)controllerMove.Z;
                }
                foreach (IMyGravityGeneratorBase item in Back)
                {
                    item.GravityAcceleration = 9.8f * (float)controllerMove.Z;
                }
            }
        }
        void Rotate()
        {
            Vector2 controllerRotate = MainController.RotationIndicator;
            float contollerRoll = MainController.RollIndicator;
            foreach (var gyro in Gyros)
            {
                gyro.SetValue("Yaw", controllerRotate.Y * -60f);
                gyro.SetValue("Pitch", controllerRotate.X * 60f);
                gyro.SetValue("Roll", contollerRoll * 60f);
            }
        }
    }
}
