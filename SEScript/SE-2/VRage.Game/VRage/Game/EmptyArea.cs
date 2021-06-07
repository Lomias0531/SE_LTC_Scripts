using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public struct EmptyArea
	{
		protected class VRage_Game_EmptyArea_003C_003EPosition_003C_003EAccessor : IMemberAccessor<EmptyArea, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref EmptyArea owner, in Vector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref EmptyArea owner, out Vector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_EmptyArea_003C_003ERadius_003C_003EAccessor : IMemberAccessor<EmptyArea, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref EmptyArea owner, in float value)
			{
				owner.Radius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref EmptyArea owner, out float value)
			{
				value = owner.Radius;
			}
		}

		private class VRage_Game_EmptyArea_003C_003EActor : IActivator, IActivator<EmptyArea>
		{
			private sealed override object CreateInstance()
			{
				return default(EmptyArea);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override EmptyArea CreateInstance()
			{
				return (EmptyArea)(object)default(EmptyArea);
			}

			EmptyArea IActivator<EmptyArea>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(19)]
		public Vector3D Position;

		[ProtoMember(22)]
		public float Radius;
	}
}
