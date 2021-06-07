using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemCubeBlock))]
	public class MyToolbarItemCubeBlock : MyToolbarItemDefinition
	{
		private MyFixedPoint m_lastAmount = 0;

		public MyFixedPoint Amount => m_lastAmount;

		public MyCubeBlockDefinition BlockDefinition => (MyCubeBlockDefinition)Definition;

		public override bool Equals(object obj)
		{
			MyToolbarItemCubeBlock myToolbarItemCubeBlock;
			if ((myToolbarItemCubeBlock = (obj as MyToolbarItemCubeBlock)) != null)
			{
				MyCubeBlockDefinition blockDefinition = BlockDefinition;
				MyCubeBlockDefinition blockDefinition2 = myToolbarItemCubeBlock.BlockDefinition;
				if (blockDefinition == blockDefinition2)
				{
					return true;
				}
				if (blockDefinition.BlockPairName == blockDefinition2.BlockPairName)
				{
					return true;
				}
				if (blockDefinition.BlockVariantsGroup != null && blockDefinition.BlockVariantsGroup == blockDefinition2.BlockVariantsGroup && blockDefinition.BlockStages != null && blockDefinition2.BlockStages != null)
				{
					return true;
				}
			}
			return base.Equals(obj);
		}

		public override bool Activate()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			MyDefinitionId myDefinitionId = new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer));
			if (localCharacter != null)
			{
				if (!MySessionComponentSafeZones.IsActionAllowed(localCharacter, MySafeZoneAction.Building, 0L, 0uL))
				{
					return false;
				}
				if (localCharacter.CurrentWeapon == null || !(localCharacter.CurrentWeapon.DefinitionId == myDefinitionId))
				{
					localCharacter.SwitchToWeapon(myDefinitionId);
				}
				MyCubeBuilder.Static.Activate(((MyCubeBlockDefinition)Definition).Id);
			}
			else if (MyBlockBuilderBase.SpectatorIsBuilding)
			{
				MyCubeBuilder.Static.Activate(((MyCubeBlockDefinition)Definition).Id);
			}
			return true;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			if (type != 0 && type != MyToolbarType.Spectator)
			{
				return type == MyToolbarType.BuildCockpit;
			}
			return true;
		}

		public override bool Init(MyObjectBuilder_ToolbarItem data)
		{
			bool flag = base.Init(data);
			base.ActivateOnClick = false;
			if (flag && MyHud.HudDefinition != null && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(BlockDefinition.BlockPairName);
				MyCubeSize[] values = MyEnum<MyCubeSize>.Values;
				foreach (MyCubeSize size in values)
				{
					MyCubeBlockDefinition myCubeBlockDefinition = definitionGroup[size];
					if (myCubeBlockDefinition != null && !myCubeBlockDefinition.BlockStages.IsNullOrEmpty())
					{
						MyObjectBuilder_GuiTexture texture = MyGuiTextures.Static.GetTexture(MyHud.HudDefinition.Toolbar.ItemStyle.VariantTexture);
						SetSubIcon(texture.Path);
					}
				}
			}
			return flag;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			ChangeInfo changeInfo = ChangeInfo.None;
			bool flag = true;
			if (MyCubeBuilder.Static == null)
			{
				return changeInfo;
			}
			MyCubeBlockDefinition myCubeBlockDefinition = MyCubeBuilder.Static.IsActivated ? MyCubeBuilder.Static.ToolbarBlockDefinition : null;
			MyCubeBlockDefinition myCubeBlockDefinition2 = Definition as MyCubeBlockDefinition;
			if (MyCubeBuilder.Static.IsActivated && myCubeBlockDefinition != null)
			{
				if (myCubeBlockDefinition.BlockPairName == myCubeBlockDefinition2.BlockPairName)
				{
					base.WantsToBeSelected = true;
				}
				else
				{
					base.WantsToBeSelected = false;
				}
			}
			else
			{
				base.WantsToBeSelected = false;
			}
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID && myCubeBlockDefinition2.CubeSize == MyCubeSize.Small && localCharacter != null)
			{
				MyFixedPoint myFixedPoint = localCharacter.GetInventory()?.GetItemAmount(Definition.Id) ?? ((MyFixedPoint)0);
				if (m_lastAmount != myFixedPoint)
				{
					m_lastAmount = myFixedPoint;
					changeInfo |= ChangeInfo.IconText;
				}
				if (MySession.Static.SurvivalMode)
				{
					flag &= (m_lastAmount > 0);
				}
				else
				{
					changeInfo |= ChangeInfo.IconText;
				}
			}
			MySessionComponentResearch @static;
			if (MySession.Static.ResearchEnabled && !MySession.Static.CreativeToolsEnabled(Sync.MyId) && (@static = MySessionComponentResearch.Static) != null)
			{
				bool flag2 = false;
				if (BlockDefinition.BlockVariantsGroup != null)
				{
					MyCubeBlockDefinition[] blocks = BlockDefinition.BlockVariantsGroup.Blocks;
					foreach (MyCubeBlockDefinition myCubeBlockDefinition3 in blocks)
					{
						if (@static.CanUse(localCharacter, myCubeBlockDefinition3.Id))
						{
							flag2 = true;
							break;
						}
					}
				}
				else
				{
					flag2 = @static.CanUse(localCharacter, BlockDefinition.Id);
				}
				flag = (flag && flag2);
			}
			if (base.Enabled != flag)
			{
				changeInfo |= SetEnabled(flag);
			}
			return changeInfo;
		}

		public override void FillGridItem(MyGuiGridItem gridItem)
		{
			if (MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID)
			{
				if (m_lastAmount > 0)
				{
					gridItem.AddText($"{m_lastAmount}x", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
				}
				else
				{
					gridItem.ClearText(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
				}
			}
		}
	}
}
