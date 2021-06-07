using System;

namespace VRage.Serialization.Xml
{
	/// <summary>
	/// Indicates that a class should be indexed for use with MyAbstractXmlSerializer.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class MyXmlSerializableAttribute : Attribute
	{
	}
}
