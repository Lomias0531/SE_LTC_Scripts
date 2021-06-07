using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
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

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Gyro))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyGyro),
		typeof(Sandbox.ModAPI.Ingame.IMyGyro)
	})]
	public class MyGyro : MyFunctionalBlock, Sandbox.ModAPI.IMyGyro, Sandbox.ModAPI.Ingame.IMyGyro, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		protected class m_gyroPower_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType gyroPower;
				ISyncType result = gyroPower = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyGyro)P_0).m_gyroPower = (Sync<float, SyncDirection.BothWays>)gyroPower;
				return result;
			}
		}

		protected class m_gyroOverride_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType gyroOverride;
				ISyncType result = gyroOverride = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyGyro)P_0).m_gyroOverride = (Sync<bool, SyncDirection.BothWays>)gyroOverride;
				return result;
			}
		}

		protected class m_gyroOverrideVelocity_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType gyroOverrideVelocity;
				ISyncType result = gyroOverrideVelocity = new Sync<Vector3, SyncDirection.BothWays>(P_1, P_2);
				((MyGyro)P_0).m_gyroOverrideVelocity = (Sync<Vector3, SyncDirection.BothWays>)gyroOverrideVelocity;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyGyro_003C_003EActor : IActivator, IActivator<MyGyro>
		{
			private sealed override object CreateInstance()
			{
				return new MyGyro();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGyro CreateInstance()
			{
				return new MyGyro();
			}

			MyGyro IActivator<MyGyro>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyGyroDefinition m_gyroDefinition;

		private int m_oldEmissiveState = -1;

		private readonly Sync<float, SyncDirection.BothWays> m_gyroPower;

		private readonly Sync<bool, SyncDirection.BothWays> m_gyroOverride;

		private Sync<Vector3, SyncDirection.BothWays> m_gyroOverrideVelocity;

		private float m_gyroMultiplier = 1f;

		private float m_powerConsumptionMultiplier = 1f;

		public bool IsPowered => base.CubeGrid.GridSystems.GyroSystem.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

		public float MaxGyroForce => m_gyroDefinition.ForceMagnitude * (float)m_gyroPower * m_gyroMultiplier;

		public float RequiredPowerInput => m_gyroDefinition.RequiredPowerInput * (float)m_gyroPower * m_powerConsumptionMultiplier;

		public float GyroPower
		{
			get
			{
				return m_gyroPower;
			}
			set
			{
				value = MathHelper.Clamp(value, 0f, 1f);
				if (value != (float)m_gyroPower)
				{
					m_gyroPower.Value = value;
				}
			}
		}

		public bool GyroOverride
		{
			get
			{
				return m_gyroOverride;
			}
			set
			{
				m_gyroOverride.Value = value;
			}
		}

		public Vector3 GyroOverrideVelocityGrid => Vector3.TransformNormal(m_gyroOverrideVelocity, base.Orientation);

		float Sandbox.ModAPI.Ingame.IMyGyro.Yaw
		{
			get
			{
				return 0f - m_gyroOverrideVelocity.Value.Y;
			}
			set
			{
				if (GyroOverride)
				{
					float num = MaxAngularRadiansPerSecond(this);
					value = MathHelper.Clamp(value, 0f - num, num);
					SetGyroTorqueYaw(this, 0f - value);
				}
			}
		}

		float Sandbox.ModAPI.Ingame.IMyGyro.Pitch
		{
			get
			{
				return 0f - m_gyroOverrideVelocity.Value.X;
			}
			set
			{
				if (GyroOverride)
				{
					float num = MaxAngularRadiansPerSecond(this);
					value = MathHelper.Clamp(value, 0f - num, num);
					SetGyroTorquePitch(this, 0f - value);
				}
			}
		}

		float Sandbox.ModAPI.Ingame.IMyGyro.Roll
		{
			get
			{
				return 0f - m_gyroOverrideVelocity.Value.Z;
			}
			set
			{
				if (GyroOverride)
				{
					float num = MaxAngularRadiansPerSecond(this);
					value = MathHelper.Clamp(value, 0f - num, num);
					SetGyroTorqueRoll(this, 0f - value);
				}
			}
		}

		float Sandbox.ModAPI.IMyGyro.GyroStrengthMultiplier
		{
			get
			{
				return m_gyroMultiplier;
			}
			set
			{
				m_gyroMultiplier = value;
				if (m_gyroMultiplier < 0.01f)
				{
					m_gyroMultiplier = 0.01f;
				}
				if (base.CubeGrid.GridSystems.GyroSystem != null)
				{
					base.CubeGrid.GridSystems.GyroSystem.MarkDirty();
				}
			}
		}

		float Sandbox.ModAPI.IMyGyro.PowerConsumptionMultiplier
		{
			get
			{
				return m_powerConsumptionMultiplier;
			}
			set
			{
				m_powerConsumptionMultiplier = value;
				if (m_powerConsumptionMultiplier < 0.01f)
				{
					m_powerConsumptionMultiplier = 0.01f;
				}
				if (base.CubeGrid.GridSystems.GyroSystem != null)
				{
					base.CubeGrid.GridSystems.GyroSystem.MarkDirty();
				}
				SetDetailedInfoDirty();
				RaisePropertiesChanged();
			}
		}

		protected override bool CheckIsWorking()
		{
			if (IsPowered)
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		private static float MaxAngularRadiansPerSecond(MyGyro gyro)
		{
			if (gyro.m_gyroDefinition.CubeSize == MyCubeSize.Small)
			{
				return MyGridPhysics.GetSmallShipMaxAngularVelocity();
			}
			return MyGridPhysics.GetLargeShipMaxAngularVelocity();
		}

		private static float MaxAngularRPM(MyGyro gyro)
		{
			if (gyro.m_gyroDefinition.CubeSize == MyCubeSize.Small)
			{
				return MyGridPhysics.GetSmallShipMaxAngularVelocity() * (30f / MathF.PI);
			}
			return MyGridPhysics.GetLargeShipMaxAngularVelocity() * (30f / MathF.PI);
		}

		public MyGyro()
		{
			CreateTerminalControls();
			m_gyroPower.ValueChanged += delegate
			{
				GyroPowerChanged();
			};
			m_gyroOverride.ValueChanged += delegate
			{
				GyroOverrideChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyGyro>())
			{
				base.CreateTerminalControls();
				MyTerminalControlSlider<MyGyro> obj = new MyTerminalControlSlider<MyGyro>("Power", MySpaceTexts.BlockPropertyTitle_GyroPower, MySpaceTexts.BlockPropertyDescription_GyroPower)
				{
					Getter = ((MyGyro x) => x.GyroPower),
					Setter = delegate(MyGyro x, float v)
					{
						x.GyroPower = v;
					},
					Writer = delegate(MyGyro x, StringBuilder result)
					{
						result.AppendInt32((int)(x.GyroPower * 100f)).Append(" %");
					},
					DefaultValue = 1f
				};
				obj.EnableActions(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE);
				MyTerminalControlFactory.AddControl(obj);
				if (MyFakes.ENABLE_GYRO_OVERRIDE)
				{
					MyTerminalControlCheckbox<MyGyro> obj2 = new MyTerminalControlCheckbox<MyGyro>("Override", MySpaceTexts.BlockPropertyTitle_GyroOverride, MySpaceTexts.BlockPropertyDescription_GyroOverride)
					{
						Getter = ((MyGyro x) => x.GyroOverride),
						Setter = delegate(MyGyro x, bool v)
						{
							x.GyroOverride = v;
						}
					};
					obj2.EnableAction();
					MyTerminalControlFactory.AddControl(obj2);
					MyTerminalControlSlider<MyGyro> myTerminalControlSlider = new MyTerminalControlSlider<MyGyro>("Yaw", MySpaceTexts.BlockPropertyTitle_GyroYawOverride, MySpaceTexts.BlockPropertyDescription_GyroYawOverride);
					myTerminalControlSlider.Getter = ((MyGyro x) => (0f - x.m_gyroOverrideVelocity.Value.Y) * (30f / MathF.PI));
					myTerminalControlSlider.Setter = delegate(MyGyro x, float v)
					{
						SetGyroTorqueYaw(x, (0f - v) * (MathF.PI / 30f));
					};
					myTerminalControlSlider.Writer = delegate(MyGyro x, StringBuilder result)
					{
						result.AppendDecimal((0f - x.m_gyroOverrideVelocity.Value.Y) * (30f / MathF.PI), 2).Append(" RPM");
					};
					myTerminalControlSlider.Enabled = ((MyGyro x) => x.GyroOverride);
					myTerminalControlSlider.DefaultValue = 0f;
					myTerminalControlSlider.SetDualLogLimits((MyGyro x) => 0.01f, MaxAngularRPM, 0.05f);
					myTerminalControlSlider.EnableActions(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE);
					MyTerminalControlFactory.AddControl(myTerminalControlSlider);
					MyTerminalControlSlider<MyGyro> myTerminalControlSlider2 = new MyTerminalControlSlider<MyGyro>("Pitch", MySpaceTexts.BlockPropertyTitle_GyroPitchOverride, MySpaceTexts.BlockPropertyDescription_GyroPitchOverride);
					myTerminalControlSlider2.Getter = ((MyGyro x) => x.m_gyroOverrideVelocity.Value.X * (30f / MathF.PI));
					myTerminalControlSlider2.Setter = delegate(MyGyro x, float v)
					{
						SetGyroTorquePitch(x, v * (MathF.PI / 30f));
					};
					myTerminalControlSlider2.Writer = delegate(MyGyro x, StringBuilder result)
					{
						result.AppendDecimal(x.m_gyroOverrideVelocity.Value.X * (30f / MathF.PI), 2).Append(" RPM");
					};
					myTerminalControlSlider2.Enabled = ((MyGyro x) => x.GyroOverride);
					myTerminalControlSlider2.DefaultValue = 0f;
					myTerminalControlSlider2.SetDualLogLimits((MyGyro x) => 0.01f, MaxAngularRPM, 0.05f);
					myTerminalControlSlider2.EnableActions(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE);
					MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
					MyTerminalControlSlider<MyGyro> myTerminalControlSlider3 = new MyTerminalControlSlider<MyGyro>("Roll", MySpaceTexts.BlockPropertyTitle_GyroRollOverride, MySpaceTexts.BlockPropertyDescription_GyroRollOverride);
					myTerminalControlSlider3.Getter = ((MyGyro x) => (0f - x.m_gyroOverrideVelocity.Value.Z) * (30f / MathF.PI));
					myTerminalControlSlider3.Setter = delegate(MyGyro x, float v)
					{
						SetGyroTorqueRoll(x, (0f - v) * (MathF.PI / 30f));
					};
					myTerminalControlSlider3.Writer = delegate(MyGyro x, StringBuilder result)
					{
						result.AppendDecimal((0f - x.m_gyroOverrideVelocity.Value.Z) * (30f / MathF.PI), 2).Append(" RPM");
					};
					myTerminalControlSlider3.Enabled = ((MyGyro x) => x.GyroOverride);
					myTerminalControlSlider3.DefaultValue = 0f;
					myTerminalControlSlider3.SetDualLogLimits((MyGyro x) => 0.01f, MaxAngularRPM, 0.05f);
					myTerminalControlSlider3.EnableActions(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE);
					MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				}
			}
		}

		private void GyroOverrideChanged()
		{
			SetGyroOverride(m_gyroOverride.Value);
		}

		private void GyroPowerChanged()
		{
			SetEmissiveStateWorking();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			m_gyroDefinition = (MyGyroDefinition)base.BlockDefinition;
			MyObjectBuilder_Gyro myObjectBuilder_Gyro = objectBuilder as MyObjectBuilder_Gyro;
			m_gyroPower.SetLocalValue(MathHelper.Clamp(myObjectBuilder_Gyro.GyroPower, 0f, 1f));
			if (MyFakes.ENABLE_GYRO_OVERRIDE)
			{
				GyroOverride = myObjectBuilder_Gyro.GyroOverride;
				float num = MaxAngularRadiansPerSecond(this);
				SerializableVector3 v = new SerializableVector3(MathHelper.Clamp(myObjectBuilder_Gyro.TargetAngularVelocity.x, 0f - num, num), MathHelper.Clamp(myObjectBuilder_Gyro.TargetAngularVelocity.y, 0f - num, num), MathHelper.Clamp(myObjectBuilder_Gyro.TargetAngularVelocity.z, 0f - num, num));
				m_gyroOverrideVelocity.Value = v;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Gyro obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_Gyro;
			obj.GyroPower = m_gyroPower;
			obj.GyroOverride = GyroOverride;
			obj.TargetAngularVelocity = m_gyroOverrideVelocity.Value;
			return obj;
		}

		public override void OnModelChange()
		{
			m_oldEmissiveState = -1;
			base.OnModelChange();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (base.IsWorking)
			{
				OnStartWorking();
			}
			m_oldEmissiveState = -1;
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(base.BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(RequiredPowerInput, detailedInfo);
		}

		public override bool SetEmissiveStateWorking()
		{
			if (GyroOverride)
			{
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, base.Render.RenderObjectIDs[0]);
			}
			if (GyroPower <= 1E-05f)
			{
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0]);
			}
			return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
		}

		public void SetGyroOverride(bool value)
		{
			base.CubeGrid.GridSystems.GyroSystem.MarkDirty();
			if (CheckIsWorking())
			{
				SetEmissiveStateWorking();
			}
		}

		private static void SetGyroTorqueYaw(MyGyro gyro, float yawValue)
		{
			Vector3 value = gyro.m_gyroOverrideVelocity.Value;
			value.Y = yawValue;
			gyro.SetGyroTorque(value);
		}

		private static void SetGyroTorquePitch(MyGyro gyro, float pitchValue)
		{
			Vector3 value = gyro.m_gyroOverrideVelocity.Value;
			value.X = pitchValue;
			gyro.SetGyroTorque(value);
		}

		private static void SetGyroTorqueRoll(MyGyro gyro, float rollValue)
		{
			Vector3 value = gyro.m_gyroOverrideVelocity.Value;
			value.Z = rollValue;
			gyro.SetGyroTorque(value);
		}

		public void SetGyroTorque(Vector3 torque)
		{
			m_gyroOverrideVelocity.Value = torque;
			base.CubeGrid.GridSystems.GyroSystem.MarkDirty();
		}
	}
}
