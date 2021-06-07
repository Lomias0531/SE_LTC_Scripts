using Sandbox.Game.Localization;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Utils;

namespace Sandbox.Game
{
	public class MyDLCs
	{
		public sealed class MyDLC
		{
			public static readonly MyDLC DeluxeEdition = new MyDLC(MyPerGameSettings.DeluxeEditionDlcId, "DeluxeEdition", MySpaceTexts.DisplayName_DLC_DeluxeEdition, MySpaceTexts.Description_DLC_DeluxeEdition, MyPerGameSettings.DeluxeEditionUrl, "Textures\\GUI\\DLCs\\Deluxe\\DeluxeIcon.DDS", "Textures\\GUI\\DLCs\\Deluxe\\DeluxeEdition.dds");

			public static readonly MyDLC DecorativeBlocks = new MyDLC(1049790u, "DecorativeBlocks", MySpaceTexts.DisplayName_DLC_DecorativeBlocks, MySpaceTexts.Description_DLC_DecorativeBlocks, "https://store.steampowered.com/app/1049790", "Textures\\GUI\\DLCs\\Decorative\\DecorativeBlocks.DDS", "Textures\\GUI\\DLCs\\Decorative\\DecorativeDLC_Badge.DDS");

			public static readonly MyDLC EconomyExpansion = new MyDLC(1135960u, "Economy", MySpaceTexts.DisplayName_DLC_EconomyExpansion, MySpaceTexts.Description_DLC_EconomyExpansion, "https://store.steampowered.com/app/1135960", "Textures\\GUI\\DLCs\\Economy\\Economy.DDS", "Textures\\GUI\\DLCs\\Economy\\EconomyDLC_Badge.DDS");

			public static readonly MyDLC StylePack = new MyDLC(1084680u, "StylePack", MySpaceTexts.DisplayName_DLC_StylePack, MySpaceTexts.Description_DLC_StylePack, "https://store.steampowered.com/app/1084680", "Textures\\GUI\\DLCs\\Style\\StylePackDLC.DDS", "Textures\\GUI\\DLCs\\Style\\StylePackDLC_Badge.DDS");

			public static readonly MyDLC DecorativeBlocks2 = new MyDLC(1167910u, "DecorativeBlocks2", MySpaceTexts.DisplayName_DLC_DecorativeBlocks2, MySpaceTexts.Description_DLC_DecorativeBlocks2, "https://store.steampowered.com/app/1167910", "Textures\\GUI\\DLCs\\Decorative2\\DecorativeBlocks.DDS", "Textures\\GUI\\DLCs\\Decorative2\\DecorativeDLC_Badge.DDS");

			public uint AppId
			{
				get;
			}

			public string Name
			{
				get;
			}

			public MyStringId DisplayName
			{
				get;
			}

			public MyStringId Description
			{
				get;
			}

			public string URL
			{
				get;
			}

			public string Icon
			{
				get;
			}

			public string Badge
			{
				get;
			}

			private MyDLC(uint appId, string name, MyStringId displayName, MyStringId description, string url, string icon, string badge)
			{
				AppId = appId;
				Name = name;
				DisplayName = displayName;
				Description = description;
				URL = url;
				Icon = icon;
				Badge = badge;
			}
		}

		private static readonly Dictionary<uint, MyDLC> m_dlcs = new Dictionary<uint, MyDLC>
		{
			{
				MyDLC.DeluxeEdition.AppId,
				MyDLC.DeluxeEdition
			},
			{
				MyDLC.DecorativeBlocks.AppId,
				MyDLC.DecorativeBlocks
			},
			{
				MyDLC.EconomyExpansion.AppId,
				MyDLC.EconomyExpansion
			},
			{
				MyDLC.StylePack.AppId,
				MyDLC.StylePack
			},
			{
				MyDLC.DecorativeBlocks2.AppId,
				MyDLC.DecorativeBlocks2
			}
		};

		private static readonly Dictionary<string, MyDLC> m_dlcsByName = new Dictionary<string, MyDLC>
		{
			{
				MyDLC.DeluxeEdition.Name,
				MyDLC.DeluxeEdition
			},
			{
				MyDLC.DecorativeBlocks.Name,
				MyDLC.DecorativeBlocks
			},
			{
				MyDLC.EconomyExpansion.Name,
				MyDLC.EconomyExpansion
			},
			{
				MyDLC.StylePack.Name,
				MyDLC.StylePack
			},
			{
				MyDLC.DecorativeBlocks2.Name,
				MyDLC.DecorativeBlocks2
			}
		};

		public static DictionaryReader<uint, MyDLC> DLCs => m_dlcs;

		public static bool TryGetDLC(uint id, out MyDLC dlc)
		{
			return m_dlcs.TryGetValue(id, out dlc);
		}

		public static bool TryGetDLC(string name, out MyDLC dlc)
		{
			return m_dlcsByName.TryGetValue(name, out dlc);
		}

		public static string GetRequiredDLCTooltip(string name)
		{
			if (TryGetDLC(name, out MyDLC dlc))
			{
				return GetRequiredDLCTooltip(dlc.AppId);
			}
			return null;
		}

		public static string GetRequiredDLCTooltip(uint id)
		{
			if (TryGetDLC(id, out MyDLC dlc))
			{
				return string.Format(MyTexts.GetString(MyCommonTexts.RequiresDlc), MyTexts.GetString(dlc.DisplayName));
			}
			return string.Format(MyTexts.GetString(MyCommonTexts.RequiresDlc), id);
		}

		public static string GetRequiredDLCStoreHint(uint id)
		{
			if (TryGetDLC(id, out MyDLC dlc))
			{
				return string.Format(MyTexts.GetString(MyCommonTexts.ShowDlcStore), MyTexts.GetString(dlc.DisplayName));
			}
			return string.Format(MyTexts.GetString(MyCommonTexts.ShowDlcStore), id);
		}

		public static string GetDLCIcon(uint id)
		{
			if (TryGetDLC(id, out MyDLC dlc))
			{
				return dlc.Icon;
			}
			return null;
		}
	}
}
