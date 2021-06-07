using Sandbox.Engine.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_ModifiableEntity), true)]
	public class MyModifiableEntity : MyEntity, IMyEventProxy, IMyEventOwner
	{
		protected sealed class AddAssetModifierSync_003C_003EVRage_ObjectBuilders_SerializableDefinitionId : ICallSite<MyModifiableEntity, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyModifiableEntity @this, in SerializableDefinitionId id, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.AddAssetModifierSync(id);
			}
		}

		private class Sandbox_Game_Entities_MyModifiableEntity_003C_003EActor : IActivator, IActivator<MyModifiableEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MyModifiableEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyModifiableEntity CreateInstance()
			{
				return new MyModifiableEntity();
			}

			MyModifiableEntity IActivator<MyModifiableEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private List<MyDefinitionId> m_assetModifiers;

		private bool m_assetModifiersDirty;

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_ModifiableEntity myObjectBuilder_ModifiableEntity = objectBuilder as MyObjectBuilder_ModifiableEntity;
			if (myObjectBuilder_ModifiableEntity != null)
			{
				m_assetModifiersDirty = false;
				if (myObjectBuilder_ModifiableEntity.AssetModifiers != null && myObjectBuilder_ModifiableEntity.AssetModifiers.Count > 0)
				{
					m_assetModifiersDirty = true;
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					m_assetModifiers = new List<MyDefinitionId>();
					foreach (SerializableDefinitionId assetModifier in myObjectBuilder_ModifiableEntity.AssetModifiers)
					{
						m_assetModifiers.Add(assetModifier);
					}
				}
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (m_assetModifiersDirty)
			{
				MySession.Static.GetComponent<MySessionComponentAssetModifiers>();
				foreach (MyDefinitionId assetModifier in m_assetModifiers)
				{
					_ = assetModifier;
				}
				m_assetModifiersDirty = false;
			}
		}

		public void AddAssetModifier(MyDefinitionId id)
		{
			MyMultiplayer.RaiseEvent(this, (Func<MyModifiableEntity, Action<SerializableDefinitionId>>)((MyModifiableEntity x) => x.AddAssetModifierSync), (SerializableDefinitionId)id, default(EndpointId));
		}

		[Event(null, 65)]
		[Reliable]
		[Broadcast]
		[Server]
		private void AddAssetModifierSync(SerializableDefinitionId id)
		{
			if (m_assetModifiers == null)
			{
				m_assetModifiers = new List<MyDefinitionId>();
			}
			m_assetModifiers.Add(id);
			MySession.Static.GetComponent<MySessionComponentAssetModifiers>();
		}

		public void RemoveAssetModifier(MyDefinitionId id)
		{
			if (m_assetModifiers != null)
			{
				m_assetModifiers.Remove(id);
			}
		}

		public List<MyDefinitionId> GetAssetModifiers()
		{
			return m_assetModifiers;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_ModifiableEntity myObjectBuilder_ModifiableEntity = (MyObjectBuilder_ModifiableEntity)base.GetObjectBuilder(copy);
			if (m_assetModifiers != null && m_assetModifiers.Count > 0)
			{
				myObjectBuilder_ModifiableEntity.AssetModifiers = new List<SerializableDefinitionId>();
				{
					foreach (MyDefinitionId assetModifier in m_assetModifiers)
					{
						myObjectBuilder_ModifiableEntity.AssetModifiers.Add(assetModifier);
					}
					return myObjectBuilder_ModifiableEntity;
				}
			}
			return myObjectBuilder_ModifiableEntity;
		}
	}
}
