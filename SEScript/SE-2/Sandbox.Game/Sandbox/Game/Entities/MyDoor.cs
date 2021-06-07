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
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Door))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyDoor),
		typeof(Sandbox.ModAPI.Ingame.IMyDoor)
	})]
	public class MyDoor : MyDoorBase, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor
	{
		private class Sandbox_Game_Entities_MyDoor_003C_003EActor : IActivator, IActivator<MyDoor>
		{
			private sealed override object CreateInstance()
			{
				return new MyDoor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDoor CreateInstance()
			{
				return new MyDoor();
			}

			MyDoor IActivator<MyDoor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float CLOSED_DISSASEMBLE_RATIO = 3.3f;

		private static readonly float EPSILON = 1E-09f;

		private MySoundPair m_openSound;

		private MySoundPair m_closeSound;

		private float m_currOpening;

		private float m_currSpeed;

		private float m_openingSpeed;

		private int m_lastUpdateTime;

		private bool m_physicsInitiated;

		private MyEntitySubpart m_leftSubpart;

		private MyEntitySubpart m_rightSubpart;

		private HkFixedConstraintData m_leftConstraintData;

		private HkConstraint m_leftConstraint;

		private HkFixedConstraintData m_rightConstraintData;

		private HkConstraint m_rightConstraint;

		public float MaxOpen = 1.2f;

		public override float DisassembleRatio => base.DisassembleRatio * 3.3f;

		DoorStatus Sandbox.ModAPI.Ingame.IMyDoor.Status
		{
			get
			{
				if ((bool)m_open)
				{
					if (!(MaxOpen - m_currOpening < EPSILON))
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

		public float OpenRatio => m_currOpening / MaxOpen;

		bool Sandbox.ModAPI.IMyDoor.IsFullyClosed => m_currOpening < EPSILON;

		public event Action<bool> DoorStateChanged;

		public event Action<Sandbox.ModAPI.IMyDoor, bool> OnDoorStateChanged;

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public MyDoor()
		{
			m_currOpening = 0f;
			m_currSpeed = 0f;
			m_open.AlwaysReject();
			m_open.ValueChanged += delegate
			{
				OnStateChange();
			};
		}

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
				SetOpenRequest(!base.Open, base.OwnerId);
			}
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			base.ResourceSink.Update();
			base.OnBuildSuccess(builtBy, instantBuild);
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			m_physicsInitiated = false;
			MyDoorDefinition myDoorDefinition = base.BlockDefinition as MyDoorDefinition;
			MyStringHash orCompute;
			if (myDoorDefinition != null)
			{
				MaxOpen = myDoorDefinition.MaxOpen;
				m_openSound = new MySoundPair(myDoorDefinition.OpenSound);
				m_closeSound = new MySoundPair(myDoorDefinition.CloseSound);
				orCompute = MyStringHash.GetOrCompute(myDoorDefinition.ResourceSinkGroup);
				m_openingSpeed = myDoorDefinition.OpeningSpeed;
			}
			else
			{
				MaxOpen = 1.2f;
				m_openSound = new MySoundPair("BlockDoorSmallOpen");
				m_closeSound = new MySoundPair("BlockDoorSmallClose");
				orCompute = MyStringHash.GetOrCompute("Doors");
				m_openingSpeed = 1f;
			}
			MyResourceSinkComponent sinkComp = new MyResourceSinkComponent();
			sinkComp.Init(orCompute, 3E-05f, () => (!base.Enabled || !base.IsFunctional) ? 0f : sinkComp.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			sinkComp.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = sinkComp;
			base.Init(builder, cubeGrid);
			base.NeedsWorldMatrix = false;
			MyObjectBuilder_Door myObjectBuilder_Door = (MyObjectBuilder_Door)builder;
			m_open.SetLocalValue(myObjectBuilder_Door.State);
			if (myObjectBuilder_Door.Opening == -1f)
			{
				m_currOpening = (base.IsFunctional ? 0f : MaxOpen);
				m_open.SetLocalValue(!base.IsFunctional);
			}
			else
			{
				m_currOpening = MathHelper.Clamp(myObjectBuilder_Door.Opening, 0f, MaxOpen);
			}
			if (!base.Enabled || !base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				UpdateSlidingDoorsPosition();
			}
			OnStateChange();
			if ((bool)m_open && base.Open && m_currOpening == MaxOpen)
			{
				UpdateSlidingDoorsPosition();
			}
			sinkComp.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			if (!Sync.IsServer)
			{
				base.NeedsWorldMatrix = true;
			}
		}

		private void InitSubparts()
		{
			DisposeSubpartConstraint(ref m_leftConstraint, ref m_leftConstraintData);
			DisposeSubpartConstraint(ref m_rightConstraint, ref m_rightConstraintData);
			base.Subparts.TryGetValue("DoorLeft", out m_leftSubpart);
			base.Subparts.TryGetValue("DoorRight", out m_rightSubpart);
			MyCubeGridRenderCell orAddCell = base.CubeGrid.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize);
			if (m_leftSubpart != null)
			{
				m_leftSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
				m_leftSubpart.NeedsWorldMatrix = false;
				m_leftSubpart.InvalidateOnMove = false;
			}
			if (m_rightSubpart != null)
			{
				m_rightSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
				m_rightSubpart.NeedsWorldMatrix = false;
				m_rightSubpart.InvalidateOnMove = false;
			}
			if (base.CubeGrid.Projector != null)
			{
				UpdateSlidingDoorsPosition();
				return;
			}
			if (!base.CubeGrid.CreatePhysics)
			{
				UpdateSlidingDoorsPosition();
				return;
			}
			if (m_leftSubpart != null && m_leftSubpart.Physics != null)
			{
				m_leftSubpart.Physics.Close();
				m_leftSubpart.Physics = null;
			}
			if (m_rightSubpart != null && m_rightSubpart.Physics != null)
			{
				m_rightSubpart.Physics.Close();
				m_rightSubpart.Physics = null;
			}
			CreateConstraints();
			m_physicsInitiated = true;
			UpdateSlidingDoorsPosition();
		}

		private void CreateConstraints()
		{
			UpdateSlidingDoorsPosition();
			CreateConstraint(m_leftSubpart, ref m_leftConstraint, ref m_leftConstraintData);
			CreateConstraint(m_rightSubpart, ref m_rightConstraint, ref m_rightConstraintData);
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_OnHavokSystemIDChanged;
		}

		private void CreateConstraint(MyEntitySubpart subpart, ref HkConstraint constraint, ref HkFixedConstraintData constraintData)
		{
			if (subpart == null)
			{
				return;
			}
			bool flag = !Sync.IsServer;
			if (subpart.Physics == null)
			{
				HkShape[] havokCollisionShapes = subpart.ModelCollision.HavokCollisionShapes;
				if (havokCollisionShapes != null && havokCollisionShapes.Length != 0)
				{
					MyPhysicsBody myPhysicsBody = new MyPhysicsBody(subpart, flag ? RigidBodyFlag.RBF_STATIC : (RigidBodyFlag.RBF_DOUBLED_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS));
					myPhysicsBody.IsSubpart = true;
					subpart.Physics = myPhysicsBody;
					HkShape shape = havokCollisionShapes[0];
					MyPositionComponentBase positionComp = subpart.PositionComp;
					Vector3 center = positionComp.LocalVolume.Center;
					HkMassProperties value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(positionComp.LocalAABB.HalfExtents, 100f);
					int collisionFilter = base.CubeGrid.IsStatic ? 9 : 16;
					myPhysicsBody.CreateFromCollisionObject(shape, center, positionComp.WorldMatrix, value, collisionFilter);
				}
			}
			if (!flag)
			{
				CreateSubpartConstraint(subpart, out constraintData, out constraint);
				base.CubeGrid.Physics.AddConstraint(constraint);
				constraint.SetVirtualMassInverse(Vector4.Zero, Vector4.One);
			}
			else
			{
				subpart.Physics.Enabled = true;
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.OnAddedToScene(source);
		}

		public override void OnRemovedFromScene(object source)
		{
			DisposeSubpartConstraint(ref m_leftConstraint, ref m_leftConstraintData);
			DisposeSubpartConstraint(ref m_rightConstraint, ref m_rightConstraintData);
			base.OnRemovedFromScene(source);
		}

		protected override void BeforeDelete()
		{
			DisposeSubpartConstraint(ref m_leftConstraint, ref m_leftConstraintData);
			DisposeSubpartConstraint(ref m_rightConstraint, ref m_rightConstraintData);
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			base.BeforeDelete();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Door obj = (MyObjectBuilder_Door)base.GetObjectBuilderCubeBlock(copy);
			obj.State = base.Open;
			obj.Opening = m_currOpening;
			obj.OpenSound = m_openSound.ToString();
			obj.CloseSound = m_closeSound.ToString();
			return obj;
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
				if (m_leftSubpart != null)
				{
					m_leftSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
				}
				if (m_rightSubpart != null)
				{
					m_rightSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
				}
			}
			base.OnCubeGridChanged(oldGrid);
		}

		private void OnStateChange()
		{
			if (m_leftSubpart != null || m_rightSubpart != null)
			{
				m_currSpeed = (m_open ? m_openingSpeed : (0f - m_openingSpeed));
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				UpdateCurrentOpening();
				UpdateSlidingDoorsPosition();
				if ((bool)m_open)
				{
					this.DoorStateChanged.InvokeIfNotNull(m_open);
					this.OnDoorStateChanged.InvokeIfNotNull(this, m_open);
				}
			}
		}

		private void StartSound(MySoundPair cuePair)
		{
			if (m_soundEmitter != null && (m_soundEmitter.Sound == null || !m_soundEmitter.Sound.IsPlaying || (!(m_soundEmitter.SoundId == cuePair.Arcade) && !(m_soundEmitter.SoundId == cuePair.Realistic))))
			{
				m_soundEmitter.StopSound(forced: true);
				m_soundEmitter.PlaySingleSound(cuePair, stopPrevious: true);
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.CubeGrid.Physics != null && ((m_currOpening != 0f && !(m_currOpening > MaxOpen)) || m_currSpeed != 0f))
			{
				UpdateSlidingDoorsPosition();
			}
		}

		public override void UpdateBeforeSimulation()
		{
			if ((base.Open && m_currOpening == MaxOpen) || (!base.Open && m_currOpening == 0f))
			{
				if (m_soundEmitter != null && m_soundEmitter.IsPlaying && m_soundEmitter.Loop && (base.BlockDefinition.DamagedSound == null || m_soundEmitter.SoundId != base.BlockDefinition.DamagedSound.SoundId))
				{
					m_soundEmitter.StopSound(forced: false);
				}
				if (m_physicsInitiated && !base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
				m_currSpeed = 0f;
				if (!m_open)
				{
					this.DoorStateChanged.InvokeIfNotNull(m_open);
					this.OnDoorStateChanged.InvokeIfNotNull(this, m_open);
				}
				return;
			}
			if (m_soundEmitter != null && base.Enabled && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				if (base.Open)
				{
					StartSound(m_openSound);
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
			if (m_currSpeed != 0f && base.Enabled && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				float num = (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime) / 1000f;
				float num2 = m_currSpeed * num;
				m_currOpening = MathHelper.Clamp(m_currOpening + num2, 0f, MaxOpen);
			}
		}

		private void UpdateSlidingDoorsPosition()
		{
			if (base.CubeGrid.Physics == null)
			{
				return;
			}
			bool flag = !Sync.IsServer;
			float num = m_currOpening * 0.65f;
			Vector3 position;
			if (m_leftSubpart != null)
			{
				position = new Vector3(0f - num, 0f, 0f);
				Matrix.CreateTranslation(ref position, out Matrix result);
				Matrix renderLocal = result * base.PositionComp.LocalMatrix;
				Matrix matrix = Matrix.Identity;
				matrix.Translation = new Vector3(0.35f, 0f, 0f);
				Matrix.Multiply(ref matrix, ref result, out matrix);
				m_leftSubpart.PositionComp.SetLocalMatrix(ref matrix, flag ? null : m_leftSubpart.Physics, updateWorld: true, ref renderLocal, forceUpdateRender: true);
				if (m_leftConstraintData != null)
				{
					if (base.CubeGrid.Physics != null)
					{
						base.CubeGrid.Physics.RigidBody.Activate();
					}
					m_leftSubpart.Physics.RigidBody.Activate();
					position = new Vector3(num, 0f, 0f);
					Matrix.CreateTranslation(ref position, out Matrix result2);
					m_leftConstraintData.SetInBodySpace(base.PositionComp.LocalMatrix, result2, base.CubeGrid.Physics, (MyPhysicsBody)m_leftSubpart.Physics);
				}
			}
			if (m_rightSubpart == null)
			{
				return;
			}
			position = new Vector3(num, 0f, 0f);
			Matrix.CreateTranslation(ref position, out Matrix result3);
			Matrix renderLocal2 = result3 * base.PositionComp.LocalMatrix;
			Matrix matrix2 = Matrix.Identity;
			matrix2.Translation = new Vector3(-0.35f, 0f, 0f);
			Matrix.Multiply(ref matrix2, ref result3, out matrix2);
			m_rightSubpart.PositionComp.SetLocalMatrix(ref matrix2, flag ? null : m_rightSubpart.Physics, updateWorld: true, ref renderLocal2, forceUpdateRender: true);
			if (m_rightConstraintData != null)
			{
				if (base.CubeGrid.Physics != null)
				{
					base.CubeGrid.Physics.RigidBody.Activate();
				}
				m_rightSubpart.Physics.RigidBody.Activate();
				Matrix pivotB = Matrix.CreateTranslation(new Vector3(0f - num, 0f, 0f));
				m_rightConstraintData.SetInBodySpace(base.PositionComp.LocalMatrix, pivotB, base.CubeGrid.Physics, (MyPhysicsBody)m_rightSubpart.Physics);
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

		public override void OnModelChange()
		{
			base.OnModelChange();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			InitSubparts();
			RecreateConstraints(base.CubeGrid, refreshInPlace: false);
		}

		private void CubeGrid_OnHavokSystemIDChanged(int id)
		{
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, refreshInPlace: true);
			}
		}

		private void RecreateConstraints(MyEntity obj, bool refreshInPlace)
		{
			if (obj != null && !obj.MarkedForClose && obj.GetPhysicsBody() != null && !obj.IsPreview && base.CubeGrid.Projector == null && (m_leftSubpart == null || (!m_leftSubpart.MarkedForClose && !m_leftSubpart.Closed)) && (m_rightSubpart == null || (!m_rightSubpart.MarkedForClose && !m_rightSubpart.Closed)))
			{
				MyCubeGridRenderCell orAddCell = base.CubeGrid.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize);
				if (m_leftSubpart != null)
				{
					m_leftSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
				}
				if (m_rightSubpart != null)
				{
					m_rightSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
				}
				DisposeSubpartConstraint(ref m_leftConstraint, ref m_leftConstraintData);
				DisposeSubpartConstraint(ref m_rightConstraint, ref m_rightConstraintData);
				if (base.InScene && base.CubeGrid.Physics != null && (base.CubeGrid.Physics.IsInWorld || base.CubeGrid.Physics.IsInWorldWelded()))
				{
					CreateConstraints();
				}
				if (obj.Physics != null)
				{
					UpdateHavokCollisionSystemID(obj.GetPhysicsBody().HavokCollisionSystemID, refreshInPlace);
				}
				UpdateSlidingDoorsPosition();
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		internal void UpdateHavokCollisionSystemID(int havokCollisionSystemID, bool refreshInPlace)
		{
			MyEntitySubpart[] array = new MyEntitySubpart[2]
			{
				m_rightSubpart,
				m_leftSubpart
			};
			foreach (MyEntitySubpart myEntitySubpart in array)
			{
				if (myEntitySubpart != null)
				{
					MyDoorBase.SetupDoorSubpart(myEntitySubpart, havokCollisionSystemID, refreshInPlace);
				}
			}
		}

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			UpdateSlidingDoorsPosition();
		}
	}
}
