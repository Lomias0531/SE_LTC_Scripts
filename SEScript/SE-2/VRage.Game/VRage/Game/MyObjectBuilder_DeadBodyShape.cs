using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyObjectBuilder_DeadBodyShape
	{
		protected class VRage_Game_MyObjectBuilder_DeadBodyShape_003C_003EBoxShapeScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DeadBodyShape, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DeadBodyShape owner, in SerializableVector3 value)
			{
				owner.BoxShapeScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DeadBodyShape owner, out SerializableVector3 value)
			{
				value = owner.BoxShapeScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DeadBodyShape_003C_003ERelativeCenterOfMass_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DeadBodyShape, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DeadBodyShape owner, in SerializableVector3 value)
			{
				owner.RelativeCenterOfMass = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DeadBodyShape owner, out SerializableVector3 value)
			{
				value = owner.RelativeCenterOfMass;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DeadBodyShape_003C_003ERelativeShapeTranslation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DeadBodyShape, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DeadBodyShape owner, in SerializableVector3 value)
			{
				owner.RelativeShapeTranslation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DeadBodyShape owner, out SerializableVector3 value)
			{
				value = owner.RelativeShapeTranslation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DeadBodyShape_003C_003EFriction_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DeadBodyShape, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DeadBodyShape owner, in float value)
			{
				owner.Friction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DeadBodyShape owner, out float value)
			{
				value = owner.Friction;
			}
		}

		private class VRage_Game_MyObjectBuilder_DeadBodyShape_003C_003EActor : IActivator, IActivator<MyObjectBuilder_DeadBodyShape>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_DeadBodyShape();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_DeadBodyShape CreateInstance()
			{
				return new MyObjectBuilder_DeadBodyShape();
			}

			MyObjectBuilder_DeadBodyShape IActivator<MyObjectBuilder_DeadBodyShape>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(23)]
		public SerializableVector3 BoxShapeScale;

		[ProtoMember(24)]
		public SerializableVector3 RelativeCenterOfMass;

		[ProtoMember(25)]
		public SerializableVector3 RelativeShapeTranslation;

		[ProtoMember(26)]
		public float Friction;
	}
}
