using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game.ModAPI
{
	[ProtoContract]
	public struct MyHitInfo
	{
		protected class VRage_Game_ModAPI_MyHitInfo_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyHitInfo, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHitInfo owner, in Vector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHitInfo owner, out Vector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_ModAPI_MyHitInfo_003C_003ENormal_003C_003EAccessor : IMemberAccessor<MyHitInfo, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHitInfo owner, in Vector3 value)
			{
				owner.Normal = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHitInfo owner, out Vector3 value)
			{
				value = owner.Normal;
			}
		}

		protected class VRage_Game_ModAPI_MyHitInfo_003C_003EVelocity_003C_003EAccessor : IMemberAccessor<MyHitInfo, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHitInfo owner, in Vector3D value)
			{
				owner.Velocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHitInfo owner, out Vector3D value)
			{
				value = owner.Velocity;
			}
		}

		protected class VRage_Game_ModAPI_MyHitInfo_003C_003EShapeKey_003C_003EAccessor : IMemberAccessor<MyHitInfo, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHitInfo owner, in uint value)
			{
				owner.ShapeKey = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHitInfo owner, out uint value)
			{
				value = owner.ShapeKey;
			}
		}

		private class VRage_Game_ModAPI_MyHitInfo_003C_003EActor : IActivator, IActivator<MyHitInfo>
		{
			private sealed override object CreateInstance()
			{
				return default(MyHitInfo);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHitInfo CreateInstance()
			{
				return (MyHitInfo)(object)default(MyHitInfo);
			}

			MyHitInfo IActivator<MyHitInfo>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public Vector3D Position;

		[ProtoMember(4)]
		public Vector3 Normal;

		[ProtoMember(7)]
		public Vector3D Velocity;

		[ProtoMember(10)]
		public uint ShapeKey;
	}
}
