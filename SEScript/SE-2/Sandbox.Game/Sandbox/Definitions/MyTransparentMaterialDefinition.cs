using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_TransparentMaterialDefinition), null)]
	public class MyTransparentMaterialDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyTransparentMaterialDefinition_003C_003EActor : IActivator, IActivator<MyTransparentMaterialDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTransparentMaterialDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTransparentMaterialDefinition CreateInstance()
			{
				return new MyTransparentMaterialDefinition();
			}

			MyTransparentMaterialDefinition IActivator<MyTransparentMaterialDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Texture;

		public string GlossTexture;

		public MyTransparentMaterialTextureType TextureType;

		public bool CanBeAffectedByLights;

		public bool AlphaMistingEnable;

		public bool UseAtlas;

		public float AlphaMistingStart;

		public float AlphaMistingEnd;

		public float SoftParticleDistanceScale;

		public float AlphaSaturation;

		public float Reflectivity;

		public float Fresnel;

		public bool IsFlareOccluder;

		public bool TriangleFaceCulling;

		public Vector4 Color = Vector4.One;

		public Vector4 ColorAdd = Vector4.Zero;

		public Vector4 ShadowMultiplier = Vector4.Zero;

		public Vector4 LightMultiplier = Vector4.One * 0.1f;

		public bool AlphaCutout;

		public Vector2I TargetSize;

		public float ReflectionShadow = 0.1f;

		public float Gloss = 0.4f;

		public float GlossTextureAdd = 0.55f;

		public float SpecularColorFactor = 20f;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TransparentMaterialDefinition myObjectBuilder_TransparentMaterialDefinition = builder as MyObjectBuilder_TransparentMaterialDefinition;
			Texture = myObjectBuilder_TransparentMaterialDefinition.Texture;
			GlossTexture = myObjectBuilder_TransparentMaterialDefinition.GlossTexture;
			if (Texture == null)
			{
				Texture = string.Empty;
			}
			TextureType = myObjectBuilder_TransparentMaterialDefinition.TextureType;
			CanBeAffectedByLights = myObjectBuilder_TransparentMaterialDefinition.CanBeAffectedByOtherLights;
			AlphaMistingEnable = myObjectBuilder_TransparentMaterialDefinition.AlphaMistingEnable;
			UseAtlas = myObjectBuilder_TransparentMaterialDefinition.UseAtlas;
			AlphaMistingStart = myObjectBuilder_TransparentMaterialDefinition.AlphaMistingStart;
			AlphaMistingEnd = myObjectBuilder_TransparentMaterialDefinition.AlphaMistingEnd;
			SoftParticleDistanceScale = myObjectBuilder_TransparentMaterialDefinition.SoftParticleDistanceScale;
			AlphaSaturation = myObjectBuilder_TransparentMaterialDefinition.AlphaSaturation;
			Reflectivity = myObjectBuilder_TransparentMaterialDefinition.Reflectivity;
			Fresnel = myObjectBuilder_TransparentMaterialDefinition.Fresnel;
			IsFlareOccluder = myObjectBuilder_TransparentMaterialDefinition.IsFlareOccluder;
			TriangleFaceCulling = myObjectBuilder_TransparentMaterialDefinition.TriangleFaceCulling;
			Color = myObjectBuilder_TransparentMaterialDefinition.Color;
			ColorAdd = myObjectBuilder_TransparentMaterialDefinition.ColorAdd;
			ShadowMultiplier = myObjectBuilder_TransparentMaterialDefinition.ShadowMultiplier;
			LightMultiplier = myObjectBuilder_TransparentMaterialDefinition.LightMultiplier;
			AlphaCutout = myObjectBuilder_TransparentMaterialDefinition.AlphaCutout;
			TargetSize = myObjectBuilder_TransparentMaterialDefinition.TargetSize;
			ReflectionShadow = myObjectBuilder_TransparentMaterialDefinition.ReflectionShadow;
			Gloss = myObjectBuilder_TransparentMaterialDefinition.Gloss;
			GlossTextureAdd = myObjectBuilder_TransparentMaterialDefinition.GlossTextureAdd;
			SpecularColorFactor = myObjectBuilder_TransparentMaterialDefinition.SpecularColorFactor;
		}
	}
}
