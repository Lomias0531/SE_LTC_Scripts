using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game.ModAPI;
using VRage.Network;

namespace Sandbox.Game.GameSystems.Chat
{
	[StaticEventOwner]
	public class CommandSave : IMyChatCommand
	{
		protected sealed class Save_003C_003ESystem_UInt64_0023System_String : ICallSite<IMyEventOwner, ulong, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong requester, in string name, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				Save(requester, name);
			}
		}

		public string CommandText => "/save";

		public string HelpText => "ChatCommand_Help_Save";

		public string HelpSimpleText => "ChatCommand_HelpSimple_Save";

		public MyPromoteLevel VisibleTo => MyPromoteLevel.Admin;

		public void Handle(string[] args)
		{
			string arg = string.Empty;
			if (args != null && args.Length != 0)
			{
				arg = args[0];
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => Save, Sync.MyId, arg);
		}

		[Event(null, 415)]
		[Reliable]
		[Server]
		public static void Save(ulong requester, string name)
		{
			if (MySession.Static.GetUserPromoteLevel(MyEventContext.Current.Sender.Value) < MyPromoteLevel.Admin)
			{
				MyEventContext.ValidationFailed();
				return;
			}
			MySandboxGame.Log.WriteLineAndConsole("Executing /save command");
			MyAsyncSaving.Start(delegate
			{
				SaveFinish(requester);
			}, string.IsNullOrEmpty(name) ? null : name);
		}

		public static void SaveFinish(ulong requesting)
		{
			long num = MySession.Static.Players.TryGetIdentityId(requesting);
			if (num > 0)
			{
				if (MyMultiplayer.Static != null)
				{
					MyMultiplayer.Static.SendChatMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_ExecutingSaveFinished), ChatChannel.GlobalScripted, num, MyTexts.GetString(MySpaceTexts.ChatBotName));
				}
				else
				{
					MyHud.Chat.ShowMessageScripted(MyTexts.GetString(MySpaceTexts.ChatBotName), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_ExecutingSaveFinished));
				}
				MySandboxGame.Log.WriteLineAndConsole("Saving finished");
			}
		}
	}
}
