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
	public class MyObjectBuilder_ToolbarItemAnimation : MyObjectBuilder_ToolbarItemDefinition
	{
		protected class VRage_Game_MyObjectBuilder_ToolbarItemAnimation_003C_003EDefinitionId_003C_003EAccessor : VRage_Game_MyObjectBuilder_ToolbarItemDefinition_003C_003EDefinitionId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemAnimation, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolbarItemAnimation owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_ToolbarItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolbarItemAnimation owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_ToolbarItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolbarItemAnimation_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemAnimation, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolbarItemAnimation owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolbarItemAnimation owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolbarItemAnimation_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemAnimation, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolbarItemAnimation owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolbarItemAnimation owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolbarItemAnimation_003C_003EdefId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarItemAnimation, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarItemAnimation owner, in SerializableDefinitionId value)
			{
				owner.defId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarItemAnimation owner, out SerializableDefinitionId value)
			{
				value = owner.defId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolbarItemAnimation_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemAnimation, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolbarItemAnimation owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolbarItemAnimation owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolbarItemAnimation_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemAnimation, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolbarItemAnimation owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolbarItemAnimation owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ToolbarItemAnimation_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ToolbarItemAnimation>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ToolbarItemAnimation();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ToolbarItemAnimation CreateInstance()
			{
				return new MyObjectBuilder_ToolbarItemAnimation();
			}

			MyObjectBuilder_ToolbarItemAnimation IActivator<MyObjectBuilder_ToolbarItemAnimation>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SerializableDefinitionId defId
		{
			get
			{
				return DefinitionId;
			}
			set
			{
				DefinitionId = value;
			}
		}

		public bool ShouldSerializedefId()
		{
			return false;
		}
	}
}
