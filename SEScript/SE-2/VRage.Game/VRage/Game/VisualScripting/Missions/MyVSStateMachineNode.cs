using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Generics;
using VRage.Generics.StateMachine;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
	public class MyVSStateMachineNode : MyStateMachineNode
	{
		private class VSNodeVariableStorage : IMyVariableStorage<bool>, IEnumerable<KeyValuePair<MyStringId, bool>>, IEnumerable
		{
			private MyStringId left;

			private MyStringId right;

			private bool m_leftValue;

			private bool m_rightvalue = true;

			public VSNodeVariableStorage()
			{
				left = MyStringId.GetOrCompute("Left");
				right = MyStringId.GetOrCompute("Right");
			}

			public void SetValue(MyStringId key, bool newValue)
			{
				if (key == left)
				{
					m_leftValue = newValue;
				}
				if (key == right)
				{
					m_rightvalue = newValue;
				}
			}

			public bool GetValue(MyStringId key, out bool value)
			{
				value = false;
				if (key == left)
				{
					value = m_leftValue;
				}
				if (key == right)
				{
					value = m_rightvalue;
				}
				return true;
			}

			public IEnumerator<KeyValuePair<MyStringId, bool>> GetEnumerator()
			{
				yield return new KeyValuePair<MyStringId, bool>(left, m_leftValue);
				yield return new KeyValuePair<MyStringId, bool>(right, m_rightvalue);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private readonly Type m_scriptType;

		private IMyStateMachineScript m_instance;

		private readonly Dictionary<MyStringId, IMyVariableStorage<bool>> m_transitionNamesToVariableStorages = new Dictionary<MyStringId, IMyVariableStorage<bool>>();

		public IMyStateMachineScript ScriptInstance => m_instance;

		public MyVSStateMachineNode(string name, Type script)
			: base(name)
		{
			m_scriptType = script;
		}

		public override void OnUpdate(MyStateMachine stateMachine)
		{
			if (m_instance == null)
			{
				foreach (IMyVariableStorage<bool> value2 in m_transitionNamesToVariableStorages.Values)
				{
					value2.SetValue(MyStringId.GetOrCompute("Left"), newValue: true);
				}
				return;
			}
			if (string.IsNullOrEmpty(m_instance.TransitionTo))
			{
				m_instance.Update();
			}
			if (string.IsNullOrEmpty(m_instance.TransitionTo))
			{
				return;
			}
			if (OutTransitions.Count == 0)
			{
				HashSet<MyStateMachineCursor>.Enumerator enumerator2 = Cursors.GetEnumerator();
				enumerator2.MoveNext();
				MyStateMachineCursor current = enumerator2.Current;
				stateMachine.DeleteCursor(current.Id);
				return;
			}
			MyStringId orCompute = MyStringId.GetOrCompute(m_instance.TransitionTo);
			using (List<MyStateMachineTransition>.Enumerator enumerator3 = OutTransitions.GetEnumerator())
			{
				while (enumerator3.MoveNext() && !(enumerator3.Current.Name == orCompute))
				{
				}
			}
			if (m_transitionNamesToVariableStorages.TryGetValue(MyStringId.GetOrCompute(m_instance.TransitionTo), out IMyVariableStorage<bool> value))
			{
				value.SetValue(MyStringId.GetOrCompute("Left"), newValue: true);
			}
		}

		protected override void TransitionAddedInternal(MyStateMachineTransition transition)
		{
			base.TransitionAddedInternal(transition);
			if (transition.TargetNode != this)
			{
				VSNodeVariableStorage vSNodeVariableStorage = new VSNodeVariableStorage();
				transition.Conditions.Add(new MyCondition<bool>((IMyVariableStorage<bool>)vSNodeVariableStorage, MyCondition<bool>.MyOperation.Equal, "Left", "Right"));
				m_transitionNamesToVariableStorages.Add(transition.Name, vSNodeVariableStorage);
			}
		}

		public void ActivateScript(bool restored = false)
		{
			if (!(m_scriptType == null) && m_instance == null)
			{
				m_instance = (Activator.CreateInstance(m_scriptType) as IMyStateMachineScript);
				if (restored)
				{
					m_instance.Deserialize();
				}
				m_instance.Init();
				foreach (IMyVariableStorage<bool> value in m_transitionNamesToVariableStorages.Values)
				{
					value.SetValue(MyStringId.GetOrCompute("Left"), newValue: false);
				}
			}
		}

		public void DisposeScript()
		{
			if (m_instance != null)
			{
				m_instance.Dispose();
				m_instance = null;
			}
		}
	}
}
