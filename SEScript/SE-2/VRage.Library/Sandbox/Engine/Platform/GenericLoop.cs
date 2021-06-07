namespace Sandbox.Engine.Platform
{
	public class GenericLoop
	{
		public delegate void VoidAction();

		private VoidAction m_tickCallback;

		public bool IsDone;

		public virtual void Run(VoidAction tickCallback)
		{
			m_tickCallback = tickCallback;
			while (!IsDone)
			{
				m_tickCallback();
			}
		}
	}
}
