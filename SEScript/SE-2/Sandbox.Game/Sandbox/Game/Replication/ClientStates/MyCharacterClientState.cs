using Havok;
using Sandbox.Game.Entities.Character;
using System;
using VRage.Game;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.Replication.ClientStates
{
	public struct MyCharacterClientState
	{
		public bool Valid;

		public float HeadX;

		public float HeadY;

		public MyCharacterMovementEnum MovementState;

		public bool Jetpack;

		public bool Dampeners;

		public bool TargetFromCamera;

		public Vector3 MoveIndicator;

		public Quaternion Rotation;

		public MyCharacterMovementFlags MovementFlags;

		public HkCharacterStateType CharacterState;

		public Vector3 SupportNormal;

		public float MovementSpeed;

		public Vector3 MovementDirection;

		public bool IsOnLadder;

		public MyCharacterClientState(BitStream stream)
		{
			HeadX = stream.ReadFloat();
			if (!HeadX.IsValid())
			{
				HeadX = 0f;
			}
			HeadY = stream.ReadFloat();
			MovementState = (MyCharacterMovementEnum)stream.ReadUInt16();
			MovementFlags = (MyCharacterMovementFlags)stream.ReadUInt16();
			Jetpack = stream.ReadBool();
			Dampeners = stream.ReadBool();
			TargetFromCamera = stream.ReadBool();
			MoveIndicator = stream.ReadNormalizedSignedVector3(8);
			Rotation = stream.ReadQuaternion();
			CharacterState = (HkCharacterStateType)stream.ReadByte();
			SupportNormal = stream.ReadVector3();
			MovementSpeed = stream.ReadFloat();
			MovementDirection = stream.ReadVector3();
			IsOnLadder = stream.ReadBool();
			Valid = true;
		}

		public void Serialize(BitStream stream)
		{
			stream.WriteFloat(HeadX);
			stream.WriteFloat(HeadY);
			stream.WriteUInt16((ushort)MovementState);
			stream.WriteUInt16((ushort)MovementFlags);
			stream.WriteBool(Jetpack);
			stream.WriteBool(Dampeners);
			stream.WriteBool(TargetFromCamera);
			stream.WriteNormalizedSignedVector3(MoveIndicator, 8);
			stream.WriteQuaternion(Rotation);
			stream.WriteByte((byte)CharacterState);
			stream.Write(SupportNormal);
			stream.WriteFloat(MovementSpeed);
			stream.Write(MovementDirection);
			stream.WriteBool(IsOnLadder);
		}
	}
}
