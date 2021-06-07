using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Generics;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MyExplosions : MySessionComponentBase
	{
		protected sealed class ProxyExplosionRequest_003C_003ESandbox_Game_MyExplosionInfoSimplified : ICallSite<IMyEventOwner, MyExplosionInfoSimplified, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyExplosionInfoSimplified explosionInfo, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ProxyExplosionRequest(explosionInfo);
			}
		}

		private static MyObjectsPool<MyExplosion> m_explosions;

		private static List<MyExplosionInfo> m_explosionBuffer1;

		private static List<MyExplosionInfo> m_explosionBuffer2;

		private static List<MyExplosionInfo> m_explosionsRead;

		private static List<MyExplosionInfo> m_explosionsWrite;

		private static List<MyExplosion> m_exploded;

		private static HashSet<long> m_activeEntityKickbacks;

		private static SortedDictionary<long, long> m_activeEntityKickbacksByTime;

		public override Type[] Dependencies => new Type[1]
		{
			typeof(MyLights)
		};

		static MyExplosions()
		{
			m_explosions = null;
			m_explosionBuffer1 = new List<MyExplosionInfo>();
			m_explosionBuffer2 = new List<MyExplosionInfo>();
			m_explosionsRead = m_explosionBuffer1;
			m_explosionsWrite = m_explosionBuffer2;
			m_exploded = new List<MyExplosion>();
			m_activeEntityKickbacks = new HashSet<long>();
			m_activeEntityKickbacksByTime = new SortedDictionary<long, long>();
		}

		public override void LoadData()
		{
			MySandboxGame.Log.WriteLine("MyExplosions.LoadData() - START");
			MySandboxGame.Log.IncreaseIndent();
			if (m_explosions == null)
			{
				m_explosions = new MyObjectsPool<MyExplosion>(1024);
			}
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyExplosions.LoadData() - END");
		}

		protected override void UnloadData()
		{
			if (m_explosions != null && m_explosions.ActiveCount > 0)
			{
				foreach (MyExplosion item in m_explosions.Active)
				{
					item?.Close();
				}
				m_explosions.DeallocateAll();
			}
			m_explosionsRead.Clear();
			m_explosionsWrite.Clear();
			m_activeEntityKickbacks.Clear();
			m_activeEntityKickbacksByTime.Clear();
		}

		public static void AddExplosion(ref MyExplosionInfo explosionInfo, bool updateSync = true)
		{
			if (MySessionComponentSafeZones.IsActionAllowed(BoundingBoxD.CreateFromSphere(explosionInfo.ExplosionSphere), MySafeZoneAction.Damage, 0L, 0uL))
			{
				if (Sync.IsServer && updateSync)
				{
					MyExplosionInfoSimplified arg = default(MyExplosionInfoSimplified);
					arg.Damage = explosionInfo.Damage;
					arg.Center = explosionInfo.ExplosionSphere.Center;
					arg.Radius = (float)explosionInfo.ExplosionSphere.Radius;
					arg.Type = explosionInfo.ExplosionType;
					arg.Flags = explosionInfo.ExplosionFlags;
					arg.VoxelCenter = explosionInfo.VoxelExplosionCenter;
					arg.ParticleScale = explosionInfo.ParticleScale;
					arg.Velocity = explosionInfo.Velocity;
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ProxyExplosionRequest, arg, default(EndpointId), explosionInfo.ExplosionSphere.Center);
				}
				m_explosionsWrite.Add(explosionInfo);
			}
		}

		public override void UpdateBeforeSimulation()
		{
			SwapBuffers();
			UpdateEntityKickbacks();
			foreach (MyExplosionInfo item2 in m_explosionsRead)
			{
				MyExplosion item = null;
				m_explosions.AllocateOrCreate(out item);
				item?.Start(item2);
			}
			m_explosionsRead.Clear();
			foreach (MyExplosion item3 in m_explosions.Active)
			{
				if (!item3.Update())
				{
					m_exploded.Add(item3);
					m_explosions.MarkForDeallocate(item3);
				}
				else
				{
					m_exploded.Add(item3);
				}
			}
			foreach (MyExplosion item4 in m_exploded)
			{
				item4.ApplyVolumetricDamageToGrid();
				item4.Clear();
			}
			m_exploded.Clear();
			m_explosions.DeallocateAllMarked();
			MyDebris.Static.UpdateBeforeSimulation();
		}

		[Event(null, 167)]
		[Reliable]
		[ServerInvoked]
		[BroadcastExcept]
		private static void ProxyExplosionRequest(MyExplosionInfoSimplified explosionInfo)
		{
			if (MySession.Static.Ready && !MyEventContext.Current.IsLocallyInvoked)
			{
				MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
				myExplosionInfo.PlayerDamage = 0f;
				myExplosionInfo.Damage = explosionInfo.Damage;
				myExplosionInfo.ExplosionType = explosionInfo.Type;
				myExplosionInfo.ExplosionSphere = new BoundingSphereD(explosionInfo.Center, explosionInfo.Radius);
				myExplosionInfo.LifespanMiliseconds = 700;
				myExplosionInfo.HitEntity = null;
				myExplosionInfo.ParticleScale = explosionInfo.ParticleScale;
				myExplosionInfo.OwnerEntity = null;
				myExplosionInfo.Direction = Vector3.Forward;
				myExplosionInfo.VoxelExplosionCenter = explosionInfo.VoxelCenter;
				myExplosionInfo.ExplosionFlags = explosionInfo.Flags;
				myExplosionInfo.VoxelCutoutScale = 1f;
				myExplosionInfo.PlaySound = true;
				myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
				myExplosionInfo.Velocity = explosionInfo.Velocity;
				MyExplosionInfo explosionInfo2 = myExplosionInfo;
				AddExplosion(ref explosionInfo2, updateSync: false);
			}
		}

		private void SwapBuffers()
		{
			if (m_explosionBuffer1 == m_explosionsRead)
			{
				m_explosionsWrite = m_explosionBuffer1;
				m_explosionsRead = m_explosionBuffer2;
			}
			else
			{
				m_explosionsWrite = m_explosionBuffer2;
				m_explosionsRead = m_explosionBuffer1;
			}
		}

		public override void Draw()
		{
			foreach (MyExplosion item in m_explosions.Active)
			{
				item.DebugDraw();
			}
		}

		public static bool ShouldUseMassScaleForEntity(MyEntity entity)
		{
			long entityId = entity.EntityId;
			if (m_activeEntityKickbacks.Contains(entityId))
			{
				return false;
			}
			long num;
			for (num = (MySession.Static.ElapsedGameTime + TimeSpan.FromSeconds(2.0)).Ticks + entityId % 100; m_activeEntityKickbacksByTime.ContainsKey(num); num++)
			{
			}
			m_activeEntityKickbacks.Add(entityId);
			m_activeEntityKickbacksByTime.Add(num, entityId);
			return true;
		}

		private void UpdateEntityKickbacks()
		{
			long ticks = MySession.Static.ElapsedGameTime.Ticks;
			while (m_activeEntityKickbacksByTime.Count != 0)
			{
				using (SortedDictionary<long, long>.Enumerator enumerator = m_activeEntityKickbacksByTime.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						KeyValuePair<long, long> current = enumerator.Current;
						if (current.Key > ticks)
						{
							return;
						}
						long value = current.Value;
						m_activeEntityKickbacks.Remove(value);
						m_activeEntityKickbacksByTime.Remove(current.Key);
					}
				}
			}
		}
	}
}
