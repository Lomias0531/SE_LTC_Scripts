using System;
using VRageMath;

namespace VRage.ModAPI
{
	/// <summary>
	/// Links to modapi actions. Delegates are set inside MyAPIGateway.
	/// VRAGE TODO: This is probably a temporary class helping us to remove sandbox.
	/// </summary>
	public static class MyAPIGatewayShortcuts
	{
		public delegate IMyCamera GetMainCameraCallback();

		public delegate BoundingBoxD GetWorldBoundariesCallback();

		public delegate Vector3D GetLocalPlayerPositionCallback();

		/// <summary>
		/// Registers entity in update loop.
		/// Parameters: IMyEntity entity (ref to entity to be registered)
		/// </summary>
		public static Action<IMyEntity> RegisterEntityUpdate;

		/// <summary>
		/// Unregisters entity from update loop.
		/// Parameters: IMyEntity entity (ref to entity to be unregistered), bool immediate (default is false)
		/// </summary>
		public static Action<IMyEntity, bool> UnregisterEntityUpdate;

		public static GetMainCameraCallback GetMainCamera;

		public static GetWorldBoundariesCallback GetWorldBoundaries;

		public static GetLocalPlayerPositionCallback GetLocalPlayerPosition;
	}
}
