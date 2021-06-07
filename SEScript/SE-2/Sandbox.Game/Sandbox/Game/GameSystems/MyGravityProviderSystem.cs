using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666)]
	public class MyGravityProviderSystem : MySessionComponentBase
	{
		private class GravityCollector
		{
			public Vector3 Gravity;

			private readonly Func<int, bool> CollectAction;

			private Vector3D WorldPoint;

			private MyDynamicAABBTreeD Tree;

			public GravityCollector()
			{
				CollectAction = CollectCallback;
			}

			public void Collect(MyDynamicAABBTreeD tree, ref Vector3D worldPoint)
			{
				Tree = tree;
				WorldPoint = worldPoint;
				tree.QueryPoint(CollectAction, ref worldPoint);
			}

			private bool CollectCallback(int proxyId)
			{
				IMyGravityProvider userData = Tree.GetUserData<IMyGravityProvider>(proxyId);
				if (userData.IsWorking && userData.IsPositionInRange(WorldPoint))
				{
					Gravity += userData.GetWorldGravity(WorldPoint);
				}
				return true;
			}
		}

		public const float G = 9.81f;

		private static Dictionary<IMyGravityProvider, int> m_proxyIdMap = new Dictionary<IMyGravityProvider, int>();

		private static MyDynamicAABBTreeD m_artificialGravityGenerators = new MyDynamicAABBTreeD(Vector3D.One * 10.0, 10.0);

		private static ConcurrentCachingList<IMyGravityProvider> m_naturalGravityGenerators = new ConcurrentCachingList<IMyGravityProvider>();

		[ThreadStatic]
		private static GravityCollector m_gravityCollector;

		protected override void UnloadData()
		{
			base.UnloadData();
			m_naturalGravityGenerators.ApplyChanges();
			if (m_proxyIdMap.Count <= 0)
			{
				_ = m_naturalGravityGenerators.Count;
				_ = 0;
			}
			m_proxyIdMap.Clear();
			m_artificialGravityGenerators.Clear();
			m_naturalGravityGenerators.ClearImmediate();
		}

		public static bool IsGravityReady()
		{
			return !m_artificialGravityGenerators.IsRootNull();
		}

		public static Vector3 CalculateTotalGravityInPoint(Vector3D worldPoint)
		{
			return CalculateTotalGravityInPoint(worldPoint, clearVectors: true);
		}

		public static Vector3 CalculateTotalGravityInPoint(Vector3D worldPoint, bool clearVectors)
		{
			_ = Vector3.Zero;
			float naturalGravityMultiplier;
			Vector3 value = CalculateNaturalGravityInPoint(worldPoint, out naturalGravityMultiplier);
			Vector3 value2 = CalculateArtificialGravityInPoint(worldPoint, CalculateArtificialGravityStrengthMultiplier(naturalGravityMultiplier));
			return value + value2;
		}

		public static Vector3 CalculateArtificialGravityInPoint(Vector3D worldPoint, float gravityMultiplier = 1f)
		{
			if (gravityMultiplier == 0f)
			{
				return Vector3.Zero;
			}
			if (m_gravityCollector == null)
			{
				m_gravityCollector = new GravityCollector();
			}
			m_gravityCollector.Gravity = Vector3.Zero;
			m_gravityCollector.Collect(m_artificialGravityGenerators, ref worldPoint);
			return m_gravityCollector.Gravity * gravityMultiplier;
		}

		public static Vector3 CalculateNaturalGravityInPoint(Vector3D worldPoint)
		{
			float naturalGravityMultiplier;
			return CalculateNaturalGravityInPoint(worldPoint, out naturalGravityMultiplier);
		}

		public static Vector3 CalculateNaturalGravityInPoint(Vector3D worldPoint, out float naturalGravityMultiplier)
		{
			naturalGravityMultiplier = 0f;
			Vector3 zero = Vector3.Zero;
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				if (naturalGravityGenerator.IsPositionInRange(worldPoint))
				{
					Vector3 worldGravity = naturalGravityGenerator.GetWorldGravity(worldPoint);
					float gravityMultiplier = naturalGravityGenerator.GetGravityMultiplier(worldPoint);
					if (gravityMultiplier > naturalGravityMultiplier)
					{
						naturalGravityMultiplier = gravityMultiplier;
					}
					zero += worldGravity;
				}
			}
			return zero;
		}

		public static float CalculateHighestNaturalGravityMultiplierInPoint(Vector3D worldPoint)
		{
			float num = 0f;
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				if (naturalGravityGenerator.IsPositionInRange(worldPoint))
				{
					float gravityMultiplier = naturalGravityGenerator.GetGravityMultiplier(worldPoint);
					if (gravityMultiplier > num)
					{
						num = gravityMultiplier;
					}
				}
			}
			return num;
		}

		public static float CalculateArtificialGravityStrengthMultiplier(float naturalGravityMultiplier)
		{
			return MathHelper.Clamp(1f - naturalGravityMultiplier * 2f, 0f, 1f);
		}

		public static double GetStrongestNaturalGravityWell(Vector3D worldPosition, out IMyGravityProvider nearestProvider)
		{
			double num = double.MinValue;
			nearestProvider = null;
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				float num2 = naturalGravityGenerator.GetWorldGravity(worldPosition).Length();
				if ((double)num2 > num)
				{
					num = num2;
					nearestProvider = naturalGravityGenerator;
				}
			}
			return num;
		}

		public static bool IsPositionInNaturalGravity(Vector3D position, double sphereSize = 0.0)
		{
			sphereSize = MathHelper.Max(sphereSize, 0.0);
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				if (naturalGravityGenerator != null && naturalGravityGenerator.IsPositionInRange(position))
				{
					return true;
				}
			}
			return false;
		}

		public static bool DoesTrajectoryIntersectNaturalGravity(Vector3D start, Vector3D end, double raySize = 0.0)
		{
			Vector3D value = start - end;
			if (Vector3D.IsZero(value))
			{
				return IsPositionInNaturalGravity(start, raySize);
			}
			Ray ray = new Ray(start, Vector3.Normalize(value));
			raySize = MathHelper.Max(raySize, 0.0);
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				if (naturalGravityGenerator != null)
				{
					MySphericalNaturalGravityComponent mySphericalNaturalGravityComponent = naturalGravityGenerator as MySphericalNaturalGravityComponent;
					if (mySphericalNaturalGravityComponent != null)
					{
						BoundingSphereD b = new BoundingSphereD(mySphericalNaturalGravityComponent.Position, (double)mySphericalNaturalGravityComponent.GravityLimit + raySize);
						if (ray.Intersects(b).HasValue)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public static void AddGravityGenerator(IMyGravityProvider gravityGenerator)
		{
			if (!m_proxyIdMap.ContainsKey(gravityGenerator))
			{
				gravityGenerator.GetProxyAABB(out BoundingBoxD aabb);
				int value = m_artificialGravityGenerators.AddProxy(ref aabb, gravityGenerator, 0u);
				m_proxyIdMap.Add(gravityGenerator, value);
			}
		}

		public static void RemoveGravityGenerator(IMyGravityProvider gravityGenerator)
		{
			if (m_proxyIdMap.TryGetValue(gravityGenerator, out int value))
			{
				m_artificialGravityGenerators.RemoveProxy(value);
				m_proxyIdMap.Remove(gravityGenerator);
			}
		}

		public static void OnGravityGeneratorMoved(IMyGravityProvider gravityGenerator, ref Vector3 velocity)
		{
			if (m_proxyIdMap.TryGetValue(gravityGenerator, out int value))
			{
				gravityGenerator.GetProxyAABB(out BoundingBoxD aabb);
				m_artificialGravityGenerators.MoveProxy(value, ref aabb, velocity);
			}
		}

		public static void AddNaturalGravityProvider(IMyGravityProvider gravityGenerator)
		{
			m_naturalGravityGenerators.Add(gravityGenerator);
		}

		public static void RemoveNaturalGravityProvider(IMyGravityProvider gravityGenerator)
		{
			m_naturalGravityGenerators.Remove(gravityGenerator);
		}
	}
}
