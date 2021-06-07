using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage;
using VRage.Collections;

namespace Sandbox.Game.Gui
{
	public static class MyTerminalControlFactory
	{
		internal class BlockData
		{
			public MyUniqueList<ITerminalControl> Controls = new MyUniqueList<ITerminalControl>();

			public MyUniqueList<ITerminalAction> Actions = new MyUniqueList<ITerminalAction>();
		}

		private static Dictionary<Type, BlockData> m_controls = new Dictionary<Type, BlockData>();

		private static FastResourceLock m_controlsLock = new FastResourceLock();

		public static bool AreControlsCreated<TBlock>()
		{
			if (m_controls.ContainsKey(typeof(TBlock)))
			{
				return true;
			}
			return false;
		}

		public static bool AreControlsCreated(Type blockType)
		{
			if (m_controls.ContainsKey(blockType))
			{
				return true;
			}
			return false;
		}

		public static void EnsureControlsAreCreated(Type blockType)
		{
			MethodInfo method = blockType.GetMethod("CreateTerminalControls", BindingFlags.Static | BindingFlags.NonPublic);
			if (!(method == null))
			{
				method.Invoke(null, new object[0]);
			}
		}

		public static void AddBaseClass<TBlock, TBase>() where TBlock : TBase where TBase : MyTerminalBlock
		{
			AddBaseClass(typeof(TBase), GetList<TBlock>());
		}

		public static void RemoveBaseClass<TBlock, TBase>() where TBlock : TBase where TBase : MyTerminalBlock
		{
			RemoveBaseClass(typeof(TBase), GetList<TBlock>());
		}

		public static void RemoveAllBaseClass<TBlock>() where TBlock : MyTerminalBlock
		{
			BlockData list = GetList<TBlock>();
			Type baseType = typeof(TBlock).BaseType;
			while (baseType != null)
			{
				RemoveBaseClass(baseType, list);
				baseType = baseType.BaseType;
			}
		}

		public static void AddControl<TBlock>(int index, MyTerminalControl<TBlock> control) where TBlock : MyTerminalBlock
		{
			GetList<TBlock>().Controls.Insert(index, control);
			AddActions(index, control);
		}

		public static void AddControl<TBlock>(MyTerminalControl<TBlock> control) where TBlock : MyTerminalBlock
		{
			GetList<TBlock>().Controls.Add(control);
			AddActions(control);
		}

		public static void AddControl<TBase, TBlock>(MyTerminalControl<TBase> control) where TBase : MyTerminalBlock where TBlock : TBase
		{
			GetList<TBlock>().Controls.Add(control);
			AddActions(control);
		}

		public static void AddControl(Type blockType, ITerminalControl control)
		{
			GetList(blockType).Controls.Add(control);
		}

		public static void AddAction<TBlock>(int index, MyTerminalAction<TBlock> Action) where TBlock : MyTerminalBlock
		{
			GetList<TBlock>().Actions.Insert(index, Action);
		}

		public static void AddAction<TBlock>(MyTerminalAction<TBlock> Action) where TBlock : MyTerminalBlock
		{
			GetList<TBlock>().Actions.Add(Action);
		}

		public static void AddAction<TBase, TBlock>(MyTerminalAction<TBase> Action) where TBase : MyTerminalBlock where TBlock : TBase
		{
			GetList<TBlock>().Actions.Add(Action);
		}

		public static void AddActions(Type blockType, ITerminalControl control)
		{
			if (control.Actions != null)
			{
				ITerminalAction[] actions = control.Actions;
				foreach (ITerminalAction item in actions)
				{
					GetList(blockType).Actions.Add(item);
				}
			}
		}

		private static void AddActions<TBlock>(MyTerminalControl<TBlock> block) where TBlock : MyTerminalBlock
		{
			if (block.Actions != null)
			{
				MyTerminalAction<TBlock>[] actions = block.Actions;
				for (int i = 0; i < actions.Length; i++)
				{
					AddAction(actions[i]);
				}
			}
		}

		public static void RemoveControl<TBlock>(IMyTerminalControl item)
		{
			RemoveControl(typeof(TBlock), item);
		}

		public static void RemoveControl(Type blockType, IMyTerminalControl controlItem)
		{
			MyUniqueList<ITerminalControl> controls = GetList(blockType).Controls;
			foreach (ITerminalControl item2 in controls)
			{
				if (item2 == (ITerminalControl)controlItem)
				{
					controls.Remove(item2);
					break;
				}
			}
			ITerminalControl terminalControl = (ITerminalControl)controlItem;
			if (terminalControl.Actions != null)
			{
				ITerminalAction[] actions = terminalControl.Actions;
				foreach (ITerminalAction item in actions)
				{
					GetList(blockType).Actions.Remove(item);
				}
			}
		}

