using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	internal class MyHudWarnings : MySessionComponentBase
	{
		public static readonly int FRAMES_BETWEEN_UPDATE = 30;

		public static bool EnableWarnings = true;

		private static List<MyHudWarningGroup> m_hudWarnings = new List<MyHudWarningGroup>();

		private static List<MyGuiSounds> m_soundQueue = new List<MyGuiSounds>();

		private static IMySourceVoice m_sound;

		private static int m_lastSoundPlayed = 0;

		private int m_updateCounter;

		public static void EnqueueSound(MyGuiSounds sound)
		{
			if (MyGuiAudio.HudWarnings)
			{
				if ((m_sound == null || !m_sound.IsPlaying) && MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastSoundPlayed > 5000)
				{
					m_sound = MyGuiAudio.PlaySound(sound);
					m_lastSoundPlayed = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				}
				else
				{
					m_soundQueue.Add(sound);
				}
			}
		}

		public static void RemoveSound(MyGuiSounds cueEnum)
		{
			if (m_sound != null && m_sound.CueEnum == MyGuiAudio.GetCue(cueEnum) && !m_sound.IsPlaying)
			{
				m_sound.Stop();
				m_sound = null;
			}
			m_soundQueue.RemoveAll((MyGuiSounds cue) => cue == cueEnum);
		}

		public static void Add(MyHudWarningGroup hudWarningGroup)
		{
			m_hudWarnings.Add(hudWarningGroup);
		}

		public static void Remove(MyHudWarningGroup hudWarningGroup)
		{
			m_hudWarnings.Remove(hudWarningGroup);
		}

		public override void LoadData()
		{
			base.LoadData();
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				List<MyHudWarning> list = new List<MyHudWarning>();
				MyHudWarning item = new MyHudWarning(HealthLowWarningMethod, 1, 300000, 0, 2500);
				list.Add(item);
				item = new MyHudWarning(HealthCritWarningMethod, 0, 300000, 0, 5000);
				list.Add(item);
				Add(new MyHudWarningGroup(list, canBeTurnedOff: false));
				list.Clear();
				item = new MyHudWarning(FuelLowWarningMethod, 1, 300000, 0, 2500);
				list.Add(item);
				item = new MyHudWarning(FuelCritWarningMethod, 0, 300000, 0, 5000);
				list.Add(item);
				Add(new MyHudWarningGroup(list, canBeTurnedOff: false));
				list.Clear();
				item = new MyHudWarning(EnergyLowWarningMethod, 2, 300000, 0, 2500);
				list.Add(item);
				item = new MyHudWarning(EnergyCritWarningMethod, 1, 300000, 0, 5000);
				list.Add(item);
				item = new MyHudWarning(EnergyNoWarningMethod, 0, 300000, 0, 5000);
				list.Add(item);
				Add(new MyHudWarningGroup(list, canBeTurnedOff: false));
				list.Clear();
				item = new MyHudWarning(MeteorInboundWarningMethod, 0, 600000, 0, 5000);
				list.Add(item);
				Add(new MyHudWarningGroup(list, canBeTurnedOff: false));
			}
		}

		private static bool HealthWarningMethod(float treshold)
		{
			if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.StatComp != null)
			{
				if (MySession.Static.LocalCharacter.StatComp.HealthRatio < treshold)
				{
					return !MySession.Static.LocalCharacter.IsDead;
				}
				return false;
			}
			return false;
		}

		private static bool IsEnergyUnderTreshold(float treshold)
		{
			if (MySession.Static.CreativeMode || MySession.Static.ControlledEntity == null)
			{
				return false;
			}
			if (MySession.Static.ControlledEntity.Entity is MyCharacter || MySession.Static.ControlledEntity == null)
			{
				MyCharacter localCharacter = MySession.Static.LocalCharacter;
				if (localCharacter == null)
				{
					return false;
				}
				if (localCharacter.SuitBattery.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) > 0f)
				{
					return false;
				}
				if (localCharacter.SuitBattery.ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId) / 1E-05f <= treshold)
				{
					return !localCharacter.IsDead;
				}
				return false;
			}
			if (MySession.Static.ControlledEntity.Entity is MyCockpit)
			{
				MyCubeGrid cubeGrid = (MySession.Static.ControlledEntity.Entity as MyCockpit).CubeGrid;
				if (cubeGrid.GridSystems == null || cubeGrid.GridSystems.ResourceDistributor == null)
				{
					return false;
				}
				MyMultipleEnabledEnum myMultipleEnabledEnum = cubeGrid.GridSystems.ResourceDistributor.SourcesEnabledByType(MyResourceDistributorComponent.ElectricityId);
				if (MyHud.ShipInfo.FuelRemainingTime <= treshold && myMultipleEnabledEnum != MyMultipleEnabledEnum.AllDisabled)
				{
					return myMultipleEnabledEnum != MyMultipleEnabledEnum.NoObjects;
				}
				return false;
			}
			return false;
		}

		private static bool IsFuelUnderThreshold(float treshold)
		{
			if (MySession.Static.CreativeMode || MySession.Static.ControlledEntity == null)
			{
				return false;
			}
			if (MySession.Static.ControlledEntity.Entity is MyCharacter)
			{
				MyCharacter localCharacter = MySession.Static.LocalCharacter;
				if (localCharacter == null)
				{
					return false;
				}
				if (localCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) < treshold)
				{
					return true;
				}
			}
			return false;
		}

		private static bool HealthLowWarningMethod(out MyGuiSounds cue, out MyStringId text)
		{
			cue = MyGuiSounds.None;
			text = MySpaceTexts.Blank;
			if (!HealthWarningMethod(MyCharacterStatComponent.HEALTH_RATIO_LOW))
			{
				return false;
			}
			cue = MyGuiSounds.HudVocHealthLow;
			text = MySpaceTexts.NotificationHealthLow;
			return true;
		}

		private static bool HealthCritWarningMethod(out MyGuiSounds cue, out MyStringId text)
		{
			cue = MyGuiSounds.None;
			text = MySpaceTexts.Blank;
			if (!HealthWarningMethod(MyCharacterStatComponent.HEALTH_RATIO_CRITICAL))
			{
				return false;
			}
			cue = MyGuiSounds.HudVocHealthCritical;
			text = MySpaceTexts.NotificationHealthCritical;
			return true;
		}

		private static bool MeteorInboundWarningMethod(out MyGuiSounds cue, out MyStringId text)
		{
			cue = MyGuiSounds.HudVocMeteorInbound;
			text = MySpaceTexts.NotificationMeteorInbound;
			if (MyMeteorShower.CurrentTarget.HasValue && MySession.Static.ControlledEntity != null)
			{
				return (double)Vector3.Distance(MyMeteorShower.CurrentTarget.Value.Center, MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition()) < 2.0 * MyMeteorShower.CurrentTarget.Value.Radius + 500.0;
			}
			return false;
		}

		private static bool EnergyLowWarningMethod(out MyGuiSounds cue, out MyStringId text)
		{
			cue = MyGuiSounds.None;
			text = MySpaceTexts.Blank;
			if (MySession.Static.ControlledEntity == null)
			{
				return false;
			}
			if (MySession.Static.ControlledEntity.Entity is MyCharacter)
			{
				if (!IsEnergyUnderTreshold(0.25f))
				{
					return false;
				}
				cue = MyGuiSounds.HudVocEnergyLow;
				if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.OxygenComponent != null && MySession.Static.LocalCharacter.OxygenComponent.NeedsOxygenFromSuit && MySession.Static.Settings.EnableOxygen)
				{
					text = MySpaceTexts.NotificationSuitEnergyLowNoDamage;
				}
				else
				{
					text = MySpaceTexts.NotificationSuitEnergyLow;
				}
			}
			else
			{
				if (!(MySession.Static.ControlledEntity.Entity is MyCockpit))
				{
					return false;
				}
				if (!IsEnergyUnderTreshold(0.125f))
				{
					return false;
				}
				MyCockpit myCockpit = (MyCockpit)MySession.Static.ControlledEntity.Entity;
				bool flag = false;
				List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.Logical.GetGroupNodes(myCockpit.CubeGrid);
				if (groupNodes == null || groupNodes.Count == 0)
				{
					return false;
				}
				foreach (MyCubeGrid item in groupNodes)
				{
					if (item.NumberOfReactors > 0)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				if (myCockpit.CubeGrid.IsStatic)
				{
					cue = MyGuiSounds.HudVocStationFuelLow;
				}
				else
				{
					cue = MyGuiSounds.HudVocShipFuelLow;
				}
				if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.OxygenComponent != null && MySession.Static.LocalCharacter.OxygenComponent.NeedsOxygenFromSuit && MySession.Static.Settings.EnableOxygen)
				{
					text = MySpaceTexts.NotificationShipEnergyLowNoDamage;
				}
				else
				{
					text = MySpaceTexts.NotificationShipEnergyLow;
				}
			}
			return true;
		}

		private static bool EnergyCritWarningMethod(out MyGuiSounds cue, out MyStringId text)
		{
			cue = MyGuiSounds.None;
			text = MySpaceTexts.Blank;
			if (MySession.Static.ControlledEntity == null)
			{
				return false;
			}
			if (MySession.Static.ControlledEntity.Entity is MyCharacter)
			{
				if (!IsEnergyUnderTreshold(0.1f))
				{
					return false;
				}
				cue = MyGuiSounds.HudVocEnergyCrit;
				if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.OxygenComponent != null && MySession.Static.LocalCharacter.OxygenComponent.NeedsOxygenFromSuit && MySession.Static.Settings.EnableOxygen)
				{
					text = MySpaceTexts.NotificationSuitEnergyCriticalNoDamage;
				}
				else
				{
					text = MySpaceTexts.NotificationSuitEnergyCritical;
				}
			}
			else
			{
				if (!(MySession.Static.ControlledEntity.Entity is MyCockpit))
				{
					return false;
				}
				if (!IsEnergyUnderTreshold(0.05f))
				{
					return false;
				}
				MyCockpit myCockpit = (MyCockpit)MySession.Static.ControlledEntity.Entity;
				bool flag = false;
				List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.Logical.GetGroupNodes(myCockpit.CubeGrid);
				if (groupNodes == null || groupNodes.Count == 0)
				{
					return false;
				}
				foreach (MyCubeGrid item in groupNodes)
				{
					if (item.NumberOfReactors > 0)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				if (myCockpit.CubeGrid.IsStatic)
				{
					cue = MyGuiSounds.HudVocStationFuelCrit;
				}
				else
				{
					cue = MyGuiSounds.HudVocShipFuelCrit;
				}
				if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.OxygenComponent != null && MySession.Static.LocalCharacter.OxygenComponent.NeedsOxygenFromSuit && MySession.Static.Settings.EnableOxygen)
				{
					text = MySpaceTexts.NotificationShipEnergyCriticalNoDamage;
				}
				else
				{
					text = MySpaceTexts.NotificationShipEnergyCritical;
				}
			}
			return true;
		}

		private static bool FuelLowWarningMethod(out MyGuiSounds cue, out MyStringId text)
		{
			cue = MyGuiSounds.None;
			text = MySpaceTexts.Blank;
			if (MySession.Static.ControlledEntity == null)
			{
				return false;
			}
			MyCharacter myCharacter = MySession.Static.ControlledEntity.Entity as MyCharacter;
			if (myCharacter == null || myCharacter.JetpackComp == null)
			{
				return false;
			}
			if (!IsFuelUnderThreshold(0.1f))
			{
				return false;
			}
			cue = MyGuiSounds.HudVocFuelLow;
			text = MySpaceTexts.NotificationSuitFuelLow;
			return true;
		}

		private static bool FuelCritWarningMethod(out MyGuiSounds cue, out MyStringId text)
		{
			cue = MyGuiSounds.None;
			text = MySpaceTexts.Blank;
			if (MySession.Static.ControlledEntity == null)
			{
				return false;
			}
			MyCharacter myCharacter = MySession.Static.ControlledEntity.Entity as MyCharacter;
			if (myCharacter == null || myCharacter.JetpackComp == null)
			{
				return false;
			}
			if (!IsFuelUnderThreshold(0.05f))
			{
				return false;
			}
			cue = MyGuiSounds.HudVocFuelCrit;
			text = MySpaceTexts.NotificationSuitFuelCritical;
			return true;
		}

		private static bool EnergyNoWarningMethod(out MyGuiSounds cue, out MyStringId text)
		{
			cue = MyGuiSounds.None;
			text = MySpaceTexts.Blank;
			if (!IsEnergyUnderTreshold(0f))
			{
				return false;
			}
			if (MySession.Static.ControlledEntity.Entity is MyCharacter)
			{
				cue = MyGuiSounds.HudVocEnergyNo;
				text = MySpaceTexts.NotificationEnergyNo;
			}
			else
			{
				if (!(MySession.Static.ControlledEntity.Entity is MyCockpit))
				{
					return false;
				}
				MyCockpit myCockpit = (MyCockpit)MySession.Static.ControlledEntity.Entity;
				bool flag = false;
				List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.Logical.GetGroupNodes(myCockpit.CubeGrid);
				if (groupNodes == null || groupNodes.Count == 0)
				{
					return false;
				}
				foreach (MyCubeGrid item in groupNodes)
				{
					if (item.NumberOfReactors > 0)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				if (myCockpit.CubeGrid.IsStatic)
				{
					cue = MyGuiSounds.HudVocStationFuelNo;
				}
				else
				{
					cue = MyGuiSounds.HudVocShipFuelNo;
				}
				text = MySpaceTexts.NotificationFuelNo;
			}
			return true;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			foreach (MyHudWarningGroup hudWarning in m_hudWarnings)
			{
				hudWarning.Clear();
			}
			m_hudWarnings.Clear();
			m_soundQueue.Clear();
			if (m_sound != null)
			{
				m_sound.Stop(force: true);
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_hudWarnings.Clear();
				m_soundQueue.Clear();
				return;
			}
			m_updateCounter++;
			if (m_updateCounter % FRAMES_BETWEEN_UPDATE == 0)
			{
				foreach (MyHudWarningGroup hudWarning in m_hudWarnings)
				{
					hudWarning.Update();
				}
				if (m_soundQueue.Count > 0 && MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastSoundPlayed > 5000)
				{
					m_lastSoundPlayed = MySandboxGame.TotalGamePlayTimeInMilliseconds;
					m_sound = MyGuiAudio.PlaySound(m_soundQueue[0]);
					m_soundQueue.RemoveAt(0);
				}
			}
		}
	}
}
