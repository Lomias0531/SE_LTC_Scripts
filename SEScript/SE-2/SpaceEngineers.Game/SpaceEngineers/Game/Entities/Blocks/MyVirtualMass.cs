using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_VirtualMass))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyArtificialMassBlock),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyArtificialMassBlock)
	})]
	public class MyVirtualMass : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyArtificialMassBlock, SpaceEngineers.Game.ModAPI.IMyVirtualMass, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyVirtualMass, SpaceEngineers.Game.ModAPI.Ingame.IMyArtificialMassBlock
	{
		private new MyVirtualMassDefinition BlockDefinition => (MyVirtualMassDefinition)base.BlockDefinition;

		float SpaceEngineers.Game.ModAPI.Ingame.IMyVirtualMass.VirtualMass => BlockDefinition.VirtualMass;

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.ResourceSink = new MyResourceSinkComponent();
			base.ResourceSink.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, CalculateRequiredPowerInput);
			base.Init(objectBuilder, cubeGrid);
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink.Update();
			if (base.Physics != null)
			{
				base.Physics.Close();
			}
			HkBoxShape shape = new HkBoxShape(new Vector3(cubeGrid.GridSize / 3f));
			HkMassProperties value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(shape.HalfExtents, BlockDefinition.VirtualMass);
			base.Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
			base.Physics.IsPhantom = false;
			base.Physics.CreateFromCollisionObject(shape, Vector3.Zero, base.WorldMatrix, value, 25);
			base.Physics.Enabled = (base.IsWorking && cubeGrid.Physics != null && cubeGrid.Physics.Enabled);
			base.Physics.RigidBody.Activate();
			shape.Base.RemoveReference();
			SetDetailedInfoDirty();
			base.NeedsWorldMatrix = true;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			base.ResourceSink.Update();
			UpdatePhysics();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			base.OnBuildSuccess(builtBy, instantBuild);
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			if (base.Physics != null)
			{
				UpdatePhysics();
			}
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			base.OnEnabledChanged();
		}

		private float CalculateRequiredPowerInput()
		{
			if (base.Enabled && base.IsFunctional)
			{
				return BlockDefinition.RequiredPowerInput;
			}
			return 0f;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.ResourceSink.Update();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentMass));
			detailedInfo.Append(base.IsWorking ? BlockDefinition.VirtualMass.ToString() : "0");
			detailedInfo.Append(" kg\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			UpdatePhysics();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private void UpdatePhysics()
		{
			if (base.Physics != null)
			{
				base.Physics.Enabled = (base.IsWorking && base.CubeGrid.Physics != null && base.CubeGrid.Physics.Enabled);
				if (base.IsWorking)
				{
					base.Physics.RigidBody.Activate();
				}
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}
	}
}
