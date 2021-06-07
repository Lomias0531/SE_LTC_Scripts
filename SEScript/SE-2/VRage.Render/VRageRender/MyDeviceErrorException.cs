using System;

namespace VRageRender
{
	public class MyDeviceErrorException : Exception
	{
		public new string Message;

		public MyDeviceErrorException(string message)
		{
			Message = message;
		}
	}
}
