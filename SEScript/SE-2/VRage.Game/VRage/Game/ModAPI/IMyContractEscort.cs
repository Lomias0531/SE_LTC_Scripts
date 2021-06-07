using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyContractEscort : IMyContract
	{
		Vector3D Start
		{
			get;
		}

		Vector3D End
		{
			get;
		}

		long OwnerIdentityId
		{
			get;
		}
	}
}
