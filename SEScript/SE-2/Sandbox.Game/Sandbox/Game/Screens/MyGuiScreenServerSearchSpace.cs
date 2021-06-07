using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenServerSearchSpace : MyGuiScreenServerSearchBase
	{
		private MySpaceServerFilterOptions SpaceFilters => base.FilterOptions as MySpaceServerFilterOptions;

		public MyGuiScreenServerSearchSpace(MyGuiScreenJoinGame joinScreen)
			: base(joinScreen)
		{
		}

		protected override void DrawTopControls()
		{
			base.DrawTopControls();
			AddNumericRangeOption(MySpaceTexts.MultiplayerJoinProductionMultipliers, MySpaceNumericOptionEnum.ProductionMultipliers);
			AddNumericRangeOption(MySpaceTexts.WorldSettings_InventorySize, MySpaceNumericOptionEnum.InventoryMultipier);
			CurrentPosition.Y += Padding;
		}

		protected override void DrawBottomControls()
		{
			base.DrawBottomControls();
			AddCheckboxRow(new MyStringId?[2]
			{
				MySpaceTexts.WorldSettings_EnableCopyPaste,
				MySpaceTexts.WorldSettings_EnableIngameScripts
			}, new MySpaceBoolOptionEnum[2]
			{
				MySpaceBoolOptionEnum.CopyPaste,
				MySpaceBoolOptionEnum.Scripts
			}, new MyStringId[2]
			{
				MySpaceTexts.ToolTipWorldSettingsEnableCopyPaste,
				MySpaceTexts.ToolTipWorldSettings_EnableIngameScripts
			});
			AddCheckboxRow(new MyStringId?[2]
			{
				MySpaceTexts.WorldSettings_PermanentDeath,
				MySpaceTexts.WorldSettings_EnableWeapons
			}, new MySpaceBoolOptionEnum[2]
			{
				MySpaceBoolOptionEnum.PermanentDeath,
				MySpaceBoolOptionEnum.Weapons
			}, new MyStringId[2]
			{
				MySpaceTexts.ToolTipWorldSettingsPermanentDeath,
				MySpaceTexts.ToolTipWorldSettingsWeapons
			});
			AddCheckboxRow(new MyStringId?[2]
			{
				MySpaceTexts.WorldSettings_Enable3rdPersonCamera,
				MySpaceTexts.WorldSettings_EnableSpectator
			}, new MySpaceBoolOptionEnum[2]
			{
				MySpaceBoolOptionEnum.ThirdPerson,
				MySpaceBoolOptionEnum.Spectator
			}, new MyStringId[2]
			{
				MySpaceTexts.ToolTipWorldSettings_Enable3rdPersonCamera,
				MySpaceTexts.ToolTipWorldSettingsEnableSpectator
			});
			AddCheckboxRow(new MyStringId?[2]
			{
				MySpaceTexts.World_Settings_EnableOxygenPressurization,
				MySpaceTexts.World_Settings_EnableOxygen
			}, new MySpaceBoolOptionEnum[2]
			{
				MySpaceBoolOptionEnum.Airtightness,
				MySpaceBoolOptionEnum.Oxygen
			}, new MyStringId[2]
			{
				MySpaceTexts.ToolTipWorldSettings_EnableOxygenPressurization,
				MySpaceTexts.ToolTipWorldSettings_EnableOxygen
			});
			AddCheckboxRow(new MyStringId?[2]
			{
				MySpaceTexts.ServerDetails_ServerManagement,
				MySpaceTexts.WorldSettings_StationVoxelSupport
			}, new MySpaceBoolOptionEnum[2]
			{
				MySpaceBoolOptionEnum.ExternalServerManagement,
				MySpaceBoolOptionEnum.UnsupportedStations
			}, new MyStringId[2]
			{
				MySpaceTexts.ServerDetails_ServerManagement,
				MySpaceTexts.ToolTipWorldSettings_StationVoxelSupport
			});
			AddCheckboxRow(new MyStringId?[2]
			{
				MySpaceTexts.WorldSettings_DestructibleBlocks,
				MySpaceTexts.WorldSettings_ThrusterDamage
			}, new MySpaceBoolOptionEnum[2]
			{
				MySpaceBoolOptionEnum.BlockDestruction,
				MySpaceBoolOptionEnum.ThrusterDamage
			}, new MyStringId[2]
			{
				MySpaceTexts.ToolTipWorldSettingsDestructibleBlocks,
				MySpaceTexts.ToolTipWorldSettingsThrusterDamage
			});
			AddCheckboxRow(new MyStringId?[2]
			{
				MySpaceTexts.WorldSettings_EnableCargoShips,
				MySpaceTexts.WorldSettings_Encounters
			}, new MySpaceBoolOptionEnum[2]
			{
				MySpaceBoolOptionEnum.CargoShips,
				MySpaceBoolOptionEnum.Encounters
			}, new MyStringId[2]
			{
				MySpaceTexts.ToolTipWorldSettingsEnableCargoShips,
				MySpaceTexts.ToolTipWorldSettings_EnableEncounters
			});
			AddCheckboxRow(new MyStringId?[2]
			{
				MySpaceTexts.WorldSettings_EnableSpiders,
				MySpaceTexts.WorldSettings_EnableRespawnShips
			}, new MySpaceBoolOptionEnum[2]
			{
				MySpaceBoolOptionEnum.Spiders,
				MySpaceBoolOptionEnum.RespawnShips
			}, new MyStringId[2]
			{
				MySpaceTexts.ToolTipWorldSettings_EnableSpiders,
				MySpaceTexts.ToolTipWorldSettings_EnableRespawnShips
			});
			AddCheckboxRow(new MyStringId?[2]
			{
				MySpaceTexts.WorldSettings_EnableDrones,
				MySpaceTexts.WorldSettings_EnableWolfs
			}, new MySpaceBoolOptionEnum[2]
			{
				MySpaceBoolOptionEnum.Drones,
				MySpaceBoolOptionEnum.Wolves
			}, new MyStringId[2]
			{
				MySpaceTexts.ToolTipWorldSettings_EnableDrones,
				MySpaceTexts.ToolTipWorldSettings_EnableWolfs
			});
		}

		protected override void DrawMidControls()
		{
			base.DrawMidControls();
			Vector2 currentPosition = CurrentPosition;
			CurrentPosition.Y = -0.102f;
			CurrentPosition.X += Padding / 2.4f;
			MyGuiControlLabel control = new MyGuiControlLabel(CurrentPosition, null, MyTexts.GetString(MySpaceTexts.WorldSettings_EnvironmentHostility));
			Controls.Add(control);
			MyGuiControlCombobox combo = new MyGuiControlCombobox(CurrentPosition);
			combo.AddItem(-1L, MyCommonTexts.Any);
			combo.AddItem(0L, MySpaceTexts.WorldSettings_EnvironmentHostilitySafe);
			combo.AddItem(1L, MySpaceTexts.WorldSettings_EnvironmentHostilityNormal);
			combo.AddItem(2L, MySpaceTexts.WorldSettings_EnvironmentHostilityCataclysm);
			combo.AddItem(3L, MySpaceTexts.WorldSettings_EnvironmentHostilityCataclysmUnreal);
			combo.Size = new Vector2(0.295f, 1f);
			combo.PositionX += combo.Size.X / 2f + Padding * 12.3f;
			combo.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_EvnironmentHostility));
			MyFilterRange filter = SpaceFilters.GetFilter(MySpaceNumericOptionEnum.EnvionmentHostility);
			if (filter.Active)
			{
				combo.SelectItemByKey((long)filter.Value.Max, sendEvent: false);
			}
			else
			{
				combo.SelectItemByKey(-1L, sendEvent: false);
			}
			combo.ItemSelected += delegate
			{
				MyFilterRange filter2 = SpaceFilters.GetFilter(MySpaceNumericOptionEnum.EnvionmentHostility);
				long selectedKey = combo.GetSelectedKey();
				if (selectedKey == -1)
				{
					filter2.Active = false;
				}
				else
				{
					filter2.Active = true;
					filter2.Value = new SerializableRange
					{
						Min = selectedKey,
						Max = selectedKey
					};
				}
			};
			Controls.Add(combo);
			CurrentPosition.X = currentPosition.X;
			CurrentPosition.Y += 0.04f + Padding;
		}

		private void AddNumericRangeOption(MyStringId text, MySpaceNumericOptionEnum key)
		{
			MyFilterRange filter = SpaceFilters.GetFilter(key);
			if (filter == null)
			{
				throw new ArgumentOutOfRangeException("key", key, "Filter not found in dictionary!");
			}
			AddNumericRangeOption(text, delegate(SerializableRange r)
			{
				filter.Value = r;
			}, filter.Value, filter.Active, delegate(MyGuiControlCheckbox c)
			{
				filter.Active = c.IsChecked;
			}, base.EnableAdvanced);
		}

		private MyGuiControlIndeterminateCheckbox[] AddCheckboxRow(MyStringId?[] text, MySpaceBoolOptionEnum[] keys, MyStringId[] tooltip)
		{
			Action<MyGuiControlIndeterminateCheckbox>[] array = new Action<MyGuiControlIndeterminateCheckbox>[2];
			CheckStateEnum[] array2 = new CheckStateEnum[2];
			if (keys.Length != 0)
			{
				MyFilterBool filter2 = SpaceFilters.GetFilter(keys[0]);
				if (filter2 == null)
				{
					throw new ArgumentOutOfRangeException("keys", keys[0], "Filter not found in dictionary!");
				}
				array[0] = delegate(MyGuiControlIndeterminateCheckbox c)
				{
					filter2.CheckValue = c.State;
				};
				array2[0] = filter2.CheckValue;
			}
			if (keys.Length > 1)
			{
				MyFilterBool filter = SpaceFilters.GetFilter(keys[1]);
				if (filter == null)
				{
					throw new ArgumentOutOfRangeException("keys", keys[1], "Filter not found in dictionary!");
				}
				array[1] = delegate(MyGuiControlIndeterminateCheckbox c)
				{
					filter.CheckValue = c.State;
				};
				array2[1] = filter.CheckValue;
			}
			if (keys.Length > 2)
			{
				throw new ArgumentOutOfRangeException();
			}
			return AddIndeterminateDuo(text, array, new MyStringId?[2]
			{
				tooltip[0],
				tooltip[1]
			}, array2, base.EnableAdvanced);
		}
	}
}
