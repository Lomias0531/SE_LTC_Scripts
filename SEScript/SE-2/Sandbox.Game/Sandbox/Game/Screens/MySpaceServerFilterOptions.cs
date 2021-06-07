using Sandbox.Engine.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;

namespace Sandbox.Game.Screens
{
	public class MySpaceServerFilterOptions : MyServerFilterOptions
	{
		public const byte SPACE_BOOL_OFFSET = 128;

		public MySpaceServerFilterOptions()
		{
		}

		public MySpaceServerFilterOptions(MyObjectBuilder_ServerFilterOptions ob)
			: base(ob)
		{
		}

		public MyFilterRange GetFilter(MySpaceNumericOptionEnum key)
		{
			return (MyFilterRange)base.Filters[(byte)key];
		}

		public MyFilterBool GetFilter(MySpaceBoolOptionEnum key)
		{
			return (MyFilterBool)base.Filters[(byte)key];
		}

		protected override Dictionary<byte, IMyFilterOption> CreateFilters()
		{
			Dictionary<byte, IMyFilterOption> dictionary = new Dictionary<byte, IMyFilterOption>();
			foreach (byte value in Enum.GetValues(typeof(MySpaceNumericOptionEnum)))
			{
				dictionary.Add(value, new MyFilterRange());
			}
			foreach (byte value2 in Enum.GetValues(typeof(MySpaceBoolOptionEnum)))
			{
				dictionary.Add(value2, new MyFilterBool());
			}
			return dictionary;
		}

		public override bool FilterServer(MyCachedServerItem server)
		{
			MyObjectBuilder_SessionSettings settings = server.Settings;
			if (settings == null)
			{
				return false;
			}
			if (!MySandboxGame.Config.ExperimentalMode && settings.IsSettingsExperimental())
			{
				return false;
			}
			if (!GetFilter(MySpaceNumericOptionEnum.InventoryMultipier).IsMatch(settings.InventorySizeMultiplier))
			{
				return false;
			}
			if (!GetFilter(MySpaceNumericOptionEnum.EnvionmentHostility).IsMatch((float)settings.EnvironmentHostility))
			{
				return false;
			}
			MyFilterRange filter = GetFilter(MySpaceNumericOptionEnum.ProductionMultipliers);
			if (!filter.IsMatch(settings.AssemblerEfficiencyMultiplier) || !filter.IsMatch(settings.AssemblerSpeedMultiplier) || !filter.IsMatch(settings.RefinerySpeedMultiplier))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Spectator).IsMatch(settings.EnableSpectator))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.CopyPaste).IsMatch(settings.EnableCopyPaste))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.ThrusterDamage).IsMatch(settings.ThrusterDamage))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.PermanentDeath).IsMatch(settings.PermanentDeath))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Weapons).IsMatch(settings.WeaponsEnabled))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.CargoShips).IsMatch(settings.CargoShipsEnabled))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.BlockDestruction).IsMatch(settings.DestructibleBlocks))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Scripts).IsMatch(settings.EnableIngameScripts))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Oxygen).IsMatch(settings.EnableOxygen))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.ThirdPerson).IsMatch(settings.Enable3rdPersonView))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Encounters).IsMatch(settings.EnableEncounters))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Airtightness).IsMatch(settings.EnableOxygenPressurization))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.UnsupportedStations).IsMatch(settings.StationVoxelSupport))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.VoxelDestruction).IsMatch(settings.EnableVoxelDestruction))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Drones).IsMatch(settings.EnableDrones))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Wolves).IsMatch(settings.EnableWolfs))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Spiders).IsMatch(settings.EnableSpiders))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.RespawnShips).IsMatch(settings.EnableRespawnShips))
			{
				return false;
			}
			if (server.Rules == null || !GetFilter(MySpaceBoolOptionEnum.ExternalServerManagement).IsMatch(server.Rules.ContainsKey("SM")))
			{
				return false;
			}
			return true;
		}

		public override bool FilterLobby(IMyLobby lobby)
		{
			if (!GetFilter(MySpaceNumericOptionEnum.InventoryMultipier).IsMatch(MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", lobby, 1f)))
			{
				return false;
			}
			MyFilterRange filter = GetFilter(MySpaceNumericOptionEnum.ProductionMultipliers);
			if (!filter.IsMatch(MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", lobby, 1f)) || !filter.IsMatch(MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", lobby, 1f)))
			{
				return false;
			}
			return true;
		}
	}
}
