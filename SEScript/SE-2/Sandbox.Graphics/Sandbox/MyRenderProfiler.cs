using System;
using VRage.Input;
using VRage.Network;
using VRage.Profiler;
using VRage.Replication;
using VRageRender;

namespace Sandbox
{
	[StaticEventOwner]
	public class MyRenderProfiler
	{
		protected sealed class OnCommandReceived_003C_003EVRage_Profiler_RenderProfilerCommand_0023System_Int32 : ICallSite<IMyEventOwner, RenderProfilerCommand, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in RenderProfilerCommand cmd, in int payload, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCommandReceived(cmd, payload);
			}
		}

		public static Action<RenderProfilerCommand, int> ServerInvoke;

		private static bool m_enabled;

		private static bool MultiplayerActive => MyMultiplayerMinimalBase.Instance != null;

		private static bool IsServer
		{
			get
			{
				if (MultiplayerActive)
				{
					return MyMultiplayerMinimalBase.Instance.IsServer;
				}
				return true;
			}
		}

		public static void ToggleProfiler(string threadName)
		{
			m_enabled = !m_enabled;
			MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.ToggleEnabled, 0, threadName);
		}

		public static void EnableAutoscale(string threadName)
		{
			MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.EnableAutoScale, 0, threadName);
		}

		public static void HandleInput()
		{
			bool flag = false;
			RenderProfilerCommand? renderProfilerCommand = null;
			int num = 0;
			if (MyInput.Static.IsAnyAltKeyPressed())
			{
				if (m_enabled)
				{
					for (int i = 0; i <= 9; i++)
					{
						MyKeys key = (MyKeys)(96 + i);
						if (MyInput.Static.IsNewKeyPressed(key))
						{
							num = i;
							renderProfilerCommand = RenderProfilerCommand.JumpToLevel;
						}
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsKeyPress(MyKeys.Space))
					{
						num += 10;
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsKeyPress(MyKeys.Space))
					{
						num += 20;
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed())
					{
						if (MyInput.Static.IsNewKeyPressed(MyKeys.Add))
						{
							renderProfilerCommand = RenderProfilerCommand.IncreaseLocalArea;
						}
						if (MyInput.Static.IsNewKeyPressed(MyKeys.Subtract))
						{
							renderProfilerCommand = RenderProfilerCommand.DecreaseLocalArea;
						}
					}
					else if (MyInput.Static.IsAnyShiftKeyPressed())
					{
						if (MyInput.Static.IsKeyPress(MyKeys.Add))
						{
							renderProfilerCommand = RenderProfilerCommand.NextThread;
						}
						if (MyInput.Static.IsKeyPress(MyKeys.Subtract))
						{
							renderProfilerCommand = RenderProfilerCommand.PreviousThread;
						}
					}
					else
					{
						if (MyInput.Static.IsNewKeyPressed(MyKeys.Add))
						{
							renderProfilerCommand = RenderProfilerCommand.NextThread;
						}
						if (MyInput.Static.IsNewKeyPressed(MyKeys.Subtract))
						{
							renderProfilerCommand = RenderProfilerCommand.PreviousThread;
						}
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.Enter))
					{
						renderProfilerCommand = RenderProfilerCommand.Pause;
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed())
					{
						if (MyInput.Static.IsNewKeyPressed(MyKeys.PageDown))
						{
							num = 1;
							renderProfilerCommand = RenderProfilerCommand.PreviousFrame;
						}
						if (MyInput.Static.IsNewKeyPressed(MyKeys.PageUp))
						{
							num = 1;
							renderProfilerCommand = RenderProfilerCommand.NextFrame;
						}
					}
					else if (MyInput.Static.IsAnyShiftKeyPressed())
					{
						if (MyInput.Static.IsKeyPress(MyKeys.PageDown))
						{
							num = 10;
							renderProfilerCommand = RenderProfilerCommand.PreviousFrame;
						}
						if (MyInput.Static.IsKeyPress(MyKeys.PageUp))
						{
							num = 10;
							renderProfilerCommand = RenderProfilerCommand.NextFrame;
						}
					}
					else
					{
						if (MyInput.Static.IsKeyPress(MyKeys.PageDown))
						{
							num = 1;
							renderProfilerCommand = RenderProfilerCommand.PreviousFrame;
						}
						if (MyInput.Static.IsKeyPress(MyKeys.PageUp))
						{
							num = 1;
							renderProfilerCommand = RenderProfilerCommand.NextFrame;
						}
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.Insert))
					{
						renderProfilerCommand = ((MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsAnyShiftKeyPressed()) ? new RenderProfilerCommand?(RenderProfilerCommand.ToggleAutoScale) : (MyInput.Static.IsAnyCtrlKeyPressed() ? new RenderProfilerCommand?(RenderProfilerCommand.Reset) : new RenderProfilerCommand?(RenderProfilerCommand.ChangeSortingOrder)));
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.Home))
					{
						renderProfilerCommand = RenderProfilerCommand.IncreaseRange;
					}
					else if (MyInput.Static.IsNewKeyPressed(MyKeys.End))
					{
						renderProfilerCommand = RenderProfilerCommand.DecreaseRange;
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed())
					{
						if (MyInput.Static.IsNewKeyPressed(MyKeys.Multiply))
						{
							renderProfilerCommand = RenderProfilerCommand.IncreaseLevel;
						}
						if (MyInput.Static.IsNewKeyPressed(MyKeys.Divide))
						{
							renderProfilerCommand = RenderProfilerCommand.DecreaseLevel;
						}
					}
					else
					{
						if (MyInput.Static.IsKeyPress(MyKeys.Multiply))
						{
							renderProfilerCommand = RenderProfilerCommand.IncreaseLevel;
						}
						if (MyInput.Static.IsKeyPress(MyKeys.Divide))
						{
							renderProfilerCommand = RenderProfilerCommand.DecreaseLevel;
						}
					}
					if (MyInput.Static.IsAnyShiftKeyPressed())
					{
						if (MyInput.Static.IsNewKeyPressed(MyKeys.Divide))
						{
							renderProfilerCommand = RenderProfilerCommand.CopyPathToClipboard;
						}
						else if (MyInput.Static.IsNewKeyPressed(MyKeys.Multiply))
						{
							renderProfilerCommand = RenderProfilerCommand.TryGoToPathInClipboard;
						}
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Home))
					{
						renderProfilerCommand = RenderProfilerCommand.JumpToRoot;
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.End))
					{
						renderProfilerCommand = RenderProfilerCommand.DisableFrameSelection;
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.S))
					{
						renderProfilerCommand = RenderProfilerCommand.GetFomServer;
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.C))
					{
						renderProfilerCommand = RenderProfilerCommand.GetFromClient;
					}
					for (byte b = 0; b < 9; b = (byte)(b + 1))
					{
						if (MyInput.Static.IsNewKeyPressed((MyKeys)(49 + b)))
						{
							renderProfilerCommand = (MyInput.Static.IsAnyCtrlKeyPressed() ? new RenderProfilerCommand?(RenderProfilerCommand.SaveToFile) : new RenderProfilerCommand?(RenderProfilerCommand.LoadFromFile));
							if (MyInput.Static.IsAnyShiftKeyPressed())
							{
								flag = true;
							}
							num = b + 1;
							break;
						}
					}
					if (MyInput.Static.IsKeyPress(MyKeys.Z))
					{
						if (renderProfilerCommand == RenderProfilerCommand.JumpToLevel)
						{
							renderProfilerCommand = RenderProfilerCommand.SwapBlockOptimized;
						}
						else if (MyInput.Static.IsNewKeyPressed(MyKeys.Decimal))
						{
							renderProfilerCommand = RenderProfilerCommand.ResetAllOptimizations;
						}
						else if (MyInput.Static.IsNewKeyPressed(MyKeys.Enter))
						{
							renderProfilerCommand = RenderProfilerCommand.ToggleOptimizationsEnabled;
						}
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.Q))
					{
						renderProfilerCommand = (MyInput.Static.IsAnyShiftKeyPressed() ? new RenderProfilerCommand?(RenderProfilerCommand.SwitchBlockRender) : new RenderProfilerCommand?(RenderProfilerCommand.SwitchGraphContent));
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.E))
					{
						renderProfilerCommand = RenderProfilerCommand.SwitchShallowProfile;
						if (MyInput.Static.IsKeyPress(MyKeys.Shift))
						{
							flag = true;
						}
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.A))
					{
						renderProfilerCommand = RenderProfilerCommand.SwitchAverageTimes;
						if (MyInput.Static.IsKeyPress(MyKeys.Shift))
						{
							flag = true;
						}
					}
				}
				if (!renderProfilerCommand.HasValue && (MyInput.Static.IsNewKeyPressed(MyKeys.Decimal) || (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad0) && !m_enabled)))
				{
					m_enabled = !m_enabled;
					renderProfilerCommand = RenderProfilerCommand.ToggleEnabled;
				}
			}
			if (renderProfilerCommand.HasValue)
			{
				if (flag && ServerInvoke != null)
				{
					ServerInvoke(renderProfilerCommand.Value, num);
				}
				else
				{
					MyRenderProxy.RenderProfilerInput(renderProfilerCommand.Value, num, null);
				}
			}
		}

		[Event(null, 311)]
		[Reliable]
		[Server]
		public static void OnCommandReceived(RenderProfilerCommand cmd, int payload)
		{
			VRage.Profiler.MyRenderProfiler.HandleInput(cmd, payload, null);
		}
	}
}
