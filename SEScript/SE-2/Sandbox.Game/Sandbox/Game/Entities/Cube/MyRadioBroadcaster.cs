using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities.Cube
{
	public class MyRadioBroadcaster : MyDataBroadcaster
	{
		protected sealed class ChangeBroadcastRadius_003C_003ESystem_Single : ICallSite<MyRadioBroadcaster, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRadioBroadcaster @this, in float newRadius, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ChangeBroadcastRadius(newRadius);
			}
		}

		private class Sandbox_Game_Entities_Cube_MyRadioBroadcaster_003C_003EActor
		{
		}

		public Action OnBroadcastRadiusChanged;

		private float m_broadcastRadius;

		private bool m_enabled;

		public bool WantsToBeEnabled = true;

		public int m_radioProxyID = -1;

		private bool m_registered;

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled == value)
				{
					return;
				}
				if (!IsProjection())
				{
					if (value)
					{
						MyRadioBroadcasters.AddBroadcaster(this);
					}
					else
					{
						MyRadioBroadcasters.RemoveBroadcaster(this);
					}
				}
				m_enabled = value;
			}
		}

		public float BroadcastRadius
		{
			get
			{
				return m_broadcastRadius;
			}
			set
			{
				if (m_broadcastRadius != value)
				{
					m_broadcastRadius = value;
					if (m_enabled)
					{
						MyRadioBroadcasters.RemoveBroadcaster(this);
						MyRadioBroadcasters.AddBroadcaster(this);
					}
					OnBroadcastRadiusChanged?.Invoke();
				}
			}
		}

		public int RadioProxyID
		{
			get
			{
				return m_radioProxyID;
			}
			set
			{
				m_radioProxyID = value;
			}
		}

		public MyRadioBroadcaster(float broadcastRadius = 100f)
		{
			m_broadcastRadius = broadcastRadius;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			Enabled = false;
		}

		public void MoveBroadcaster()
		{
			MyRadioBroadcasters.MoveBroadcaster(this);
		}

		private bool IsProjection()
		{
			MyCubeBlock myCubeBlock = base.Entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				return myCubeBlock.CubeGrid.Physics == null;
			}
			return false;
		}

		public override void InitProxyObjectBuilder(MyObjectBuilder_ProxyAntenna ob)
		{
			base.InitProxyObjectBuilder(ob);
			ob.IsLaser = false;
			ob.BroadcastRadius = BroadcastRadius;
		}

		public void RaiseBroadcastRadiusChanged()
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyRadioBroadcaster x) => x.ChangeBroadcastRadius, BroadcastRadius);
			}
		}

		[Event(null, 118)]
		[Reliable]
		[Broadcast]
		public void ChangeBroadcastRadius(float newRadius)
		{
			BroadcastRadius = newRadius;
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			if (base.Entity.GetTopMostParent().Physics != null)
			{
				m_registered = true;
				MyAntennaSystem.Static.RegisterAntenna(this);
			}
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
			if (m_registered)
			{
				MyAntennaSystem.Static.UnregisterAntenna(this);
			}
			m_registered = false;
		}
	}
}
