using System;

namespace VRage
{
	/// <summary>
	/// Specifies a static read-only default value field for structs
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class StructDefaultAttribute : Attribute
	{
	}
}
