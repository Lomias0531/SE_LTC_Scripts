using System;
using VRage.Collections;
using VRageRender;

namespace VRage.Game.ModAPI
{
	public interface IMyConfig
	{
		bool? AmbientOcclusionEnabled
		{
			get;
		}

		MyTextureAnisoFiltering? AnisotropicFiltering
		{
			get;
		}

		MyAntialiasingMode? AntialiasingMode
		{
			get;
		}

		bool CaptureMouse
		{
			get;
		}

		bool ControlsHints
		{
			get;
		}

		int CubeBuilderBuildingMode
		{
			get;
		}

		bool CubeBuilderUseSymmetry
		{
			get;
		}

		HashSetReader<ulong> DontSendVoicePlayers
		{
			get;
		}

		bool EnableDamageEffects
		{
			get;
		}

		bool EnableDynamicMusic
		{
			get;
		}

		bool EnableMuteWhenNotInFocus
		{
			get;
		}

		bool EnablePerformanceWarnings
		{
			get;
		}

		bool EnableReverb
		{
			get;
		}

		bool EnableVoiceChat
		{
			get;
		}

		float FieldOfView
		{
			get;
		}

		bool FirstTimeRun
		{
			get;
		}

		float FlaresIntensity
		{
			get;
		}

		float GameVolume
		{
			get;
		}

		MyGraphicsRenderer GraphicsRenderer
		{
			get;
		}

		float? GrassDensityFactor
		{
			get;
		}

		float? GrassDrawDistance
		{
			get;
		}

		float HUDBkOpacity
		{
			get;
		}

		bool HudWarnings
		{
			get;
		}

		MyLanguagesEnum Language
		{
			get;
		}

		bool MemoryLimits
		{
			get;
		}

		bool MinimalHud
		{
			get;
		}

		int HudState
		{
			get;
		}

		MyRenderQualityEnum? ModelQuality
		{
			get;
		}

		float MusicVolume
		{
			get;
		}

		HashSetReader<ulong> MutedPlayers
		{
			get;
		}

		int RefreshRate
		{
			get;
		}

		bool RotationHints
		{
			get;
		}

		int? ScreenHeight
		{
			get;
		}

		float ScreenshotSizeMultiplier
		{
			get;
		}

		int? ScreenWidth
		{
			get;
		}

		MyRenderQualityEnum? ShaderQuality
		{
			get;
		}

		MyShadowsQuality? ShadowQuality
		{
			get;
		}

		bool ShipSoundsAreBasedOnSpeed
		{
			get;
		}

		bool ShowCrosshair
		{
			get;
		}

		bool EnableTrading
		{
			get;
		}

		string Skin
		{
			get;
		}

		MyTextureQuality? TextureQuality
		{
			get;
		}

		float UIBkOpacity
		{
			get;
		}

		float UIOpacity
		{
			get;
		}

		float? VegetationDrawDistance
		{
			get;
		}

		int VerticalSync
		{
			get;
		}

		int VideoAdapter
		{
			get;
		}

		float VoiceChatVolume
		{
			get;
		}

		MyRenderQualityEnum? VoxelQuality
		{
			get;
		}

		MyWindowModeEnum WindowMode
		{
			get;
		}

		[Obsolete]
		DictionaryReader<string, object> ControlsButtons
		{
			get;
		}

		[Obsolete]
		DictionaryReader<string, object> ControlsGeneral
		{
			get;
		}
	}
}
