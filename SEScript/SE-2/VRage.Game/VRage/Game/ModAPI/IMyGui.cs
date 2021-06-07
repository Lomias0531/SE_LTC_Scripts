using System;
using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
	public interface IMyGui
	{
		/// <summary>
		/// Gets the name of the currently open GUI screen.
		/// </summary>
		string ActiveGamePlayScreen
		{
			get;
		}

		/// <summary>
		/// Gets the entity the player is currently interacting with.
		/// </summary>
		IMyEntity InteractedEntity
		{
			get;
		}

		/// <summary>
		/// Gets an enum describing the currently open GUI screen.
		/// </summary>
		MyTerminalPageEnum GetCurrentScreen
		{
			get;
		}

		/// <summary>
		/// Checks if the chat entry box is visible.
		/// </summary>
		bool ChatEntryVisible
		{
			get;
		}

		/// <summary>
		/// Checks if the cursor is visible.
		/// </summary>
		bool IsCursorVisible
		{
			get;
		}

		/// <summary>
		/// Event triggered on gui control created.
		/// </summary>
		event Action<object> GuiControlCreated;

		/// <summary>
		/// Event triggered on gui control removed.
		/// </summary>
		event Action<object> GuiControlRemoved;
	}
}
