using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_MergeBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyShipMergeBlock),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyShipMergeBlock)
	})]
	public class MyShipMergeBlock : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyShipMergeBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyShipMergeBlock
	{
		[Flags]
		private enum UpdateBeforeFlags : byte
		{
			None = 0x0,
			EnableConstraint = 0x1,
			UpdateIsWorking = 0x2
		}

		private enum MergeState
		{
			UNSET,
			NONE,
			WORKING,
			CONSTRAINED,
			LOCKED
		}

		private struct MergeData
		{
			public bool PositionOk;

			public bool RotationOk;

			public bool AxisOk;

			public float Distance;

			public float RotationDelta;

			public float AxisDelta;

			public float ConstraintStrength;

			public float StrengthFactor;

			public Vector3 RelativeVelocity;
		}

		protected class m_mergeState_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType mergeState;
				ISyncType result = mergeState = new Sync<MergeState, SyncDirection.FromServer>(P_1, P_2);
				((MyShipMergeBlock)P_0).m_mergeState = (Sync<MergeState, SyncDirection.FromServer>)mergeState;
				return result;
			}
		}

		private HkConstraint m_constraint;

		private MyShipMergeBlock m_other;

		private MyConcurrentHashSet<MyCubeGrid> m_gridList = new MyConcurrentHashSet<MyCubeGrid>();

		private ushort m_frameCounter;

		private UpdateBeforeFlags m_updateBeforeFlags;

		private Base6Directions.Direction m_forward;

		private Base6Directions.Direction m_right;

		private Base6Directions.Direction m_otherRight;

		private Sync<MergeState, SyncDirection.FromServer> m_mergeState;

		private bool HasConstraint;

		public bool InConstraint => m_constraint != null;

		private HkConstraint SafeConstraint
		{
			get
			{
				if (m_constraint != null && !m_constraint.InWorld)
				{
					RemoveConstraintInBoth();
				}
				return m_constraint;
			}
		}

		public MyShipMergeBlock Other => m_other ?? GetOtherMergeBlock();

		public int GridCount => m_gridList.Count;

		public Base6Directions.Direction OtherRight => m_otherRight;

		private bool IsWithinWorldLimits
		{
			get
			{
				if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
				{
					return true;
				}
				if (MySession.Static.MaxGridSize != 0)
				{
					return base.CubeGrid.BlocksCount + m_other.CubeGrid.BlocksCount <= MySession.Static.MaxGridSize;
				}
				return true;
			}
		}

		public bool IsLocked => (MergeState)m_mergeState == MergeState.LOCKED;

		SpaceEngineers.Game.ModAPI.IMyShipMergeBlock SpaceEngineers.Game.ModAPI.IMyShipMergeBlock.Other => Other;

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyShipMergeBlock.IsConnected => Other != null;

		private event Action BeforeMerge;

		event Action SpaceEngineers.Game.ModAPI.IMyShipMergeBlock.BeforeMerge
		{
			add
			{
				BeforeMerge += value;
			}
			remove
			{
				BeforeMerge += value;
			}
		}

		public MyShipMergeBlock()
		{
			if (!Sync.IsServer)
			{
				m_mergeState.ValueChanged += delegate
				{
					UpdateEmissivity();
				};
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			LoadDummies();
			SlimBlock.DeformationRatio = (base.BlockDefinition as MyMergeBlockDefinition).DeformationRatio;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			base.NeedsWorldMatrix = true;
			AddDebugRenderComponent(new MyDebugRenderComponentShipMergeBlock(this));
		}

		protected override bool CheckIsWorking()
		{
			MyShipMergeBlock otherMergeBlock = GetOtherMergeBlock();
			if (otherMergeBlock == null || otherMergeBlock.FriendlyWithBlock(this))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.IsWorkingChanged += MyShipMergeBlock_IsWorkingChanged;
			base.CheckConnectionAllowed = !base.IsWorking;
			base.Physics.Enabled = base.IsWorking;
			UpdateState();
			GetOtherMergeBlock()?.UpdateIsWorkingBeforeNextFrame();
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			GetOtherMergeBlock()?.UpdateIsWorkingBeforeNextFrame();
			RemoveConstraintInBoth();
			if (Sync.IsServer)
			{
				m_mergeState.Value = MergeState.UNSET;
			}
		}

		public void UpdateIsWorkingBeforeNextFrame()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			m_updateBeforeFlags |= UpdateBeforeFlags.UpdateIsWorking;
		}

		private void LoadDummies()
		{
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(base.BlockDefinition.Model).Dummies)
			{
				if (dummy.Key.ToLower().Contains("merge"))
				{
					Matrix matrix = dummy.Value.Matrix;
					Vector3 vector = matrix.Scale / 2f;
					Vector3 vec = Vector3.DominantAxisProjection(matrix.Translation / vector);
					vec.Normalize();
					m_forward = Base6Directions.GetDirection(vec);
					m_right = Base6Directions.GetPerpendicular(m_forward);
					MatrixD worldTransform = MatrixD.Normalize(matrix) * base.WorldMatrix;
					HkBvShape shape = CreateFieldShape(vector);
					base.Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_STATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
					base.Physics.IsPhantom = true;
					base.Physics.CreateFromCollisionObject(shape, matrix.Translation, worldTransform, null, 24);
					base.Physics.Enabled = base.IsWorking;
					base.Physics.RigidBody.ContactPointCallbackEnabled = true;
					shape.Base.RemoveReference();
					break;
				}
			}
		}

		private HkBvShape CreateFieldShape(Vector3 extents)
		{
			return new HkBvShape(childShape: new HkPhantomCallbackShape(phantom_Enter, phantom_Leave), boundingVolumeShape: new HkBoxShape(extents), policy: HkReferencePolicy.TakeOwnership);
		}

		private void phantom_Leave(HkPhantomCallbackShape shape, HkRigidBody body)
		{
			List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
			foreach (VRage.ModAPI.IMyEntity item in allEntities)
			{
				m_gridList.Remove(item as MyCubeGrid);
			}
			allEntities.Clear();
		}

		private void phantom_Enter(HkPhantomCallbackShape shape, HkRigidBody body)
		{
			List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
			foreach (VRage.ModAPI.IMyEntity item in allEntities)
			{
				MyCubeGrid myCubeGrid = item as MyCubeGrid;
				if (myCubeGrid != null && myCubeGrid.GridSizeEnum == base.CubeGrid.GridSizeEnum && myCubeGrid != base.CubeGrid && !(myCubeGrid.Physics.RigidBody != body))
				{
					m_gridList.Add(myCubeGrid);
				}
			}
			allEntities.Clear();
		}

		private void CalculateMergeArea(out Vector3I minI, out Vector3I maxI)
		{
			Vector3I intVector = Base6Directions.GetIntVector(base.Orientation.TransformDirection(m_forward));
			minI = base.Min + intVector;
			maxI = base.Max + intVector;
			if (intVector.X + intVector.Y + intVector.Z == -1)
			{
				maxI += (maxI - minI) * intVector;
			}
			else
			{
				minI += (maxI - minI) * intVector;
			}
		}

		private MySlimBlock GetBlockInMergeArea()
		{
			CalculateMergeArea(out Vector3I minI, out Vector3I maxI);
			Vector3I next = minI;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref minI, ref maxI);
			while (vector3I_RangeIterator.IsValid())
			{
				MySlimBlock cubeBlock = base.CubeGrid.GetCubeBlock(next);
				if (cubeBlock != null)
				{
					return cubeBlock;
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			return null;
		}

		private MyShipMergeBlock GetOtherMergeBlock()
		{
			CalculateMergeArea(out Vector3I minI, out Vector3I maxI);
			Vector3I next = minI;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref minI, ref maxI);
			while (vector3I_RangeIterator.IsValid())
			{
				MySlimBlock cubeBlock = base.CubeGrid.GetCubeBlock(next);
				if (cubeBlock != null && cubeBlock.FatBlock != null)
				{
					MyShipMergeBlock myShipMergeBlock = cubeBlock.FatBlock as MyShipMergeBlock;
					if (myShipMergeBlock != null)
					{
						myShipMergeBlock.CalculateMergeArea(out Vector3I minI2, out Vector3I maxI2);
						Vector3I intVector = Base6Directions.GetIntVector(base.Orientation.TransformDirection(m_forward));
						minI2 = maxI - (minI2 + intVector);
						maxI2 = maxI2 + intVector - minI;
						if (minI2.X >= 0 && minI2.Y >= 0 && minI2.Z >= 0 && maxI2.X >= 0 && maxI2.Y >= 0 && maxI2.Z >= 0)
						{
							return myShipMergeBlock;
						}
					}
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			return null;
		}

		private Vector3 GetMergeNormalWorld()
		{
			return base.WorldMatrix.GetDirectionVector(m_forward);
		}

		private void MyShipMergeBlock_IsWorkingChanged(MyCubeBlock obj)
		{
			if (base.Physics != null)
			{
				base.Physics.Enabled = base.IsWorking;
			}
			if (!base.IsWorking && GetOtherMergeBlock() == null && InConstraint)
			{
				RemoveConstraintInBoth();
			}
			base.CheckConnectionAllowed = !base.IsWorking;
			base.CubeGrid.UpdateBlockNeighbours(SlimBlock);
			UpdateState();
		}

		protected override void OnStopWorking()
		{
			UpdateState();
			base.OnStopWorking();
		}

		protected override void OnStartWorking()
		{
			UpdateState();
			base.OnStartWorking();
		}

		private void UpdateState()
		{
			if (!base.InScene || !Sync.IsServer)
			{
				return;
			}
			MergeState mergeState = MergeState.WORKING;
			MyShipMergeBlock otherMergeBlock = GetOtherMergeBlock();
			if (!base.IsWorking)
			{
				mergeState = MergeState.NONE;
			}
			else if (otherMergeBlock != null && otherMergeBlock.IsWorking)
			{
				Base6Directions.Direction flippedDirection = Base6Directions.GetFlippedDirection(otherMergeBlock.Orientation.TransformDirection(otherMergeBlock.m_forward));
				Base6Directions.Direction direction = base.Orientation.TransformDirection(m_forward);
				if (flippedDirection == direction)
				{
					mergeState = MergeState.LOCKED;
				}
			}
			else if (InConstraint)
			{
				mergeState = MergeState.CONSTRAINED;
			}
			if (mergeState != (MergeState)m_mergeState)
			{
				m_mergeState.Value = mergeState;
				UpdateEmissivity();
				otherMergeBlock?.UpdateIsWorkingBeforeNextFrame();
			}
		}

		public override void CheckEmissiveState(bool force)
		{
			if (force && Sync.IsServer)
			{
				m_mergeState.Value = MergeState.UNSET;
			}
			UpdateState();
			if ((MergeState)m_mergeState != 0)
			{
				UpdateEmissivity();
			}
		}

		private void UpdateEmissivity()
		{
			switch (m_mergeState.Value)
			{
			case MergeState.UNSET:
				break;
			case MergeState.LOCKED:
				SetEmissiveState(MyCubeBlock.m_emissiveNames.Locked, base.Render.RenderObjectIDs[0]);
				break;
			case MergeState.CONSTRAINED:
				SetEmissiveState(MyCubeBlock.m_emissiveNames.Constraint, base.Render.RenderObjectIDs[0]);
				break;
			case MergeState.WORKING:
				SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
				break;
			case MergeState.NONE:
			{
				MyShipMergeBlock otherMergeBlock = GetOtherMergeBlock();
				if (otherMergeBlock != null && !otherMergeBlock.FriendlyWithBlock(this))
				{
					SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
				}
				else if (base.IsFunctional)
				{
					SetEmissiveStateDisabled();
				}
				else
				{
					SetEmissiveStateDamaged();
				}
				break;
			}
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			UpdateState();
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			UpdateIsWorkingBeforeNextFrame();
		}

		private void CalculateMergeData(ref MergeData data)
		{
			float num = (base.BlockDefinition as MyMergeBlockDefinition)?.Strength ?? 0.1f;
			data.Distance = (float)(base.WorldMatrix.Translation - m_other.WorldMatrix.Translation).Length() - base.CubeGrid.GridSize;
			data.StrengthFactor = (float)Math.Exp((0f - data.Distance) / base.CubeGrid.GridSize);
			float num2 = MathHelper.Lerp(0f, num * ((base.CubeGrid.GridSizeEnum == MyCubeSize.Large) ? 0.005f : 0.1f), data.StrengthFactor);
			Vector3 velocityAtPoint = base.CubeGrid.Physics.GetVelocityAtPoint(base.PositionComp.GetPosition());
			Vector3 velocityAtPoint2 = m_other.CubeGrid.Physics.GetVelocityAtPoint(m_other.PositionComp.GetPosition());
			data.RelativeVelocity = velocityAtPoint2 - velocityAtPoint;
			float num3 = 1f;
			float num4 = data.RelativeVelocity.Length();
			num3 = Math.Max(3.6f / ((num4 > 0.1f) ? num4 : 0.1f), 1f);
			data.ConstraintStrength = num2 / num3;
			Vector3 vector = m_other.PositionComp.GetPosition() - base.PositionComp.GetPosition();
			Vector3 value = base.WorldMatrix.GetDirectionVector(m_forward);
			data.Distance = vector.Length();
			data.PositionOk = (data.Distance < base.CubeGrid.GridSize + 0.17f);
			data.AxisDelta = (float)(value + m_other.WorldMatrix.GetDirectionVector(m_forward)).Length();
			data.AxisOk = (data.AxisDelta < 0.1f);
			data.RotationDelta = (float)(base.WorldMatrix.GetDirectionVector(m_right) - m_other.WorldMatrix.GetDirectionVector(m_other.m_otherRight)).Length();
			data.RotationOk = (data.RotationDelta < 0.08f);
		}

		private void DebugDrawInfo(Vector2 offset)
		{
			MergeData data = default(MergeData);
			CalculateMergeData(ref data);
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, 75f) + offset, "x = " + data.StrengthFactor, Color.Green, 0.8f);
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f) + offset, "Merge block strength: " + data.ConstraintStrength, Color.Green, 0.8f);
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, 15f) + offset, "Merge block dist: " + (data.Distance - base.CubeGrid.GridSize), data.PositionOk ? Color.Green : Color.Red, 0.8f);
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, 30f) + offset, "Frame counter: " + m_frameCounter, (m_frameCounter >= 6) ? Color.Green : Color.Red, 0.8f);
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, 45f) + offset, "Rotation difference: " + data.RotationDelta, data.RotationOk ? Color.Green : Color.Red, 0.8f);
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, 60f) + offset, "Axis difference: " + data.AxisDelta, data.AxisOk ? Color.Green : Color.Red, 0.8f);
			float num = data.RelativeVelocity.Length();
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, 90f) + offset, (num > 0.5f) ? "Quick" : "Slow", (num > 0.5f) ? Color.Red : Color.Green, 0.8f);
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (!(SafeConstraint != null))
			{
				return;
			}
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CONNECTORS_AND_MERGE_BLOCKS && base.CustomName.ToString() == "DEBUG")
			{
				DebugDrawInfo(new Vector2(0f, 0f));
				m_other.DebugDrawInfo(new Vector2(0f, 120f));
				MyRenderProxy.DebugDrawLine3D(base.PositionComp.GetPosition(), base.PositionComp.GetPosition() + base.WorldMatrix.GetDirectionVector(m_right) * 10.0, Color.Red, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(m_other.PositionComp.GetPosition(), m_other.PositionComp.GetPosition() + m_other.WorldMatrix.GetDirectionVector(m_other.m_otherRight) * 10.0, Color.Red, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(base.PositionComp.GetPosition(), base.PositionComp.GetPosition() + base.WorldMatrix.GetDirectionVector(m_otherRight) * 5.0, Color.Yellow, Color.Yellow, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(m_other.PositionComp.GetPosition(), m_other.PositionComp.GetPosition() + m_other.WorldMatrix.GetDirectionVector(m_other.m_right) * 5.0, Color.Yellow, Color.Yellow, depthRead: false);
			}
			Vector3 velocityAtPoint = base.CubeGrid.Physics.GetVelocityAtPoint(base.PositionComp.GetPosition());
			Vector3 value = m_other.CubeGrid.Physics.GetVelocityAtPoint(m_other.PositionComp.GetPosition()) - velocityAtPoint;
			if (value.Length() > 0.5f)
			{
				if (!base.CubeGrid.Physics.IsStatic)
				{
					base.CubeGrid.Physics.LinearVelocity += value * 0.05f;
				}
				if (!m_other.CubeGrid.Physics.IsStatic)
				{
					m_other.CubeGrid.Physics.LinearVelocity -= value * 0.05f;
				}
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (!CheckUnobstructed())
			{
				if (SafeConstraint != null)
				{
					RemoveConstraintInBoth();
				}
			}
			else if (SafeConstraint != null)
			{
				if ((!base.CubeGrid.IsStatic && m_other.CubeGrid.IsStatic) || !base.IsWorking || !m_other.IsWorking || !IsWithinWorldLimits)
				{
					return;
				}
				_ = (base.BlockDefinition as MyMergeBlockDefinition)?.Strength;
				if ((float)(base.WorldMatrix.Translation - m_other.WorldMatrix.Translation).Length() - base.CubeGrid.GridSize > base.CubeGrid.GridSize * 3f)
				{
					RemoveConstraintInBoth();
					return;
				}
				MergeData data = default(MergeData);
				CalculateMergeData(ref data);
				(m_constraint.ConstraintData as HkMalleableConstraintData).Strength = data.ConstraintStrength;
				if (data.PositionOk && data.AxisOk && data.RotationOk)
				{
					if (m_frameCounter++ < 3)
					{
						return;
					}
					Vector3I gridOffset = CalculateOtherGridOffset();
					Vector3I gridOffset2 = m_other.CalculateOtherGridOffset();
					if (!base.CubeGrid.CanMergeCubes(m_other.CubeGrid, gridOffset))
					{
						if (base.CubeGrid.GridSystems.ControlSystem.IsLocallyControlled || m_other.CubeGrid.GridSystems.ControlSystem.IsLocallyControlled)
						{
							MyHud.Notifications.Add(MyNotificationSingletons.ObstructingBlockDuringMerge);
						}
						return;
					}
					if (this.BeforeMerge != null)
					{
						this.BeforeMerge();
					}
					if (Sync.IsServer)
					{
						foreach (MySlimBlock block in base.CubeGrid.GetBlocks())
						{
							MyShipMergeBlock myShipMergeBlock = block.FatBlock as MyShipMergeBlock;
							if (myShipMergeBlock != null && myShipMergeBlock != this && myShipMergeBlock.InConstraint)
							{
								(block.FatBlock as MyShipMergeBlock).RemoveConstraintInBoth();
							}
						}
						if (base.CubeGrid.MergeGrid_MergeBlock(m_other.CubeGrid, gridOffset) == null)
						{
							m_other.CubeGrid.MergeGrid_MergeBlock(base.CubeGrid, gridOffset2, checkMergeOrder: false);
						}
						RemoveConstraintInBoth();
					}
				}
				else
				{
					m_frameCounter = 0;
				}
			}
			else
			{
				foreach (MyCubeGrid grid in m_gridList)
				{
					if (!grid.MarkedForClose)
					{
						Vector3I position = Vector3I.Zero;
						double distanceSquared = double.MaxValue;
						LineD line = new LineD(base.Physics.ClusterToWorld(base.Physics.RigidBody.Position), base.Physics.ClusterToWorld(base.Physics.RigidBody.Position) + GetMergeNormalWorld());
						if (grid.GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared))
						{
							MyShipMergeBlock myShipMergeBlock2 = grid.GetCubeBlock(position).FatBlock as MyShipMergeBlock;
							if (myShipMergeBlock2 != null)
							{
								if (!myShipMergeBlock2.InConstraint && myShipMergeBlock2.IsWorking && myShipMergeBlock2.CheckUnobstructed() && !(myShipMergeBlock2.GetMergeNormalWorld().Dot(GetMergeNormalWorld()) > 0f) && myShipMergeBlock2.FriendlyWithBlock(this))
								{
									CreateConstraint(grid, myShipMergeBlock2);
									base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
									m_updateBeforeFlags |= UpdateBeforeFlags.EnableConstraint;
								}
								break;
							}
						}
					}
				}
			}
		}

		private bool CheckUnobstructed()
		{
			return GetBlockInMergeArea() == null;
		}

		private Vector3I CalculateOtherGridOffset()
		{
			Vector3 value = ConstraintPositionInGridSpace() / base.CubeGrid.GridSize;
			Vector3 position = -m_other.ConstraintPositionInGridSpace() / m_other.CubeGrid.GridSize;
			Base6Directions.Direction direction = base.Orientation.TransformDirection(m_right);
			MatrixI matrix = MatrixI.CreateRotation(newB: base.Orientation.TransformDirection(m_forward), oldB: Base6Directions.GetFlippedDirection(m_other.Orientation.TransformDirection(m_other.m_forward)), oldA: m_other.CubeGrid.WorldMatrix.GetClosestDirection(base.CubeGrid.WorldMatrix.GetDirectionVector(direction)), newA: direction);
			Vector3.Transform(ref position, ref matrix, out Vector3 result);
			return Vector3I.Round(value + result);
		}

		private Vector3 ConstraintPositionInGridSpace()
		{
			return base.Position * base.CubeGrid.GridSize + base.PositionComp.LocalMatrix.GetDirectionVector(m_forward) * (base.CubeGrid.GridSize * 0.5f);
		}

		private void CreateConstraint(MyCubeGrid other, MyShipMergeBlock block)
		{
			HkPrismaticConstraintData hkPrismaticConstraintData = new HkPrismaticConstraintData();
			Vector3 posA = ConstraintPositionInGridSpace();
			Vector3 posB = block.ConstraintPositionInGridSpace();
			Vector3 directionVector = base.PositionComp.LocalMatrix.GetDirectionVector(m_forward);
			Vector3 directionVector2 = base.PositionComp.LocalMatrix.GetDirectionVector(m_right);
			Vector3 axisB = -block.PositionComp.LocalMatrix.GetDirectionVector(m_forward);
			Base6Directions.Direction closestDirection = block.WorldMatrix.GetClosestDirection(base.WorldMatrix.GetDirectionVector(m_right));
			Base6Directions.Direction closestDirection2 = base.WorldMatrix.GetClosestDirection(block.WorldMatrix.GetDirectionVector(block.m_right));
			Vector3 directionVector3 = block.PositionComp.LocalMatrix.GetDirectionVector(closestDirection);
			hkPrismaticConstraintData.SetInBodySpace(posA, posB, directionVector, axisB, directionVector2, directionVector3, base.CubeGrid.Physics, other.Physics);
			HkMalleableConstraintData hkMalleableConstraintData = new HkMalleableConstraintData();
			hkMalleableConstraintData.SetData(hkPrismaticConstraintData);
			hkPrismaticConstraintData.ClearHandle();
			hkPrismaticConstraintData = null;
			hkMalleableConstraintData.Strength = 1E-05f;
			HkConstraint hkConstraint = new HkConstraint(base.CubeGrid.Physics.RigidBody, other.Physics.RigidBody, hkMalleableConstraintData);
			AddConstraint(hkConstraint);
			SetConstraint(block, hkConstraint, closestDirection2);
			m_other.SetConstraint(this, hkConstraint, closestDirection);
		}

		private void AddConstraint(HkConstraint newConstraint)
		{
			HasConstraint = true;
			base.CubeGrid.Physics.AddConstraint(newConstraint);
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (m_updateBeforeFlags.HasFlag(UpdateBeforeFlags.EnableConstraint))
			{
				if (SafeConstraint != null)
				{
					m_constraint.Enabled = true;
				}
			}
			else if (m_updateBeforeFlags.HasFlag(UpdateBeforeFlags.UpdateIsWorking))
			{
				UpdateIsWorking();
				UpdateState();
			}
			m_updateBeforeFlags = UpdateBeforeFlags.None;
		}

		public override bool ConnectionAllowed(ref Vector3I otherBlockPos, ref Vector3I faceNormal, MyCubeBlockDefinition def)
		{
			return ConnectionAllowedInternal(ref faceNormal, def);
		}

		public override bool ConnectionAllowed(ref Vector3I otherBlockMinPos, ref Vector3I otherBlockMaxPos, ref Vector3I faceNormal, MyCubeBlockDefinition def)
		{
			return ConnectionAllowedInternal(ref faceNormal, def);
		}

		private bool ConnectionAllowedInternal(ref Vector3I faceNormal, MyCubeBlockDefinition def)
		{
			if (base.IsWorking)
			{
				return true;
			}
			if (def != base.BlockDefinition)
			{
				return true;
			}
			if (base.Orientation.TransformDirectionInverse(Base6Directions.GetDirection(faceNormal)) != m_forward)
			{
				return true;
			}
			return false;
		}

		protected void SetConstraint(MyShipMergeBlock otherBlock, HkConstraint constraint, Base6Directions.Direction otherRight)
		{
			if (!(m_constraint != null) && m_other == null)
			{
				m_constraint = constraint;
				m_other = otherBlock;
				m_otherRight = otherRight;
				UpdateState();
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		protected void RemoveConstraint()
		{
			m_constraint = null;
			m_other = null;
			UpdateState();
			if (!base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		protected void RemoveConstraintInBoth()
		{
			if (HasConstraint)
			{
				m_other.RemoveConstraint();
				base.CubeGrid.Physics.RemoveConstraint(m_constraint);
				m_constraint.Dispose();
				RemoveConstraint();
				HasConstraint = false;
			}
			else if (m_other != null)
			{
				m_other.RemoveConstraintInBoth();
			}
		}

		protected override void Closing()
		{
			base.Closing();
			if (InConstraint)
			{
				RemoveConstraintInBoth();
			}
		}

		public override int GetBlockSpecificState()
		{
			if ((MergeState)m_mergeState != MergeState.LOCKED)
			{
				if ((MergeState)m_mergeState != MergeState.CONSTRAINED)
				{
					return 0;
				}
				return 1;
			}
			return 2;
		}
	}
}
