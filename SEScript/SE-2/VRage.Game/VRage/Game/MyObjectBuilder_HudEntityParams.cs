using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.Gui;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_HudEntityParams : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in Vector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out Vector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in long value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out long value)
			{
				value = owner.EntityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003EText_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003EFlagsEnum_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, MyHudIndicatorFlagsEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in MyHudIndicatorFlagsEnum value)
			{
				owner.FlagsEnum = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out MyHudIndicatorFlagsEnum value)
			{
				value = owner.FlagsEnum;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in long value)
			{
				owner.Owner = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out long value)
			{
				value = owner.Owner;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003EShare_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in MyOwnershipShareModeEnum value)
			{
				owner.Share = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out MyOwnershipShareModeEnum value)
			{
				value = owner.Share;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003EBlinkingTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in float value)
			{
				owner.BlinkingTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out float value)
			{
				value = owner.BlinkingTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HudEntityParams, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HudEntityParams, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HudEntityParams, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HudEntityParams, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HudEntityParams owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HudEntityParams owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_HudEntityParams_003C_003EActor : IActivator, IActivator<MyObjectBuilder_HudEntityParams>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_HudEntityParams();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_HudEntityParams CreateInstance()
			{
				return new MyObjectBuilder_HudEntityParams();
			}

			MyObjectBuilder_HudEntityParams IActivator<MyObjectBuilder_HudEntityParams>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector3D Position;

		public long EntityId;

		public string Text;

		public MyHudIndicatorFlagsEnum FlagsEnum;

		public long Owner;

		public MyOwnershipShareModeEnum Share;

		public float BlinkingTime;
	}
}
