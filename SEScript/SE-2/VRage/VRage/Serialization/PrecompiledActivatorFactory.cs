using System;
using System.Runtime.CompilerServices;
using VRage.Library.Extensions;
using VRage.Network;

namespace VRage.Serialization
{
	/// <summary>
	/// Activator factory that supports creating instances of precompiled types.
	/// </summary>
	public sealed class PrecompiledActivatorFactory : IActivatorFactory
	{
		private abstract class ActivatorWrapper<TBase>
		{
			public abstract TBase Create();
		}

		private class ActivatorWrapper<TBase, TInstance> : ActivatorWrapper<TBase> where TInstance : TBase
		{
			private readonly IActivator<TInstance> m_instance;

			public ActivatorWrapper(IActivator<TInstance> instance)
			{
				m_instance = instance;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public override TBase Create()
			{
				return (TBase)(object)m_instance.CreateInstance();
			}
		}

		private readonly IActivatorFactory m_fallback = new ExpressionBaseActivatorFactory();

		/// <inheritdoc />
		public Func<T> CreateActivator<T>()
		{
			IActivator<T> activator = CodegenUtils.GetActivator<T>();
			if (activator != null)
			{
				return activator.CreateInstance;
			}
			return m_fallback.CreateActivator<T>();
		}

		/// <inheritdoc />
		public Func<T> CreateActivator<T>(Type subtype)
		{
			IActivator activator = CodegenUtils.GetActivator(subtype);
			if (activator != null)
			{
				return ((ActivatorWrapper<T>)Activator.CreateInstance(typeof(ActivatorWrapper<, >).MakeGenericType(typeof(T), subtype), activator)).Create;
			}
			return m_fallback.CreateActivator<T>(subtype);
		}
	}
}
