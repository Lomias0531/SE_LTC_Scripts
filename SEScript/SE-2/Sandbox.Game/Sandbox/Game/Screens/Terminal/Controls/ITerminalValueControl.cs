using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Interfaces;
using System;
using VRage.Library.Collections;

namespace Sandbox.Game.Screens.Terminal.Controls
{
	internal interface ITerminalValueControl<TBlock, TValue> : ITerminalProperty<TValue>, ITerminalProperty, ITerminalControl, ITerminalControlSync where TBlock : MyTerminalBlock
	{
		TValue GetValue(TBlock block);

		void SetValue(TBlock block, TValue value);

		TValue GetDefaultValue(TBlock block);

		[Obsolete("Use GetMinimum instead")]
		TValue GetMininum(TBlock block);

		TValue GetMinimum(TBlock block);

		TValue GetMaximum(TBlock block);

		void Serialize(BitStream stream, TBlock block);
	}
}
