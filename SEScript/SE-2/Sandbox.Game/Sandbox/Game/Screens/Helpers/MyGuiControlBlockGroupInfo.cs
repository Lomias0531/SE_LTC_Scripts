using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlBlockGroupInfo))]
	public class MyGuiControlBlockGroupInfo : MyGuiControlStackPanel
	{
		private MyGuiControlLabel m_title;

		private MyGuiControlGrid m_blockVariantGrid;

		private MyGuiControlMultilineText m_helpText;

		private MyGuiControlBlockInfo m_componentsInfo;

		private MyGuiControlPanel m_helpTextBackground;

		private MyGuiControlPanel m_componentsBackground;

		private MyGuiControlButton m_blockTypeIconSmall;

		private MyGuiControlButton m_blockTypeIconLarge;

		private MyGuiControlGrid m_blocksBuildPlanner;

		private MyCubeSize m_userSizeChoice;

		private float m_blockNameOriginalOffset;

		public MyCubeBlockDefinition SelectedDefinition
		{
			get;
			private set;
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			m_userSizeChoice = MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode;
			m_title = new MyGuiControlLabel();
			m_title.Size = new Vector2(0.77f, 1f);
			MyGuiControlButton.StyleDefinition customStyle = new MyGuiControlButton.StyleDefinition
			{
				NormalFont = "White",
				HighlightFont = "White",
				NormalTexture = MyGuiConstants.TEXTURE_HUD_GRID_LARGE_FIT,
				HighlightTexture = MyGuiConstants.TEXTURE_HUD_GRID_LARGE_FIT
			};
			MyGuiControlButton.StyleDefinition customStyle2 = new MyGuiControlButton.StyleDefinition
			{
				NormalFont = "White",
				HighlightFont = "White",
				NormalTexture = MyGuiConstants.TEXTURE_HUD_GRID_SMALL_FIT,
				HighlightTexture = MyGuiConstants.TEXTURE_HUD_GRID_SMALL_FIT
			};
			m_blockTypeIconLarge = new MyGuiControlButton();
			m_blockTypeIconLarge.SetCustomStyle(customStyle);
			m_blockTypeIconLarge.Size = new Vector2(0f, 0.7f);
			Thickness margin = new Thickness(0.01f, 0.15f, 0f, 0f);
			m_blockTypeIconLarge.Margin = margin;
			m_blockTypeIconSmall = new MyGuiControlButton();
			m_blockTypeIconSmall.SetCustomStyle(customStyle2);
			m_blockTypeIconSmall.Size = m_blockTypeIconLarge.Size;
			margin.Left = 0.05f;
			m_blockTypeIconSmall.Margin = margin;
			m_blockTypeIconSmall.ClickCallbackRespectsEnabledState = false;
			m_blockTypeIconLarge.ClickCallbackRespectsEnabledState = false;
			m_blockTypeIconLarge.ButtonClicked += OnSizeSelectorClicked;
			m_blockTypeIconSmall.ButtonClicked += OnSizeSelectorClicked;
			MyGuiControlStackPanel myGuiControlStackPanel = new MyGuiControlStackPanel();
			myGuiControlStackPanel.Orientation = MyGuiOrientation.Horizontal;
			myGuiControlStackPanel.Size = new Vector2(0.95f, 0.06f);
			myGuiControlStackPanel.Margin = new Thickness(0.025f, 0f, 0f, 0f);
			myGuiControlStackPanel.Add(m_title);
			myGuiControlStackPanel.Add(m_blockTypeIconSmall);
			myGuiControlStackPanel.Add(m_blockTypeIconLarge);
			Add(myGuiControlStackPanel);
			m_blockVariantGrid = new MyGuiControlGrid();
			m_blockVariantGrid.VisualStyle = MyGuiControlGridStyleEnum.BlockInfo;
			m_blockVariantGrid.RowsCount = 1;
			m_blockVariantGrid.ColumnsCount = 8;
			m_blockVariantGrid.Size = new Vector2(1f, 0.073f);
			m_blockVariantGrid.Margin = new Thickness(0.013f, 0f, 0f, 0f);
			m_blockVariantGrid.ItemSelected += OnBlockVariantSelected;
			m_blockVariantGrid.ItemClicked += OnBlockVariantGrid_ItemClicked;
			m_blockVariantGrid.ItemAccepted += OnBlockVariantGrid_ItemClicked;
			Add(m_blockVariantGrid);
			m_helpTextBackground = new MyGuiControlPanel();
			m_helpTextBackground.Size = new Vector2(0.95f, 0.23f);
			m_helpTextBackground.Margin = new Thickness(0.025f, 0f, 0f, 0.01f);
			m_helpTextBackground.ColorMask = new Vector4(142f / (339f * MathF.PI), 46f / 255f, 52f / 255f, 0.9f);
			m_helpTextBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
			m_helpText = new MyGuiControlMultilineText(null, null, null, "Blue", 0.640000045f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_helpText.Size = new Vector2(1f, 1f);
			m_helpText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_helpText.OnLinkClicked += delegate(MyGuiControlBase x, string y)
			{
				MyGameService.OpenOverlayUrl(y);
			};
			Add(m_helpTextBackground);
			m_componentsBackground = new MyGuiControlPanel();
			m_componentsBackground.Size = new Vector2(0.95f, 0.484f);
			m_componentsBackground.Margin = new Thickness(0.025f, 0f, 0f, 0f);
			m_componentsBackground.ColorMask = new Vector4(142f / (339f * MathF.PI), 46f / 255f, 52f / 255f, 0.9f);
			m_componentsBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
			Add(m_componentsBackground);
			m_componentsInfo = CreateBlockInfoControl();
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
			myGuiControlLabel.Size = new Vector2(0.77f, 1f);
			myGuiControlLabel.Text = MyTexts.Get(MySpaceTexts.BuildPlanner).ToString();
			myGuiControlLabel.Margin = new Thickness(0.025f, 0.015f, 0.025f, 0.015f);
			Add(myGuiControlLabel);
			m_blocksBuildPlanner = new MyGuiControlGrid();
			m_blocksBuildPlanner.VisualStyle = MyGuiControlGridStyleEnum.BlockInfo;
			m_blocksBuildPlanner.RowsCount = 1;
			m_blocksBuildPlanner.ColumnsCount = 1;
			m_blocksBuildPlanner.Size = new Vector2(0.975f, 0.073f);
			m_blocksBuildPlanner.Margin = new Thickness(0.013f, 0f, 0.025f, 0f);
			m_blocksBuildPlanner.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			m_blocksBuildPlanner.ItemDoubleClicked += blocksToDo_ItemDoubleClicked;
			m_blocksBuildPlanner.ItemClicked += blocksToDo_ItemClicked;
			m_blocksBuildPlanner.ItemAccepted += blocksToDo_ItemClicked;
			m_blocksBuildPlanner.ItemBackgroundColorMask = new Vector4(0.882352948f, 239f / 255f, 1f, 1f);
			Add(m_blocksBuildPlanner);
			UpdateBuildPlanner();
			ForEachChild(delegate(MyGuiControlStackPanel parent, MyGuiControlBase control)
			{
				Vector2 size = parent.Size;
				Vector2 size2 = control.Size * size;
				if (size2.X == 0f)
				{
					size2.X = size2.Y;
				}
				else if (size2.Y == 0f)
				{
					size2.Y = size2.X;
				}
				if (control is MyGuiControlButton)
				{
					size2 *= new Vector2(0.75f, 1f);
				}
				control.Size = size2;
				control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				Thickness margin2 = control.Margin;
				control.Margin = new Thickness(margin2.Left * size.X, margin2.Top * size.Y, margin2.Right * size.X, margin2.Bottom * size.Y);
			});
		}

		private void OnBlockVariantGrid_ItemClicked(MyGuiControlGrid arg1, MyGuiControlGrid.EventArgs arg2)
		{
			if (arg2.Button == MySharedButtonsEnum.Ternary)
			{
				m_blockVariantGrid.SelectedIndex = arg2.ItemIndex;
				if (MySession.Static.LocalCharacter.AddToBuildPlanner(SelectedDefinition))
				{
					UpdateBuildPlanner();
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				}
			}
		}

		private void blocksToDo_ItemDoubleClicked(MyGuiControlGrid arg1, MyGuiControlGrid.EventArgs arg2)
		{
			MySession.Static.LocalCharacter.RemoveAtBuildPlanner(arg2.ColumnIndex);
			UpdateBuildPlanner();
		}

		private void blocksToDo_ItemClicked(MyGuiControlGrid arg1, MyGuiControlGrid.EventArgs arg2)
		{
			if (arg2.Button == MySharedButtonsEnum.Primary)
			{
				if (SelectedDefinition != null && arg2.ColumnIndex == MySession.Static.LocalCharacter.BuildPlanner.Count && MySession.Static.LocalCharacter.AddToBuildPlanner(SelectedDefinition))
				{
					UpdateBuildPlanner();
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				}
			}
			else if (arg2.Button == MySharedButtonsEnum.Secondary)
			{
				MySession.Static.LocalCharacter.RemoveAtBuildPlanner(arg2.ColumnIndex);
				MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
			}
			UpdateBuildPlanner();
		}

		private void OnSizeSelectorClicked(MyGuiControlButton x)
		{
			OnUserSizePreferenceChanged((x != m_blockTypeIconLarge) ? MyCubeSize.Small : MyCubeSize.Large);
		}

		public void OnUserSizePreferenceChanged(MyCubeSize targetSize)
		{
			if (m_userSizeChoice != targetSize)
			{
				MyGuiControlButton myGuiControlButton = (targetSize == MyCubeSize.Small) ? m_blockTypeIconSmall : m_blockTypeIconLarge;
				MyGuiControlButton myGuiControlButton2 = (targetSize == MyCubeSize.Large) ? m_blockTypeIconSmall : m_blockTypeIconLarge;
				if (myGuiControlButton2.Visible)
				{
					m_userSizeChoice = targetSize;
					myGuiControlButton.Enabled = !myGuiControlButton.Enabled;
					myGuiControlButton2.Enabled = !myGuiControlButton.Enabled;
					RecreateDetail();
				}
			}
		}

		private void OnBlockVariantSelected(MyGuiControlGrid _, MyGuiControlGrid.EventArgs args)
		{
			RecreateDetail();
		}

		private void RecreateDetail()
		{
			MyGuiGridItem selectedItem = m_blockVariantGrid.SelectedItem;
			if (selectedItem == null)
			{
				m_blockVariantGrid.SelectedIndex = 0;
			}
			else
			{
				(selectedItem.UserData as MyGuiScreenToolbarConfigBase.GridItemUserData).Action();
			}
		}

		private void SetBlockDetail(MyCubeBlockDefinition[] definitions)
		{
			int num = 0;
			MyCubeBlockDefinition myCubeBlockDefinition;
			while (true)
			{
				if (num >= definitions.Length)
				{
					return;
				}
				myCubeBlockDefinition = definitions[num];
				if (myCubeBlockDefinition != null)
				{
					SetTexts(myCubeBlockDefinition);
					MyGuiControlButton myGuiControlButton = (myCubeBlockDefinition.CubeSize == MyCubeSize.Large) ? m_blockTypeIconLarge : m_blockTypeIconSmall;
					if (myGuiControlButton.Enabled && myGuiControlButton.Visible)
					{
						break;
					}
				}
				num++;
			}
			SelectedDefinition = myCubeBlockDefinition;
			m_componentsInfo.BlockInfo.LoadDefinition(myCubeBlockDefinition);
		}

		private void SetGeneralDefinitionDetail(MyDefinitionBase definition)
		{
			SelectedDefinition = (definition as MyCubeBlockDefinition);
			SetTexts(definition);
		}

		private void SetTexts(MyDefinitionBase definition)
		{
			StringBuilder stringBuilder = definition.DisplayNameEnum.HasValue ? MyTexts.Get(definition.DisplayNameEnum.Value) : new StringBuilder(definition.DisplayNameText);
			Vector2 vector = MyGuiManager.MeasureString(m_title.Font, stringBuilder, 1f);
			float num = Math.Min(m_title.Size.X / vector.X, 1f);
			Vector2 size = m_title.Size;
			m_title.TextToDraw = stringBuilder;
			m_title.TextScale = num / MyGuiManager.LanguageTextScale;
			m_title.Size = size;
			m_title.PositionY = m_blockNameOriginalOffset + (size.Y - vector.Y * m_title.TextScaleWithLanguage) / 2f;
			m_helpText.Text = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(definition.DescriptionText))
			{
				m_helpText.AppendText(definition.DescriptionText);
			}
			if (definition is MyCubeBlockDefinition && !MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, definition.Id))
			{
				AppendSpacingIfNeeded();
				m_helpText.AppendText(MyTexts.Get(MySpaceTexts.NotificationBlockNotResearched));
				return;
			}
			MyDLCs.MyDLC firstMissingDefinitionDLC = MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
			if (firstMissingDefinitionDLC != null)
			{
				AppendSpacingIfNeeded();
				m_helpText.AppendImage(firstMissingDefinitionDLC.Icon, new Vector2(20f, 20f) / MyGuiConstants.GUI_OPTIMAL_SIZE, Color.White);
				m_helpText.AppendText("     ");
				m_helpText.AppendLink(firstMissingDefinitionDLC.URL, MyDLCs.GetRequiredDLCStoreHint(firstMissingDefinitionDLC.AppId));
				return;
			}
			MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(definition.Id.SubtypeName);
			if (inventoryItemDefinition != null && inventoryItemDefinition.CanBePurchased && (inventoryItemDefinition.ItemSlot == MyGameInventoryItemSlot.Emote || inventoryItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor))
			{
				AppendSpacingIfNeeded();
				m_helpText.AppendImage(MyGuiConstants.TEXTURE_ICON_FAKE.Texture, new Vector2(20f, 20f) / MyGuiConstants.GUI_OPTIMAL_SIZE, Color.White);
				m_helpText.AppendText("     ");
				m_helpText.AppendLink($"https://store.steampowered.com/itemstore/244850/detail/{inventoryItemDefinition.ID}", MyTexts.GetString(MyCommonTexts.ShowInGameInventoryStore));
			}
			void AppendSpacingIfNeeded()
			{
				if (!string.IsNullOrWhiteSpace(definition.DescriptionText))
				{
					m_helpText.AppendText("\n");
					m_helpText.AppendText("\n");
				}
			}
		}

		public override void UpdateArrange()
		{
			base.UpdateArrange();
			ForEachChild(delegate(MyGuiControlStackPanel _, MyGuiControlBase child)
			{
				MyGuiControlStackPanel myGuiControlStackPanel;
				if ((myGuiControlStackPanel = (child as MyGuiControlStackPanel)) != null)
				{
					myGuiControlStackPanel.UpdateArrange();
				}
			});
			m_helpText.Size = m_helpTextBackground.Size * 0.9f;
			m_helpText.Position = m_helpTextBackground.Position + m_helpTextBackground.Size * 0.05f;
			m_blockNameOriginalOffset = m_title.PositionY;
			m_componentsInfo.Size = m_componentsBackground.Size;
			m_componentsInfo.Position = m_componentsBackground.Position;
		}

		public void SetBlockGroup(MyCubeBlockDefinitionGroup group)
		{
			MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup = group;
			MyBlockVariantGroup blockVariantsGroup = group.AnyPublic.BlockVariantsGroup;
			if (blockVariantsGroup != null)
			{
				group = blockVariantsGroup.BlockGroups[0];
			}
			ClearGrid();
			SetBlockModeEnabled(enabled: true);
			HashSet<MyCubeBlockDefinitionGroup> blockGroups = new HashSet<MyCubeBlockDefinitionGroup>();
			if (blockVariantsGroup != null)
			{
				MyCubeBlockDefinition[] blocks = blockVariantsGroup.Blocks;
				for (int i = 0; i < blocks.Length; i++)
				{
					string blockPairName = blocks[i].BlockPairName;
					if (blockPairName != null)
					{
						blockGroups.Add(MyDefinitionManager.Static.GetDefinitionGroup(blockPairName));
					}
				}
			}
			else
			{
				blockGroups.Add(group);
				AddStages(group.Small);
				AddStages(group.Large);
			}
			m_blockVariantGrid.ColumnsCount = blockGroups.Count;
			int num = 0;
			int value = 0;
			foreach (MyCubeBlockDefinitionGroup item in blockGroups)
			{
				MyCubeBlockDefinition lhs = item.Small;
				MyCubeBlockDefinition rhs = item.Large;
				if (m_userSizeChoice == MyCubeSize.Large)
				{
					MyUtils.Swap(ref lhs, ref rhs);
				}
				AddItemVariantDefinition(lhs, rhs);
				if (item == myCubeBlockDefinitionGroup)
				{
					value = num;
				}
				num++;
			}
			m_blockVariantGrid.SelectedIndex = value;
			void AddStages(MyCubeBlockDefinition block)
			{
				if (block?.BlockStages != null)
				{
					MyDefinitionId[] blockStages = block.BlockStages;
					foreach (MyDefinitionId id in blockStages)
					{
						MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
						if (cubeBlockDefinition != null)
						{
							MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName);
							if (blockGroups.Add(definitionGroup))
							{
								AddStages(definitionGroup.Small);
								AddStages(definitionGroup.Large);
							}
						}
					}
				}
			}
		}

		private void UpdateSizeIcons(bool smallExists, bool largeExists)
		{
			m_blockTypeIconSmall.Visible = smallExists;
			m_blockTypeIconLarge.Visible = largeExists;
			MyGuiControlButton lhs = m_blockTypeIconSmall;
			MyGuiControlButton rhs = m_blockTypeIconLarge;
			if (m_userSizeChoice == MyCubeSize.Large)
			{
				MyUtils.Swap(ref lhs, ref rhs);
			}
			if (!lhs.Visible)
			{
				MyUtils.Swap(ref lhs, ref rhs);
			}
			lhs.Enabled = true;
			rhs.Enabled = false;
		}

		public void SetGeneralDefinition(MyDefinitionBase definition)
		{
			ClearGrid();
			SetBlockModeEnabled(enabled: false);
			m_blockVariantGrid.Add(new MyGuiGridItem(definition.Icons, (string)null, (string)null, (object)new MyGuiScreenToolbarConfigBase.GridItemUserData
			{
				Action = delegate
				{
					SetGeneralDefinitionDetail(definition);
				},
				ItemData = (() => MyToolbarItemFactory.ObjectBuilderFromDefinition(definition))
			}, enabled: true, 1f));
			m_blockVariantGrid.SelectedIndex = 0;
			m_blockVariantGrid.ColumnsCount = 1;
		}

		private void ClearGrid()
		{
			m_blockVariantGrid.SelectedIndex = null;
			m_blockVariantGrid.SetItemsToDefault();
		}

		public void SetBlockModeEnabled(bool enabled)
		{
			m_componentsInfo.Visible = enabled;
			m_blockTypeIconLarge.Visible = false;
			m_blockTypeIconSmall.Visible = false;
			m_componentsBackground.Visible = enabled;
		}

		private void AddItemVariantDefinition(MyCubeBlockDefinition primary, MyCubeBlockDefinition secondary)
		{
			if (primary != null)
			{
				_ = secondary;
			}
			string text = null;
			string[] icons = null;
			if (IsAllowed(primary))
			{
				icons = primary.Icons;
				text = GetSubIcon(primary);
			}
			else
			{
				primary = null;
			}
			if (IsAllowed(secondary))
			{
				icons = secondary.Icons;
				text = GetSubIcon(secondary);
			}
			else
			{
				secondary = null;
			}
			if (primary != null || secondary != null)
			{
				m_blockVariantGrid.Add(new MyGuiGridItem(icons, (string)null, (string)null, (object)new MyGuiScreenToolbarConfigBase.GridItemUserData
				{
					Action = delegate
					{
						bool smallExists = false;
						bool largeExists = false;
						MyCubeBlockDefinition[] array = new MyCubeBlockDefinition[2]
						{
							primary,
							secondary
						};
						MyCubeBlockDefinition[] array2 = array;
						foreach (MyCubeBlockDefinition myCubeBlockDefinition in array2)
						{
							if (myCubeBlockDefinition != null)
							{
								if (myCubeBlockDefinition.CubeSize == MyCubeSize.Small)
								{
									smallExists = true;
								}
								else
								{
									largeExists = true;
								}
							}
						}
						UpdateSizeIcons(smallExists, largeExists);
						SetBlockDetail(array);
					},
					ItemData = (() => MyToolbarItemFactory.ObjectBuilderFromDefinition(SelectedDefinition))
				}, enabled: true, 1f)
				{
					SubIcon2 = text,
					Enabled = string.IsNullOrEmpty(text)
				});
			}
			string GetSubIcon(MyCubeBlockDefinition block)
			{
				if (block == null)
				{
					return null;
				}
				if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, block.Id))
				{
					return "Textures\\GUI\\Icons\\HUD 2017\\ProgressionTree.png";
				}
				return MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(primary ?? secondary, Sync.MyId)?.Icon;
			}
		}

		public void ForEachChild(Action<MyGuiControlStackPanel, MyGuiControlBase> action)
		{
			ForEachChildRecursive(this);
			void ForEachChildRecursive(MyGuiControlStackPanel parent)
			{
				foreach (MyGuiControlBase control in parent.GetControls(onlyVisible: false))
				{
					action(parent, control);
					MyGuiControlStackPanel parent2;
					if ((parent2 = (control as MyGuiControlStackPanel)) != null)
					{
						ForEachChildRecursive(parent2);
					}
				}
			}
		}

		private static bool IsAllowed(MyDefinitionBase blockDefinition)
		{
			if (blockDefinition == null)
			{
				return false;
			}
			if (!blockDefinition.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
			{
				return false;
			}
			if (!blockDefinition.AvailableInSurvival && MySession.Static.SurvivalMode)
			{
				return false;
			}
			return true;
		}

		private static MyGuiControlBlockInfo CreateBlockInfoControl()
		{
			MyGuiControlBlockInfo.MyControlBlockInfoStyle style = default(MyGuiControlBlockInfo.MyControlBlockInfoStyle);
			style.BackgroundColormask = new Vector4(142f / (339f * MathF.PI), 46f / 255f, 52f / 255f, 1f);
			style.BlockNameLabelFont = "Blue";
			style.EnableBlockTypeLabel = false;
			style.ComponentsLabelText = MySpaceTexts.HudBlockInfo_Components;
			style.ComponentsLabelFont = "Blue";
			style.InstalledRequiredLabelText = MySpaceTexts.HudBlockInfo_Installed_Required;
			style.InstalledRequiredLabelFont = "Blue";
			style.RequiredLabelText = MyCommonTexts.HudBlockInfo_Required;
			style.IntegrityLabelFont = "White";
			style.IntegrityBackgroundColor = new Vector4(26f / 85f, 116f / 255f, 137f / 255f, 1f);
			style.IntegrityForegroundColor = new Vector4(0.5f, 0.1f, 0.1f, 1f);
			style.IntegrityForegroundColorOverCritical = new Vector4(118f / 255f, 166f / 255f, 64f / 85f, 1f);
			style.LeftColumnBackgroundColor = new Vector4(0f, 0f, 1f, 0f);
			style.TitleBackgroundColor = new Vector4(53f / 255f, 4f / 15f, 76f / 255f, 1f);
			style.TitleSeparatorColor = new Vector4(0.4117647f, 7f / 15f, 44f / 85f, 1f);
			style.ComponentLineMissingFont = "Red";
			style.ComponentLineAllMountedFont = "White";
			style.ComponentLineAllInstalledFont = "Blue";
			style.ComponentLineDefaultFont = "Blue";
			style.ComponentLineDefaultColor = new Vector4(0.6f, 0.6f, 0.6f, 1f);
			style.ShowAvailableComponents = false;
			style.EnableBlockTypePanel = false;
			style.HiddenPCU = false;
			style.HiddenHeader = true;
			return new MyGuiControlBlockInfo(style, progressMode: false)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BlockInfo = new MyHudBlockInfo(),
				BackgroundTexture = null
			};
		}

		public void RegisterAllControls(MyGuiControls controls)
		{
			ForEachChild(delegate(MyGuiControlStackPanel _, MyGuiControlBase x)
			{
				controls.Add(x);
				x.CanHaveFocus = false;
			});
			controls.Add(m_helpText);
			controls.Add(m_componentsInfo);
		}

		protected override void OnVisibleChanged()
		{
			base.OnVisibleChanged();
			if (m_title != null)
			{
				ForEachChild(delegate(MyGuiControlStackPanel _, MyGuiControlBase x)
				{
					x.Visible = base.Visible;
				});
				m_helpText.Visible = base.Visible;
				m_componentsInfo.Visible = base.Visible;
			}
		}

		public MyGuiControlGrid GetGridForDragAndDrop()
		{
			return m_blockVariantGrid;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			MyGuiConstants.TEXTURE_WBORDER_LIST.Draw(positionAbsoluteTopLeft, base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, backgroundTransitionAlpha));
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		public bool IsBuildPlannerGrid(MyGuiControlGrid grid)
		{
			return m_blocksBuildPlanner == grid;
		}

		public void UpdateBuildPlanner()
		{
			m_blocksBuildPlanner.Items.Clear();
			if (MySession.Static != null && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.BuildPlanner != null)
			{
				foreach (MyIdentity.BuildPlanItem item in MySession.Static.LocalCharacter.BuildPlanner)
				{
					MyToolTips myToolTips = new MyToolTips();
					myToolTips.AddToolTip(item.BlockDefinition.DisplayNameText, 0.8f);
					MyGuiGridItem myGuiGridItem = new MyGuiGridItem(item.BlockDefinition.Icons[0], toolTips: myToolTips, userData: item, subicon: (item.BlockDefinition.CubeSize == MyCubeSize.Large) ? MyGuiConstants.TEXTURE_HUD_GRID_LARGE_FIT.Center.Texture : MyGuiConstants.TEXTURE_HUD_GRID_SMALL_FIT.Center.Texture);
					if (item.IsInProgress)
					{
						myGuiGridItem.OverlayPercent = (item.IsInProgress ? 0.5f : 0f);
					}
					foreach (MyIdentity.BuildPlanItem.Component component in item.Components)
					{
						myGuiGridItem.ToolTip.AddToolTip(component.Count + "x " + component.ComponentDefinition.DisplayNameText);
					}
					m_blocksBuildPlanner.Items.Add(myGuiGridItem);
				}
				m_blocksBuildPlanner.Items.Add(new MyGuiGridItem(new string[1]
				{
					"Textures\\GUI\\Controls\\button_increase.dds"
				}, null, MyTexts.Get(MySpaceTexts.TooltipBuildScreen_BuildPlanner).ToString()));
				m_blocksBuildPlanner.ColumnsCount = Math.Min(MySession.Static.LocalCharacter.BuildPlanner.Count + 1, 8);
			}
		}
	}
}
