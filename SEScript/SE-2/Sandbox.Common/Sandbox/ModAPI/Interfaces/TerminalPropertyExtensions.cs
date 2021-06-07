using Sandbox.ModAPI.Ingame;
using System;
using VRageMath;

namespace Sandbox.ModAPI.Interfaces
{
	/// <summary>
	/// Terminal block extension for property access
	/// </summary>
	public static class TerminalPropertyExtensions
	{
		/// <summary>
		/// Property type cast
		/// </summary>
		/// <typeparam name="TValue">value of type <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="property"><see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> reference</param>
		/// <returns>reference to <see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> value of specified type</returns>
		public static ITerminalProperty<TValue> As<TValue>(this ITerminalProperty property)
		{
			return property as ITerminalProperty<TValue>;
		}

		/// <summary>
		/// Property type cast
		/// </summary>
		/// <typeparam name="TValue">value of type <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="property"><see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> reference</param>
		/// <returns>reference to <see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> value of specified type</returns>
		public static ITerminalProperty<TValue> Cast<TValue>(this ITerminalProperty property)
		{
			if (property == null)
			{
				throw new InvalidOperationException("Invalid property");
			}
			ITerminalProperty<TValue> terminalProperty = property.As<TValue>();
			if (terminalProperty == null)
			{
				throw new InvalidOperationException($"Property {property.Id} is not of type {typeof(TValue).Name}, correct type is {property.TypeName}");
			}
			return terminalProperty;
		}

		/// <summary>
		/// Check property type
		/// </summary>
		/// <typeparam name="TValue">value of type <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="property"><see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> reference</param>
		/// <returns>true if type matches</returns>
		public static bool Is<TValue>(this ITerminalProperty property)
		{
			return property is ITerminalProperty<TValue>;
		}

		/// <summary>
		/// Property type cast
		/// </summary>
		/// <param name="property"><see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> reference</param>
		/// <returns>reference to <see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> value of specified type (float)</returns>
		public static ITerminalProperty<float> AsFloat(this ITerminalProperty property)
		{
			return property.As<float>();
		}

		/// <summary>
		/// Property type cast
		/// </summary>
		/// <param name="property"><see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> reference</param>
		/// <returns>reference to <see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> value of specified type (Color)</returns>
		public static ITerminalProperty<Color> AsColor(this ITerminalProperty property)
		{
			return property.As<Color>();
		}

		/// <summary>
		/// Property type cast
		/// </summary>
		/// <param name="property"><see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> reference</param>
		/// <returns>reference to <see cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty`1" /> value of specified type (bool)</returns>
		public static ITerminalProperty<bool> AsBool(this ITerminalProperty property)
		{
			return property.As<bool>();
		}

		/// <summary>
		/// Returns value of specified property
		/// </summary>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <returns>property value as float</returns>
		public static float GetValueFloat(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId)
		{
			return block.GetValue<float>(propertyId);
		}

		/// <summary>
		/// Set float value of property
		/// </summary>
		/// <typeparam name="T"><see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <param name="value">value to set</param>
		public static void SetValueFloat(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId, float value)
		{
			block.SetValue(propertyId, value);
		}

		/// <summary>
		/// Returns value of specified property
		/// </summary>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <returns>property value as bool</returns>
		public static bool GetValueBool(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId)
		{
			return block.GetValue<bool>(propertyId);
		}

		/// <summary>
		/// Set bool value of property
		/// </summary>
		/// <typeparam name="T"><see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <param name="value">value to set</param>
		public static void SetValueBool(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId, bool value)
		{
			block.SetValue(propertyId, value);
		}

		/// <summary>
		/// Returns value of specified property
		/// </summary>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <returns>property value as Color</returns>
		public static Color GetValueColor(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId)
		{
			return block.GetValue<Color>(propertyId);
		}

		/// <summary>
		/// Set bool value of property
		/// </summary>
		/// <typeparam name="T"><see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <param name="value">value to set</param>
		public static void SetValueColor(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId, Color value)
		{
			block.SetValue(propertyId, value);
		}

		/// <summary>
		/// Returns value of specified property as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" />
		/// </summary>
		/// <typeparam name="T">required value type of <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <returns>property value as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		public static T GetValue<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId)
		{
			return block.GetProperty(propertyId).Cast<T>().GetValue(block);
		}

		/// <summary>
		/// Returns default value of specified property as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" />
		/// </summary>
		/// <typeparam name="T">required value type of <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <returns>property value as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		public static T GetDefaultValue<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId)
		{
			return block.GetProperty(propertyId).Cast<T>().GetDefaultValue(block);
		}

		/// <summary>
		/// Returns minimum value of specified property as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /> - this call is obsolete due typo in name, use <see cref="M:Sandbox.ModAPI.Interfaces.TerminalPropertyExtensions.GetMinimum``1(Sandbox.ModAPI.Ingame.IMyTerminalBlock,System.String)" />
		/// </summary>
		/// <typeparam name="T">required value type of <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <returns>property value as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		[Obsolete("Use GetMinimum instead")]
		public static T GetMininum<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId)
		{
			return block.GetProperty(propertyId).Cast<T>().GetMinimum(block);
		}

		/// <summary>
		/// Returns minimum value of specified property as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" />
		/// </summary>
		/// <typeparam name="T">required value type of <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <returns>property value as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		public static T GetMinimum<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId)
		{
			return block.GetProperty(propertyId).Cast<T>().GetMinimum(block);
		}

		/// <summary>
		/// Returns maximum value of specified property as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" />
		/// </summary>
		/// <typeparam name="T">required value type of <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <returns>property value as <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></returns>
		public static T GetMaximum<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId)
		{
			return block.GetProperty(propertyId).Cast<T>().GetMaximum(block);
		}

		/// <summary>
		/// Set value of property with type of <see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" />
		/// </summary>
		/// <typeparam name="T"><see cref="P:Sandbox.ModAPI.Interfaces.ITerminalProperty.TypeName" /></typeparam>
		/// <param name="block">block reference</param>
		/// <param name="propertyId">property id (name)</param>
		/// <param name="value">value to set</param>
		public static void SetValue<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId, T value)
		{
			block.GetProperty(propertyId).Cast<T>().SetValue(block, value);
		}
	}
}
