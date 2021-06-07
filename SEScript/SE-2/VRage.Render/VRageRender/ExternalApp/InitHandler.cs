using VRage;

namespace VRageRender.ExternalApp
{
	/// <summary>
	/// Initializes window on render thread and returns it's handle of window/control where to draw
	/// </summary>
	public delegate IVRageWindow InitHandler();
}
