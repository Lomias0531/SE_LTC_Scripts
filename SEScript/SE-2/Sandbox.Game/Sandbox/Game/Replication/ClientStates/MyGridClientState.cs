using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.Replication.ClientStates
{
	public struct MyGridClientState
	{
		public bool Valid;

		public Vector3 Move;

		public Vector2 Rotation;

		public float Roll;

		public MyGridClientState(BitStream stream)
		{
			Rotation = new Vector2
			{
				X = stream.ReadFloat(),
				Y = stream.ReadFloat()
			};
			Roll = stream.ReadFloat();
			Move = new Vector3
			{
				X = stream.ReadFloat(),
				Y = stream.ReadFloat(),
				Z = stream.ReadFloat()
			};
			Valid = true;
		}

		public void Serialize(BitStream stream)
		{
			stream.WriteFloat(Rotation.X);
			stream.WriteFloat(Rotation.Y);
			stream.WriteFloat(Roll);
			stream.WriteFloat(Move.X);
			stream.WriteFloat(Move.Y);
			stream.WriteFloat(Move.Z);
		}
	}
}
