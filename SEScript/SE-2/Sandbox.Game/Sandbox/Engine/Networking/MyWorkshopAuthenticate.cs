using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.GameServices;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
	internal static class MyWorkshopAuthenticate
	{
		private static bool m_inProgress;

		private static event Action m_onResume;

		public static void Start(Action onResume)
		{
			MyLog.Default.WriteLine("MyWorkshopAuthenticate.Start " + m_inProgress);
			m_onResume += onResume;
			if (!m_inProgress)
			{
				m_inProgress = true;
				MyStringId orCompute = MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.UGCService_Email), MyGameService.WorkshopService.ServiceName));
				MyGuiScreenDialogText myGuiScreenDialogText = new MyGuiScreenDialogText(string.Empty, orCompute, isTopMostScreen: true);
				myGuiScreenDialogText.OnConfirmed += delegate(string argument)
				{
					MyLog.Default.WriteLine("RequestWorkshopSecurityCode" + argument);
					MyGameService.RequestWorkshopSecurityCode(argument, AskSecurityNumber);
				};
				myGuiScreenDialogText.OnCancelled += delegate
				{
					Stop();
				};
				MyGuiSandbox.AddScreen(myGuiScreenDialogText);
			}
		}

		private static void Stop()
		{
			MyLog.Default.WriteLine("MyWorkshopAuthenticate.Stop");
			Action onResume = MyWorkshopAuthenticate.m_onResume;
			MyWorkshopAuthenticate.m_onResume = null;
			m_inProgress = false;
			onResume.InvokeIfNotNull();
		}

		private static void AskSecurityNumber(MyGameServiceCallResult result)
		{
			MyLog.Default.WriteLine("MyWorkshopAuthenticate.AskSecurityNumber " + result);
			if (result != MyGameServiceCallResult.OK)
			{
				MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(MyGameService.WorkshopService.ServiceName + " error: " + result)));
				Stop();
				return;
			}
			MyGuiScreenDialogText myGuiScreenDialogText = new MyGuiScreenDialogText(string.Empty, MyCommonTexts.ModIO_EnterCode, isTopMostScreen: true);
			myGuiScreenDialogText.OnConfirmed += delegate(string argument)
			{
				MyGameService.AuthenticateWorkshopWithSecurityCode(argument, SecurityNumberResult);
			};
			myGuiScreenDialogText.OnCancelled += delegate
			{
				Stop();
			};
			MyGuiSandbox.AddScreen(myGuiScreenDialogText);
		}

		private static void SecurityNumberResult(MyGameServiceCallResult result)
		{
			MyLog.Default.WriteLine("MyWorkshopAuthenticate.SecurityNumberResult " + result);
			if (result != MyGameServiceCallResult.OK)
			{
				MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(result.ToString())));
			}
			else
			{
				MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("Success")));
			}
			Stop();
		}
	}
}
