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
	public class MyObjectBuilder_BehaviorTreeControlNodeMemory : MyObjectBuilder_BehaviorTreeNodeMemory
	{
		protected class VRage_Game_MyObjectBuilder_BehaviorTreeControlNodeMemory_003C_003EInitialIndex_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeControlNodeMemory, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, in int value)
			{
				owner.InitialIndex = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, out int value)
			{
				value = owner.InitialIndex;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeControlNodeMemory_003C_003EInitCalled_003C_003EAccessor : VRage_Game_MyObjectBuilder_BehaviorTreeNodeMemory_003C_003EInitCalled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeControlNodeMemory, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_BehaviorTreeNodeMemory*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_BehaviorTreeNodeMemory*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeControlNodeMemory_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeControlNodeMemory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeControlNodeMemory_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeControlNodeMemory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeControlNodeMemory_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeControlNodeMemory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeControlNodeMemory_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeControlNodeMemory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeControlNodeMemory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_BehaviorTreeControlNodeMemory_003C_003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeControlNodeMemory>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeControlNodeMemory();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_BehaviorTreeControlNodeMemory CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeControlNodeMemory();
			}

			MyObjectBuilder_BehaviorTreeControlNodeMemory IActivator<MyObjectBuilder_BehaviorTreeControlNodeMemory>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlAttribute]
		[ProtoMember(1)]
		[DefaultValue(0)]
		public int InitialIndex;
	}
}
