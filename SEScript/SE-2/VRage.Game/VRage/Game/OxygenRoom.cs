using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public struct OxygenRoom
	{
		protected class VRage_Game_OxygenRoom_003C_003EStartingPosition_003C_003EAccessor : IMemberAccessor<OxygenRoom, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref OxygenRoom owner, in Vector3I value)
			{
				owner.StartingPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref OxygenRoom owner, out Vector3I value)
			{
				value = owner.StartingPosition;
			}
		}

		protected class VRage_Game_OxygenRoom_003C_003EOxygenAmount_003C_003EAccessor : IMemberAccessor<OxygenRoom, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref OxygenRoom owner, in float value)
			{
				owner.OxygenAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref OxygenRoom owner, out float value)
			{
				value = owner.OxygenAmount;
			}
		}

		private class VRage_Game_OxygenRoom_003C_003EActor : IActivator, IActivator<OxygenRoom>
		{
			private sealed override object CreateInstance()
			{
				return default(OxygenRoom);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override OxygenRoom CreateInstance()
			{
				return (OxygenRoom)(object)default(OxygenRoom);
			}

			OxygenRoom IActivator<OxygenRoom>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(112)]
		public Vector3I StartingPosition;

		[ProtoMember(115)]
		[XmlAttribute]
		public float OxygenAmount;
	}
}
