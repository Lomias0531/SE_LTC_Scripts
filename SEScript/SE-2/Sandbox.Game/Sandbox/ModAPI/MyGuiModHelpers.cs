using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	public class MyGuiModHelpers : IMyGui
	{
		public string ActiveGamePlayScreen
		{
			get
			{
				MyGuiScreenBase activeGameplayScreen = MyGuiScreenGamePlay.ActiveGameplayScreen;
				if (activeGameplayScreen == null)
				{
					if (MyGuiScreenTerminal.GetCurrentScreen() != MyTerminalPageEnum.None)
					{
						return "MyGuiScreenTerminal";
					}
					return null;
				}
				return activeGameplayScreen.Name;
			}
		}

		public IMyEntity InteractedEntity => MyGuiScreenTerminal.InteractedEntity;

		public MyTerminalPageEnum GetCurrentScreen => MyGuiScreenTerminal.GetCurrentScreen();

		public bool ChatEntryVisible
		{
			get
			{
				if (MyGuiScreenChat.Static == null || MyGuiScreenChat.Static.ChatTextbox == null)
				{
					return false;
				}
				return MyGuiScreenChat.Static.ChatTextbox.Visible;
			}
		}

		public bool IsCursorVisible => MySandboxGame.Static.IsCursorVisible;

		public event Action<object> GuiControlCreated
		{
			add
			{
				MyGuiSandbox.GuiControlCreated = (Action<object>)Delegate.Combine(MyGuiSandbox.GuiControlCreated, GetDelegate(value));
			}
			remove
			{
				MyGuiSandbox.GuiControlCreated = (Action<object>)Delegate.Remove(MyGuiSandbox.GuiControlCreated, GetDelegate(value));
			}
		}

		public event Action<object> GuiControlRemoved
		{
			add
			{
				MyGuiSandbox.GuiControlRemoved = (Action<object>)Delegate.Combine(MyGuiSandbox.GuiControlRemoved, GetDelegate(value));
			}
			remove
			{
				MyGuiSandbox.GuiControlRemoved = (Action<object>)Delegate.Remove(MyGuiSandbox.GuiControlRemoved, GetDelegate(value));
			}
		}

		private Action<object> GetDelegate(Action<object> value)
		{
			return (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), value.Target, value.Method);
		}
	}
}
