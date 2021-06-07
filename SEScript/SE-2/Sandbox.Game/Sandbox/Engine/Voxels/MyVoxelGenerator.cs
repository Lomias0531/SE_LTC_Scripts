using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library;
using VRage.ModAPI;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public static class MyVoxelGenerator
	{
		internal struct CutOutSphere : IVoxelOperator
		{
			public float RadSq;

			public Vector3D Center;

			public bool Changed;

			public VoxelOperatorFlags Flags => VoxelOperatorFlags.ReadWrite;

			public void Op(ref Vector3I pos, MyStorageDataTypeEnum dataType, ref byte content)
			{
				if (content != 0)
				{
					Vector3D value = pos;
					if (Vector3D.DistanceSquared(Center, value) < (double)RadSq)
					{
						Changed = true;
						content = 0;
					}
				}
			}
		}

		private struct ClampingInfo
		{
			public bool X;

			public bool Y;

			public bool Z;

			public ClampingInfo(bool X, bool Y, bool Z)
			{
				this.X = X;
				this.Y = Y;
				this.Z = Z;
			}
		}

		private const int CELL_SIZE = 16;

		private const int VOXEL_CLAMP_BORDER_DISTANCE = 2;

		[ThreadStatic]
		private static MyStorageData m_cache;

		private static readonly List<MyEntity> m_overlapList = new List<MyEntity>();

		public static void MakeCrater(MyVoxelBase voxelMap, BoundingSphereD sphere, Vector3 direction, MyVoxelMaterialDefinition material)
		{
			try
			{
				MakeCraterInternal(voxelMap, ref sphere, ref direction, material);
			}
			catch (NullReferenceException arg)
			{
				MyLog.Default.Error("NRE while creating asteroid crater." + MyEnvironment.NewLine + arg);
			}
		}

		private static void MakeCraterInternal(MyVoxelBase voxelMap, ref BoundingSphereD sphere, ref Vector3 direction, MyVoxelMaterialDefinition material)
		{
			Vector3 vector = Vector3.Normalize(sphere.Center - voxelMap.RootVoxel.WorldMatrix.Translation);
			Vector3D worldPosition = sphere.Center - (sphere.Radius - 1.0) * 1.2999999523162842;
			Vector3D worldPosition2 = sphere.Center + (sphere.Radius + 1.0) * 1.2999999523162842;
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(voxelMap.PositionLeftBottomCorner, ref worldPosition, out Vector3I voxelCoord);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(voxelMap.PositionLeftBottomCorner, ref worldPosition2, out Vector3I voxelCoord2);
			voxelMap.Storage.ClampVoxelCoord(ref voxelCoord);
			voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
			Vector3I lodVoxelRangeMin = voxelCoord + voxelMap.StorageMin;
			Vector3I lodVoxelRangeMax = voxelCoord2 + voxelMap.StorageMin;
			bool flag = false;
			if (m_cache == null)
			{
				m_cache = new MyStorageData();
			}
			m_cache.Resize(voxelCoord, voxelCoord2);
			MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
			voxelMap.Storage.ReadRange(m_cache, MyStorageDataTypeFlags.ContentAndMaterial, 0, lodVoxelRangeMin, lodVoxelRangeMax, ref requestFlags);
			int num = 0;
			Vector3I p = (voxelCoord2 - voxelCoord) / 2;
			byte materialIdx = m_cache.Material(ref p);
			float num2 = 1f - Vector3.Dot(vector, direction);
			Vector3 vector2 = sphere.Center - vector * (float)sphere.Radius * 1.1f;
			float num3 = (float)(sphere.Radius * 1.5);
			float num4 = num3 * num3;
			float num5 = 0.5f * (2f * num3 + 0.5f);
			float num6 = 0.5f * (-2f * num3 + 0.5f);
			Vector3 value = vector2 + vector * (float)sphere.Radius * (0.7f + num2) + direction * (float)sphere.Radius * 0.65f;
			float num7 = (float)sphere.Radius;
			float num8 = num7 * num7;
			float num9 = 0.5f * (2f * num7 + 0.5f);
			float num10 = 0.5f * (-2f * num7 + 0.5f);
			Vector3 value2 = vector2 + vector * (float)sphere.Radius * num2 + direction * (float)sphere.Radius * 0.3f;
			float num11 = (float)(sphere.Radius * 0.10000000149011612);
			float num12 = num11 * num11;
			float num13 = 0.5f * (2f * num11 + 0.5f);
			Vector3I voxelCoord3 = default(Vector3I);
			voxelCoord3.Z = voxelCoord.Z;
			p.Z = 0;
			while (voxelCoord3.Z <= voxelCoord2.Z)
			{
				voxelCoord3.Y = voxelCoord.Y;
				p.Y = 0;
				while (voxelCoord3.Y <= voxelCoord2.Y)
				{
					voxelCoord3.X = voxelCoord.X;
					p.X = 0;
					for (; voxelCoord3.X <= voxelCoord2.X; voxelCoord3.X++, p.X++)
					{
						MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionLeftBottomCorner, ref voxelCoord3, out Vector3D worldPosition3);
						byte b = m_cache.Content(ref p);
						if (b != byte.MaxValue)
						{
							float num14 = (float)(worldPosition3 - vector2).LengthSquared();
							float num15 = num14 - num4;
							byte b2;
							if (num15 > num5)
							{
								b2 = 0;
							}
							else if (num15 < num6)
							{
								b2 = byte.MaxValue;
							}
							else
							{
								float num16 = (float)Math.Sqrt((double)(num14 + num4) - (double)(2f * num3) * Math.Sqrt(num14));
								if (num15 < 0f)
								{
									num16 = 0f - num16;
								}
								b2 = (byte)(127f - num16 / 0.5f * 127f);
							}
							if (b2 > b)
							{
								if (material != null)
								{
									m_cache.Material(ref p, materialIdx);
								}
								flag = true;
								m_cache.Content(ref p, b2);
							}
						}
						float num17 = (float)(worldPosition3 - value).LengthSquared();
						float num18 = num17 - num8;
						byte b3;
						if (num18 > num9)
						{
							b3 = 0;
						}
						else if (num18 < num10)
						{
							b3 = byte.MaxValue;
						}
						else
						{
							float num19 = (float)Math.Sqrt((double)(num17 + num8) - (double)(2f * num7) * Math.Sqrt(num17));
							if (num18 < 0f)
							{
								num19 = 0f - num19;
							}
							b3 = (byte)(127f - num19 / 0.5f * 127f);
						}
						b = m_cache.Content(ref p);
						if (b > 0 && b3 > 0)
						{
							flag = true;
							int num20 = b - b3;
							if (num20 < 0)
							{
								num20 = 0;
							}
							m_cache.Content(ref p, (byte)num20);
							num += b - num20;
						}
						float num21 = (float)(worldPosition3 - value2).LengthSquared() - num12;
						if (num21 <= 1.5f)
						{
							MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(m_cache.Material(ref p));
							MyVoxelMaterialDefinition myVoxelMaterialDefinition = material;
							if (num21 > 0f)
							{
								byte b4 = m_cache.Content(ref p);
								if (b4 == byte.MaxValue)
								{
									myVoxelMaterialDefinition = voxelMaterialDefinition;
								}
								if (num21 >= num13 && b4 != 0)
								{
									myVoxelMaterialDefinition = voxelMaterialDefinition;
								}
							}
							if (voxelMaterialDefinition == myVoxelMaterialDefinition)
							{
								continue;
							}
							m_cache.Material(ref p, myVoxelMaterialDefinition.Index);
							flag = true;
						}
						if ((float)(worldPosition3 - vector2).LengthSquared() - num4 <= 0f)
						{
							b = m_cache.Content(ref p);
							if (b > 0 && m_cache.WrinkleVoxelContent(ref p, 0.5f, 0.45f))
							{
								flag = true;
							}
						}
					}
					voxelCoord3.Y++;
					p.Y++;
				}
				voxelCoord3.Z++;
				p.Z++;
			}
			if (flag)
			{
				RemoveSmallVoxelsUsingChachedVoxels();
				voxelCoord += voxelMap.StorageMin;
				voxelCoord2 += voxelMap.StorageMin;
				voxelMap.Storage.WriteRange(m_cache, MyStorageDataTypeFlags.ContentAndMaterial, voxelCoord, voxelCoord2);
				BoundingBoxD cutOutBox = new MyShapeSphere
				{
					Center = sphere.Center,
					Radius = (float)(sphere.Radius * 1.5)
				}.GetWorldBoundaries();
				NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, voxelMap, ref cutOutBox);
			}
		}

		public static void RequestPaintInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx)
		{
			MyVoxelBase myVoxelBase = voxelMap as MyVoxelBase;
			MyShape myShape = voxelShape as MyShape;
			if (myVoxelBase != null)
			{
				myShape?.SendPaintRequest(myVoxelBase, materialIdx);
			}
		}

		public static void RequestFillInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx)
		{
			MyVoxelBase myVoxelBase = voxelMap as MyVoxelBase;
			MyShape myShape = voxelShape as MyShape;
			if (myVoxelBase != null)
			{
				myShape?.SendFillRequest(myVoxelBase, materialIdx);
			}
		}

		public static void RequestRevertShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape)
		{
			MyVoxelBase myVoxelBase = voxelMap as MyVoxelBase;
			MyShape myShape = voxelShape as MyShape;
			if (myVoxelBase != null)
			{
				myShape?.SendRevertRequest(myVoxelBase);
			}
		}

		public static void RequestCutOutShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape)
		{
			MyVoxelBase myVoxelBase = voxelMap as MyVoxelBase;
			MyShape myShape = voxelShape as MyShape;
			if (myVoxelBase != null)
			{
				myShape?.SendCutOutRequest(myVoxelBase);
			}
		}

		public static void CutOutShapeWithProperties(MyVoxelBase voxelMap, MyShape shape, out float voxelsCountInPercent, out MyVoxelMaterialDefinition voxelMaterial, Dictionary<MyVoxelMaterialDefinition, int> exactCutOutMaterials = null, bool updateSync = false, bool onlyCheck = false, bool applyDamageMaterial = false, bool onlyApplyMaterial = false, bool skipCache = false)
		{
			if (!MySession.Static.EnableVoxelDestruction || voxelMap == null || voxelMap.Storage == null || shape == null)
			{
				voxelsCountInPercent = 0f;
				voxelMaterial = null;
				return;
			}
			int num = 0;
			int num2 = 0;
			bool num3 = exactCutOutMaterials != null;
			MatrixD transformation = shape.Transformation;
			MatrixD transformation2 = transformation * voxelMap.PositionComp.WorldMatrixInvScaled;
			transformation2.Translation += voxelMap.SizeInMetresHalf;
			shape.Transformation = transformation2;
			BoundingBoxD shapeAabb = shape.GetWorldBoundaries();
			ComputeShapeBounds(voxelMap, ref shapeAabb, Vector3.Zero, voxelMap.Storage.Size, out Vector3I minCorner, out Vector3I maxCorner);
			bool flag = exactCutOutMaterials != null || applyDamageMaterial;
			Vector3I voxelCoord = minCorner - 1;
			Vector3I voxelCoord2 = maxCorner + 1;
			voxelMap.Storage.ClampVoxelCoord(ref voxelCoord);
			voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
			if (m_cache == null)
			{
				m_cache = new MyStorageData();
			}
			m_cache.Resize(voxelCoord, voxelCoord2);
			MyVoxelRequestFlags requestFlags = (MyVoxelRequestFlags)(((!skipCache) ? 64 : 0) | (flag ? 2 : 0));
			voxelMap.Storage.ReadRange(m_cache, (!flag) ? MyStorageDataTypeFlags.Content : MyStorageDataTypeFlags.ContentAndMaterial, 0, voxelCoord, voxelCoord2, ref requestFlags);
			if (num3)
			{
				Vector3I p = m_cache.Size3D / 2;
				voxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(m_cache.Material(ref p));
			}
			else
			{
				Vector3I p = (voxelCoord + voxelCoord2) / 2;
				voxelMaterial = voxelMap.Storage.GetMaterialAt(ref p);
			}
			MyVoxelMaterialDefinition key = null;
			Vector3I a = default(Vector3I);
			a.X = minCorner.X;
			while (a.X <= maxCorner.X)
			{
				a.Y = minCorner.Y;
				while (a.Y <= maxCorner.Y)
				{
					a.Z = minCorner.Z;
					while (a.Z <= maxCorner.Z)
					{
						Vector3I p2 = a - voxelCoord;
						int linearIdx = m_cache.ComputeLinear(ref p2);
						byte b = m_cache.Content(linearIdx);
						if (b != 0)
						{
							Vector3D voxelPosition = (Vector3D)(a - voxelMap.StorageMin) * 1.0;
							float volume = shape.GetVolume(ref voxelPosition);
							if (volume != 0f)
							{
								int num4 = (int)(volume * 255f);
								int num5 = Math.Max(b - num4, 0);
								int num6 = b - num5;
								if ((int)b / 10 != num5 / 10)
								{
									if (!onlyCheck && !onlyApplyMaterial)
									{
										m_cache.Content(linearIdx, (byte)num5);
									}
									num += b;
									num2 += num6;
									byte b2 = m_cache.Material(linearIdx);
									if (num5 == 0)
									{
										m_cache.Material(linearIdx, byte.MaxValue);
									}
									if (b2 != byte.MaxValue)
									{
										if (flag)
										{
											key = MyDefinitionManager.Static.GetVoxelMaterialDefinition(b2);
										}
										if (exactCutOutMaterials != null)
										{
											exactCutOutMaterials.TryGetValue(key, out int value);
											value = (exactCutOutMaterials[key] = value + (MyFakes.ENABLE_REMOVED_VOXEL_CONTENT_HACK ? ((int)((float)num6 * 3.9f)) : num6));
										}
									}
								}
							}
						}
						a.Z++;
					}
					a.Y++;
				}
				a.X++;
			}
			if (num2 > 0 && updateSync && Sync.IsServer && !onlyCheck)
			{
				shape.SendDrillCutOutRequest(voxelMap, applyDamageMaterial);
			}
			if (num2 > 0 && !onlyCheck)
			{
				RemoveSmallVoxelsUsingChachedVoxels();
				MyStorageDataTypeFlags dataToWrite = MyStorageDataTypeFlags.ContentAndMaterial;
				if (MyFakes.LOG_NAVMESH_GENERATION && MyAIComponent.Static.Pathfinding != null)
				{
					MyAIComponent.Static.Pathfinding.GetPathfindingLog().LogStorageWrite(voxelMap, m_cache, dataToWrite, voxelCoord, voxelCoord2);
				}
				voxelMap.Storage.WriteRange(m_cache, dataToWrite, voxelCoord, voxelCoord2, notify: false, skipCache);
			}
			voxelsCountInPercent = (((float)num > 0f) ? ((float)num2 / (float)num) : 0f);
			if (num2 > 0)
			{
				BoundingBoxD cutOutBox = shape.GetWorldBoundaries();
				MySandboxGame.Static.Invoke(delegate
				{
					if (voxelMap.Storage != null)
					{
						voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
						NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, voxelMap, ref cutOutBox);
					}
				}, "CutOutShapeWithProperties notify");
			}
			shape.Transformation = transformation;
		}

		public static void ChangeMaterialsInShape(MyVoxelBase voxelMap, MyShape shape, byte materialIdx, bool[] materialsToChange)
		{
			if (voxelMap != null && shape != null)
			{
				using (voxelMap.Pin())
				{
					if (!voxelMap.MarkedForClose)
					{
						MatrixD transformation = shape.Transformation * voxelMap.PositionComp.WorldMatrixInvScaled;
						transformation.Translation += voxelMap.SizeInMetresHalf;
						shape.Transformation = transformation;
						BoundingBoxD shapeAabb = shape.GetWorldBoundaries();
						ComputeShapeBounds(voxelMap, ref shapeAabb, Vector3.Zero, voxelMap.Storage.Size, out Vector3I voxelMin, out Vector3I voxelMax);
						Vector3I voxelCoord = voxelMin - 1;
						Vector3I voxelCoord2 = voxelMax + 1;
						voxelMap.Storage.ClampVoxelCoord(ref voxelCoord);
						voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
						if (m_cache == null)
						{
							m_cache = new MyStorageData();
						}
						m_cache.Resize(voxelCoord, voxelCoord2);
						MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent | MyVoxelRequestFlags.AdviseCache;
						voxelMap.Storage.ReadRange(m_cache, MyStorageDataTypeFlags.Material, 0, voxelCoord, voxelCoord2, ref requestFlags);
						Vector3I voxelCoord3 = default(Vector3I);
						voxelCoord3.X = voxelMin.X;
						while (voxelCoord3.X <= voxelMax.X)
						{
							voxelCoord3.Y = voxelMin.Y;
							while (voxelCoord3.Y <= voxelMax.Y)
							{
								voxelCoord3.Z = voxelMin.Z;
								while (voxelCoord3.Z <= voxelMax.Z)
								{
									Vector3I p = voxelCoord3 - voxelMin;
									int linearIdx = m_cache.ComputeLinear(ref p);
									byte b = m_cache.Material(linearIdx);
									if (materialsToChange[b])
									{
										MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionLeftBottomCorner, ref voxelCoord3, out Vector3D worldPosition);
										if (shape.GetVolume(ref worldPosition) > 0.5f && m_cache.Material(ref p) != byte.MaxValue)
										{
											m_cache.Material(ref p, materialIdx);
										}
									}
									voxelCoord3.Z++;
								}
								voxelCoord3.Y++;
							}
							voxelCoord3.X++;
						}
					}
				}
			}
		}

		public static void RevertShape(MyVoxelBase voxelMap, MyShape shape)
		{
			using (voxelMap.Pin())
			{
				if (!voxelMap.MarkedForClose)
				{
					GetVoxelShapeDimensions(voxelMap, shape, out Vector3I minCorner, out Vector3I maxCorner, out Vector3I _);
					minCorner = Vector3I.Max(Vector3I.One, minCorner);
					maxCorner = Vector3I.Max(minCorner, maxCorner - Vector3I.One);
					voxelMap.Storage.DeleteRange(MyStorageDataTypeFlags.ContentAndMaterial, minCorner, maxCorner, notify: false);
					BoundingBoxD cutOutBox = shape.GetWorldBoundaries();
					MySandboxGame.Static.Invoke(delegate
					{
						if (voxelMap.Storage != null)
						{
							voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
							NotifyVoxelChanged(MyVoxelBase.OperationType.Revert, voxelMap, ref cutOutBox);
						}
					}, "RevertShape notify");
				}
			}
		}

		public static void FillInShape(MyVoxelBase voxelMap, MyShape shape, byte materialIdx)
		{
			using (voxelMap.Pin())
			{
				if (!voxelMap.MarkedForClose)
				{
					ulong num = 0uL;
					GetVoxelShapeDimensions(voxelMap, shape, out Vector3I minCorner, out Vector3I maxCorner, out Vector3I numCells);
					minCorner = Vector3I.Max(Vector3I.One, minCorner);
					maxCorner = Vector3I.Max(minCorner, maxCorner - Vector3I.One);
					if (m_cache == null)
					{
						m_cache = new MyStorageData();
					}
					Vector3I_RangeIterator it = new Vector3I_RangeIterator(ref Vector3I.Zero, ref numCells);
					while (it.IsValid())
					{
						GetCellCorners(ref minCorner, ref maxCorner, ref it, out Vector3I cellMinCorner, out Vector3I cellMaxCorner);
						Vector3I originalValue = cellMinCorner;
						Vector3I originalValue2 = cellMaxCorner;
						voxelMap.Storage.ClampVoxelCoord(ref cellMinCorner, 0);
						voxelMap.Storage.ClampVoxelCoord(ref cellMaxCorner, 0);
						ClampingInfo clampingInfo = CheckForClamping(originalValue, cellMinCorner);
						ClampingInfo clampingInfo2 = CheckForClamping(originalValue2, cellMaxCorner);
						m_cache.Resize(cellMinCorner, cellMaxCorner);
						MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
						voxelMap.Storage.ReadRange(m_cache, MyStorageDataTypeFlags.ContentAndMaterial, 0, cellMinCorner, cellMaxCorner, ref requestFlags);
						ulong num2 = 0uL;
						Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref cellMinCorner, ref cellMaxCorner);
						while (vector3I_RangeIterator.IsValid())
						{
							Vector3I p = vector3I_RangeIterator.Current - cellMinCorner;
							byte b = m_cache.Content(ref p);
							if (b != byte.MaxValue || m_cache.Material(ref p) != materialIdx)
							{
								if ((vector3I_RangeIterator.Current.X == cellMinCorner.X && clampingInfo.X) || (vector3I_RangeIterator.Current.X == cellMaxCorner.X && clampingInfo2.X) || (vector3I_RangeIterator.Current.Y == cellMinCorner.Y && clampingInfo.Y) || (vector3I_RangeIterator.Current.Y == cellMaxCorner.Y && clampingInfo2.Y) || (vector3I_RangeIterator.Current.Z == cellMinCorner.Z && clampingInfo.Z) || (vector3I_RangeIterator.Current.Z == cellMaxCorner.Z && clampingInfo2.Z))
								{
									if (b != 0)
									{
										m_cache.Material(ref p, materialIdx);
									}
								}
								else
								{
									MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionLeftBottomCorner, ref vector3I_RangeIterator.Current, out Vector3D worldPosition);
									float volume = shape.GetVolume(ref worldPosition);
									if (!(volume <= 0f))
									{
										int val = (int)(volume * 255f);
										long num3 = Math.Max(b, val);
										m_cache.Content(ref p, (byte)num3);
										if (num3 != 0L)
										{
											m_cache.Material(ref p, materialIdx);
										}
										num2 = (ulong)((long)num2 + (num3 - b));
									}
								}
							}
							vector3I_RangeIterator.MoveNext();
						}
						if (num2 != 0)
						{
							RemoveSmallVoxelsUsingChachedVoxels();
							voxelMap.Storage.WriteRange(m_cache, MyStorageDataTypeFlags.ContentAndMaterial, cellMinCorner, cellMaxCorner, notify: false, skipCache: true);
						}
						num += num2;
						it.MoveNext();
					}
					if (num != 0)
					{
						BoundingBoxD cutOutBox = shape.GetWorldBoundaries();
						MySandboxGame.Static.Invoke(delegate
						{
							if (voxelMap.Storage != null)
							{
								voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
								NotifyVoxelChanged(MyVoxelBase.OperationType.Fill, voxelMap, ref cutOutBox);
							}
						}, "FillInShape Notify");
					}
				}
			}
		}

		public static void PaintInShape(MyVoxelBase voxelMap, MyShape shape, byte materialIdx)
		{
			using (voxelMap.Pin())
			{
				if (!voxelMap.MarkedForClose)
				{
					GetVoxelShapeDimensions(voxelMap, shape, out Vector3I minCorner, out Vector3I maxCorner, out Vector3I numCells);
					if (m_cache == null)
					{
						m_cache = new MyStorageData();
					}
					Vector3I_RangeIterator it = new Vector3I_RangeIterator(ref Vector3I.Zero, ref numCells);
					while (it.IsValid())
					{
						GetCellCorners(ref minCorner, ref maxCorner, ref it, out Vector3I cellMinCorner, out Vector3I cellMaxCorner);
						m_cache.Resize(cellMinCorner, cellMaxCorner);
						MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
						voxelMap.Storage.ReadRange(m_cache, MyStorageDataTypeFlags.ContentAndMaterial, 0, cellMinCorner, cellMaxCorner, ref requestFlags);
						Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref cellMinCorner, ref cellMaxCorner);
						while (vector3I_RangeIterator.IsValid())
						{
							Vector3I p = vector3I_RangeIterator.Current - cellMinCorner;
							MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionLeftBottomCorner, ref vector3I_RangeIterator.Current, out Vector3D worldPosition);
							if (shape.GetVolume(ref worldPosition) > 0.5f && m_cache.Material(ref p) != byte.MaxValue)
							{
								m_cache.Material(ref p, materialIdx);
							}
							vector3I_RangeIterator.MoveNext();
						}
						voxelMap.Storage.WriteRange(m_cache, MyStorageDataTypeFlags.Material, cellMinCorner, cellMaxCorner, notify: false, skipCache: true);
						it.MoveNext();
					}
					MySandboxGame.Static.Invoke(delegate
					{
						if (voxelMap.Storage != null)
						{
							voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
						}
					}, "PaintInShape notify");
				}
			}
		}

		public static bool CutOutSphereFast(MyVoxelBase voxelMap, ref Vector3D center, float radius, out Vector3I cacheMin, out Vector3I cacheMax, bool notifyChanged)
		{
			MatrixD worldMatrixInvScaled = voxelMap.PositionComp.WorldMatrixInvScaled;
			worldMatrixInvScaled.Translation += voxelMap.SizeInMetresHalf;
			BoundingBoxD shapeAabb = BoundingBoxD.CreateFromSphere(new BoundingSphereD(center, radius)).TransformFast(worldMatrixInvScaled);
			ComputeShapeBounds(voxelMap, ref shapeAabb, Vector3.Zero, voxelMap.Storage.Size, out Vector3I voxelMin, out Vector3I voxelMax);
			cacheMin = voxelMin - 1;
			cacheMax = voxelMax + 1;
			voxelMap.Storage.ClampVoxelCoord(ref cacheMin);
			voxelMap.Storage.ClampVoxelCoord(ref cacheMax);
			CutOutSphere voxelOperator = default(CutOutSphere);
			voxelOperator.RadSq = radius * radius;
			voxelOperator.Center = Vector3D.Transform(center, worldMatrixInvScaled) - (Vector3D)(cacheMin - voxelMap.StorageMin);
			voxelMap.Storage.ExecuteOperationFast(ref voxelOperator, MyStorageDataTypeFlags.Content, ref cacheMin, ref cacheMax, notifyChanged);
			return voxelOperator.Changed;
		}

		public static void CutOutShape(MyVoxelBase voxelMap, MyShape shape, bool voxelHand = false)
		{
			if (MySession.Static.EnableVoxelDestruction || MySession.Static.HighSimulationQuality)
			{
				using (voxelMap.Pin())
				{
					if (!voxelMap.MarkedForClose)
					{
						GetVoxelShapeDimensions(voxelMap, shape, out Vector3I minCorner, out Vector3I maxCorner, out Vector3I numCells);
						ulong num = 0uL;
						if (m_cache == null)
						{
							m_cache = new MyStorageData();
						}
						Vector3I_RangeIterator it = new Vector3I_RangeIterator(ref Vector3I.Zero, ref numCells);
						while (it.IsValid())
						{
							GetCellCorners(ref minCorner, ref maxCorner, ref it, out Vector3I cellMinCorner, out Vector3I cellMaxCorner);
							Vector3I voxelCoord = cellMinCorner - 1;
							Vector3I voxelCoord2 = cellMaxCorner + 1;
							voxelMap.Storage.ClampVoxelCoord(ref voxelCoord);
							voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
							ulong num2 = 0uL;
							m_cache.Resize(voxelCoord, voxelCoord2);
							MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
							voxelMap.Storage.ReadRange(m_cache, MyStorageDataTypeFlags.ContentAndMaterial, 0, voxelCoord, voxelCoord2, ref requestFlags);
							Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref cellMinCorner, ref cellMaxCorner);
							while (vector3I_RangeIterator.IsValid())
							{
								Vector3I p = vector3I_RangeIterator.Current - voxelCoord;
								byte b = m_cache.Content(ref p);
								if (b != 0)
								{
									MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionLeftBottomCorner, ref vector3I_RangeIterator.Current, out Vector3D worldPosition);
									float volume = shape.GetVolume(ref worldPosition);
									if (volume != 0f)
									{
										int num3 = Math.Min((int)(255f - volume * 255f), b);
										ulong num4 = (ulong)Math.Abs(b - num3);
										m_cache.Content(ref p, (byte)num3);
										if (num3 == 0)
										{
											m_cache.Material(ref p, byte.MaxValue);
										}
										num2 += num4;
									}
								}
								vector3I_RangeIterator.MoveNext();
							}
							if (num2 != 0)
							{
								RemoveSmallVoxelsUsingChachedVoxels();
								voxelMap.Storage.WriteRange(m_cache, MyStorageDataTypeFlags.ContentAndMaterial, voxelCoord, voxelCoord2, notify: false, skipCache: true);
							}
							num += num2;
							it.MoveNext();
						}
						if (num != 0)
						{
							BoundingBoxD cutOutBox = shape.GetWorldBoundaries();
							MySandboxGame.Static.Invoke(delegate
							{
								if (voxelMap.Storage != null)
								{
									voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
									NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, voxelMap, ref cutOutBox);
								}
							}, "CutOutShape notify");
						}
					}
				}
			}
		}

		private static ClampingInfo CheckForClamping(Vector3I originalValue, Vector3I clampedValue)
		{
			ClampingInfo result = new ClampingInfo(X: false, Y: false, Z: false);
			if (originalValue.X != clampedValue.X)
			{
				result.X = true;
			}
			if (originalValue.Y != clampedValue.Y)
			{
				result.Y = true;
			}
			if (originalValue.Z != clampedValue.Z)
			{
				result.Z = true;
			}
			return result;
		}

		private static void RemoveSmallVoxelsUsingChachedVoxels()
		{
			Vector3I size3D = m_cache.Size3D;
			Vector3I max = size3D - 1;
			Vector3I p = default(Vector3I);
			p.X = 0;
			Vector3I p2 = default(Vector3I);
			while (p.X < size3D.X)
			{
				p.Y = 0;
				while (p.Y < size3D.Y)
				{
					p.Z = 0;
					while (p.Z < size3D.Z)
					{
						int num = m_cache.Content(ref p);
						if (num > 0 && num < 127)
						{
							Vector3I value = p - 1;
							Vector3I value2 = p + 1;
							Vector3I.Clamp(ref value, ref Vector3I.Zero, ref max, out value);
							Vector3I.Clamp(ref value2, ref Vector3I.Zero, ref max, out value2);
							bool flag = false;
							p2.X = value.X;
							while (p2.X <= value2.X)
							{
								p2.Y = value.Y;
								while (p2.Y <= value2.Y)
								{
									p2.Z = value.Z;
									while (p2.Z <= value2.Z)
									{
										if (m_cache.Content(ref p2) < 127)
										{
											p2.Z++;
											continue;
										}
										goto IL_00cb;
									}
									p2.Y++;
								}
								p2.X++;
								continue;
								IL_00cb:
								flag = true;
								break;
							}
							if (!flag)
							{
								m_cache.Content(ref p, 0);
								m_cache.Material(ref p, byte.MaxValue);
							}
						}
						p.Z++;
					}
					p.Y++;
				}
				p.X++;
			}
		}

		private static void ComputeShapeBounds(MyVoxelBase voxelMap, ref BoundingBoxD shapeAabb, Vector3D voxelMapMinCorner, Vector3I storageSize, out Vector3I voxelMin, out Vector3I voxelMax)
		{
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(voxelMapMinCorner, ref shapeAabb.Min, out voxelMin);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(voxelMapMinCorner, ref shapeAabb.Max, out voxelMax);
			voxelMin += voxelMap.StorageMin;
			voxelMax += voxelMap.StorageMin;
			voxelMax += 1;
			storageSize -= 1;
			Vector3I.Clamp(ref voxelMin, ref Vector3I.Zero, ref storageSize, out voxelMin);
			Vector3I.Clamp(ref voxelMax, ref Vector3I.Zero, ref storageSize, out voxelMax);
		}

		private static void GetVoxelShapeDimensions(MyVoxelBase voxelMap, MyShape shape, out Vector3I minCorner, out Vector3I maxCorner, out Vector3I numCells)
		{
			BoundingBoxD shapeAabb = shape.GetWorldBoundaries();
			ComputeShapeBounds(voxelMap, ref shapeAabb, voxelMap.PositionLeftBottomCorner, voxelMap.Storage.Size, out minCorner, out maxCorner);
			numCells = new Vector3I((maxCorner.X - minCorner.X) / 16, (maxCorner.Y - minCorner.Y) / 16, (maxCorner.Z - minCorner.Z) / 16);
		}

		private static void GetCellCorners(ref Vector3I minCorner, ref Vector3I maxCorner, ref Vector3I_RangeIterator it, out Vector3I cellMinCorner, out Vector3I cellMaxCorner)
		{
			cellMinCorner = new Vector3I(Math.Min(maxCorner.X, minCorner.X + it.Current.X * 16), Math.Min(maxCorner.Y, minCorner.Y + it.Current.Y * 16), Math.Min(maxCorner.Z, minCorner.Z + it.Current.Z * 16));
			cellMaxCorner = new Vector3I(Math.Min(maxCorner.X, cellMinCorner.X + 16), Math.Min(maxCorner.Y, cellMinCorner.Y + 16), Math.Min(maxCorner.Z, cellMinCorner.Z + 16));
		}

		public static void NotifyVoxelChanged(MyVoxelBase.OperationType type, MyVoxelBase voxelMap, ref BoundingBoxD cutOutBox)
		{
			cutOutBox.Inflate(0.25);
			MyGamePruningStructure.GetTopmostEntitiesInBox(ref cutOutBox, m_overlapList);
			if (MyFakes.ENABLE_BLOCKS_IN_VOXELS_TEST)
			{
				foreach (MyEntity overlap in m_overlapList)
				{
					if (Sync.IsServer)
					{
						MyCubeGrid myCubeGrid = overlap as MyCubeGrid;
						if (myCubeGrid != null && myCubeGrid.IsStatic)
						{
							if (myCubeGrid.Physics != null && myCubeGrid.Physics.Shape != null)
							{
								myCubeGrid.Physics.Shape.RecalculateConnectionsToWorld(myCubeGrid.GetBlocks());
							}
							if (type == MyVoxelBase.OperationType.Cut)
							{
								myCubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
							}
						}
					}
					MyPhysicsBody myPhysicsBody = overlap.Physics as MyPhysicsBody;
					if (myPhysicsBody != null && !myPhysicsBody.IsStatic && myPhysicsBody.RigidBody != null)
					{
						myPhysicsBody.RigidBody.Activate();
					}
				}
			}
			m_overlapList.Clear();
			if (Sync.IsServer)
			{
				MyPlanetEnvironmentComponent myPlanetEnvironmentComponent = voxelMap.Components.Get<MyPlanetEnvironmentComponent>();
				if (myPlanetEnvironmentComponent != null)
				{
					myPlanetEnvironmentComponent.GetSectorsInRange(ref cutOutBox, m_overlapList);
					foreach (MyEnvironmentSector overlap2 in m_overlapList)
					{
						overlap2.DisableItemsInBox(ref cutOutBox);
					}
					m_overlapList.Clear();
				}
			}
		}
	}
}
