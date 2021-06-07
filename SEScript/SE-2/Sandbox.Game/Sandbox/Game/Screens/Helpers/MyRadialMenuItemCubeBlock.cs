using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
	[MyRadialMenuItemDescriptor(typeof(MyObjectBuilder_RadialMenuItemCubeBlock))]
	internal class MyRadialMenuItemCubeBlock : MyRadialMenuItem
	{
		[Flags]
		public enum EnabledState
		{
			Enabled = 0x0,
			DLC = 0x1,
			Research = 0x2,
			Other = 0x4
		}

		public override bool CanBeActivated
		{
			get
			{
				MyDLCs.MyDLC missingDLC;
				return IsBlockGroupEnabled(MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(BlockVariantGroup), out missingDLC) == EnabledState.Enabled;
			}
		}

		public MyBlockVariantGroup BlockVariantGroup
		{
			get;
			private set;
		}

		public override void Init(MyObjectBuilder_RadialMenuItem builder)
		{
			MyObjectBuilder_RadialMenuItemCubeBlock myObjectBuilder_RadialMenuItemCubeBlock = (MyObjectBuilder_RadialMenuItemCubeBlock)builder;
			MyDefinitionManager.Static.GetBlockVariantGroupDefinitions().TryGetValue(myObjectBuilder_RadialMenuItemCubeBlock.Id.SubtypeId, out MyBlockVariantGroup value);
			BlockVariantGroup = value;
			Icon = value.Icons[0];
			Label = MyTexts.GetString(value.DisplayNameEnum.Value);
		}

		public override bool Enabled()
		{
			MyCubeBlockDefinition[] blocks = BlockVariantGroup.Blocks;
			for (int i = 0; i < blocks.Length; i++)
			{
				if (IsBlockEnabled(blocks[i], out MyDLCs.MyDLC _) == EnabledState.Enabled)
				{
					return true;
				}
			}
			return false;
		}

		public override void Activate(params object[] parameters)
		{
			MyDefinitionId weaponDefinition = new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer));
			MySession.Static.LocalCharacter?.SwitchToWeapon(weaponDefinition);
			MyCubeSize myCubeSize = MyCubeSize.Large;
			object obj;
			if (parameters.Length != 0 && (obj = parameters[0]) is MyCubeSize)
			{
				MyCubeSize myCubeSize2 = (MyCubeSize)obj;
				myCubeSize = myCubeSize2;
			}
			MyCubeBlockDefinitionGroup currentBlockForBlockVariantGroup = MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(BlockVariantGroup);
			MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(myCubeSize);
			MyCubeBlockDefinition myCubeBlockDefinition = currentBlockForBlockVariantGroup[myCubeSize] ?? currentBlockForBlockVariantGroup.AnyPublic;
			MyCubeBlockDefinition myCubeBlockDefinition2 = myCubeBlockDefinition.BlockVariantsGroup?.PrimaryGUIBlock;
			if (myCubeBlockDefinition2 != null)
			{
				myCubeBlockDefinition = myCubeBlockDefinition2;
			}
			MyCubeBuilder.Static.Activate(myCubeBlockDefinition.Id);
			MyCubeBuilder.Static.SetToolType(MyCubeBuilderToolType.BuildTool);
			if (MySessionComponentVoxelHand.Static.Enabled)
			{
				MySessionComponentVoxelHand.Static.Enabled = false;
			}
		}

		public static EnabledState IsBlockGroupEnabled(MyCubeBlockDefinitionGroup blocks, out MyDLCs.MyDLC missingDLC)
		{
			MyDLCs.MyDLC missingDLC2;
			EnabledState enabledState = IsBlockEnabled(blocks.Small, out missingDLC2);
			MyDLCs.MyDLC missingDLC3;
			EnabledState enabledState2 = IsBlockEnabled(blocks.Large, out missingDLC3);
			if (enabledState > enabledState2)
			{
				missingDLC = missingDLC2;
				return enabledState;
			}
			missingDLC = missingDLC3;
			return enabledState2;
		}

		public static EnabledState IsBlockEnabled(MyCubeBlockDefinition block, out MyDLCs.MyDLC missingDLC)
		{
			missingDLC = null;
			EnabledState enabledState = EnabledState.Enabled;
			if (block != null)
			{
				if (!block.Public)
				{
					enabledState |= EnabledState.Other;
				}
				MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
				missingDLC = component.GetFirstMissingDefinitionDLC(block, Sync.MyId);
				if (!MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, block.Id) && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
				{
					enabledState |= EnabledState.Research;
				}
				if (missingDLC != null)
				{
					enabledState |= EnabledState.DLC;
				}
			}
			return enabledState;
		}
	}
}
