using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Groups;

namespace Sandbox.ModAPI
{
	public class MyTerminalControlFactoryHelper : IMyTerminalActionsHelper
	{
		private static MyTerminalControlFactoryHelper m_instance;

		private List<Sandbox.Game.Gui.ITerminalAction> m_actionList = new List<Sandbox.Game.Gui.ITerminalAction>();

		private List<ITerminalProperty> m_valueControls = new List<ITerminalProperty>();

		public static MyTerminalControlFactoryHelper Static
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyTerminalControlFactoryHelper();
				}
				return m_instance;
			}
		}

		void IMyTerminalActionsHelper.GetActions(Type blockType, List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect)
		{
			if (typeof(MyTerminalBlock).IsAssignableFrom(blockType))
			{
				MyTerminalControlFactory.GetActions(blockType, m_actionList);
				foreach (Sandbox.Game.Gui.ITerminalAction action in m_actionList)
				{
					if ((collect == null || collect(action)) && action.IsValidForToolbarType(MyToolbarType.ButtonPanel))
					{
						resultList.Add(action);
					}
				}
				m_actionList.Clear();
			}
		}

		void IMyTerminalActionsHelper.SearchActionsOfName(string name, Type blockType, List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null)
		{
			if (typeof(MyTerminalBlock).IsAssignableFrom(blockType))
			{
				MyTerminalControlFactory.GetActions(blockType, m_actionList);
				foreach (Sandbox.Game.Gui.ITerminalAction action in m_actionList)
				{
					if ((collect == null || collect(action)) && action.Id.ToString().Contains(name) && action.IsValidForToolbarType(MyToolbarType.ButtonPanel))
					{
						resultList.Add(action);
					}
				}
				m_actionList.Clear();
			}
		}

		Sandbox.ModAPI.Interfaces.ITerminalAction IMyTerminalActionsHelper.GetActionWithName(string name, Type blockType)
		{
			if (!typeof(MyTerminalBlock).IsAssignableFrom(blockType))
			{
				return null;
			}
			MyTerminalControlFactory.GetActions(blockType, m_actionList);
			foreach (Sandbox.Game.Gui.ITerminalAction action in m_actionList)
			{
				if (action.Id.ToString() == name && action.IsValidForToolbarType(MyToolbarType.ButtonPanel))
				{
					m_actionList.Clear();
					return action;
				}
			}
			m_actionList.Clear();
			return null;
		}

		public ITerminalProperty GetProperty(string id, Type blockType)
		{
			if (!typeof(MyTerminalBlock).IsAssignableFrom(blockType))
			{
				return null;
			}
			MyTerminalControlFactory.GetValueControls(blockType, m_valueControls);
			foreach (ITerminalProperty valueControl in m_valueControls)
			{
				if (valueControl.Id == id)
				{
					m_valueControls.Clear();
					return valueControl;
				}
			}
			m_valueControls.Clear();
			return null;
		}

		public void GetProperties(Type blockType, List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
		{
			if (typeof(MyTerminalBlock).IsAssignableFrom(blockType))
			{
				MyTerminalControlFactory.GetValueControls(blockType, m_valueControls);
				foreach (ITerminalProperty valueControl in m_valueControls)
				{
					if (collect == null || collect(valueControl))
					{
						resultList.Add(valueControl);
					}
				}
				m_valueControls.Clear();
			}
		}

		IMyGridTerminalSystem IMyTerminalActionsHelper.GetTerminalSystemForGrid(IMyCubeGrid grid)
		{
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid as MyCubeGrid);
			if (group != null && group.GroupData != null)
			{
				return group.GroupData.TerminalSystem;
			}
			return null;
		}
	}
}
