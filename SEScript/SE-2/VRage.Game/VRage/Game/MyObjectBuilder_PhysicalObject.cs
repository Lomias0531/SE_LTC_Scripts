using ProtoBuf;
using System.ComponentModel;
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
	public class MyObjectBuilder_PhysicalObject : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_PhysicalObject_003C_003EFlags_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalObject, MyItemFlags>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalObject owner, in MyItemFlags value)
			{
				owner.Flags = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalObject owner, out MyItemFlags value)
			{
				value = owner.Flags;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalObject_003C_003EDurabilityHP_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalObject, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PhysicalObject owner, in float? value)
			{
				owner.DurabilityHP = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PhysicalObject owner, out float? value)
			{
				value = owner.DurabilityHP;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalObject_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalObject, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalObject owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalObject owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalObject_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalObject, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalObject owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalObject owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalObject_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalObject, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalObject owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalObject owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PhysicalObject_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalObject, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PhysicalObject owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PhysicalObject owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_PhysicalObject_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PhysicalObject>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PhysicalObject();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PhysicalObject CreateInstance()
			{
				return new MyObjectBuilder_PhysicalObject();
			}

			MyObjectBuilder_PhysicalObject IActivator<MyObjectBuilder_PhysicalObject>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[DefaultValue(MyItemFlags.None)]
		public MyItemFlags Flags;

		/// <summary>
		/// This is used for GUI to show the amount of health points (durability) of the weapons and tools. This is updated through Durability entity component if entity exists..
		/// </summary>
		[ProtoMember(4)]
		[DefaultValue(null)]
		public float? DurabilityHP;

		public bool ShouldSerializeDurabilityHP()
		{
			return DurabilityHP.HasValue;
		}

		public virtual bool CanStack(MyObjectBuilder_PhysicalObject a)
		{
			if (a == null)
			{
				return false;
			}
			return CanStack(a.TypeId, a.SubtypeId, a.Flags);
		}

		public virtual bool CanStack(MyObjectBuilderType typeId, MyStringHash subtypeId, MyItemFlags flags)
		{
			if (flags == Flags && typeId == base.TypeId && subtypeId == base.SubtypeId)
			{
				return true;
			}
			return false;
		}

		public MyObjectBuilder_PhysicalObject()
			: this(MyItemFlags.None)
		{
		}

		public MyObjectBuilder_PhysicalObject(MyItemFlags flags)
		{
			Flags = flags;
		}

		public virtual MyDefinitionId GetObjectId()
		{
			return this.GetId();
		}
	}
}
