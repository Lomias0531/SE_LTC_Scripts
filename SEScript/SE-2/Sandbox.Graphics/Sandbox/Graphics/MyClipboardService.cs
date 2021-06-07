using EmptyKeys.UserInterface.Mvvm;
using System.Threading;
using VRage;

namespace Sandbox.Graphics
{
	public class MyClipboardService : IClipboardService
	{
		private string m_clipboardText;

		public string GetText()
		{
			Thread thread = new Thread(PasteFromClipboard);
			thread.ApartmentState = ApartmentState.STA;
			thread.Start();
			thread.Join();
			return m_clipboardText;
		}

		private void PasteFromClipboard()
		{
			m_clipboardText = MyVRage.Platform.Clipboard;
		}

		public void SetText(string text)
		{
			MyVRage.Platform.Clipboard = text;
		}
	}
}
