using System;

namespace VRage
{
	/// <summary>
	///  Flags that define appearance and behaviour of a standard message box displayed by a call to the MessageBox function.
	///  </summary>    
	[Flags]
	public enum MessageBoxOptions
	{
		OkOnly = 0x0,
		OkCancel = 0x1,
		AbortRetryIgnore = 0x2,
		YesNoCancel = 0x3,
		YesNo = 0x4,
		RetryCancel = 0x5,
		CancelTryContinue = 0x6,
		IconHand = 0x10,
		IconQuestion = 0x20,
		IconExclamation = 0x30,
		IconAsterisk = 0x40
	}
}
