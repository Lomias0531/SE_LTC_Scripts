namespace VRage
{
	public interface IMyImeProcessor
	{
		bool IsComposing
		{
			get;
		}

		void Activate(IMyImeActiveControl textElement);

		void Deactivate();

		void RecaptureTopScreen(IVRageGuiScreen screenWithFocus);

		void RegisterActiveScreen(IVRageGuiScreen screen);

		void UnregisterActiveScreen(IVRageGuiScreen screen);

		void ProcessInvoke();

		void CaretRepositionReaction();
	}
}
