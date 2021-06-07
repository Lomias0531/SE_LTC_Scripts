namespace VRage
{
	public interface IVRageGuiScreen
	{
		IVRageGuiControl FocusedControl
		{
			get;
			set;
		}

		bool IsOpened
		{
			get;
		}

		void AddControl(IVRageGuiControl control);

		bool RemoveControl(IVRageGuiControl control);

		bool ContainsControl(IVRageGuiControl control);
	}
}
