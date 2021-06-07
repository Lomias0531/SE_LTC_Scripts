using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BlockVariantGroup), null)]
	public class MyBlockVariantGroup : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyBlockVariantGroup_003C_003EActor : IActivator, IActivator<MyBlockVariantGroup>
		{
			private sealed override object CreateInstance()
			{
				return new MyBlockVariantGroup();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBlockVariantGroup CreateInstance()
			{
				return new MyBlockVariantGroup();
			}

			MyBlockVariantGroup IActivator<MyBlockVariantGroup>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyCubeBlockDefinition[] Blocks;

		private SerializableDefinitionId[] m_blockIdsToResolve;

		public MyCubeBlockDefinitionGroup[] BlockGroups
		{
			get;
			private set;
		}

		public MyCubeBlockDefinition PrimaryGUIBlock
		{
			get;
			private set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_BlockVariantGroup myObjectBuilder_BlockVariantGroup = (MyObjectBuilder_BlockVariantGroup)builder;
			m_blockIdsToResolve = myObjectBuilder_BlockVariantGroup.Blocks;
		}

		public void ResolveBlocks()
		{
			Blocks = new MyCubeBlockDefinition[m_blockIdsToResolve.Length];
			for (int i = 0; i < m_blockIdsToResolve.Length; i++)
			{
				SerializableDefinitionId v = m_blockIdsToResolve[i];
				Blocks[i] = (MyCubeBlockDefinition)MyDefinitionManager.Static.GetDefinition(v);
			}
			m_blockIdsToResolve = null;
		}

		public new void Postprocess()
		{
			HashSet<MyCubeBlockDefinitionGroup> hashSet = new HashSet<MyCubeBlockDefinitionGroup>();
			MyCubeBlockDefinition[] blocks = Blocks;
			foreach (MyCubeBlockDefinition myCubeBlockDefinition in blocks)
			{
				hashSet.Add(MyDefinitionManager.Static.GetDefinitionGroup(myCubeBlockDefinition.BlockPairName));
			}
			BlockGroups = hashSet.ToArray();
			CreateBlockStages();
			PrimaryGUIBlock = Blocks[0];
			blocks = Blocks;
			foreach (MyCubeBlockDefinition myCubeBlockDefinition2 in blocks)
			{
				myCubeBlockDefinition2.GuiVisible = (PrimaryGUIBlock == myCubeBlockDefinition2);
			}
			if (Icons.IsNullOrEmpty())
			{
				Icons = PrimaryGUIBlock.Icons;
			}
			if (!DisplayNameEnum.HasValue)
			{
				if (!string.IsNullOrEmpty(DisplayNameString))
				{
					DisplayNameEnum = MyStringId.GetOrCompute(DisplayNameString);
				}
				else if (!string.IsNullOrEmpty(DisplayNameText))
				{
					DisplayNameEnum = MyStringId.GetOrCompute(DisplayNameText);
				}
				else if (PrimaryGUIBlock.DisplayNameEnum.HasValue)
				{
					DisplayNameEnum = PrimaryGUIBlock.DisplayNameEnum.Value;
				}
				else
				{
					DisplayNameEnum = MyStringId.GetOrCompute(PrimaryGUIBlock.DisplayNameText);
				}
			}
		}

		private void CreateBlockStages()
		{
			//Could not decode local function '[SpecializedMethod Sandbox.Definitions.MyBlockVariantGroup.<CreateBlockStages>g__MoveFront|13_2[0200148E Sandbox.Definitions.MyCubeBlockDefinition](array:Sandbox.Definitions.MyCubeBlockDefinition[], element:Sandbox.Definitions.MyCubeBlockDefinition, offset:System.Int32):020000FC System.Boolean]'
			MyCubeSize[] blockSizes = MyEnum<MyCubeSize>.Values;
			MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup = null;
			MyCubeBlockDefinition[] blocks = Blocks;
			foreach (MyCubeBlockDefinition myCubeBlockDefinition in blocks)
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(myCubeBlockDefinition.BlockPairName);
				if (HasBlocksForAllSizes(definitionGroup))
				{
					myCubeBlockDefinitionGroup = definitionGroup;
					break;
				}
			}
			MyCubeSize[] array;
			if (myCubeBlockDefinitionGroup != null)
			{
				int j;
				for (j = 0; j < Blocks.Length; j++)
				{
					MyCubeBlockDefinition myCubeBlockDefinition2 = Blocks[j];
					bool flag = false;
					array = blockSizes;
					foreach (MyCubeSize size2 in array)
					{
						if (myCubeBlockDefinitionGroup[size2] == myCubeBlockDefinition2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
				array = blockSizes;
				foreach (MyCubeSize size3 in array)
				{
					MyCubeBlockDefinition myCubeBlockDefinition3 = myCubeBlockDefinitionGroup[size3];
					ConstructFullVariantsFor(myCubeBlockDefinition3);
					if (_003CCreateBlockStages_003Eg__MoveFront_007C13_2(Blocks, myCubeBlockDefinition3, j))
					{
						j++;
					}
				}
				_003CCreateBlockStages_003Eg__MoveFront_007C13_2(BlockGroups, myCubeBlockDefinitionGroup, 0);
				return;
			}
			array = blockSizes;
			foreach (MyCubeSize size in array)
			{
				MyCubeBlockDefinition myCubeBlockDefinition4 = Blocks.FirstOrDefault((MyCubeBlockDefinition x) => x.CubeSize == size && x.Public);
				if (myCubeBlockDefinition4 != null)
				{
					ConstructFullVariantsFor(myCubeBlockDefinition4);
				}
			}
			void ConstructFullVariantsFor(MyCubeBlockDefinition block)
			{
				block.BlockStages = (from x in Blocks
					where x != block && x.CubeSize == block.CubeSize
					select x.Id).ToArray();
			}
			bool HasBlocksForAllSizes(MyCubeBlockDefinitionGroup blockPair)
			{
				MyCubeSize[] array2 = blockSizes;
				foreach (MyCubeSize size4 in array2)
				{
					if (blockPair[size4] == null)
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}
