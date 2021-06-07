using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game.ModAPI;
using VRage.Network;

namespace Sandbox.Game.GameSystems.Chat
{
	[StaticEventOwner]
	public class CommandStop : IMyChatCommand
	{
		protected sealed class Stop_003C_003ESystem_UInt64_0023System_String : ICallSite<IMyEventOwner, ulong, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong requester, in string name, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				Stop(requester, name);
			}
		}

		public string CommandText => "/stop";

		public string HelpText => "ChatCommand_Help_Stop";

		public string HelpSimpleText => "ChatCommand_HelpSimple_Stop";

		public MyPromoteLevel VisibleTo => MyPromoteLevel.Admin;

		public void Handle(string[] args)
		{
			string arg = string.Empty;
			if (args != null && args.Length != 0)
			{
				arg = args[0];
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => Stop, Sync.MyId, arg);
		}

		[Event(null, 479)]
		[Reliable]
		[Server]
		public static void Stop(ulong requester, string name)
		{
			if (!Sync.IsDedicated)
			{
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_StopRequiresDS));
				return;
			}
			if (MySession.Static.GetUserPromoteLevel(MyEventContext.Current.Sender.Value) < MyPromoteLevel.Admin)
			{
				MyEventContext.ValidationFailed();
				return;
			}
			MySandboxGame.Log.WriteLineAndConsole("Executing /stop command");
			MySandboxGame.ExitThreadSafe();
		}

		public static void SaveFinish(ulong requesting)
		{
			long num = MySession.Static.Players.TryGetIdentityId(requesting);
			if (num != 0L)
			{
				MyMultiplayer.Static.SendChatMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_StopExecuting), ChatChannel.GlobalScripted, num, MyTexts.GetString(MySpaceTexts.ChatBotName));
			}
		}
	}
}
