using VRage;

namespace VRageRender
{
	public struct MyPassLoddingSetting
	{
		public int LodShiftVisible;

		public int LodShift;

		public int MinLod;

		[StructDefault]
		public static readonly MyPassLoddingSetting Default;
	}
}
