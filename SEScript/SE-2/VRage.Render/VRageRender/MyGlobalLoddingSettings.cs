using VRage;

namespace VRageRender
{
	public struct MyGlobalLoddingSettings
	{
		public float ObjectDistanceAdd;

		public float ObjectDistanceMult;

		public float MinTransitionInSeconds;

		public float MaxTransitionInSeconds;

		public float TransitionDeadZoneConst;

		public float TransitionDeadZoneDistanceMult;

		public float HisteresisRatio;

		public double MaxDistanceForSmoothCameraMovement;

		public bool IsUpdateEnabled;

		public bool EnableLodSelection;

		public int LodSelection;

		[StructDefault]
		public static readonly MyGlobalLoddingSettings Default = new MyGlobalLoddingSettings
		{
			ObjectDistanceAdd = 0f,
			ObjectDistanceMult = 1f,
			MaxDistanceForSmoothCameraMovement = 10.0,
			IsUpdateEnabled = true,
			EnableLodSelection = false,
			LodSelection = 0,
			HisteresisRatio = 0.1f
		};
	}
}
