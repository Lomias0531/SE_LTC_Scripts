using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Library.Collections
{
	/// <summary>
	/// A simple thread safe manager for all sorts of pooled objects.
	///
	/// This 
	/// </summary>
	public static class PoolManager
	{
		public struct ReturnHandle<TObject> : IDisposable where TObject : new()
		{
			private TObject m_obj;

			public ReturnHandle(TObject data)
			{
				this = default(ReturnHandle<TObject>);
				m_obj = data;
			}

			public void Dispose()
			{
				Return(ref m_obj);
			}
		}

		public struct ArrayReturnHandle<TElement> : IDisposable
		{
			private TElement[] m_array;

			public ArrayReturnHandle(TElement[] data)
			{
				m_array = data;
			}

			public void Dispose()
			{
				ArrayPool<TElement>.Shared.Return(m_array);
			}
		}

		private static readonly ConcurrentDictionary<Type, IConcurrentPool> Pools = new ConcurrentDictionary<Type, IConcurrentPool>();

		/// <summary>
		/// Preallocate a number of elements for a given object pool.
		///
		/// This call only produces an effect if the pool has not yet been initialized.
		/// </summary>
		/// <typeparam name="TPooled">The type of the pooled object.</typeparam>
		/// <param name="size">The number of elements to preallocate, this can be zero.</param>
		public static void Preallocate<TPooled>(int size) where TPooled : new()
		{
			Type typeFromHandle = typeof(TPooled);
			if (!Pools.ContainsKey(typeFromHandle))
			{
				Pools[typeFromHandle] = GetPool<TPooled>(typeFromHandle, size);
			}
		}

		/// <summary>
		/// Get an instance from a pool.
		///
		/// Pooled resources represent data structures that are cached to prevent unnecessary memory allocations in time critical moments.
		///
		/// Pooled resources must always be return to the pool when no longer necessary.
		/// </summary>
		/// <typeparam name="TPooled">The type of the object to retrieve.</typeparam>
		/// <returns>Retrieve an instance of an object from the appropriate pool, if no pool exists one is created for the type.</returns>
		public static TPooled Get<TPooled>() where TPooled : new()
		{
			Type typeFromHandle = typeof(TPooled);
			if (!Pools.TryGetValue(typeFromHandle, out IConcurrentPool value))
			{
				value = (Pools[typeFromHandle] = GetPool<TPooled>(typeFromHandle));
			}
			return (TPooled)value.Get();
		}

		/// <summary>
		/// Get an instance from a pool.
		///
		/// Pooled resources represent data structures that are cached to prevent unnecessary memory allocations in time critical moments.
		///
		/// Pooled resources must always be return to the pool when no longer necessary.
		/// </summary>
		/// <typeparam name="TPooled">The type of the object to retrieve.</typeparam>
		/// <returns>Retrieve an instance of an object from the appropriate pool, if no pool exists one is created for the type.</returns>
		public static ReturnHandle<TPooled> Get<TPooled>(out TPooled poolObject) where TPooled : new()
		{
			Type typeFromHandle = typeof(TPooled);
			if (!Pools.TryGetValue(typeFromHandle, out IConcurrentPool value))
			{
				value = (Pools[typeFromHandle] = GetPool<TPooled>(typeFromHandle));
			}
			poolObject = (TPooled)value.Get();
			return new ReturnHandle<TPooled>(poolObject);
		}

		private static IConcurrentPool GetPool<TPooled>(Type type, int preallocated = 0) where TPooled : new()
		{
			Type typeFromHandle = typeof(ICollection<>);
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeFromHandle)
				{
					Type type3 = typeof(MyConcurrentCollectionPool<, >).MakeGenericType(type, type2.GetGenericArguments()[0]);
					return (IConcurrentPool)Activator.CreateInstance(type3, preallocated);
				}
			}
			return new MyConcurrentPool<TPooled>(preallocated);
		}

		/// <summary>
		/// Return an object to it's pool.
		///
		/// Note that if no pool has been allocated for the object type this call does nothing.
		///
		/// If this type of behaviour is undesirable one may preallocate the pool <see cref="M:VRage.Library.Collections.PoolManager.Preallocate``1(System.Int32)" />.
		/// In this case a preallocation of size 0 ensures the pool exists but does not allocate any instances.
		/// </summary>
		/// <typeparam name="TPooled">The type of the object to return to the pool.</typeparam>
		/// <param name="obj">An object to return to a pool.</param>
		public static void Return<TPooled>(ref TPooled obj) where TPooled : new()
		{
			Type typeFromHandle = typeof(TPooled);
			if (Pools.TryGetValue(typeFromHandle, out IConcurrentPool value))
			{
				value.Return(obj);
			}
			obj = default(TPooled);
		}

		public static ArrayReturnHandle<TElement> BorrowArray<TElement>(int size, out TElement[] array)
		{
			array = ArrayPool<TElement>.Shared.Rent(size);
			return new ArrayReturnHandle<TElement>(array);
		}

		public static ArrayReturnHandle<TElement> BorrowSpan<TElement>(int size, out Span<TElement> span)
		{
			TElement[] array = ArrayPool<TElement>.Shared.Rent(size);
			span = new Span<TElement>(array, 0, size);
			return new ArrayReturnHandle<TElement>(array);
		}
	}
}
