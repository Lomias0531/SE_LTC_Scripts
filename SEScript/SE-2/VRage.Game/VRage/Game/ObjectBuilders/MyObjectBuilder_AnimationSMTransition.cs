using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageRender.Animations;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationSMTransition : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003EFrom_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value)
			{
				owner.From = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value)
			{
				value = owner.From;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003ETo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value)
			{
				owner.To = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value)
			{
				value = owner.To;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003ETimeInSec_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in double value)
			{
				owner.TimeInSec = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out double value)
			{
				value = owner.TimeInSec;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003ESync_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyAnimationTransitionSyncType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in MyAnimationTransitionSyncType value)
			{
				owner.Sync = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out MyAnimationTransitionSyncType value)
			{
				value = owner.Sync;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003EConditions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyObjectBuilder_AnimationSMConditionsConjunction[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in MyObjectBuilder_AnimationSMConditionsConjunction[] value)
			{
				owner.Conditions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out MyObjectBuilder_AnimationSMConditionsConjunction[] value)
			{
				value = owner.Conditions;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003EPriority_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in int? value)
			{
				owner.Priority = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out int? value)
			{
				value = owner.Priority;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003ECurve_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyAnimationTransitionCurve>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in MyAnimationTransitionCurve value)
			{
				owner.Curve = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out MyAnimationTransitionCurve value)
			{
				value = owner.Curve;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMTransition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMTransition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationSMTransition CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMTransition();
			}

			MyObjectBuilder_AnimationSMTransition IActivator<MyObjectBuilder_AnimationSMTransition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute]
		public string Name;

		[ProtoMember(4)]
		[XmlAttribute]
		public string From;

		[ProtoMember(7)]
		[XmlAttribute]
		public string To;

		[ProtoMember(10)]
		[XmlAttribute]
		public double TimeInSec;

		[ProtoMember(13)]
		[XmlAttribute]
		public MyAnimationTransitionSyncType Sync;

		[ProtoMember(16)]
		[XmlArrayItem("Conjunction")]
		public MyObjectBuilder_AnimationSMConditionsConjunction[] Conditions;

		[ProtoMember(19)]
		public int? Priority;

		[ProtoMember(22)]
		[XmlAttribute]
		public MyAnimationTransitionCurve Curve;
	}
}
