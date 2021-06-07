using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_TriggerPositionLeft : MyObjectBuilder_Trigger
	{
		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003EPos_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in Vector3D value)
			{
				owner.Pos = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out Vector3D value)
			{
				value = owner.Pos;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003EDistance2_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in double value)
			{
				owner.Distance2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out double value)
			{
				value = owner.Distance2;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003EIsTrue_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003EIsTrue_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003EMessage_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003EMessage_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003EWwwLink_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003EWwwLink_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003ENextMission_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003ENextMission_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_TriggerPositionLeft_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TriggerPositionLeft>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TriggerPositionLeft();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TriggerPositionLeft CreateInstance()
			{
				return new MyObjectBuilder_TriggerPositionLeft();
			}

			MyObjectBuilder_TriggerPositionLeft IActivator<MyObjectBuilder_TriggerPositionLeft>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public Vector3D Pos;

		[ProtoMember(4)]
		public double Distance2;
	}
}
