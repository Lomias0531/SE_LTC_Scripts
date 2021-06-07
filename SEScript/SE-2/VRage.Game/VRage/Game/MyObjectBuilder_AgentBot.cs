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
	public class MyObjectBuilder_AgentBot : MyObjectBuilder_Bot
	{
		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003EAiTarget_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentBot, MyObjectBuilder_AiTarget>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentBot owner, in MyObjectBuilder_AiTarget value)
			{
				owner.AiTarget = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentBot owner, out MyObjectBuilder_AiTarget value)
			{
				value = owner.AiTarget;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003ERemoveAfterDeath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentBot, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentBot owner, in bool value)
			{
				owner.RemoveAfterDeath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentBot owner, out bool value)
			{
				value = owner.RemoveAfterDeath;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003ERespawnCounter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentBot, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentBot owner, in int value)
			{
				owner.RespawnCounter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentBot owner, out int value)
			{
				value = owner.RespawnCounter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003EBotDefId_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003EBotDefId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AgentBot owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_Bot*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AgentBot owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_Bot*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003EBotMemory_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003EBotMemory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, MyObjectBuilder_BotMemory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AgentBot owner, in MyObjectBuilder_BotMemory value)
			{
				Set(ref *(MyObjectBuilder_Bot*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AgentBot owner, out MyObjectBuilder_BotMemory value)
			{
				Get(ref *(MyObjectBuilder_Bot*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003ELastBehaviorTree_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003ELastBehaviorTree_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AgentBot owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Bot*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AgentBot owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Bot*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AgentBot owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AgentBot owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AgentBot owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AgentBot owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AgentBot owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AgentBot owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentBot_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AgentBot owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AgentBot owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_AgentBot_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AgentBot>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AgentBot();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AgentBot CreateInstance()
			{
				return new MyObjectBuilder_AgentBot();
			}

			MyObjectBuilder_AgentBot IActivator<MyObjectBuilder_AgentBot>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyObjectBuilder_AiTarget AiTarget;

		[ProtoMember(4)]
		public bool RemoveAfterDeath;

		[ProtoMember(7)]
		public int RespawnCounter;
	}
}
