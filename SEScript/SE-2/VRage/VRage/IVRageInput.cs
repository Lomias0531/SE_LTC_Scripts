using System.Collections.Generic;
using VRageMath;

namespace VRage
{
	public interface IVRageInput
	{
		Vector2 MousePosition
		{
			get;
			set;
		}

		Vector2 MouseAreaSize
		{
			get;
		}

		bool MouseCapture
		{
			get;
			set;
		}

		bool ShowCursor
		{
			get;
			set;
		}

		int KeyboardDelay
		{
			get;
		}

		int KeyboardSpeed
		{
			get;
		}

		void AddChar(char ch);

		void GetBufferedTextInput(ref List<char> currentTextInput);
	}
}
