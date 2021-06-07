using System;

namespace VRage.Voxels
{
	/// <summary>
	/// Flags used when requesting voxel materials and content.
	///
	/// These flags allow for optimizations such as avoiding
	/// expensive material computations or quickly assigning the
	/// whole storage the same material or content.
	/// </summary>
	[Flags]
	public enum MyVoxelRequestFlags
	{
		SurfaceMaterial = 0x1,
		ConsiderContent = 0x2,
		ForPhysics = 0x4,
		EmptyData = 0x8,
		FullContent = 0x10,
		OneMaterial = 0x20,
		AdviseCache = 0x40,
		ContentChecked = 0x80,
		ContentCheckedDeep = 0x100,
		UseNativeProvider = 0x200,
		Postprocess = 0x400,
		DoNotCheck = 0x10000,
		PreciseOrePositions = 0x20000,
		RequestFlags = 0x3
	}
}
