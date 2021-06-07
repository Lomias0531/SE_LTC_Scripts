using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	internal class MyRadialMenuComponent : MySessionComponentBase
	{
		private MyRadialMenuSection m_lastUsedBlocks;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_lastUsedBlocks = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute("RadialMenuGroupTitle_LastUsed"));
			MyDefinitionManager.Static.GetRadialMenuDefinition("Toolbar").Sections.Insert(0, m_lastUsedBlocks);
		}

		public void InitDefaultLastUsed(MyObjectBuilder_Toolbar toolbar)
		{
			foreach (MyObjectBuilder_Toolbar.Slot slot in toolbar.Slots)
			{
				if (m_lastUsedBlocks.Items.Count >= 8)
				{
					break;
				}
				MyObjectBuilder_ToolbarItemCubeBlock myObjectBuilder_ToolbarItemCubeBlock;
				if ((myObjectBuilder_ToolbarItemCubeBlock = (slot.Data as MyObjectBuilder_ToolbarItemCubeBlock)) != null)
				{
					MyBlockVariantGroup blockVariantsGroup = MyDefinitionManager.Static.GetCubeBlockDefinition(myObjectBuilder_ToolbarItemCubeBlock.DefinitionId).BlockVariantsGroup;
					MyObjectBuilder_RadialMenuItemCubeBlock builder = new MyObjectBuilder_RadialMenuItemCubeBlock
					{
						Id = blockVariantsGroup.Id
					};
					MyRadialMenuItemCubeBlock myRadialMenuItemCubeBlock = new MyRadialMenuItemCubeBlock();
					myRadialMenuItemCubeBlock.Init(builder);
					m_lastUsedBlocks.Items.Add(myRadialMenuItemCubeBlock);
				}
			}
		}

		public void PushLastUsedBlock(MyRadialMenuItemCubeBlock block)
		{
			if (block != null && !m_lastUsedBlocks.Items.Contains(block))
			{
				m_lastUsedBlocks.Items.Insert(0, block);
				if (m_lastUsedBlocks.Items.Count > 8)
				{
					m_lastUsedBlocks.Items.RemoveAt(8);
				}
			}
		}

		public void ShowSystemRadialMenu(MyStringId context, Func<bool> inputCallback)
		{
			string subtype = (context == MySpaceBindingCreator.AX_ACTIONS) ? "SystemShip" : ((!(context == MySpaceBindingCreator.AX_BUILD) && !(context == MySpaceBindingCreator.AX_SYMMETRY)) ? "SystemDefault" : "SystemBuild");
			MyGuiSandbox.AddScreen(new MyGuiControlRadialMenuSystem(MyDefinitionManager.Static.GetRadialMenuDefinition(subtype), MyControlsSpace.SYSTEM_RADIAL_MENU, inputCallback));
		}
	}
}
