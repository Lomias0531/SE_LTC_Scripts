using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.Game.Gui
{
	public class MyTerminalAction<TBlock> : ITerminalAction, Sandbox.ModAPI.Interfaces.ITerminalAction, IMyTerminalAction where TBlock : MyTerminalBlock
	{
		private readonly string m_id;

		private string m_icon;

		private StringBuilder m_name;

		private List<TerminalActionParameter> m_parameterDefinitions = new List<TerminalActionParameter>();

		private Action<TBlock> m_action;

		private Action<TBlock, ListReader<TerminalActionParameter>> m_actionWithParameters;

		public Func<TBlock, bool> Enabled = (TBlock b) => true;

		public Func<TBlock, bool> Callable = (TBlock b) => true;

		public List<MyToolbarType> InvalidToolbarTypes;

		public bool ValidForGroups = true;

		public MyTerminalControl<TBlock>.WriterDelegate Writer;

		public Action<IList<TerminalActionParameter>, Action<bool>> DoUserParameterRequest;

		public Action<TBlock> Action
		{
			get
			{
				return m_action;
			}
			set
			{
				m_action = value;
				m_actionWithParameters = delegate(TBlock block, ListReader<TerminalActionParameter> parameters)
				{
					m_action(block);
				};
			}
		}

		public Action<TBlock, ListReader<TerminalActionParameter>> ActionWithParameters
		{
			get
			{
				return m_actionWithParameters;
			}
			set
			{
				m_actionWithParameters = value;
				m_action = delegate(TBlock block)
				{
					m_actionWithParameters(block, new ListReader<TerminalActionParameter>(ParameterDefinitions));
				};
			}
		}

		public string Id => m_id;

		public string Icon => m_icon;

		public StringBuilder Name => m_name;

		string Sandbox.ModAPI.Interfaces.ITerminalAction.Id => Id;

		string Sandbox.ModAPI.Interfaces.ITerminalAction.Icon => Icon;

		StringBuilder Sandbox.ModAPI.Interfaces.ITerminalAction.Name => Name;

		public List<TerminalActionParameter> ParameterDefinitions => m_parameterDefinitions;

		Func<Sandbox.ModAPI.IMyTerminalBlock, bool> IMyTerminalAction.Enabled
		{
			get
			{
				Func<TBlock, bool> oldEnabled = Enabled;
				return (Sandbox.ModAPI.IMyTerminalBlock x) => oldEnabled((TBlock)x);
			}
			set
			{
				Enabled = value;
			}
		}

		List<MyToolbarType> IMyTerminalAction.InvalidToolbarTypes
		{
			get
			{
				return InvalidToolbarTypes;
			}
			set
			{
				InvalidToolbarTypes = value;
			}
		}

		bool IMyTerminalAction.ValidForGroups
		{
			get
			{
				return ValidForGroups;
			}
			set
			{
				ValidForGroups = value;
			}
		}

		StringBuilder IMyTerminalAction.Name
		{
			get
			{
				return Name;
			}
			set
			{
				m_name = value;
			}
		}

		string IMyTerminalAction.Icon
		{
			get
			{
				return Icon;
			}
			set
			{
				m_icon = value;
			}
		}

		Action<Sandbox.ModAPI.IMyTerminalBlock> IMyTerminalAction.Action
		{
			get
			{
				Action<TBlock> oldAction = Action;
				return delegate(Sandbox.ModAPI.IMyTerminalBlock x)
				{
					oldAction((TBlock)x);
				};
			}
			set
			{
				Action = value;
			}
		}

		Action<Sandbox.ModAPI.IMyTerminalBlock, StringBuilder> IMyTerminalAction.Writer
		{
			get
			{
				MyTerminalControl<TBlock>.WriterDelegate oldWriter = Writer;
				return delegate(Sandbox.ModAPI.IMyTerminalBlock x, StringBuilder y)
				{
					oldWriter((TBlock)x, y);
				};
			}
			set
			{
				Writer = value.Invoke;
			}
		}

		public MyTerminalAction(string id, StringBuilder name, string icon)
		{
			m_id = id;
			m_name = name;
			m_icon = icon;
		}

		public MyTerminalAction(string id, StringBuilder name, Action<TBlock> action, string icon)
		{
			m_id = id;
			m_name = name;
			Action = action;
			m_icon = icon;
		}

		public MyTerminalAction(string id, StringBuilder name, Action<TBlock, ListReader<TerminalActionParameter>> action, string icon)
		{
			m_id = id;
			m_name = name;
			ActionWithParameters = action;
			m_icon = icon;
		}

		public MyTerminalAction(string id, StringBuilder name, Action<TBlock> action, MyTerminalControl<TBlock>.WriterDelegate valueWriter, string icon)
		{
			m_id = id;
			m_name = name;
			Action = action;
			m_icon = icon;
			Writer = valueWriter;
		}

		public MyTerminalAction(string id, StringBuilder name, Action<TBlock, ListReader<TerminalActionParameter>> action, MyTerminalControl<TBlock>.WriterDelegate valueWriter, string icon)
		{
			m_id = id;
			m_name = name;
			ActionWithParameters = action;
			m_icon = icon;
			Writer = valueWriter;
		}

		public MyTerminalAction(string id, StringBuilder name, Action<TBlock> action, MyTerminalControl<TBlock>.WriterDelegate valueWriter, string icon, Func<TBlock, bool> enabled = null, Func<TBlock, bool> callable = null)
			: this(id, name, action, valueWriter, icon)
		{
			if (enabled != null)
			{
				Enabled = enabled;
			}
			if (callable != null)
			{
				Callable = callable;
			}
		}

		public void Apply(MyTerminalBlock block, ListReader<TerminalActionParameter> parameters)
		{
			TBlock val = (TBlock)block;
			if (Enabled(val) && IsCallable(val))
			{
				m_actionWithParameters(val, parameters);
			}
		}

		public void Apply(MyTerminalBlock block)
		{
			TBlock val = (TBlock)block;
			if (Enabled(val) && IsCallable(val))
			{
				m_action(val);
			}
		}

		public bool IsEnabled(MyTerminalBlock block)
		{
			if (!string.IsNullOrEmpty(Id) && (Id.Equals("IncreaseWeld speed") || Id.Equals("DecreaseWeld speed") || Id.Equals("Force weld")))
			{
				return false;
			}
			if (Enabled((TBlock)block))
			{
				return IsCallable((TBlock)block);
			}
			return false;
		}

		public bool IsCallable(MyTerminalBlock block)
		{
			if (Callable != null)
			{
				return Callable((TBlock)block);
			}
			return true;
		}

		public bool IsValidForToolbarType(MyToolbarType type)
		{
			if (InvalidToolbarTypes == null)
			{
				return true;
			}
			return !InvalidToolbarTypes.Contains(type);
		}

		public bool IsValidForGroups()
		{
			return ValidForGroups;
		}

		ListReader<TerminalActionParameter> ITerminalAction.GetParameterDefinitions()
		{
			return m_parameterDefinitions;
		}

		public void WriteValue(MyTerminalBlock block, StringBuilder appendTo)
		{
			if (Writer != null && IsCallable((TBlock)block))
			{
				Writer((TBlock)block, appendTo);
			}
		}

		public void RequestParameterCollection(IList<TerminalActionParameter> parameters, Action<bool> callback)
		{
			if (parameters == null)
			{
				throw new ArgumentException("parameters");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			Action<IList<TerminalActionParameter>, Action<bool>> doUserParameterRequest = DoUserParameterRequest;
			List<TerminalActionParameter> parameterDefinitions = ParameterDefinitions;
			parameters.Clear();
			foreach (TerminalActionParameter item in parameterDefinitions)
			{
				parameters.Add(item);
			}
			if (doUserParameterRequest == null)
			{
				callback(obj: true);
			}
			else
			{
				doUserParameterRequest(parameters, callback);
			}
		}

		void Sandbox.ModAPI.Interfaces.ITerminalAction.Apply(IMyCubeBlock block)
		{
			if (block is TBlock)
			{
				Apply(block as MyTerminalBlock);
			}
		}

		void Sandbox.ModAPI.Interfaces.ITerminalAction.Apply(IMyCubeBlock block, ListReader<TerminalActionParameter> parameters)
		{
			if (block is TBlock)
			{
				Apply(block as MyTerminalBlock, parameters);
			}
		}

		void Sandbox.ModAPI.Interfaces.ITerminalAction.WriteValue(IMyCubeBlock block, StringBuilder appendTo)
		{
			if (block is TBlock)
			{
				WriteValue(block as MyTerminalBlock, appendTo);
			}
		}

		bool Sandbox.ModAPI.Interfaces.ITerminalAction.IsEnabled(IMyCubeBlock block)
		{
			if (block is TBlock)
			{
				return IsEnabled(block as MyTerminalBlock);
			}
			return false;
		}
	}
}
