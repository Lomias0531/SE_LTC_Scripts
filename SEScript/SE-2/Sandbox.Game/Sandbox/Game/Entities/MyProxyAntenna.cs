using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_ProxyAntenna), true)]
	internal class MyProxyAntenna : MyEntity, IMyComponentOwner<MyIDModule>
	{
		private class Sandbox_Game_Entities_MyProxyAntenna_003C_003EActor : IActivator, IActivator<MyProxyAntenna>
		{
			private sealed override object CreateInstance()
			{
				return new MyProxyAntenna();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProxyAntenna CreateInstance()
			{
				return new MyProxyAntenna();
			}

			MyProxyAntenna IActivator<MyProxyAntenna>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Dictionary<long, MyHudEntityParams> m_savedHudParams = new Dictionary<long, MyHudEntityParams>();

		private bool m_active;

		private MyIDModule m_IDModule = new MyIDModule();

		private bool m_registered;

		public bool Active
		{
			get
			{
				return m_active;
			}
			set
			{
				if (m_active == value)
				{
					return;
				}
				m_active = value;
				if (Receiver != null)
				{
					Receiver.Enabled = value;
					if (value)
					{
						base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
					}
					else
					{
						base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
					}
				}
				MyRadioBroadcaster myRadioBroadcaster = Broadcaster as MyRadioBroadcaster;
				if (myRadioBroadcaster != null)
				{
					myRadioBroadcaster.Enabled = m_active;
					return;
				}
				MyLaserBroadcaster myLaserBroadcaster = Broadcaster as MyLaserBroadcaster;
				if (myLaserBroadcaster == null)
				{
					return;
				}
				if (m_active)
				{
					if (!MyAntennaSystem.Static.LaserAntennas.ContainsKey(AntennaEntityId))
					{
						MyAntennaSystem.Static.AddLaser(AntennaEntityId, myLaserBroadcaster, register: false);
					}
				}
				else
				{
					MyAntennaSystem.Static.RemoveLaser(AntennaEntityId, register: false);
				}
			}
		}

		public bool IsCharacter
		{
			get;
			private set;
		}

		public MyDataBroadcaster Broadcaster
		{
			get
			{
				return base.Components.Get<MyDataBroadcaster>();
			}
			set
			{
				base.Components.Add(value);
			}
		}

		public MyDataReceiver Receiver
		{
			get
			{
				return base.Components.Get<MyDataReceiver>();
			}
			set
			{
				base.Components.Add(value);
			}
		}

		public MyAntennaSystem.BroadcasterInfo Info
		{
			get;
			set;
		}

		public long AntennaEntityId
		{
			get;
			private set;
		}

		public long? SuccessfullyContacting
		{
			get;
			set;
		}

		public bool HasRemoteControl
		{
			get;
			set;
		}

		public long? MainRemoteControlOwner
		{
			get;
			set;
		}

		public long? MainRemoteControlId
		{
			get;
			set;
		}

		public MyOwnershipShareModeEnum MainRemoteControlSharing
		{
			get;
			set;
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_ProxyAntenna myObjectBuilder_ProxyAntenna = objectBuilder as MyObjectBuilder_ProxyAntenna;
			AntennaEntityId = myObjectBuilder_ProxyAntenna.AntennaEntityId;
			base.PositionComp.SetPosition(myObjectBuilder_ProxyAntenna.Position);
			IsCharacter = myObjectBuilder_ProxyAntenna.IsCharacter;
			if (!myObjectBuilder_ProxyAntenna.IsLaser)
			{
				MyRadioBroadcaster myRadioBroadcaster = (MyRadioBroadcaster)(Broadcaster = new MyRadioBroadcaster());
				myRadioBroadcaster.BroadcastRadius = myObjectBuilder_ProxyAntenna.BroadcastRadius;
				base.PositionComp.OnPositionChanged += WorldPositionChanged;
				if (myObjectBuilder_ProxyAntenna.HasReceiver)
				{
					Receiver = new MyRadioReceiver();
				}
			}
			else
			{
				MyLaserBroadcaster myLaserBroadcaster = (MyLaserBroadcaster)(Broadcaster = new MyLaserBroadcaster());
				SuccessfullyContacting = myObjectBuilder_ProxyAntenna.SuccessfullyContacting;
				myLaserBroadcaster.StateText.Clear().Append(myObjectBuilder_ProxyAntenna.StateText);
				Receiver = new MyLaserReceiver();
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			m_IDModule.Owner = myObjectBuilder_ProxyAntenna.Owner;
			m_IDModule.ShareMode = myObjectBuilder_ProxyAntenna.Share;
			Info = new MyAntennaSystem.BroadcasterInfo
			{
				EntityId = myObjectBuilder_ProxyAntenna.InfoEntityId,
				Name = myObjectBuilder_ProxyAntenna.InfoName
			};
			foreach (MyObjectBuilder_HudEntityParams hudParam in myObjectBuilder_ProxyAntenna.HudParams)
			{
				m_savedHudParams[hudParam.EntityId] = new MyHudEntityParams(hudParam);
			}
			HasRemoteControl = myObjectBuilder_ProxyAntenna.HasRemote;
			MainRemoteControlOwner = myObjectBuilder_ProxyAntenna.MainRemoteOwner;
			MainRemoteControlId = myObjectBuilder_ProxyAntenna.MainRemoteId;
			MainRemoteControlSharing = myObjectBuilder_ProxyAntenna.MainRemoteSharing;
		}

		public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
		{
			m_hudParams.Clear();
			foreach (MyHudEntityParams value in m_savedHudParams.Values)
			{
				m_hudParams.Add(new MyHudEntityParams
				{
					EntityId = value.EntityId,
					FlagsEnum = value.FlagsEnum,
					Owner = value.Owner,
					Share = value.Share,
					Position = base.PositionComp.GetPosition(),
					Text = value.Text,
					BlinkingTime = value.BlinkingTime
				});
			}
			return m_hudParams;
		}

		public override void UpdateOnceBeforeFrame()
		{
			m_registered = true;
			MyAntennaSystem.Static.RegisterAntenna(Broadcaster);
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (Receiver != null)
			{
				Receiver.UpdateBroadcastersInRange();
			}
		}

		private void WorldPositionChanged(object source)
		{
			(Broadcaster as MyRadioBroadcaster)?.MoveBroadcaster();
		}

		protected override void Closing()
		{
			if (m_registered)
			{
				MyAntennaSystem.Static.UnregisterAntenna(Broadcaster);
			}
			m_registered = false;
			base.Closing();
		}

		public void ChangeOwner(long newOwner, MyOwnershipShareModeEnum newShare)
		{
			m_IDModule.Owner = newOwner;
			m_IDModule.ShareMode = newShare;
		}

		public void ChangeHudParams(List<MyObjectBuilder_HudEntityParams> newHudParams)
		{
			foreach (MyObjectBuilder_HudEntityParams newHudParam in newHudParams)
			{
				m_savedHudParams[newHudParam.EntityId] = new MyHudEntityParams(newHudParam);
			}
		}

		bool IMyComponentOwner<MyIDModule>.GetComponent(out MyIDModule component)
		{
			component = m_IDModule;
			return m_IDModule != null;
		}
	}
}
