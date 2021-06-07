using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyTerminalBlock : IMyCubeBlock, IMyEntity
	{
		string CustomName
		{
			get;
			set;
		}

		string CustomNameWithFaction
		{
			get;
		}

		string DetailedInfo
		{
			get;
		}

		string CustomInfo
		{
			get;
		}

		/// <summary>
		/// Gets or sets the Custom Data string.
		/// NOTE: Only use this for user input. For storing large mod configs, create your own MyModStorageComponent
		/// </summary>
		string CustomData
		{
			get;
			set;
		}

		bool ShowOnHUD
		{
			get;
			set;
		}

		bool ShowInTerminal
		{
			get;
			set;
		}

		bool ShowInToolbarConfig
		{
			get;
			set;
		}

		bool ShowInInventory
		{
			get;
			set;
		}

		bool HasLocalPlayerAccess();

		bool HasPlayerAccess(long playerId);

		[Obsolete("Use the setter of Customname")]
		void SetCustomName(string text);

		[Obsolete("Use the setter of Customname")]
		void SetCustomName(StringBuilder text);

		void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null);

		void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null);

		ITerminalAction GetActionWithName(string name);

		ITerminalProperty GetProperty(string id);

		void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null);

		/// <summary>
		/// <para>
		/// Determines whether this block is mechanically connected to the other. This is any block connected
		/// with rotors or pistons or other mechanical devices, but not things like connectors. This will in most
		/// cases constitute your complete construct.
		/// </para>
		/// <para>
		/// Be aware that using merge blocks combines grids into one, so this function will not filter out grids
		/// connected that way. Also be aware that detaching the heads of pistons and rotors will cause this
		/// connection to change.
		/// </para>
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		bool IsSameConstructAs(IMyTerminalBlock other);
	}
}
