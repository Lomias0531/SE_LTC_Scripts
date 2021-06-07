using Medieval.ObjectBuilders.Definitions;
using System;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
	[MyDefinitionType(typeof(MyObjectBuilder_VoxelMaterialDefinition), null)]
	public class MyVoxelMaterialDefinition : MyDefinitionBase
	{
		private class VRage_Game_MyVoxelMaterialDefinition_003C_003EActor : IActivator, IActivator<MyVoxelMaterialDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelMaterialDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelMaterialDefinition CreateInstance()
			{
				return new MyVoxelMaterialDefinition();
			}

			MyVoxelMaterialDefinition IActivator<MyVoxelMaterialDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static byte m_indexCounter;

		public string MaterialTypeName;

		public string MinedOre;

		public float MinedOreRatio;

		public bool CanBeHarvested;

		public bool IsRare;

		public int MinVersion;

		public int MaxVersion;

		public bool SpawnsInAsteroids;

		public bool SpawnsFromMeteorites;

		public string VoxelHandPreview;

		public float Friction;

		public float Restitution;

		public string LandingEffect;

		public int AsteroidGeneratorSpawnProbabilityMultiplier;

		public string BareVariant;

		private MyStringId m_materialTypeNameIdCache;

		private MyStringHash m_materialTypeNameHashCache;

		public MyStringHash DamagedMaterial;

		public MyRenderVoxelMaterialData RenderParams;

		public Vector3? ColorKey;

		public MyStringId MaterialTypeNameId
		{
			get
			{
				if (m_materialTypeNameIdCache == default(MyStringId))
				{
					m_materialTypeNameIdCache = MyStringId.GetOrCompute(MaterialTypeName);
				}
				return m_materialTypeNameIdCache;
			}
		}

		public MyStringHash MaterialTypeNameHash
		{
			get
			{
				if (m_materialTypeNameHashCache == default(MyStringHash))
				{
					m_materialTypeNameHashCache = MyStringHash.GetOrCompute(MaterialTypeName);
				}
				return m_materialTypeNameHashCache;
			}
		}

		/// <summary>
		/// Value generated at runtime to ensure correctness. Do not serialize or deserialize.
		/// This is what the old cast to int used to result into, but now numbers depend on order in XML file.
		/// TODO Serialize to XML and ensure upon loading that these values are starting from 0 and continuous.
		/// </summary>
		public byte Index
		{
			get;
			set;
		}

		public bool HasDamageMaterial => DamagedMaterial != MyStringHash.NullOrEmpty;

		public string Icon
		{
			get
			{
				if (Icons != null && Icons.Length != 0)
				{
					return Icons[0];
				}
				return RenderParams.TextureSets[0].ColorMetalXZnY;
			}
		}

		public void AssignIndex()
		{
			Index = m_indexCounter++;
			RenderParams.Index = Index;
		}

		public static void ResetIndexing()
		{
			m_indexCounter = 0;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase ob)
		{
			base.Init(ob);
			MyObjectBuilder_Dx11VoxelMaterialDefinition myObjectBuilder_Dx11VoxelMaterialDefinition = ob as MyObjectBuilder_Dx11VoxelMaterialDefinition;
			MaterialTypeName = myObjectBuilder_Dx11VoxelMaterialDefinition.MaterialTypeName;
			MinedOre = myObjectBuilder_Dx11VoxelMaterialDefinition.MinedOre;
			MinedOreRatio = myObjectBuilder_Dx11VoxelMaterialDefinition.MinedOreRatio;
			CanBeHarvested = myObjectBuilder_Dx11VoxelMaterialDefinition.CanBeHarvested;
			IsRare = myObjectBuilder_Dx11VoxelMaterialDefinition.IsRare;
			SpawnsInAsteroids = myObjectBuilder_Dx11VoxelMaterialDefinition.SpawnsInAsteroids;
			SpawnsFromMeteorites = myObjectBuilder_Dx11VoxelMaterialDefinition.SpawnsFromMeteorites;
			VoxelHandPreview = myObjectBuilder_Dx11VoxelMaterialDefinition.VoxelHandPreview;
			MinVersion = myObjectBuilder_Dx11VoxelMaterialDefinition.MinVersion;
			MaxVersion = myObjectBuilder_Dx11VoxelMaterialDefinition.MaxVersion;
			DamagedMaterial = MyStringHash.GetOrCompute(myObjectBuilder_Dx11VoxelMaterialDefinition.DamagedMaterial);
			Friction = myObjectBuilder_Dx11VoxelMaterialDefinition.Friction;
			Restitution = myObjectBuilder_Dx11VoxelMaterialDefinition.Restitution;
			LandingEffect = myObjectBuilder_Dx11VoxelMaterialDefinition.LandingEffect;
			BareVariant = myObjectBuilder_Dx11VoxelMaterialDefinition.BareVariant;
			AsteroidGeneratorSpawnProbabilityMultiplier = myObjectBuilder_Dx11VoxelMaterialDefinition.AsteroidGeneratorSpawnProbabilityMultiplier;
			if (myObjectBuilder_Dx11VoxelMaterialDefinition.ColorKey.HasValue)
			{
				ColorKey = ColorExtensions.ColorToHSV(myObjectBuilder_Dx11VoxelMaterialDefinition.ColorKey.Value);
			}
			RenderParams.Index = Index;
			RenderParams.TextureSets = new MyRenderVoxelMaterialData.TextureSet[3];
			RenderParams.TextureSets[0].ColorMetalXZnY = myObjectBuilder_Dx11VoxelMaterialDefinition.ColorMetalXZnY;
			RenderParams.TextureSets[0].ColorMetalY = myObjectBuilder_Dx11VoxelMaterialDefinition.ColorMetalY;
			RenderParams.TextureSets[0].NormalGlossXZnY = myObjectBuilder_Dx11VoxelMaterialDefinition.NormalGlossXZnY;
			RenderParams.TextureSets[0].NormalGlossY = myObjectBuilder_Dx11VoxelMaterialDefinition.NormalGlossY;
			RenderParams.TextureSets[0].ExtXZnY = myObjectBuilder_Dx11VoxelMaterialDefinition.ExtXZnY;
			RenderParams.TextureSets[0].ExtY = myObjectBuilder_Dx11VoxelMaterialDefinition.ExtY;
			RenderParams.TextureSets[0].Check();
			RenderParams.TextureSets[1].ColorMetalXZnY = (myObjectBuilder_Dx11VoxelMaterialDefinition.ColorMetalXZnYFar1 ?? RenderParams.TextureSets[0].ColorMetalXZnY);
			RenderParams.TextureSets[1].ColorMetalY = (myObjectBuilder_Dx11VoxelMaterialDefinition.ColorMetalYFar1 ?? RenderParams.TextureSets[1].ColorMetalXZnY);
			RenderParams.TextureSets[1].NormalGlossXZnY = (myObjectBuilder_Dx11VoxelMaterialDefinition.NormalGlossXZnYFar1 ?? RenderParams.TextureSets[0].NormalGlossXZnY);
			RenderParams.TextureSets[1].NormalGlossY = (myObjectBuilder_Dx11VoxelMaterialDefinition.NormalGlossYFar1 ?? RenderParams.TextureSets[1].NormalGlossXZnY);
			RenderParams.TextureSets[1].ExtXZnY = (myObjectBuilder_Dx11VoxelMaterialDefinition.ExtXZnYFar1 ?? RenderParams.TextureSets[0].ExtXZnY);
			RenderParams.TextureSets[1].ExtY = (myObjectBuilder_Dx11VoxelMaterialDefinition.ExtYFar1 ?? RenderParams.TextureSets[1].ExtXZnY);
			RenderParams.TextureSets[2].ColorMetalXZnY = (myObjectBuilder_Dx11VoxelMaterialDefinition.ColorMetalXZnYFar2 ?? RenderParams.TextureSets[1].ColorMetalXZnY);
			RenderParams.TextureSets[2].ColorMetalY = (myObjectBuilder_Dx11VoxelMaterialDefinition.ColorMetalYFar2 ?? RenderParams.TextureSets[2].ColorMetalXZnY);
			RenderParams.TextureSets[2].NormalGlossXZnY = (myObjectBuilder_Dx11VoxelMaterialDefinition.NormalGlossXZnYFar2 ?? RenderParams.TextureSets[1].NormalGlossXZnY);
			RenderParams.TextureSets[2].NormalGlossY = (myObjectBuilder_Dx11VoxelMaterialDefinition.NormalGlossYFar2 ?? RenderParams.TextureSets[2].NormalGlossXZnY);
			RenderParams.TextureSets[2].ExtXZnY = (myObjectBuilder_Dx11VoxelMaterialDefinition.ExtXZnYFar2 ?? RenderParams.TextureSets[1].ExtXZnY);
			RenderParams.TextureSets[2].ExtY = (myObjectBuilder_Dx11VoxelMaterialDefinition.ExtYFar2 ?? RenderParams.TextureSets[2].ExtXZnY);
			RenderParams.StandardTilingSetup.InitialScale = myObjectBuilder_Dx11VoxelMaterialDefinition.InitialScale;
			RenderParams.StandardTilingSetup.ScaleMultiplier = myObjectBuilder_Dx11VoxelMaterialDefinition.ScaleMultiplier;
			RenderParams.StandardTilingSetup.InitialDistance = myObjectBuilder_Dx11VoxelMaterialDefinition.InitialDistance;
			RenderParams.StandardTilingSetup.DistanceMultiplier = myObjectBuilder_Dx11VoxelMaterialDefinition.DistanceMultiplier;
			RenderParams.StandardTilingSetup.TilingScale = myObjectBuilder_Dx11VoxelMaterialDefinition.TilingScale;
			RenderParams.StandardTilingSetup.Far1Distance = myObjectBuilder_Dx11VoxelMaterialDefinition.Far1Distance;
			RenderParams.StandardTilingSetup.Far2Distance = myObjectBuilder_Dx11VoxelMaterialDefinition.Far2Distance;
			RenderParams.StandardTilingSetup.Far3Distance = myObjectBuilder_Dx11VoxelMaterialDefinition.Far3Distance;
			RenderParams.StandardTilingSetup.Far1Scale = myObjectBuilder_Dx11VoxelMaterialDefinition.Far1Scale;
			RenderParams.StandardTilingSetup.Far2Scale = myObjectBuilder_Dx11VoxelMaterialDefinition.Far2Scale;
			RenderParams.StandardTilingSetup.Far3Scale = myObjectBuilder_Dx11VoxelMaterialDefinition.Far3Scale;
			RenderParams.StandardTilingSetup.ExtensionDetailScale = myObjectBuilder_Dx11VoxelMaterialDefinition.ExtDetailScale;
			if (myObjectBuilder_Dx11VoxelMaterialDefinition.SimpleTilingSetup == null)
			{
				RenderParams.SimpleTilingSetup = RenderParams.StandardTilingSetup;
			}
			else
			{
				TilingSetup simpleTilingSetup = myObjectBuilder_Dx11VoxelMaterialDefinition.SimpleTilingSetup;
				RenderParams.SimpleTilingSetup.InitialScale = simpleTilingSetup.InitialScale;
				RenderParams.SimpleTilingSetup.ScaleMultiplier = simpleTilingSetup.ScaleMultiplier;
				RenderParams.SimpleTilingSetup.InitialDistance = simpleTilingSetup.InitialDistance;
				RenderParams.SimpleTilingSetup.DistanceMultiplier = simpleTilingSetup.DistanceMultiplier;
				RenderParams.SimpleTilingSetup.TilingScale = simpleTilingSetup.TilingScale;
				RenderParams.SimpleTilingSetup.Far1Distance = simpleTilingSetup.Far1Distance;
				RenderParams.SimpleTilingSetup.Far2Distance = simpleTilingSetup.Far2Distance;
				RenderParams.SimpleTilingSetup.Far3Distance = simpleTilingSetup.Far3Distance;
				RenderParams.SimpleTilingSetup.Far1Scale = simpleTilingSetup.Far1Scale;
				RenderParams.SimpleTilingSetup.Far2Scale = simpleTilingSetup.Far2Scale;
				RenderParams.SimpleTilingSetup.Far3Scale = simpleTilingSetup.Far3Scale;
				RenderParams.SimpleTilingSetup.ExtensionDetailScale = simpleTilingSetup.ExtDetailScale;
			}
			RenderParams.Far3Color = myObjectBuilder_Dx11VoxelMaterialDefinition.Far3Color;
			MyRenderFoliageData value = default(MyRenderFoliageData);
			if (myObjectBuilder_Dx11VoxelMaterialDefinition.FoliageColorTextureArray != null)
			{
				value.Type = (MyFoliageType)myObjectBuilder_Dx11VoxelMaterialDefinition.FoliageType;
				value.Density = myObjectBuilder_Dx11VoxelMaterialDefinition.FoliageDensity;
				string[] foliageColorTextureArray = myObjectBuilder_Dx11VoxelMaterialDefinition.FoliageColorTextureArray;
				string[] foliageNormalTextureArray = myObjectBuilder_Dx11VoxelMaterialDefinition.FoliageNormalTextureArray;
				int val;
				if (foliageNormalTextureArray != null)
				{
					if (foliageColorTextureArray.Length != foliageNormalTextureArray.Length)
					{
						MyLog.Default.Warning("Legacy foliage format has different size normal and color arrays, only the minimum length will be used.");
					}
					val = Math.Min(foliageColorTextureArray.Length, foliageNormalTextureArray.Length);
				}
				else
				{
					val = foliageColorTextureArray.Length;
				}
				val = Math.Min(val, 16);
				value.Entries = new MyRenderFoliageData.FoliageEntry[val];
				for (int i = 0; i < val; i++)
				{
					value.Entries[i] = new MyRenderFoliageData.FoliageEntry
					{
						ColorAlphaTexture = foliageColorTextureArray[i],
						NormalGlossTexture = ((foliageNormalTextureArray != null) ? foliageNormalTextureArray[i] : null),
						Probability = 1f,
						Size = myObjectBuilder_Dx11VoxelMaterialDefinition.FoliageScale,
						SizeVariation = myObjectBuilder_Dx11VoxelMaterialDefinition.FoliageRandomRescaleMult
					};
				}
			}
			if (value.Density > 0f)
			{
				RenderParams.Foliage = value;
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			return null;
		}

		public void UpdateVoxelMaterial()
		{
			MyRenderProxy.UpdateRenderVoxelMaterials(new MyRenderVoxelMaterialData[1]
			{
				RenderParams
			});
		}
	}
}
