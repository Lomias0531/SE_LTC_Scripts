using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;

namespace SEScript
{
    class LTC_Veil:API
    {
        bool CheckReady = false;
        List<IMyGravityGeneratorSphere> GravityRestraint_In;
        List<IMyGravityGeneratorSphere> GravityRestraint_Out;
        List<IMyGravityGenerator> GravityControls;
        IMyGravityGenerator NormalGravity;
        List<IMyShipConnector> StoneSpitter;
        List<IMyCargoContainer> Cargos;
        void Main()
        {
            if(!CheckReady)
            {
                CheckComponents();
                return;
            }
        }
        void CheckComponents()
        {
            GravityRestraint_In = new List<IMyGravityGeneratorSphere>();
            GravityRestraint_Out = new List<IMyGravityGeneratorSphere>();
            GravityControls = new List<IMyGravityGenerator>();
            StoneSpitter = new List<IMyShipConnector>();
            Cargos = new List<IMyCargoContainer>();
        }
    }
}
