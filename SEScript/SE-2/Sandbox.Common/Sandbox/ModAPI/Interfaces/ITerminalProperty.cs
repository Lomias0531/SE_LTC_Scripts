using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Interfaces
{
	/// <summary>
	/// Terminal block property definition
	/// </summary>
	public interface ITerminalProperty
	{
		/// <summary>
		/// Property Id (value name)
		/// </summary>
		string Id
		{
			get;
		}

		/// <summary>
		/// Property type (bool - <see cref="T:System.Boolean" />, float - <see cref="T:System.Single" />, color - <see cref="T:VRageMath.Color" />)
		/// </summary>
		string TypeName
		{
			get;
		}
	}
	/// <summary>
	/// Terminal block property access
	/// </summary>
	/// <typeparam name="TValue">Property type (<see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" />)</typeparam>
	public interface ITerminalProperty<TValue> : ITerminalProperty
	{
		/// <summary>
		/// Retrieve property value
		/// </summary>
		/// <param name="block">block reference</param>
		/// <returns>value of type <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		TValue GetValue(IMyCubeBlock block);

		/// <summary>
		/// Set property value
		/// </summary>
		/// <param name="block">block reference</param>
		/// <param name="value">value of type <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></param>
		void SetValue(IMyCubeBlock block, TValue value);

		/// <summary>
		/// Default value of property (if value is not set, or value from block definition)
		/// </summary>
		/// <param name="block">block reference</param>
		/// <returns>value of type <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		TValue GetDefaultValue(IMyCubeBlock block);

		/// <summary>
		/// Minimum value of property (value from block definition) - this function is obsolete, because it contains typo in name, use <see cref="!:GetMinimum(Sandbox.ModAPI.Ingame.IMyCubeBlock)" />
		/// </summary>
		/// <param name="block">block reference</param>
		/// <returns>value of type <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		[Obsolete("Use GetMinimum instead")]
		TValue GetMininum(IMyCubeBlock block);

		/// <summary>
		/// Minimum value of property (value from block definition)
		/// </summary>
		/// <param name="block">block reference</param>
		/// <returns>value of type <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		TValue GetMinimum(IMyCubeBlock block);

		/// <summary>
		/// Maximum value of property (value from block definition)
		/// </summary>
		/// <param name="block">block reference</param>
		/// <returns>value of type <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		TValue GetMaximum(IMyCubeBlock block);
	}
}
