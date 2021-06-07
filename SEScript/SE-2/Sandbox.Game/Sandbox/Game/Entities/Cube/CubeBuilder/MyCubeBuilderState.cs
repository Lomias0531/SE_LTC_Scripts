using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Entities.Cube.CubeBuilder
{
	public class MyCubeBuilderState
	{
		public Dictionary<MyDefinitionId, Quaternion> RotationsByDefinitionHash = new Dictionary<MyDefinitionId, Quaternion>(MyDefinitionId.Comparer);

		public Dictionary<MyDefinitionId, int> LastSelectedStageIndexForGroup = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		public List<MyCubeBlockDefinition> CurrentBlockDefinitionStages = new List<MyCubeBlockDefinition>();

		private MyCubeBlockDefinitionWithVariants m_definitionWithVariants;

		private MyCubeSize m_cubeSizeMode;

		public MyCubeBlockDefinition CurrentBlockDefinition
		{
			get
			{
				return m_definitionWithVariants;
			}
			set
			{
				if (value == null)
				{
					m_definitionWithVariants = null;
					CurrentBlockDefinitionStages.Clear();
					return;
				}
				m_definitionWithVariants = new MyCubeBlockDefinitionWithVariants(value, -1);
				if (!MyFakes.ENABLE_BLOCK_STAGES || CurrentBlockDefinitionStages.Contains(value))
				{
					return;
				}
				CurrentBlockDefinitionStages.Clear();
				if (value.BlockStages == null)
				{
					return;
				}
				CurrentBlockDefinitionStages.Add(value);
				MyDefinitionId[] blockStages = value.BlockStages;
				foreach (MyDefinitionId defId in blockStages)
				{
					MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out MyCubeBlockDefinition blockDefinition);
					if (blockDefinition != null)
					{
						CurrentBlockDefinitionStages.Add(blockDefinition);
					}
				}
			}
		}

		public MyCubeBlockDefinition StartBlockDefinition
		{
			get;
			private set;
		}

		public MyCubeSize CubeSizeMode => m_cubeSizeMode;

		public event Action<MyCubeSize> OnBlockSizeChanged;

		public void SetCurrentBlockForBlockVariantGroup(MyCubeBlockDefinitionGroup blockGroup)
		{
			MyBlockVariantGroup myBlockVariantGroup = blockGroup.AnyPublic?.BlockVariantsGroup;
			if (myBlockVariantGroup != null)
			{
				int value = Array.IndexOf(myBlockVariantGroup.BlockGroups, blockGroup);
				LastSelectedStageIndexForGroup[myBlockVariantGroup.Id] = value;
			}
		}

		public MyCubeBlockDefinitionGroup GetCurrentBlockForBlockVariantGroup(MyBlockVariantGroup variants, bool respectRestrictions = false)
		{
			int num = LastSelectedStageIndexForGroup.GetValueOrDefault(variants.Id, 0);
			if (num >= variants.BlockGroups.Length)
			{
				num = 0;
			}
			int i = 0;
			int num2;
			for (num2 = variants.BlockGroups.Length; i < num2; i++)
			{
				MyCubeBlockDefinition anyPublic = variants.BlockGroups[(num + i) % num2].AnyPublic;
				if (!respectRestrictions || (MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(anyPublic, Sync.MyId) && (MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySessionComponentResearch.Static.CanUse(MySession.Static.LocalCharacter, anyPublic.Id))))
				{
					break;
				}
			}
			return variants.BlockGroups[(num + i) % num2];
		}

		public void UpdateCubeBlockDefinition(MyDefinitionId? id, MatrixD localMatrixAdd)
		{
			if (!id.HasValue)
			{
				return;
			}
			if (CurrentBlockDefinition != null)
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(CurrentBlockDefinition.BlockPairName);
				if (CurrentBlockDefinitionStages.Count > 1)
				{
					definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(CurrentBlockDefinitionStages[0].BlockPairName);
				}
				Quaternion value = Quaternion.CreateFromRotationMatrix(localMatrixAdd);
				if (definitionGroup.Small != null)
				{
					RotationsByDefinitionHash[definitionGroup.Small.Id] = value;
				}
				if (definitionGroup.Large != null)
				{
					RotationsByDefinitionHash[definitionGroup.Large.Id] = value;
				}
			}
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(id.Value);
			if (cubeBlockDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
			{
				CurrentBlockDefinition = cubeBlockDefinition;
			}
			else
			{
				CurrentBlockDefinition = ((cubeBlockDefinition.CubeSize == MyCubeSize.Large) ? MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Small : MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Large);
			}
			StartBlockDefinition = CurrentBlockDefinition;
		}

		public void UpdateCurrentBlockToLastSelectedVariant()
		{
			MyBlockVariantGroup myBlockVariantGroup = CurrentBlockDefinition?.BlockVariantsGroup;
			if (myBlockVariantGroup != null && CurrentBlockDefinitionStages.Count != 0)
			{
				MyCubeBlockDefinition myCubeBlockDefinition = GetCurrentBlockForBlockVariantGroup(myBlockVariantGroup, respectRestrictions: true)[CurrentBlockDefinition.CubeSize];
				if (myCubeBlockDefinition != null && myCubeBlockDefinition.Public)
				{
					CurrentBlockDefinition = myCubeBlockDefinition;
				}
			}
		}

		public void ChooseComplementBlock()
		{
			MyCubeBlockDefinitionWithVariants definitionWithVariants = m_definitionWithVariants;
			if (definitionWithVariants == null)
			{
				return;
			}
			MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionWithVariants.Base.BlockPairName);
			if (definitionWithVariants.Base.CubeSize == MyCubeSize.Small)
			{
				if (definitionGroup.Large != null && (definitionGroup.Large.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
				{
					CurrentBlockDefinition = definitionGroup.Large;
				}
			}
			else if (definitionWithVariants.Base.CubeSize == MyCubeSize.Large && definitionGroup.Small != null && (definitionGroup.Small.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
			{
				CurrentBlockDefinition = definitionGroup.Small;
			}
		}

		public bool HasComplementBlock()
		{
			if (m_definitionWithVariants != null)
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(m_definitionWithVariants.Base.BlockPairName);
				if (m_definitionWithVariants.Base.CubeSize == MyCubeSize.Small)
				{
					if (definitionGroup.Large != null && (definitionGroup.Large.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
					{
						return true;
					}
				}
				else if (m_definitionWithVariants.Base.CubeSize == MyCubeSize.Large && definitionGroup.Small != null && (definitionGroup.Small.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
				{
					return true;
				}
			}
			return false;
		}

		public void SetCubeSize(MyCubeSize newCubeSize)
		{
			m_cubeSizeMode = newCubeSize;
			bool flag = true;
			if (CurrentBlockDefinitionStages.Count != 0)
			{
				MyCubeBlockDefinition myCubeBlockDefinition = CurrentBlockDefinition?.BlockVariantsGroup?.Blocks?.FirstOrDefault((MyCubeBlockDefinition x) => x.CubeSize == m_cubeSizeMode && x.BlockStages != null);
				if (myCubeBlockDefinition != null)
				{
					flag = false;
					CurrentBlockDefinition = myCubeBlockDefinition;
					UpdateCurrentBlockToLastSelectedVariant();
				}
			}
			if (flag)
			{
				UpdateComplementBlock();
			}
			this.OnBlockSizeChanged?.Invoke(newCubeSize);
		}

		internal void UpdateComplementBlock()
		{
			_ = CurrentBlockDefinition;
			_ = StartBlockDefinition;
			if (CurrentBlockDefinition == null || StartBlockDefinition == null)
			{
				return;
			}
			MyCubeBlockDefinition myCubeBlockDefinition = MyDefinitionManager.Static.GetDefinitionGroup(CurrentBlockDefinition.BlockPairName)[m_cubeSizeMode];
			if (myCubeBlockDefinition == null)
			{
				myCubeBlockDefinition = MyDefinitionManager.Static.GetDefinitionGroup(StartBlockDefinition.BlockPairName)[m_cubeSizeMode];
			}
			if (myCubeBlockDefinition == null && CurrentBlockDefinitionStages.Count != 0)
			{
				MyBlockVariantGroup blockVariantsGroup = StartBlockDefinition.BlockVariantsGroup;
				if (blockVariantsGroup != null)
				{
					myCubeBlockDefinition = blockVariantsGroup.Blocks.FirstOrDefault((MyCubeBlockDefinition x) => x.CubeSize == m_cubeSizeMode);
				}
			}
			CurrentBlockDefinition = myCubeBlockDefinition;
		}
	}
}
