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
	public class MyObjectBuilder_UpdateTrigger : MyObjectBuilder_TriggerBase
	{
		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_UpdateTrigger, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in int value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out int value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EType_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in int value)
			{
				Set(ref *(MyObjectBuilder_TriggerBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out int value)
			{
				Get(ref *(MyObjectBuilder_TriggerBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EAABB_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EAABB_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableBoundingBoxD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in SerializableBoundingBoxD value)
			{
				Set(ref *(MyObjectBuilder_TriggerBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out SerializableBoundingBoxD value)
			{
				Get(ref *(MyObjectBuilder_TriggerBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EBoundingSphere_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EBoundingSphere_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableBoundingSphereD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in SerializableBoundingSphereD value)
			{
				Set(ref *(MyObjectBuilder_TriggerBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out SerializableBoundingSphereD value)
			{
				Get(ref *(MyObjectBuilder_TriggerBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EOffset_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in SerializableVector3D value)
			{
				Set(ref *(MyObjectBuilder_TriggerBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out SerializableVector3D value)
			{
				Get(ref *(MyObjectBuilder_TriggerBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EActor : IActivator, IActivator<MyObjectBuilder_UpdateTrigger>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_UpdateTrigger();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_UpdateTrigger CreateInstance()
			{
				return new MyObjectBuilder_UpdateTrigger();
			}

			MyObjectBuilder_UpdateTrigger IActivator<MyObjectBuilder_UpdateTrigger>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int Size = 25000;
	}
}
