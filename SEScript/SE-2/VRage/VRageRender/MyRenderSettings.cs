using VRageMath;

namespace VRageRender
{
	/// <summary>
	/// Settings for whole render. To make settings per draw, use RenderSetup
	/// </summary>
	public struct MyRenderSettings
	{
		public static readonly MyRenderSettings Default;

		public bool EnableAnsel;

		public bool EnableAnselWithSprites;

		public bool UseGeometryArrayTextures;

		public bool EnableShadows;

		public bool DebugRenderClipmapCells;

		public bool DebugTextureLodColor;

		public bool Wireframe;

		public bool DisplayGbufferColor;

		public bool DisplayGbufferAlbedo;

		public bool DisplayGbufferNormal;

		public bool DisplayGbufferNormalView;

		public bool DisplayGbufferGlossiness;

		public bool DisplayGbufferMetalness;

		public bool DisplayGbufferLOD;

		public bool DisplayMipmap;

		public bool DisplayGbufferAO;

		public bool DisplayEmissive;

		public bool DisplayEdgeMask;

		public bool DisplayNDotL;

		public bool DisplayDepth;

		public bool DisplayReprojectedDepth;

		public bool DisplayStencil;

		public bool DisplayEnvProbe;

		public bool DisplayEnvProbeOriginal;

		public bool DisplayEnvProbeFar;

		public bool DisplayEnvProbeFarOriginal;

		public bool DisplayEnvProbeIntensities;

		public int DisplayEnvProbeMipLevel;

		public bool DisplayBloomFilter;

		public bool DisplayBloomMin;

		public bool DisplayTransparencyHeatMap;

		public bool DisplayTransparencyHeatMapInGrayscale;

		public bool DisplayAO;

		public bool DisplayAmbientDiffuse;

		public bool DisplayAmbientSpecular;

		public bool DisplayIDs;

		public bool DisplayAabbs;

		public bool DisplayTreeAabbs;

		public bool DrawMeshes;

		public bool DrawInstancedMeshes;

		public bool DrawTransparentModels;

		public bool DrawTransparentModelsInstanced;

		public bool DrawDynamicInstances;

		public bool DrawGlass;

		public bool DrawAlphamasked;

		public bool DrawBillboards;

		public bool DrawImpostors;

		public bool DrawVoxels;

		public bool DrawMergeInstanced;

		public bool DrawNonMergeInstanced;

		public bool DrawOcclusionQueriesDebug;

		public bool DrawGroupOcclusionQueriesDebug;

		public bool DrawGroups;

		public int CullGroupsThreshold;

		public bool UseIncrementalCulling;

		public int IncrementalCullFrames;

		public float IncrementalCullingTreeFallbackThreshold;

		public bool FreezeTerrainQueries;

		public float GrassGeometryScalingNearDistance;

		public float GrassGeometryScalingFarDistance;

		public float GrassGeometryDistanceScalingFactor;

		public bool DisplayShadowsWithDebug;

		public bool DisplayShadowVolumes;

		public bool DisplayShadowSplitsWithDebug;

		public bool DisplayParticleShadowSplitsWithDebug;

		public bool DrawCascadeShadowTextures;

		public bool DrawSpotShadowTextures;

		public bool ShadowCameraFrozen;

		public int ZoomCascadeTextureIndex;

		public int RwTexturePool_FramesToPreserveTextures;

		public bool UseDebugMissingFileTextures;

		public bool IgnoreOcclusionQueries;

		public bool DisableOcclusionQueries;

		public bool ShadowCascadeUsageBasedSkip;

		public bool DisableShadowCascadeOcclusionQueries;

		public int RenderThreadCount;

		public bool ForceImmediateContext;

		public bool RenderThreadHighPriority;

		public bool ForceSlowCPU;

		public bool DisplayHistogram;

		public bool DisplayHdrIntensity;

		public bool DisplayHDRTest;

		public float WindStrength;

		public float WindAzimuth;

		public bool DebugDrawDecals;

		public bool OffscreenSpritesRendering;

		public MyRenderSettings1 User;

		public Vector4 TransparentColorMultiplier;

		public float TransparentReflectivityMultiplier;

		public float TransparentFresnelMultiplier;

		public float TransparentGlossMultiplier;

		public bool SkipGlobalROWMUpdate;

		public bool DisplayNormals;

		public bool HDREnabled;

		public float FlaresIntensity;

		public bool RenderBlocksToEnvProbe;

		static MyRenderSettings()
		{
			Default = new MyRenderSettings
			{
				EnableAnsel = false,
				EnableAnselWithSprites = false,
				UseGeometryArrayTextures = false,
				FlaresIntensity = 1f,
				EnableShadows = true,
				DebugRenderClipmapCells = false,
				DebugTextureLodColor = false,
				Wireframe = false,
				DisplayGbufferColor = false,
				DisplayGbufferAlbedo = false,
				DisplayGbufferNormal = false,
				DisplayGbufferNormalView = false,
				DisplayGbufferGlossiness = false,
				DisplayGbufferMetalness = false,
				DisplayGbufferLOD = false,
				DisplayMipmap = false,
				DisplayGbufferAO = false,
				DisplayEmissive = false,
				DisplayEdgeMask = false,
				DisplayNDotL = false,
				DisplayDepth = false,
				DisplayReprojectedDepth = false,
				DisplayStencil = false,
				DisplayEnvProbe = false,
				DisplayBloomFilter = false,
				DisplayBloomMin = false,
				DisplayAO = false,
				DisplayAmbientDiffuse = false,
				DisplayAmbientSpecular = false,
				DisplayIDs = false,
				DisplayTreeAabbs = false,
				DisplayAabbs = false,
				DrawMeshes = true,
				DrawInstancedMeshes = true,
				DrawTransparentModels = true,
				DrawTransparentModelsInstanced = true,
				DrawDynamicInstances = true,
				DrawGlass = true,
				DrawAlphamasked = true,
				DrawImpostors = true,
				DrawBillboards = true,
				DrawVoxels = true,
				DrawMergeInstanced = false,
				DrawNonMergeInstanced = true,
				DrawGroups = true,
				CullGroupsThreshold = 512,
				UseIncrementalCulling = true,
				IncrementalCullFrames = 10,
				IncrementalCullingTreeFallbackThreshold = 0.5f,
				FreezeTerrainQueries = false,
				GrassGeometryScalingNearDistance = 50f,
				GrassGeometryScalingFarDistance = 350f,
				GrassGeometryDistanceScalingFactor = 5f,
				DisplayShadowSplitsWithDebug = false,
				DisplayParticleShadowSplitsWithDebug = false,
				ZoomCascadeTextureIndex = -1,
				DrawCascadeShadowTextures = false,
				DrawSpotShadowTextures = false,
				RwTexturePool_FramesToPreserveTextures = 16,
				UseDebugMissingFileTextures = false,
				IgnoreOcclusionQueries = false,
				RenderThreadCount = int.MaxValue,
				ForceImmediateContext = false,
				RenderThreadHighPriority = false,
				DisplayHistogram = false,
				DisplayHDRTest = false,
				WindStrength = 0.3f,
				WindAzimuth = 0f,
				DebugDrawDecals = false,
				OffscreenSpritesRendering = false,
				TransparentColorMultiplier = Vector4.One,
				TransparentReflectivityMultiplier = 1f,
				TransparentFresnelMultiplier = 1f,
				TransparentGlossMultiplier = 1f,
				HDREnabled = true,
				RenderBlocksToEnvProbe = true,
				ShadowCascadeUsageBasedSkip = true
			};
		}
	}
}
