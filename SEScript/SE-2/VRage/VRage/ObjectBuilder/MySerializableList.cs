using System.Collections.Generic;

namespace VRage.ObjectBuilder
{
	/// <summary>
	/// Custom list class to be used with object builders.
	///
	/// Thsi class provides a custom Add method that ignores null values,
	/// this is useful in conjunction with the abstract xml serializer
	/// because when elements are not deserializable they are returned as null.
	/// </summary>
	/// <typeparam name="TItem">The type of the list element.</typeparam>
	public class MySerializableList<TItem> : List<TItem>
	{
		public MySerializableList()
		{
		}

		public MySerializableList(int capacity)
			: base(capacity)
		{
		}

		public MySerializableList(IEnumerable<TItem> collection)
			: base(collection)
		{
		}

		/// <summary>
		/// Add value while checking if the value is not null.
		/// </summary>
		/// <param name="item"></param>
		public new void Add(TItem item)
		{
			if (item != null)
			{
				base.Add(item);
			}
		}
	}
}
