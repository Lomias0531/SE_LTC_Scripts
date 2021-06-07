using System;

namespace VRageRender
{
	/// <summary>
	/// Entity flags.
	/// </summary>
	[Flags]
	public enum RenderFlags
	{
		/// <summary>
		/// Skip the object in render if detected that it is too small
		/// </summary>
		SkipIfTooSmall = 0x1,
		/// <summary>
		/// Needs resolve cast shadows flag (done by parallel raycast to sun)
		/// </summary>
		NeedsResolveCastShadow = 0x2,
		/// <summary>
		/// Casts only one raycast to determine shadow casting
		/// </summary>
		FastCastShadowResolve = 0x4,
		/// <summary>
		/// Tells if this object should cast shadows
		/// </summary>
		CastShadows = 0x8,
		/// <summary>
		/// Specifies whether draw this entity or not.
		/// </summary>
		Visible = 0x10,
		/// <summary>
		/// Specifies whether this entity should be drawn even when it is outside the set view distance
		/// </summary>
		DrawOutsideViewDistance = 0x20,
		/// <summary>
		/// Specifies whether entity is "near", near entities are cockpit and weapons, these entities are rendered in special way
		/// </summary>
		Near = 0x40,
		/// <summary>
		/// Tells if this object should use PlayerHeadMatrix as matrix for draw
		/// </summary>
		UseCustomDrawMatrix = 0x80,
		/// <summary>
		/// Use local AABB box for shadow LOD, not used
		/// </summary>
		ShadowLodBox = 0x100,
		/// <summary>
		/// No culling of back faces
		/// </summary>
		NoBackFaceCulling = 0x200,
		SkipInMainView = 0x400,
		ForceOldPipeline = 0x800,
		CastShadowsOnLow = 0x1000,
		DrawInAllCascades = 0x2000,
		DistanceFade = 0x4000,
		SkipInDepth = 0x8000,
		MetalnessColorable = 0x10000,
		SkipInForward = 0x20000
	}
}
