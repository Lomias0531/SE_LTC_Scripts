using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AreaTrigger : MyObjectBuilder_TriggerBase
	{
		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AreaTrigger, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AreaTrigger owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AreaTrigger owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003EType_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaTrigger owner, in int value)
			{
				Set(ref *(MyObjectBuilder_TriggerBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaTrigger owner, out int value)
			{
				Get(ref *(MyObjectBuilder_TriggerBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003EAABB_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EAABB_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, SerializableBoundingBoxD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaTrigger owner, in SerializableBoundingBoxD value)
			{
				Set(ref *(MyObjectBuilder_TriggerBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaTrigger owner, out SerializableBoundingBoxD value)
			{
				Get(ref *(MyObjectBuilder_TriggerBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003EBoundingSphere_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EBoundingSphere_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, SerializableBoundingSphereD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaTrigger owner, in SerializableBoundingSphereD value)
			{
				Set(ref *(MyObjectBuilder_TriggerBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaTrigger owner, out SerializableBoundingSphereD value)
			{
				Get(ref *(MyObjectBuilder_TriggerBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003EOffset_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaTrigger owner, in SerializableVector3D value)
			{
				Set(ref *(MyObjectBuilder_TriggerBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaTrigger owner, out SerializableVector3D value)
			{
				Get(ref *(MyObjectBuilder_TriggerBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaTrigger owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaTrigger owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaTrigger owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaTrigger owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaTrigger owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaTrigger owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaTrigger owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaTrigger owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AreaTrigger>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AreaTrigger();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AreaTrigger CreateInstance()
			{
				return new MyObjectBuilder_AreaTrigger();
			}

			MyObjectBuilder_AreaTrigger IActivator<MyObjectBuilder_AreaTrigger>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Name;
	}
}
