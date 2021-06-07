using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenSafeZoneFilter : MyGuiScreenDebugBase
	{
		public MyGuiControlListbox m_entityListbox;

		public MyGuiControlListbox m_restrictedListbox;

		private MyGuiControlCombobox m_accessCombobox;

		private MyGuiControlCombobox m_restrictionTypeCombobox;

		private MyGuiControlButton m_playersFilter;

		private MyGuiControlButton m_gridsFilter;

		private MyGuiControlButton m_floatingObjectsFilter;

		private MyGuiControlButton m_factionsFilter;

		private MyGuiControlButton m_moveLeftButton;

		private MyGuiControlButton m_moveLeftAllButton;

		private MyGuiControlButton m_moveRightButton;

		private MyGuiControlButton m_moveRightAllButton;

		private MyGuiControlButton m_closeButton;

		private MyGuiControlButton m_addContainedToSafeButton;

		private MyGuiControlLabel m_modeLabel;

		private MyGuiControlLabel m_controlLabelList;

		private MyGuiControlLabel m_controlLabelEntity;

		public MySafeZone m_selectedSafeZone;

		private MyGuiScreenAdminMenu.MyRestrictedTypeEnum m_selectedFilter;

		public MyGuiScreenSafeZoneFilter(Vector2 position, MySafeZone safeZone)
			: base(position, new Vector2(0.7f, 0.644084f), MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, isTopMostScreen: true)
		{
			base.CloseButtonEnabled = true;
			m_selectedSafeZone = safeZone;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ConfigureFilter), Color.White.ToVector4(), new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.88f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.88f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.88f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.88f);
			Controls.Add(myGuiControlSeparatorList);
			m_playersFilter = MakeButtonCategory(new Vector2(-0.292999983f, -0.205f), "Character", MyTexts.GetString(MyCommonTexts.JoinGame_ColumnTitle_Players), OnFilterChange, MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player);
			m_factionsFilter = MakeButtonCategory(new Vector2(-0.257f, -0.205f), "Animation", MyTexts.GetString(MyCommonTexts.ScreenPlayers_Factions), OnFilterChange, MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction);
			m_gridsFilter = MakeButtonCategory(new Vector2(-0.221f, -0.205f), "Block", MyTexts.GetString(MySpaceTexts.Grids), OnFilterChange, MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid);
			m_floatingObjectsFilter = MakeButtonCategory(new Vector2(-0.185f, -0.205f), "Modpack", MyTexts.GetString(MySpaceTexts.FloatingObjects), OnFilterChange, MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects);
			Vector2 value = new Vector2(0f, -0.223f);
			Controls.Add(m_moveLeftButton = MakeButtonTiny(value + 5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 3.141593f, MyTexts.GetString(MySpaceTexts.Remove), MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, OnRemoveRestricted));
			Controls.Add(m_moveLeftAllButton = MakeButtonTiny(value + 6f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 3.141593f, MyTexts.GetString(MySpaceTexts.RemoveAll), MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, OnRemoveAllRestricted));
			Controls.Add(m_moveRightAllButton = MakeButtonTiny(value + 2f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 0f, MyTexts.GetString(MySpaceTexts.AddAll), MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, OnAddAllRestricted));
			Controls.Add(m_moveRightButton = MakeButtonTiny(value + 3f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 0f, MyTexts.GetString(MySpaceTexts.Add), MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, OnAddRestricted));
			m_moveLeftButton.Enabled = false;
			m_moveRightButton.Enabled = false;
			m_modeLabel = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.022f, -0.214f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.SafeZone_Mode)
			};
			Controls.Add(m_modeLabel);
			m_accessCombobox = AddCombo(m_selectedSafeZone.AccessTypePlayers, OnAccessChanged, enabled: true, 4);
			m_accessCombobox.Position = new Vector2(0.308f, -0.224f);
			m_accessCombobox.Size = new Vector2(0.287f - m_modeLabel.Size.X - 0.01f, 0.1f);
			m_accessCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_accessCombobox.ItemSelected += m_accessCombobox_ItemSelected;
			m_controlLabelList = new MyGuiControlLabel
			{
				Position = new Vector2(0.03f, -0.173f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.SafeZone_SafeZoneFilter)
			};
			MyGuiControlPanel control = new MyGuiControlPanel(new Vector2(m_controlLabelList.PositionX - 0.0085f, m_controlLabelList.Position.Y - 0.005f), new Vector2(0.2865f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			Controls.Add(control);
			Controls.Add(m_controlLabelList);
			m_controlLabelEntity = new MyGuiControlLabel
			{
				Position = new Vector2(-0.3f, -0.173f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.SafeZone_ListOfEntities)
			};
			MyGuiControlPanel control2 = new MyGuiControlPanel(new Vector2(m_controlLabelEntity.PositionX - 0.0085f, m_controlLabelEntity.Position.Y - 0.005f), new Vector2(0.2865f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			Controls.Add(control2);
			Controls.Add(m_controlLabelEntity);
			m_restrictedListbox = new MyGuiControlListbox(Vector2.Zero, MyGuiControlListboxStyleEnum.Blueprints);
			m_restrictedListbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_restrictedListbox.Enabled = true;
			m_restrictedListbox.VisibleRowsCount = 9;
			m_restrictedListbox.Position = m_restrictedListbox.Size / 2f + m_currentPosition;
			m_restrictedListbox.MultiSelect = true;
			Controls.Add(m_restrictedListbox);
			m_restrictedListbox.Position = new Vector2(0.022f, -0.145f);
			m_restrictedListbox.ItemsSelected += OnSelectRestrictedItem;
			m_restrictedListbox.ItemDoubleClicked += OnDoubleClickRestrictedItem;
			m_entityListbox = new MyGuiControlListbox(Vector2.Zero, MyGuiControlListboxStyleEnum.Blueprints);
			m_entityListbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_entityListbox.Enabled = true;
			m_entityListbox.VisibleRowsCount = 9;
			m_entityListbox.Position = m_restrictedListbox.Size / 2f + m_currentPosition;
			m_entityListbox.MultiSelect = true;
			Controls.Add(m_entityListbox);
			m_entityListbox.Position = new Vector2(-0.308f, -0.145f);
			m_entityListbox.ItemsSelected += OnSelectEntityItem;
			m_entityListbox.ItemDoubleClicked += OnDoubleClickEntityItem;
			m_closeButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Close), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancel);
			m_addContainedToSafeButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_FilterContained), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnAddContainedToSafe);
			Vector2 value2 = new Vector2(0.002f, m_size.Value.Y / 2f - 0.071f);
			Vector2 value3 = new Vector2(0.018f, 0f);
			m_closeButton.Position = value2 - value3;
			m_addContainedToSafeButton.Position = value2 + value3;
			m_addContainedToSafeButton.SetToolTip(MySpaceTexts.ToolTipSafeZone_AddContained);
			m_closeButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Close));
			Controls.Add(m_closeButton);
			Controls.Add(m_addContainedToSafeButton);
			m_playersFilter.Selected = true;
			m_playersFilter.HighlightType = MyGuiControlHighlightType.FORCED;
			m_controlLabelList.Text = MyTexts.GetString(MySpaceTexts.SafeZone_SafeZoneFilter) + " " + m_playersFilter.Tooltips.ToolTips[0].Text;
			m_controlLabelEntity.Text = MyTexts.GetString(MySpaceTexts.SafeZone_ListOfEntities) + " " + m_playersFilter.Tooltips.ToolTips[0].Text;
			UpdateRestrictedData();
			OnRestrictionChanged(m_selectedFilter);
		}

		private void m_accessCombobox_ItemSelected()
		{
		}

		private void OnFilterChange(MyGuiControlButton button)
		{
			m_selectedFilter = (MyGuiScreenAdminMenu.MyRestrictedTypeEnum)button.UserData;
			if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
			{
				m_accessCombobox.SelectItemByIndex((int)m_selectedSafeZone.AccessTypePlayers);
			}
			else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
			{
				m_accessCombobox.SelectItemByIndex((int)m_selectedSafeZone.AccessTypeFactions);
			}
			else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
			{
				m_accessCombobox.SelectItemByIndex((int)m_selectedSafeZone.AccessTypeGrids);
			}
			else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
			{
				m_accessCombobox.SelectItemByIndex((int)m_selectedSafeZone.AccessTypeFloatingObjects);
			}
			m_playersFilter.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			m_factionsFilter.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			m_gridsFilter.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			m_floatingObjectsFilter.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			button.Selected = true;
			button.HighlightType = MyGuiControlHighlightType.FORCED;
			m_controlLabelList.Text = MyTexts.GetString(MySpaceTexts.SafeZone_SafeZoneFilter) + " " + button.Tooltips.ToolTips[0].Text;
			m_controlLabelEntity.Text = MyTexts.GetString(MySpaceTexts.SafeZone_ListOfEntities) + " " + button.Tooltips.ToolTips[0].Text;
			OnRestrictionChanged(m_selectedFilter);
		}

		private void OnAddContainedToSafe(MyGuiControlButton button)
		{
			if (m_selectedSafeZone != null)
			{
				m_selectedSafeZone.AddContainedToList();
				UpdateRestrictedData();
				RequestUpdateSafeZone();
				OnRestrictionChanged(m_selectedFilter);
			}
		}

		private void OnCancel(MyGuiControlButton button)
		{
			CloseScreen();
		}

		private void OnSelectRestrictedItem(MyGuiControlListbox list)
		{
			m_moveLeftButton.Enabled = (list.SelectedItems.Count > 0);
		}

		private void OnDoubleClickRestrictedItem(MyGuiControlListbox list)
		{
			OnRemoveRestricted(null);
		}

		private void OnSelectEntityItem(MyGuiControlListbox list)
		{
			m_moveRightButton.Enabled = (list.SelectedItems.Count > 0);
		}

		private void OnDoubleClickEntityItem(MyGuiControlListbox list)
		{
			OnAddRestricted(null);
		}

		private void OnRestrictionChanged(MyGuiScreenAdminMenu.MyRestrictedTypeEnum restrictionType)
		{
			UpdateRestrictedData();
			switch (restrictionType)
			{
			case MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid:
				ShowFilteredEntities(MyEntityList.MyEntityTypeEnum.Grids);
				break;
			case MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects:
				ShowFilteredEntities(MyEntityList.MyEntityTypeEnum.FloatingObjects);
				break;
			case MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player:
				m_entityListbox.Items.Clear();
				foreach (MyPlayer.PlayerId allPlayer in MySession.Static.Players.GetAllPlayers())
				{
					MyPlayer player = null;
					if (Sync.Players.TryGetPlayerById(allPlayer, out player) && !m_selectedSafeZone.Players.Contains(player.Identity.IdentityId))
					{
						m_entityListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(player.DisplayName), null, null, player.Identity.IdentityId));
					}
				}
				break;
			case MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction:
				m_entityListbox.Items.Clear();
				foreach (KeyValuePair<long, IMyFaction> faction in MySession.Static.Factions.Factions)
				{
					if (!m_selectedSafeZone.Factions.Contains(faction.Value))
					{
						m_entityListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(faction.Value.Name), null, null, faction.Value));
					}
				}
				break;
			}
		}

		private void OnAddRestricted(MyGuiControlButton button)
		{
			if (m_selectedSafeZone != null)
			{
				if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
				{
					foreach (MyGuiControlListbox.Item selectedItem in m_entityListbox.SelectedItems)
					{
						long item = (long)selectedItem.UserData;
						m_selectedSafeZone.Players.Add(item);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
				{
					foreach (MyGuiControlListbox.Item selectedItem2 in m_entityListbox.SelectedItems)
					{
						long item2 = (long)selectedItem2.UserData;
						m_selectedSafeZone.Entities.Add(item2);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
				{
					foreach (MyGuiControlListbox.Item selectedItem3 in m_entityListbox.SelectedItems)
					{
						MyFaction item3 = (MyFaction)selectedItem3.UserData;
						m_selectedSafeZone.Factions.Add(item3);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
				{
					foreach (MyGuiControlListbox.Item selectedItem4 in m_entityListbox.SelectedItems)
					{
						long item4 = (long)selectedItem4.UserData;
						m_selectedSafeZone.Entities.Add(item4);
					}
					UpdateRestrictedData();
				}
				RequestUpdateSafeZone();
				OnRestrictionChanged(m_selectedFilter);
			}
		}

		private void OnAddAllRestricted(MyGuiControlButton button)
		{
			if (m_selectedSafeZone != null)
			{
				if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
				{
					foreach (MyGuiControlListbox.Item item5 in m_entityListbox.Items)
					{
						long item = (long)item5.UserData;
						m_selectedSafeZone.Players.Add(item);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
				{
					foreach (MyGuiControlListbox.Item item6 in m_entityListbox.Items)
					{
						long item2 = (long)item6.UserData;
						m_selectedSafeZone.Entities.Add(item2);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
				{
					foreach (MyGuiControlListbox.Item item7 in m_entityListbox.Items)
					{
						MyFaction item3 = (MyFaction)item7.UserData;
						m_selectedSafeZone.Factions.Add(item3);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
				{
					foreach (MyGuiControlListbox.Item item8 in m_entityListbox.Items)
					{
						long item4 = (long)item8.UserData;
						m_selectedSafeZone.Entities.Add(item4);
					}
					UpdateRestrictedData();
				}
				RequestUpdateSafeZone();
				OnRestrictionChanged(m_selectedFilter);
			}
		}

		private void OnRemoveRestricted(MyGuiControlButton button)
		{
			if (m_selectedSafeZone != null)
			{
				if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
				{
					foreach (MyGuiControlListbox.Item selectedItem in m_restrictedListbox.SelectedItems)
					{
						long item = (long)selectedItem.UserData;
						m_selectedSafeZone.Players.Remove(item);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
				{
					foreach (MyGuiControlListbox.Item selectedItem2 in m_restrictedListbox.SelectedItems)
					{
						long item2 = (long)selectedItem2.UserData;
						m_selectedSafeZone.Entities.Remove(item2);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
				{
					foreach (MyGuiControlListbox.Item selectedItem3 in m_restrictedListbox.SelectedItems)
					{
						MyFaction item3 = (MyFaction)selectedItem3.UserData;
						m_selectedSafeZone.Factions.Remove(item3);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
				{
					foreach (MyGuiControlListbox.Item selectedItem4 in m_restrictedListbox.SelectedItems)
					{
						long item4 = (long)selectedItem4.UserData;
						m_selectedSafeZone.Entities.Remove(item4);
					}
					UpdateRestrictedData();
				}
				RequestUpdateSafeZone();
				OnRestrictionChanged(m_selectedFilter);
			}
		}

		private void OnRemoveAllRestricted(MyGuiControlButton button)
		{
			if (m_selectedSafeZone != null)
			{
				if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
				{
					foreach (MyGuiControlListbox.Item item5 in m_restrictedListbox.Items)
					{
						long item = (long)item5.UserData;
						m_selectedSafeZone.Players.Remove(item);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
				{
					foreach (MyGuiControlListbox.Item item6 in m_restrictedListbox.Items)
					{
						long item2 = (long)item6.UserData;
						m_selectedSafeZone.Entities.Remove(item2);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
				{
					foreach (MyGuiControlListbox.Item item7 in m_restrictedListbox.Items)
					{
						MyFaction item3 = (MyFaction)item7.UserData;
						m_selectedSafeZone.Factions.Remove(item3);
					}
					UpdateRestrictedData();
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
				{
					foreach (MyGuiControlListbox.Item item8 in m_restrictedListbox.Items)
					{
						long item4 = (long)item8.UserData;
						m_selectedSafeZone.Entities.Remove(item4);
					}
					UpdateRestrictedData();
				}
				RequestUpdateSafeZone();
				OnRestrictionChanged(m_selectedFilter);
			}
		}

		private void UpdateRestrictedData()
		{
			m_restrictedListbox.ClearItems();
			if (m_selectedSafeZone != null)
			{
				if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
				{
					foreach (long player in m_selectedSafeZone.Players)
					{
						if (Sync.Players.TryGetPlayerId(player, out MyPlayer.PlayerId result))
						{
							MyIdentity myIdentity = Sync.Players.TryGetPlayerIdentity(result);
							if (myIdentity != null)
							{
								m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(myIdentity.DisplayName), null, null, player));
							}
							else
							{
								m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(player.ToString()), null, null, player));
							}
						}
						else
						{
							m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(player.ToString()), null, null, player));
						}
					}
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
				{
					foreach (long entity3 in m_selectedSafeZone.Entities)
					{
						if (MyEntities.TryGetEntityById(entity3, out MyEntity entity))
						{
							MyCubeGrid myCubeGrid = entity as MyCubeGrid;
							if (myCubeGrid != null && !myCubeGrid.Closed && myCubeGrid.Physics != null)
							{
								string value = entity.DisplayName ?? entity.Name ?? entity.ToString();
								m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(value), null, null, entity3));
							}
						}
						else
						{
							m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(entity3.ToString()), null, null, entity3));
						}
					}
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
				{
					foreach (long entity4 in m_selectedSafeZone.Entities)
					{
						if (MyEntities.TryGetEntityById(entity4, out MyEntity entity2))
						{
							MyFloatingObject myFloatingObject = entity2 as MyFloatingObject;
							if (myFloatingObject != null)
							{
								if (myFloatingObject.Closed || myFloatingObject.Physics == null)
								{
									continue;
								}
								string value2 = entity2.DisplayName ?? entity2.Name ?? entity2.ToString();
								m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(value2), null, null, entity4));
							}
							MyInventoryBagEntity myInventoryBagEntity = entity2 as MyInventoryBagEntity;
							if (myInventoryBagEntity != null && !myInventoryBagEntity.Closed && myInventoryBagEntity.Physics != null)
							{
								string value3 = entity2.DisplayName ?? entity2.Name ?? entity2.ToString();
								m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(value3), null, null, entity4));
							}
						}
						else
						{
							m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(entity4.ToString()), null, null, entity4));
						}
					}
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
				{
					foreach (MyFaction faction in m_selectedSafeZone.Factions)
					{
						m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(faction.Name), null, null, faction));
					}
				}
			}
		}

		private void RequestUpdateSafeZone()
		{
			if (m_selectedSafeZone != null)
			{
				MySessionComponentSafeZones.RequestUpdateSafeZone((MyObjectBuilder_SafeZone)m_selectedSafeZone.GetObjectBuilder());
			}
		}

		private void ShowFilteredEntities(MyEntityList.MyEntityTypeEnum restrictionType)
		{
			MyGuiScreenAdminMenu.RequestEntityList(restrictionType);
		}

		private void OnAccessChanged(MySafeZoneAccess access)
		{
			if (m_selectedSafeZone != null)
			{
				if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
				{
					m_selectedSafeZone.AccessTypePlayers = access;
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
				{
					m_selectedSafeZone.AccessTypeFactions = access;
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
				{
					m_selectedSafeZone.AccessTypeGrids = access;
				}
				else if (m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
				{
					m_selectedSafeZone.AccessTypeFloatingObjects = access;
				}
				RequestUpdateSafeZone();
			}
		}

		private MyGuiControlButton MakeButtonTiny(Vector2 position, float rotation, string toolTip, MyGuiHighlightTexture icon, Action<MyGuiControlButton> onClick, Vector2? size = null)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Square, size, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			icon.SizePx = new Vector2(64f, 64f);
			myGuiControlButton.Icon = icon;
			myGuiControlButton.IconRotation = rotation;
			myGuiControlButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			return myGuiControlButton;
		}

		private MyGuiControlButton MakeButtonCategoryTiny(Vector2 position, float rotation, string toolTip, MyGuiHighlightTexture icon, Action<MyGuiControlButton> onClick, Vector2? size = null)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Square48, size, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			icon.SizePx = new Vector2(48f, 48f);
			myGuiControlButton.Icon = icon;
			myGuiControlButton.IconRotation = rotation;
			myGuiControlButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			return myGuiControlButton;
		}

		private MyGuiControlButton MakeButtonCategory(Vector2 position, string textureName, string tooltip, Action<MyGuiControlButton> myAction, MyGuiScreenAdminMenu.MyRestrictedTypeEnum myEnum)
		{
			MyGuiControlButton myGuiControlButton = MakeButtonCategoryTiny(position, 0f, tooltip, new MyGuiHighlightTexture
			{
				Normal = $"Textures\\GUI\\Icons\\buttons\\small_variant\\{textureName}.dds",
				Highlight = $"Textures\\GUI\\Icons\\buttons\\small_variant\\{textureName}Highlight.dds",
				SizePx = new Vector2(48f, 48f)
			}, myAction);
			myGuiControlButton.UserData = myEnum;
			Controls.Add(myGuiControlButton);
			myGuiControlButton.Size = new Vector2(0.005f, 0.005f);
			return myGuiControlButton;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiRenameDialog";
		}
	}
}
