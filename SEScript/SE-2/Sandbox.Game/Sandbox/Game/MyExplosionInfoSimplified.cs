using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game
{
	[Serializable]
	public struct MyExplosionInfoSimplified
	{
		protected class Sandbox_Game_MyExplosionInfoSimplified_003C_003EDamage_003C_003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyExplosionInfoSimplified owner, in float value)
			{
				owner.Damage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyExplosionInfoSimplified owner, out float value)
			{
				value = owner.Damage;
			}
		}

		protected class Sandbox_Game_MyExplosionInfoSimplified_003C_003ECenter_003C_003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyExplosionInfoSimplified owner, in Vector3D value)
			{
				owner.Center = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyExplosionInfoSimplified owner, out Vector3D value)
			{
				value = owner.Center;
			}
		}

		protected class Sandbox_Game_MyExplosionInfoSimplified_003C_003ERadius_003C_003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyExplosionInfoSimplified owner, in float value)
			{
				owner.Radius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyExplosionInfoSimplified owner, out float value)
			{
				value = owner.Radius;
			}
		}

		protected class Sandbox_Game_MyExplosionInfoSimplified_003C_003EType_003C_003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, MyExplosionTypeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyExplosionInfoSimplified owner, in MyExplosionTypeEnum value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyExplosionInfoSimplified owner, out MyExplosionTypeEnum value)
			{
				value = owner.Type;
			}
		}

		protected class Sandbox_Game_MyExplosionInfoSimplified_003C_003EFlags_003C_003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, MyExplosionFlags>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyExplosionInfoSimplified owner, in MyExplosionFlags value)
			{
				owner.Flags = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyExplosionInfoSimplified owner, out MyExplosionFlags value)
			{
				value = owner.Flags;
			}
		}

		protected class Sandbox_Game_MyExplosionInfoSimplified_003C_003EVoxelCenter_003C_003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyExplosionInfoSimplified owner, in Vector3D value)
			{
				owner.VoxelCenter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyExplosionInfoSimplified owner, out Vector3D value)
			{
				value = owner.VoxelCenter;
			}
		}

		protected class Sandbox_Game_MyExplosionInfoSimplified_003C_003EParticleScale_003C_003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyExplosionInfoSimplified owner, in float value)
			{
				owner.ParticleScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyExplosionInfoSimplified owner, out float value)
			{
				value = owner.ParticleScale;
			}
		}

		protected class Sandbox_Game_MyExplosionInfoSimplified_003C_003EVelocity_003C_003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyExplosionInfoSimplified owner, in Vector3 value)
			{
				owner.Velocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyExplosionInfoSimplified owner, out Vector3 value)
			{
				value = owner.Velocity;
			}
		}

		public float Damage;

		public Vector3D Center;

		public float Radius;

		public MyExplosionTypeEnum Type;

		public MyExplosionFlags Flags;

		public Vector3D VoxelCenter;

		public float ParticleScale;

		public Vector3 Velocity;
	}
}
