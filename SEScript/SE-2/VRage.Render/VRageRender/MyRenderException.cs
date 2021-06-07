using System;

namespace VRageRender
{
	public class MyRenderException : Exception
	{
		private MyRenderExceptionEnum m_type;

		public MyRenderExceptionEnum Type => m_type;

		public MyRenderException(string message, MyRenderExceptionEnum type = MyRenderExceptionEnum.Unassigned)
			: base(message)
		{
			m_type = type;
		}
	}
}
