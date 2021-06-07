using System.Reflection;

namespace VRageRender.Import
{
	[Obfuscation(Feature = "cw symbol renaming", ApplyToMembers = true, Exclude = true)]
	public enum MyMeshDrawTechniqueOld : byte
	{
		MESH,
		VOXELS_DEBRIS,
		VOXEL_MAP,
		ALPHA_MASKED,
		FOLIAGE,
		DECAL,
		DECAL_CUTOUT,
		HOLO,
		VOXEL_MAP_SINGLE,
		VOXEL_MAP_MULTI,
		SKINNED,
		GLASS
	}
}
