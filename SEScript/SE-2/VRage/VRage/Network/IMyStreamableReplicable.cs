using System;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Network
{
	public interface IMyStreamableReplicable
	{
		bool NeedsToBeStreamed
		{
			get;
		}

		void Serialize(BitStream stream, HashSet<string> cachedData, Endpoint forClient, Action writeData);

		void LoadDone(BitStream stream);

		void LoadCancel();

		void CreateStreamingStateGroup();

		IMyStateGroup GetStreamingStateGroup();

		/// <summary>
		/// Client deserializes object and adds it to proper collection (e.g. MyEntities).
		/// Loading done handler can be called synchronously or asynchronously (but always from Update thread).
		/// </summary>
		void OnLoadBegin(Action<bool> loadingDoneHandler);
	}
}
