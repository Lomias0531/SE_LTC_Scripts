using System;
using System.Collections.Generic;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// This is the entry point for all communication operations.
	/// </summary>
	public interface IMyIntergridCommunicationSystem
	{
		/// <summary>
		/// Gets communication address for current programmable block.
		/// </summary>
		long Me
		{
			get;
		}

		/// <summary>
		/// Gets unicast listener for current programmable block.
		/// </summary>
		IMyUnicastListener UnicastListener
		{
			get;
		}

		/// <summary>
		/// Determines if given endpoint is currently reachable. Similar to sending ICMP message.
		/// </summary>
		bool IsEndpointReachable(long address, TransmissionDistance transmissionDistance = TransmissionDistance.AntennaRelay);

		/// <summary>
		/// Registers broadcast listener with given tag for current programmable block. 
		/// In case there is already another active broadcast lister with given tag new listener is NOT registered and the already active one is returned instead.
		/// </summary>
		/// <param name="tag">String tag broadcast listener should listen for.</param>
		/// <returns>Active broadcast listener for given tag.</returns>
		IMyBroadcastListener RegisterBroadcastListener(string tag);

		/// <summary>
		/// Disables given broadcast listener. In case given broadcast listener is not active nothing happens.
		/// Instance of this broadcast listener remains valid and all pending messages may be accepted as normal.
		/// Disabling broadcast listener also disables it's message callback, if active.
		/// Consuming the last pending message will permanently disable the the provided listener and it's never going to be activated again.
		/// ==&gt; Registering new broadcast lister with the same tag will will allocate new listener instance instead.
		/// </summary>
		/// <param name="broadcastListener">Broadcast listener which should be deactivated.</param>
		void DisableBroadcastListener(IMyBroadcastListener broadcastListener);

		/// <summary>
		/// Retrieves list of all active broadcast listeners and listeners with pending messages, registered by current programmable block.
		/// Returned list is snapshot of current state and is not updated by future operations.
		/// </summary>
		/// <returns>List or all active broadcast listeners and listeners with pending messages.</returns>
		void GetBroadcastListeners(List<IMyBroadcastListener> broadcastListeners, Func<IMyBroadcastListener, bool> collect = null);

		/// <summary>
		/// Sends broadcast message with given content and tag. 
		/// This is fire and forget operation and cannot fail.
		/// Only broadcast listeners listening to this tag will accept this message.
		/// Important: Message will be delivered only to currently reachable IGC endpoints.
		/// </summary>
		/// <param name="data">Message data to be send.</param>
		/// <param name="tag">Tag of broadcast listeners this message should be accepted by.</param>
		/// <param name="transmissionDistance">Specifies how far will the be broadcasted</param>
		void SendBroadcastMessage<TData>(string tag, TData data, TransmissionDistance transmissionDistance = TransmissionDistance.AntennaRelay);

		/// <summary>
		/// Sends unicast message with given content to the PB with specified address.
		/// This operation may fail in case the given IGC endpoint is currently unreachable.
		/// </summary>
		/// <param name="data">Message data to be send.</param>
		/// <param name="tag">Tags the message type so that the receiving party has easier time recognizing the message</param>
		/// <param name="addressee">IGC endpoint to send this message to.</param>
		/// <returns></returns>
		bool SendUnicastMessage<TData>(long addressee, string tag, TData data);
	}
}
