using VRageMath;

namespace Sandbox.Game.Entities.Character
{
	internal class MyMoveNetCommand : IMyNetworkCommand
	{
		private MyCharacter m_character;

		private Vector3 m_move;

		private Quaternion m_rotation;

		public bool ExecuteBeforeMoveAndRotate => false;

		public MyMoveNetCommand(MyCharacter character, ref Vector3 move, ref Quaternion rotation)
		{
			m_character = character;
			m_move = move;
			m_rotation = rotation;
		}

		public void Apply()
		{
			m_character.ApplyRotation(m_rotation);
			m_character.MoveAndRotate(m_move, Vector2.Zero, 0f);
			m_character.MoveAndRotateInternal(m_move, Vector2.Zero, 0f, Vector3.Zero);
		}
	}
}
