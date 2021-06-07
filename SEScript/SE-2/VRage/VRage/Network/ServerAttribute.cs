using System;

namespace VRage.Network
{
	/// <summary>
	/// Server method. Decorated method is be called by client on server or by server locally.
	/// Server performs validation. Pass null as validation method to perform no validation (not recommended)
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerAttribute : Attribute
	{
		public readonly string Validation;

		public readonly ValidationType ValidationFlags;

		public ServerAttribute()
		{
		}

		public ServerAttribute(string validationMethod)
		{
			Validation = validationMethod;
		}

		public ServerAttribute(ValidationType validationFlags)
		{
			ValidationFlags = validationFlags;
		}
	}
}
