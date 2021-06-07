using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public class MyPlanetSurfaceDetail
	{
		protected class VRage_Game_MyPlanetSurfaceDetail_003C_003ETexture_003C_003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetSurfaceDetail owner, in string value)
			{
				owner.Texture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetSurfaceDetail owner, out string value)
			{
				value = owner.Texture;
			}
		}

		protected class VRage_Game_MyPlanetSurfaceDetail_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetSurfaceDetail owner, in float value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetSurfaceDetail owner, out float value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_MyPlanetSurfaceDetail_003C_003EScale_003C_003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetSurfaceDetail owner, in float value)
			{
				owner.Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetSurfaceDetail owner, out float value)
			{
				value = owner.Scale;
			}
		}

		protected class VRage_Game_MyPlanetSurfaceDetail_003C_003ESlope_003C_003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetSurfaceDetail owner, in SerializableRange value)
			{
				owner.Slope = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetSurfaceDetail owner, out SerializableRange value)
			{
				value = owner.Slope;
			}
		}

		protected class VRage_Game_MyPlanetSurfaceDetail_003C_003ETransition_003C_003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetSurfaceDetail owner, in float value)
			{
				owner.Transition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetSurfaceDetail owner, out float value)
			{
				value = owner.Transition;
			}
		}

		private class VRage_Game_MyPlanetSurfaceDetail_003C_003EActor : IActivator, IActivator<MyPlanetSurfaceDetail>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetSurfaceDetail();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetSurfaceDetail CreateInstance()
			{
				return new MyPlanetSurfaceDetail();
			}

			MyPlanetSurfaceDetail IActivator<MyPlanetSurfaceDetail>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(33)]
		public string Texture;

		[ProtoMember(34)]
		public float Size;

		[ProtoMember(35)]
		public float Scale;

		[ProtoMember(36)]
		public SerializableRange Slope;

		[ProtoMember(37)]
		public float Transition;
	}
}
