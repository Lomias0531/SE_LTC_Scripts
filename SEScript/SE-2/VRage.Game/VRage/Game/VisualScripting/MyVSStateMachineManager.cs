using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Missions;
using VRage.Generics;
using VRage.ObjectBuilders;

namespace VRage.Game.VisualScripting
{
	public class MyVSStateMachineManager
	{
		private readonly CachingList<MyVSStateMachine> m_runningMachines = new CachingList<MyVSStateMachine>();

		private readonly Dictionary<string, MyObjectBuilder_ScriptSM> m_machineDefinitions = new Dictionary<string, MyObjectBuilder_ScriptSM>();

		public IEnumerable<MyVSStateMachine> RunningMachines => m_runningMachines;

		public Dictionary<string, MyObjectBuilder_ScriptSM> MachineDefinitions => m_machineDefinitions;

		public event Action<MyVSStateMachine> StateMachineStarted;

		public void Update()
		{
			m_runningMachines.ApplyChanges();
			foreach (MyVSStateMachine runningMachine in m_runningMachines)
			{
				runningMachine.Update();
				if (runningMachine.ActiveCursorCount == 0)
				{
					m_runningMachines.Remove(runningMachine);
					if (MyVisualScriptLogicProvider.MissionFinished != null)
					{
						MyVisualScriptLogicProvider.MissionFinished(runningMachine.Name);
					}
				}
			}
		}

		public string AddMachine(string filePath)
		{
			if (!MyObjectBuilderSerializer.DeserializeXML(filePath, out MyObjectBuilder_VSFiles objectBuilder) || objectBuilder.StateMachine == null)
			{
				return null;
			}
			if (m_machineDefinitions.ContainsKey(objectBuilder.StateMachine.Name))
			{
				return null;
			}
			m_machineDefinitions.Add(objectBuilder.StateMachine.Name, objectBuilder.StateMachine);
			return objectBuilder.StateMachine.Name;
		}

		public bool Run(string machineName, long ownerId = 0L)
		{
			if (m_machineDefinitions.TryGetValue(machineName, out MyObjectBuilder_ScriptSM value))
			{
				MyVSStateMachine myVSStateMachine = new MyVSStateMachine();
				myVSStateMachine.Init(value, ownerId);
				m_runningMachines.Add(myVSStateMachine);
				if (MyVisualScriptLogicProvider.MissionStarted != null)
				{
					MyVisualScriptLogicProvider.MissionStarted(myVSStateMachine.Name);
				}
				if (this.StateMachineStarted != null)
				{
					this.StateMachineStarted(myVSStateMachine);
				}
				return true;
			}
			return false;
		}

		public bool Restore(string machineName, IEnumerable<MyObjectBuilder_ScriptSMCursor> cursors)
		{
			if (!m_machineDefinitions.TryGetValue(machineName, out MyObjectBuilder_ScriptSM value))
			{
				return false;
			}
			MyObjectBuilder_ScriptSM ob = new MyObjectBuilder_ScriptSM
			{
				Name = value.Name,
				Nodes = value.Nodes,
				Transitions = value.Transitions
			};
			MyVSStateMachine myVSStateMachine = new MyVSStateMachine();
			myVSStateMachine.Init(ob);
			foreach (MyObjectBuilder_ScriptSMCursor cursor in cursors)
			{
				if (myVSStateMachine.RestoreCursor(cursor.NodeName) == null)
				{
					return false;
				}
			}
			m_runningMachines.Add(myVSStateMachine);
			if (this.StateMachineStarted != null)
			{
				this.StateMachineStarted(myVSStateMachine);
			}
			return true;
		}

		public void Dispose()
		{
			foreach (MyVSStateMachine runningMachine in m_runningMachines)
			{
				runningMachine.Dispose();
			}
			m_runningMachines.Clear();
		}

		public MyObjectBuilder_ScriptStateMachineManager GetObjectBuilder()
		{
			IReadOnlyList<MyVSStateMachine> readOnlyList2;
			if (m_runningMachines.HasChanges)
			{
				IReadOnlyList<MyVSStateMachine> readOnlyList = m_runningMachines.CopyWithChanges();
				readOnlyList2 = readOnlyList;
			}
			else
			{
				IReadOnlyList<MyVSStateMachine> readOnlyList = m_runningMachines;
				readOnlyList2 = readOnlyList;
			}
			MyObjectBuilder_ScriptStateMachineManager myObjectBuilder_ScriptStateMachineManager = new MyObjectBuilder_ScriptStateMachineManager
			{
				ActiveStateMachines = new List<MyObjectBuilder_ScriptStateMachineManager.CursorStruct>()
			};
			foreach (MyVSStateMachine item in readOnlyList2)
			{
				List<MyStateMachineCursor> activeCursors = item.ActiveCursors;
				MyObjectBuilder_ScriptSMCursor[] array = new MyObjectBuilder_ScriptSMCursor[activeCursors.Count];
				for (int i = 0; i < activeCursors.Count; i++)
				{
					array[i] = new MyObjectBuilder_ScriptSMCursor
					{
						NodeName = activeCursors[i].Node.Name
					};
				}
				myObjectBuilder_ScriptStateMachineManager.ActiveStateMachines.Add(new MyObjectBuilder_ScriptStateMachineManager.CursorStruct
				{
					Cursors = array,
					StateMachineName = item.Name
				});
			}
			return myObjectBuilder_ScriptStateMachineManager;
		}
	}
}
