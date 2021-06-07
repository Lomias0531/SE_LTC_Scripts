using Sandbox.Engine.Platform;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;

namespace Sandbox.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_ContainerDropComponent), true)]
	public class MyContainerDropComponent : MyEntityComponentBase
	{
		private class Sandbox_Game_Components_MyContainerDropComponent_003C_003EActor : IActivator, IActivator<MyContainerDropComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyContainerDropComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContainerDropComponent CreateInstance()
			{
				return new MyContainerDropComponent();
			}

			MyContainerDropComponent IActivator<MyContainerDropComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyEntity3DSoundEmitter m_soundEmitter;

		private string m_playingSound;

		private bool m_playSound;

		public bool Competetive
		{
			get;
			private set;
		}

		public string GPSName
		{
			get;
			private set;
		}

		public long Owner
		{
			get;
			private set;
		}

		public long GridEntityId
		{
			get;
			set;
		}

		public override string ComponentTypeDebugString => "ContainerDropComponent";

		public MyContainerDropComponent()
		{
		}

		public MyContainerDropComponent(bool competetive, string gpsName, long owner, string sound)
		{
			Competetive = competetive;
			GPSName = gpsName;
			Owner = owner;
			m_playingSound = sound;
			m_playSound = !string.IsNullOrEmpty(m_playingSound);
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_ContainerDropComponent obj = MyComponentFactory.CreateObjectBuilder(this) as MyObjectBuilder_ContainerDropComponent;
			obj.Competetive = Competetive;
			obj.GPSName = GPSName;
			obj.Owner = Owner;
			obj.PlayingSound = m_playingSound;
			return obj;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase baseBuilder)
		{
			MyObjectBuilder_ContainerDropComponent myObjectBuilder_ContainerDropComponent = baseBuilder as MyObjectBuilder_ContainerDropComponent;
			Competetive = myObjectBuilder_ContainerDropComponent.Competetive;
			GPSName = myObjectBuilder_ContainerDropComponent.GPSName;
			Owner = myObjectBuilder_ContainerDropComponent.Owner;
			m_playingSound = myObjectBuilder_ContainerDropComponent.PlayingSound;
			m_playSound = !string.IsNullOrEmpty(m_playingSound);
		}

		public void PlaySound(string soundName)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_playingSound = soundName;
				return;
			}
			MySoundPair mySoundPair = new MySoundPair(soundName);
			if (mySoundPair.Arcade.IsNull && mySoundPair.Realistic.IsNull)
			{
				return;
			}
			if (m_soundEmitter == null)
			{
				m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity)base.Entity, useStaticList: true);
				MyCubeBlock myCubeBlock = base.Entity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeBlock.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
				}
			}
			m_soundEmitter.PlaySound(mySoundPair, stopPrevious: true);
			m_playingSound = soundName;
		}

		public void StopSound()
		{
			if (m_soundEmitter != null && m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			if (m_playSound)
			{
				m_playSound = false;
				PlaySound(m_playingSound);
			}
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			if (m_playSound)
			{
				m_playSound = false;
				PlaySound(m_playingSound);
			}
		}

		public void UpdateSound()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			StopSound();
			m_soundEmitter = null;
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
			StopSound();
			m_soundEmitter = null;
			if (Sync.IsServer)
			{
				MySession.Static.GetComponent<MySessionComponentContainerDropSystem>().ContainerDestroyed(this);
			}
		}
	}
}
