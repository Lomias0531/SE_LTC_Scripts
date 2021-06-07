using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyObjectBuilder_MyFeetIKSettings
	{
		protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EMovementState_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in string value)
			{
				owner.MovementState = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out string value)
			{
				value = owner.MovementState;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in bool value)
			{
				owner.Enabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out bool value)
			{
				value = owner.Enabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EBelowReachableDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value)
			{
				owner.BelowReachableDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value)
			{
				value = owner.BelowReachableDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EAboveReachableDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value)
			{
				owner.AboveReachableDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value)
			{
				value = owner.AboveReachableDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EVerticalShiftUpGain_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value)
			{
				owner.VerticalShiftUpGain = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value)
			{
				value = owner.VerticalShiftUpGain;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EVerticalShiftDownGain_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value)
			{
				owner.VerticalShiftDownGain = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value)
			{
				value = owner.VerticalShiftDownGain;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EFootLenght_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value)
			{
				owner.FootLenght = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value)
			{
				value = owner.FootLenght;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EFootWidth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value)
			{
				owner.FootWidth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value)
			{
				value = owner.FootWidth;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EAnkleHeight_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value)
			{
				owner.AnkleHeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value)
			{
				value = owner.AnkleHeight;
			}
		}

		private class VRage_Game_MyObjectBuilder_MyFeetIKSettings_003C_003EActor : IActivator, IActivator<MyObjectBuilder_MyFeetIKSettings>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_MyFeetIKSettings();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_MyFeetIKSettings CreateInstance()
			{
				return new MyObjectBuilder_MyFeetIKSettings();
			}

			MyObjectBuilder_MyFeetIKSettings IActivator<MyObjectBuilder_MyFeetIKSettings>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(14)]
		public string MovementState;

		[ProtoMember(15)]
		public bool Enabled;

		[ProtoMember(16)]
		public float BelowReachableDistance;

		[ProtoMember(17)]
		public float AboveReachableDistance;

		[ProtoMember(18)]
		public float VerticalShiftUpGain;

		[ProtoMember(19)]
		public float VerticalShiftDownGain;

		[ProtoMember(20)]
		public float FootLenght;

		[ProtoMember(21)]
		public float FootWidth;

		[ProtoMember(22)]
		public float AnkleHeight;
	}
}
