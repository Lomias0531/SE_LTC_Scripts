using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game
{
	/// <summary>
	/// Global (environment) mergeable definitions
	/// </summary>
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("EnvironmentDefinition")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_EnvironmentDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public struct EnvironmentalParticleSettings
		{
			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EId_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in SerializableDefinitionId value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out SerializableDefinitionId value)
				{
					value = owner.Id;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in string value)
				{
					owner.Material = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out string value)
				{
					value = owner.Material;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EColor_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, Vector4>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in Vector4 value)
				{
					owner.Color = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out Vector4 value)
				{
					value = owner.Color;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EMaterialPlanet_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in string value)
				{
					owner.MaterialPlanet = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out string value)
				{
					value = owner.MaterialPlanet;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EColorPlanet_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, Vector4>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in Vector4 value)
				{
					owner.ColorPlanet = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out Vector4 value)
				{
					value = owner.ColorPlanet;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EMaxSpawnDistance_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in float value)
				{
					owner.MaxSpawnDistance = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out float value)
				{
					value = owner.MaxSpawnDistance;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EDespawnDistance_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in float value)
				{
					owner.DespawnDistance = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out float value)
				{
					value = owner.DespawnDistance;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EDensity_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in float value)
				{
					owner.Density = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out float value)
				{
					value = owner.Density;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EMaxLifeTime_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in int value)
				{
					owner.MaxLifeTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out int value)
				{
					value = owner.MaxLifeTime;
				}
			}

			protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EMaxParticles_003C_003EAccessor : IMemberAccessor<EnvironmentalParticleSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EnvironmentalParticleSettings owner, in int value)
				{
					owner.MaxParticles = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EnvironmentalParticleSettings owner, out int value)
				{
					value = owner.MaxParticles;
				}
			}

			private class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticleSettings_003C_003EActor : IActivator, IActivator<EnvironmentalParticleSettings>
			{
				private sealed override object CreateInstance()
				{
					return default(EnvironmentalParticleSettings);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override EnvironmentalParticleSettings CreateInstance()
				{
					return (EnvironmentalParticleSettings)(object)default(EnvironmentalParticleSettings);
				}

				EnvironmentalParticleSettings IActivator<EnvironmentalParticleSettings>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public SerializableDefinitionId Id;

			[ProtoMember(4)]
			public string Material;

			[ProtoMember(7)]
			public Vector4 Color;

			[ProtoMember(10)]
			public string MaterialPlanet;

			[ProtoMember(13)]
			public Vector4 ColorPlanet;

			[ProtoMember(16)]
			public float MaxSpawnDistance;

			[ProtoMember(19)]
			public float DespawnDistance;

			[ProtoMember(22)]
			public float Density;

			[ProtoMember(25)]
			public int MaxLifeTime;

			[ProtoMember(28)]
			public int MaxParticles;
		}

		public static class Defaults
		{
			public const float SmallShipMaxSpeed = 100f;

			public const float LargeShipMaxSpeed = 100f;

			public const float SmallShipMaxAngularSpeed = 36000f;

			public const float LargeShipMaxAngularSpeed = 18000f;

			public static readonly Vector4 ContourHighlightColor = new Vector4(1f, 1f, 0f, 0.05f);

			public static readonly Vector4 ContourHighlightColorAccessDenied = new Vector4(1f, 0f, 0f, 0.05f);

			public const float ContourHighlightThickness = 5f;

			public const float HighlightPulseInSeconds = 0f;

			public const string EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube.dds";

			public const string ScaryFaceTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube_ScaryFace.dds";

			public static readonly DateTime ScaryFaceFrom = new DateTime(2019, 10, 25);

			public static readonly DateTime ScaryFaceTo = new DateTime(2019, 11, 1);

			public const string Christmas2019Texture = "Textures\\BackgroundCube\\Final\\BackgroundCube_Christmas.dds";

			public static readonly DateTime Christmas2019From = new DateTime(2019, 12, 19);

			public static readonly DateTime Christmas2019To = new DateTime(2020, 1, 3);

			public static readonly MyOrientation EnvironmentOrientation = new MyOrientation(MathHelper.ToRadians(60.3955536f), MathHelper.ToRadians(-61.1861954f), MathHelper.ToRadians(90.90578f));
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EFogProperties_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyFogProperties>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyFogProperties value)
			{
				owner.FogProperties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyFogProperties value)
			{
				value = owner.FogProperties;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EPlanetProperties_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyPlanetProperties>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyPlanetProperties value)
			{
				owner.PlanetProperties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyPlanetProperties value)
			{
				value = owner.PlanetProperties;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003ESunProperties_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MySunProperties>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MySunProperties value)
			{
				owner.SunProperties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MySunProperties value)
			{
				value = owner.SunProperties;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EPostProcessSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyPostprocessSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyPostprocessSettings value)
			{
				owner.PostProcessSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyPostprocessSettings value)
			{
				value = owner.PostProcessSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003ESSAOSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MySSAOSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MySSAOSettings value)
			{
				owner.SSAOSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MySSAOSettings value)
			{
				value = owner.SSAOSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EHBAOSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyHBAOData>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyHBAOData value)
			{
				owner.HBAOSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyHBAOData value)
			{
				value = owner.HBAOSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EShadowSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyShadowsSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyShadowsSettings value)
			{
				owner.ShadowSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyShadowsSettings value)
			{
				value = owner.ShadowSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003ELowLoddingSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyNewLoddingSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyNewLoddingSettings value)
			{
				owner.LowLoddingSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyNewLoddingSettings value)
			{
				value = owner.LowLoddingSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EMediumLoddingSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyNewLoddingSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyNewLoddingSettings value)
			{
				owner.MediumLoddingSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyNewLoddingSettings value)
			{
				value = owner.MediumLoddingSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EHighLoddingSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyNewLoddingSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyNewLoddingSettings value)
			{
				owner.HighLoddingSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyNewLoddingSettings value)
			{
				value = owner.HighLoddingSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EExtremeLoddingSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyNewLoddingSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyNewLoddingSettings value)
			{
				owner.ExtremeLoddingSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyNewLoddingSettings value)
			{
				value = owner.ExtremeLoddingSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentalParticles_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, List<EnvironmentalParticleSettings>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in List<EnvironmentalParticleSettings> value)
			{
				owner.EnvironmentalParticles = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out List<EnvironmentalParticleSettings> value)
			{
				value = owner.EnvironmentalParticles;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003ESmallShipMaxSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value)
			{
				owner.SmallShipMaxSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value)
			{
				value = owner.SmallShipMaxSpeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003ELargeShipMaxSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value)
			{
				owner.LargeShipMaxSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value)
			{
				value = owner.LargeShipMaxSpeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003ESmallShipMaxAngularSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value)
			{
				owner.SmallShipMaxAngularSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value)
			{
				value = owner.SmallShipMaxAngularSpeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003ELargeShipMaxAngularSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value)
			{
				owner.LargeShipMaxAngularSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value)
			{
				value = owner.LargeShipMaxAngularSpeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EContourHighlightColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in Vector4 value)
			{
				owner.ContourHighlightColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out Vector4 value)
			{
				value = owner.ContourHighlightColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EContourHighlightColorAccessDenied_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in Vector4 value)
			{
				owner.ContourHighlightColorAccessDenied = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out Vector4 value)
			{
				value = owner.ContourHighlightColorAccessDenied;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EContourHighlightThickness_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value)
			{
				owner.ContourHighlightThickness = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value)
			{
				value = owner.ContourHighlightThickness;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EHighlightPulseInSeconds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value)
			{
				owner.HighlightPulseInSeconds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value)
			{
				value = owner.HighlightPulseInSeconds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value)
			{
				owner.EnvironmentTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value)
			{
				value = owner.EnvironmentTexture;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnvironmentOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyOrientation value)
			{
				owner.EnvironmentOrientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyOrientation value)
			{
				value = owner.EnvironmentOrientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_EnvironmentDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EnvironmentDefinition CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentDefinition();
			}

			MyObjectBuilder_EnvironmentDefinition IActivator<MyObjectBuilder_EnvironmentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlElement(Type = typeof(MyStructXmlSerializer<MyFogProperties>))]
		public MyFogProperties FogProperties = MyFogProperties.Default;

		[XmlElement(Type = typeof(MyStructXmlSerializer<MyPlanetProperties>))]
		public MyPlanetProperties PlanetProperties = MyPlanetProperties.Default;

		[XmlElement(Type = typeof(MyStructXmlSerializer<MySunProperties>))]
		public MySunProperties SunProperties = MySunProperties.Default;

		[XmlElement(Type = typeof(MyStructXmlSerializer<MyPostprocessSettings>))]
		public MyPostprocessSettings PostProcessSettings = MyPostprocessSettings.Default;

		[XmlElement(Type = typeof(MyStructXmlSerializer<MySSAOSettings>))]
		public MySSAOSettings SSAOSettings = MySSAOSettings.Default;

		[XmlElement(Type = typeof(MyStructXmlSerializer<MyHBAOData>))]
		public MyHBAOData HBAOSettings = MyHBAOData.Default;

		public MyShadowsSettings ShadowSettings = new MyShadowsSettings();

		public MyNewLoddingSettings LowLoddingSettings = new MyNewLoddingSettings();

		public MyNewLoddingSettings MediumLoddingSettings = new MyNewLoddingSettings();

		public MyNewLoddingSettings HighLoddingSettings = new MyNewLoddingSettings();

		public MyNewLoddingSettings ExtremeLoddingSettings = new MyNewLoddingSettings();

		[ProtoMember(31)]
		[XmlArrayItem("ParticleType")]
		public List<EnvironmentalParticleSettings> EnvironmentalParticles = new List<EnvironmentalParticleSettings>();

		public float SmallShipMaxSpeed = 100f;

		public float LargeShipMaxSpeed = 100f;

		public float SmallShipMaxAngularSpeed = 36000f;

		public float LargeShipMaxAngularSpeed = 18000f;

		public Vector4 ContourHighlightColor = Defaults.ContourHighlightColor;

		public Vector4 ContourHighlightColorAccessDenied = Defaults.ContourHighlightColorAccessDenied;

		public float ContourHighlightThickness = 5f;

		public float HighlightPulseInSeconds;

		[ModdableContentFile("dds")]
		public string EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube.dds";

		public MyOrientation EnvironmentOrientation = Defaults.EnvironmentOrientation;
	}
}
