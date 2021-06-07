using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_LastSession : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003EPath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LastSession owner, in string value)
			{
				owner.Path = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LastSession owner, out string value)
			{
				value = owner.Path;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003EIsContentWorlds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LastSession owner, in bool value)
			{
				owner.IsContentWorlds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LastSession owner, out bool value)
			{
				value = owner.IsContentWorlds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003EIsOnline_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LastSession owner, in bool value)
			{
				owner.IsOnline = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LastSession owner, out bool value)
			{
				value = owner.IsOnline;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003EIsLobby_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LastSession owner, in bool value)
			{
				owner.IsLobby = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LastSession owner, out bool value)
			{
				value = owner.IsLobby;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003EGameName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LastSession owner, in string value)
			{
				owner.GameName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LastSession owner, out string value)
			{
				value = owner.GameName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003EServerIP_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LastSession owner, in string value)
			{
				owner.ServerIP = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LastSession owner, out string value)
			{
				value = owner.ServerIP;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003EServerPort_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LastSession owner, in int value)
			{
				owner.ServerPort = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LastSession owner, out int value)
			{
				value = owner.ServerPort;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LastSession, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_LastSession owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_LastSession owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LastSession, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_LastSession owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_LastSession owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LastSession, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_LastSession owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_LastSession owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LastSession_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LastSession, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_LastSession owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_LastSession owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_LastSession_003C_003EActor : IActivator, IActivator<MyObjectBuilder_LastSession>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_LastSession();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_LastSession CreateInstance()
			{
				return new MyObjectBuilder_LastSession();
			}

			MyObjectBuilder_LastSession IActivator<MyObjectBuilder_LastSession>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Path;

		[ProtoMember(4)]
		public bool IsContentWorlds;

		[ProtoMember(7)]
		public bool IsOnline;

		[ProtoMember(10)]
		public bool IsLobby;

		[ProtoMember(13)]
		public string GameName;

		[ProtoMember(16)]
		public string ServerIP;

		[ProtoMember(19)]
		public int ServerPort;
	}
}
