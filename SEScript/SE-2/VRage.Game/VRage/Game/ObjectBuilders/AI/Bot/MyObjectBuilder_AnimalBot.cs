using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.AI.Bot
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimalBot : MyObjectBuilder_AgentBot
	{
		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003EAiTarget_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentBot_003C_003EAiTarget_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, MyObjectBuilder_AiTarget>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in MyObjectBuilder_AiTarget value)
			{
				Set(ref *(MyObjectBuilder_AgentBot*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out MyObjectBuilder_AiTarget value)
			{
				Get(ref *(MyObjectBuilder_AgentBot*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003ERemoveAfterDeath_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentBot_003C_003ERemoveAfterDeath_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AgentBot*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AgentBot*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003ERespawnCounter_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentBot_003C_003ERespawnCounter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in int value)
			{
				Set(ref *(MyObjectBuilder_AgentBot*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out int value)
			{
				Get(ref *(MyObjectBuilder_AgentBot*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003EBotDefId_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003EBotDefId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_Bot*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_Bot*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003EBotMemory_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003EBotMemory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, MyObjectBuilder_BotMemory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in MyObjectBuilder_BotMemory value)
			{
				Set(ref *(MyObjectBuilder_Bot*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out MyObjectBuilder_BotMemory value)
			{
				Get(ref *(MyObjectBuilder_Bot*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003ELastBehaviorTree_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003ELastBehaviorTree_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Bot*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Bot*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBot owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBot owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimalBot>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimalBot();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimalBot CreateInstance()
			{
				return new MyObjectBuilder_AnimalBot();
			}

			MyObjectBuilder_AnimalBot IActivator<MyObjectBuilder_AnimalBot>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
