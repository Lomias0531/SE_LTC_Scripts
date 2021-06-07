using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
	public class MyLaserReceiver : MyDataReceiver
	{
		private class Sandbox_Game_Entities_Blocks_MyLaserReceiver_003C_003EActor : IActivator, IActivator<MyLaserReceiver>
		{
			private sealed override object CreateInstance()
			{
				return new MyLaserReceiver();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLaserReceiver CreateInstance()
			{
				return new MyLaserReceiver();
			}

			MyLaserReceiver IActivator<MyLaserReceiver>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void GetBroadcastersInMyRange(ref HashSet<MyDataBroadcaster> broadcastersInRange)
		{
			foreach (MyLaserBroadcaster value in MyAntennaSystem.Static.LaserAntennas.Values)
			{
				if (value != base.Broadcaster && (value.RealAntenna == null || (value.RealAntenna.Enabled && value.RealAntenna.IsFunctional && value.RealAntenna.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f)) && value.SuccessfullyContacting == base.Broadcaster.AntennaEntityId)
				{
					broadcastersInRange.Add(value);
				}
			}
			MyAntennaSystem.Static.GetEntityBroadcasters(base.Entity as MyEntity, ref broadcastersInRange, 0L);
		}
	}
}
