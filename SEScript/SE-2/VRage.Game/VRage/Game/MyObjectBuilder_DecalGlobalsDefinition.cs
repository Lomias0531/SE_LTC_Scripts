using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	public class MyObjectBuilder_DecalGlobalsDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EDecalQueueSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in int value)
			{
				owner.DecalQueueSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out int value)
			{
				value = owner.DecalQueueSize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalGlobalsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DecalGlobalsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DecalGlobalsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_DecalGlobalsDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_DecalGlobalsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_DecalGlobalsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_DecalGlobalsDefinition CreateInstance()
			{
				return new MyObjectBuilder_DecalGlobalsDefinition();
			}

			MyObjectBuilder_DecalGlobalsDefinition IActivator<MyObjectBuilder_DecalGlobalsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(25)]
		public int DecalQueueSize;
	}
}
