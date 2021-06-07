using System;
using VRage.Network;

namespace VRageRender.Messages
{
	[GenerateActivator]
	public class UpdateData
	{
		public readonly Type DataType;

		public Type ComponentType
		{
			get;
			internal set;
		}

		protected UpdateData(Type componentType)
		{
			DataType = GetType();
			ComponentType = componentType;
		}

		public UpdateData()
		{
			throw new Exception("Invalid constructor");
		}

		public T As<T>() where T : UpdateData
		{
			return (T)this;
		}
	}
	public class UpdateData<T> : UpdateData
	{
		public UpdateData()
			: base(typeof(T))
		{
		}
	}
}
