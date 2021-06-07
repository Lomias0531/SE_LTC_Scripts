using System;
using System.Collections.Generic;

namespace VRage
{
	public sealed class MyServiceManager
	{
		private static MyServiceManager singleton = new MyServiceManager();

		private Dictionary<Type, object> services;

		private object lockObject;

		public static MyServiceManager Instance => singleton;

		private MyServiceManager()
		{
			lockObject = new object();
			services = new Dictionary<Type, object>();
		}

		public void AddService<T>(T serviceInstance) where T : class
		{
			lock (lockObject)
			{
				services[typeof(T)] = serviceInstance;
			}
		}

		public T GetService<T>() where T : class
		{
			object value;
			lock (lockObject)
			{
				services.TryGetValue(typeof(T), out value);
			}
			return value as T;
		}

		public void RemoveService<T>()
		{
			lock (lockObject)
			{
				services.Remove(typeof(T));
			}
		}
	}
}
