using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_EntityStatRegenEffect : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003ETickAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value)
			{
				owner.TickAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value)
			{
				value = owner.TickAmount;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003EInterval_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value)
			{
				owner.Interval = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value)
			{
				value = owner.Interval;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003EMaxRegenRatio_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value)
			{
				owner.MaxRegenRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value)
			{
				value = owner.MaxRegenRatio;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003EMinRegenRatio_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value)
			{
				owner.MinRegenRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value)
			{
				value = owner.MinRegenRatio;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003EAliveTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value)
			{
				owner.AliveTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value)
			{
				value = owner.AliveTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003EDuration_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value)
			{
				owner.Duration = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value)
			{
				value = owner.Duration;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EntityStatRegenEffect>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EntityStatRegenEffect();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EntityStatRegenEffect CreateInstance()
			{
				return new MyObjectBuilder_EntityStatRegenEffect();
			}

			MyObjectBuilder_EntityStatRegenEffect IActivator<MyObjectBuilder_EntityStatRegenEffect>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float TickAmount;

		[ProtoMember(4)]
		public float Interval = 1f;

		[ProtoMember(7)]
		public float MaxRegenRatio = 1f;

		[ProtoMember(10)]
		public float MinRegenRatio;

		[ProtoMember(13)]
		public float AliveTime;

		[ProtoMember(16)]
		public float Duration = -1f;
	}
}
