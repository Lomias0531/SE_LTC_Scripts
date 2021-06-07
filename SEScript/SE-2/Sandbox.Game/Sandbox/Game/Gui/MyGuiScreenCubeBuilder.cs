using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenCubeBuilder : MyGuiScreenToolbarConfigBase
	{
		private MyGuiGridItem m_lastGridBlocksMouseOverItem;

		public MyGuiScreenCubeBuilder(int scrollOffset = 0, MyCubeBlock owner = null, int? gamepadSlot = null)
			: this(scrollOffset, owner, null, hideOtherPages: false, gamepadSlot)
		{
		}

		public MyGuiScreenCubeBuilder(int scrollOffset, MyCubeBlock owner, string selectedPage, bool hideOtherPages, int? gamepadSlot = null)
			: base(MyHud.HudDefinition.Toolbar, scrollOffset, owner, gamepadSlot)
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenCubeBuilder.ctor START");
			MyGuiScreenToolbarConfigBase.Static = this;
			m_scrollOffset = (float)scrollOffset / 6.5f;
			m_size = new Vector2(1f, 1f);
			m_canShareInput = true;
			m_drawEvenWithoutFocus = true;
			base.EnabledBackgroundFade = true;
			m_screenOwner = owner;
			RecreateControls(contructor: true);
			if (!string.IsNullOrWhiteSpace(selectedPage))
			{
				foreach (MyGuiControlTabPage page in m_tabControl.Pages)
				{
					if (page.Name == selectedPage)
					{
						m_tabControl.SelectedPage = page.PageKey;
						break;
					}
				}
			}
			if (hideOtherPages)
			{
				foreach (MyGuiControlTabPage page2 in m_tabControl.Pages)
				{
					bool isTabVisible = page2.Enabled = (page2.PageKey == m_tabControl.SelectedPage);
					page2.IsTabVisible = isTabVisible;
				}
			}
			MySandboxGame.Log.WriteLine("MyGuiScreenCubeBuilder.ctor END");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenCubeBuilder";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			m_gridBlocks.MouseOverIndexChanged += OnGridMouseOverIndexChanged;
			m_gridBlocks.ItemSelected += OnSelectedItemChanged;
			m_researchGraph.MouseOverItemChanged += m_researchGraph_MouseOverItemChanged;
			m_researchGraph.SelectedItemChanged += m_researchGraph_SelectedItemChanged;
			m_blockGroupInfo = (MyGuiControlBlockGroupInfo)Controls.GetControlByName("BlockInfoPanel");
			m_blockGroupInfo.RegisterAllControls(Controls);
			m_blockGroupInfo.ColorMask = m_gridBlocks.ColorMask;
			m_blockGroupInfo.GetGridForDragAndDrop().ItemDragged += base.grid_OnDrag;
			m_blockGroupInfo.UpdateArrange();
			base.CloseButtonStyle = MyGuiControlButtonStyleEnum.CloseBackground;
			m_blockGroupInfo.SetBlockModeEnabled(!m_shipMode);
			foreach (MyGuiControlBase control in m_blockGroupInfo.GetControls())
			{
				control.Visible = !m_shipMode;
				MyGuiControlStackPanel myGuiControlStackPanel;
				if ((myGuiControlStackPanel = (control as MyGuiControlStackPanel)) != null)
				{
					foreach (MyGuiControlBase control2 in myGuiControlStackPanel.GetControls())
					{
						control2.Visible = !m_shipMode;
					}
				}
			}
		}

		private void m_researchGraph_SelectedItemChanged(object sender, EventArgs e)
		{
			if (m_researchGraph.Visible)
			{
				MyGuiGridItem selectedItem = m_researchGraph.SelectedItem;
				ShowItem(selectedItem);
			}
		}

		private void OnSelectedItemChanged(MyGuiControlGrid arg1, MyGuiControlGrid.EventArgs arg2)
		{
			OnGridMouseOverIndexChanged(arg1, arg2);
		}

		private void m_researchGraph_MouseOverItemChanged(object sender, EventArgs e)
		{
			if (m_researchGraph.Visible)
			{
				MyGuiGridItem gridItem = m_researchGraph.MouseOverItem ?? m_researchGraph.SelectedItem;
				ShowItem(gridItem);
			}
		}

		private void OnGridMouseOverIndexChanged(MyGuiControlGrid myGuiControlGrid, MyGuiControlGrid.EventArgs eventArgs)
		{
			if (m_gridBlocks.Visible && !m_dragAndDrop.IsActive())
			{
				MyGuiGridItem gridItem = m_gridBlocks.MouseOverItem ?? m_gridBlocks.SelectedItem;
				ShowItem(gridItem);
			}
		}

		private void ShowItem(MyGuiGridItem gridItem)
		{
			if (m_gamepadSlot.HasValue || gridItem == null || m_lastGridBlocksMouseOverItem == gridItem)
			{
				return;
			}
			MyObjectBuilder_ToolbarItemDefinition myObjectBuilder_ToolbarItemDefinition = (gridItem.UserData as GridItemUserData)?.ItemData() as MyObjectBuilder_ToolbarItemDefinition;
			if (myObjectBuilder_ToolbarItemDefinition == null || !MyDefinitionManager.Static.TryGetDefinition(myObjectBuilder_ToolbarItemDefinition.DefinitionId, out MyDefinitionBase definition))
			{
				return;
			}
			m_blockGroupInfo.Visible = true;
			m_lastGridBlocksMouseOverItem = gridItem;
			MyDefinitionBase myDefinitionBase = definition;
			if (myDefinitionBase == null)
			{
				return;
			}
			MyCubeBlockDefinition myCubeBlockDefinition;
			if ((myCubeBlockDefinition = (myDefinitionBase as MyCubeBlockDefinition)) == null)
			{
				MyPhysicalItemDefinition myPhysicalItemDefinition;
				if ((myPhysicalItemDefinition = (myDefinitionBase as MyPhysicalItemDefinition)) == null)
				{
					MyAnimationDefinition myAnimationDefinition;
					if ((myAnimationDefinition = (myDefinitionBase as MyAnimationDefinition)) == null)
					{
						MyEmoteDefinition myEmoteDefinition;
						if ((myEmoteDefinition = (myDefinitionBase as MyEmoteDefinition)) == null)
						{
							MyVoxelHandDefinition myVoxelHandDefinition;
							if ((myVoxelHandDefinition = (myDefinitionBase as MyVoxelHandDefinition)) != null)
							{
								MyVoxelHandDefinition generalDefinition = myVoxelHandDefinition;
								m_blockGroupInfo.SetGeneralDefinition(generalDefinition);
							}
						}
						else
						{
							MyEmoteDefinition generalDefinition2 = myEmoteDefinition;
							m_blockGroupInfo.SetGeneralDefinition(generalDefinition2);
						}
					}
					else
					{
						MyAnimationDefinition generalDefinition3 = myAnimationDefinition;
						m_blockGroupInfo.SetGeneralDefinition(generalDefinition3);
					}
				}
				else
				{
					MyPhysicalItemDefinition generalDefinition4 = myPhysicalItemDefinition;
					m_blockGroupInfo.SetGeneralDefinition(generalDefinition4);
				}
			}
			else
			{
				MyCubeBlockDefinition myCubeBlockDefinition2 = myCubeBlockDefinition;
				m_blockGroupInfo.SetBlockGroup(MyDefinitionManager.Static.GetDefinitionGroup(myCubeBlockDefinition2.BlockPairName));
			}
		}
	}
}
