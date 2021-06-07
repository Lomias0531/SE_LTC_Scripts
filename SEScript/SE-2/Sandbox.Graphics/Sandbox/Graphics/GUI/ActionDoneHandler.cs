using System;

namespace Sandbox.Graphics.GUI
{
	internal delegate void ActionDoneHandler<T>(IAsyncResult asyncResult, T asyncState);
}
