using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment.Modules
{
	public class MyBreakableEnvironmentProxy : IMyEnvironmentModuleProxy
	{
		private struct BreakAtData
		{
			public readonly int itemId;

			public readonly Vector3D hitpos;

			public readonly Vector3D hitnormal;

			public readonly double impactEnergy;

			public BreakAtData(int itemId, Vector3D hitpos, Vector3D hitnormal, double impactEnergy)
			{
				this.itemId = itemId;
				this.hitpos = hitpos;
				this.hitnormal = hitnormal;
				this.impactEnergy = impactEnergy;
			}
		}

		[Serializable]
		private struct Impact
		{
			protected class Sandbox_Game_WorldEnvironment_Modules_MyBreakableEnvironmentProxy_003C_003EImpact_003C_003EPosition_003C_003EAccessor : IMemberAccessor<Impact, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Impact owner, in Vector3D value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Impact owner, out Vector3D value)
				{
					value = owner.Position;
				}
			}

			protected class Sandbox_Game_WorldEnvironment_Modules_MyBreakableEnvironmentProxy_003C_003EImpact_003C_003ENormal_003C_003EAccessor : IMemberAccessor<Impact, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Impact owner, in Vector3D value)
				{
					owner.Normal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Impact owner, out Vector3D value)
				{
					value = owner.Normal;
				}
			}

			protected class Sandbox_Game_WorldEnvironment_Modules_MyBreakableEnvironmentProxy_003C_003EImpact_003C_003EEnergy_003C_003EAccessor : IMemberAccessor<Impact, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Impact owner, in double value)
				{
					owner.Energy = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Impact owner, out double value)
				{
					value = owner.Energy;
				}
			}

			public Vector3D Position;

			public Vector3D Normal;

			public double Energy;

			public Impact(Vector3D position, Vector3D normal, double energy)
			{
				Position = position;
				Normal = normal;
				Energy = energy;
			}
		}

		private const int BrokenItemLifeSpan = 20000;

		private int m_scheduledBreaksCount;

		private ConcurrentDictionary<int, BreakAtData> m_scheduledBreaks = new ConcurrentDictionary<int, BreakAtData>();

		private readonly Action m_BreakAtDelegate;

		private MyEnvironmentSector m_sector;

		public long SectorId => m_sector.SectorId;

		public MyBreakableEnvironmentProxy()
		{
			m_BreakAtDelegate = BreakAtInvoke;
		}

		public void Init(MyEnvironmentSector sector, List<int> items)
		{
			m_sector = sector;
			if (Sync.IsServer)
			{
				m_sector.OnContactPoint += SectorOnContactPoint;
			}
		}

		private void SectorOnContactPoint(int itemId, MyEntity other, ref MyPhysics.MyContactPointEvent e)
		{
			if (m_sector.DataView.Items[itemId].ModelIndex < 0)
			{
				return;
			}
			float num = Math.Abs(e.ContactPointEvent.SeparatingVelocity);
			if (other == null || other.Physics == null || other is MyFloatingObject || other is IMyHandheldGunObject<MyDeviceBase> || (other.Physics.RigidBody != null && other.Physics.RigidBody.Layer == 20))
			{
				return;
			}
			MyCubeGrid myCubeGrid = other as MyCubeGrid;
			float num2 = (myCubeGrid == null) ? MyDestructionHelper.MassFromHavok(other.Physics.Mass) : MyGridPhysicalGroupData.GetGroupSharedProperties(myCubeGrid, checkMultithreading: false).Mass;
			double num3 = num * num * num2;
			if (num3 > ItemResilience(itemId))
			{
				int num4 = Interlocked.Increment(ref m_scheduledBreaksCount);
				Vector3D position = e.Position;
				Vector3 normal = e.ContactPointEvent.ContactPoint.Normal;
				if (m_scheduledBreaks.TryAdd(itemId, new BreakAtData(itemId, position, normal, num3)) && num4 == 1)
				{
					MySandboxGame.Static.Invoke(m_BreakAtDelegate, "MyBreakableEnvironmentProxy::BreakAt");
				}
			}
			if (other is MyMeteor)
			{
				m_sector.EnableItem(itemId, enabled: false);
			}
		}

		private void BreakAtInvoke()
		{
			foreach (BreakAtData value in m_scheduledBreaks.Values)
			{
				BreakAt(value.itemId, value.hitpos, value.hitnormal, value.impactEnergy);
			}
			m_scheduledBreaks.Clear();
			m_scheduledBreaksCount = 0;
		}

		public void BreakAt(int itemId, Vector3D hitpos, Vector3D hitnormal, double impactEnergy)
		{
			impactEnergy = MathHelper.Clamp(impactEnergy, 0.0, ItemResilience(itemId) * 10.0);
			Impact imp = new Impact(hitpos, hitnormal, impactEnergy);
			m_sector.RaiseItemEvent(this, itemId, imp);
			DisableItemAndCreateDebris(ref imp, itemId);
		}

		private void DisableItemAndCreateDebris(ref Impact imp, int itemId)
		{
			if (m_sector.GetModelIndex(itemId) < 0)
			{
				return;
			}
			MyParticlesManager.TryCreateParticleEffect("Tree Destruction", MatrixD.CreateTranslation(imp.Position), out MyParticleEffect _);
			if (m_sector.LodLevel <= 1)
			{
				MyEntity myEntity = CreateDebris(itemId);
				if (myEntity != null)
				{
					float mass = myEntity.Physics.Mass;
					Vector3D v = (float)Math.Sqrt(imp.Energy / (double)mass) / (0.0166666675f * MyFakes.SIMULATION_SPEED) * 0.8f * imp.Normal;
					Vector3D value = myEntity.Physics.CenterOfMassWorld + myEntity.WorldMatrix.Up * (imp.Position - myEntity.Physics.CenterOfMassWorld).Length();
					myEntity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, v, value, null);
				}
			}
			m_sector.EnableItem(itemId, enabled: false);
		}

		private MyEntity CreateDebris(int itemId)
		{
			ItemInfo itemInfo = m_sector.DataView.Items[itemId];
			Vector3D value = itemInfo.Position + m_sector.SectorCenter;
			MyPhysicalModelDefinition modelForId = m_sector.Owner.GetModelForId(itemInfo.ModelIndex);
			string text = modelForId.Model.Insert(modelForId.Model.Length - 4, "_broken");
			bool flag = false;
			string model = modelForId.Model;
			if (MyModels.GetModelOnlyData(text) != null)
			{
				flag = true;
				model = text;
			}
			MyEntity myEntity = MyDebris.Static.CreateTreeDebris(model);
			MyDebrisBase.MyDebrisBaseLogic obj = (MyDebrisBase.MyDebrisBaseLogic)myEntity.GameLogic;
			obj.LifespanInMiliseconds = 20000;
			MatrixD position = MatrixD.CreateFromQuaternion(itemInfo.Rotation);
			position.Translation = value + position.Up * ((!flag) ? 5 : 0);
			obj.Start(position, Vector3.Zero, randomRotation: false);
			return myEntity;
		}

		private double ItemResilience(int itemId)
		{
			return 200000.0;
		}

		public void Close()
		{
			m_sector.OnContactPoint -= SectorOnContactPoint;
		}

		public void CommitLodChange(int lodBefore, int lodAfter)
		{
		}

		public void CommitPhysicsChange(bool enabled)
		{
		}

		public void OnItemChange(int item, short newModel)
		{
		}

		public void OnItemChangeBatch(List<int> items, int offset, short newModel)
		{
		}

		public void HandleSyncEvent(int item, object data, bool fromClient)
		{
			Impact imp = (Impact)data;
			DisableItemAndCreateDebris(ref imp, item);
		}

		public void DebugDraw()
		{
		}
	}
}
