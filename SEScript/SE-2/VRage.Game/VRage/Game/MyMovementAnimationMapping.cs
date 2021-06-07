using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyMovementAnimationMapping
	{
		protected class VRage_Game_MyMovementAnimationMapping_003C_003EName_003C_003EAccessor : IMemberAccessor<MyMovementAnimationMapping, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyMovementAnimationMapping owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyMovementAnimationMapping owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyMovementAnimationMapping_003C_003EAnimationSubtypeName_003C_003EAccessor : IMemberAccessor<MyMovementAnimationMapping, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyMovementAnimationMapping owner, in string value)
			{
				owner.AnimationSubtypeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyMovementAnimationMapping owner, out string value)
			{
				value = owner.AnimationSubtypeName;
			}
		}

		private class VRage_Game_MyMovementAnimationMapping_003C_003EActor : IActivator, IActivator<MyMovementAnimationMapping>
		{
			private sealed override object CreateInstance()
			{
				return new MyMovementAnimationMapping();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMovementAnimationMapping CreateInstance()
			{
				return new MyMovementAnimationMapping();
			}

			MyMovementAnimationMapping IActivator<MyMovementAnimationMapping>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(12)]
		[XmlAttribute]
		public string Name;

		[ProtoMember(13)]
		[XmlAttribute]
		public string AnimationSubtypeName;
	}
}
