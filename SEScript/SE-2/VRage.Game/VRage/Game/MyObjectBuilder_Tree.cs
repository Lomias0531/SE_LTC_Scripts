using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Tree : MyObjectBuilder_EntityBase
	{
		protected class VRage_Game_MyObjectBuilder_Tree_003C_003EEntityId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in long value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out long value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Tree_003C_003EPersistentFlags_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPersistentFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, MyPersistentEntityFlags2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in MyPersistentEntityFlags2 value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out MyPersistentEntityFlags2 value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Tree_003C_003EName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Tree_003C_003EPositionAndOrientation_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPositionAndOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in MyPositionAndOrientation? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out MyPositionAndOrientation? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Tree_003C_003EComponentContainer_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Tree_003C_003EEntityDefinitionId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityDefinitionId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Tree_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Tree_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Tree_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Tree_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Tree, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Tree owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Tree owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Tree_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Tree>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Tree();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Tree CreateInstance()
			{
				return new MyObjectBuilder_Tree();
			}

			MyObjectBuilder_Tree IActivator<MyObjectBuilder_Tree>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
