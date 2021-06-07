using ProtoBuf;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game
{
	public class MyCachedServerItem
	{
		[ProtoContract]
		public class MyServerData
		{
			protected class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003ESettings_003C_003EAccessor : IMemberAccessor<MyServerData, MyObjectBuilder_SessionSettings>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyServerData owner, in MyObjectBuilder_SessionSettings value)
				{
					owner.Settings = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyServerData owner, out MyObjectBuilder_SessionSettings value)
				{
					value = owner.Settings;
				}
			}

			protected class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003EExperimentalMode_003C_003EAccessor : IMemberAccessor<MyServerData, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyServerData owner, in bool value)
				{
					owner.ExperimentalMode = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyServerData owner, out bool value)
				{
					value = owner.ExperimentalMode;
				}
			}

			protected class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003EMods_003C_003EAccessor : IMemberAccessor<MyServerData, List<ulong>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyServerData owner, in List<ulong> value)
				{
					owner.Mods = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyServerData owner, out List<ulong> value)
				{
					value = owner.Mods;
				}
			}

			protected class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003EDescription_003C_003EAccessor : IMemberAccessor<MyServerData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyServerData owner, in string value)
				{
					owner.Description = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyServerData owner, out string value)
				{
					value = owner.Description;
				}
			}

			private class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003EActor : IActivator, IActivator<MyServerData>
			{
				private sealed override object CreateInstance()
				{
					return new MyServerData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyServerData CreateInstance()
				{
					return new MyServerData();
				}

				MyServerData IActivator<MyServerData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public MyObjectBuilder_SessionSettings Settings;

			[ProtoMember(4)]
			public bool ExperimentalMode;

			[ProtoMember(7)]
			public List<ulong> Mods = new List<ulong>();

			[ProtoMember(10)]
			public string Description;
		}

		public readonly bool AllowedInGroup;

		public readonly MyGameServerItem Server;

		public Dictionary<string, string> Rules;

		private MyServerData m_data = new MyServerData();

		private const int RULE_LENGTH = 93;

		public MyObjectBuilder_SessionSettings Settings => m_data.Settings;

		public bool ExperimentalMode => m_data.ExperimentalMode;

		public string Description => m_data.Description;

		public List<ulong> Mods => m_data.Mods;

		public MyCachedServerItem()
		{
		}

		public MyCachedServerItem(MyGameServerItem server)
		{
			Server = server;
			Rules = null;
			ulong gameTagByPrefixUlong = server.GetGameTagByPrefixUlong("groupId");
			AllowedInGroup = (gameTagByPrefixUlong == 0L || MyGameService.IsUserInGroup(gameTagByPrefixUlong));
		}

		public static void SendSettingsToSteam()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated || MyGameService.GameServer == null)
			{
				return;
			}
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				MyServerData instance = new MyServerData
				{
					Settings = MySession.Static.Settings,
					ExperimentalMode = MySession.Static.IsSettingsExperimental(),
					Mods = MySession.Static.Mods.Select((MyObjectBuilder_Checkpoint.ModItem m) => m.PublishedFileId).Distinct().ToList(),
					Description = ((MySandboxGame.ConfigDedicated == null) ? null : MySandboxGame.ConfigDedicated.ServerDescription)
				};
				Serializer.Serialize(memoryStream, instance);
				array = MyCompression.Compress(memoryStream.ToArray());
			}
			MyGameService.GameServer.SetKeyValue("sc", array.Length.ToString());
			for (int i = 0; (double)i < Math.Ceiling((double)array.Length / 93.0); i++)
			{
				byte[] array2 = new byte[93];
				int num = array.Length - 93 * i;
				if (num >= 93)
				{
					Array.Copy(array, i * 93, array2, 0, 93);
				}
				else
				{
					array2 = new byte[num];
					Array.Copy(array, i * 93, array2, 0, num);
				}
				MyGameService.GameServer.SetKeyValue("sc" + i, Convert.ToBase64String(array2));
			}
		}

		public void DeserializeSettings()
		{
			string value = null;
			try
			{
				if (Rules.TryGetValue("sc", out value))
				{
					int num = int.Parse(value);
					byte[] array = new byte[num];
					for (int i = 0; (double)i < Math.Ceiling((double)num / 93.0); i++)
					{
						byte[] array2 = Convert.FromBase64String(Rules["sc" + i]);
						Array.Copy(array2, 0, array, i * 93, array2.Length);
					}
					using (MemoryStream source = new MemoryStream(MyCompression.Decompress(array)))
					{
						m_data = Serializer.Deserialize<MyServerData>(source);
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLineAndConsole("Failed to deserialize session settings for server!");
				MyLog.Default.WriteLineAndConsole(value);
				MyLog.Default.WriteLineAndConsole(ex.ToString());
			}
		}
	}
}
