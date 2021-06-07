using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Collections;

namespace Sandbox.Game.Screens.Terminal.Controls
{
	public abstract class MyTerminalValueControl<TBlock, TValue> : MyTerminalControl<TBlock>, ITerminalValueControl<TBlock, TValue>, ITerminalProperty<TValue>, ITerminalProperty, ITerminalControl, ITerminalControlSync, IMyTerminalValueControl<TValue> where TBlock : MyTerminalBlock
	{
		public delegate TValue GetterDelegate(TBlock block);

		public delegate void SetterDelegate(TBlock block, TValue value);

		public delegate void SerializerDelegate(BitStream stream, ref TValue value);

		public delegate void ExternalSetterDelegate(IMyTerminalBlock block, TValue value);

		public SerializerDelegate Serializer;

		public GetterDelegate Getter
		{
			get;
			set;
		}

		public SetterDelegate Setter
		{
			get;
			set;
		}

		string ITerminalProperty.Id => Id;

		string ITerminalProperty.TypeName => typeof(TValue).Name;

		Func<IMyTerminalBlock, TValue> IMyTerminalValueControl<TValue>.Getter
		{
			get
			{
				GetterDelegate oldGetter = Getter;
				return (IMyTerminalBlock x) => oldGetter((TBlock)x);
			}
			set
			{
				Getter = value.Invoke;
			}
		}

		Action<IMyTerminalBlock, TValue> IMyTerminalValueControl<TValue>.Setter
		{
			get
			{
				SetterDelegate oldSetter = Setter;
				return delegate(IMyTerminalBlock x, TValue y)
				{
					oldSetter((TBlock)x, y);
				};
			}
			set
			{
				Setter = value.Invoke;
			}
		}

		public MyTerminalValueControl(string id)
			: base(id)
		{
		}

		public virtual TValue GetValue(TBlock block)
		{
			return Getter(block);
		}

		public virtual void SetValue(TBlock block, TValue value)
		{
			Setter(block, value);
			block.NotifyTerminalValueChanged(this);
		}

		public virtual void Serialize(BitStream stream, TBlock block)
		{
			if (stream.Reading)
			{
				TValue value = default(TValue);
				Serializer(stream, ref value);
				SetValue(block, value);
			}
			else
			{
				TValue value2 = GetValue(block);
				Serializer(stream, ref value2);
			}
		}

		public abstract TValue GetDefaultValue(TBlock block);

		[Obsolete("Use GetMinimum instead")]
		public TValue GetMininum(TBlock block)
		{
			return GetMinimum(block);
		}

		public abstract TValue GetMinimum(TBlock block);

		public abstract TValue GetMaximum(TBlock block);

		public TValue GetValue(IMyCubeBlock block)
		{
			return GetValue((TBlock)block);
		}

		public void SetValue(IMyCubeBlock block, TValue value)
		{
			SetValue((TBlock)block, value);
		}

		public TValue GetDefaultValue(IMyCubeBlock block)
		{
			return GetDefaultValue((TBlock)block);
		}

		[Obsolete("Use GetMinimum instead")]
		public TValue GetMininum(IMyCubeBlock block)
		{
			return GetMinimum((TBlock)block);
		}

		public TValue GetMinimum(IMyCubeBlock block)
		{
			return GetMinimum((TBlock)block);
		}

		public TValue GetMaximum(IMyCubeBlock block)
		{
			return GetMaximum((TBlock)block);
		}

		public void Serialize(BitStream stream, MyTerminalBlock block)
		{
			Serialize(stream, (TBlock)block);
		}
	}
}
