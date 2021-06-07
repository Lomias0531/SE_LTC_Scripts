using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlToolbar : MyGuiControlBase
	{
		protected static StringBuilder m_textCache = new StringBuilder();

		protected MyGuiControlGrid m_toolbarItemsGrid;

		protected MyGuiControlLabel m_selectedItemLabel;

		protected MyGuiControlPanel m_colorVariantPanel;

		protected MyGuiControlPanel m_skinVariantPanel;

		protected MyGuiControlContextMenu m_contextMenu;

		protected List<MyGuiControlLabel> m_pageLabelList = new List<MyGuiControlLabel>();

		protected MyToolbar m_shownToolbar;

		protected MyObjectBuilder_ToolbarControlVisualStyle m_style;

		protected MyObjectBuilder_GuiTexture m_itemVarianTtexture;

		protected List<MyStatControls> m_statControls = new List<MyStatControls>();

		protected MyGuiCompositeTexture m_pageCompositeTexture;

		protected MyGuiCompositeTexture m_pageHighlightCompositeTexture;

		private bool m_gridSizeLargeBlock = true;

		protected int m_contextMenuItemIndex = -1;

		public bool UseContextMenu = true;

		public MyToolbar ShownToolbar => m_shownToolbar;

		public MyGuiControlGrid ToolbarGrid => m_toolbarItemsGrid;

		public bool DrawNumbers => MyToolbarComponent.CurrentToolbar.DrawNumbers;

		public Func<int, ColoredIcon> GetSymbol => MyToolbarComponent.CurrentToolbar.GetSymbol;

		public MyGuiControlToolbar(MyObjectBuilder_ToolbarControlVisualStyle toolbarStyle)
		{
			MyToolbarComponent.CurrentToolbarChanged += ToolbarComponent_CurrentToolbarChanged;
			m_style = toolbarStyle;
			RecreateControls(contructor: true);
			ShowToolbar(MyToolbarComponent.CurrentToolbar);
			base.CanFocusChildren = true;
		}

		protected override void OnVisibleChanged()
		{
			base.OnVisibleChanged();
			MyToolbarComponent.IsToolbarControlShown = base.Visible;
		}

		public override void OnRemoving()
		{
			MyToolbarComponent.CurrentToolbarChanged -= ToolbarComponent_CurrentToolbarChanged;
			if (m_shownToolbar != null)
			{
				m_shownToolbar.ItemChanged -= Toolbar_ItemChanged;
				m_shownToolbar.ItemUpdated -= Toolbar_ItemUpdated;
				m_shownToolbar.SelectedSlotChanged -= Toolbar_SelectedSlotChanged;
				m_shownToolbar.SlotActivated -= Toolbar_SlotActivated;
				m_shownToolbar.ItemEnabledChanged -= Toolbar_ItemEnabledChanged;
				m_shownToolbar.CurrentPageChanged -= Toolbar_CurrentPageChanged;
				m_shownToolbar = null;
			}
			MyToolbarComponent.IsToolbarControlShown = false;
			base.OnRemoving();
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null)
			{
				myGuiControlBase = HandleInputElements();
			}
			if (UseContextMenu && MyInput.Static.IsMouseReleased(MyMouseButtonsEnum.Right) && m_contextMenu.Enabled)
			{
				m_contextMenu.Enabled = false;
				m_contextMenu.Activate();
			}
			return myGuiControlBase;
		}

		public override void Update()
		{
			base.Update();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (m_style.VisibleCondition != null && !m_style.VisibleCondition.Eval())
			{
				return;
			}
			Color color = new Vector3(MyPlayer.SelectedColor.X, MathHelper.Clamp(MyPlayer.SelectedColor.Y + 0.8f, 0f, 1f), MathHelper.Clamp(MyPlayer.SelectedColor.Z + 0.55f, 0f, 1f)).HSVtoColor();
			m_colorVariantPanel.ColorMask = color.ToVector4();
			if (!string.IsNullOrEmpty(MyPlayer.SelectedArmorSkin))
			{
				MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), MyPlayer.SelectedArmorSkin);
				MyAssetModifierDefinition assetModifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(id);
				if (assetModifierDefinition != null && !assetModifierDefinition.Icons.IsNullOrEmpty())
				{
					m_skinVariantPanel.BackgroundTexture = new MyGuiCompositeTexture(assetModifierDefinition.Icons[0]);
				}
			}
			else
			{
				m_skinVariantPanel.BackgroundTexture = null;
			}
			m_statControls.ForEach(delegate(MyStatControls x)
			{
				x.Draw(transitionAlpha, backgroundTransitionAlpha);
			});
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		protected override void OnPositionChanged()
		{
			m_statControls.ForEach(delegate(MyStatControls x)
			{
				x.Position = base.Position;
			});
		}

		protected override void OnSizeChanged()
		{
			RefreshInternals();
			base.OnSizeChanged();
		}

		private void RefreshInternals()
		{
			RepositionControls();
		}

		private void RepositionControls()
		{
			Vector2 vector = base.Size * 0.5f;
			m_toolbarItemsGrid.Position = vector;
			m_selectedItemLabel.Position = m_style.SelectedItemPosition;
			if (m_style.SelectedItemTextScale.HasValue)
			{
				m_selectedItemLabel.TextScale = m_style.SelectedItemTextScale.Value;
			}
			m_colorVariantPanel.Position = m_style.ColorPanelStyle.Offset;
			m_skinVariantPanel.Position = m_colorVariantPanel.Position + new Vector2(0.72f * m_colorVariantPanel.Size.X, 0f);
			vector = m_style.PageStyle.PagesOffset;
			foreach (MyGuiControlLabel pageLabel in m_pageLabelList)
			{
				pageLabel.Position = vector + new Vector2(pageLabel.Size.X * 0.5f, (0f - pageLabel.Size.Y) * 0.5f);
				vector.X += pageLabel.Size.X + 0.001f;
			}
			if (UseContextMenu)
			{
				Elements.Remove(m_contextMenu);
				Elements.Add(m_contextMenu);
			}
		}

		private void RecreateControls(bool contructor)
		{
			if (m_style.VisibleCondition != null)
			{
				InitStatConditions(m_style.VisibleCondition);
			}
			m_toolbarItemsGrid = new MyGuiControlGrid
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM,
				VisualStyle = MyGuiControlGridStyleEnum.Toolbar,
				ColumnsCount = MyToolbarComponent.CurrentToolbar.SlotCount + 1,
				RowsCount = 1
			};
			m_toolbarItemsGrid.ItemDoubleClicked += grid_ItemDoubleClicked;
			m_toolbarItemsGrid.ItemClickedWithoutDoubleClick += grid_ItemClicked;
			m_toolbarItemsGrid.ItemAccepted += grid_ItemDoubleClicked;
			m_selectedItemLabel = new MyGuiControlLabel();
			m_colorVariantPanel = new MyGuiControlPanel(null, m_style.ColorPanelStyle.Size);
			m_colorVariantPanel.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
			m_skinVariantPanel = new MyGuiControlPanel(null, new Vector2(m_style.ColorPanelStyle.Size.X * 0.66f) * new Vector2(0.75f, 1f));
			m_skinVariantPanel.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
			m_contextMenu = new MyGuiControlContextMenu();
			m_contextMenu.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			m_contextMenu.Deactivate();
			m_contextMenu.ItemClicked += contextMenu_ItemClicked;
			Elements.Add(m_colorVariantPanel);
			Elements.Add(m_skinVariantPanel);
			Elements.Add(m_selectedItemLabel);
			Elements.Add(m_toolbarItemsGrid);
			Elements.Add(m_contextMenu);
			m_colorVariantPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_skinVariantPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_selectedItemLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
			m_toolbarItemsGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_contextMenu.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			SetupToolbarStyle();
			RefreshInternals();
		}

		public bool IsToolbarGrid(MyGuiControlGrid grid)
		{
			return m_toolbarItemsGrid == grid;
		}

		public void ShowToolbar(MyToolbar toolbar)
		{
			if (m_shownToolbar != null)
			{
				m_shownToolbar.ItemChanged -= Toolbar_ItemChanged;
				m_shownToolbar.ItemUpdated -= Toolbar_ItemUpdated;
				m_shownToolbar.SelectedSlotChanged -= Toolbar_SelectedSlotChanged;
				m_shownToolbar.SlotActivated -= Toolbar_SlotActivated;
				m_shownToolbar.ItemEnabledChanged -= Toolbar_ItemEnabledChanged;
				m_shownToolbar.CurrentPageChanged -= Toolbar_CurrentPageChanged;
				foreach (MyGuiControlLabel pageLabel in m_pageLabelList)
				{
					Elements.Remove(pageLabel);
				}
				m_pageLabelList.Clear();
			}
			m_shownToolbar = toolbar;
			if (m_shownToolbar == null)
			{
				m_toolbarItemsGrid.Enabled = false;
				m_toolbarItemsGrid.Visible = false;
				return;
			}
			int slotCount = toolbar.SlotCount;
			m_toolbarItemsGrid.ColumnsCount = slotCount + (toolbar.ShowHolsterSlot ? 1 : 0);
			for (int i = 0; i < slotCount; i++)
			{
				SetGridItemAt(i, toolbar.GetSlotItem(i), clear: true);
			}
			m_selectedItemLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
			m_colorVariantPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_colorVariantPanel.Visible = MyFakes.ENABLE_BLOCK_COLORING;
			m_skinVariantPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_skinVariantPanel.Visible = MyFakes.ENABLE_BLOCK_COLORING;
			if (toolbar.ShowHolsterSlot)
			{
				SetGridItemAt(slotCount, new MyToolbarItemEmpty(), new string[1]
				{
					"Textures\\GUI\\Icons\\HideWeapon.dds"
				}, null, MyTexts.GetString(MyCommonTexts.HideWeapon));
			}
			if (toolbar.PageCount > 1)
			{
				for (int j = 0; j < toolbar.PageCount; j++)
				{
					m_textCache.Clear();
					m_textCache.AppendInt32(j + 1);
					MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyToolbarComponent.GetSlotControlText(j).ToString() ?? m_textCache.ToString());
					myGuiControlLabel.BackgroundTexture = m_pageCompositeTexture;
					myGuiControlLabel.TextScale = (m_style.PageStyle.NumberSize ?? 0.7f);
					myGuiControlLabel.Size = m_toolbarItemsGrid.ItemSize * new Vector2(0.5f, 0.35f);
					myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
					m_pageLabelList.Add(myGuiControlLabel);
					Elements.Add(myGuiControlLabel);
				}
			}
			RepositionControls();
			HighlightCurrentPageLabel();
			RefreshSelectedItem(toolbar);
			m_shownToolbar.ItemChanged -= Toolbar_ItemChanged;
			m_shownToolbar.ItemChanged += Toolbar_ItemChanged;
			m_shownToolbar.ItemUpdated -= Toolbar_ItemUpdated;
			m_shownToolbar.ItemUpdated += Toolbar_ItemUpdated;
			m_shownToolbar.SelectedSlotChanged -= Toolbar_SelectedSlotChanged;
			m_shownToolbar.SelectedSlotChanged += Toolbar_SelectedSlotChanged;
			m_shownToolbar.SlotActivated -= Toolbar_SlotActivated;
			m_shownToolbar.SlotActivated += Toolbar_SlotActivated;
			m_shownToolbar.ItemEnabledChanged -= Toolbar_ItemEnabledChanged;
			m_shownToolbar.ItemEnabledChanged += Toolbar_ItemEnabledChanged;
			m_shownToolbar.CurrentPageChanged -= Toolbar_CurrentPageChanged;
			m_shownToolbar.CurrentPageChanged += Toolbar_CurrentPageChanged;
			Vector2 vector3 = base.MaxSize = (base.MinSize = new Vector2(m_toolbarItemsGrid.Size.X, m_toolbarItemsGrid.Size.Y + m_selectedItemLabel.Size.Y + m_colorVariantPanel.Size.Y));
			m_toolbarItemsGrid.Enabled = true;
			m_toolbarItemsGrid.Visible = true;
		}

		private void SetupToolbarStyle()
		{
			MyGuiBorderThickness itemMargin = m_style.ItemStyle.Margin.HasValue ? new MyGuiBorderThickness(m_style.ItemStyle.Margin.Value.Left, m_style.ItemStyle.Margin.Value.Right, m_style.ItemStyle.Margin.Value.Top, m_style.ItemStyle.Margin.Value.Botton) : new MyGuiBorderThickness(2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			MyObjectBuilder_GuiTexture texture = MyGuiTextures.Static.GetTexture(m_style.ItemStyle.Texture);
			MyObjectBuilder_GuiTexture texture2 = MyGuiTextures.Static.GetTexture(m_style.ItemStyle.TextureHighlight);
			Vector2 sizePx = new Vector2((float)texture.SizePx.X * m_style.ItemStyle.ItemTextureScale.X, (float)texture.SizePx.Y * m_style.ItemStyle.ItemTextureScale.Y);
			MyGuiHighlightTexture myGuiHighlightTexture = default(MyGuiHighlightTexture);
			myGuiHighlightTexture.Normal = texture.Path;
			myGuiHighlightTexture.Highlight = texture2.Path;
			myGuiHighlightTexture.SizePx = sizePx;
			MyGuiHighlightTexture itemTexture = myGuiHighlightTexture;
			MyGuiStyleDefinition customStyleDefinition = new MyGuiStyleDefinition
			{
				ItemTexture = itemTexture,
				ItemFontNormal = m_style.ItemStyle.FontNormal,
				ItemFontHighlight = m_style.ItemStyle.FontHighlight,
				SizeOverride = itemTexture.SizeGui * new Vector2(10f, 1f),
				ItemMargin = itemMargin,
				ItemTextScale = m_style.ItemStyle.TextScale,
				FitSizeToItems = true
			};
			m_toolbarItemsGrid.SetCustomStyleDefinition(customStyleDefinition);
			m_pageCompositeTexture = MyGuiCompositeTexture.CreateFromDefinition(m_style.PageStyle.PageCompositeTexture);
			m_pageHighlightCompositeTexture = MyGuiCompositeTexture.CreateFromDefinition(m_style.PageStyle.PageHighlightCompositeTexture);
			m_itemVarianTtexture = MyGuiTextures.Static.GetTexture(m_style.ItemStyle.VariantTexture);
			m_colorVariantPanel.BackgroundTexture = MyGuiCompositeTexture.CreateFromDefinition(m_style.ColorPanelStyle.Texture);
			m_skinVariantPanel.BackgroundTexture = MyGuiCompositeTexture.CreateFromDefinition(m_style.ColorPanelStyle.Texture);
			InitStatControls();
		}

		private void InitStatControls()
		{
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			Vector2 vector = new Vector2(fullscreenRectangle.Width, fullscreenRectangle.Height);
			if (m_style.StatControls != null)
			{
				MyObjectBuilder_StatControls[] statControls = m_style.StatControls;
				foreach (MyObjectBuilder_StatControls myObjectBuilder_StatControls in statControls)
				{
					float uiScale = myObjectBuilder_StatControls.ApplyHudScale ? (MyGuiManager.GetSafeScreenScale() * MyHud.HudElementsScaleMultiplier) : MyGuiManager.GetSafeScreenScale();
					MyStatControls myStatControls = new MyStatControls(myObjectBuilder_StatControls, uiScale);
					Vector2 coordScreen = myObjectBuilder_StatControls.Position * vector;
					myStatControls.Position = MyUtils.AlignCoord(coordScreen, vector, myObjectBuilder_StatControls.OriginAlign);
					m_statControls.Add(myStatControls);
				}
			}
		}

		private void RefreshSelectedItem(MyToolbar toolbar)
		{
			m_toolbarItemsGrid.SelectedIndex = toolbar.SelectedSlot;
			MyToolbarItem selectedItem = toolbar.SelectedItem;
			if (selectedItem != null)
			{
				m_selectedItemLabel.Text = selectedItem.DisplayName.ToString();
				m_colorVariantPanel.Visible = (selectedItem is MyToolbarItemCubeBlock && MyFakes.ENABLE_BLOCK_COLORING);
				m_skinVariantPanel.Visible = m_colorVariantPanel.Visible;
			}
			else
			{
				m_colorVariantPanel.Visible = false;
				m_skinVariantPanel.Visible = false;
				m_selectedItemLabel.Text = string.Empty;
			}
		}

		private void HighlightCurrentPageLabel()
		{
			int currentPage = m_shownToolbar.CurrentPage;
			for (int i = 0; i < m_pageLabelList.Count; i++)
			{
				if (i != currentPage && m_pageLabelList[i].BackgroundTexture == m_pageHighlightCompositeTexture)
				{
					m_pageLabelList[i].BackgroundTexture = m_pageCompositeTexture;
				}
				else if (i == currentPage && m_pageLabelList[i].BackgroundTexture == m_pageCompositeTexture)
				{
					m_pageLabelList[i].BackgroundTexture = m_pageHighlightCompositeTexture;
				}
			}
		}

		private void SetGridItemAt(int slot, MyToolbarItem item, bool clear = false)
		{
			if (item != null)
			{
				SetGridItemAt(slot, item, item.Icons, item.SubIcon, item.DisplayName.ToString(), GetSymbol(slot), clear);
			}
			else
			{
				SetGridItemAt(slot, null, null, null, null, GetSymbol(slot), clear);
			}
		}

		protected virtual void SetGridItemAt(int slot, MyToolbarItem item, string[] icons, string subicon, string tooltip, ColoredIcon? symbol = null, bool clear = false)
		{
			MyGuiGridItem myGuiGridItem = m_toolbarItemsGrid.GetItemAt(slot);
			if (myGuiGridItem == null)
			{
				myGuiGridItem = new MyGuiGridItem(icons, subicon, tooltip, item)
				{
					SubIconOffset = m_style.ItemStyle.VariantOffset
				};
				m_toolbarItemsGrid.SetItemAt(slot, myGuiGridItem);
			}
			else
			{
				myGuiGridItem.UserData = item;
				myGuiGridItem.Icons = icons;
				myGuiGridItem.SubIcon = subicon;
				myGuiGridItem.SubIconOffset = m_style.ItemStyle.VariantOffset;
				if (myGuiGridItem.ToolTip == null)
				{
					myGuiGridItem.ToolTip = new MyToolTips();
				}
				myGuiGridItem.ToolTip.ToolTips.Clear();
				myGuiGridItem.ToolTip.AddToolTip(tooltip);
			}
			if (item == null || clear)
			{
				myGuiGridItem.ClearAllText();
			}
			if (DrawNumbers)
			{
				myGuiGridItem.AddText(MyToolbarComponent.GetSlotControlText(slot));
			}
			item?.FillGridItem(myGuiGridItem);
			myGuiGridItem.Enabled = (item?.Enabled ?? true);
			if (symbol.HasValue)
			{
				myGuiGridItem.AddIcon(symbol.Value, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
		}

		private void RemoveToolbarItem(int slot)
		{
			if (slot < MyToolbarComponent.CurrentToolbar.SlotCount)
			{
				MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, null);
			}
		}

		private void InitStatConditions(ConditionBase conditionBase)
		{
			StatCondition statCondition = conditionBase as StatCondition;
			if (statCondition != null)
			{
				IMyHudStat stat = MyHud.Stats.GetStat(statCondition.StatId);
				statCondition.SetStat(stat);
				return;
			}
			Condition condition = conditionBase as Condition;
			if (condition != null)
			{
				ConditionBase[] terms = condition.Terms;
				foreach (ConditionBase conditionBase2 in terms)
				{
					InitStatConditions(conditionBase2);
				}
			}
		}

		private void ToolbarComponent_CurrentToolbarChanged()
		{
			ShowToolbar(MyToolbarComponent.CurrentToolbar);
		}

		private void Toolbar_SelectedSlotChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
		{
			RefreshSelectedItem(toolbar);
		}

		private void Toolbar_SlotActivated(MyToolbar toolbar, MyToolbar.SlotArgs args, bool userActivated)
		{
			m_toolbarItemsGrid.blinkSlot(args.SlotNumber);
		}

		private void Toolbar_ItemChanged(MyToolbar toolbar, MyToolbar.IndexArgs args)
		{
			UpdateItemAtIndex(toolbar, args.ItemIndex);
		}

		private void Toolbar_ItemUpdated(MyToolbar toolbar, MyToolbar.IndexArgs args, MyToolbarItem.ChangeInfo changes)
		{
			if (changes == MyToolbarItem.ChangeInfo.Icon)
			{
				UpdateItemIcon(toolbar, args);
			}
			else
			{
				UpdateItemAtIndex(toolbar, args.ItemIndex);
			}
		}

		private void UpdateItemAtIndex(MyToolbar toolbar, int index)
		{
			int num = toolbar.IndexToSlot(index);
			if (toolbar.IsValidIndex(index) && toolbar.IsValidSlot(num))
			{
				SetGridItemAt(num, toolbar[index], clear: true);
				if (toolbar.SelectedSlot == num)
				{
					RefreshSelectedItem(toolbar);
				}
			}
		}

		private void Toolbar_ItemEnabledChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
		{
			if (args.SlotNumber.HasValue)
			{
				int value = args.SlotNumber.Value;
				MyGuiGridItem itemAt = m_toolbarItemsGrid.GetItemAt(value);
				if (itemAt != null)
				{
					itemAt.Enabled = toolbar.IsEnabled(toolbar.SlotToIndex(value));
				}
				return;
			}
			for (int i = 0; i < m_toolbarItemsGrid.ColumnsCount; i++)
			{
				MyGuiGridItem itemAt2 = m_toolbarItemsGrid.GetItemAt(i);
				if (itemAt2 != null)
				{
					itemAt2.Enabled = toolbar.IsEnabled(toolbar.SlotToIndex(i));
				}
			}
		}

		private void UpdateItemIcon(MyToolbar toolbar, MyToolbar.IndexArgs args)
		{
			if (toolbar.IsValidIndex(args.ItemIndex))
			{
				int num = toolbar.IndexToSlot(args.ItemIndex);
				if (num != -1)
				{
					MyGuiGridItem itemAt = m_toolbarItemsGrid.GetItemAt(num);
					if (itemAt != null)
					{
						itemAt.Icons = toolbar.GetItemIcons(args.ItemIndex);
					}
				}
				return;
			}
			for (int i = 0; i < m_toolbarItemsGrid.ColumnsCount; i++)
			{
				MyGuiGridItem itemAt2 = m_toolbarItemsGrid.GetItemAt(i);
				if (itemAt2 != null)
				{
					itemAt2.Icons = toolbar.GetItemIcons(toolbar.SlotToIndex(i));
				}
			}
		}

		private void Toolbar_CurrentPageChanged(MyToolbar toolbar, MyToolbar.PageChangeArgs args)
		{
			if (UseContextMenu)
			{
				m_contextMenu.Deactivate();
			}
			HighlightCurrentPageLabel();
			for (int i = 0; i < MyToolbarComponent.CurrentToolbar.SlotCount; i++)
			{
				SetGridItemAt(i, toolbar.GetSlotItem(i), clear: true);
			}
		}

		private void grid_ItemClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			if (eventArgs.Button == MySharedButtonsEnum.Secondary)
			{
				int columnIndex = eventArgs.ColumnIndex;
				MyToolbar currentToolbar = MyToolbarComponent.CurrentToolbar;
				MyToolbarItem slotItem = currentToolbar.GetSlotItem(columnIndex);
				if (slotItem == null)
				{
					return;
				}
				if (slotItem is MyToolbarItemActions)
				{
					ListReader<ITerminalAction> listReader = (slotItem as MyToolbarItemActions).PossibleActions(ShownToolbar.ToolbarType);
					if (UseContextMenu && listReader.Count > 0)
					{
						m_contextMenu.CreateNewContextMenu();
						foreach (ITerminalAction item in listReader)
						{
							m_contextMenu.AddItem(item.Name, "", item.Icon, item.Id);
						}
						m_contextMenu.AddItem(MyTexts.Get(MySpaceTexts.BlockAction_RemoveFromToolbar));
						m_contextMenu.Enabled = true;
						m_contextMenuItemIndex = currentToolbar.SlotToIndex(columnIndex);
					}
					else
					{
						RemoveToolbarItem(eventArgs.ColumnIndex);
					}
				}
				else
				{
					RemoveToolbarItem(eventArgs.ColumnIndex);
				}
			}
			if (m_shownToolbar.IsValidIndex(eventArgs.ColumnIndex))
			{
				m_shownToolbar.ActivateItemAtSlot(eventArgs.ColumnIndex, checkIfWantsToBeActivated: true);
			}
		}

		private void grid_ItemDoubleClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			RemoveToolbarItem(eventArgs.ColumnIndex);
			if (m_shownToolbar.IsValidIndex(eventArgs.ColumnIndex))
			{
				m_shownToolbar.ActivateItemAtSlot(eventArgs.ColumnIndex);
			}
		}

		private void contextMenu_ItemClicked(MyGuiControlContextMenu sender, MyGuiControlContextMenu.EventArgs args)
		{
			int itemIndex = args.ItemIndex;
			MyToolbar currentToolbar = MyToolbarComponent.CurrentToolbar;
			if (currentToolbar == null)
			{
				return;
			}
			int slot = currentToolbar.IndexToSlot(m_contextMenuItemIndex);
			if (currentToolbar.IsValidSlot(slot))
			{
				MyToolbarItem slotItem = currentToolbar.GetSlotItem(slot);
				MyToolbarItemActions myToolbarItemActions = slotItem as MyToolbarItemActions;
				if (slotItem != null)
				{
					if (itemIndex < 0 || itemIndex >= myToolbarItemActions.PossibleActions(ShownToolbar.ToolbarType).Count)
					{
						RemoveToolbarItem(slot);
					}
					else
					{
						myToolbarItemActions.ActionId = (string)args.UserData;
						for (int i = 0; i < MyToolbarComponent.CurrentToolbar.SlotCount; i++)
						{
							MyToolbarItem slotItem2 = currentToolbar.GetSlotItem(i);
							if (slotItem2 != null && slotItem2.Equals(myToolbarItemActions))
							{
								MyToolbarComponent.CurrentToolbar.SetItemAtSlot(i, null);
							}
						}
						MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, myToolbarItemActions);
					}
				}
			}
			m_contextMenuItemIndex = -1;
		}

		public void HandleDragAndDrop(object sender, MyDragAndDropEventArgs eventArgs)
		{
			MyToolbarItem myToolbarItem = eventArgs.Item.UserData as MyToolbarItem;
			if (myToolbarItem != null)
			{
				int itemIndex = MyToolbarComponent.CurrentToolbar.GetItemIndex(myToolbarItem);
				if (eventArgs.DropTo != null && IsToolbarGrid(eventArgs.DropTo.Grid))
				{
					MyToolbarItem itemAtSlot = MyToolbarComponent.CurrentToolbar.GetItemAtSlot(eventArgs.DropTo.ItemIndex);
					int slot = MyToolbarComponent.CurrentToolbar.IndexToSlot(itemIndex);
					int itemIndex2 = eventArgs.DropTo.ItemIndex;
					MyToolbarComponent.CurrentToolbar.SetItemAtSlot(itemIndex2, myToolbarItem);
					MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, itemAtSlot);
				}
				else
				{
					MyToolbarComponent.CurrentToolbar.SetItemAtIndex(itemIndex, null);
				}
			}
		}
	}
}
