using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	public class MyTransparentMaterial
	{
		public readonly MyStringId Id;

		public readonly MyTransparentMaterialTextureType TextureType;

		public readonly string Texture;

		public readonly string GlossTexture;

		public readonly bool CanBeAffectedByOtherLights;

		public readonly bool AlphaMistingEnable;

		public readonly bool UseAtlas;

		public readonly float AlphaMistingStart;

		public readonly float AlphaMistingEnd;

		public readonly float SoftParticleDistanceScale;

		public readonly float AlphaSaturation;

		public readonly bool AlphaCutout;

		public Vector2 UVOffset;

		public Vector2 UVSize;

		public readonly Vector2I TargetSize;

		public Vector4 Color;

		public Vector4 ColorAdd;

		public Vector4 ShadowMultiplier;

		public Vector4 LightMultiplier;

		public readonly float Reflectivity;

		public readonly float Fresnel;

		public readonly float ReflectionShadow;

		public readonly float Gloss;

		public readonly float GlossTextureAdd;

		public readonly float SpecularColorFactor;

		public readonly bool IsFlareOccluder;

		public readonly bool TriangleFaceCulling;

		public MyTransparentMaterial(MyStringId id, MyTransparentMaterialTextureType textureType, string texture, string glossTexture, float softParticleDistanceScale, bool canBeAffectedByOtherLights, bool alphaMistingEnable, Vector4 color, Vector4 colorAdd, Vector4 shadowMultiplier, Vector4 lightMultiplier, bool isFlareOccluder, bool triangleFaceCulling, bool useAtlas = false, float alphaMistingStart = 1f, float alphaMistingEnd = 4f, float alphaSaturation = 1f, float reflectivity = 0f, bool alphaCutout = false, Vector2I? targetSize = null, float fresnel = 1f, float reflectionShadow = 0.1f, float gloss = 0.4f, float glossTextureAdd = 0.55f, float specularColorFactor = 20f)
		{
			Id = id;
			TextureType = textureType;
			Texture = texture;
			GlossTexture = glossTexture;
			SoftParticleDistanceScale = softParticleDistanceScale;
			CanBeAffectedByOtherLights = canBeAffectedByOtherLights;
			AlphaMistingEnable = alphaMistingEnable;
			UseAtlas = useAtlas;
			AlphaMistingStart = alphaMistingStart;
			AlphaMistingEnd = alphaMistingEnd;
			AlphaSaturation = alphaSaturation;
			AlphaCutout = alphaCutout;
			Color = color.ToLinearRGB().PremultiplyColor();
			ColorAdd = colorAdd.ToLinearRGB();
			ShadowMultiplier = shadowMultiplier;
			LightMultiplier = lightMultiplier;
			IsFlareOccluder = isFlareOccluder;
			TriangleFaceCulling = triangleFaceCulling;
			Reflectivity = reflectivity;
			Fresnel = fresnel;
			ReflectionShadow = reflectionShadow;
			Gloss = gloss;
			GlossTextureAdd = glossTextureAdd;
			SpecularColorFactor = specularColorFactor;
			TargetSize = (targetSize ?? new Vector2I(-1, -1));
			UVOffset = new Vector2(0f, 0f);
			UVSize = new Vector2(1f, 1f);
		}
	}
}
