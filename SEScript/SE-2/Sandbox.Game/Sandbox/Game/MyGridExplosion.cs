using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game
{
	public class MyGridExplosion
	{
		public struct MyRaycastDamageInfo
		{
			public float DamageRemaining;

			public float DistanceToExplosion;

			public MyRaycastDamageInfo(float damageRemaining, float distanceToExplosion)
			{
				DamageRemaining = damageRemaining;
				DistanceToExplosion = distanceToExplosion;
			}
		}

		public bool GridWasHit;

		public readonly HashSet<MyCubeGrid> AffectedCubeGrids = new HashSet<MyCubeGrid>();

		public readonly HashSet<MySlimBlock> AffectedCubeBlocks = new HashSet<MySlimBlock>();

		private Dictionary<MySlimBlock, float> m_damagedBlocks = new Dictionary<MySlimBlock, float>();

		private Dictionary<MySlimBlock, MyRaycastDamageInfo> m_damageRemaining = new Dictionary<MySlimBlock, MyRaycastDamageInfo>();

		private Stack<MySlimBlock> m_castBlocks = new Stack<MySlimBlock>();

		private BoundingSphereD m_explosion;

		private float m_explosionDamage;

		private int stackOverflowGuard;

		private const int MAX_PHYSICS_RECURSION_COUNT = 10;

		private List<Vector3I> m_cells = new List<Vector3I>();

		public Dictionary<MySlimBlock, float> DamagedBlocks => m_damagedBlocks;

		public Dictionary<MySlimBlock, MyRaycastDamageInfo> DamageRemaining => m_damageRemaining;

		public float Damage => m_explosionDamage;

		public BoundingSphereD Sphere => m_explosion;

		public void Init(BoundingSphereD explosion, float explosionDamage)
		{
			m_explosion = explosion;
			m_explosionDamage = explosionDamage;
			AffectedCubeBlocks.Clear();
			AffectedCubeGrids.Clear();
			m_damageRemaining.Clear();
			m_damagedBlocks.Clear();
			m_castBlocks.Clear();
		}

		public void ComputeDamagedBlocks()
		{
			foreach (MySlimBlock affectedCubeBlock in AffectedCubeBlocks)
			{
				m_castBlocks.Clear();
				MyRaycastDamageInfo value = CastDDA(affectedCubeBlock);
				while (m_castBlocks.Count > 0)
				{
					MySlimBlock mySlimBlock = m_castBlocks.Pop();
					if (mySlimBlock.FatBlock is MyWarhead)
					{
						m_damagedBlocks[mySlimBlock] = 1E+07f;
					}
					else
					{
						float num = (float)(mySlimBlock.WorldAABB.Center - m_explosion.Center).Length();
						if (value.DamageRemaining > 0f)
						{
							float num2 = MathHelper.Clamp(1f - (num - value.DistanceToExplosion) / ((float)m_explosion.Radius - value.DistanceToExplosion), 0f, 1f);
							if (num2 > 0f)
							{
								m_damagedBlocks.Add(mySlimBlock, value.DamageRemaining * num2 * mySlimBlock.DeformationRatio);
								value.DamageRemaining = Math.Max(0f, value.DamageRemaining * num2 - mySlimBlock.Integrity / mySlimBlock.DeformationRatio);
							}
							else
							{
								m_damagedBlocks.Add(mySlimBlock, value.DamageRemaining);
							}
						}
						else
						{
							value.DamageRemaining = 0f;
						}
						value.DistanceToExplosion = Math.Abs(num);
						m_damageRemaining.Add(mySlimBlock, value);
					}
				}
			}
		}

		public MyRaycastDamageInfo ComputeDamageForEntity(Vector3D worldPosition)
		{
			return new MyRaycastDamageInfo(m_explosionDamage, (float)(worldPosition - m_explosion.Center).Length());
		}

		private MyRaycastDamageInfo CastDDA(MySlimBlock cubeBlock)
		{
			if (m_damageRemaining.ContainsKey(cubeBlock))
			{
				return m_damageRemaining[cubeBlock];
			}
			stackOverflowGuard = 0;
			m_castBlocks.Push(cubeBlock);
			cubeBlock.ComputeWorldCenter(out Vector3D worldCenter);
			m_cells.Clear();
			cubeBlock.CubeGrid.RayCastCells(worldCenter, m_explosion.Center, m_cells);
			(m_explosion.Center - worldCenter).Normalize();
			foreach (Vector3I cell in m_cells)
			{
				Vector3D vector3D = Vector3D.Transform(cell * cubeBlock.CubeGrid.GridSize, cubeBlock.CubeGrid.WorldMatrix);
				_ = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_DDA_RAYCASTS;
				MySlimBlock cubeBlock2 = cubeBlock.CubeGrid.GetCubeBlock(cell);
				if (cubeBlock2 == null)
				{
					if (IsExplosionInsideCell(cell, cubeBlock.CubeGrid))
					{
						return new MyRaycastDamageInfo(m_explosionDamage, (float)(vector3D - m_explosion.Center).Length());
					}
					return CastPhysicsRay(vector3D);
				}
				if (cubeBlock2 != cubeBlock)
				{
					if (m_damageRemaining.ContainsKey(cubeBlock2))
					{
						return m_damageRemaining[cubeBlock2];
					}
					if (!m_castBlocks.Contains(cubeBlock2))
					{
						m_castBlocks.Push(cubeBlock2);
					}
				}
				else if (IsExplosionInsideCell(cell, cubeBlock.CubeGrid))
				{
					return new MyRaycastDamageInfo(m_explosionDamage, (float)(vector3D - m_explosion.Center).Length());
				}
			}
			return new MyRaycastDamageInfo(m_explosionDamage, (float)(worldCenter - m_explosion.Center).Length());
		}

		private bool IsExplosionInsideCell(Vector3I cell, MyCubeGrid cellGrid)
		{
			return cellGrid.WorldToGridInteger(m_explosion.Center) == cell;
		}

		private MyRaycastDamageInfo CastPhysicsRay(Vector3D fromWorldPos)
		{
			Vector3D position = Vector3D.Zero;
			IMyEntity myEntity = null;
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(fromWorldPos, m_explosion.Center, 29);
			if (hitInfo.HasValue)
			{
				myEntity = ((hitInfo.Value.HkHitInfo.Body.UserObject != null) ? ((MyPhysicsBody)hitInfo.Value.HkHitInfo.Body.UserObject).Entity : null);
				position = hitInfo.Value.Position;
			}
			Vector3D normal = m_explosion.Center - fromWorldPos;
			float distanceToExplosion = (float)normal.Normalize();
			MyCubeGrid myCubeGrid = myEntity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				MyCubeBlock myCubeBlock = myEntity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeGrid = myCubeBlock.CubeGrid;
				}
			}
			if (myCubeGrid != null)
			{
				Vector3D value = Vector3D.Transform(position, myCubeGrid.PositionComp.WorldMatrixNormalizedInv) * myCubeGrid.GridSizeR;
				Vector3D vector3D = Vector3D.TransformNormal(normal, myCubeGrid.PositionComp.WorldMatrixNormalizedInv) * 1.0 / 8.0;
				for (int i = 0; i < 5; i++)
				{
					Vector3I pos = Vector3I.Round(value);
					MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(pos);
					if (cubeBlock != null)
					{
						if (m_castBlocks.Contains(cubeBlock))
						{
							return new MyRaycastDamageInfo(0f, distanceToExplosion);
						}
						return CastDDA(cubeBlock);
					}
					value += vector3D;
				}
				position = Vector3D.Transform(value * myCubeGrid.GridSize, myCubeGrid.WorldMatrix);
				Vector3D min = Vector3D.Min(fromWorldPos, position);
				Vector3D max = Vector3D.Max(fromWorldPos, position);
				if (new BoundingBoxD(min, max).Contains(m_explosion.Center) == ContainmentType.Contains)
				{
					return new MyRaycastDamageInfo(m_explosionDamage, distanceToExplosion);
				}
				stackOverflowGuard++;
				if (stackOverflowGuard > 10)
				{
					_ = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS;
					return new MyRaycastDamageInfo(0f, distanceToExplosion);
				}
				_ = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS;
				return CastPhysicsRay(position);
			}
			if (hitInfo.HasValue)
			{
				_ = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS;
				return new MyRaycastDamageInfo(0f, distanceToExplosion);
			}
			return new MyRaycastDamageInfo(m_explosionDamage, distanceToExplosion);
		}

		[Conditional("DEBUG")]
		private void DrawRay(Vector3D from, Vector3D to, float damage, bool depthRead = true)
		{
			if (!(damage > 0f))
			{
				_ = Color.Blue;
			}
			else
			{
				Color.Lerp(Color.Green, Color.Red, damage / m_explosionDamage);
			}
		}

		[Conditional("DEBUG")]
		private void DrawRay(Vector3D from, Vector3D to, Color color, bool depthRead = true)
		{
			if (MyAlexDebugInputComponent.Static != null)
			{
				MyAlexDebugInputComponent.Static.AddDebugLine(new MyAlexDebugInputComponent.LineInfo(from, to, color, depthRead: false));
			}
		}
	}
}
