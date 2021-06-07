using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;

namespace Sandbox.Game.Gui
{
	public interface ITerminalAction : Sandbox.ModAPI.Interfaces.ITerminalAction
	{
		new string Id
		{
			get;
		}

		new string Icon
		{
			get;
		}

		new StringBuilder Name
		{
			get;
		}

		void Apply(MyTerminalBlock block);

		void WriteValue(MyTerminalBlock block, StringBuilder appendTo);

		bool IsEnabled(MyTerminalBlock block);

		bool IsValidForToolbarType(MyToolbarType toolbarType);

		bool IsValidForGroups();

		ListReader<TerminalActionParameter> GetParameterDefinitions();

		void RequestParameterCollection(IList<TerminalActionParameter> parameters, Action<bool> callback);

		void Apply(MyTerminalBlock block, ListReader<TerminalActionParameter> parameters);
	}
}
