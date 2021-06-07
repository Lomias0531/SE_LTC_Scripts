using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_MissionTriggers : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003EWinTriggers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, List<MyObjectBuilder_Trigger>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MissionTriggers owner, in List<MyObjectBuilder_Trigger> value)
			{
				owner.WinTriggers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MissionTriggers owner, out List<MyObjectBuilder_Trigger> value)
			{
				value = owner.WinTriggers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003ELoseTriggers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, List<MyObjectBuilder_Trigger>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MissionTriggers owner, in List<MyObjectBuilder_Trigger> value)
			{
				owner.LoseTriggers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MissionTriggers owner, out List<MyObjectBuilder_Trigger> value)
			{
				value = owner.LoseTriggers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003Emessage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MissionTriggers owner, in string value)
			{
				owner.message = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MissionTriggers owner, out string value)
			{
				value = owner.message;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003EWon_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MissionTriggers owner, in bool value)
			{
				owner.Won = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MissionTriggers owner, out bool value)
			{
				value = owner.Won;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003ELost_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MissionTriggers owner, in bool value)
			{
				owner.Lost = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MissionTriggers owner, out bool value)
			{
				value = owner.Lost;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MissionTriggers, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MissionTriggers owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MissionTriggers owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MissionTriggers, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MissionTriggers owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MissionTriggers owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MissionTriggers, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MissionTriggers owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MissionTriggers owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MissionTriggers, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MissionTriggers owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MissionTriggers owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_MissionTriggers_003C_003EActor : IActivator, IActivator<MyObjectBuilder_MissionTriggers>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_MissionTriggers();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_MissionTriggers CreateInstance()
			{
				return new MyObjectBuilder_MissionTriggers();
			}

			MyObjectBuilder_MissionTriggers IActivator<MyObjectBuilder_MissionTriggers>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public List<MyObjectBuilder_Trigger> WinTriggers = new List<MyObjectBuilder_Trigger>();

		[ProtoMember(4)]
		public List<MyObjectBuilder_Trigger> LoseTriggers = new List<MyObjectBuilder_Trigger>();

		[ProtoMember(7)]
		public string message;

		[ProtoMember(10)]
		public bool Won;

		[ProtoMember(13)]
		public bool Lost;
	}
}
