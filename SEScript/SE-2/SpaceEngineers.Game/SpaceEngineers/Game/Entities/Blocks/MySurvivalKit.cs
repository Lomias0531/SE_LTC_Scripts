using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SurvivalKit))]
	public class MySurvivalKit : MyAssembler, IMyLifeSupportingBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, IMyRechargeSocketOwner, IMySpawnBlock
	{
		protected sealed class RequestSupport_003C_003ESystem_Int64 : ICallSite<MySurvivalKit, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySurvivalKit @this, in long userId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RequestSupport(userId);
			}
		}

		protected sealed class SetSpawnTextEvent_003C_003ESystem_String : ICallSite<MySurvivalKit, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySurvivalKit @this, in string text, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetSpawnTextEvent(text);
			}
		}

		private readonly List<MyTextPanelComponent> m_panels = new List<MyTextPanelComponent>();

		private MyLifeSupportingComponent m_lifeSupportingComponent;

		public new MySurvivalKitDefinition BlockDefinition => (MySurvivalKitDefinition)base.BlockDefinition;

		public override bool SupportsAdvancedFunctions => false;

		MyRechargeSocket IMyRechargeSocketOwner.RechargeSocket => m_lifeSupportingComponent.RechargeSocket;

		bool IMyLifeSupportingBlock.RefuelAllowed => true;

		bool IMyLifeSupportingBlock.HealingAllowed => true;

		MyLifeSupportingBlockType IMyLifeSupportingBlock.BlockType => MyLifeSupportingBlockType.SurvivalKit;

		public override int GUIPriority => 600;

		/// <summary>
		/// The text displayed in the spawn menu
		/// </summary>
		public StringBuilder SpawnName
		{
			get;
			private set;
		}

		string IMySpawnBlock.SpawnName => SpawnName.ToString();

		public MySurvivalKit()
		{
			SpawnName = new StringBuilder();
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MySurvivalKit>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MySurvivalKit>("SpawnName", MySpaceTexts.SurvivalKit_SpawnNameLabel, MySpaceTexts.SurvivalKit_SpawnNameToolTip)
				{
					Getter = ((MySurvivalKit x) => x.SpawnName),
					Setter = delegate(MySurvivalKit x, StringBuilder v)
					{
						x.SetSpawnName(v);
					},
					SupportsMultipleBlocks = false
				});
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_SurvivalKit obj = (MyObjectBuilder_SurvivalKit)base.GetObjectBuilderCubeBlock(copy);
			obj.SpawnName = SpawnName.ToString();
			return obj;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			MyObjectBuilder_SurvivalKit myObjectBuilder_SurvivalKit = objectBuilder as MyObjectBuilder_SurvivalKit;
			SpawnName.Clear();
			if (myObjectBuilder_SurvivalKit.SpawnName != null)
			{
				SpawnName.Append(myObjectBuilder_SurvivalKit.SpawnName);
			}
			MySoundPair progressSound = new MySoundPair(BlockDefinition.ProgressSound);
			m_lifeSupportingComponent = new MyLifeSupportingComponent(this, progressSound);
			base.Components.Add(m_lifeSupportingComponent);
			if (base.CubeGrid.CreatePhysics)
			{
				base.Components.Add((MyEntityRespawnComponentBase)new MyRespawnComponent());
			}
			base.ResourceSink.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			List<ScreenArea> screenAreas = BlockDefinition.ScreenAreas;
			if (screenAreas != null && screenAreas.Count > 0)
			{
				for (int i = 0; i < screenAreas.Count; i++)
				{
					MyTextPanelComponent myTextPanelComponent = new MyTextPanelComponent(i, this, screenAreas[i].Name, screenAreas[i].DisplayName, screenAreas[i].TextureResolution);
					m_panels.Add(myTextPanelComponent);
					base.SyncType.Append(myTextPanelComponent);
					myTextPanelComponent.Init();
				}
			}
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			m_lifeSupportingComponent.Update10();
		}

		public override void UpdateSoundEmitters()
		{
			base.UpdateSoundEmitters();
			m_lifeSupportingComponent.UpdateSoundEmitters();
		}

		void IMyLifeSupportingBlock.ShowTerminal(MyCharacter user)
		{
			MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, this);
		}

		void IMyLifeSupportingBlock.BroadcastSupportRequest(MyCharacter user)
		{
			MyMultiplayer.RaiseEvent(this, (MySurvivalKit x) => x.RequestSupport, user.EntityId);
		}

		[Event(null, 163)]
		[Reliable]
		[Server(ValidationType.Access)]
		[Broadcast]
		private void RequestSupport(long userId)
		{
			if (GetUserRelationToOwner(MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value)).IsFriendly() || MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				MyEntities.TryGetEntityById(userId, out MyCharacter entity);
				if (entity != null)
				{
					m_lifeSupportingComponent.ProvideSupport(entity);
				}
			}
		}

		public override bool AllowSelfPulling()
		{
			return true;
		}

		private void SetSpawnName(StringBuilder text)
		{
			if (SpawnName.CompareUpdate(text))
			{
				MyMultiplayer.RaiseEvent(this, (MySurvivalKit x) => x.SetSpawnTextEvent, text.ToString());
			}
		}

		[Event(null, 206)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		protected void SetSpawnTextEvent(string text)
		{
			SpawnName.CompareUpdate(text);
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateScreen();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_panels.Count > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			foreach (MyTextPanelComponent panel in m_panels)
			{
				panel.SetRender((MyRenderComponentScreenAreas)base.Render);
				((MyRenderComponentScreenAreas)base.Render).AddScreenArea(base.Render.RenderObjectIDs, panel.Name);
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public void UpdateScreen()
		{
			if (!CheckIsWorking())
			{
				for (int i = 0; i < m_panels.Count; i++)
				{
					((MyRenderComponentScreenAreas)base.Render).ChangeTexture(i, m_panels[i].GetPathForID("Offline"));
				}
			}
			else
			{
				for (int j = 0; j < m_panels.Count; j++)
				{
					((MyRenderComponentScreenAreas)base.Render).ChangeTexture(j, null);
				}
			}
		}

		public override float GetEfficiencyMultiplierForBlueprint(MyBlueprintDefinitionBase targetBlueprint)
		{
			MyBlueprintDefinitionBase.Item[] prerequisites = targetBlueprint.Prerequisites;
			for (int i = 0; i < prerequisites.Length; i++)
			{
				if (prerequisites[i].Id.TypeId == typeof(MyObjectBuilder_Ore))
				{
					return 1f;
				}
			}
			return base.GetEfficiencyMultiplierForBlueprint(targetBlueprint);
		}

		protected override void OnStartWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}
	}
}
