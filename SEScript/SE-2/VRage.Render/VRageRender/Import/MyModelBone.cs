using VRageMath;

namespace VRageRender.Import
{
	public sealed class MyModelBone
	{
		public int Index;

		public string Name;

		public int Parent;

		public Matrix Transform;

		public override string ToString()
		{
			return Name + " (" + Index + ")";
		}
	}
}
