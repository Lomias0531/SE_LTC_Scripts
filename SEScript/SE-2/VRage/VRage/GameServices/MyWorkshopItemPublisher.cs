using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public class MyWorkshopItemPublisher : IDisposable
	{
		public delegate void PublishItemResult(MyGameServiceCallResult result, ulong publishedId);

		private class MyNameComparer : IComparer<MyWorkshopItem>
		{
			public int Compare(MyWorkshopItem x, MyWorkshopItem y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x != null && y == null)
				{
					return 1;
				}
				if (x == null && y != null)
				{
					return -1;
				}
				return string.CompareOrdinal(x.Title, y.Title);
			}
		}

		public List<string> Tags = new List<string>();

		public List<ulong> Dependencies = new List<ulong>();

		public HashSet<uint> DLCs = new HashSet<uint>();

		public static IComparer<MyWorkshopItem> NameComparer = new MyNameComparer();

		public string Title
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Thumbnail
		{
			get;
			set;
		}

		public string Folder
		{
			get;
			set;
		}

		public ulong Id
		{
			get;
			set;
		}

		public MyModMetadata Metadata
		{
			get;
			set;
		}

		public MyPublishedFileVisibility Visibility
		{
			get;
			set;
		}

		public event PublishItemResult ItemPublished;

		protected MyWorkshopItemPublisher()
		{
		}

		~MyWorkshopItemPublisher()
		{
			Dispose();
		}

		public virtual void Dispose()
		{
			if (Tags != null)
			{
				Tags.Clear();
			}
			if (Dependencies != null)
			{
				Dependencies.Clear();
			}
			if (DLCs != null)
			{
				DLCs.Clear();
			}
			Metadata = null;
		}

		/// <summary>
		/// Publish the item to workshop.
		/// If 'id' is filled in, it will try to update the existing item.
		/// If no 'id is filled in, it will create a new item instead.
		/// </summary>
		public virtual void Publish()
		{
		}

		protected virtual void OnItemPublished(MyGameServiceCallResult result, ulong publishedId)
		{
			this.ItemPublished?.Invoke(result, publishedId);
		}

		protected bool Equals(MyWorkshopItem other)
		{
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((MyWorkshopItem)obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[{0}] {1}", Id, Title ?? "N/A");
		}
	}
}
