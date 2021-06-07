using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
	public interface IMyPlayerCollection
	{
		long Count
		{
			get;
		}

		void ExtendControl(IMyControllableEntity entityWithControl, IMyEntity entityGettingControl);

		void GetPlayers(List<IMyPlayer> players, Func<IMyPlayer, bool> collect = null);

		bool HasExtendedControl(IMyControllableEntity firstEntity, IMyEntity secondEntity);

		void ReduceControl(IMyControllableEntity entityWhichKeepsControl, IMyEntity entityWhichLoosesControl);

		void RemoveControlledEntity(IMyEntity entity);

		void TryExtendControl(IMyControllableEntity entityWithControl, IMyEntity entityGettingControl);

		bool TryReduceControl(IMyControllableEntity entityWhichKeepsControl, IMyEntity entityWhichLoosesControl);

		void SetControlledEntity(ulong steamUserId, IMyEntity entity);

		IMyPlayer GetPlayerControllingEntity(IMyEntity entity);

		void GetAllIdentites(List<IMyIdentity> identities, Func<IMyIdentity, bool> collect = null);

		long TryGetIdentityId(ulong steamId);

		ulong TryGetSteamId(long identityId);

		/// <summary>
		/// Requests change of the balance (money) for specific identity id
		/// </summary>
		/// <param name="identityId">identity id</param>
		/// <param name="amount">amount to be added/subtracted</param>
		void RequestChangeBalance(long identityId, long amount);
	}
}
