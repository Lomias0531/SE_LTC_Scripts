using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public class MyWorkshopQuery : IDisposable
	{
		public delegate void QueryCompletedDelegate(MyGameServiceCallResult result);

		/// <summary>
		/// Type of item to query.
		/// </summary>
		public WorkshopItemType ItemType;

		/// <summary>
		/// Type of item list to query from.
		/// </summary>
		public WorkshopListType ListType;

		/// <summary>
		/// Returned workshop items.
		/// </summary>
		public List<MyWorkshopItem> Items
		{
			get;
			protected set;
		}

		/// <summary>
		/// Total results returned.
		/// </summary>
		public uint TotalResults
		{
			get;
			protected set;
		}

		/// <summary>
		/// How many items can be queried per page.
		/// </summary>
		public virtual uint ItemsPerPage
		{
			get;
			protected set;
		}

		/// <summary>
		/// Whether or not the query is currently running.
		/// </summary>
		public virtual bool IsRunning
		{
			get;
			protected set;
		}

		public string SearchString
		{
			get;
			set;
		}

		/// <summary>
		/// Tags to match
		/// </summary>
		public List<string> RequiredTags
		{
			get;
			set;
		}

		/// <summary>
		/// Do all required tags needs to be present, or just any?
		/// </summary>
		public bool RequireAllTags
		{
			get;
			set;
		}

		/// <summary>
		/// Don't match if any of these tags are included
		/// </summary>
		public List<string> ExcludedTags
		{
			get;
			set;
		}

		/// <summary>
		/// Search only items subscribed by the user specified.
		/// </summary>
		public ulong UserId
		{
			get;
			set;
		}

		/// <summary>
		/// Query only item ids specified here.
		/// </summary>
		public List<ulong> ItemIds
		{
			get;
			set;
		}

		public event QueryCompletedDelegate QueryCompleted;

		protected MyWorkshopQuery()
		{
		}

		~MyWorkshopQuery()
		{
			Dispose();
		}

		/// <summary>
		/// Runs the query with specified parameters.
		/// Does nothing if the query is already running.
		/// </summary>
		public virtual void Run()
		{
		}

		public virtual void Dispose()
		{
		}

		protected virtual void OnQueryCompleted(MyGameServiceCallResult result)
		{
			this.QueryCompleted?.Invoke(result);
		}
	}
}
