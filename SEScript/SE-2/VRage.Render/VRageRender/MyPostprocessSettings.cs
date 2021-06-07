using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using VRage;
using VRage.Network;
using VRageMath;

namespace VRageRender
{
	[Serializable]
	public struct MyPostprocessSettings
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		[XmlType("MyPostprocessSettings.Layout")]
		public struct Layout
		{
			public float Contrast;

			public float Brightness;

			public float ConstantLuminance;

			public float LuminanceExposure;

			public float Saturation;

			public float BrightnessFactorR;

			public float BrightnessFactorG;

			public float BrightnessFactorB;

			public Vector3 TemperatureColor;

			public float TemperatureStrength;

			public float Vibrance;

			public float EyeAdaptationTau;

			public float BloomExposure;

			public float BloomLumaThreshold;

			public float BloomMult;

			public float BloomEmissiveness;

			public float BloomDepthStrength;

			public float BloomDepthSlope;

			public Vector3 LightColor;

			public float Res2;

			public Vector3 DarkColor;

			public float SepiaStrength;

			public float EyeAdaptationSpeedUp;

			public float EyeAdaptationSpeedDown;

			public float WhitePoint;

			public float BloomDirtRatio;

			public int GrainSize;

			public float GrainAmount;

			public float GrainStrength;

			public float ChromaticFactor;

			public float VignetteStart;

			public float VignetteLength;

			public float Res0;

			public float Res1;

			[StructDefault]
			public static readonly Layout Default;

			static Layout()
			{
				Default = new Layout
				{
					Contrast = 1f,
					Brightness = 1f,
					ConstantLuminance = 0.1f,
					LuminanceExposure = 1f,
					Saturation = 1f,
					BrightnessFactorR = 1f,
					BrightnessFactorG = 1f,
					BrightnessFactorB = 1f,
					EyeAdaptationTau = 0.3f,
					Vibrance = 0f,
					TemperatureStrength = 0f,
					BloomEmissiveness = 1f,
					BloomExposure = 5.8f,
					BloomLumaThreshold = 0.16f,
					BloomMult = 0.28f,
					BloomDepthStrength = 2f,
					BloomDepthSlope = 0.3f,
					LightColor = new Vector3(1f, 0.9f, 0.5f),
					DarkColor = new Vector3(0.2f, 0.05f, 0f),
					SepiaStrength = 0f,
					EyeAdaptationSpeedUp = 2f,
					EyeAdaptationSpeedDown = 1f,
					WhitePoint = 6f,
					BloomDirtRatio = 0.5f,
					GrainSize = 1,
					GrainAmount = 0.1f,
					GrainStrength = 0f,
					ChromaticFactor = 0.1f,
					VignetteStart = 2f,
					VignetteLength = 2f
				};
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EEnableTonemapping_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in bool value)
			{
				owner.EnableTonemapping = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out bool value)
			{
				value = owner.EnableTonemapping;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EEnableEyeAdaptation_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in bool value)
			{
				owner.EnableEyeAdaptation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out bool value)
			{
				value = owner.EnableEyeAdaptation;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EHighQualityBloom_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in bool value)
			{
				owner.HighQualityBloom = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out bool value)
			{
				value = owner.HighQualityBloom;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EBloomAntiFlickerFilter_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in bool value)
			{
				owner.BloomAntiFlickerFilter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out bool value)
			{
				value = owner.BloomAntiFlickerFilter;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EBloomSize_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in int value)
			{
				owner.BloomSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out int value)
			{
				value = owner.BloomSize;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003ETemperature_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in float value)
			{
				owner.Temperature = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out float value)
			{
				value = owner.Temperature;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EBloomEnabled_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in bool value)
			{
				owner.BloomEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out bool value)
			{
				value = owner.BloomEnabled;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EHistogramLogMin_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in float value)
			{
				owner.HistogramLogMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out float value)
			{
				value = owner.HistogramLogMin;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EHistogramLogMax_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in float value)
			{
				owner.HistogramLogMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out float value)
			{
				value = owner.HistogramLogMax;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EHistogramFilterMin_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in float value)
			{
				owner.HistogramFilterMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out float value)
			{
				value = owner.HistogramFilterMin;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EHistogramFilterMax_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in float value)
			{
				owner.HistogramFilterMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out float value)
			{
				value = owner.HistogramFilterMax;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EMinEyeAdaptationLogBrightness_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in float value)
			{
				owner.MinEyeAdaptationLogBrightness = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out float value)
			{
				value = owner.MinEyeAdaptationLogBrightness;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EMaxEyeAdaptationLogBrightness_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in float value)
			{
				owner.MaxEyeAdaptationLogBrightness = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out float value)
			{
				value = owner.MaxEyeAdaptationLogBrightness;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EHistogramLuminanceThreshold_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in float value)
			{
				owner.HistogramLuminanceThreshold = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out float value)
			{
				value = owner.HistogramLuminanceThreshold;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EHistogramSkyboxFactor_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in float value)
			{
				owner.HistogramSkyboxFactor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out float value)
			{
				value = owner.HistogramSkyboxFactor;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EEyeAdaptationPrioritizeScreenCenter_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in bool value)
			{
				owner.EyeAdaptationPrioritizeScreenCenter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out bool value)
			{
				value = owner.EyeAdaptationPrioritizeScreenCenter;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EChromaticIterations_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in int value)
			{
				owner.ChromaticIterations = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out int value)
			{
				value = owner.ChromaticIterations;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EDirtTexture_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in string value)
			{
				owner.DirtTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out string value)
			{
				value = owner.DirtTexture;
			}
		}

