using System.Runtime.InteropServices;

namespace VRage.Input
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MyMouseState
	{
		public int X;

		public int Y;

		public int ScrollWheelValue;

		public bool LeftButton;

		public bool RightButton;

		public bool MiddleButton;

		public bool XButton1;

		public bool XButton2;

		public MyMouseState(int x, int y, int scrollWheel, bool leftButton, bool middleButton, bool rightButton, bool xButton1, bool xButton2)
		{
			X = x;
			Y = y;
			ScrollWheelValue = scrollWheel;
			LeftButton = leftButton;
			MiddleButton = middleButton;
			RightButton = rightButton;
			XButton1 = xButton1;
			XButton2 = xButton2;
		}

		public bool Equals(ref MyMouseState state)
		{
			if (state.X == X && state.Y == Y && state.ScrollWheelValue == ScrollWheelValue && state.LeftButton == LeftButton && state.RightButton == RightButton && state.MiddleButton == MiddleButton && state.XButton1 == XButton1)
			{
				return state.XButton2 == XButton2;
			}
			return false;
		}

		public override string ToString()
		{
			return $"{X},{Y},{ScrollWheelValue} Buttons: {LeftButton}, {RightButton}, {MiddleButton}, {XButton1}, {XButton2}";
		}
	}
}
