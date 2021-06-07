using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Utils
{
	/// <summary>
	/// Temporaly stores information about item (event/place) existence. This is useful if you want to launch some actions only from time to time.
	/// You ask timed cache whether your last event still takes effect.
	/// </summary>
	public class MyTimedItemCache
	{
		private readonly HashSet<long> m_eventHappenedHere = new HashSet<long>();

		private readonly Queue<KeyValuePair<long, int>> m_eventQueue = new Queue<KeyValuePair<long, int>>();

		public int EventTimeoutMs
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="eventTimeoutMs">Time before </param>
		public MyTimedItemCache(int eventTimeoutMs)
		{
			EventTimeoutMs = eventTimeoutMs;
		}

		/// <summary>
		/// Generic item. Check whether generic item exists in the timed cache. 
		///
		/// autoinsert param: If the item is not found, it is inserted to the cache if the argument autoinsert is true.
		/// </summary>
		public bool IsItemPresent(long itemHashCode, int currentTimeMs, bool autoinsert = true)
		{
			while (m_eventQueue.Count > 0 && currentTimeMs > m_eventQueue.Peek().Value)
			{
				m_eventHappenedHere.Remove(m_eventQueue.Dequeue().Key);
			}
			if (!m_eventHappenedHere.Contains(itemHashCode))
			{
				if (autoinsert)
				{
					m_eventHappenedHere.Add(itemHashCode);
					m_eventQueue.Enqueue(new KeyValuePair<long, int>(itemHashCode, currentTimeMs + EventTimeoutMs));
				}
				return false;
			}
			return true;
		}

		/// <summary>
		/// Helper function. Check temporal usage of space. Check whether the place is taken. Internally converts position to generic item.
		/// Please use consistent eventSpaceMapping, otherwise cache will not find your items.
		///
		/// autoinsert param: If the item is not found, it is inserted to the cache if the argument autoinsert is true.
		/// </summary>
		public bool IsPlaceUsed(Vector3D position, double eventSpaceMapping, int currentTimeMs, bool autoinsert = true)
		{
			Vector3D vector3D = position * eventSpaceMapping;
			vector3D.X = Math.Floor(vector3D.X);
			vector3D.Y = Math.Floor(vector3D.Y);
			vector3D.Z = Math.Floor(vector3D.Z);
			long hash = vector3D.GetHash();
			return IsItemPresent(hash, currentTimeMs, autoinsert);
		}
	}
}
