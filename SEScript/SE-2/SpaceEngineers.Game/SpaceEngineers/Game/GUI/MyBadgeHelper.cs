using Sandbox.Engine.Networking;
using Sandbox.Game;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
	public class MyBadgeHelper
	{
		private enum MyBannerStatus
		{
			Offline,
			Installed,
			NotInstalled
		}

		private class MyBadge
		{
			public MyBannerStatus Status;

			public uint DLCId;

			public string AchievementName;

			public string Texture;
		}

		private static MyStringHash Deluxe = MyStringHash.GetOrCompute("Deluxe");

		private static MyStringHash DecoBlockDlc = MyStringHash.GetOrCompute("Decoratives");

		private static MyStringHash DecoBlockDlc2 = MyStringHash.GetOrCompute("Decoratives2");

		private static MyStringHash StylePackDlc = MyStringHash.GetOrCompute("StylePack");

		private static MyStringHash EconomyExpansionDlc = MyStringHash.GetOrCompute("EconomyExpansion");

		private static MyStringHash PromotedEngineer = MyStringHash.GetOrCompute("PromotedEngineer");

		/// <summary>
		/// Collection of the round badges under the main logo.
		/// </summary>
		private Dictionary<MyStringHash, MyBadge> m_badges = new Dictionary<MyStringHash, MyBadge>
		{
			{
				Deluxe,
				new MyBadge
				{
					Status = MyBannerStatus.Offline,
					Texture = MyDLCs.MyDLC.DeluxeEdition.Badge,
					DLCId = MyDLCs.MyDLC.DeluxeEdition.AppId,
					AchievementName = ""
				}
			},
			{
				PromotedEngineer,
				new MyBadge
				{
					Status = MyBannerStatus.Offline,
					Texture = "Textures\\GUI\\PromotedEngineer.dds",
					DLCId = 0u,
					AchievementName = "Promoted_engineer"
				}
			},
			{
				DecoBlockDlc,
				new MyBadge
				{
					Status = MyBannerStatus.Offline,
					Texture = MyDLCs.MyDLC.DecorativeBlocks.Badge,
					DLCId = MyDLCs.MyDLC.DecorativeBlocks.AppId,
					AchievementName = ""
				}
			},
			{
				StylePackDlc,
				new MyBadge
				{
					Status = MyBannerStatus.Offline,
					Texture = MyDLCs.MyDLC.StylePack.Badge,
					DLCId = MyDLCs.MyDLC.StylePack.AppId,
					AchievementName = ""
				}
			},
			{
				EconomyExpansionDlc,
				new MyBadge
				{
					Status = MyBannerStatus.Offline,
					Texture = MyDLCs.MyDLC.EconomyExpansion.Badge,
					DLCId = MyDLCs.MyDLC.EconomyExpansion.AppId,
					AchievementName = ""
				}
			},
			{
				DecoBlockDlc2,
				new MyBadge
				{
					Status = MyBannerStatus.Offline,
					Texture = MyDLCs.MyDLC.DecorativeBlocks2.Badge,
					DLCId = MyDLCs.MyDLC.DecorativeBlocks2.AppId,
					AchievementName = ""
				}
			}
		};

		public void DrawGameLogo(float transitionAlpha, Vector2 position)
		{
			MyGuiSandbox.DrawGameLogo(transitionAlpha, position);
			position.X += 0.005f;
			position.Y += 0.19f;
			Vector2 vector = position;
			Vector2 size = new Vector2(114f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			int num = 0;
			foreach (KeyValuePair<MyStringHash, MyBadge> badge in m_badges)
			{
				if (badge.Value.Status == MyBannerStatus.Installed)
				{
					MyGuiSandbox.DrawBadge(badge.Value.Texture, transitionAlpha, position, size);
					position.X += size.X;
					num++;
					if (num >= 6)
					{
						vector.Y += size.Y;
						position = vector;
						num = 0;
					}
				}
			}
		}

		public void RefreshGameLogo()
		{
			foreach (KeyValuePair<MyStringHash, MyBadge> badge in m_badges)
			{
				if (MyGameService.IsActive)
				{
					if (badge.Value.DLCId != 0 && MyGameService.IsDlcInstalled(badge.Value.DLCId))
					{
						badge.Value.Status = MyBannerStatus.Installed;
					}
					else if (!string.IsNullOrEmpty(badge.Value.AchievementName) && MyGameService.GetAchievement(badge.Value.AchievementName, null, 0f).IsUnlocked)
					{
						badge.Value.Status = MyBannerStatus.Installed;
					}
					else
					{
						badge.Value.Status = MyBannerStatus.NotInstalled;
					}
				}
				else
				{
					badge.Value.Status = MyBannerStatus.Offline;
				}
			}
		}
	}
}
