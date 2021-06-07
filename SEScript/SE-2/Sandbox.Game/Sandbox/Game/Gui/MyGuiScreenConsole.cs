using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Linq;
using System.Text;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
	internal class MyGuiScreenConsole : MyGuiScreenBase
	{
		private enum MyConsoleKeys
		{
			UP,
			DOWN,
			ENTER
		}

		private class MyConsoleKeyTimerController
		{
			public MyKeys Key;

			public int LastKeyPressTime;

			public MyConsoleKeyTimerController(MyKeys key)
			{
				Key = key;
				LastKeyPressTime = -60000;
			}
		}

		private static MyGuiScreenConsole m_instance;

		private MyGuiControlTextbox m_commandLine;

		private MyGuiControlMultilineText m_displayScreen;

		private MyGuiControlContextMenu m_autoComplete;

		private StringBuilder m_commandText = new StringBuilder();

		private string BufferText = "";

		private float m_screenScale;

		private Vector2 m_margin;

		private static MyConsoleKeyTimerController[] m_keys;

		public override string GetFriendlyName()
		{
			return "Console Screen";
		}

		public MyGuiScreenConsole()
		{
			m_backgroundTexture = MyGuiConstants.TEXTURE_MESSAGEBOX_BACKGROUND_INFO.Texture;
			m_backgroundColor = new Vector4(0f, 0f, 0f, 0.75f);
			m_position = new Vector2(0.5f, 0.25f);
			m_screenScale = MyGuiManager.GetHudSize().X / MyGuiManager.GetHudSize().Y / 1.33333337f;
			m_size = new Vector2(m_screenScale, 0.5f);
			m_margin = new Vector2(0.06f, 0.04f);
			m_keys = new MyConsoleKeyTimerController[3];
			m_keys[0] = new MyConsoleKeyTimerController(MyKeys.Up);
			m_keys[1] = new MyConsoleKeyTimerController(MyKeys.Down);
			m_keys[2] = new MyConsoleKeyTimerController(MyKeys.Enter);
		}

		public override void RecreateControls(bool constructor)
		{
			m_screenScale = MyGuiManager.GetHudSize().X / MyGuiManager.GetHudSize().Y / 1.33333337f;
			m_size = new Vector2(m_screenScale, 0.5f);
			base.RecreateControls(constructor);
			Vector4 value = new Vector4(1f, 1f, 0f, 1f);
			float textScale = 1f;
			m_commandLine = new MyGuiControlTextbox(new Vector2(0f, 0.25f), null, 512, value);
			m_commandLine.Position -= new Vector2(0f, m_commandLine.Size.Y + m_margin.Y / 2f);
			m_commandLine.Size = new Vector2(m_screenScale, m_commandLine.Size.Y) - 2f * m_margin;
			m_commandLine.ColorMask = new Vector4(0f, 0f, 0f, 0.5f);
			m_commandLine.VisualStyle = MyGuiControlTextboxStyleEnum.Debug;
			m_commandLine.TextChanged += commandLine_TextChanged;
			m_commandLine.Name = "CommandLine";
			m_autoComplete = new MyGuiControlContextMenu();
			m_autoComplete.ItemClicked += autoComplete_ItemClicked;
			m_autoComplete.Deactivate();
			m_autoComplete.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_autoComplete.ColorMask = new Vector4(0f, 0f, 0f, 0.5f);
			m_autoComplete.AllowKeyboardNavigation = true;
			m_autoComplete.Name = "AutoComplete";
			m_displayScreen = new MyGuiControlMultilineText(new Vector2(-0.5f * m_screenScale, -0.25f) + m_margin, new Vector2(m_screenScale, 0.5f - m_commandLine.Size.Y) - 2f * m_margin, null, "Debug", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, selectable: true);
			m_displayScreen.TextColor = Color.Yellow;
			m_displayScreen.TextScale = textScale;
			m_displayScreen.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_displayScreen.Text = MyConsole.DisplayScreen;
			m_displayScreen.ColorMask = new Vector4(0f, 0f, 0f, 0.5f);
			m_displayScreen.Name = "DisplayScreen";
			Controls.Add(m_displayScreen);
			Controls.Add(m_commandLine);
			Controls.Add(m_autoComplete);
		}

		public static void Show()
		{
			m_instance = new MyGuiScreenConsole();
			m_instance.RecreateControls(constructor: true);
			MyGuiSandbox.AddScreen(m_instance);
		}

		protected override void OnClosed()
		{
			base.OnClosed();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			bool flag = false;
			if (base.FocusedControl == m_commandLine && MyInput.Static.IsKeyPress(MyKeys.Up) && !m_autoComplete.Visible)
			{
				if (IsEnoughDelay(MyConsoleKeys.UP, 100) && !m_autoComplete.Visible)
				{
					UpdateLastKeyPressTimes(MyConsoleKeys.UP);
					if (MyConsole.GetLine() == "")
					{
						BufferText = m_commandLine.Text;
					}
					MyConsole.PreviousLine();
					if (MyConsole.GetLine() == "")
					{
						m_commandLine.Text = BufferText;
					}
					else
					{
						m_commandLine.Text = MyConsole.GetLine();
					}
					m_commandLine.MoveCarriageToEnd();
				}
				flag = true;
			}
			if (base.FocusedControl == m_commandLine && MyInput.Static.IsKeyPress(MyKeys.Down) && !m_autoComplete.Visible)
			{
				if (IsEnoughDelay(MyConsoleKeys.DOWN, 100) && !m_autoComplete.Visible)
				{
					UpdateLastKeyPressTimes(MyConsoleKeys.DOWN);
					if (MyConsole.GetLine() == "")
					{
						BufferText = m_commandLine.Text;
					}
					MyConsole.NextLine();
					if (MyConsole.GetLine() == "")
					{
						m_commandLine.Text = BufferText;
					}
					else
					{
						m_commandLine.Text = MyConsole.GetLine();
					}
					m_commandLine.MoveCarriageToEnd();
				}
				flag = true;
			}
			if (base.FocusedControl == m_commandLine && MyInput.Static.IsKeyPress(MyKeys.Enter) && !m_commandLine.Text.Equals("") && !m_autoComplete.Visible && IsEnoughDelay(MyConsoleKeys.ENTER, 100))
			{
				UpdateLastKeyPressTimes(MyConsoleKeys.ENTER);
				if (!m_autoComplete.Visible)
				{
					BufferText = "";
					MyConsole.ParseCommand(m_commandLine.Text);
					MyConsole.NextLine();
					m_displayScreen.Text = MyConsole.DisplayScreen;
					m_displayScreen.ScrollbarOffsetV = 1f;
					m_commandLine.Text = "";
					flag = true;
				}
			}
			if (!flag)
			{
				base.HandleInput(receivedFocusInThisUpdate);
			}
		}

		public void commandLine_TextChanged(MyGuiControlTextbox sender)
		{
			string text = sender.Text;
			MyCommand command;
			if (text.Length == 0 || !sender.Text.ElementAt(sender.Text.Length - 1).Equals('.'))
			{
				if (m_autoComplete.Enabled)
				{
					m_autoComplete.Enabled = false;
					m_autoComplete.Deactivate();
				}
			}
			else if (MyConsole.TryGetCommand(text.Substring(0, text.Length - 1), out command))
			{
				m_autoComplete.CreateNewContextMenu();
				m_autoComplete.Position = new Vector2((1f - m_screenScale) / 2f + m_margin.X, m_size.Value.Y - 2f * m_margin.Y) + MyGuiManager.MeasureString("Debug", new StringBuilder(m_commandLine.Text), m_commandLine.TextScaleWithLanguage);
				foreach (string method in command.Methods)
				{
					m_autoComplete.AddItem(new StringBuilder(method).Append(" ").Append((object)command.GetHint(method)), "", "", method);
				}
				m_autoComplete.Enabled = true;
				m_autoComplete.Activate(autoPositionOnMouseTip: false);
			}
		}

		public void autoComplete_ItemClicked(MyGuiControlContextMenu sender, MyGuiControlContextMenu.EventArgs args)
		{
			m_commandLine.Text += (string)m_autoComplete.Items[args.ItemIndex].UserData;
			m_commandLine.MoveCarriageToEnd();
			base.FocusedControl = m_commandLine;
		}

		private bool IsEnoughDelay(MyConsoleKeys key, int forcedDelay)
		{
			MyConsoleKeyTimerController myConsoleKeyTimerController = m_keys[(int)key];
			if (myConsoleKeyTimerController == null)
			{
				return true;
			}
			return MyGuiManager.TotalTimeInMilliseconds - myConsoleKeyTimerController.LastKeyPressTime > forcedDelay;
		}

		private void UpdateLastKeyPressTimes(MyConsoleKeys key)
		{
			MyConsoleKeyTimerController myConsoleKeyTimerController = m_keys[(int)key];
			if (myConsoleKeyTimerController != null)
			{
				myConsoleKeyTimerController.LastKeyPressTime = MyGuiManager.TotalTimeInMilliseconds;
			}
		}
	}
}
