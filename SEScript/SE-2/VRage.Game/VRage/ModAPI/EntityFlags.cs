using System;

namespace VRage.ModAPI
{
	/// <summary>
	/// Entity flags.
	/// </summary>
	[Flags]
	public enum EntityFlags
	{
		/// <summary>
		/// No flags
		/// </summary>
		None = 0x1,
		/// <summary>
		/// Specifies whether draw this entity or not.
		/// </summary>
		Visible = 0x2,
		/// <summary>
		/// Specifies whether save entity when saving sector or not
		/// </summary>
		Save = 0x8,
		/// <summary>
		/// Specifies whether entity is "near", near entities are cockpit and weapons, these entities are rendered in special way
		/// </summary>
		Near = 0x10,
		/// <summary>
		/// On this entity and its children will be called UpdateBeforeSimulation and UpdateAfterSimulation each frame
		/// </summary>
		NeedsUpdate = 0x20,
		NeedsResolveCastShadow = 0x40,
		FastCastShadowResolve = 0x80,
		SkipIfTooSmall = 0x100,
		NeedsUpdate10 = 0x200,
		NeedsUpdate100 = 0x400,
		/// <summary>
		/// Draw method of this entity will be called when suitable
		/// </summary>
		NeedsDraw = 0x800,
		/// <summary>
		/// If object is moved, invalidate its renderobjects (update render)
		/// </summary>
		InvalidateOnMove = 0x1000,
		/// <summary>
		/// Synchronize object during multiplayer
		/// </summary>
		Sync = 0x2000,
		/// <summary>
		/// Draw method of this entity will be called when suitable and only from parent
		/// </summary>
		NeedsDrawFromParent = 0x4000,
		/// <summary>
		/// Draw LOD shadow as box
		/// </summary>
		ShadowBoxLod = 0x8000,
		/// <summary>
		/// Render the entity using dithering to simulate transparency
		/// </summary>
		Transparent = 0x10000,
		/// <summary>
		/// Entity updated once before first frame.
		/// </summary>
		NeedsUpdateBeforeNextFrame = 0x20000,
		DrawOutsideViewDistance = 0x40000,
		IsGamePrunningStructureObject = 0x80000,
		/// <summary>
		/// If child, its world matrix must be always updated
		/// </summary>
		NeedsWorldMatrix = 0x100000,
		/// <summary>
		/// Do not use in prunning, even though it is a root entity
		/// </summary>
		IsNotGamePrunningStructureObject = 0x200000,
		NeedsSimulate = 0x400000,
		UpdateRender = 0x800000,
		Default = 0x90114A
	}
}
