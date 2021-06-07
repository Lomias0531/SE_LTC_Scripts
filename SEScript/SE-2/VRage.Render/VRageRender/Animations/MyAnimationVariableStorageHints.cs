using System.Collections.Generic;
using VRage.Utils;

namespace VRageRender.Animations
{
	/// <summary>
	/// Variable storage - Hints for the user, common variable names, descriptions, string ids.
	/// </summary>
	public static class MyAnimationVariableStorageHints
	{
		public struct MyVariableNameHint
		{
			/// <summary>
			/// Variable name.
			/// </summary>
			public string Name;

			/// <summary>
			/// Description of the variable.
			/// </summary>
			public string Hint;

			public MyVariableNameHint(string name, string hint)
			{
				Name = name;
				Hint = hint;
			}
		}

		public static readonly Dictionary<MyStringId, MyVariableNameHint> VariableNameHints;

		public static readonly MyStringId StrIdAnimationFinished;

		public static readonly MyStringId StrIdRandom;

		public static readonly MyStringId StrIdRandomStable;

		public static readonly MyStringId StrIdCrouch;

		public static readonly MyStringId StrIdDead;

		public static readonly MyStringId StrIdFalling;

		public static readonly MyStringId StrIdFirstPerson;

		public static readonly MyStringId StrIdForcedFirstPerson;

		public static readonly MyStringId StrIdFlying;

		public static readonly MyStringId StrIdHoldingTool;

		public static readonly MyStringId StrIdIronsight;

		public static readonly MyStringId StrIdJumping;

		public static readonly MyStringId StrIdLean;

		public static readonly MyStringId StrIdShooting;

		public static readonly MyStringId StrIdSitting;

		public static readonly MyStringId StrIdSpeed;

		public static readonly MyStringId StrIdSpeedAngle;

		public static readonly MyStringId StrIdSpeedX;

		public static readonly MyStringId StrIdSpeedY;

		public static readonly MyStringId StrIdSpeedZ;

		public static readonly MyStringId StrIdTurningSpeed;

		public static readonly MyStringId StrIdLadder;

		public static readonly MyStringId StrIdHelmetOpen;

		public static readonly MyStringId StrIdActionJump;

		public static readonly MyStringId StrIdActionAttack;

		public static readonly MyStringId StrIdActionHurt;

		public static readonly MyStringId StrIdActionShout;

		static MyAnimationVariableStorageHints()
		{
			VariableNameHints = new Dictionary<MyStringId, MyVariableNameHint>(32);
			StrIdAnimationFinished = MyStringId.GetOrCompute("@animationfinished");
			StrIdRandom = MyStringId.GetOrCompute("@random");
			StrIdRandomStable = MyStringId.GetOrCompute("@randomstable");
			StrIdCrouch = MyStringId.GetOrCompute("crouch");
			StrIdDead = MyStringId.GetOrCompute("dead");
			StrIdFalling = MyStringId.GetOrCompute("falling");
			StrIdFirstPerson = MyStringId.GetOrCompute("firstperson");
			StrIdForcedFirstPerson = MyStringId.GetOrCompute("forcedFirstperson");
			StrIdFlying = MyStringId.GetOrCompute("flying");
			StrIdHoldingTool = MyStringId.GetOrCompute("holdingtool");
			StrIdIronsight = MyStringId.GetOrCompute("ironsight");
			StrIdJumping = MyStringId.GetOrCompute("jumping");
			StrIdLean = MyStringId.GetOrCompute("lean");
			StrIdShooting = MyStringId.GetOrCompute("shooting");
			StrIdSitting = MyStringId.GetOrCompute("sitting");
			StrIdSpeed = MyStringId.GetOrCompute("speed");
			StrIdSpeedAngle = MyStringId.GetOrCompute("speed_angle");
			StrIdSpeedX = MyStringId.GetOrCompute("speed_x");
			StrIdSpeedY = MyStringId.GetOrCompute("speed_y");
			StrIdSpeedZ = MyStringId.GetOrCompute("speed_z");
			StrIdTurningSpeed = MyStringId.GetOrCompute("turningspeed");
			StrIdLadder = MyStringId.GetOrCompute("ladder");
			StrIdHelmetOpen = MyStringId.GetOrCompute("helmetopen");
			StrIdActionJump = MyStringId.GetOrCompute("jump");
			StrIdActionAttack = MyStringId.GetOrCompute("attack");
			StrIdActionHurt = MyStringId.GetOrCompute("hurt");
			StrIdActionShout = MyStringId.GetOrCompute("shout");
			VariableNameHints.Add(StrIdAnimationFinished, new MyVariableNameHint("@animationfinished", "Percentage of animation played [0 - 1]"));
			VariableNameHints.Add(StrIdRandom, new MyVariableNameHint("@random", "Random number, unique number generated each access [0 - 1]"));
			VariableNameHints.Add(StrIdRandomStable, new MyVariableNameHint("@randomstable", "Random number, unique number generated each frame [0 - 1]"));
			VariableNameHints.Add(StrIdCrouch, new MyVariableNameHint("crouch", "Character is crouched [0 or 1]"));
			VariableNameHints.Add(StrIdDead, new MyVariableNameHint("dead", "Character is dead [0 or 1]"));
			VariableNameHints.Add(StrIdFalling, new MyVariableNameHint("falling", "Character is falling [0 or 1]"));
			VariableNameHints.Add(StrIdFirstPerson, new MyVariableNameHint("firstperson", "Character camera is in first person mode [0 or 1]"));
			VariableNameHints.Add(StrIdForcedFirstPerson, new MyVariableNameHint("forcedfirstperson", "Character camera is in forced first person mode [0 or 1]"));
			VariableNameHints.Add(StrIdFlying, new MyVariableNameHint("flying", "Character is flying [0 or 1]"));
			VariableNameHints.Add(StrIdHoldingTool, new MyVariableNameHint("holdingtool", "Character is holding a tool [0 or 1]"));
			VariableNameHints.Add(StrIdIronsight, new MyVariableNameHint("ironsight", "Character is using ironsight to aim [0 or 1]"));
			VariableNameHints.Add(StrIdJumping, new MyVariableNameHint("jumping", "Character is jumping [0 or 1]"));
			VariableNameHints.Add(StrIdLean, new MyVariableNameHint("lean", "Character leaning angle [-90 - 90]"));
			VariableNameHints.Add(StrIdShooting, new MyVariableNameHint("shooting", "Character is shooting [0 or 1]"));
			VariableNameHints.Add(StrIdSitting, new MyVariableNameHint("sitting", "Character is sitting [0 or 1]"));
			VariableNameHints.Add(StrIdSpeed, new MyVariableNameHint("speed", "Character speed [0 or more]"));
			VariableNameHints.Add(StrIdSpeedAngle, new MyVariableNameHint("speed_angle", "Character movement angle [0 - 360, clockwise]"));
			VariableNameHints.Add(StrIdSpeedX, new MyVariableNameHint("speed_x", "Character x speed (left-right) [m/s]"));
			VariableNameHints.Add(StrIdSpeedY, new MyVariableNameHint("speed_y", "Character y speed (down-up) [m/s]"));
			VariableNameHints.Add(StrIdSpeedZ, new MyVariableNameHint("speed_z", "Character z speed (backward-forward) [m/s]"));
			VariableNameHints.Add(StrIdTurningSpeed, new MyVariableNameHint("turningspeed", "Character turning speed [clockwise, degrees]"));
		}
	}
}
