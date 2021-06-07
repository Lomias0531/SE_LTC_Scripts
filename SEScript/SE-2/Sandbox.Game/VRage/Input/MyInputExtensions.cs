using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.World;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Input
{
	public static class MyInputExtensions
	{
		public const float MOUSE_ROTATION_INDICATOR_MULTIPLIER = 0.075f;

		public const float ROTATION_INDICATOR_MULTIPLIER = 0.15f;

		public static float GetRoll(this IMyInput self)
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			return MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_RIGHT) - MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_LEFT);
		}

		public static float GetDeveloperRoll(this IMyInput self)
		{
			return 0f + (self.IsGameControlPressed(MyControlsSpace.ROLL_LEFT) ? (-1f) : 0f) + (self.IsGameControlPressed(MyControlsSpace.ROLL_RIGHT) ? 1f : 0f);
		}

		public static Vector3 GetPositionDelta(this IMyInput self)
		{
			Vector3 zero = Vector3.Zero;
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			zero.X = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.STRAFE_RIGHT) - MyControllerHelper.IsControlAnalog(context, MyControlsSpace.STRAFE_LEFT);
			zero.Y = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.JUMP) - MyControllerHelper.IsControlAnalog(context, MyControlsSpace.CROUCH);
			zero.Z = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.BACKWARD) - MyControllerHelper.IsControlAnalog(context, MyControlsSpace.FORWARD);
			return zero;
		}

		public static Vector2 GetRotation(this IMyInput self)
		{
			Vector2 result = Vector2.Zero;
			result = new Vector2(self.GetMouseYForGamePlayF(), self.GetMouseXForGamePlayF()) * 0.075f;
			MyStringId cX_CHARACTER = MyControllerHelper.CX_CHARACTER;
			result.X -= MyControllerHelper.IsControlAnalog(cX_CHARACTER, MyControlsSpace.ROTATION_UP);
			result.X += MyControllerHelper.IsControlAnalog(cX_CHARACTER, MyControlsSpace.ROTATION_DOWN);
			result.Y -= MyControllerHelper.IsControlAnalog(cX_CHARACTER, MyControlsSpace.ROTATION_LEFT);
			result.Y += MyControllerHelper.IsControlAnalog(cX_CHARACTER, MyControlsSpace.ROTATION_RIGHT);
			result *= 9f;
			return result;
		}

		public static Vector2 GetCursorPositionDelta(this IMyInput self)
		{
			Vector2 one = Vector2.One;
			return new Vector2(self.GetMouseX(), self.GetMouseY()) * one;
		}

		public static float GetRoll(this VRage.ModAPI.IMyInput self)
		{
			return ((IMyInput)self).GetRoll();
		}

		public static Vector3 GetPositionDelta(this VRage.ModAPI.IMyInput self)
		{
			return ((IMyInput)self).GetPositionDelta();
		}

		public static Vector2 GetRotation(this VRage.ModAPI.IMyInput self)
		{
			return ((IMyInput)self).GetRotation();
		}

		public static Vector2 GetCursorPositionDelta(this VRage.ModAPI.IMyInput self)
		{
			return ((IMyInput)self).GetCursorPositionDelta();
		}
	}
}
