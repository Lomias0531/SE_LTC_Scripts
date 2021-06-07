using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlInventoryOwner : MyGuiControlBase
	{
		private static readonly StringBuilder m_textCache = new StringBuilder();

		private static readonly Vector2 m_internalPadding = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE;

		private MyGuiControlLabel m_nameLabel;

		private MyGuiControlLabel m_accountLabel;

		private MyGuiControlLabel m_accountValueLabel;

		private MyGuiControlImage m_imageCurrency;

		private List<MyGuiControlLabel> m_massLabels;

		private List<MyGuiControlLabel> m_volumeLabels;

		private List<MyGuiControlLabel> m_volumeValueLabels;

		private List<MyGuiControlGrid> m_inventoryGrids;

		private MyEntity m_inventoryOwner;

		public MyEntity InventoryOwner
		{
			get
			{
				return m_inventoryOwner;
			}
			set
			{
				if (m_inventoryOwner != value)
				{
					ReplaceCurrentInventoryOwner(value);
				}
			}
		}

		public List<MyGuiControlGrid> ContentGrids => m_inventoryGrids;

		public event Action<MyGuiControlInventoryOwner> InventoryContentsChanged;

		public MyGuiControlInventoryOwner(MyEntity owner, Vector4 labelColorMask)
			: base(null, null, null, null, new MyGuiCompositeTexture
			{
				Center = new MyGuiSizedTexture
				{
					Texture = "Textures\\GUI\\Controls\\item_highlight_dark.dds"
				}
			}, isActiveControl: false, canHaveFocus: true)
		{
			BorderHighlightEnabled = true;
			m_nameLabel = MakeLabel();
			m_nameLabel.ColorMask = labelColorMask;
			m_accountLabel = MakeLabel(MySpaceTexts.Currency_Default_Account_Label);
			m_accountLabel.ColorMask = labelColorMask;
			m_accountLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_accountLabel.AutoEllipsis = false;
			m_accountValueLabel = MakeLabel(MyStringId.GetOrCompute("N/A"));
			m_accountValueLabel.ColorMask = labelColorMask;
			m_accountValueLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
			float num = (float)safeGuiRectangle.Width / (float)safeGuiRectangle.Height;
			m_imageCurrency = new MyGuiControlImage
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = m_accountValueLabel.Position + new Vector2(0.005f, 0f),
				Name = "imageCurrency",
				Size = new Vector2(0.018f, num * 0.018f) * 0.85f,
				Visible = false
			};
			MyGuiControlImage imageCurrency = m_imageCurrency;
			string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
			imageCurrency.SetTexture((icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty);
			m_massLabels = new List<MyGuiControlLabel>();
			m_volumeLabels = new List<MyGuiControlLabel>();
			m_volumeValueLabels = new List<MyGuiControlLabel>();
			m_inventoryGrids = new List<MyGuiControlGrid>();
			ShowTooltipWhenDisabled = true;
			m_nameLabel.Name = "NameLabel";
			Elements.Add(m_nameLabel);
			Elements.Add(m_accountLabel);
			Elements.Add(m_accountValueLabel);
			Elements.Add(m_imageCurrency);
			InventoryOwner = owner;
			base.CanFocusChildren = true;
		}

		private MyGuiControlGrid MakeInventoryGrid(MyInventory inventory)
		{
			MyGuiControlGrid myGuiControlGrid = new MyGuiControlGrid();
			myGuiControlGrid.Name = "InventoryGrid";
			myGuiControlGrid.VisualStyle = MyGuiControlGridStyleEnum.Inventory;
			myGuiControlGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlGrid.ColumnsCount = 7;
			myGuiControlGrid.RowsCount = 1;
			myGuiControlGrid.ShowTooltipWhenDisabled = true;
			myGuiControlGrid.UserData = inventory;
			myGuiControlGrid.MouseOverIndexChanged += OnMouseOverInvItem;
			return myGuiControlGrid;
		}

		private void OnMouseOverInvItem(MyGuiControlGrid arg1, MyGuiControlGrid.EventArgs arg2)
		{
			if (arg2.ItemIndex != -1 && arg1.IsValidIndex(arg2.ItemIndex))
			{
				MyGuiGridItem itemAt = arg1.GetItemAt(arg2.ItemIndex);
				object userData;
				if ((userData = itemAt.UserData) is MyPhysicalInventoryItem)
				{
					MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)userData;
					itemAt.ToolTip.ToolTips.Clear();
					itemAt.ToolTip.AddToolTip(GenerateTooltip(item));
				}
			}
		}

		private MyGuiControlLabel MakeMassLabel(MyInventory inventory)
		{
			MyGuiControlLabel myGuiControlLabel = MakeLabel(MySpaceTexts.ScreenTerminalInventory_Mass);
			myGuiControlLabel.Name = "MassLabel";
			return myGuiControlLabel;
		}

		private MyGuiControlLabel MakeVolumeLabel(MyInventory inventory)
		{
			MyGuiControlLabel myGuiControlLabel = MakeLabel(MySpaceTexts.ScreenTerminalInventory_Volume);
			myGuiControlLabel.Name = "VolumeLabel";
			return myGuiControlLabel;
		}

		public override void OnRemoving()
		{
			if (m_inventoryOwner != null)
			{
				DetachOwner();
			}
			foreach (MyGuiControlGrid inventoryGrid in m_inventoryGrids)
			{
				DetachInventoryEvents(inventoryGrid);
			}
			m_inventoryGrids.Clear();
			this.InventoryContentsChanged = null;
			base.OnRemoving();
		}

		protected override void OnSizeChanged()
		{
			RefreshInternals();
			base.Size = ComputeControlSize();
			base.OnSizeChanged();
		}

		protected override void OnEnabledChanged()
		{
			RefreshInternals();
			base.OnEnabledChanged();
		}

		public override MyGuiControlBase HandleInput()
		{
			base.HandleInput();
			if (HandleInputElements() != null)
			{
				return this;
			}
			return null;
		}

		public override void Update()
		{
			m_nameLabel.Text = m_inventoryOwner.DisplayNameText;
			m_nameLabel.Size = new Vector2(base.Size.X - m_internalPadding.X * 2f, m_nameLabel.Size.Y);
			base.Update();
		}

		private void RefreshInternals()
		{
			if (m_nameLabel != null)
			{
				Vector2 vector = base.Size - m_internalPadding * 2f;
				m_nameLabel.Position = ComputeControlPositionFromTopLeft(Vector2.Zero);
				m_nameLabel.Size = new Vector2(vector.X, m_nameLabel.Size.Y);
				Vector2 value = ComputeControlPositionFromTopLeft(Vector2.Zero) + new Vector2(vector.X - m_internalPadding.X * 0.4f, 0f);
				m_accountValueLabel.Position = value - new Vector2(m_imageCurrency.Size.X + 0.002f, 0f);
				m_accountValueLabel.Size = new Vector2(vector.X * 0.3f, m_nameLabel.Size.Y);
				m_imageCurrency.Position = m_accountValueLabel.Position + new Vector2(0.004f, 0.0005f);
				MyPlayer myPlayer = null;
				MyEntity entity;
				if ((entity = (MySession.Static.ControlledEntity as MyEntity)) != null)
				{
					myPlayer = MySession.Static.Players.GetControllingPlayer(entity);
				}
				if (myPlayer != null && myPlayer.Identity != null && myPlayer.Character == m_inventoryOwner)
				{
					m_accountLabel.Visible = true;
					m_accountValueLabel.Visible = true;
					m_accountValueLabel.Text = MyBankingSystem.Static.GetBalanceShortString(myPlayer.Identity.IdentityId, addCurrencyShortName: false);
					m_imageCurrency.Visible = true;
				}
				else
				{
					m_accountLabel.Visible = false;
					m_accountValueLabel.Visible = false;
					m_imageCurrency.Visible = false;
				}
				m_accountLabel.Position = m_accountValueLabel.Position - new Vector2(m_accountValueLabel.Size.X, 0f);
				Vector2 vector2 = ComputeControlPositionFromTopLeft(new Vector2(0f, 0.03f));
				RefreshInventoryGridSizes();
				for (int i = 0; i < m_inventoryGrids.Count; i++)
				{
					MyGuiControlLabel myGuiControlLabel = m_massLabels[i];
					MyGuiControlLabel myGuiControlLabel2 = m_volumeLabels[i];
					MyGuiControlLabel myGuiControlLabel3 = m_volumeValueLabels[i];
					MyGuiControlGrid myGuiControlGrid = m_inventoryGrids[i];
					myGuiControlLabel.Position = vector2 + new Vector2(0.005f, -0.005f);
					myGuiControlLabel2.Position = new Vector2(m_accountLabel.Position.X - m_accountLabel.Size.X, myGuiControlLabel.Position.Y);
					myGuiControlLabel3.Position = new Vector2(value.X, myGuiControlLabel2.Position.Y);
					myGuiControlLabel.Size = new Vector2(myGuiControlLabel2.Position.X - myGuiControlLabel.Position.X, myGuiControlLabel.Size.Y);
					myGuiControlLabel2.Size = new Vector2(vector.X - myGuiControlLabel.Size.X, myGuiControlLabel2.Size.Y);
					vector2.Y += myGuiControlLabel.Size.Y + m_internalPadding.Y * 0.5f;
					myGuiControlGrid.Position = vector2;
					vector2.Y += myGuiControlGrid.Size.Y + m_internalPadding.Y;
				}
			}
		}

		private void RefreshInventoryContents()
		{
			if (m_inventoryOwner == null)
			{
				return;
			}
			for (int i = 0; i < m_inventoryOwner.InventoryCount; i++)
			{
				MyInventory inventory = m_inventoryOwner.GetInventory(i);
				if (inventory == null)
				{
					continue;
				}
				MyGuiControlGrid myGuiControlGrid = m_inventoryGrids[i];
				MyGuiControlLabel myGuiControlLabel = m_massLabels[i];
				_ = m_volumeLabels[i];
				MyGuiControlLabel myGuiControlLabel2 = m_volumeValueLabels[i];
				int? selectedIndex = myGuiControlGrid.SelectedIndex;
				myGuiControlGrid.Clear();
				myGuiControlLabel.UpdateFormatParams(((double)inventory.CurrentMass).ToString("N", CultureInfo.InvariantCulture));
				string text = ((double)MyFixedPoint.MultiplySafe(inventory.CurrentVolume, 1000)).ToString("N", CultureInfo.InvariantCulture);
				if (inventory.IsConstrained)
				{
					text = text + " / " + ((double)MyFixedPoint.MultiplySafe(inventory.MaxVolume, 1000)).ToString("N", CultureInfo.InvariantCulture);
				}
				myGuiControlLabel2.UpdateFormatParams(text);
				if (inventory.Constraint != null)
				{
					myGuiControlGrid.EmptyItemIcon = inventory.Constraint.Icon;
					myGuiControlGrid.SetEmptyItemToolTip(inventory.Constraint.Description);
				}
				else
				{
					myGuiControlGrid.EmptyItemIcon = null;
					myGuiControlGrid.SetEmptyItemToolTip(null);
				}
				foreach (MyPhysicalInventoryItem item in inventory.GetItems())
				{
					myGuiControlGrid.Add(CreateInventoryGridItem(item));
				}
				if (selectedIndex.HasValue)
				{
					if (myGuiControlGrid.IsValidIndex(selectedIndex.Value))
					{
						myGuiControlGrid.SelectedIndex = selectedIndex;
					}
					else
					{
						myGuiControlGrid.SelectLastItem();
					}
				}
				else
				{
					myGuiControlGrid.SelectedIndex = null;
				}
			}
			RefreshInventoryGridSizes();
			base.Size = ComputeControlSize();
			RefreshInternals();
		}

		private void RefreshInventoryGridSizes()
		{
			foreach (MyGuiControlGrid inventoryGrid in m_inventoryGrids)
			{
				int count = ((MyInventory)inventoryGrid.UserData).GetItems().Count;
				inventoryGrid.ColumnsCount = Math.Max(1, (int)((base.Size.X - m_internalPadding.X * 2f) / (inventoryGrid.ItemSize.X * 1.01f)));
				inventoryGrid.RowsCount = Math.Max(1, (int)Math.Ceiling((float)(count + 1) / (float)inventoryGrid.ColumnsCount));
				inventoryGrid.TrimEmptyItems();
			}
		}

		private Vector2 ComputeControlPositionFromTopLeft(Vector2 offset)
		{
			return m_internalPadding + base.Size * -0.5f + offset;
		}

		private Vector2 ComputeControlPositionFromTopCenter(Vector2 offset)
		{
			return new Vector2(0f, m_internalPadding.Y + base.Size.Y * -0.5f) + offset;
		}

		private MyGuiControlLabel MakeLabel(MyStringId? text = null, MyGuiDrawAlignEnum labelAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			float textScale = 612f / 925f;
			return new MyGuiControlLabel(null, null, text.HasValue ? MyTexts.GetString(text.Value) : null, null, textScale, "Blue", labelAlign)
			{
				AutoEllipsis = true
			};
		}

		private void ReplaceCurrentInventoryOwner(MyEntity owner)
		{
			DetachOwner();
			AttachOwner(owner);
		}

		private void inventory_OnContentsChanged(MyInventoryBase obj)
		{
			RefreshInventoryContents();
			if (this.InventoryContentsChanged != null)
			{
				this.InventoryContentsChanged(this);
			}
		}

		private Vector2 ComputeControlSize()
		{
			float num = m_nameLabel.Size.Y + m_internalPadding.Y * 2f;
			for (int i = 0; i < m_inventoryGrids.Count; i++)
			{
				MyGuiControlGrid myGuiControlGrid = m_inventoryGrids[i];
				MyGuiControlLabel myGuiControlLabel = m_massLabels[i];
				num += myGuiControlLabel.Size.Y + m_internalPadding.Y * 0.5f;
				num += myGuiControlGrid.Size.Y + m_internalPadding.Y;
			}
			return new Vector2(base.Size.X, num);
		}

		private void AttachOwner(MyEntity owner)
		{
			if (owner != null)
			{
				m_nameLabel.Text = owner.DisplayNameText;
				for (int i = 0; i < owner.InventoryCount; i++)
				{
					MyInventory inventory = owner.GetInventory(i);
					inventory.UserData = this;
					inventory.ContentsChanged += inventory_OnContentsChanged;
					MyGuiControlLabel myGuiControlLabel = MakeMassLabel(inventory);
					Elements.Add(myGuiControlLabel);
					m_massLabels.Add(myGuiControlLabel);
					MyGuiControlLabel myGuiControlLabel2 = MakeVolumeLabel(inventory);
					Elements.Add(myGuiControlLabel2);
					m_volumeLabels.Add(myGuiControlLabel2);
					MyGuiControlLabel myGuiControlLabel3 = MakeLabel(null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
					myGuiControlLabel3.Text = MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_VolumeValue);
					myGuiControlLabel3.AutoEllipsis = false;
					Elements.Add(myGuiControlLabel3);
					m_volumeValueLabels.Add(myGuiControlLabel3);
					MyGuiControlGrid myGuiControlGrid = MakeInventoryGrid(inventory);
					Elements.Add(myGuiControlGrid);
					m_inventoryGrids.Add(myGuiControlGrid);
				}
				m_inventoryOwner = owner;
				RefreshInventoryContents();
			}
		}

		private void DetachInventoryEvents(MyGuiControlGrid inventory)
		{
			inventory.MouseOverIndexChanged -= OnMouseOverInvItem;
		}

		private void DetachOwner()
		{
			if (m_inventoryOwner == null)
			{
				return;
			}
			for (int i = 0; i < m_inventoryOwner.InventoryCount; i++)
			{
				MyInventory inventory = m_inventoryOwner.GetInventory(i);
				if (inventory.UserData == this)
				{
					inventory.UserData = null;
				}
				inventory.ContentsChanged -= inventory_OnContentsChanged;
			}
			for (int j = 0; j < m_inventoryGrids.Count; j++)
			{
				DetachInventoryEvents(m_inventoryGrids[j]);
				Elements.Remove(m_massLabels[j]);
				Elements.Remove(m_volumeLabels[j]);
				Elements.Remove(m_volumeValueLabels[j]);
				Elements.Remove(m_inventoryGrids[j]);
			}
			m_inventoryGrids.Clear();
			m_massLabels.Clear();
			m_volumeLabels.Clear();
			m_volumeValueLabels.Clear();
			m_inventoryOwner = null;
		}

		public static void FormatItemAmount(MyPhysicalInventoryItem item, StringBuilder text)
		{
			Type type = item.Content.GetType();
			double amount = (double)item.Amount;
			if (item.Content.GetType() == typeof(MyObjectBuilder_GasContainerObject) || item.Content.GetType().BaseType == typeof(MyObjectBuilder_GasContainerObject))
			{
				amount = ((MyObjectBuilder_GasContainerObject)item.Content).GasLevel * 100f;
			}
			FormatItemAmount(type, amount, text);
		}

		public static void FormatItemAmount(Type typeId, double amount, StringBuilder text)
		{
			try
			{
				if (typeId == typeof(MyObjectBuilder_Ore) || typeId == typeof(MyObjectBuilder_Ingot))
				{
					if (amount < 0.01)
					{
						text.Append(amount.ToString("<0.01", CultureInfo.InvariantCulture));
					}
					else if (amount < 10.0)
					{
						text.Append(amount.ToString("0.##", CultureInfo.InvariantCulture));
					}
					else if (amount < 100.0)
					{
						text.Append(amount.ToString("0.#", CultureInfo.InvariantCulture));
					}
					else if (amount < 1000.0)
					{
						text.Append(amount.ToString("0.", CultureInfo.InvariantCulture));
					}
					else if (amount < 10000.0)
					{
						text.Append((amount / 1000.0).ToString("0.##k", CultureInfo.InvariantCulture));
					}
					else if (amount < 100000.0)
					{
						text.Append((amount / 1000.0).ToString("0.#k", CultureInfo.InvariantCulture));
					}
					else
					{
						text.Append((amount / 1000.0).ToString("#,##0.k", CultureInfo.InvariantCulture));
					}
				}
				else if (!(typeId == typeof(MyObjectBuilder_PhysicalGunObject)))
				{
					if (typeId == typeof(MyObjectBuilder_GasContainerObject) || typeId.BaseType == typeof(MyObjectBuilder_GasContainerObject))
					{
						text.Append((int)amount + "%");
					}
					else
					{
						int num = (int)amount;
						if (amount - (double)num > 0.0)
						{
							text.Append('~');
						}
						if (amount < 10000.0)
						{
							text.Append(num.ToString("#,##0.x", CultureInfo.InvariantCulture));
						}
						else
						{
							text.Append((num / 1000).ToString("#,##0.k", CultureInfo.InvariantCulture));
						}
					}
				}
			}
			catch (OverflowException)
			{
				text.Append("ERROR");
			}
		}

		public static string GenerateTooltip(MyPhysicalInventoryItem item)
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(item.Content);
			double num = (double)physicalItemDefinition.Mass * (double)item.Amount;
			double num2 = (double)(physicalItemDefinition.Volume * 1000f) * (double)item.Amount;
			return string.Format(MyTexts.GetString(MySpaceTexts.ToolTipTerminalInventory_ItemInfo), physicalItemDefinition.GetTooltipDisplayName(item.Content), (num < 0.01) ? "<0.01" : num.ToString("N", CultureInfo.InvariantCulture), (num2 < 0.01) ? "<0.01" : num2.ToString("N", CultureInfo.InvariantCulture), (item.Content.Flags == MyItemFlags.Damaged) ? MyTexts.Get(MyCommonTexts.ItemDamagedDescription) : MyTexts.Get(MySpaceTexts.Blank), (physicalItemDefinition.ExtraInventoryTooltipLine != null) ? physicalItemDefinition.ExtraInventoryTooltipLine : MyTexts.Get(MySpaceTexts.Blank));
		}

		public static MyGuiGridItem CreateInventoryGridItem(MyPhysicalInventoryItem item)
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(item.Content);
			MyGuiGridItem myGuiGridItem = new MyGuiGridItem(physicalItemDefinition.Icons, null, userData: item, toolTip: GenerateTooltip(item));
			if (MyFakes.SHOW_INVENTORY_ITEM_IDS)
			{
				myGuiGridItem.ToolTip.AddToolTip(new StringBuilder().AppendFormat("ItemID: {0}", item.ItemId).ToString());
			}
			FormatItemAmount(item, m_textCache);
			myGuiGridItem.AddText(m_textCache, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
			m_textCache.Clear();
			if (physicalItemDefinition.IconSymbol.HasValue)
			{
				myGuiGridItem.AddText(MyTexts.Get(physicalItemDefinition.IconSymbol.Value));
			}
			return myGuiGridItem;
		}

		public void RefreshOwnerInventory()
		{
			for (int i = 0; i < m_inventoryOwner.InventoryCount; i++)
			{
				MyInventory inventory = m_inventoryOwner.GetInventory(i);
				inventory.UserData = this;
				inventory.ContentsChanged += inventory_OnContentsChanged;
			}
		}

		public void RemoveInventoryEvents()
		{
			for (int i = 0; i < m_inventoryOwner.InventoryCount; i++)
			{
				m_inventoryOwner.GetInventory(i).ContentsChanged -= inventory_OnContentsChanged;
			}
		}

		public override void OnFocusChanged(MyGuiControlBase control, bool focus)
		{
			base.Owner?.OnFocusChanged(control, focus);
		}
	}
}
