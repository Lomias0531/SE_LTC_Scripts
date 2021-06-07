using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Contains the CurveKeys making up a Curve.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class CurveKeyCollection : ICollection<CurveKey>, IEnumerable<CurveKey>, IEnumerable
	{
		protected class VRageMath_CurveKeyCollection_003C_003EKeys_003C_003EAccessor : IMemberAccessor<CurveKeyCollection, List<CurveKey>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKeyCollection owner, in List<CurveKey> value)
			{
				owner.Keys = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKeyCollection owner, out List<CurveKey> value)
			{
				value = owner.Keys;
			}
		}

		protected class VRageMath_CurveKeyCollection_003C_003EIsCacheAvailable_003C_003EAccessor : IMemberAccessor<CurveKeyCollection, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKeyCollection owner, in bool value)
			{
				owner.IsCacheAvailable = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKeyCollection owner, out bool value)
			{
				value = owner.IsCacheAvailable;
			}
		}

		protected class VRageMath_CurveKeyCollection_003C_003ETimeRange_003C_003EAccessor : IMemberAccessor<CurveKeyCollection, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKeyCollection owner, in float value)
			{
				owner.TimeRange = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKeyCollection owner, out float value)
			{
				value = owner.TimeRange;
			}
		}

		protected class VRageMath_CurveKeyCollection_003C_003EInvTimeRange_003C_003EAccessor : IMemberAccessor<CurveKeyCollection, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKeyCollection owner, in float value)
			{
				owner.InvTimeRange = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKeyCollection owner, out float value)
			{
				value = owner.InvTimeRange;
			}
		}

		private List<CurveKey> Keys = new List<CurveKey>();

		internal bool IsCacheAvailable = true;

		internal float TimeRange;

		internal float InvTimeRange;

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The array index of the element.</param>
		public CurveKey this[int index]
		{
			get
			{
				return Keys[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if ((double)Keys[index].Position == (double)value.Position)
				{
					Keys[index] = value;
					return;
				}
				Keys.RemoveAt(index);
				Add(value);
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the CurveKeyCollection.
		/// </summary>
		public int Count => Keys.Count;

		/// <summary>
		/// Returns a value indicating whether the CurveKeyCollection is read-only.
		/// </summary>
		public bool IsReadOnly => false;

		public void Add(object tmp)
		{
		}

		/// <summary>
		/// Determines the index of a CurveKey in the CurveKeyCollection.
		/// </summary>
		/// <param name="item">CurveKey to locate in the CurveKeyCollection.</param>
		public int IndexOf(CurveKey item)
		{
			return Keys.IndexOf(item);
		}

		/// <summary>
		/// Removes the CurveKey at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveAt(int index)
		{
			Keys.RemoveAt(index);
			IsCacheAvailable = false;
		}

		/// <summary>
		/// Adds a CurveKey to the CurveKeyCollection.
		/// </summary>
		/// <param name="item">The CurveKey to add.</param>
		public void Add(CurveKey item)
		{
			if (item == null)
			{
				throw new ArgumentNullException();
			}
			int i = Keys.BinarySearch(item);
			if (i >= 0)
			{
				for (; i < Keys.Count && (double)item.Position == (double)Keys[i].Position; i++)
				{
				}
			}
			else
			{
				i = ~i;
			}
			Keys.Insert(i, item);
			IsCacheAvailable = false;
		}

		/// <summary>
		/// Removes all CurveKeys from the CurveKeyCollection.
		/// </summary>
		public void Clear()
		{
			Keys.Clear();
			TimeRange = (InvTimeRange = 0f);
			IsCacheAvailable = false;
		}

		/// <summary>
		/// Determines whether the CurveKeyCollection contains a specific CurveKey.
		/// </summary>
		/// <param name="item">true if the CurveKey is found in the CurveKeyCollection; false otherwise.</param>
		public bool Contains(CurveKey item)
		{
			return Keys.Contains(item);
		}

		/// <summary>
		/// Copies the CurveKeys of the CurveKeyCollection to an array, starting at the array index provided.
		/// </summary>
		/// <param name="array">The destination of the CurveKeys copied from CurveKeyCollection. The array must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in the array to start copying from.</param>
		public void CopyTo(CurveKey[] array, int arrayIndex)
		{
			Keys.CopyTo(array, arrayIndex);
			IsCacheAvailable = false;
		}

		/// <summary>
		/// Removes the first occurrence of a specific CurveKey from the CurveKeyCollection.
		/// </summary>
		/// <param name="item">The CurveKey to remove from the CurveKeyCollection.</param>
		public bool Remove(CurveKey item)
		{
			IsCacheAvailable = false;
			return Keys.Remove(item);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the CurveKeyCollection.
		/// </summary>
		public IEnumerator<CurveKey> GetEnumerator()
		{
			return Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Keys.GetEnumerator();
		}

		/// <summary>
		/// Creates a copy of the CurveKeyCollection.
		/// </summary>
		public CurveKeyCollection Clone()
		{
			return new CurveKeyCollection
			{
				Keys = new List<CurveKey>(Keys),
				InvTimeRange = InvTimeRange,
				TimeRange = TimeRange,
				IsCacheAvailable = true
			};
		}

		internal void ComputeCacheValues()
		{
			TimeRange = (InvTimeRange = 0f);
			if (Keys.Count > 1)
			{
				TimeRange = Keys[Keys.Count - 1].Position - Keys[0].Position;
				if ((double)TimeRange > 1.40129846432482E-45)
				{
					InvTimeRange = 1f / TimeRange;
				}
			}
			IsCacheAvailable = true;
		}
	}
}
