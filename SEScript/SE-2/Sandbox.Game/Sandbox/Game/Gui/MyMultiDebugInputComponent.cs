using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Input;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public abstract class MyMultiDebugInputComponent : MyDebugComponent
	{
		[Serializable]
		public struct MultidebugData
		{
			protected class Sandbox_Game_Gui_MyMultiDebugInputComponent_003C_003EMultidebugData_003C_003EActiveDebug_003C_003EAccessor : IMemberAccessor<MultidebugData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MultidebugData owner, in int value)
				{
					owner.ActiveDebug = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MultidebugData owner, out int value)
				{
					value = owner.ActiveDebug;
				}
			}

			protected class Sandbox_Game_Gui_MyMultiDebugInputComponent_003C_003EMultidebugData_003C_003EChildDatas_003C_003EAccessor : IMemberAccessor<MultidebugData, object[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MultidebugData owner, in object[] value)
				{
					owner.ChildDatas = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MultidebugData owner, out object[] value)
				{
					value = owner.ChildDatas;
				}
			}

			public int ActiveDebug;

			public object[] ChildDatas;
		}

		private int m_activeMode;

		private List<MyKeys> m_keys = new List<MyKeys>();

		public abstract MyDebugComponent[] Components
		{
			get;
		}

		public MyDebugComponent ActiveComponent
		{
			get
			{
				if (Components == null || Components.Length == 0)
				{
					return null;
				}
				return Components[m_activeMode];
			}
		}

		public override object InputData
		{
			get
			{
				MyDebugComponent[] components = Components;
				object[] array = new object[components.Length];
				for (int i = 0; i < components.Length; i++)
				{
					array[i] = components[i].InputData;
				}
				MultidebugData value = default(MultidebugData);
				value.ActiveDebug = m_activeMode;
				value.ChildDatas = array;
				return new MultidebugData?(value);
			}
			set
			{
				MultidebugData? multidebugData = value as MultidebugData?;
				if (multidebugData.HasValue)
				{
					m_activeMode = multidebugData.Value.ActiveDebug;
					MyDebugComponent[] components = Components;
					if (components.Length == multidebugData.Value.ChildDatas.Length)
					{
						for (int i = 0; i < components.Length; i++)
						{
							components[i].InputData = multidebugData.Value.ChildDatas[i];
						}
					}
				}
				else
				{
					m_activeMode = 0;
				}
			}
		}

		public override void Draw()
		{
			MyDebugComponent[] components = Components;
			if (components == null || components.Length == 0)
			{
				Text(Color.Red, 1.5f, "{0} Debug Input - NO COMPONENTS", GetName());
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (components.Length != 0)
			{
				stringBuilder.Append(FormatComponentName(0));
			}
			for (int i = 1; i < components.Length; i++)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(FormatComponentName(i));
			}
			Text(Color.Yellow, 1.5f, "{0} Debug Input: {1}", GetName(), stringBuilder.ToString());
			if (MySandboxGame.Config.DebugComponentsInfo == MyDebugComponentInfoState.FullInfo)
			{
				Text(Color.White, 1.2f, "Select Tab: Left WinKey + Tab Number");
			}
			VSpace(5f);
			DrawInternal();
			components[m_activeMode].Draw();
		}

		public virtual void DrawInternal()
		{
		}

		public override void Update10()
		{
			base.Update10();
			if (ActiveComponent != null)
			{
				ActiveComponent.Update10();
			}
		}

		public override void Update100()
		{
			base.Update100();
			if (ActiveComponent != null)
			{
				ActiveComponent.Update100();
			}
		}

		private string FormatComponentName(int index)
		{
			string name = Components[index].GetName();
			if (index != m_activeMode)
			{
				return $"{name}({index})";
			}
			return $"{name.ToUpper()}({index})";
		}

		public override bool HandleInput()
		{
			if (Components == null || Components.Length == 0)
			{
				return false;
			}
			if (MyInput.Static.IsKeyPress(MyKeys.LeftWindows) || MyInput.Static.IsKeyPress(MyKeys.RightWindows))
			{
				MyInput.Static.GetPressedKeys(m_keys);
				int num = m_activeMode;
				foreach (MyKeys key in m_keys)
				{
					if ((int)key >= 96 && (int)key <= 105)
					{
						int num2 = (int)(key - 96);
						if (num2 < Components.Length)
						{
							num = num2;
						}
					}
				}
				if (m_activeMode != num)
				{
					m_activeMode = num;
					Save();
					return true;
				}
			}
			return Components[m_activeMode].HandleInput();
		}
	}
}
