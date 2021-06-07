namespace VRage.Game
{
	public static class MyCharacterMovement
	{
		public const ushort MovementTypeMask = 15;

		public const ushort MovementDirectionMask = 1008;

		public const ushort MovementSpeedMask = 3072;

		public const ushort RotationMask = 12288;

		public const ushort Standing = 0;

		public const ushort Sitting = 1;

		public const ushort Crouching = 2;

		public const ushort Flying = 3;

		public const ushort Falling = 4;

		public const ushort Jump = 5;

		public const ushort Died = 6;

		public const ushort Ladder = 7;

		public const ushort NoDirection = 0;

		public const ushort Forward = 16;

		public const ushort Backward = 32;

		public const ushort Left = 64;

		public const ushort Right = 128;

		public const ushort Up = 256;

		public const ushort Down = 512;

		public const ushort NormalSpeed = 0;

		public const ushort Fast = 1024;

		public const ushort VeryFast = 2048;

		public const ushort NotRotating = 0;

		public const ushort RotatingLeft = 4096;

		public const ushort RotatingRight = 8192;

		public const ushort LadderOut = 16384;

		public static ushort GetMode(this MyCharacterMovementEnum value)
		{
			return (ushort)(value & (MyCharacterMovementEnum)15);
		}

		public static ushort GetDirection(this MyCharacterMovementEnum value)
		{
			return (ushort)(value & (MyCharacterMovementEnum)1008);
		}

		public static ushort GetSpeed(this MyCharacterMovementEnum value)
		{
			return (ushort)(value & (MyCharacterMovementEnum)3072);
		}
	}
}
