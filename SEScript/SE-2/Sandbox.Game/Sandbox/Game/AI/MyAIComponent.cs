using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI.BehaviorTree;
using Sandbox.Game.AI.Commands;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
	[MySessionComponentDescriptor(MyUpdateOrder.Simulation | MyUpdateOrder.AfterSimulation, 1000, typeof(MyObjectBuilder_AIComponent), null)]
	public class MyAIComponent : MySessionComponentBase
	{
		private struct AgentSpawnData
		{
			public MyAgentDefinition AgentDefinition;

			public Vector3D? SpawnPosition;

			public bool CreatedByPlayer;

			public int BotId;

			public AgentSpawnData(MyAgentDefinition agentDefinition, int botId, Vector3D? spawnPosition = null, bool createAlways = false)
			{
				AgentDefinition = agentDefinition;
				SpawnPosition = spawnPosition;
				CreatedByPlayer = createAlways;
				BotId = botId;
			}
		}

		public struct AgentGroupData
		{
			public MyAgentDefinition AgentDefinition;

			public int Count;

			public AgentGroupData(MyAgentDefinition agentDefinition, int count)
			{
				AgentDefinition = agentDefinition;
				Count = count;
			}
		}

		private struct BotRemovalRequest
		{
			public int SerialId;

			public bool RemoveCharacter;
		}

		private MyBotCollection m_botCollection;

		private IMyPathfinding m_pathfinding;

		private MyBehaviorTreeCollection m_behaviorTreeCollection;

		private Dictionary<int, MyObjectBuilder_Bot> m_loadedBotObjectBuildersByHandle;

		private List<int> m_loadedLocalPlayers;

		private List<Vector3D> m_tmpSpawnPoints = new List<Vector3D>();

		public static MyAIComponent Static;

		public static MyBotFactoryBase BotFactory;

		private int m_lastBotId;

		private Dictionary<int, AgentSpawnData> m_agentsToSpawn;

		private MyHudNotification m_maxBotNotification;

		private bool m_debugDrawPathfinding;

		public MyAgentDefinition BotToSpawn;

		public MyAiCommandDefinition CommandDefinition;

		private MyConcurrentQueue<BotRemovalRequest> m_removeQueue;

		private MyConcurrentQueue<AgentSpawnData> m_processQueue;

		private FastResourceLock m_lock;

		private BoundingBoxD m_debugTargetAABB;

		public MyBotCollection Bots => m_botCollection;

		public IMyPathfinding Pathfinding => m_pathfinding;

		public MyBehaviorTreeCollection BehaviorTrees => m_behaviorTreeCollection;

		public override Type[] Dependencies => new Type[1]
		{
			typeof(MyToolbarComponent)
		};

		public Vector3D? DebugTarget
		{
			get;
			private set;
		}

		public event Action<int, MyBotDefinition> BotCreatedEvent;

		public MyAIComponent()
		{
			Static = this;
			BotFactory = (Activator.CreateInstance(MyPerGameSettings.BotFactoryType) as MyBotFactoryBase);
		}

		public override void LoadData()
		{
			base.LoadData();
			if (MyPerGameSettings.EnableAi)
			{
				Sync.Players.NewPlayerRequestSucceeded += PlayerCreated;
				Sync.Players.LocalPlayerLoaded += LocalPlayerLoaded;
				Sync.Players.NewPlayerRequestFailed += Players_NewPlayerRequestFailed;
				if (Sync.IsServer)
				{
					Sync.Players.PlayerRemoved += Players_PlayerRemoved;
					Sync.Players.PlayerRequesting += Players_PlayerRequesting;
				}
				if (MyPerGameSettings.PathfindingType != null)
				{
					m_pathfinding = (Activator.CreateInstance(MyPerGameSettings.PathfindingType) as IMyPathfinding);
				}
				m_behaviorTreeCollection = new MyBehaviorTreeCollection();
				m_botCollection = new MyBotCollection(m_behaviorTreeCollection);
				m_loadedLocalPlayers = new List<int>();
				m_loadedBotObjectBuildersByHandle = new Dictionary<int, MyObjectBuilder_Bot>();
				m_agentsToSpawn = new Dictionary<int, AgentSpawnData>();
				m_removeQueue = new MyConcurrentQueue<BotRemovalRequest>();
				m_maxBotNotification = new MyHudNotification(MyCommonTexts.NotificationMaximumNumberBots, 2000, "Red");
				m_processQueue = new MyConcurrentQueue<AgentSpawnData>();
				m_lock = new FastResourceLock();
				if (MyFakes.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION && MyVRage.Platform.Window != null)
				{
					MyVRage.Platform.Window.AddMessageHandler(1034u, OnUploadNewTree);
					MyVRage.Platform.Window.AddMessageHandler(1036u, OnBreakDebugging);
					MyVRage.Platform.Window.AddMessageHandler(1035u, OnResumeDebugging);
				}
				MyToolbarComponent.CurrentToolbar.SelectedSlotChanged += CurrentToolbar_SelectedSlotChanged;
				MyToolbarComponent.CurrentToolbar.SlotActivated += CurrentToolbar_SlotActivated;
				MyToolbarComponent.CurrentToolbar.Unselected += CurrentToolbar_Unselected;
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponentBuilder)
		{
			if (MyPerGameSettings.EnableAi)
			{
				base.Init(sessionComponentBuilder);
				MyObjectBuilder_AIComponent myObjectBuilder_AIComponent = (MyObjectBuilder_AIComponent)sessionComponentBuilder;
				if (myObjectBuilder_AIComponent.BotBrains != null)
				{
					foreach (MyObjectBuilder_AIComponent.BotData botBrain in myObjectBuilder_AIComponent.BotBrains)
					{
						m_loadedBotObjectBuildersByHandle[botBrain.PlayerHandle] = botBrain.BotBrain;
					}
				}
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (MyPerGameSettings.EnableAi)
			{
				foreach (int loadedLocalPlayer in m_loadedLocalPlayers)
				{
					MyObjectBuilder_Bot value = null;
					m_loadedBotObjectBuildersByHandle.TryGetValue(loadedLocalPlayer, out value);
					if (value == null || value.TypeId == value.BotDefId.TypeId)
					{
						CreateBot(loadedLocalPlayer, value);
					}
				}
				m_loadedLocalPlayers.Clear();
				m_loadedBotObjectBuildersByHandle.Clear();
				Sync.Players.LocalPlayerRemoved += LocalPlayerRemoved;
			}
		}

		public override void Simulate()
		{
			if (!MyPerGameSettings.EnableAi)
			{
				return;
			}
			if (MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP_SETTING)
			{
				if (!MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP)
				{
					return;
				}
			}
			else if (MyFakes.DEBUG_ONE_AI_STEP_SETTING)
			{
				if (!MyFakes.DEBUG_ONE_AI_STEP)
				{
					return;
				}
				MyFakes.DEBUG_ONE_AI_STEP = false;
			}
			MySimpleProfiler.Begin("AI", MySimpleProfiler.ProfilingBlockType.OTHER, "Simulate");
			if (m_pathfinding != null)
			{
				m_pathfinding.Update();
			}
			base.Simulate();
			m_behaviorTreeCollection.Update();
			m_botCollection.Update();
			MySimpleProfiler.End("Simulate");
		}

		public void PathfindingSetDrawDebug(bool drawDebug)
		{
			m_debugDrawPathfinding = drawDebug;
		}

		public void PathfindingSetDrawNavmesh(bool drawNavmesh)
		{
			(m_pathfinding as MyRDPathfinding)?.SetDrawNavmesh(drawNavmesh);
		}

		public void GenerateNavmeshTile(Vector3D? target)
		{
			if (target.HasValue)
			{
				Vector3D worldCenter = target.Value + 0.1f;
				MyDestinationSphere end = new MyDestinationSphere(ref worldCenter, 1f);
				Static.Pathfinding.FindPathGlobal(target.Value - 0.10000000149011612, end, null).GetNextTarget(target.Value, out Vector3D _, out float _, out IMyEntity _);
			}
			DebugTarget = target;
		}

		public void InvalidateNavmeshPosition(Vector3D? target)
		{
			if (target.HasValue)
			{
				MyRDPathfinding myRDPathfinding = (MyRDPathfinding)Static.Pathfinding;
				if (myRDPathfinding != null)
				{
					BoundingBoxD areaBox = new BoundingBoxD(target.Value - 0.1, target.Value + 0.1);
					myRDPathfinding.InvalidateArea(areaBox);
				}
			}
			DebugTarget = target;
		}

		public void SetPathfindingDebugTarget(Vector3D? target)
		{
			MyExternalPathfinding myExternalPathfinding = m_pathfinding as MyExternalPathfinding;
			if (myExternalPathfinding != null)
			{
				myExternalPathfinding.SetTarget(target);
			}
			else if (target.HasValue)
			{
				m_debugTargetAABB = new MyOrientedBoundingBoxD(target.Value, new Vector3D(5.0, 5.0, 5.0), Quaternion.Identity).GetAABB();
				List<MyEntity> result = new List<MyEntity>();
				MyGamePruningStructure.GetAllEntitiesInBox(ref m_debugTargetAABB, result);
			}
			DebugTarget = target;
		}

		private void DrawDebugTarget()
		{
			if (DebugTarget.HasValue)
			{
				MyRenderProxy.DebugDrawSphere(DebugTarget.Value, 0.2f, Color.Red, 0f, depthRead: false);
				MyRenderProxy.DebugDrawAABB(m_debugTargetAABB, Color.Green);
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (MyPerGameSettings.EnableAi)
			{
				PerformBotRemovals();
				AgentSpawnData instance;
				while (m_processQueue.TryDequeue(out instance))
				{
					m_agentsToSpawn[instance.BotId] = instance;
					Sync.Players.RequestNewPlayer(instance.BotId, MyDefinitionManager.Static.GetRandomCharacterName(), instance.AgentDefinition.BotModel, realPlayer: false, initialPlayer: false);
				}
				if (m_debugDrawPathfinding && m_pathfinding != null)
				{
					m_pathfinding.DebugDraw();
				}
				m_botCollection.DebugDraw();
				DebugDrawBots();
				DrawDebugTarget();
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			if (MyPerGameSettings.EnableAi)
			{
				Sync.Players.NewPlayerRequestSucceeded -= PlayerCreated;
				Sync.Players.LocalPlayerRemoved -= LocalPlayerRemoved;
				Sync.Players.LocalPlayerLoaded -= LocalPlayerLoaded;
				Sync.Players.NewPlayerRequestFailed -= Players_NewPlayerRequestFailed;
				if (Sync.IsServer)
				{
					Sync.Players.PlayerRequesting -= Players_PlayerRequesting;
					Sync.Players.PlayerRemoved -= Players_PlayerRemoved;
				}
				if (m_pathfinding != null)
				{
					m_pathfinding.UnloadData();
				}
				m_botCollection.UnloadData();
				m_botCollection = null;
				m_pathfinding = null;
				if (MyFakes.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION && MyVRage.Platform?.Window != null)
				{
					MyVRage.Platform.Window.RemoveMessageHandler(1034u, OnUploadNewTree);
					MyVRage.Platform.Window.RemoveMessageHandler(1036u, OnBreakDebugging);
					MyVRage.Platform.Window.RemoveMessageHandler(1035u, OnResumeDebugging);
				}
				if (MyToolbarComponent.CurrentToolbar != null)
				{
					MyToolbarComponent.CurrentToolbar.SelectedSlotChanged -= CurrentToolbar_SelectedSlotChanged;
					MyToolbarComponent.CurrentToolbar.SlotActivated -= CurrentToolbar_SlotActivated;
					MyToolbarComponent.CurrentToolbar.Unselected -= CurrentToolbar_Unselected;
				}
			}
			Static = null;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			if (!MyPerGameSettings.EnableAi)
			{
				return null;
			}
			MyObjectBuilder_AIComponent myObjectBuilder_AIComponent = (MyObjectBuilder_AIComponent)base.GetObjectBuilder();
			myObjectBuilder_AIComponent.BotBrains = new List<MyObjectBuilder_AIComponent.BotData>();
			m_botCollection.GetBotsData(myObjectBuilder_AIComponent.BotBrains);
			return myObjectBuilder_AIComponent;
		}

		public int SpawnNewBot(MyAgentDefinition agentDefinition)
		{
			Vector3D spawnPosition = default(Vector3D);
			if (!BotFactory.GetBotSpawnPosition(agentDefinition.BehaviorType, out spawnPosition))
			{
				return 0;
			}
			return SpawnNewBotInternal(agentDefinition, spawnPosition);
		}

		public int SpawnNewBot(MyAgentDefinition agentDefinition, Vector3D position, bool createdByPlayer = true)
		{
			return SpawnNewBotInternal(agentDefinition, position, createdByPlayer);
		}

		public bool SpawnNewBotGroup(string type, List<AgentGroupData> groupData, List<int> outIds)
		{
			int num = 0;
			foreach (AgentGroupData groupDatum in groupData)
			{
				num += groupDatum.Count;
			}
			BotFactory.GetBotGroupSpawnPositions(type, num, m_tmpSpawnPoints);
			int count = m_tmpSpawnPoints.Count;
			int i = 0;
			int num2 = 0;
			int num3 = 0;
			for (; i < count; i++)
			{
				int item = SpawnNewBotInternal(groupData[num2].AgentDefinition, m_tmpSpawnPoints[i]);
				outIds?.Add(item);
				if (groupData[num2].Count == ++num3)
				{
					num3 = 0;
					num2++;
				}
			}
			m_tmpSpawnPoints.Clear();
			return count == num;
		}

		private int SpawnNewBotInternal(MyAgentDefinition agentDefinition, Vector3D? spawnPosition = null, bool createdByPlayer = false)
		{
			int lastBotId;
			using (m_lock.AcquireExclusiveUsing())
			{
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					if (onlinePlayer.Id.SteamId == Sync.MyId && onlinePlayer.Id.SerialId > m_lastBotId)
					{
						m_lastBotId = onlinePlayer.Id.SerialId;
					}
				}
				m_lastBotId++;
				lastBotId = m_lastBotId;
			}
			m_processQueue.Enqueue(new AgentSpawnData(agentDefinition, lastBotId, spawnPosition, createdByPlayer));
			return lastBotId;
		}

		public int SpawnNewBot(MyAgentDefinition agentDefinition, Vector3D? spawnPosition)
		{
			return SpawnNewBotInternal(agentDefinition, spawnPosition, createdByPlayer: true);
		}

		public bool CanSpawnMoreBots(MyPlayer.PlayerId pid)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			if (MyFakes.DEVELOPMENT_PRESET)
			{
				return true;
			}
			if (Sync.MyId == pid.SteamId)
			{
				AgentSpawnData value = default(AgentSpawnData);
				if (m_agentsToSpawn.TryGetValue(pid.SerialId, out value))
				{
					if (value.CreatedByPlayer)
					{
						return Bots.GetCreatedBotCount() < BotFactory.MaximumBotPerPlayer;
					}
					return Bots.GetGeneratedBotCount() < BotFactory.MaximumUncontrolledBotCount;
				}
				return false;
			}
			int num = 0;
			ulong steamId = pid.SteamId;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SteamId == steamId && onlinePlayer.Id.SerialId != 0)
				{
					num++;
				}
			}
			return num < BotFactory.MaximumBotPerPlayer;
		}

		public int GetAvailableUncontrolledBotsCount()
		{
			return BotFactory.MaximumUncontrolledBotCount - Bots.GetGeneratedBotCount();
		}

		public int GetBotCount(string behaviorType)
		{
			return m_botCollection.GetCurrentBotsCount(behaviorType);
		}

		public void CleanUnusedIdentities()
		{
			List<MyPlayer.PlayerId> list = new List<MyPlayer.PlayerId>();
			foreach (MyPlayer.PlayerId allPlayer in Sync.Players.GetAllPlayers())
			{
				list.Add(allPlayer);
			}
			foreach (MyPlayer.PlayerId item in list)
			{
				if (item.SteamId == Sync.MyId && item.SerialId != 0 && Sync.Players.GetPlayerById(item) == null)
				{
					long num = Sync.Players.TryGetIdentityId(item.SteamId, item.SerialId);
					if (num != 0L)
					{
						Sync.Players.RemoveIdentity(num, item);
					}
				}
			}
		}

		private void PlayerCreated(MyPlayer.PlayerId playerId)
		{
			if (Sync.Players.GetPlayerById(playerId) != null && !Sync.Players.GetPlayerById(playerId).IsRealPlayer)
			{
				CreateBot(playerId.SerialId);
			}
		}

		private void LocalPlayerLoaded(int playerNumber)
		{
			if (playerNumber != 0 && !m_loadedLocalPlayers.Contains(playerNumber))
			{
				m_loadedLocalPlayers.Add(playerNumber);
			}
		}

		private void Players_NewPlayerRequestFailed(int serialId)
		{
			if (serialId != 0 && m_agentsToSpawn.ContainsKey(serialId))
			{
				AgentSpawnData agentSpawnData = m_agentsToSpawn[serialId];
				m_agentsToSpawn.Remove(serialId);
				if (agentSpawnData.CreatedByPlayer)
				{
					MyHud.Notifications.Add(m_maxBotNotification);
				}
			}
		}

		private void Players_PlayerRequesting(PlayerRequestArgs args)
		{
			if (args.PlayerId.SerialId != 0)
			{
				if (!CanSpawnMoreBots(args.PlayerId))
				{
					args.Cancel = true;
				}
				else
				{
					Bots.TotalBotCount++;
				}
			}
		}

		private void Players_PlayerRemoved(MyPlayer.PlayerId pid)
		{
			if (Sync.IsServer && pid.SerialId != 0)
			{
				Bots.TotalBotCount--;
			}
		}

		private void CreateBot(int playerNumber)
		{
			CreateBot(playerNumber, null);
		}

		private void CreateBot(int playerNumber, MyObjectBuilder_Bot botBuilder)
		{
			if (BotFactory == null)
			{
				return;
			}
			MyPlayer player = Sync.Clients.LocalClient.GetPlayer(playerNumber);
			if (player == null)
			{
				return;
			}
			bool flag = m_agentsToSpawn.ContainsKey(playerNumber);
			bool load = botBuilder != null;
			bool flag2 = false;
			MyBotDefinition botDefinition = null;
			AgentSpawnData agentSpawnData = default(AgentSpawnData);
			if (flag)
			{
				agentSpawnData = m_agentsToSpawn[playerNumber];
				flag2 = agentSpawnData.CreatedByPlayer;
				botDefinition = agentSpawnData.AgentDefinition;
				m_agentsToSpawn.Remove(playerNumber);
			}
			else
			{
				if (botBuilder == null || botBuilder.BotDefId.TypeId.IsNull)
				{
					MyPlayer player2 = null;
					if (Sync.Players.TryGetPlayerById(new MyPlayer.PlayerId(Sync.MyId, playerNumber), out player2))
					{
						Sync.Players.RemovePlayer(player2);
					}
					return;
				}
				MyDefinitionManager.Static.TryGetBotDefinition(botBuilder.BotDefId, out botDefinition);
				if (botDefinition == null)
				{
					return;
				}
			}
			if (((player.Character == null || !player.Character.IsDead) && BotFactory.CanCreateBotOfType(botDefinition.BehaviorType, load)) || flag2)
			{
				IMyBot myBot = null;
				myBot = ((!flag) ? BotFactory.CreateBot(player, botBuilder, botDefinition) : BotFactory.CreateBot(player, botBuilder, agentSpawnData.AgentDefinition));
				if (myBot == null)
				{
					MyLog.Default.WriteLine(string.Concat("Could not create a bot for player ", player, "!"));
					return;
				}
				m_botCollection.AddBot(playerNumber, myBot);
				if (flag && myBot is IMyEntityBot)
				{
					(myBot as IMyEntityBot).Spawn(agentSpawnData.SpawnPosition, flag2);
				}
				if (this.BotCreatedEvent != null)
				{
					this.BotCreatedEvent(playerNumber, myBot.BotDefinition);
				}
			}
			else
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, playerNumber));
				Sync.Players.RemovePlayer(playerById);
			}
		}

		public void DespawnBotsOfType(string botType)
		{
			foreach (KeyValuePair<int, IMyBot> allBot in m_botCollection.GetAllBots())
			{
				if (allBot.Value.BotDefinition.BehaviorType == botType)
				{
					Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, allBot.Key));
					RemoveBot(allBot.Key, removeCharacter: true);
				}
			}
			PerformBotRemovals();
		}

		private void PerformBotRemovals()
		{
			BotRemovalRequest instance;
			while (m_removeQueue.TryDequeue(out instance))
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, instance.SerialId));
				if (playerById != null)
				{
					Sync.Players.RemovePlayer(playerById, instance.RemoveCharacter);
				}
			}
		}

		public void RemoveBot(int playerNumber, bool removeCharacter = false)
		{
			BotRemovalRequest instance = default(BotRemovalRequest);
			instance.SerialId = playerNumber;
			instance.RemoveCharacter = removeCharacter;
			m_removeQueue.Enqueue(instance);
		}

		private void LocalPlayerRemoved(int playerNumber)
		{
			if (playerNumber != 0)
			{
				m_botCollection.TryRemoveBot(playerNumber);
			}
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.PRIMARY_TOOL_ACTION))
			{
				if (MySession.Static.ControlledEntity != null && BotToSpawn != null)
				{
					TrySpawnBot();
				}
				if (MySession.Static.ControlledEntity != null && CommandDefinition != null)
				{
					UseCommand();
				}
			}
		}

		public void TrySpawnBot(MyAgentDefinition agentDefinition)
		{
			BotToSpawn = agentDefinition;
			TrySpawnBot();
		}

		private void CurrentToolbar_SelectedSlotChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
		{
			if (!(toolbar.SelectedItem is MyToolbarItemBot))
			{
				BotToSpawn = null;
			}
			if (!(toolbar.SelectedItem is MyToolbarItemAiCommand))
			{
				CommandDefinition = null;
			}
		}

		private void CurrentToolbar_SlotActivated(MyToolbar toolbar, MyToolbar.SlotArgs args, bool userActivated)
		{
			if (!(toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemBot))
			{
				BotToSpawn = null;
			}
			if (!(toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemAiCommand))
			{
				CommandDefinition = null;
			}
		}

		private void CurrentToolbar_Unselected(MyToolbar toolbar)
		{
			BotToSpawn = null;
			CommandDefinition = null;
		}

		private void TrySpawnBot()
		{
			Vector3D position;
			if (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity)
			{
				MatrixD headMatrix = MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true);
				position = headMatrix.Translation;
				_ = headMatrix.Forward;
			}
			else
			{
				position = MySector.MainCamera.Position;
				_ = MySector.MainCamera.WorldMatrix.Forward;
			}
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			LineD lineD = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 1000f);
			MyPhysics.CastRay(lineD.From, lineD.To, list, 15);
			if (list.Count == 0)
			{
				Static.SpawnNewBot(BotToSpawn, position);
				return;
			}
			MyPhysics.HitInfo? hitInfo = null;
			foreach (MyPhysics.HitInfo item in list)
			{
				IMyEntity hitEntity = item.HkHitInfo.GetHitEntity();
				if (hitEntity is MyCubeGrid)
				{
					hitInfo = item;
					break;
				}
				if (hitEntity is MyVoxelBase)
				{
					hitInfo = item;
					break;
				}
				if (hitEntity is MyVoxelPhysics)
				{
					hitInfo = item;
					break;
				}
			}
			Vector3D position2 = (!hitInfo.HasValue) ? MySector.MainCamera.Position : hitInfo.Value.Position;
			Static.SpawnNewBot(BotToSpawn, position2);
		}

		private void UseCommand()
		{
			MyAiCommandBehavior myAiCommandBehavior = new MyAiCommandBehavior();
			myAiCommandBehavior.InitCommand(CommandDefinition);
			myAiCommandBehavior.ActivateCommand();
		}

		public static int GenerateBotId(int lastSpawnedBot)
		{
			int num = lastSpawnedBot;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SteamId == Sync.MyId)
				{
					num = Math.Max(num, onlinePlayer.Id.SerialId);
				}
			}
			return num + 1;
		}

		public static int GenerateBotId()
		{
			int lastBotId = Static.m_lastBotId;
			Static.m_lastBotId = GenerateBotId(lastBotId);
			return Static.m_lastBotId;
		}

		public void DebugDrawBots()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				m_botCollection.DebugDrawBots();
			}
		}

		public void DebugSelectNextBot()
		{
			m_botCollection.DebugSelectNextBot();
		}

		public void DebugSelectPreviousBot()
		{
			m_botCollection.DebugSelectPreviousBot();
		}

		public void DebugRemoveFirstBot()
		{
			if (m_botCollection.HasBot)
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, m_botCollection.GetHandleToFirstBot()));
				Sync.Players.RemovePlayer(playerById);
			}
		}

		private void OnUploadNewTree(ref MyMessage msg)
		{
			if (m_behaviorTreeCollection != null)
			{
				MyBehaviorTree outBehaviorTree = null;
				MyBehaviorDefinition definition = null;
				if (MyBehaviorTreeCollection.LoadUploadedBehaviorTree(out definition) && m_behaviorTreeCollection.HasBehavior(definition.Id.SubtypeId))
				{
					m_botCollection.ResetBots(definition.Id.SubtypeName);
					m_behaviorTreeCollection.RebuildBehaviorTree(definition, out outBehaviorTree);
					m_botCollection.CheckCompatibilityWithBots(outBehaviorTree);
				}
				IntPtr windowHandle = IntPtr.Zero;
				if (m_behaviorTreeCollection.TryGetValidToolWindow(out windowHandle))
				{
					MyVRage.Platform.PostMessage(windowHandle, 1028u, IntPtr.Zero, IntPtr.Zero);
				}
			}
		}

		private void OnBreakDebugging(ref MyMessage msg)
		{
			if (m_behaviorTreeCollection != null)
			{
				m_behaviorTreeCollection.DebugBreakDebugging = true;
			}
		}

		private void OnResumeDebugging(ref MyMessage msg)
		{
			if (m_behaviorTreeCollection != null)
			{
				m_behaviorTreeCollection.DebugBreakDebugging = false;
			}
		}
	}
}
