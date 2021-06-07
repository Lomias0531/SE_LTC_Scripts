using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public interface IMyGuiControlsOwner
	{
		List<MyGuiControlBase> VisibleElements
		{
			get;
		}

		bool Visible
		{
			get;
		}

		string DebugNamePath
		{
			get;
		}

		string Name
		{
			get;
		}

		IMyGuiControlsOwner Owner
		{
			get;
		}

		Vector2 GetPositionAbsolute();

		Vector2 GetPositionAbsoluteTopLeft();

		Vector2 GetPositionAbsoluteCenter();

		Vector2? GetSize();

		void OnFocusChanged(MyGuiControlBase control, bool focus);

		MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page);
	}
}
