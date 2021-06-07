namespace System.Reflection
{
	public static class AttributeExtensions
	{
		public static bool HasAttribute<T>(this MemberInfo element) where T : Attribute
		{
			return Attribute.IsDefined(element, typeof(T));
		}
	}
}
