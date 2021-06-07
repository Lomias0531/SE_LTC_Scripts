using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.AI
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_DroneAI : MyObjectBuilder_AutomaticBehaviour
	{
		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003ECurrentPreset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneAI, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneAI owner, in string value)
			{
				owner.CurrentPreset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneAI owner, out string value)
			{
				value = owner.CurrentPreset;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003EAlternativebehaviorSwitched_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneAI, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneAI owner, in bool value)
			{
				owner.AlternativebehaviorSwitched = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneAI owner, out bool value)
			{
				value = owner.AlternativebehaviorSwitched;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003EReturnPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneAI, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneAI owner, in SerializableVector3D value)
			{
				owner.ReturnPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneAI owner, out SerializableVector3D value)
			{
				value = owner.ReturnPosition;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003ECanSkipWaypoint_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneAI, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneAI owner, in bool value)
			{
				owner.CanSkipWaypoint = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneAI owner, out bool value)
			{
				value = owner.CanSkipWaypoint;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003ENeedUpdate_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ENeedUpdate_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003EIsActive_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EIsActive_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003ECollisionAvoidance_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ECollisionAvoidance_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003EPlayerPriority_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EPlayerPriority_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in int value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out int value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003EMaxPlayerDistance_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EMaxPlayerDistance_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in float value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out float value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003ECycleWaypoints_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ECycleWaypoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003EInAmbushMode_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EInAmbushMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003ECurrentTarget_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ECurrentTarget_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in long value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out long value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003ESpeedLimit_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ESpeedLimit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in float value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out float value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003ETargetList_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ETargetList_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, List<DroneTargetSerializable>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in List<DroneTargetSerializable> value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out List<DroneTargetSerializable> value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003EWaypointList_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EWaypointList_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in List<long> value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out List<long> value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003EPrioritizationStyle_003C_003EAccessor : VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EPrioritizationStyle_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, TargetPrioritization>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in TargetPrioritization value)
			{
				Set(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out TargetPrioritization value)
			{
				Get(ref *(MyObjectBuilder_AutomaticBehaviour*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneAI owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneAI owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI_003C_003EActor : IActivator, IActivator<MyObjectBuilder_DroneAI>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_DroneAI();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_DroneAI CreateInstance()
			{
				return new MyObjectBuilder_DroneAI();
			}

			MyObjectBuilder_DroneAI IActivator<MyObjectBuilder_DroneAI>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string CurrentPreset = string.Empty;

		[ProtoMember(4)]
		public bool AlternativebehaviorSwitched;

		[ProtoMember(7)]
		public SerializableVector3D ReturnPosition;

		[ProtoMember(10)]
		public bool CanSkipWaypoint = true;
	}
}
