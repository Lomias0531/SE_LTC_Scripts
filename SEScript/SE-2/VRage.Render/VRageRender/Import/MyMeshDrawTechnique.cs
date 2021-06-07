using System.Reflection;

namespace VRageRender.Import
{
	[Obfuscation(Feature = "cw symbol renaming", ApplyToMembers = true, Exclude = true)]
	public enum MyMeshDrawTechnique : byte
	{
		MESH,
		VOXELS_DEBRIS,
		VOXEL_MAP,
		ALPHA_MASKED,
		ALPHA_MASKED_SINGLE_SIDED,
		FOLIAGE,
		DECAL,
		DECAL_NOPREMULT,
		DECAL_CUTOUT,
		HOLO,
		VOXEL_MAP_SINGLE,
		VOXEL_MAP_MULTI,
		SKINNED,
		MESH_INSTANCED,
		MESH_INSTANCED_SKINNED,
		GLASS,
		MESH_INSTANCED_GENERIC,
		MESH_INSTANCED_GENERIC_MASKED,
		ATMOSPHERE,
		CLOUD_LAYER,
		SHIELD,
		SHIELD_LIT,
		COUNT
	}
}
