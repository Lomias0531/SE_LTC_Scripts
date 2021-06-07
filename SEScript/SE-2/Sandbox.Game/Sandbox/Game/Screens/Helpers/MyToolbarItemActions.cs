using Sandbox.Game.Gui;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	internal abstract class MyToolbarItemActions : MyToolbarItem
	{
		private string m_actionId;

		protected bool ActionChanged
		{
			get;
			set;
		}

		public string ActionId
		{
			get
			{
				return m_actionId;
			}
			set
			{
				if ((m_actionId == null || !m_actionId.Equals(value)) && (m_actionId != null || value != null))
				{
					m_actionId = value;
					ActionChanged = true;
				}
			}
		}

		public abstract ListReader<ITerminalAction> AllActions
		{
			get;
		}

		public abstract ListReader<ITerminalAction> PossibleActions(MyToolbarType toolbarType);

		public ITerminalAction GetCurrentAction()
		{
			return GetActionOrNull(ActionId);
		}

		public ITerminalAction GetActionOrNull(string id)
		{
			foreach (ITerminalAction allAction in AllActions)
			{
				if (allAction.Id == id)
				{
					return allAction;
				}
			}
			return null;
		}

		protected void SetAction(string action)
		{
			ActionId = action;
			if (ActionId == null)
			{
				ListReader<ITerminalAction> allActions = AllActions;
				if (allActions.Count > 0)
				{
					ActionId = allActions.ItemAt(0).Id;
				}
			}
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			if (ActionId == null)
			{
				ListReader<ITerminalAction> allActions = AllActions;
				if (allActions.Count > 0)
				{
					ActionId = allActions.ItemAt(0).Id;
				}
			}
			return ChangeInfo.None;
		}
	}
}
