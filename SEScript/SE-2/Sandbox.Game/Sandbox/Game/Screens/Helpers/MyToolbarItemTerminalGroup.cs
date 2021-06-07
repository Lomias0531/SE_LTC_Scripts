using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemTerminalGroup))]
	internal class MyToolbarItemTerminalGroup : MyToolbarItemActions, IMyToolbarItemEntity
	{
		private static HashSet<Type> tmpBlockTypes = new HashSet<Type>();

		private static List<MyTerminalBlock> m_tmpBlocks = new List<MyTerminalBlock>();

		private static StringBuilder m_tmpStringBuilder = new StringBuilder();

		private StringBuilder m_groupName;

		private long m_blockEntityId;

		private bool m_wasValid;

		public override ListReader<ITerminalAction> AllActions
		{
			get
			{
				bool genericType;
				return GetActions(GetBlocks(), out genericType);
			}
		}

		private ListReader<MyTerminalBlock> GetBlocks()
		{
			MyEntities.TryGetEntityById(m_blockEntityId, out MyCubeBlock entity);
			if (entity == null)
			{
				return ListReader<MyTerminalBlock>.Empty;
			}
			MyCubeGrid cubeGrid = entity.CubeGrid;
			if (cubeGrid == null || cubeGrid.GridSystems.TerminalSystem == null)
			{
				return ListReader<MyTerminalBlock>.Empty;
			}
			foreach (MyBlockGroup blockGroup in cubeGrid.GridSystems.TerminalSystem.BlockGroups)
			{
				if (blockGroup.Name.Equals(m_groupName))
				{
					return blockGroup.Blocks.ToList();
				}
			}
			return ListReader<MyTerminalBlock>.Empty;
		}

		private ListReader<ITerminalAction> GetActions(ListReader<MyTerminalBlock> blocks, out bool genericType)
		{
			try
			{
				bool flag = true;
				foreach (MyTerminalBlock item in blocks)
				{
					flag = (flag && item is MyFunctionalBlock);
					tmpBlockTypes.Add(item.GetType());
				}
				if (tmpBlockTypes.Count == 1)
				{
					genericType = false;
					return GetValidActions(blocks.ItemAt(0).GetType(), blocks);
				}
				if (tmpBlockTypes.Count == 0 || !flag)
				{
					genericType = true;
					return ListReader<ITerminalAction>.Empty;
				}
				genericType = true;
				Type blockType = FindBaseClass(tmpBlockTypes.ToArray(), typeof(MyFunctionalBlock));
				return GetValidActions(blockType, blocks);
			}
			finally
			{
				tmpBlockTypes.Clear();
			}
		}

		public static Type FindBaseClass(Type[] types, Type baseKnownCommonType)
		{
			Type type = types[0];
			Dictionary<Type, int> dictionary = new Dictionary<Type, int>();
			dictionary.Add(baseKnownCommonType, types.Length);
			for (int i = 0; i < types.Length; i++)
			{
				type = types[i];
				while (type != baseKnownCommonType)
				{
					if (dictionary.ContainsKey(type))
					{
						dictionary[type]++;
					}
					else
					{
						dictionary[type] = 1;
					}
					type = type.BaseType;
				}
			}
			type = types[0];
			while (dictionary[type] != types.Length)
			{
				type = type.BaseType;
			}
			return type;
		}

		private ListReader<ITerminalAction> GetValidActions(Type blockType, ListReader<MyTerminalBlock> blocks)
		{
			UniqueListReader<ITerminalAction> actions = MyTerminalControlFactory.GetActions(blockType);
			List<ITerminalAction> list = new List<ITerminalAction>();
			foreach (ITerminalAction item in actions)
			{
				if (item.IsValidForGroups())
				{
					bool flag = false;
					foreach (MyTerminalBlock item2 in blocks)
					{
						if (item.IsEnabled(item2))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		private ITerminalAction FindAction(ListReader<ITerminalAction> actions, string name)
		{
			foreach (ITerminalAction item in actions)
			{
				if (item.Id == name)
				{
					return item;
				}
			}
			return null;
		}

		private MyTerminalBlock FirstFunctional(ListReader<MyTerminalBlock> blocks, MyEntity owner, long playerID)
		{
			foreach (MyTerminalBlock item in blocks)
			{
				if (item.IsFunctional && (item.HasPlayerAccess(playerID) || item.HasPlayerAccess((owner as MyTerminalBlock).OwnerId)))
				{
					return item;
				}
			}
			return null;
		}

		public override ListReader<ITerminalAction> PossibleActions(MyToolbarType toolbarType)
		{
			return AllActions;
		}

		public override bool Activate()
		{
			ListReader<MyTerminalBlock> blocks = GetBlocks();
			bool genericType;
			ITerminalAction terminalAction = FindAction(GetActions(blocks, out genericType), base.ActionId);
			if (terminalAction == null)
			{
				return false;
			}
			try
			{
				foreach (MyTerminalBlock item in blocks)
				{
					m_tmpBlocks.Add(item);
				}
				foreach (MyTerminalBlock tmpBlock in m_tmpBlocks)
				{
					if (tmpBlock != null && tmpBlock.IsFunctional)
					{
						terminalAction.Apply(tmpBlock);
					}
				}
			}
			finally
			{
				m_tmpBlocks.Clear();
			}
			return true;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			if (type != 0)
			{
				return type != MyToolbarType.Spectator;
			}
			return false;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			ChangeInfo changeInfo = base.Update(owner, playerID);
			ListReader<MyTerminalBlock> blocks = GetBlocks();
			bool genericType;
			ITerminalAction terminalAction = FindAction(GetActions(blocks, out genericType), base.ActionId);
			MyTerminalBlock myTerminalBlock = FirstFunctional(blocks, owner, playerID);
			changeInfo |= SetEnabled(terminalAction != null && myTerminalBlock != null);
			changeInfo |= SetIcons((!genericType) ? blocks.ItemAt(0).BlockDefinition.Icons : new string[1]
			{
				"Textures\\GUI\\Icons\\GroupIcon.dds"
			});
			changeInfo |= SetSubIcon(terminalAction?.Icon);
			if (terminalAction != null && !m_wasValid)
			{
				m_tmpStringBuilder.Clear();
				m_tmpStringBuilder.AppendStringBuilder(m_groupName);
				m_tmpStringBuilder.Append(" - ");
				m_tmpStringBuilder.Append((object)terminalAction.Name);
				changeInfo |= SetDisplayName(m_tmpStringBuilder.ToString());
				m_tmpStringBuilder.Clear();
				m_wasValid = true;
			}
			else if (terminalAction == null)
			{
				m_wasValid = false;
			}
			if (terminalAction != null && blocks.Count > 0)
			{
				m_tmpStringBuilder.Clear();
				terminalAction.WriteValue(myTerminalBlock ?? blocks.ItemAt(0), m_tmpStringBuilder);
				changeInfo |= SetIconText(m_tmpStringBuilder);
				m_tmpStringBuilder.Clear();
			}
			return changeInfo;
		}

		public bool CompareEntityIds(long id)
		{
			return m_blockEntityId == id;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			MyToolbarItemTerminalGroup myToolbarItemTerminalGroup = obj as MyToolbarItemTerminalGroup;
			if (myToolbarItemTerminalGroup != null && m_blockEntityId == myToolbarItemTerminalGroup.m_blockEntityId && m_groupName.Equals(myToolbarItemTerminalGroup.m_groupName))
			{
				return base.ActionId == myToolbarItemTerminalGroup.ActionId;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((m_blockEntityId.GetHashCode() * 397) ^ m_groupName.GetHashCode()) * 397) ^ base.ActionId.GetHashCode();
		}

		public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
		{
			base.WantsToBeActivated = false;
			base.WantsToBeSelected = false;
			base.ActivateOnClick = true;
			MyObjectBuilder_ToolbarItemTerminalGroup myObjectBuilder_ToolbarItemTerminalGroup = (MyObjectBuilder_ToolbarItemTerminalGroup)objBuilder;
			SetDisplayName(myObjectBuilder_ToolbarItemTerminalGroup.GroupName);
			if (myObjectBuilder_ToolbarItemTerminalGroup.BlockEntityId == 0L)
			{
				m_wasValid = false;
				return false;
			}
			m_blockEntityId = myObjectBuilder_ToolbarItemTerminalGroup.BlockEntityId;
			m_groupName = new StringBuilder(myObjectBuilder_ToolbarItemTerminalGroup.GroupName);
			m_wasValid = true;
			SetAction(myObjectBuilder_ToolbarItemTerminalGroup._Action);
			return true;
		}

		public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
		{
			MyObjectBuilder_ToolbarItemTerminalGroup obj = (MyObjectBuilder_ToolbarItemTerminalGroup)MyToolbarItemFactory.CreateObjectBuilder(this);
			obj.GroupName = m_groupName.ToString();
			obj.BlockEntityId = m_blockEntityId;
			obj._Action = base.ActionId;
			return obj;
		}
	}
}
