using VRageMath;

namespace VRage.Network
{
	public interface IMyEntityReplicable
	{
		MatrixD WorldMatrix
		{
			get;
		}

		long EntityId
		{
			get;
		}
	}
}
