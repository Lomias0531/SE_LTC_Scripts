using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public struct SerializableLineSectionInformation
	{
		protected class VRage_Game_SerializableLineSectionInformation_003C_003EDirection_003C_003EAccessor : IMemberAccessor<SerializableLineSectionInformation, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableLineSectionInformation owner, in Base6Directions.Direction value)
			{
				owner.Direction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableLineSectionInformation owner, out Base6Directions.Direction value)
			{
				value = owner.Direction;
			}
		}

		protected class VRage_Game_SerializableLineSectionInformation_003C_003ELength_003C_003EAccessor : IMemberAccessor<SerializableLineSectionInformation, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableLineSectionInformation owner, in int value)
			{
				owner.Length = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableLineSectionInformation owner, out int value)
			{
				value = owner.Length;
			}
		}

		private class VRage_Game_SerializableLineSectionInformation_003C_003EActor : IActivator, IActivator<SerializableLineSectionInformation>
		{
			private sealed override object CreateInstance()
			{
				return default(SerializableLineSectionInformation);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override SerializableLineSectionInformation CreateInstance()
			{
				return (SerializableLineSectionInformation)(object)default(SerializableLineSectionInformation);
			}

			SerializableLineSectionInformation IActivator<SerializableLineSectionInformation>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute]
		public Base6Directions.Direction Direction;

		[ProtoMember(4)]
		[XmlAttribute]
		public int Length;
	}
}
