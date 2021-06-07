using System.Reflection;

namespace Sandbox.Game.Gui
{
	public static class MemberInfoExtensions
	{
		public static object GetValue(this MemberInfo info, object instance)
		{
			object result = null;
			FieldInfo fieldInfo = info as FieldInfo;
			if (fieldInfo != null)
			{
				result = fieldInfo.GetValue(instance);
			}
			PropertyInfo propertyInfo = info as PropertyInfo;
			if (propertyInfo != null && propertyInfo.GetIndexParameters().Length == 0)
			{
				result = propertyInfo.GetValue(instance, null);
			}
			return result;
		}
	}
}
