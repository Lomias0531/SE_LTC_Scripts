using System;
using System.IO;
using VRageMath;

namespace VRage
{
	public interface IVRageWindow
	{
		/// <summary>
		/// True when Present on device should be called (e.g. window not minimized)
		/// </summary>
		bool DrawEnabled
		{
			get;
		}

		bool IsActive
		{
			get;
		}

		Vector2I ClientSize
		{
			get;
		}

		event Action OnExit;

		void DoEvents();

		void Exit();

		bool UpdateRenderThread();

		void UpdateMainThread();

		void SetCursor(Stream stream);

		void AddMessageHandler(uint wm, ActionRef<MyMessage> action);

		void RemoveMessageHandler(uint wm, ActionRef<MyMessage> action);

		void ShowAndFocus();

		/// <summary>
		/// Hide this window from view.
		/// </summary>
		void Hide();
	}
}
