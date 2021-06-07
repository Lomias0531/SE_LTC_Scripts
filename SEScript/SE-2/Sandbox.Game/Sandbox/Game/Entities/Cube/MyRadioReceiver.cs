using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyRadioReceiver : MyDataReceiver
	{
		private class Sandbox_Game_Entities_Cube_MyRadioReceiver_003C_003EActor : IActivator, IActivator<MyRadioReceiver>
		{
			private sealed override object CreateInstance()
			{
				return new MyRadioReceiver();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRadioReceiver CreateInstance()
			{
				return new MyRadioReceiver();
			}

			MyRadioReceiver IActivator<MyRadioReceiver>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			base.Enabled = true;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			base.Enabled = false;
		}

		protected override void GetBroadcastersInMyRange(ref HashSet<MyDataBroadcaster> broadcastersInRange)
		{
			m_tmpBroadcasters.Clear();
			MyRadioBroadcasters.GetAllBroadcastersInSphere(new BoundingSphereD(base.Entity.PositionComp.GetPosition(), 0.5), m_tmpBroadcasters);
			foreach (MyDataBroadcaster tmpBroadcaster in m_tmpBroadcasters)
			{
				broadcastersInRange.Add(tmpBroadcaster);
			}
			MyAntennaSystem.Static.GetEntityBroadcasters(base.Entity as MyEntity, ref broadcastersInRange, 0L);
		}
	}
}