		protected class VRageRender_MyPostprocessSettings_003C_003EData_003C_003EAccessor : IMemberAccessor<MyPostprocessSettings, Layout>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPostprocessSettings owner, in Layout value)
			{
				owner.Data = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPostprocessSettings owner, out Layout value)
			{
				value = owner.Data;
			}
		}

		[StructDefault]
		public static readonly MyPostprocessSettings Default;

		public bool EnableTonemapping;

		public bool EnableEyeAdaptation;

		public bool HighQualityBloom;

		public bool BloomAntiFlickerFilter;

		public int BloomSize;

		public float Temperature;

		public bool BloomEnabled;

		public float HistogramLogMin;

		public float HistogramLogMax;

		public float HistogramFilterMin;

		public float HistogramFilterMax;

		public float MinEyeAdaptationLogBrightness;

		public float MaxEyeAdaptationLogBrightness;

		public float HistogramLuminanceThreshold;

		public float HistogramSkyboxFactor;

		public bool EyeAdaptationPrioritizeScreenCenter;

		public int ChromaticIterations;

		public string DirtTexture;

		[XmlElement(Type = typeof(MyStructXmlSerializer<Layout>))]
		public Layout Data;

		static MyPostprocessSettings()
		{
			Default = new MyPostprocessSettings
			{
				EnableTonemapping = true,
				EnableEyeAdaptation = false,
				BloomSize = 6,
				Temperature = 6500f,
				HighQualityBloom = true,
				BloomAntiFlickerFilter = true,
				BloomEnabled = true,
				HistogramLogMin = -4f,
				HistogramLogMax = 4f,
				HistogramFilterMin = 70f,
				HistogramFilterMax = 95f,
				MinEyeAdaptationLogBrightness = -1f,
				MaxEyeAdaptationLogBrightness = 2f,
				HistogramLuminanceThreshold = 0f,
				HistogramSkyboxFactor = 0.5f,
				EyeAdaptationPrioritizeScreenCenter = false,
				ChromaticIterations = 4,
				DirtTexture = "",
				Data = Layout.Default
			};
		}

		public static MyPostprocessSettings LerpExposure(ref MyPostprocessSettings A, ref MyPostprocessSettings B, float t)
		{
			MyPostprocessSettings result = A;
			result.Data.LuminanceExposure = MathHelper.Lerp(A.Data.LuminanceExposure, B.Data.LuminanceExposure, t);
			return result;
		}

		public Layout GetProcessedData()
		{
			Layout data = Data;
			if (EnableEyeAdaptation)
			{
				data.ConstantLuminance = -1f;
			}
			else
			{
				data.EyeAdaptationTau = 0f;
			}
			data.TemperatureColor = ColorExtensions.TemperatureToRGB(Temperature);
			return data;
		}
	}
}
