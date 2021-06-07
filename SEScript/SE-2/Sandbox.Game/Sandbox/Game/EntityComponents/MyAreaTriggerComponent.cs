using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[MyComponentBuilder(typeof(MyObjectBuilder_AreaTrigger), true)]
	public class MyAreaTriggerComponent : MyTriggerComponent
	{
		private class Sandbox_Game_EntityComponents_MyAreaTriggerComponent_003C_003EActor : IActivator, IActivator<MyAreaTriggerComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyAreaTriggerComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAreaTriggerComponent CreateInstance()
			{
				return new MyAreaTriggerComponent();
			}

			MyAreaTriggerComponent IActivator<MyAreaTriggerComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly HashSet<MyEntity> m_prevQuery = new HashSet<MyEntity>();

		private readonly List<MyEntity> m_resultsToRemove = new List<MyEntity>();

		public Action<long, string> EntityEntered;

		public string Name
		{
			get;
			set;
		}

		public double Radius
		{
			get
			{
				return m_boundingSphere.Radius;
			}
			set
			{
				m_boundingSphere.Radius = value;
			}
		}

		public new Vector3D Center
		{
			get
			{
				return m_boundingSphere.Center;
			}
			set
			{
				m_boundingSphere.Center = value;
				if (base.Entity != null)
				{
					DefaultTranslation = m_boundingSphere.Center - base.Entity.PositionComp.GetPosition();
				}
			}
		}

		public MyAreaTriggerComponent()
			: this(string.Empty)
		{
		}

		public MyAreaTriggerComponent(string name)
			: base(TriggerType.Sphere, 20u)
		{
			Name = name;
		}

		protected override void UpdateInternal()
		{
			base.UpdateInternal();
			foreach (MyEntity item in m_prevQuery)
			{
				if (!base.QueryResult.Contains(item))
				{
					if (MyVisualScriptLogicProvider.AreaTrigger_EntityLeft != null)
					{
						MyVisualScriptLogicProvider.AreaTrigger_EntityLeft(Name, item.EntityId, item.Name);
					}
					if (MyVisualScriptLogicProvider.AreaTrigger_Left != null)
					{
						MyCharacter myCharacter = item as MyCharacter;
						if ((myCharacter == null || !myCharacter.IsBot) && MySession.Static.Players.ControlledEntities.TryGetValue(item.EntityId, out MyPlayer.PlayerId value))
						{
							MyIdentity myIdentity = MySession.Static.Players.TryGetPlayerIdentity(value);
							MyVisualScriptLogicProvider.AreaTrigger_Left(Name, myIdentity.IdentityId);
						}
					}
					m_resultsToRemove.Add(item);
				}
			}
			foreach (MyEntity item2 in m_resultsToRemove)
			{
				m_prevQuery.Remove(item2);
			}
			m_resultsToRemove.Clear();
			foreach (MyEntity item3 in base.QueryResult)
			{
				if (m_prevQuery.Add(item3))
				{
					if (MyVisualScriptLogicProvider.AreaTrigger_EntityEntered != null)
					{
						MyVisualScriptLogicProvider.AreaTrigger_EntityEntered(Name, item3.EntityId, item3.Name);
					}
					if (EntityEntered != null)
					{
						EntityEntered(item3.EntityId, item3.Name);
					}
					if (MyVisualScriptLogicProvider.AreaTrigger_Entered != null)
					{
						MyCharacter myCharacter2 = item3 as MyCharacter;
						if ((myCharacter2 == null || !myCharacter2.IsBot) && MySession.Static.Players.ControlledEntities.TryGetValue(item3.EntityId, out MyPlayer.PlayerId value2))
						{
							MyIdentity myIdentity2 = MySession.Static.Players.TryGetPlayerIdentity(value2);
							MyVisualScriptLogicProvider.AreaTrigger_Entered(Name, myIdentity2.IdentityId);
						}
					}
				}
			}
		}

		protected override bool QueryEvaluator(MyEntity entity)
		{
			if (entity is MyCharacter)
			{
				return true;
			}
			if (entity is MyCubeGrid)
			{
				return true;
			}
			return false;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy)
		{
			MyObjectBuilder_AreaTrigger obj = base.Serialize(copy) as MyObjectBuilder_AreaTrigger;
			obj.Name = Name;
			return obj;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_AreaTrigger myObjectBuilder_AreaTrigger = (MyObjectBuilder_AreaTrigger)builder;
			Name = myObjectBuilder_AreaTrigger.Name;
		}

		public override bool IsSerialized()
		{
			return true;
		}
	}
}
