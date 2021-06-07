using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
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
	[MyCubeBlockType(typeof(MyObjectBuilder_GravityGeneratorSphere))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyGravityGeneratorSphere),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorSphere)
	})]
	public class MyGravityGeneratorSphere : MyGravityGeneratorBase, SpaceEngineers.Game.ModAPI.IMyGravityGeneratorSphere, SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase, IMyGravityProvider, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorSphere
	{
		protected class m_radius_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType radius;
				ISyncType result = radius = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyGravityGeneratorSphere)P_0).m_radius = (Sync<float, SyncDirection.BothWays>)radius;
				return result;
			}
		}

		private const float DEFAULT_RADIUS = 100f;

		private readonly Sync<float, SyncDirection.BothWays> m_radius;

		private float m_defaultVolume;

		private new MyGravityGeneratorSphereDefinition BlockDefinition => (MyGravityGeneratorSphereDefinition)base.BlockDefinition;

		public float Radius
		{
			get
			{
				return m_radius;
			}
			set
			{
				m_radius.Value = value;
			}
		}

		private float MaxInput => (float)(Math.Pow(BlockDefinition.MaxRadius, BlockDefinition.ConsumptionPower) / (double)(float)Math.Pow(100.0, BlockDefinition.ConsumptionPower) * (double)BlockDefinition.BasePowerInput);

		float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase.Gravity => base.GravityAcceleration;

		float SpaceEngineers.Game.ModAPI.IMyGravityGeneratorSphere.Radius
		{
			get
			{
				return Radius;
			}
			set
			{
				Radius = MathHelper.Clamp(value, BlockDefinition.MinRadius, BlockDefinition.MaxRadius);
			}
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorSphere.Radius
		{
			get
			{
				return Radius;
			}
			set
			{
				Radius = MathHelper.Clamp(value, BlockDefinition.MinRadius, BlockDefinition.MaxRadius);
			}
		}

		public override float GetRadius()
		{
			return m_radius;
		}

		public MyGravityGeneratorSphere()
		{
			CreateTerminalControls();
			m_radius.ValueChanged += delegate
			{
				UpdateFieldShape();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyGravityGeneratorSphere>())
			{
				base.CreateTerminalControls();
				if (MyFakes.ENABLE_GRAVITY_GENERATOR_SPHERE)
				{
					MyTerminalControlSlider<MyGravityGeneratorSphere> obj = new MyTerminalControlSlider<MyGravityGeneratorSphere>("Radius", MySpaceTexts.BlockPropertyTitle_GravityFieldRadius, MySpaceTexts.BlockPropertyDescription_GravityFieldRadius)
					{
						DefaultValue = 100f,
						Getter = ((MyGravityGeneratorSphere x) => x.Radius),
						Setter = delegate(MyGravityGeneratorSphere x, float v)
						{
							if (v < x.BlockDefinition.MinRadius)
							{
								v = x.BlockDefinition.MinRadius;
							}
							x.Radius = v;
						},
						Normalizer = ((MyGravityGeneratorSphere x, float v) => (v == 0f) ? 0f : ((v - x.BlockDefinition.MinRadius) / (x.BlockDefinition.MaxRadius - x.BlockDefinition.MinRadius))),
						Denormalizer = ((MyGravityGeneratorSphere x, float v) => (v == 0f) ? 0f : (v * (x.BlockDefinition.MaxRadius - x.BlockDefinition.MinRadius) + x.BlockDefinition.MinRadius)),
						Writer = delegate(MyGravityGeneratorSphere x, StringBuilder result)
						{
							result.AppendInt32((int)(float)x.m_radius).Append(" m");
						}
					};
					obj.EnableActions();
					MyTerminalControlFactory.AddControl(obj);
					MyTerminalControlSlider<MyGravityGeneratorSphere> myTerminalControlSlider = new MyTerminalControlSlider<MyGravityGeneratorSphere>("Gravity", MySpaceTexts.BlockPropertyTitle_GravityAcceleration, MySpaceTexts.BlockPropertyDescription_GravityAcceleration);
					myTerminalControlSlider.SetLimits((MyGravityGeneratorSphere x) => x.BlockDefinition.MinGravityAcceleration, (MyGravityGeneratorSphere x) => x.BlockDefinition.MaxGravityAcceleration);
					myTerminalControlSlider.DefaultValue = 9.81f;
					myTerminalControlSlider.Getter = ((MyGravityGeneratorSphere x) => x.GravityAcceleration);
					myTerminalControlSlider.Setter = delegate(MyGravityGeneratorSphere x, float v)
					{
						if (float.IsNaN(v) || float.IsInfinity(v))
						{
							v = 0f;
						}
						x.GravityAcceleration = v;
					};
					myTerminalControlSlider.Writer = delegate(MyGravityGeneratorSphere x, StringBuilder result)
					{
						result.AppendFormat("{0:F1} m/s² ({1:F2} g)", x.GravityAcceleration, x.GravityAcceleration / 9.81f);
					};
					myTerminalControlSlider.EnableActions();
					MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				}
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_GravityGeneratorSphere myObjectBuilder_GravityGeneratorSphere = (MyObjectBuilder_GravityGeneratorSphere)objectBuilder;
			m_radius.SetLocalValue(MathHelper.Clamp(myObjectBuilder_GravityGeneratorSphere.Radius, BlockDefinition.MinRadius, BlockDefinition.MaxRadius));
			m_gravityAcceleration.SetLocalValue(MathHelper.Clamp(myObjectBuilder_GravityGeneratorSphere.GravityAcceleration, BlockDefinition.MinGravityAcceleration, BlockDefinition.MaxGravityAcceleration));
			m_defaultVolume = (float)(Math.Pow(100.0, BlockDefinition.ConsumptionPower) * Math.PI * 0.75);
			if (base.CubeGrid.CreatePhysics)
			{
				AddDebugRenderComponent(new MyDebugRenderComponentGravityGeneratorSphere(this));
			}
		}

		protected override void InitializeSinkComponent()
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, MaxInput, CalculateRequiredPowerInput);
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
			MyObjectBuilder_GravityGeneratorSphere obj = (MyObjectBuilder_GravityGeneratorSphere)base.GetObjectBuilderCubeBlock(copy);
			obj.Radius = m_radius;
			obj.GravityAcceleration = m_gravityAcceleration;
			return obj;
		}

		public override void UpdateBeforeSimulation()
		{
			if (MyFakes.ENABLE_GRAVITY_GENERATOR_SPHERE)
			{
				base.UpdateBeforeSimulation();
			}
		}

		protected override float CalculateRequiredPowerInput()
		{
			if (base.Enabled && base.IsFunctional)
			{
				return CalculateRequiredPowerInputForRadius(m_radius);
			}
			return 0f;
		}

		private float CalculateRequiredPowerInputForRadius(float radius)
		{
			return (float)(Math.Pow(radius, BlockDefinition.ConsumptionPower) * Math.PI * 0.75) / m_defaultVolume * BlockDefinition.BasePowerInput * (Math.Abs(m_gravityAcceleration) / 9.81f);
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
			return (base.WorldMatrix.Translation - worldPoint).LengthSquared() < (double)((float)m_radius * (float)m_radius);
		}

		public override void GetProxyAABB(out BoundingBoxD aabb)
		{
			BoundingSphereD sphere = new BoundingSphereD(base.PositionComp.GetPosition(), (float)m_radius);
			BoundingBoxD.CreateFromSphere(ref sphere, out aabb);
		}

		public override Vector3 GetWorldGravity(Vector3D worldPoint)
		{
			Vector3D v = base.WorldMatrix.Translation - worldPoint;
			v.Normalize();
			return (Vector3)v * base.GravityAcceleration;
		}

		protected override HkShape GetHkShape()
		{
			return new HkSphereShape(m_radius);
		}
	}
}
