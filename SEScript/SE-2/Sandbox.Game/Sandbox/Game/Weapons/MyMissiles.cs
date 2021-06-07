using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyMissiles : MySessionComponentBase
	{
		private struct MissileId
		{
			public SerializableDefinitionId WeaponDefinitionId;

			public SerializableDefinitionId AmmoMagazineId;
		}

		private static readonly Dictionary<MissileId, Queue<MyMissile>> m_missiles = new Dictionary<MissileId, Queue<MyMissile>>();

		private static readonly MyDynamicAABBTreeD m_missileTree = new MyDynamicAABBTreeD(Vector3D.One * 10.0, 10.0);

		public override Type[] Dependencies => new Type[1]
		{
			typeof(MyPhysics)
		};

		public override void LoadData()
		{
		}

		protected override void UnloadData()
		{
			foreach (KeyValuePair<MissileId, Queue<MyMissile>> missile in m_missiles)
			{
				while (missile.Value.Count > 0)
				{
					missile.Value.Dequeue().Close();
				}
			}
			m_missiles.Clear();
			m_missileTree.Clear();
		}

		public static void Add(MyObjectBuilder_Missile builder)
		{
			MissileId missileId = default(MissileId);
			missileId.AmmoMagazineId = builder.AmmoMagazineId;
			missileId.WeaponDefinitionId = builder.WeaponDefinitionId;
			MissileId key = missileId;
			if (m_missiles.TryGetValue(key, out Queue<MyMissile> value) && value.Count > 0)
			{
				MyMissile myMissile = value.Dequeue();
				myMissile.UpdateData(builder);
				myMissile.m_pruningProxyId = -1;
				MyEntities.Add(myMissile);
				RegisterMissile(myMissile);
			}
			else
			{
				MyEntities.CreateFromObjectBuilderParallel(builder, addToScene: true, delegate(MyEntity x)
				{
					MyMissile obj = x as MyMissile;
					obj.m_pruningProxyId = -1;
					RegisterMissile(obj);
				});
			}
		}

		public static void Remove(long entityId)
		{
			MyMissile myMissile = MyEntities.GetEntityById(entityId) as MyMissile;
			if (myMissile != null)
			{
				Return(myMissile);
			}
		}

		public static void Return(MyMissile missile)
		{
			if (missile.InScene)
			{
				MissileId missileId = default(MissileId);
				missileId.AmmoMagazineId = missile.AmmoMagazineId;
				missileId.WeaponDefinitionId = missile.WeaponDefinitionId;
				MissileId key = missileId;
				if (!m_missiles.TryGetValue(key, out Queue<MyMissile> value))
				{
					value = new Queue<MyMissile>();
					m_missiles.Add(key, value);
				}
				value.Enqueue(missile);
				MyEntities.Remove(missile);
				UnregisterMissile(missile);
			}
		}

		private static void RegisterMissile(MyMissile missile)
		{
			if (missile.m_pruningProxyId == -1)
			{
				BoundingSphereD sphere = new BoundingSphereD(missile.PositionComp.GetPosition(), 1.0);
				BoundingBoxD.CreateFromSphere(ref sphere, out BoundingBoxD result);
				missile.m_pruningProxyId = m_missileTree.AddProxy(ref result, missile, 0u);
			}
		}

		private static void UnregisterMissile(MyMissile missile)
		{
			if (missile.m_pruningProxyId != -1)
			{
				m_missileTree.RemoveProxy(missile.m_pruningProxyId);
				missile.m_pruningProxyId = -1;
			}
		}

		public static void OnMissileMoved(MyMissile missile, ref Vector3 velocity)
		{
			if (missile.m_pruningProxyId != -1)
			{
				BoundingSphereD sphere = new BoundingSphereD(missile.PositionComp.GetPosition(), 1.0);
				BoundingBoxD.CreateFromSphere(ref sphere, out BoundingBoxD result);
				m_missileTree.MoveProxy(missile.m_pruningProxyId, ref result, velocity);
			}
		}

		public static void GetAllMissilesInSphere(ref BoundingSphereD sphere, List<MyEntity> result)
		{
			m_missileTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
		}
	}
}
