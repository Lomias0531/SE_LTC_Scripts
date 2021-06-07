using VRageMath;

namespace Sandbox.Game.Entities.Character
{
	internal class MyDeltaNetCommand : IMyNetworkCommand
	{
		private MyCharacter m_character;

		private Vector3D m_delta;

		public bool ExecuteBeforeMoveAndRotate => true;

		public MyDeltaNetCommand(MyCharacter character, ref Vector3D delta)
		{
			m_character = character;
			m_delta = delta;
		}

		public void Apply()
		{
			MatrixD worldMatrix = m_character.WorldMatrix;
			worldMatrix.Translation += m_delta;
			m_character.PositionComp.SetWorldMatrix(worldMatrix, null, forceUpdate: true);
		}
	}
}
