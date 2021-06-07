using System;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyIdentity
	{
		[Obsolete("Use IdentityId instead.")]
		long PlayerId
		{
			get;
		}

		/// <summary>
		/// Player's unique identity id.
		/// </summary>
		/// <remarks>This will change when the player dies with permadeath enabled.</remarks>
		long IdentityId
		{
			get;
		}

		/// <summary>
		/// Name of player.
		/// </summary>
		string DisplayName
		{
			get;
		}

		/// <summary>
		/// Gets the model the player is using.
		/// </summary>
		string Model
		{
			get;
		}

		/// <summary>
		/// The player's model color mask
		/// </summary>
		Vector3? ColorMask
		{
			get;
		}

		/// <summary>
		/// Gets if the player is dead
		/// </summary>
		bool IsDead
		{
			get;
		}

		/// <summary>
		/// Triggered when the player's character changes.
		/// </summary>
		/// <remarks>First Action argument is old character; second is new character.</remarks>
		event Action<IMyCharacter, IMyCharacter> CharacterChanged;
	}
}
