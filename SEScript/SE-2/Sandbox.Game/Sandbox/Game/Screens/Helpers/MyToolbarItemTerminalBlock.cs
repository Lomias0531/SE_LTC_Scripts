using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemTerminalBlock))]
	internal class MyToolbarItemTerminalBlock : MyToolbarItemActions, IMyToolbarItemEntity
	{
		private long m_blockEntityId;

		private bool m_wasValid;

		private bool m_nameChanged;

		private MyTerminalBlock m_block;

		private List<TerminalActionParameter> m_parameters = new List<TerminalActionParameter>();

		private static List<ITerminalAction> m_tmpEnabledActions = new List<ITerminalAction>();

		private static ListReader<ITerminalAction> m_tmpEnabledActionsReader = new ListReader<ITerminalAction>(m_tmpEnabledActions);

		private static StringBuilder m_tmpStringBuilder = new StringBuilder();

		private MyTerminalBlock m_registeredBlock;

		public override ListReader<ITerminalAction> AllActions => GetActions(null);

		public List<TerminalActionParameter> Parameters => m_parameters;

		private bool TryGetBlock()
		{
			bool num = MyEntities.TryGetEntityById(m_blockEntityId, out m_block);
			if (num)
			{
				RegisterEvents();
			}
			return num;
		}

		public override ListReader<ITerminalAction> PossibleActions(MyToolbarType type)
		{
			return GetActions(type);
		}

		private ListReader<ITerminalAction> GetActions(MyToolbarType? type)
		{
			if (m_block == null)
			{
				return ListReader<ITerminalAction>.Empty;
			}
			m_tmpEnabledActions.Clear();
			foreach (ITerminalAction action in MyTerminalControls.Static.GetActions(m_block))
			{
				if (action.IsEnabled(m_block) && (!type.HasValue || action.IsValidForToolbarType(type.Value)))
				{
					m_tmpEnabledActions.Add(action);
				}
			}
			return m_tmpEnabledActionsReader;
		}

		public override bool Activate()
		{
			ITerminalAction currentAction = GetCurrentAction();
			if (m_block != null && currentAction != null)
			{
				currentAction.Apply(m_block, Parameters);
				return true;
			}
			return false;
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
			if (m_block == null)
			{
				TryGetBlock();
			}
			ITerminalAction currentAction = GetCurrentAction();
			bool flag = m_block != null && currentAction != null && MyCubeGridGroups.Static.Physical.HasSameGroup((owner as MyTerminalBlock).CubeGrid, m_block.CubeGrid);
			changeInfo |= SetEnabled(flag && m_block.IsFunctional && (m_block.HasPlayerAccess(playerID) || m_block.HasPlayerAccess((owner as MyTerminalBlock).OwnerId)));
			if (m_block != null)
			{
				changeInfo |= SetIcons(m_block.BlockDefinition.Icons);
			}
			if (flag)
			{
				if (!m_wasValid || base.ActionChanged)
				{
					changeInfo |= SetIcons(m_block.BlockDefinition.Icons);
					changeInfo |= SetSubIcon(currentAction.Icon);
					changeInfo |= UpdateCustomName(currentAction);
				}
				else if (m_nameChanged)
				{
					changeInfo |= UpdateCustomName(currentAction);
				}
				m_tmpStringBuilder.Clear();
				currentAction.WriteValue(m_block, m_tmpStringBuilder);
				changeInfo |= SetIconText(m_tmpStringBuilder);
				m_tmpStringBuilder.Clear();
			}
			m_wasValid = flag;
			m_nameChanged = false;
			base.ActionChanged = false;
			return changeInfo;
		}

		public string GetBlockName()
		{
			if (m_block == null)
			{
				return string.Empty;
			}
			return m_block.CustomName.ToString();
		}

		public string GetActionName()
		{
			ITerminalAction currentAction = GetCurrentAction();
			if (currentAction == null)
			{
				return string.Empty;
			}
			return currentAction.Name.ToString();
		}

		private ChangeInfo UpdateCustomName(ITerminalAction action)
		{
			try
			{
				m_tmpStringBuilder.Clear();
				m_tmpStringBuilder.AppendStringBuilder(m_block.CustomName);
				m_tmpStringBuilder.Append(" - ");
				m_tmpStringBuilder.AppendStringBuilder(action.Name);
				return SetDisplayName(m_tmpStringBuilder.ToString());
			}
			finally
			{
				m_tmpStringBuilder.Clear();
			}
		}

		public bool CompareEntityIds(long id)
		{
			return id == m_blockEntityId;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			MyToolbarItemTerminalBlock myToolbarItemTerminalBlock = obj as MyToolbarItemTerminalBlock;
			if (myToolbarItemTerminalBlock == null || m_blockEntityId != myToolbarItemTerminalBlock.m_blockEntityId || !(base.ActionId == myToolbarItemTerminalBlock.ActionId))
			{
				return false;
			}
			if (m_parameters.Count != myToolbarItemTerminalBlock.Parameters.Count)
			{
				return false;
			}
			for (int i = 0; i < m_parameters.Count; i++)
			{
				TerminalActionParameter terminalActionParameter = m_parameters[i];
				TerminalActionParameter terminalActionParameter2 = myToolbarItemTerminalBlock.Parameters[i];
				if (terminalActionParameter.TypeCode != terminalActionParameter2.TypeCode)
				{
					return false;
				}
				if (!object.Equals(terminalActionParameter.Value, terminalActionParameter2.Value))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			return (m_blockEntityId.GetHashCode() * 397) ^ base.ActionId.GetHashCode();
		}

		public override bool Init(MyObjectBuilder_ToolbarItem objectBuilder)
		{
			base.WantsToBeActivated = false;
			base.WantsToBeSelected = false;
			base.ActivateOnClick = true;
			m_block = null;
			MyObjectBuilder_ToolbarItemTerminalBlock myObjectBuilder_ToolbarItemTerminalBlock = (MyObjectBuilder_ToolbarItemTerminalBlock)objectBuilder;
			m_blockEntityId = myObjectBuilder_ToolbarItemTerminalBlock.BlockEntityId;
			if (m_blockEntityId == 0L)
			{
				m_wasValid = false;
				return false;
			}
			TryGetBlock();
			SetAction(myObjectBuilder_ToolbarItemTerminalBlock._Action);
			if (myObjectBuilder_ToolbarItemTerminalBlock.Parameters != null && myObjectBuilder_ToolbarItemTerminalBlock.Parameters.Count > 0)
			{
				m_parameters.Clear();
				foreach (MyObjectBuilder_ToolbarItemActionParameter parameter in myObjectBuilder_ToolbarItemTerminalBlock.Parameters)
				{
					m_parameters.Add(TerminalActionParameter.Deserialize(parameter.Value, parameter.TypeCode));
				}
			}
			return true;
		}

		private void RegisterEvents()
		{
			UnregisterEvents();
			m_block.CustomNameChanged += block_CustomNameChanged;
			m_block.OnClose += block_OnClose;
			m_registeredBlock = m_block;
		}

		private void UnregisterEvents()
		{
			if (m_registeredBlock != null)
			{
				m_registeredBlock.CustomNameChanged -= block_CustomNameChanged;
				m_registeredBlock.OnClose -= block_OnClose;
				m_registeredBlock = null;
			}
		}

		private void block_CustomNameChanged(MyTerminalBlock obj)
		{
			m_nameChanged = true;
		}

		private void block_OnClose(MyEntity obj)
		{
			UnregisterEvents();
			m_block = null;
		}

		public override void OnRemovedFromToolbar(MyToolbar toolbar)
		{
			if (m_block != null)
			{
				UnregisterEvents();
			}
			base.OnRemovedFromToolbar(toolbar);
		}

		public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
		{
			MyObjectBuilder_ToolbarItemTerminalBlock myObjectBuilder_ToolbarItemTerminalBlock = (MyObjectBuilder_ToolbarItemTerminalBlock)MyToolbarItemFactory.CreateObjectBuilder(this);
			myObjectBuilder_ToolbarItemTerminalBlock.BlockEntityId = m_blockEntityId;
			myObjectBuilder_ToolbarItemTerminalBlock._Action = base.ActionId;
			myObjectBuilder_ToolbarItemTerminalBlock.Parameters.Clear();
			foreach (TerminalActionParameter parameter in m_parameters)
			{
				myObjectBuilder_ToolbarItemTerminalBlock.Parameters.Add(parameter.GetObjectBuilder());
			}
			return myObjectBuilder_ToolbarItemTerminalBlock;
		}
	}
}
