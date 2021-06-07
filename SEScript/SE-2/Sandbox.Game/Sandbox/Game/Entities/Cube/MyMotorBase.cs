using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyMotorBase),
		typeof(Sandbox.ModAPI.Ingame.IMyMotorBase)
	})]
	public abstract class MyMotorBase : MyMechanicalConnectionBlockBase, Sandbox.ModAPI.IMyMotorBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyMotorBase
	{
		protected class m_dummyDisplacement_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType dummyDisplacement;
				ISyncType result = dummyDisplacement = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorBase)P_0).m_dummyDisplacement = (Sync<float, SyncDirection.BothWays>)dummyDisplacement;
				return result;
			}
		}

		private const string ROTOR_DUMMY_KEY = "electric_motor";

		private new static List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();

		private Vector3 m_dummyPos;

		protected readonly Sync<float, SyncDirection.BothWays> m_dummyDisplacement;

		public Vector3 DummyPosition
		{
			get
			{
				Vector3 zero = Vector3.Zero;
				if (!(m_dummyPos.Length() > 0f))
				{
					zero = new Vector3(0f, m_dummyDisplacement, 0f);
				}
				else
				{
					zero = Vector3.DominantAxisProjection(m_dummyPos);
					zero.Normalize();
					zero *= (float)m_dummyDisplacement;
				}
				return Vector3.Transform(m_dummyPos + zero, base.PositionComp.LocalMatrix);
			}
		}

		public float DummyDisplacement
		{
			get
			{
				return (float)m_dummyDisplacement + ModelDummyDisplacement;
			}
			set
			{
				if (!m_dummyDisplacement.Value.IsEqual(value - ModelDummyDisplacement))
				{
					m_dummyDisplacement.Value = value - ModelDummyDisplacement;
				}
			}
		}

		public MyCubeGrid RotorGrid => base.TopGrid;

		public MyCubeBlock Rotor => base.TopBlock;

		public float RequiredPowerInput => MotorDefinition.RequiredPowerInput;

		protected MyMotorStatorDefinition MotorDefinition => (MyMotorStatorDefinition)base.BlockDefinition;

		protected virtual float ModelDummyDisplacement => 0f;

		public Vector3 RotorAngularVelocity => base.CubeGrid.Physics.RigidBody.AngularVelocity - base.TopGrid.Physics.RigidBody.AngularVelocity;

		public Vector3 AngularVelocity
		{
			get
			{
				return base.TopGrid.Physics.RigidBody.AngularVelocity;
			}
			set
			{
				base.TopGrid.Physics.RigidBody.AngularVelocity = value;
			}
		}

		public float MaxRotorAngularVelocity => MyGridPhysics.GetShipMaxAngularVelocity(base.CubeGrid.GridSizeEnum);

		VRage.Game.ModAPI.IMyCubeGrid Sandbox.ModAPI.IMyMotorBase.RotorGrid => base.TopGrid;

		VRage.Game.ModAPI.IMyCubeBlock Sandbox.ModAPI.IMyMotorBase.Rotor => base.TopBlock;

		event Action<Sandbox.ModAPI.IMyMotorBase> Sandbox.ModAPI.IMyMotorBase.AttachedEntityChanged
		{
			add
			{
				base.AttachedEntityChanged += GetDelegate(value);
			}
			remove
			{
				base.AttachedEntityChanged -= GetDelegate(value);
			}
		}

		protected void CheckDisplacementLimits()
		{
			if (base.TopGrid == null)
			{
				return;
			}
			if (base.TopGrid.GridSizeEnum == MyCubeSize.Small)
			{
				if (DummyDisplacement < MotorDefinition.RotorDisplacementMinSmall)
				{
					DummyDisplacement = MotorDefinition.RotorDisplacementMinSmall;
				}
				if (DummyDisplacement > MotorDefinition.RotorDisplacementMaxSmall)
				{
					DummyDisplacement = MotorDefinition.RotorDisplacementMaxSmall;
				}
			}
			else
			{
				if (DummyDisplacement < MotorDefinition.RotorDisplacementMin)
				{
					DummyDisplacement = MotorDefinition.RotorDisplacementMin;
				}
				if (DummyDisplacement > MotorDefinition.RotorDisplacementMax)
				{
					DummyDisplacement = MotorDefinition.RotorDisplacementMax;
				}
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

		protected virtual float ComputeRequiredPowerInput()
		{
			if (!base.CheckIsWorking())
			{
				return 0f;
			}
			return base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(MotorDefinition.ResourceSinkGroup, MotorDefinition.RequiredPowerInput, ComputeRequiredPowerInput);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.Update();
			m_dummyDisplacement.SetLocalValue(0f);
			m_dummyDisplacement.ValueChanged += m_dummyDisplacement_ValueChanged;
			LoadDummyPosition();
			MyObjectBuilder_MotorBase myObjectBuilder_MotorBase = objectBuilder as MyObjectBuilder_MotorBase;
			if (Sync.IsServer && myObjectBuilder_MotorBase.RotorEntityId.HasValue && myObjectBuilder_MotorBase.RotorEntityId.Value != 0L)
			{
				m_connectionState.Value = new State
				{
					TopBlockId = myObjectBuilder_MotorBase.RotorEntityId,
					Welded = (myObjectBuilder_MotorBase.WeldedEntityId.HasValue || myObjectBuilder_MotorBase.ForceWeld)
				};
			}
			AddDebugRenderComponent(new MyDebugRenderComponentMotorBase(this));
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		private void m_dummyDisplacement_ValueChanged(SyncBase obj)
		{
			if (Sync.IsServer)
			{
				CheckDisplacementLimits();
			}
			if (m_constraint != null)
			{
				base.CubeGrid.Physics.RigidBody.Activate();
				if (base.TopGrid != null)
				{
					base.TopGrid.Physics.RigidBody.Activate();
				}
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

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		private void LoadDummyPosition()
		{
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(base.BlockDefinition.Model).Dummies)
			{
				if (dummy.Key.StartsWith("electric_motor", StringComparison.InvariantCultureIgnoreCase))
				{
					m_dummyPos = Matrix.Normalize(dummy.Value.Matrix).Translation;
					break;
				}
			}
		}

		protected override MatrixD GetTopGridMatrix()
		{
			return MatrixD.CreateWorld(Vector3D.Transform(DummyPosition, base.CubeGrid.WorldMatrix), base.WorldMatrix.Forward, base.WorldMatrix.Up);
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			UpdateSoundState();
		}

		public override void ComputeTopQueryBox(out Vector3D pos, out Vector3 halfExtents, out Quaternion orientation)
		{
			MatrixD worldMatrix = base.WorldMatrix;
			orientation = Quaternion.CreateFromRotationMatrix(worldMatrix);
			halfExtents = Vector3.One * base.CubeGrid.GridSize * 0.35f;
			halfExtents.Y = base.CubeGrid.GridSize * 0.25f;
			pos = worldMatrix.Translation + 0.35f * base.CubeGrid.GridSize * base.WorldMatrix.Up;
		}

		protected virtual void UpdateSoundState()
		{
			if (!MySandboxGame.IsGameReady || m_soundEmitter == null || !base.IsWorking)
			{
				return;
			}
			if (base.TopGrid == null || base.TopGrid.Physics == null)
			{
				m_soundEmitter.StopSound(forced: true);
				return;
			}
			if (base.IsWorking && Math.Abs(base.TopGrid.Physics.RigidBody.DeltaAngle.W) > 0.00025f)
			{
				m_soundEmitter.PlaySingleSound(base.BlockDefinition.PrimarySound, stopPrevious: true);
			}
			else
			{
				m_soundEmitter.StopSound(forced: false);
			}
			if (m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying)
			{
				float semitones = 4f * (Math.Abs(RotorAngularVelocity.Length()) - 0.5f * MaxRotorAngularVelocity) / MaxRotorAngularVelocity;
				m_soundEmitter.Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio(semitones);
			}
		}

		protected override Vector3D TransformPosition(ref Vector3D position)
		{
			return Vector3D.Transform(DummyPosition, base.CubeGrid.WorldMatrix);
		}

		protected override void DisposeConstraint(MyCubeGrid topGrid)
		{
			if (m_constraint != null)
			{
				base.CubeGrid.Physics.RemoveConstraint(m_constraint);
				m_constraint.Dispose();
				m_constraint = null;
			}
			base.DisposeConstraint(topGrid);
		}

		private Action<MyMechanicalConnectionBlockBase> GetDelegate(Action<Sandbox.ModAPI.IMyMotorBase> value)
		{
			return (Action<MyMechanicalConnectionBlockBase>)Delegate.CreateDelegate(typeof(Action<MyMechanicalConnectionBlockBase>), value.Target, value.Method);
		}

		void Sandbox.ModAPI.IMyMotorBase.Attach(Sandbox.ModAPI.IMyMotorRotor rotor, bool updateGroup)
		{
			((Sandbox.ModAPI.IMyMechanicalConnectionBlock)this).Attach((Sandbox.ModAPI.IMyAttachableTopBlock)rotor, updateGroup);
		}
	}
}
