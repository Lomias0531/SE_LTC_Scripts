using ObjectBuilders.Discovery;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;

namespace SpaceEngineers.Game.EntityComponents.GameLogic.Discovery
{
	[MyComponentBuilder(typeof(MyObjectBuilder_CharacterDiscoveryComponent), true)]
	public class MyCharacterDiscoveryComponent : MyEntityComponentBase
	{
		private MyHudNotification m_hudNotificiation;

		private MyRadioReceiver m_reciever;

		public override string ComponentTypeDebugString => "CharacterDiscoveryComponent";

		public MyCharacterDiscoveryComponent()
		{
			m_hudNotificiation = new MyHudNotification(MySpaceTexts.Faction_Discovered_Info);
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			m_reciever = (base.Container.Get<MyDataReceiver>() as MyRadioReceiver);
			if (m_reciever != null)
			{
				if (Sync.IsServer)
				{
					m_reciever.OnBroadcasterFound += OnBroadcasterDiscovered;
				}
				MySession.Static.Factions.OnFactionDiscovered += OnFactionDiscovered;
			}
		}

		public override void OnRemovedFromScene()
		{
			MySession.Static.Factions.OnFactionDiscovered -= OnFactionDiscovered;
			if (Sync.IsServer)
			{
				m_reciever.OnBroadcasterFound -= OnBroadcasterDiscovered;
			}
			base.OnRemovedFromScene();
		}

		/// <summary>
		/// This is called when new broadcaster is found. It is only happening on server.
		/// </summary>
		/// <param name="broadcaster">Found broadcaster.</param>
		private void OnBroadcasterDiscovered(MyDataBroadcaster broadcaster)
		{
			MyPlayer controllingPlayer = GetControllingPlayer();
			if (controllingPlayer == null)
			{
				return;
			}
			MyCubeBlock myCubeBlock = broadcaster.Entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(myCubeBlock.OwnerId);
				if (playerFaction != null && MySession.Static.Factions.IsNpcFaction(playerFaction.Tag) && !MySession.Static.Factions.IsFactionDiscovered(controllingPlayer.Id, playerFaction.FactionId))
				{
					MySession.Static.Factions.AddDiscoveredFaction(controllingPlayer.Id, playerFaction.FactionId);
				}
			}
		}

		/// <summary>
		/// This is called on clients on response from server when faction is discovered.
		/// </summary>
		/// <param name="discoveredFaction"></param>
		private void OnFactionDiscovered(MyFaction discoveredFaction, MyPlayer.PlayerId playerId)
		{
			if (!Sync.IsDedicated && MySession.Static != null)
			{
				MyPlayer controllingPlayer = GetControllingPlayer();
				MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
				if (controllingPlayer?.Id == localHumanPlayer.Id && controllingPlayer != null && controllingPlayer.Id == playerId)
				{
					m_hudNotificiation.SetTextFormatArguments(discoveredFaction.Name);
					MyHud.Notifications.Add(m_hudNotificiation);
				}
			}
		}

		private MyPlayer GetControllingPlayer()
		{
			MyEntity myEntity = base.Entity as MyEntity;
			if (myEntity == null)
			{
				return null;
			}
			MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(myEntity);
			MyCharacter myCharacter;
			if (controllingPlayer == null && (myCharacter = (myEntity as MyCharacter)) != null && myCharacter.IsUsing != null)
			{
				controllingPlayer = MySession.Static.Players.GetControllingPlayer(myCharacter.IsUsing);
			}
			return controllingPlayer;
		}
	}
}
