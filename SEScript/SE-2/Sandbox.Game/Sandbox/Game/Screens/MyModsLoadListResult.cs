using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using System.Collections.Generic;
using VRage.GameServices;

namespace Sandbox.Game.Screens
{
	public class MyModsLoadListResult : IMyAsyncResult
	{
		public bool IsCompleted => Task.IsComplete;

		public Task Task
		{
			get;
			private set;
		}

		public List<MyWorkshopItem> SubscribedMods
		{
			get;
			private set;
		}

		public List<MyWorkshopItem> SetMods
		{
			get;
			private set;
		}

		public MyModsLoadListResult(HashSet<ulong> ids)
		{
			Task = Parallel.Start(delegate
			{
				SubscribedMods = new List<MyWorkshopItem>(ids.Count);
				SetMods = new List<MyWorkshopItem>();
				if (MyGameService.IsOnline && MyWorkshop.GetSubscribedModsBlocking(SubscribedMods))
				{
					HashSet<ulong> hashSet = new HashSet<ulong>(ids);
					foreach (MyWorkshopItem subscribedMod in SubscribedMods)
					{
						hashSet.Remove(subscribedMod.Id);
					}
					if (hashSet.Count > 0)
					{
						MyWorkshop.GetItemsBlockingUGC(hashSet, SetMods);
					}
				}
			});
		}
	}
}
