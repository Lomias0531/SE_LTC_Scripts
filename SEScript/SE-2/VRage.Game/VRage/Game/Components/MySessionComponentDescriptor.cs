using System;
using VRage.ObjectBuilders;

namespace VRage.Game.Components
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class MySessionComponentDescriptor : Attribute
	{
		public MyUpdateOrder UpdateOrder;

		/// <summary>
		/// Lower Priority is loaded before higher Priority
		/// </summary>
		public int Priority;

		public MyObjectBuilderType ObjectBuilderType;

		public Type ComponentType;

		public MySessionComponentDescriptor(MyUpdateOrder updateOrder)
			: this(updateOrder, 1000)
		{
		}

		public MySessionComponentDescriptor(MyUpdateOrder updateOrder, int priority)
			: this(updateOrder, priority, null)
		{
		}

		public MySessionComponentDescriptor(MyUpdateOrder updateOrder, int priority, Type obType, Type registrationType = null)
		{
			UpdateOrder = updateOrder;
			Priority = priority;
			ObjectBuilderType = obType;
			if (obType != null && !typeof(MyObjectBuilder_SessionComponent).IsAssignableFrom(obType))
			{
				ObjectBuilderType = MyObjectBuilderType.Invalid;
			}
			ComponentType = registrationType;
		}
	}
}
