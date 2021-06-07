using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public static class MyDirectionExtensions
	{
		public static float HorizontalFactor(this MyDirection direction)
		{
			if (direction != MyDirection.Left && direction != MyDirection.Right)
			{
				return 11f;
			}
			return 1f;
		}

		public static float VerticalFactor(this MyDirection direction)
		{
			if (direction != MyDirection.Left && direction != MyDirection.Right)
			{
				return 1f;
			}
			return 11f;
		}

		public static float Distance(this MyDirection direction, Vector2 v1, Vector2 v2)
		{
			return ((v1 - v2) * new Vector2(direction.HorizontalFactor(), direction.VerticalFactor())).Length();
		}

		public static float GetDistance(this MyDirection direction, ref RectangleF focused, ref RectangleF next)
		{
			bool flag = next.Right < focused.X;
			bool flag2 = focused.Right < next.X;
			bool flag3 = next.Bottom < focused.Y;
			bool flag4 = focused.Bottom < next.Y;
			if (flag4 && flag)
			{
				return direction.Distance(new Vector2(focused.X, focused.Bottom), new Vector2(next.Right, next.Y));
			}
			if (flag && flag3)
			{
				return direction.Distance(new Vector2(focused.X, focused.Y), new Vector2(next.Right, next.Bottom));
			}
			if (flag3 && flag2)
			{
				return direction.Distance(new Vector2(focused.Right, focused.Y), new Vector2(next.X, next.Bottom));
			}
			if (flag2 && flag4)
			{
				return direction.Distance(new Vector2(focused.Right, focused.Bottom), new Vector2(next.X, next.Y));
			}
			if (flag)
			{
				return (focused.X - next.Right) * direction.HorizontalFactor();
			}
			if (flag2)
			{
				return (next.X - focused.Right) * direction.HorizontalFactor();
			}
			if (flag3)
			{
				return (focused.Y - next.Bottom) * direction.VerticalFactor();
			}
			if (flag4)
			{
				return (next.Y - focused.Bottom) * direction.VerticalFactor();
			}
			return 0f;
		}

		public static float ControlDistance(this MyDirection direction, ref RectangleF focused, ref RectangleF next)
		{
			bool flag;
			switch (direction)
			{
			case MyDirection.Up:
				flag = (focused.Y >= next.Y + next.Height - 0.0032f);
				break;
			case MyDirection.Down:
				flag = (focused.Y + focused.Height <= next.Y + 0.0032f);
				break;
			case MyDirection.Left:
				flag = (focused.X >= next.X + next.Width - 0.0032f);
				break;
			case MyDirection.Right:
				flag = (focused.X + focused.Width <= next.X + 0.0032f);
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
				return direction.GetDistance(ref focused, ref next);
			}
			return -1f;
		}
	}
}
