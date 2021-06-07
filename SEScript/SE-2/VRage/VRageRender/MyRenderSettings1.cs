using System;

namespace VRageRender
{
	/// <summary>
	/// Naming convention from DX. Newer version for Dx11 render.
	/// Put only settings that player can control (either directly or indirectly) using options here.
	/// Don't put debug crap here!
	/// </summary>
	public struct MyRenderSettings1 : IEquatable<MyRenderSettings1>
	{
		public bool HqTarget;

		public MyAntialiasingMode AntialiasingMode;

		public bool AmbientOcclusionEnabled;

		public MyShadowsQuality ShadowQuality;

		public MyTextureQuality TextureQuality;

		public MyTextureAnisoFiltering AnisotropicFiltering;

		public MyRenderQualityEnum ModelQuality;

		public MyRenderQualityEnum VoxelQuality;

		public bool HqDepth;

		public MyRenderQualityEnum VoxelShaderQuality;

		public MyRenderQualityEnum AlphaMaskedShaderQuality;

		public MyRenderQualityEnum AtmosphereShaderQuality;

		public float GrassDrawDistance;

		public float GrassDensityFactor;

		public float DistanceFade;

		public override int GetHashCode()
		{
			return ((ValueType)this).GetHashCode();
		}

		bool IEquatable<MyRenderSettings1>.Equals(MyRenderSettings1 other)
		{
			return Equals(ref other);
		}

		public override bool Equals(object other)
		{
			MyRenderSettings1 other2 = (MyRenderSettings1)other;
			return Equals(ref other2);
		}

		public bool Equals(ref MyRenderSettings1 other)
		{
			if (GrassDensityFactor.IsEqual(other.GrassDensityFactor, 0.1f) && GrassDrawDistance.IsEqual(other.GrassDrawDistance, 2f) && ModelQuality == other.ModelQuality && VoxelQuality == other.VoxelQuality && AntialiasingMode == other.AntialiasingMode && ShadowQuality == other.ShadowQuality && AmbientOcclusionEnabled == other.AmbientOcclusionEnabled && TextureQuality == other.TextureQuality && AnisotropicFiltering == other.AnisotropicFiltering && HqDepth == other.HqDepth && VoxelShaderQuality == other.VoxelShaderQuality && AlphaMaskedShaderQuality == other.AlphaMaskedShaderQuality && AtmosphereShaderQuality == other.AtmosphereShaderQuality)
			{
				return DistanceFade.IsEqual(other.DistanceFade, 4f);
			}
			return false;
		}
	}
}
