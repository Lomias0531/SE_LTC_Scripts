using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using System;
using System.Text;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyHudShipInfo
	{
		private enum LineEnum
		{
			ReflectorLights,
			Mass,
			Speed,
			PowerUsage,
			ReactorsMaxOutput,
			ThrustCount,
			DampenersState,
			GyroCount,
			FuelTime,
			NumberOfBatteries,
			PowerState,
			LandingGearState,
			LandingGearStateSecondLine
		}

		private static StringBuilder m_formattingCache = new StringBuilder();

		private MyMultipleEnabledEnum m_reflectorLights;

		private int m_mass;

		public bool SpeedInKmH;

		private float m_speed;

		private float m_powerUsage;

		private float m_reactors;

		private int m_landingGearsInProximity;

		private int m_landingGearsLocked;

		private int m_landingGearsTotal;

		private int m_thrustCount;

		private int m_gyroCount;

		private int m_numberOfBatteries;

		private float m_fuelRemainingTime;

		private MyResourceStateEnum m_resourceState;

		private bool m_dampenersEnabled;

		private bool m_needsRefresh = true;

		private MyHudNameValueData m_data;

		public MyMultipleEnabledEnum ReflectorLights
		{
			get
			{
				return m_reflectorLights;
			}
			set
			{
				if (m_reflectorLights != value)
				{
					m_reflectorLights = value;
					m_needsRefresh = true;
				}
			}
		}

		public int Mass
		{
			get
			{
				return m_mass;
			}
			set
			{
				if (m_mass != value)
				{
					m_mass = value;
					m_needsRefresh = true;
				}
			}
		}

		public float Speed
		{
			get
			{
				return m_speed;
			}
			set
			{
				if (m_speed != value)
				{
					m_speed = value;
					m_needsRefresh = true;
				}
			}
		}

		public float PowerUsage
		{
			get
			{
				return m_powerUsage;
			}
			set
			{
				if (m_powerUsage != value)
				{
					m_powerUsage = value;
					m_needsRefresh = true;
				}
			}
		}

		public float Reactors
		{
			get
			{
				return m_reactors;
			}
			set
			{
				if (m_reactors != value)
				{
					m_reactors = value;
					m_needsRefresh = true;
				}
			}
		}

		public int LandingGearsInProximity
		{
			get
			{
				return m_landingGearsInProximity;
			}
			set
			{
				if (m_landingGearsInProximity != value)
				{
					m_landingGearsInProximity = value;
					m_needsRefresh = true;
				}
			}
		}

		public int LandingGearsLocked
		{
			get
			{
				return m_landingGearsLocked;
			}
			set
			{
				if (m_landingGearsLocked != value)
				{
					m_landingGearsLocked = value;
					m_needsRefresh = true;
				}
			}
		}

		public int LandingGearsTotal
		{
			get
			{
				return m_landingGearsTotal;
			}
			set
			{
				if (m_landingGearsTotal != value)
				{
					m_landingGearsTotal = value;
					m_needsRefresh = true;
				}
			}
		}

		public int ThrustCount
		{
			get
			{
				return m_thrustCount;
			}
			set
			{
				if (m_thrustCount != value)
				{
					m_thrustCount = value;
					m_needsRefresh = true;
				}
			}
		}

		public int GyroCount
		{
			get
			{
				return m_gyroCount;
			}
			set
			{
				if (m_gyroCount != value)
				{
					m_gyroCount = value;
					m_needsRefresh = true;
				}
			}
		}

		public int NumberOfBatteries
		{
			get
			{
				return m_numberOfBatteries;
			}
			set
			{
				if (m_numberOfBatteries != value)
				{
					m_numberOfBatteries = value;
					m_needsRefresh = true;
				}
			}
		}

		public float FuelRemainingTime
		{
			get
			{
				return m_fuelRemainingTime;
			}
			set
			{
				if (m_fuelRemainingTime != value)
				{
					m_fuelRemainingTime = value;
					m_needsRefresh = true;
				}
			}
		}

		public MyResourceStateEnum ResourceState
		{
			get
			{
				return m_resourceState;
			}
			set
			{
				if (m_resourceState != value)
				{
					m_resourceState = value;
					m_needsRefresh = true;
				}
			}
		}

		public bool DampenersEnabled
		{
			get
			{
				return m_dampenersEnabled;
			}
			set
			{
				if (m_dampenersEnabled != value)
				{
					m_dampenersEnabled = value;
					m_needsRefresh = true;
				}
			}
		}

		public MyHudNameValueData Data
		{
			get
			{
				if (m_needsRefresh)
				{
					Refresh();
				}
				return m_data;
			}
		}

		public bool Visible
		{
			get;
			private set;
		}

		public MyHudShipInfo()
		{
			m_data = new MyHudNameValueData(typeof(LineEnum).GetEnumValues().Length);
			Reload();
		}

		public void Reload()
		{
			MyHudNameValueData data = Data;
			data[1].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameMass));
			data[2].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameSpeed));
			data[3].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNamePowerUsage));
			data[4].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameReactors));
			data[8].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameFuelTime));
			data[9].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameNumberOfBatteries));
			data[7].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameGyroscopes));
			data[5].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameThrusts));
			data[6].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameDampeners));
			data[11].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameLandingGear));
			m_needsRefresh = true;
		}

		public void Show(Action<MyHudShipInfo> propertiesInit)
		{
			Visible = true;
			propertiesInit?.Invoke(this);
		}

		public void Hide()
		{
			Visible = false;
		}

		private void Refresh()
		{
			m_needsRefresh = false;
			MyHudNameValueData data = Data;
			data[0].Name.Clear().AppendStringBuilder((ReflectorLights == MyMultipleEnabledEnum.AllDisabled) ? MyTexts.Get(MySpaceTexts.HudInfoReflectorsOff) : ((ReflectorLights == MyMultipleEnabledEnum.NoObjects) ? MyTexts.Get(MySpaceTexts.HudInfoNoReflectors) : MyTexts.Get(MySpaceTexts.HudInfoReflectorsOn)));
			if (Mass == 0)
			{
				data[1].Value.Clear().Append("-").Append(" kg");
			}
			else
			{
				data[1].Value.Clear().AppendInt32(Mass).Append(" kg");
			}
			if (SpeedInKmH)
			{
				data[2].Value.Clear().AppendDecimal(Speed * 3.6f, 1).Append(" km/h");
			}
			else
			{
				data[2].Value.Clear().AppendDecimal(Speed, 1).Append(" m/s");
			}
			MyHudNameValueData.Data data2 = data[10];
			if (ResourceState == MyResourceStateEnum.NoPower)
			{
				data2.Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNoPower));
				data2.Visible = true;
			}
			else
			{
				data2.Visible = false;
			}
			MyHudNameValueData.Data data3 = data[3];
			if (ResourceState == MyResourceStateEnum.OverloadBlackout || ResourceState == MyResourceStateEnum.OverloadAdaptible)
			{
				data3.NameFont = (data3.ValueFont = "Red");
			}
			else
			{
				data3.NameFont = (data3.ValueFont = null);
			}
			data3.Value.Clear();
			if (ResourceState == MyResourceStateEnum.OverloadBlackout)
			{
				data3.Value.AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoPowerOverload));
			}
			else
			{
				data3.Value.AppendDecimal(PowerUsage * 100f, 2).Append(" %");
			}
			StringBuilder value = data[4].Value;
			value.Clear();
			MyValueFormatter.AppendWorkInBestUnit(Reactors, value);
			MyHudNameValueData.Data data4 = data[8];
			data4.Value.Clear();
			if (ResourceState != MyResourceStateEnum.NoPower)
			{
				MyValueFormatter.AppendTimeInBestUnit(FuelRemainingTime * 3600f, data4.Value);
				data4.Visible = true;
			}
			else
			{
				data4.Visible = false;
			}
			data[9].Value.Clear().AppendInt32(NumberOfBatteries);
			MyHudNameValueData.Data data5 = data[7];
			data5.Value.Clear().AppendInt32(GyroCount);
			if (GyroCount == 0)
			{
				data5.NameFont = (data5.ValueFont = "Red");
			}
			else
			{
				data5.NameFont = (data5.ValueFont = null);
			}
			MyHudNameValueData.Data data6 = data[5];
			data6.Value.Clear().AppendInt32(ThrustCount);
			if (ThrustCount == 0)
			{
				data6.NameFont = (data6.ValueFont = "Red");
			}
			else
			{
				data6.NameFont = (data6.ValueFont = null);
			}
			data[6].Value.Clear().AppendStringBuilder(MyTexts.Get(DampenersEnabled ? MySpaceTexts.HudInfoOn : MySpaceTexts.HudInfoOff));
			MyHudNameValueData.Data data7 = data[11];
			MyHudNameValueData.Data data8 = data[12];
			if (LandingGearsLocked > 0)
			{
				data[12].Name.Clear().Append("  ").AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameLocked));
				data7.Value.Clear().Append(LandingGearsTotal);
				data8.Value.Clear().AppendInt32(LandingGearsLocked);
			}
			else
			{
				data[12].Name.Clear().Append("  ").AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudInfoNameInProximity));
				data7.Value.Clear().Append(LandingGearsTotal);
				data8.Value.Clear().AppendInt32(LandingGearsInProximity);
			}
		}
	}
}
