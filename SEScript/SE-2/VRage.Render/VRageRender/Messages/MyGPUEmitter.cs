using VRageMath;

namespace VRageRender.Messages
{
	public struct MyGPUEmitter
	{
		public uint GID;

		public float ParticlesPerSecond;

		public float ParticlesPerFrame;

		public string AtlasTexture;

		public Vector2I AtlasDimension;

		public int AtlasFrameOffset;

		public int AtlasFrameModulo;

		public Vector3D WorldPosition;

		public float CameraBias;

		public uint EffectID;

		public uint ParentID;

		public float GravityFactor;

		public float DistanceMaxSqr;

		public MyGPUEmitterData Data;

		public int MaxParticles()
		{
			return (int)(ParticlesPerSecond * Data.ParticleLifeSpan) + 1;
		}
	}
}
