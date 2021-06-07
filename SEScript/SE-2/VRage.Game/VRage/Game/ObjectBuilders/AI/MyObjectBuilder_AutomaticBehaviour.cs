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
	public class MyObjectBuilder_AutomaticBehaviour : MyObjectBuilder_Base
	{
		[ProtoContract]
		public struct DroneTargetSerializable
		{
			protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EDroneTargetSerializable_003C_003ETargetId_003C_003EAccessor : IMemberAccessor<DroneTargetSerializable, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref DroneTargetSerializable owner, in long value)
				{
					owner.TargetId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref DroneTargetSerializable owner, out long value)
				{
					value = owner.TargetId;
				}
			}

			protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EDroneTargetSerializable_003C_003EPriority_003C_003EAccessor : IMemberAccessor<DroneTargetSerializable, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref DroneTargetSerializable owner, in int value)
				{
					owner.Priority = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref DroneTargetSerializable owner, out int value)
				{
					value = owner.Priority;
				}
			}

			private class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EDroneTargetSerializable_003C_003EActor : IActivator, IActivator<DroneTargetSerializable>
			{
				private sealed override object CreateInstance()
				{
					return default(DroneTargetSerializable);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override DroneTargetSerializable CreateInstance()
				{
					return (DroneTargetSerializable)(object)default(DroneTargetSerializable);
				}

				DroneTargetSerializable IActivator<DroneTargetSerializable>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long TargetId;

			[ProtoMember(4)]
			public int Priority;

			public DroneTargetSerializable(long targetId, int priority)
			{
				TargetId = targetId;
				Priority = priority;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ENeedUpdate_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value)
			{
				owner.NeedUpdate = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value)
			{
				value = owner.NeedUpdate;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EIsActive_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value)
			{
				owner.IsActive = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value)
			{
				value = owner.IsActive;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ECollisionAvoidance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value)
			{
				owner.CollisionAvoidance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value)
			{
				value = owner.CollisionAvoidance;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EPlayerPriority_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in int value)
			{
				owner.PlayerPriority = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out int value)
			{
				value = owner.PlayerPriority;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EMaxPlayerDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in float value)
			{
				owner.MaxPlayerDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out float value)
			{
				value = owner.MaxPlayerDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ECycleWaypoints_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value)
			{
				owner.CycleWaypoints = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value)
			{
				value = owner.CycleWaypoints;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EInAmbushMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value)
			{
				owner.InAmbushMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value)
			{
				value = owner.InAmbushMode;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ECurrentTarget_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in long value)
			{
				owner.CurrentTarget = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out long value)
			{
				value = owner.CurrentTarget;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ESpeedLimit_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in float value)
			{
				owner.SpeedLimit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out float value)
			{
				value = owner.SpeedLimit;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ETargetList_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, List<DroneTargetSerializable>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in List<DroneTargetSerializable> value)
			{
				owner.TargetList = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out List<DroneTargetSerializable> value)
			{
				value = owner.TargetList;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EWaypointList_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in List<long> value)
			{
				owner.WaypointList = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out List<long> value)
			{
				value = owner.WaypointList;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EPrioritizationStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, TargetPrioritization>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in TargetPrioritization value)
			{
				owner.PrioritizationStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out TargetPrioritization value)
			{
				value = owner.PrioritizationStyle;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AutomaticBehaviour>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AutomaticBehaviour();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AutomaticBehaviour CreateInstance()
			{
				return new MyObjectBuilder_AutomaticBehaviour();
			}

			MyObjectBuilder_AutomaticBehaviour IActivator<MyObjectBuilder_AutomaticBehaviour>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		public bool NeedUpdate = true;

		[ProtoMember(10)]
		public bool IsActive = true;

		[ProtoMember(13)]
		public bool CollisionAvoidance = true;

		[ProtoMember(16)]
		public int PlayerPriority = 10;

		[ProtoMember(19)]
		public float MaxPlayerDistance = 10000f;

		[ProtoMember(22)]
		public bool CycleWaypoints;

		[ProtoMember(25)]
		public bool InAmbushMode;

		[ProtoMember(28)]
		public long CurrentTarget;

		[ProtoMember(31)]
		public float SpeedLimit = float.MinValue;

		[ProtoMember(34)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<DroneTargetSerializable> TargetList;

		[ProtoMember(37)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<long> WaypointList;

		[ProtoMember(40)]
		public TargetPrioritization PrioritizationStyle = TargetPrioritization.PriorityRandom;

		public MyObjectBuilder_AutomaticBehaviour()
		{
			TargetList = new List<DroneTargetSerializable>();
			WaypointList = new List<long>();
		}
	}
}