		private static void AddActions<TBlock>(int index, MyTerminalControl<TBlock> block) where TBlock : MyTerminalBlock
		{
			if (block.Actions != null)
			{
				MyTerminalAction<TBlock>[] actions = block.Actions;
				foreach (MyTerminalAction<TBlock> action in actions)
				{
					AddAction(index++, action);
				}
			}
		}

		public static UniqueListReader<ITerminalControl> GetControls(Type blockType)
		{
			return GetList(blockType).Controls.Items;
		}

		public static UniqueListReader<ITerminalAction> GetActions(Type blockType)
		{
			return GetList(blockType).Actions.Items;
		}

		public static void GetControls(Type blockType, List<ITerminalControl> resultList)
		{
			foreach (ITerminalControl item in GetList(blockType).Controls.Items)
			{
				resultList.Add(item);
			}
		}

		public static void GetValueControls(Type blockType, List<ITerminalProperty> resultList)
		{
			foreach (ITerminalControl item in GetList(blockType).Controls.Items)
			{
				ITerminalProperty terminalProperty = item as ITerminalProperty;
				if (terminalProperty != null)
				{
					resultList.Add(terminalProperty);
				}
			}
		}

		public static void GetActions(Type blockType, List<ITerminalAction> resultList)
		{
			foreach (ITerminalAction item in GetList(blockType).Actions.Items)
			{
				resultList.Add(item);
			}
		}

		public static void GetControls<TBlock>(List<MyTerminalControl<TBlock>> resultList) where TBlock : MyTerminalBlock
		{
			foreach (ITerminalControl item in GetList<TBlock>().Controls.Items)
			{
				resultList.Add((MyTerminalControl<TBlock>)item);
			}
		}

		public static void GetValueControls<TBlock>(Type blockType, List<ITerminalProperty> resultList) where TBlock : MyTerminalBlock
		{
			foreach (ITerminalControl item in GetList<TBlock>().Controls.Items)
			{
				ITerminalProperty terminalProperty = item as ITerminalProperty;
				if (terminalProperty != null)
				{
					resultList.Add(terminalProperty);
				}
			}
		}

		public static void GetActions<TBlock>(List<MyTerminalAction<TBlock>> resultList) where TBlock : MyTerminalBlock
		{
			foreach (ITerminalAction item in GetList<TBlock>().Actions.Items)
			{
				resultList.Add((MyTerminalAction<TBlock>)item);
			}
		}

		public static void Unload()
		{
			m_controls.Clear();
		}

		private static void RemoveBaseClass(Type baseClass, BlockData resultList)
		{
			if (m_controls.TryGetValue(baseClass, out BlockData value))
			{
				foreach (ITerminalControl item in value.Controls.Items)
				{
					resultList.Controls.Remove(item);
				}
				foreach (ITerminalAction item2 in value.Actions.Items)
				{
					resultList.Actions.Remove(item2);
				}
			}
		}

		private static void AddBaseClass(Type baseClass, BlockData resultList)
		{
			MethodInfo method = baseClass.GetMethod("CreateTerminalControls", BindingFlags.Static | BindingFlags.NonPublic);
			if (method != null)
			{
				method.Invoke(null, new object[0]);
			}
			if (m_controls.TryGetValue(baseClass, out BlockData value))
			{
				foreach (ITerminalControl item in value.Controls.Items)
				{
					resultList.Controls.Add(item);
				}
				foreach (ITerminalAction item2 in value.Actions.Items)
				{
					resultList.Actions.Add(item2);
				}
			}
		}

		private static BlockData GetList<TBlock>()
		{
			return GetList(typeof(TBlock));
		}

		internal static BlockData GetList(Type type)
		{
			if (!m_controls.TryGetValue(type, out BlockData value))
			{
				return InitializeControls(type);
			}
			return value;
		}

		internal static BlockData InitializeControls(Type type)
		{
			BlockData blockData = new BlockData();
			using (m_controlsLock.AcquireExclusiveUsing())
			{
				m_controls[type] = blockData;
			}
			Type baseType = type.BaseType;
			while (baseType != null)
			{
				AddBaseClass(baseType, blockData);
				baseType = baseType.BaseType;
			}
			return blockData;
		}
	}
}
