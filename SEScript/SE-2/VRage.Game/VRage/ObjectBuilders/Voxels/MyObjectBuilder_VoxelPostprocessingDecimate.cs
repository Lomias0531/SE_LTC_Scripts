using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Utils;

namespace VRage.ObjectBuilders.Voxels
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_VoxelPostprocessingDecimate : MyObjectBuilder_VoxelPostprocessing
	{
		[ProtoContract]
		public class Settings
		{
			protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003ESettings_003C_003EFromLod_003C_003EAccessor : IMemberAccessor<Settings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Settings owner, in int value)
				{
					owner.FromLod = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Settings owner, out int value)
				{
					value = owner.FromLod;
				}
			}

			protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003ESettings_003C_003EFeatureAngle_003C_003EAccessor : IMemberAccessor<Settings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Settings owner, in float value)
				{
					owner.FeatureAngle = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Settings owner, out float value)
				{
					value = owner.FeatureAngle;
				}
			}

			protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003ESettings_003C_003EEdgeThreshold_003C_003EAccessor : IMemberAccessor<Settings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Settings owner, in float value)
				{
					owner.EdgeThreshold = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Settings owner, out float value)
				{
					value = owner.EdgeThreshold;
				}
			}

			protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003ESettings_003C_003EPlaneThreshold_003C_003EAccessor : IMemberAccessor<Settings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Settings owner, in float value)
				{
					owner.PlaneThreshold = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Settings owner, out float value)
				{
					value = owner.PlaneThreshold;
				}
			}

			protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003ESettings_003C_003EIgnoreEdges_003C_003EAccessor : IMemberAccessor<Settings, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Settings owner, in bool value)
				{
					owner.IgnoreEdges = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Settings owner, out bool value)
				{
					value = owner.IgnoreEdges;
				}
			}

			private class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003ESettings_003C_003EActor : IActivator, IActivator<Settings>
			{
				private sealed override object CreateInstance()
				{
					return new Settings();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Settings CreateInstance()
				{
					return new Settings();
				}

				Settings IActivator<Settings>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[Description("Minimum lod level these settings apply. Subsequent sets must have strictly ascending lods.")]
			public int FromLod;

			[Description("The minimum angle to be considered a feature edge. Value is In Radians")]
			public float FeatureAngle;

			[Description("Distance threshold for an edge vertex to be discarded.")]
			public float EdgeThreshold;

			[Description("Distance threshold for an internal vertex to be discarded.")]
			public float PlaneThreshold;

			[Description("Weather edge vertices should be considered or not for removal.")]
			public bool IgnoreEdges = true;
		}

		protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003ELodSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, List<Settings>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, in List<Settings> value)
			{
				owner.LodSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, out List<Settings> value)
			{
				value = owner.LodSettings;
			}
		}

		protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003EForPhysics_003C_003EAccessor : VRage_ObjectBuilders_Definitions_Components_MyObjectBuilder_VoxelPostprocessing_003C_003EForPhysics_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_VoxelPostprocessing*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_VoxelPostprocessing*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelPostprocessingDecimate owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate_003C_003EActor : IActivator, IActivator<MyObjectBuilder_VoxelPostprocessingDecimate>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_VoxelPostprocessingDecimate();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_VoxelPostprocessingDecimate CreateInstance()
			{
				return new MyObjectBuilder_VoxelPostprocessingDecimate();
			}

			MyObjectBuilder_VoxelPostprocessingDecimate IActivator<MyObjectBuilder_VoxelPostprocessingDecimate>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[Description("Set of lod range settings pairs.")]
		public List<Settings> LodSettings;
	}
}
