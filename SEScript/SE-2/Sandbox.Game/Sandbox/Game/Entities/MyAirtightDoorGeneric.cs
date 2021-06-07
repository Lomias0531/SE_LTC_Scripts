using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyAirtightDoorBase),
		typeof(Sandbox.ModAPI.Ingame.IMyAirtightDoorBase)
	})]
	public abstract class MyAirtightDoorGeneric : MyDoorBase, Sandbox.ModAPI.IMyAirtightDoorBase, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor, Sandbox.ModAPI.Ingame.IMyAirtightDoorBase
	{
		private MySoundPair m_sound;

		private MySoundPair m_openSound;

		private MySoundPair m_closeSound;

		protected float m_currOpening;

		protected float m_subpartMovementDistance = 2.5f;

		protected float m_openingSpeed = 0.3f;

		protected float m_currSpeed;

		private int m_lastUpdateTime;

		private static readonly float EPSILON = 1E-09f;

		protected List<MyEntitySubpart> m_subparts = new List<MyEntitySubpart>();

		protected List<HkConstraint> m_subpartConstraints = new List<HkConstraint>();

		protected List<HkFixedConstraintData> m_subpartConstraintsData = new List<HkFixedConstraintData>();

		protected static string[] m_emissiveTextureNames;

		protected Color m_prevEmissiveColor;

		protected float m_prevEmissivity = -1f;

		private HashSet<VRage.ModAPI.IMyEntity> m_children = new HashSet<VRage.ModAPI.IMyEntity>();

		private bool m_updated;

		private bool m_stateChange;

		DoorStatus Sandbox.ModAPI.Ingame.IMyDoor.Status
		{
			get
			{
				if ((bool)m_open)
				{
					if (!(1f - m_currOpening < EPSILON))
					{
						return DoorStatus.Opening;
					}
					return DoorStatus.Open;
				}
				if (!(m_currOpening < EPSILON))
				{
					return DoorStatus.Closing;
				}
				return DoorStatus.Closed;
			}
		}

		public float OpenRatio => m_currOpening;

		bool Sandbox.ModAPI.IMyDoor.IsFullyClosed => m_currOpening < EPSILON;

		private new MyAirtightDoorGenericDefinition BlockDefinition => (MyAirtightDoorGenericDefinition)base.BlockDefinition;

		public event Action<bool> DoorStateChanged;

		public event Action<Sandbox.ModAPI.IMyDoor, bool> OnDoorStateChanged;

		void Sandbox.ModAPI.Ingame.IMyDoor.OpenDoor()
		{
			if (base.IsWorking)
			{
				DoorStatus status = ((Sandbox.ModAPI.Ingame.IMyDoor)this).Status;
				if ((uint)status > 1u)
				{
					((Sandbox.ModAPI.Ingame.IMyDoor)this).ToggleDoor();
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyDoor.CloseDoor()
		{
			if (base.IsWorking)
			{
				DoorStatus status = ((Sandbox.ModAPI.Ingame.IMyDoor)this).Status;
				if ((uint)(status - 2) > 1u)
				{
					((Sandbox.ModAPI.Ingame.IMyDoor)this).ToggleDoor();
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyDoor.ToggleDoor()
		{
			if (base.IsWorking)
			{
				m_open.Value = !m_open;
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public MyAirtightDoorGeneric()
		{
			m_currOpening = 0f;
			m_currSpeed = 0f;
			m_open.ValueChanged += delegate
			{
				DoChangeOpenClose();
			};
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.ResourceSink = new MyResourceSinkComponent();
			base.ResourceSink.Init(MyStringHash.GetOrCompute(BlockDefinition.ResourceSinkGroup), BlockDefinition.PowerConsumptionMoving, UpdatePowerInput);
			base.Init(builder, cubeGrid);
			base.NeedsWorldMatrix = false;
			MyObjectBuilder_AirtightDoorGeneric myObjectBuilder_AirtightDoorGeneric = (MyObjectBuilder_AirtightDoorGeneric)builder;
			m_open.SetLocalValue(myObjectBuilder_AirtightDoorGeneric.Open);
			m_currOpening = MathHelper.Clamp(myObjectBuilder_AirtightDoorGeneric.CurrOpening, 0f, 1f);
			m_openingSpeed = BlockDefinition.OpeningSpeed;
			m_sound = new MySoundPair(BlockDefinition.Sound);
			m_openSound = new MySoundPair(BlockDefinition.OpenSound);
			m_closeSound = new MySoundPair(BlockDefinition.CloseSound);
			m_subpartMovementDistance = BlockDefinition.SubpartMovementDistance;
			if (!base.Enabled || !base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				UpdateDoorPosition();
			}
			OnStateChange();
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.Update();
			if (!Sync.IsServer)
			{
				base.NeedsWorldMatrix = true;
			}
		}

		protected virtual void FillSubparts()
		{
		}

		protected override void BeforeDelete()
		{
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			DisposeConstraints();
			base.BeforeDelete();
		}

		public override void OnRemovedFromScene(object source)
		{
			DisposeConstraints();
			base.OnRemovedFromScene(source);
		}

		private void InitSubparts()
		{
			FillSubparts();
			MyCubeGridRenderCell orAddCell = base.CubeGrid.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize);
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				subpart.Render.SetParent(0, orAddCell.ParentCullObject);
				subpart.NeedsWorldMatrix = false;
				subpart.InvalidateOnMove = false;
			}
			UpdateEmissivity(force: true);
			DisposeConstraints();
			if (!base.CubeGrid.CreatePhysics)
			{
				UpdateDoorPosition();
				return;
			}
			foreach (MyEntitySubpart subpart2 in m_subparts)
			{
				if (subpart2.Physics != null)
				{
					subpart2.Physics.Close();
					subpart2.Physics = null;
				}
			}
			if (base.CubeGrid.Projector != null)
			{
				UpdateDoorPosition();
				return;
			}
			CreateConstraints();
			UpdateDoorPosition();
		}

		private void CreateConstraints()
		{
			UpdateDoorPosition();
			bool flag = !Sync.IsServer;
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				if (subpart.Physics == null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
				{
					HkShape shape = subpart.ModelCollision.HavokCollisionShapes[0];
					subpart.Physics = new MyPhysicsBody(subpart, flag ? RigidBodyFlag.RBF_STATIC : (RigidBodyFlag.RBF_DOUBLED_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS));
					Vector3 center = subpart.PositionComp.LocalVolume.Center;
					HkMassProperties value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(subpart.PositionComp.LocalAABB.HalfExtents, 100f);
					value.Volume = subpart.PositionComp.LocalAABB.Volume();
					subpart.GetPhysicsBody().CreateFromCollisionObject(shape, center, subpart.WorldMatrix, value, 9);
					((MyPhysicsBody)subpart.Physics).IsSubpart = true;
				}
				if (subpart.Physics != null)
				{
					if (!flag)
					{
						CreateSubpartConstraint(subpart, out HkFixedConstraintData constraintData, out HkConstraint constraint);
						m_subpartConstraintsData.Add(constraintData);
						m_subpartConstraints.Add(constraint);
						base.CubeGrid.Physics.AddConstraint(constraint);
						constraint.SetVirtualMassInverse(Vector4.Zero, Vector4.One);
					}
					else
					{
						subpart.Physics.Enabled = true;
					}
				}
			}
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_OnHavokSystemIDChanged;
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, refreshInPlace: false);
			}
		}

		private void DisposeConstraints()
		{
			for (int i = 0; i < m_subpartConstraints.Count; i++)
			{
				HkConstraint constraint = m_subpartConstraints[i];
				HkFixedConstraintData constraintData = m_subpartConstraintsData[i];
				DisposeSubpartConstraint(ref constraint, ref constraintData);
			}
			m_subpartConstraints.Clear();
			m_subpartConstraintsData.Clear();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_AirtightDoorGeneric obj = (MyObjectBuilder_AirtightDoorGeneric)base.GetObjectBuilderCubeBlock(copy);
			obj.Open = m_open;
			obj.CurrOpening = m_currOpening;
			return obj;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.CubeGrid.Physics != null && m_subparts.Count != 0 && m_currSpeed != 0f && base.Enabled && base.IsWorking && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				UpdateDoorPosition();
			}
		}

		public override bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
		{
			base.Hierarchy.GetChildrenRecursive(m_children);
			foreach (MyEntity child in m_children)
			{
				MyModel model = child.Model;
				if (model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB(child, ref aabb))
				{
					return true;
				}
			}
			return base.Model?.GetTrianglePruningStructure().GetIntersectionWithAABB(this, ref aabb) ?? false;
		}

		public override void UpdateBeforeSimulation()
		{
			if (m_stateChange && (((bool)m_open && 1f - m_currOpening < EPSILON) || (!m_open && m_currOpening < EPSILON)))
			{
				if (m_soundEmitter != null && m_soundEmitter.Loop)
				{
					m_soundEmitter.StopSound(forced: false);
					m_soundEmitter.PlaySingleSound(m_sound, stopPrevious: false, skipIntro: false, skipToEnd: true);
				}
				m_currSpeed = 0f;
				if (!base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
				base.ResourceSink.Update();
				RaisePropertiesChanged();
				if (!m_open)
				{
					this.DoorStateChanged.InvokeIfNotNull(m_open);
					this.OnDoorStateChanged.InvokeIfNotNull(this, m_open);
				}
				m_stateChange = false;
			}
			if (m_soundEmitter != null && base.Enabled && base.IsWorking && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && m_currSpeed != 0f)
			{
				if (base.Open)
				{
					if (m_openSound.Equals(MySoundPair.Empty))
					{
						StartSound(m_sound);
					}
					else
					{
						StartSound(m_openSound);
					}
				}
				else if (m_closeSound.Equals(MySoundPair.Empty))
				{
					StartSound(m_sound);
				}
				else
				{
					StartSound(m_closeSound);
				}
			}
			base.UpdateBeforeSimulation();
			UpdateCurrentOpening();
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		private void UpdateCurrentOpening()
		{
			if (base.Enabled && base.IsWorking && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				float num = (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime) / 1000f;
				float num2 = m_currSpeed * num;
				m_currOpening = MathHelper.Clamp(m_currOpening + num2, 0f, 1f);
			}
		}

		protected abstract void UpdateDoorPosition();

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
		}

		public override void OnAddedToScene(object source)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.OnAddedToScene(source);
			UpdateEmissivity();
		}

		protected virtual void UpdateEmissivity(bool force = false)
		{
		}

		protected void SetEmissive(Color color, float emissivity = 1f, bool force = false)
		{
			if (base.Render.RenderObjectIDs[0] != uint.MaxValue && (force || color != m_prevEmissiveColor || m_prevEmissivity != emissivity))
			{
				string[] emissiveTextureNames = m_emissiveTextureNames;
				foreach (string emissiveName in emissiveTextureNames)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], emissiveName, color, emissivity);
				}
				m_prevEmissiveColor = color;
				m_prevEmissivity = emissivity;
			}
		}

		public void ChangeOpenClose(bool open)
		{
			if (open != (bool)m_open)
			{
				m_open.Value = open;
			}
		}

		internal void DoChangeOpenClose()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: false);
			}
			OnStateChange();
			RaisePropertiesChanged();
		}

		private void OnStateChange()
		{
			if ((bool)m_open)
			{
				m_currSpeed = m_openingSpeed;
			}
			else
			{
				m_currSpeed = 0f - m_openingSpeed;
			}
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds - 1;
			UpdateCurrentOpening();
			UpdateDoorPosition();
			if ((bool)m_open)
			{
				this.DoorStateChanged.InvokeIfNotNull(m_open);
				this.OnDoorStateChanged.InvokeIfNotNull(this, m_open);
			}
			m_stateChange = true;
		}

		private void RecreateConstraints()
		{
			MyCubeGridRenderCell orAddCell = base.CubeGrid.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize);
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				if (subpart.Closed || subpart.MarkedForClose)
				{
					return;
				}
				subpart.Render.SetParent(0, orAddCell.ParentCullObject);
				subpart.NeedsWorldMatrix = false;
				subpart.InvalidateOnMove = false;
			}
			DisposeConstraints();
			if (base.InScene && base.CubeGrid.Physics != null && (base.CubeGrid.Physics.IsInWorld || base.CubeGrid.Physics.IsInWorldWelded()))
			{
				CreateConstraints();
			}
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, refreshInPlace: false);
			}
			UpdateDoorPosition();
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
			UpdateEmissivity();
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			base.ResourceSink.Update();
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, refreshInPlace: true);
			}
			base.OnBuildSuccess(builtBy, instantBuild);
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			oldGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_OnHavokSystemIDChanged;
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, refreshInPlace: true);
			}
			if (base.InScene)
			{
				MyCubeGridRenderCell orAddCell = base.CubeGrid.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize);
				foreach (MyEntitySubpart subpart in m_subparts)
				{
					subpart.Render.SetParent(0, orAddCell.ParentCullObject);
				}
			}
			base.OnCubeGridChanged(oldGrid);
		}

		private void CubeGrid_OnHavokSystemIDChanged(int id)
		{
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, refreshInPlace: true);
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			InitSubparts();
			RecreateConstraints();
		}

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			UpdateDoorPosition();
		}

		internal void UpdateHavokCollisionSystemID(int havokCollisionSystemID, bool refreshInPlace)
		{
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				MyDoorBase.SetupDoorSubpart(subpart, havokCollisionSystemID, refreshInPlace);
			}
		}

		protected float UpdatePowerInput()
		{
			if (!base.Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			if (m_currSpeed == 0f)
			{
				return BlockDefinition.PowerConsumptionIdle;
			}
			return BlockDefinition.PowerConsumptionMoving;
		}

		protected bool IsEnoughPower()
		{
			if (base.ResourceSink != null)
			{
				return base.ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, BlockDefinition.PowerConsumptionMoving);
			}
			return false;
		}

		private void StartSound(MySoundPair cuePair)
		{
			if (m_soundEmitter.Sound == null || !m_soundEmitter.Sound.IsPlaying || (!(m_soundEmitter.SoundId == cuePair.Arcade) && !(m_soundEmitter.SoundId == cuePair.Realistic)))
			{
				m_soundEmitter.StopSound(forced: true);
				m_soundEmitter.PlaySingleSound(cuePair, stopPrevious: true);
			}
		}

		protected override void Closing()
		{
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.Closing();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			UpdateEmissivity();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			UpdateEmissivity();
		}
	}
}
