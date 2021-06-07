using Sandbox.Engine.Platform;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Text;
using VRage;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.Helpers
{
	public static class MyAsyncSaving
	{
		private static Action m_callbackOnFinished;

		private static int m_inProgressCount;

		private static bool m_screenshotTaken;

		public static bool InProgress => m_inProgressCount > 0;

		private static void PushInProgress()
		{
			m_inProgressCount++;
		}

		private static void PopInProgress()
		{
			m_inProgressCount--;
		}

		public static void Start(Action callbackOnFinished = null, string customName = null, bool wait = false)
		{
			PushInProgress();
			m_callbackOnFinished = callbackOnFinished;
			OnSnapshotDone(MySession.Static.Save(out MySessionSnapshot snapshot, customName), snapshot, wait);
		}

		private static void OnSnapshotDone(bool snapshotSuccess, MySessionSnapshot snapshot, bool wait)
		{
			if (snapshotSuccess)
			{
				Func<bool> screenshotTaken = null;
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					string thumbPath = MySession.Static.ThumbPath;
					try
					{
						if (File.Exists(thumbPath))
						{
							File.Delete(thumbPath);
						}
						m_screenshotTaken = false;
						MySandboxGame.Static.OnScreenshotTaken += OnScreenshotTaken;
						MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), thumbPath, debug: false, ignoreSprites: true, showNotification: false);
						screenshotTaken = (() => m_screenshotTaken);
					}
					catch (Exception ex)
					{
						MySandboxGame.Log.WriteLine("Could not take session thumb screenshot. Exception:");
						MySandboxGame.Log.WriteLine(ex);
					}
				}
				if (wait)
				{
					snapshot.Save(null);
					SaveFinished(snapshot);
				}
				else
				{
					snapshot.SaveParallel(screenshotTaken, delegate
					{
						SaveFinished(snapshot);
					});
				}
			}
			else
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.WorldNotSaved), MySession.Static.Name), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
				PopInProgress();
			}
			if (m_callbackOnFinished != null)
			{
				m_callbackOnFinished();
			}
			m_callbackOnFinished = null;
		}

		private static void OnScreenshotTaken(object sender, EventArgs e)
		{
			MySandboxGame.Static.OnScreenshotTaken -= OnScreenshotTaken;
			m_screenshotTaken = true;
		}

		private static void SaveFinished(MySessionSnapshot snapshot)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null)
			{
				if (snapshot.SavingSuccess)
				{
					MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.WorldSaved);
					myHudNotification.SetTextFormatArguments(MySession.Static.Name);
					MyHud.Notifications.Add(myHudNotification);
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.WorldNotSaved), MySession.Static.Name), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
			}
			PopInProgress();
		}
	}
}
