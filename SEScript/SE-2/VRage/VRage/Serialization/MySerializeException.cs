using System;

namespace VRage.Serialization
{
	public class MySerializeException : Exception
	{
		public MySerializeErrorEnum Error;

		public MySerializeException(MySerializeErrorEnum error)
		{
			Error = error;
		}
	}
}
