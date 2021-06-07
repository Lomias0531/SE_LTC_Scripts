using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenSaveAs : MyGuiScreenBase
	{
		private class SaveResult : IMyAsyncResult
		{
			public bool IsCompleted => Task.IsComplete;

			public Task Task
			{
				get;
				private set;
			}

			public SaveResult(string saveDir, string sessionPath, MyWorldInfo copyFrom)
			{
				Task = Parallel.Start(delegate
				{
					SaveAsync(saveDir, sessionPath, copyFrom);
				});
			}

			private void SaveAsync(string newSaveName, string sessionPath, MyWorldInfo copyFrom)
			{
				string sessionSavesPath = MyLocalCache.GetSessionSavesPath(newSaveName, contentFolder: false, createIfNotExists: false);
				while (Directory.Exists(sessionSavesPath))
				{
					sessionSavesPath = MyLocalCache.GetSessionSavesPath(newSaveName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
				}
				Directory.CreateDirectory(sessionSavesPath);
				MyUtils.CopyDirectory(sessionPath, sessionSavesPath);
				ulong sizeInBytes;
				MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(sessionSavesPath, out sizeInBytes);
				myObjectBuilder_Checkpoint.SessionName = copyFrom.SessionName;
				myObjectBuilder_Checkpoint.WorkshopId = null;
				MyLocalCache.SaveCheckpoint(myObjectBuilder_Checkpoint, sessionSavesPath);
			}
		}

		private MyGuiControlTextbox m_nameTextbox;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private MyWorldInfo m_copyFrom;

		private List<string> m_existingSessionNames;

		private string m_sessionPath;

		private bool m_fromMainMenu;

		public event Action SaveAsConfirm;

		public MyGuiScreenSaveAs(MyWorldInfo copyFrom, string sessionPath, List<string> existingSessionNames)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(87f / 175f, 147f / 524f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			AddCaption(MyCommonTexts.ScreenCaptionSaveAs, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList2);
			float y = -0.027f;
			m_nameTextbox = new MyGuiControlTextbox(new Vector2(0f, y), copyFrom.SessionName, 75);
			m_nameTextbox.Size = new Vector2(0.385f, 1f);
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkButtonClick);
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelButtonClick);
			Vector2 value = new Vector2(0.002f, m_size.Value.Y / 2f - 0.071f);
			Vector2 value2 = new Vector2(0.018f, 0f);
			m_okButton.Position = value - value2;
			m_cancelButton.Position = value + value2;
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
			m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			m_nameTextbox.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsName), 5, 128));
			Controls.Add(m_nameTextbox);
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			m_nameTextbox.MoveCarriageToEnd();
			m_copyFrom = copyFrom;
			m_sessionPath = sessionPath;
			m_existingSessionNames = existingSessionNames;
			base.CloseButtonEnabled = true;
			OnEnterCallback = OnEnterPressed;
		}

		public MyGuiScreenSaveAs(string sessionName)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(87f / 175f, 147f / 524f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			AddCaption(MyCommonTexts.ScreenCaptionSaveAs, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.122f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList2);
			m_existingSessionNames = null;
			m_fromMainMenu = true;
			float y = -0.027f;
			m_nameTextbox = new MyGuiControlTextbox(new Vector2(0f, y), sessionName, 75);
			m_nameTextbox.Size = new Vector2(0.385f, 1f);
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkButtonClick);
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelButtonClick);
			Vector2 value = new Vector2(0.002f, m_size.Value.Y / 2f - 0.045f);
			Vector2 value2 = new Vector2(0.018f, 0f);
			m_okButton.Position = value - value2;
			m_cancelButton.Position = value + value2;
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
			m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			m_nameTextbox.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsName), 5, 128));
			Controls.Add(m_nameTextbox);
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			m_nameTextbox.MoveCarriageToEnd();
			base.CloseButtonEnabled = true;
			OnEnterCallback = OnEnterPressed;
		}

		private void OnEnterPressed()
		{
			TrySaveAs();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenSaveAs";
		}

		private void OnCancelButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		private void OnOkButtonClick(MyGuiControlButton sender)
		{
			TrySaveAs();
		}

		private bool TrySaveAs()
		{
			MyStringId? myStringId = null;
			if (m_nameTextbox.Text.ToString().Replace(':', '-').IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
			{
				myStringId = MyCommonTexts.ErrorNameInvalid;
			}
			else if (m_nameTextbox.Text.Length < 5)
			{
				myStringId = MyCommonTexts.ErrorNameTooShort;
			}
			else if (m_nameTextbox.Text.Length > 128)
			{
				myStringId = MyCommonTexts.ErrorNameTooLong;
			}
			if (m_existingSessionNames != null)
			{
				foreach (string existingSessionName in m_existingSessionNames)
				{
					if (existingSessionName == m_nameTextbox.Text)
					{
						myStringId = MyCommonTexts.ErrorNameAlreadyExists;
					}
				}
			}
			if (myStringId.HasValue)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(myStringId.Value), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox.SkipTransition = true;
				myGuiScreenMessageBox.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
				return false;
			}
			if (m_fromMainMenu)
			{
				string text = MyUtils.StripInvalidChars(m_nameTextbox.Text);
				if (string.IsNullOrWhiteSpace(text))
				{
					text = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
				}
				MyAsyncSaving.Start(null, text);
				MySession.Static.Name = m_nameTextbox.Text;
				CloseScreen();
				return true;
			}
			m_copyFrom.SessionName = m_nameTextbox.Text;
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.SavingPleaseWait, null, () => new SaveResult(MyUtils.StripInvalidChars(m_nameTextbox.Text), m_sessionPath, m_copyFrom), delegate(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
			{
				screen.CloseScreen();
				CloseScreen();
				this.SaveAsConfirm?.Invoke();
			}));
			return true;
		}
	}
}
