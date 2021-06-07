using Sandbox.Definitions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.World
{
	[MyEventType(typeof(MyObjectBuilder_GlobalEventBase), true)]
	[MyEventType(typeof(MyObjectBuilder_GlobalEventDefinition), false)]
	public class MyGlobalEventBase : IComparable
	{
		public bool IsOneTime => !Definition.MinActivationTime.HasValue;

		public bool IsPeriodic => !IsOneTime;

		public bool IsInPast => ActivationTime.Ticks <= 0;

		public bool IsInFuture => ActivationTime.Ticks > 0;

		public bool IsHandlerValid => Action != null;

		public MyGlobalEventDefinition Definition
		{
			get;
			private set;
		}

		public MethodInfo Action
		{
			get;
			private set;
		}

		public TimeSpan ActivationTime
		{
			get;
			private set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public bool RemoveAfterHandlerExit
		{
			get;
			set;
		}

		public virtual void InitFromDefinition(MyGlobalEventDefinition definition)
		{
			Definition = definition;
			Action = MyGlobalEventFactory.GetEventHandler(Definition.Id);
			if (Definition.FirstActivationTime.HasValue)
			{
				ActivationTime = Definition.FirstActivationTime.Value;
			}
			else
			{
				RecalculateActivationTime();
			}
			Enabled = true;
			RemoveAfterHandlerExit = false;
		}

		public virtual void Init(MyObjectBuilder_GlobalEventBase ob)
		{
			Definition = MyDefinitionManager.Static.GetEventDefinition(ob.GetId());
			Action = MyGlobalEventFactory.GetEventHandler(ob.GetId());
			ActivationTime = TimeSpan.FromMilliseconds(ob.ActivationTimeMs);
			Enabled = ob.Enabled;
			RemoveAfterHandlerExit = false;
		}

		public virtual MyObjectBuilder_GlobalEventBase GetObjectBuilder()
		{
			MyObjectBuilder_GlobalEventBase obj = MyObjectBuilderSerializer.CreateNewObject(Definition.Id.TypeId, Definition.Id.SubtypeName) as MyObjectBuilder_GlobalEventBase;
			obj.ActivationTimeMs = ActivationTime.Ticks / 10000;
			obj.Enabled = Enabled;
			return obj;
		}

		public void RecalculateActivationTime()
		{
			if (Definition.MinActivationTime == Definition.MaxActivationTime)
			{
				ActivationTime = Definition.MinActivationTime.Value;
			}
			else
			{
				ActivationTime = MyUtils.GetRandomTimeSpan(Definition.MinActivationTime.Value, Definition.MaxActivationTime.Value);
			}
			MySandboxGame.Log.WriteLine("MyGlobalEvent.RecalculateActivationTime:");
			MySandboxGame.Log.WriteLine("Next activation in " + ActivationTime.ToString());
		}

		public void SetActivationTime(TimeSpan time)
		{
			ActivationTime = time;
		}

		public int CompareTo(object obj)
		{
			if (!(obj is MyGlobalEventBase))
			{
				return 0;
			}
			TimeSpan timeSpan = ActivationTime - (obj as MyGlobalEventBase).ActivationTime;
			if (timeSpan.Ticks == 0L)
			{
				return RuntimeHelpers.GetHashCode(this) - RuntimeHelpers.GetHashCode(obj);
			}
			if (timeSpan.Ticks >= 0)
			{
				return 1;
			}
			return -1;
		}
	}
}
