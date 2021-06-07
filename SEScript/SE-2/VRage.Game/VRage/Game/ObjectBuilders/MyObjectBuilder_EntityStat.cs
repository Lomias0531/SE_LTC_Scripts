using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_EntityStat : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStat owner, in float value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStat owner, out float value)
			{
				value = owner.Value;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003EMaxValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStat owner, in float value)
			{
				owner.MaxValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStat owner, out float value)
			{
				value = owner.MaxValue;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003EStatRegenAmountMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStat owner, in float value)
			{
				owner.StatRegenAmountMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStat owner, out float value)
			{
				value = owner.StatRegenAmountMultiplier;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003EStatRegenAmountMultiplierDuration_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStat owner, in float value)
			{
				owner.StatRegenAmountMultiplierDuration = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStat owner, out float value)
			{
				value = owner.StatRegenAmountMultiplierDuration;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003EEffects_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, MyObjectBuilder_EntityStatRegenEffect[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStat owner, in MyObjectBuilder_EntityStatRegenEffect[] value)
			{
				owner.Effects = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStat owner, out MyObjectBuilder_EntityStatRegenEffect[] value)
			{
				value = owner.Effects;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStat, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStat owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStat owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStat, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStat owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStat owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStat, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStat owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStat owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStat, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStat owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStat owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EntityStat>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EntityStat();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EntityStat CreateInstance()
			{
				return new MyObjectBuilder_EntityStat();
			}

			MyObjectBuilder_EntityStat IActivator<MyObjectBuilder_EntityStat>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float Value = 1f;

		[ProtoMember(4)]
		public float MaxValue = 1f;

		[ProtoMember(7)]
		public float StatRegenAmountMultiplier = 1f;

		[ProtoMember(10)]
		public float StatRegenAmountMultiplierDuration;

		[ProtoMember(13)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_EntityStatRegenEffect[] Effects;
	}
}
