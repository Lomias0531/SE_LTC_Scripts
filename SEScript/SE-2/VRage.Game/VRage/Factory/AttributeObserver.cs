using System;

namespace VRage.Factory
{
	/// <summary>
	/// Delegate describing a method used to monitor type annotating attributes.
	/// </summary>
	/// <param name="type">The annotated type.</param>
	/// <param name="attr">The attribute annotating.</param>
	public delegate void AttributeObserver(Type type, Attribute attr);
}
