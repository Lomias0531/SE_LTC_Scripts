using System;
using System.Collections.Generic;

namespace VRage.Input
{
	public interface IVRageInput2 : IDisposable
	{
		void GetMouseState(out MyMouseState state);

		List<string> EnumerateJoystickNames();

		string InitializeJoystickIfPossible(string joystickInstanceName);

		bool IsJoystickAxisSupported(MyJoystickAxesEnum axis);

		bool IsJoystickConnected();

		void GetJoystickState(ref MyJoystickState state);

		void ShowVirtualKeyboardIfNeeded(Action<string> onSuccess, Action onCancel = null, string defaultText = null, string title = null, int maxLength = 0);

		unsafe void GetAsyncKeyStates(byte* data);
	}
}
