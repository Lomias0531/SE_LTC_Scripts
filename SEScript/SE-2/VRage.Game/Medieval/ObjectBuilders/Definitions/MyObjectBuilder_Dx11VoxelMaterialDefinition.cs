using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Medieval.ObjectBuilders.Definitions
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Dx11VoxelMaterialDefinition : MyObjectBuilder_VoxelMaterialDefinition
	{
		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EColorMetalXZnY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ColorMetalXZnY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ColorMetalXZnY;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EColorMetalY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ColorMetalY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ColorMetalY;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ENormalGlossXZnY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.NormalGlossXZnY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.NormalGlossXZnY;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ENormalGlossY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.NormalGlossY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.NormalGlossY;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EExtXZnY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ExtXZnY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ExtXZnY;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EExtY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ExtY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ExtY;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EColorMetalXZnYFar1_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ColorMetalXZnYFar1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ColorMetalXZnYFar1;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EColorMetalYFar1_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ColorMetalYFar1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ColorMetalYFar1;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ENormalGlossXZnYFar1_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.NormalGlossXZnYFar1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.NormalGlossXZnYFar1;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ENormalGlossYFar1_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.NormalGlossYFar1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.NormalGlossYFar1;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.Scale;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EScaleFar1_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.ScaleFar1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.ScaleFar1;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EExtXZnYFar1_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ExtXZnYFar1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ExtXZnYFar1;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EExtYFar1_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ExtYFar1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ExtYFar1;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFoliageTextureArray1_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.FoliageTextureArray1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.FoliageTextureArray1;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFoliageTextureArray2_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.FoliageTextureArray2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.FoliageTextureArray2;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFoliageColorTextureArray_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string[] value)
			{
				owner.FoliageColorTextureArray = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string[] value)
			{
				value = owner.FoliageColorTextureArray;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFoliageNormalTextureArray_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string[] value)
			{
				owner.FoliageNormalTextureArray = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string[] value)
			{
				value = owner.FoliageNormalTextureArray;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFoliageDensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.FoliageDensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.FoliageDensity;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFoliageScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in Vector2 value)
			{
				owner.FoliageScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out Vector2 value)
			{
				value = owner.FoliageScale;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFoliageRandomRescaleMult_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.FoliageRandomRescaleMult = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.FoliageRandomRescaleMult;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFoliageType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in int value)
			{
				owner.FoliageType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out int value)
			{
				value = owner.FoliageType;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EBiomeValueMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in byte value)
			{
				owner.BiomeValueMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out byte value)
			{
				value = owner.BiomeValueMin;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EBiomeValueMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in byte value)
			{
				owner.BiomeValueMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out byte value)
			{
				value = owner.BiomeValueMax;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EColorMetalXZnYFar2_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ColorMetalXZnYFar2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ColorMetalXZnYFar2;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EColorMetalYFar2_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ColorMetalYFar2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ColorMetalYFar2;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ENormalGlossXZnYFar2_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.NormalGlossXZnYFar2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.NormalGlossXZnYFar2;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ENormalGlossYFar2_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.NormalGlossYFar2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.NormalGlossYFar2;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EExtXZnYFar2_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ExtXZnYFar2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ExtXZnYFar2;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EExtYFar2_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				owner.ExtYFar2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				value = owner.ExtYFar2;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EInitialScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.InitialScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.InitialScale;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EScaleMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.ScaleMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.ScaleMultiplier;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EInitialDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.InitialDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.InitialDistance;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EDistanceMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.DistanceMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.DistanceMultiplier;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ETilingScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.TilingScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.TilingScale;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFar1Distance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.Far1Distance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.Far1Distance;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFar2Distance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.Far2Distance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.Far2Distance;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFar3Distance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.Far3Distance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.Far3Distance;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFar1Scale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.Far1Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.Far1Scale;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFar2Scale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.Far2Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.Far2Scale;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFar3Scale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.Far3Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.Far3Scale;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFar3Color_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in Vector4 value)
			{
				owner.Far3Color = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out Vector4 value)
			{
				value = owner.Far3Color;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EExtDetailScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				owner.ExtDetailScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				value = owner.ExtDetailScale;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ESimpleTilingSetup_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, TilingSetup>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in TilingSetup value)
			{
				owner.SimpleTilingSetup = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out TilingSetup value)
			{
				value = owner.SimpleTilingSetup;
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EMaterialTypeName_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMaterialTypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EMinedOre_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMinedOre_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EMinedOreRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMinedOreRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ECanBeHarvested_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ECanBeHarvested_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EIsRare_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EIsRare_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EUseTwoTextures_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EUseTwoTextures_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EVoxelHandPreview_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EVoxelHandPreview_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EMinVersion_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMinVersion_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EMaxVersion_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMaxVersion_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ESpawnsInAsteroids_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ESpawnsInAsteroids_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ESpawnsFromMeteorites_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ESpawnsFromMeteorites_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EDamagedMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EDamagedMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EFriction_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EFriction_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ERestitution_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ERestitution_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EColorKey_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EColorKey_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, ColorDefinitionRGBA?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in ColorDefinitionRGBA? value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out ColorDefinitionRGBA? value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ELandingEffect_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ELandingEffect_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EAsteroidGeneratorSpawnProbabilityMultiplier_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EAsteroidGeneratorSpawnProbabilityMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EBareVariant_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EBareVariant_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VoxelMaterialDefinition*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Dx11VoxelMaterialDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Dx11VoxelMaterialDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Dx11VoxelMaterialDefinition CreateInstance()
			{
				return new MyObjectBuilder_Dx11VoxelMaterialDefinition();
			}

			MyObjectBuilder_Dx11VoxelMaterialDefinition IActivator<MyObjectBuilder_Dx11VoxelMaterialDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[ModdableContentFile("dds")]
		public string ColorMetalXZnY;

		[ProtoMember(4)]
		[ModdableContentFile("dds")]
		public string ColorMetalY;

		[ProtoMember(7)]
		[ModdableContentFile("dds")]
		public string NormalGlossXZnY;

		[ProtoMember(10)]
		[ModdableContentFile("dds")]
		public string NormalGlossY;

		[ProtoMember(13)]
		[ModdableContentFile("dds")]
		public string ExtXZnY;

		[ProtoMember(16)]
		[ModdableContentFile("dds")]
		public string ExtY;

		[ProtoMember(19)]
		[ModdableContentFile("dds")]
		public string ColorMetalXZnYFar1;

		[ProtoMember(22)]
		[ModdableContentFile("dds")]
		public string ColorMetalYFar1;

		[ProtoMember(25)]
		[ModdableContentFile("dds")]
		public string NormalGlossXZnYFar1;

		[ProtoMember(28)]
		[ModdableContentFile("dds")]
		public string NormalGlossYFar1;

		[ProtoMember(31)]
		public float Scale = 8f;

		[ProtoMember(34)]
		public float ScaleFar1 = 8f;

		[ProtoMember(37)]
		[ModdableContentFile("dds")]
		public string ExtXZnYFar1;

		[ProtoMember(40)]
		[ModdableContentFile("dds")]
		public string ExtYFar1;

		[ProtoMember(43)]
		[ModdableContentFile("dds")]
		public string FoliageTextureArray1;

		[ProtoMember(46)]
		[ModdableContentFile("dds")]
		public string FoliageTextureArray2;

		[ProtoMember(49)]
		[ModdableContentFile("dds")]
		[XmlArrayItem("Color")]
		public string[] FoliageColorTextureArray;

		[ProtoMember(52)]
		[ModdableContentFile("dds")]
		[XmlArrayItem("Normal")]
		public string[] FoliageNormalTextureArray;

		[ProtoMember(55)]
		public float FoliageDensity;

		[ProtoMember(58)]
		public Vector2 FoliageScale = Vector2.One;

		[ProtoMember(61)]
		public float FoliageRandomRescaleMult;

		[ProtoMember(64)]
		public int FoliageType;

		[ProtoMember(67)]
		public byte BiomeValueMin;

		[ProtoMember(70)]
		public byte BiomeValueMax;

		[ProtoMember(73)]
		[ModdableContentFile("dds")]
		public string ColorMetalXZnYFar2;

		[ProtoMember(76)]
		[ModdableContentFile("dds")]
		public string ColorMetalYFar2;

		[ProtoMember(79)]
		[ModdableContentFile("dds")]
		public string NormalGlossXZnYFar2;

		[ProtoMember(82)]
		[ModdableContentFile("dds")]
		public string NormalGlossYFar2;

		[ProtoMember(85)]
		[ModdableContentFile("dds")]
		public string ExtXZnYFar2;

		[ProtoMember(88)]
		[ModdableContentFile("dds")]
		public string ExtYFar2;

		[ProtoMember(91)]
		public float InitialScale = 2f;

		[ProtoMember(94)]
		public float ScaleMultiplier = 4f;

		[ProtoMember(97)]
		public float InitialDistance = 5f;

		[ProtoMember(100)]
		public float DistanceMultiplier = 4f;

		[ProtoMember(103)]
		public float TilingScale = 32f;

		[ProtoMember(106)]
		public float Far1Distance;

		[ProtoMember(109)]
		public float Far2Distance;

		[ProtoMember(112)]
		public float Far3Distance;

		[ProtoMember(115)]
		public float Far1Scale = 400f;

		[ProtoMember(118)]
		public float Far2Scale = 2000f;

		[ProtoMember(121)]
		public float Far3Scale = 7000f;

		[ProtoMember(124)]
		public Vector4 Far3Color = Color.Black;

		[ProtoMember(127)]
		public float ExtDetailScale;

		[ProtoMember(130, IsRequired = false)]
		public TilingSetup SimpleTilingSetup;
	}
}
