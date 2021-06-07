using VRage.Collections;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyFaction
	{
		long FactionId
		{
			get;
		}

		string Tag
		{
			get;
		}

		string Name
		{
			get;
		}

		string Description
		{
			get;
		}

		string PrivateInfo
		{
			get;
		}

		MyStringId? FactionIcon
		{
			get;
		}

		bool AutoAcceptMember
		{
			get;
		}

		bool AutoAcceptPeace
		{
			get;
		}

		bool AcceptHumans
		{
			get;
		}

		long FounderId
		{
			get;
		}

		Vector3 CustomColor
		{
			get;
		}

		Vector3 IconColor
		{
			get;
		}

		DictionaryReader<long, MyFactionMember> Members
		{
			get;
		}

		DictionaryReader<long, MyFactionMember> JoinRequests
		{
			get;
		}

		bool IsFounder(long playerId);

		bool IsLeader(long playerId);

		bool IsMember(long playerId);

		bool IsNeutral(long playerId);

		bool IsEnemy(long playerId);

		bool IsFriendly(long playerId);

		bool IsEveryoneNpc();

		/// <summary>
		/// Gets balance of an account associated with faction.
		/// </summary>
		/// <param name="balance">Returns current balance of the account. (If called on client, can return delayed value, as changes to balace have to be synchronized first)</param>
		/// <returns>True if account was found. Otherwise false.</returns>
		bool TryGetBalanceInfo(out long balance);

		/// <summary>
		/// Gets balance of an account associated with faction. Format is 'BALANCE CURRENCYSHORTNAME'.
		/// </summary>
		/// <returns>Current balance of the account in form of formatted string. If Banking System does not exist method returns null.</returns>
		string GetBalanceShortString();

		/// <summary>
		/// Changes the balance of the account of this faction by given amount. Sends a message to server with the request.
		/// </summary>
		/// <param name="amount">Amount by which to change te balance.</param>
		void RequestChangeBalance(long amount);
	}
}
