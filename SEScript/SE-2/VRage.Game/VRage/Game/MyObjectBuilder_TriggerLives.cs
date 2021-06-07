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
	public class MyObjectBuilder_TriggerLives : MyObjectBuilder_Trigger
	{
		protected class VRage_Game_MyObjectBuilder_TriggerLives_003C_003ELives_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerLives, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TriggerLives owner, in int value)
			{
				owner.Lives = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TriggerLives owner, out int value)
			{
				value = owner.Lives;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerLives_003C_003EIsTrue_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003EIsTrue_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerLives, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerLives owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerLives owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerLives_003C_003EMessage_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003EMessage_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerLives, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerLives owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerLives owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerLives_003C_003EWwwLink_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003EWwwLink_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerLives, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerLives owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerLives owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerLives_003C_003ENextMission_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003ENextMission_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerLives, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerLives owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerLives owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerLives_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerLives, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerLives owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerLives owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerLives_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerLives, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerLives owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerLives owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerLives_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerLives, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerLives owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerLives owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerLives_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerLives, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerLives owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerLives owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_TriggerLives_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TriggerLives>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TriggerLives();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TriggerLives CreateInstance()
			{
				return new MyObjectBuilder_TriggerLives();
			}

			MyObjectBuilder_TriggerLives IActivator<MyObjectBuilder_TriggerLives>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int Lives;
	}
}
