using System;
using VRage.Game.Components.Interfaces;
using VRage.Game.Components.Session;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Components
{
	public abstract class MySessionComponentBase : IMyUserInputComponent
	{
		public readonly string DebugName;

		public readonly int Priority;

		public readonly Type ComponentType;

		public IMySession Session;

		private bool m_initialized;

		public MyUpdateOrder UpdateOrder
		{
			get;
			private set;
		}

		public MyObjectBuilderType ObjectBuilderType
		{
			get;
			private set;
		}

		public IMyModContext ModContext
		{
			get;
			set;
		}

		public bool Loaded
		{
			get;
			private set;
		}

		public bool Initialized => m_initialized;

		public bool UpdateOnPause
		{
			get;
			set;
		}

		public MyDefinitionId? Definition
		{
			get;
			set;
		}

		public virtual Type[] Dependencies => Type.EmptyTypes;

		/// <summary>
		/// Indicates whether a session component should be used in current configuration.
		/// Example: MyDestructionData component returns true only when game uses Havok Destruction
		/// </summary>
		public virtual bool IsRequiredByGame => false;

		public virtual bool UpdatedBeforeInit()
		{
			return false;
		}

		public MySessionComponentBase()
		{
			Type type = GetType();
			MySessionComponentDescriptor mySessionComponentDescriptor = (MySessionComponentDescriptor)Attribute.GetCustomAttribute(type, typeof(MySessionComponentDescriptor), inherit: false);
			DebugName = type.Name;
			Priority = mySessionComponentDescriptor.Priority;
			UpdateOrder = mySessionComponentDescriptor.UpdateOrder;
			ObjectBuilderType = mySessionComponentDescriptor.ObjectBuilderType;
			ComponentType = mySessionComponentDescriptor.ComponentType;
			if (ObjectBuilderType != MyObjectBuilderType.Invalid)
			{
				MySessionComponentMapping.Map(GetType(), ObjectBuilderType);
			}
			if (ComponentType == null)
			{
				ComponentType = GetType();
			}
			else if (ComponentType == GetType() || ComponentType.IsSubclassOf(GetType()))
			{
				MyLog.Default.Error("Component {0} tries to register itself as a component it does not inherit from ({1}). Ignoring...", GetType(), ComponentType);
				ComponentType = GetType();
			}
		}

		public void SetUpdateOrder(MyUpdateOrder order)
		{
			Session.SetComponentUpdateOrder(this, order);
			UpdateOrder = order;
		}

		public virtual void InitFromDefinition(MySessionComponentDefinition definition)
		{
		}

		public virtual void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			m_initialized = true;
			if (sessionComponent != null && sessionComponent.Definition.HasValue)
			{
				Definition = sessionComponent.Definition;
			}
			if (Definition.HasValue)
			{
				MySessionComponentDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MySessionComponentDefinition>(Definition.Value);
				if (definition == null)
				{
					MyLog.Default.Warning("Missing definition {0} : for session component {1}", Definition, GetType().Name);
				}
				else
				{
					InitFromDefinition(definition);
				}
			}
		}

		public virtual MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			if (ObjectBuilderType != MyObjectBuilderType.Invalid)
			{
				MyObjectBuilder_SessionComponent obj = Activator.CreateInstance(ObjectBuilderType) as MyObjectBuilder_SessionComponent;
				obj.Definition = Definition;
				return obj;
			}
			return null;
		}

		public void AfterLoadData()
		{
			Loaded = true;
		}

		public void UnloadDataConditional()
		{
			if (Loaded)
			{
				UnloadData();
				Loaded = false;
			}
		}

		public virtual void LoadData()
		{
		}

		protected virtual void UnloadData()
		{
		}

		public virtual void SaveData()
		{
		}

		public virtual void BeforeStart()
		{
		}

		public virtual void UpdateBeforeSimulation()
		{
		}

		public virtual void Simulate()
		{
		}

		public virtual void UpdateAfterSimulation()
		{
		}

		public virtual void UpdatingStopped()
		{
		}

		public virtual void Draw()
		{
		}

		public virtual void HandleInput()
		{
		}

		public override string ToString()
		{
			return DebugName;
		}
	}
}
