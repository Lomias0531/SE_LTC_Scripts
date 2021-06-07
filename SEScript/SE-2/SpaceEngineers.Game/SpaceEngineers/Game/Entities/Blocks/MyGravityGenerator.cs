using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_GravityGenerator))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyGravityGenerator),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator)
	})]
	public class MyGravityGenerator : MyGravityGeneratorBase, SpaceEngineers.Game.ModAPI.IMyGravityGenerator, SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase, IMyGravityProvider, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator
	{
		protected class m_fieldSize_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType fieldSize;
				ISyncType result = fieldSize = new Sync<Vector3, SyncDirection.BothWays>(P_1, P_2);
				((MyGravityGenerator)P_0).m_fieldSize = (Sync<Vector3, SyncDirection.BothWays>)fieldSize;
				return result;
			}
		}

		private const int NUM_DECIMALS = 0;

		private BoundingBox m_gizmoBoundingBox;

		private readonly Sync<Vector3, SyncDirection.BothWays> m_fieldSize;

		private new MyGravityGeneratorDefinition BlockDefinition => (MyGravityGeneratorDefinition)base.BlockDefinition;

		public Vector3 FieldSize
		{
			get
			{
				return m_fieldSize;
			}
			set
			{
				if (m_fieldSize.Value != value)
				{
					Vector3 value2 = value;
					value2.X = MathHelper.Clamp(value2.X, BlockDefinition.MinFieldSize.X, BlockDefinition.MaxFieldSize.X);
					value2.Y = MathHelper.Clamp(value2.Y, BlockDefinition.MinFieldSize.Y, BlockDefinition.MaxFieldSize.Y);
					value2.Z = MathHelper.Clamp(value2.Z, BlockDefinition.MinFieldSize.Z, BlockDefinition.MaxFieldSize.Z);
					m_fieldSize.Value = value2;
				}
			}
		}

		Vector3 SpaceEngineers.Game.ModAPI.IMyGravityGenerator.FieldSize
		{
			get
			{
				return FieldSize;
			}
			set
			{
				FieldSize = value;
			}
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator.FieldWidth => m_fieldSize.Value.X;

		float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator.FieldHeight => m_fieldSize.Value.Y;

		float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator.FieldDepth => m_fieldSize.Value.Z;

		Vector3 SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator.FieldSize
		{
			get
			{
				return FieldSize;
			}
			set
			{
				FieldSize = value;
			}
		}

		public override BoundingBox? GetBoundingBox()
		{
			m_gizmoBoundingBox.Min = base.PositionComp.LocalVolume.Center - FieldSize / 2f;
			m_gizmoBoundingBox.Max = base.PositionComp.LocalVolume.Center + FieldSize / 2f;
			return m_gizmoBoundingBox;
		}

		public MyGravityGenerator()
		{
			CreateTerminalControls();
			m_fieldSize.ValueChanged += delegate
			{
				UpdateFieldShape();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyGravityGenerator>())
			{
				base.CreateTerminalControls();
				MyTerminalControlSlider<MyGravityGenerator> myTerminalControlSlider = new MyTerminalControlSlider<MyGravityGenerator>("Width", MySpaceTexts.BlockPropertyTitle_GravityFieldWidth, MySpaceTexts.BlockPropertyDescription_GravityFieldWidth);
				myTerminalControlSlider.SetLimits((MyGravityGenerator x) => x.BlockDefinition.MinFieldSize.X, (MyGravityGenerator x) => x.BlockDefinition.MaxFieldSize.X);
				myTerminalControlSlider.DefaultValue = 150f;
				myTerminalControlSlider.Getter = ((MyGravityGenerator x) => x.m_fieldSize.Value.X);
				myTerminalControlSlider.Setter = delegate(MyGravityGenerator x, float v)
				{
					Vector3 value3 = x.m_fieldSize;
					value3.X = v;
					x.m_fieldSize.Value = value3;
				};
				myTerminalControlSlider.Writer = delegate(MyGravityGenerator x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_fieldSize.Value.X, 0)).Append(" m");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MyGravityGenerator> myTerminalControlSlider2 = new MyTerminalControlSlider<MyGravityGenerator>("Height", MySpaceTexts.BlockPropertyTitle_GravityFieldHeight, MySpaceTexts.BlockPropertyDescription_GravityFieldHeight);
				myTerminalControlSlider2.SetLimits((MyGravityGenerator x) => x.BlockDefinition.MinFieldSize.Y, (MyGravityGenerator x) => x.BlockDefinition.MaxFieldSize.Y);
				myTerminalControlSlider2.DefaultValue = 150f;
				myTerminalControlSlider2.Getter = ((MyGravityGenerator x) => x.m_fieldSize.Value.Y);
				myTerminalControlSlider2.Setter = delegate(MyGravityGenerator x, float v)
				{
					Vector3 value2 = x.m_fieldSize;
					value2.Y = v;
					x.m_fieldSize.Value = value2;
				};
				myTerminalControlSlider2.Writer = delegate(MyGravityGenerator x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_fieldSize.Value.Y, 0)).Append(" m");
				};
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				MyTerminalControlSlider<MyGravityGenerator> myTerminalControlSlider3 = new MyTerminalControlSlider<MyGravityGenerator>("Depth", MySpaceTexts.BlockPropertyTitle_GravityFieldDepth, MySpaceTexts.BlockPropertyDescription_GravityFieldDepth);
				myTerminalControlSlider3.SetLimits((MyGravityGenerator x) => x.BlockDefinition.MinFieldSize.Z, (MyGravityGenerator x) => x.BlockDefinition.MaxFieldSize.Z);
				myTerminalControlSlider3.DefaultValue = 150f;
				myTerminalControlSlider3.Getter = ((MyGravityGenerator x) => x.m_fieldSize.Value.Z);
				myTerminalControlSlider3.Setter = delegate(MyGravityGenerator x, float v)
				{
					Vector3 value = x.m_fieldSize;
					value.Z = v;
					x.m_fieldSize.Value = value;
				};
				myTerminalControlSlider3.Writer = delegate(MyGravityGenerator x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_fieldSize.Value.Z, 0)).Append(" m");
				};
				myTerminalControlSlider3.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				MyTerminalControlSlider<MyGravityGenerator> myTerminalControlSlider4 = new MyTerminalControlSlider<MyGravityGenerator>("Gravity", MySpaceTexts.BlockPropertyTitle_GravityAcceleration, MySpaceTexts.BlockPropertyDescription_GravityAcceleration);
				myTerminalControlSlider4.SetLimits((MyGravityGenerator x) => x.BlockDefinition.MinGravityAcceleration, (MyGravityGenerator x) => x.BlockDefinition.MaxGravityAcceleration);
				myTerminalControlSlider4.DefaultValue = 9.81f;
				myTerminalControlSlider4.Getter = ((MyGravityGenerator x) => x.GravityAcceleration);
				myTerminalControlSlider4.Setter = delegate(MyGravityGenerator x, float v)
				{
					if (float.IsNaN(v) || float.IsInfinity(v))
					{
						v = 0f;
					}
					x.GravityAcceleration = v;
				};
				myTerminalControlSlider4.Writer = delegate(MyGravityGenerator x, StringBuilder result)
				{
					result.AppendFormat("{0:F1} m/s² ({1:F2} g)", x.GravityAcceleration, x.GravityAcceleration / 9.81f);
				};
				myTerminalControlSlider4.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_GravityGenerator myObjectBuilder_GravityGenerator = (MyObjectBuilder_GravityGenerator)objectBuilder;
			FieldSize = myObjectBuilder_GravityGenerator.FieldSize;
			m_fieldSize.SetLocalValue(FieldSize);
			m_gravityAcceleration.SetLocalValue(MathHelper.Clamp(myObjectBuilder_GravityGenerator.GravityAcceleration, BlockDefinition.MinGravityAcceleration, BlockDefinition.MaxGravityAcceleration));
			if (BlockDefinition.EmissiveColorPreset == MyStringHash.NullOrEmpty)
			{
				BlockDefinition.EmissiveColorPreset = MyStringHash.GetOrCompute("GravityBlock");
			}
		}

		protected override void InitializeSinkComponent()
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, CalculateRequiredPowerInput);
			base.ResourceSink = myResourceSinkComponent;
			if (base.CubeGrid.CreatePhysics)
			{
				base.ResourceSink.IsPoweredChanged += base.Receiver_IsPoweredChanged;
				base.ResourceSink.RequiredInputChanged += base.Receiver_RequiredInputChanged;
				base.IsWorkingChanged += base.OnIsWorkingChanged;
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_GravityGenerator obj = (MyObjectBuilder_GravityGenerator)base.GetObjectBuilderCubeBlock(copy);
			obj.FieldSize = m_fieldSize.Value;
			obj.GravityAcceleration = m_gravityAcceleration;
			return obj;
		}

		protected override float CalculateRequiredPowerInput()
		{
			if (base.Enabled && base.IsFunctional)
			{
				return 0.0003f * Math.Abs(m_gravityAcceleration) * (float)Math.Pow(m_fieldSize.Value.Volume, 0.35);
			}
			return 0f;
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0f, detailedInfo);
		}

		public override bool IsPositionInRange(Vector3D worldPoint)
		{
			Vector3 v = m_fieldSize.Value * 0.5f;
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(base.WorldMatrix.Translation, v, Quaternion.CreateFromRotationMatrix(base.WorldMatrix));
			return myOrientedBoundingBoxD.Contains(ref worldPoint);
		}

		public override void GetProxyAABB(out BoundingBoxD aabb)
		{
			Vector3 v = m_fieldSize.Value * 0.5f;
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(base.WorldMatrix.Translation, v, Quaternion.CreateFromRotationMatrix(base.WorldMatrix));
			aabb = myOrientedBoundingBoxD.GetAABB();
		}

		public override Vector3 GetWorldGravity(Vector3D worldPoint)
		{
			return Vector3.TransformNormal(Vector3.Down * base.GravityAcceleration, base.WorldMatrix);
		}

		protected override HkShape GetHkShape()
		{
			return new HkBoxShape(m_fieldSize.Value * 0.5f);
		}
	}
}
