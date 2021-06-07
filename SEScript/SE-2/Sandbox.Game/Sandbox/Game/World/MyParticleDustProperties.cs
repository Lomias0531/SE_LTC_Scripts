using VRageMath;

namespace Sandbox.Game.World
{
	internal class MyParticleDustProperties
	{
		public bool Enabled;

		public float DustBillboardRadius = 3f;

		public float DustFieldCountInDirectionHalf = 5f;

		public float DistanceBetween = 180f;

		public float AnimSpeed = 0.004f;

		public Color Color = Color.White;

		public int Texture;

		public MyParticleDustProperties InterpolateWith(MyParticleDustProperties otherProperties, float interpolator)
		{
			return new MyParticleDustProperties
			{
				DustFieldCountInDirectionHalf = MathHelper.Lerp(DustFieldCountInDirectionHalf, otherProperties.DustFieldCountInDirectionHalf, interpolator),
				DistanceBetween = MathHelper.Lerp(DistanceBetween, otherProperties.DistanceBetween, interpolator),
				AnimSpeed = MathHelper.Lerp(AnimSpeed, otherProperties.AnimSpeed, interpolator),
				Color = Color.Lerp(Color, otherProperties.Color, interpolator),
				Enabled = (MathHelper.Lerp(Enabled ? 1 : 0, otherProperties.Enabled ? 1 : 0, interpolator) > 0.5f),
				DustBillboardRadius = ((interpolator <= 0.5f) ? DustBillboardRadius : otherProperties.DustBillboardRadius),
				Texture = ((interpolator <= 0.5f) ? Texture : otherProperties.Texture)
			};
		}
	}
}
