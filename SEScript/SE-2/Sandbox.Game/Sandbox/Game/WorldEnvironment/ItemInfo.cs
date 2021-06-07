using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public struct ItemInfo
	{
		public Vector3 Position;

		public short DefinitionIndex;

		public short ModelIndex;

		public Quaternion Rotation;

		public override string ToString()
		{
			return $"Model: {ModelIndex}; Def: {DefinitionIndex}";
		}
	}
}
