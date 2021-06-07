using Havok;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Crash tests")]
	internal class MyGuiScreenDebugCrashTests : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugCrashTests()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugCrashTests";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCaption("Crash tests", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddButton(new StringBuilder("Exception in update thread."), UpdateThreadException);
			AddButton(new StringBuilder("Exception in render thread."), RenderThreadException);
			AddButton(new StringBuilder("Exception in worker thread."), WorkerThreadException);
			AddButton(new StringBuilder("Main thread invoked exception."), MainThreadInvokedException);
			AddButton(new StringBuilder("Update thread out of memory."), OutOfMemoryUpdateException);
			AddButton(new StringBuilder("Worker thread out of memory."), OutOfMemoryWorkerException);
			AddButton(new StringBuilder("Havok access violation."), HavokAccessViolationException);
			AddButton(new StringBuilder("Divide by zero."), DivideByZero);
			AddSubcaption("Log");
			AddButton(new StringBuilder("Send log"), SendLog);
			m_currentPosition.Y += 0.01f;
		}

		private void SendLog(MyGuiControlButton button)
		{
			MyLog.Default.WriteLine("Send long requested");
			MyLog.Default.Flush();
			MySandboxGame.PerformNotInteractiveReport.InvokeIfNotNull();
			button.Text = "Sent";
		}

		private void UpdateThreadException(MyGuiControlButton sender)
		{
			throw new InvalidOperationException("Forced exception");
		}

		private void RenderThreadException(MyGuiControlButton sender)
		{
			MyRenderProxy.DebugCrashRenderThread();
		}

		private void WorkerThreadException(MyGuiControlButton sender)
		{
			ThreadPool.QueueUserWorkItem(WorkerThreadCrasher);
		}

		private void MainThreadInvokedException(MyGuiControlButton sender)
		{
			MySandboxGame.Static.Invoke(MainThreadCrasher, "DebugCrashTest");
		}

		private void OutOfMemoryUpdateException(MyGuiControlButton sender)
		{
			Allocate();
		}

		private void OutOfMemoryWorkerException(MyGuiControlButton sender)
		{
			ThreadPool.QueueUserWorkItem(Allocate);
		}

		private void HavokAccessViolationException(MyGuiControlButton sender)
		{
			ThreadPool.QueueUserWorkItem(HavokAccessViolation);
		}

		private void HavokAccessViolation(object state = null)
		{
			Console.WriteLine(new HkRigidBodyCinfo().LinearVelocity);
		}

		private void Allocate(object state = null)
		{
			List<byte[]> list = new List<byte[]>();
			for (int i = 0; i < 10000000; i++)
			{
				byte[] array = new byte[1024000];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = (byte)(j ^ list.Count);
				}
				list.Add(array);
			}
			Console.WriteLine(list.Count);
		}

		private void MainThreadCrasher()
		{
			throw new InvalidOperationException("Forced exception");
		}

		private void WorkerThreadCrasher(object state)
		{
			Thread.Sleep(2000);
			throw new InvalidOperationException("Forced exception");
		}

		private void DivideByZero(MyGuiControlButton sender)
		{
			int num = 7;
			Console.WriteLine(14 / (14 - 2 * num));
		}
	}
}
