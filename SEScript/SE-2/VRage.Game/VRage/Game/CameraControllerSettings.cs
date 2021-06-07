using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class CameraControllerSettings
	{
		protected class VRage_Game_CameraControllerSettings_003C_003EIsFirstPerson_003C_003EAccessor : IMemberAccessor<CameraControllerSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CameraControllerSettings owner, in bool value)
			{
				owner.IsFirstPerson = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CameraControllerSettings owner, out bool value)
			{
				value = owner.IsFirstPerson;
			}
		}

		protected class VRage_Game_CameraControllerSettings_003C_003EDistance_003C_003EAccessor : IMemberAccessor<CameraControllerSettings, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CameraControllerSettings owner, in double value)
			{
				owner.Distance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CameraControllerSettings owner, out double value)
			{
				value = owner.Distance;
			}
		}

		protected class VRage_Game_CameraControllerSettings_003C_003EHeadAngle_003C_003EAccessor : IMemberAccessor<CameraControllerSettings, SerializableVector2?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CameraControllerSettings owner, in SerializableVector2? value)
			{
				owner.HeadAngle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CameraControllerSettings owner, out SerializableVector2? value)
			{
				value = owner.HeadAngle;
			}
		}

		protected class VRage_Game_CameraControllerSettings_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<CameraControllerSettings, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CameraControllerSettings owner, in long value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CameraControllerSettings owner, out long value)
			{
				value = owner.EntityId;
			}
		}

		private class VRage_Game_CameraControllerSettings_003C_003EActor : IActivator, IActivator<CameraControllerSettings>
		{
			private sealed override object CreateInstance()
			{
				return new CameraControllerSettings();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override CameraControllerSettings CreateInstance()
			{
				return new CameraControllerSettings();
			}

			CameraControllerSettings IActivator<CameraControllerSettings>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public bool IsFirstPerson;

		[ProtoMember(4)]
		public double Distance;

		[ProtoMember(7)]
		public SerializableVector2? HeadAngle;

		[XmlAttribute]
		public long EntityId;
	}
}
