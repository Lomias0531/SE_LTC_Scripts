using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Common;
using VRage.Game.ModAPI;
using VRage.Game.Voxels;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Game.World
{
	public class MyWorldGenerator
	{
		public struct Args
		{
			public MyScenarioDefinition Scenario;

			public int AsteroidAmount;
		}

		public class OperationTypeAttribute : MyFactoryTagAttribute
		{
			public OperationTypeAttribute(Type objectBuilderType)
				: base(objectBuilderType)
			{
			}
		}

		public static class OperationFactory
		{
			private static MyObjectFactory<OperationTypeAttribute, MyWorldGeneratorOperationBase> m_objectFactory;

			static OperationFactory()
			{
				m_objectFactory = new MyObjectFactory<OperationTypeAttribute, MyWorldGeneratorOperationBase>();
				m_objectFactory.RegisterFromCreatedObjectAssembly();
				m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
				m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
				m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
			}

			public static MyWorldGeneratorOperationBase CreateInstance(MyObjectBuilder_WorldGeneratorOperation builder)
			{
				MyWorldGeneratorOperationBase myWorldGeneratorOperationBase = m_objectFactory.CreateInstance(builder.TypeId);
				myWorldGeneratorOperationBase.Init(builder);
				return myWorldGeneratorOperationBase;
			}

			public static MyObjectBuilder_WorldGeneratorOperation CreateObjectBuilder(MyWorldGeneratorOperationBase instance)
			{
				return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_WorldGeneratorOperation>(instance);
			}
		}

		[OperationType(typeof(MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab))]
		public class OperationAddShipPrefab : MyWorldGeneratorOperationBase
		{
			public string PrefabFile;

			public bool UseFirstGridOrigin;

			public MyPositionAndOrientation Transform = MyPositionAndOrientation.Default;

			public float RandomRadius;

			public override void Apply()
			{
				MyFaction myFaction = null;
				if (FactionTag != null)
				{
					myFaction = MySession.Static.Factions.TryGetOrCreateFactionByTag(FactionTag);
				}
				long ownerId = myFaction?.FounderId ?? 0;
				if (RandomRadius == 0f)
				{
					MyPrefabManager.Static.AddShipPrefab(PrefabFile, Transform.GetMatrix(), ownerId, UseFirstGridOrigin);
				}
				else
				{
					MyPrefabManager.Static.AddShipPrefabRandomPosition(PrefabFile, Transform.Position, RandomRadius, ownerId);
				}
			}

			public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
			{
				base.Init(builder);
				MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab myObjectBuilder_WorldGeneratorOperation_AddShipPrefab = builder as MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab;
				PrefabFile = myObjectBuilder_WorldGeneratorOperation_AddShipPrefab.PrefabFile;
				UseFirstGridOrigin = myObjectBuilder_WorldGeneratorOperation_AddShipPrefab.UseFirstGridOrigin;
				Transform = myObjectBuilder_WorldGeneratorOperation_AddShipPrefab.Transform;
				RandomRadius = myObjectBuilder_WorldGeneratorOperation_AddShipPrefab.RandomRadius;
			}

			public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
			{
				MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab obj = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab;
				obj.PrefabFile = PrefabFile;
				obj.Transform = Transform;
				obj.RandomRadius = RandomRadius;
				return obj;
			}
		}

		[OperationType(typeof(MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab))]
		public class OperationAddAsteroidPrefab : MyWorldGeneratorOperationBase
		{
			public string Name;

			public string PrefabName;

			public Vector3 Position;

			public override void Apply()
			{
				AddAsteroidPrefab(PrefabName, Position, Name);
			}

			public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
			{
				base.Init(builder);
				MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab myObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab = builder as MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab;
				Name = myObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab.Name;
				PrefabName = myObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab.PrefabFile;
				Position = myObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab.Position;
			}

			public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
			{
				MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab obj = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab;
				obj.Name = Name;
				obj.PrefabFile = PrefabName;
				obj.Position = Position;
				return obj;
			}
		}

		[OperationType(typeof(MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab))]
		public class OperationAddObjectsPrefab : MyWorldGeneratorOperationBase
		{
			public string PrefabFile;

			public override void Apply()
			{
				AddObjectsPrefab(PrefabFile);
			}

			public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
			{
				base.Init(builder);
				MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab myObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab = builder as MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab;
				PrefabFile = myObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab.PrefabFile;
			}

			public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
			{
				MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab obj = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab;
				obj.PrefabFile = PrefabFile;
				return obj;
			}
		}

		[OperationType(typeof(MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab))]
		public class OperationSetupBasePrefab : MyWorldGeneratorOperationBase
		{
			public string PrefabFile;

			public Vector3 Offset;

			public string AsteroidName;

			public string BeaconName;

			public override void Apply()
			{
				MyFaction myFaction = null;
				if (FactionTag != null)
				{
					myFaction = MySession.Static.Factions.TryGetOrCreateFactionByTag(FactionTag);
				}
				long ownerId = myFaction?.FounderId ?? 0;
				SetupBase(PrefabFile, Offset, AsteroidName, BeaconName, ownerId);
			}

			public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
			{
				base.Init(builder);
				MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab myObjectBuilder_WorldGeneratorOperation_SetupBasePrefab = builder as MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab;
				PrefabFile = myObjectBuilder_WorldGeneratorOperation_SetupBasePrefab.PrefabFile;
				Offset = myObjectBuilder_WorldGeneratorOperation_SetupBasePrefab.Offset;
				AsteroidName = myObjectBuilder_WorldGeneratorOperation_SetupBasePrefab.AsteroidName;
				BeaconName = myObjectBuilder_WorldGeneratorOperation_SetupBasePrefab.BeaconName;
			}

			public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
			{
				MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab obj = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab;
				obj.PrefabFile = PrefabFile;
				obj.Offset = Offset;
				obj.AsteroidName = AsteroidName;
				obj.BeaconName = BeaconName;
				return obj;
			}
		}

		[OperationType(typeof(MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab))]
		public class OperationAddPlanetPrefab : MyWorldGeneratorOperationBase
		{
			public string PrefabName;

			public string DefinitionName;

			public Vector3D Position;

			public bool AddGPS;

			public override void Apply()
			{
				AddPlanetPrefab(PrefabName, DefinitionName, Position, AddGPS, fadeIn: true);
			}

			public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
			{
				base.Init(builder);
				MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab myObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab = builder as MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab;
				DefinitionName = myObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab.DefinitionName;
				PrefabName = myObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab.PrefabName;
				Position = myObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab.Position;
				AddGPS = myObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab.AddGPS;
			}

			public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
			{
				MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab obj = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab;
				obj.DefinitionName = DefinitionName;
				obj.PrefabName = PrefabName;
				obj.Position = Position;
				obj.AddGPS = AddGPS;
				return obj;
			}
		}

		[OperationType(typeof(MyObjectBuilder_WorldGeneratorOperation_CreatePlanet))]
		public class OperationCreatePlanet : MyWorldGeneratorOperationBase
		{
			public string DefinitionName;

			public bool AddGPS;

			public Vector3D PositionMinCorner;

			public Vector3D PositionCenter;

			public float Diameter;

			public override void Apply()
			{
				MyPlanetGeneratorDefinition generatorDef = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(DefinitionName));
				if (generatorDef == null)
				{
					string msg = $"Definition for planet {DefinitionName} could not be found. Skipping.";
					MyLog.Default.WriteLine(msg);
					return;
				}
				Vector3D positionMinCorner = PositionMinCorner;
				if (PositionCenter.IsValid())
				{
					positionMinCorner = PositionCenter;
					Vector3I value = MyVoxelCoordSystems.FindBestOctreeSize(Diameter * (1f + generatorDef.HillParams.Max));
					positionMinCorner -= (Vector3D)value / 2.0;
				}
				int num = MyRandom.Instance.Next();
				CreatePlanet(DefinitionName + "-" + num + "d" + Diameter, generatorDef.FolderName, ref positionMinCorner, num, Diameter, MyRandom.Instance.NextLong(), ref generatorDef, AddGPS);
			}

			public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
			{
				base.Init(builder);
				MyObjectBuilder_WorldGeneratorOperation_CreatePlanet myObjectBuilder_WorldGeneratorOperation_CreatePlanet = builder as MyObjectBuilder_WorldGeneratorOperation_CreatePlanet;
				DefinitionName = myObjectBuilder_WorldGeneratorOperation_CreatePlanet.DefinitionName;
				DefinitionName = myObjectBuilder_WorldGeneratorOperation_CreatePlanet.DefinitionName;
				AddGPS = myObjectBuilder_WorldGeneratorOperation_CreatePlanet.AddGPS;
				Diameter = myObjectBuilder_WorldGeneratorOperation_CreatePlanet.Diameter;
				PositionMinCorner = myObjectBuilder_WorldGeneratorOperation_CreatePlanet.PositionMinCorner;
				PositionCenter = myObjectBuilder_WorldGeneratorOperation_CreatePlanet.PositionCenter;
			}

			public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
			{
				MyObjectBuilder_WorldGeneratorOperation_CreatePlanet obj = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_CreatePlanet;
				obj.DefinitionName = DefinitionName;
				obj.DefinitionName = DefinitionName;
				obj.AddGPS = AddGPS;
				obj.Diameter = Diameter;
				obj.PositionMinCorner = PositionMinCorner;
				obj.PositionCenter = PositionCenter;
				return obj;
			}
		}

		public class StartingStateTypeAttribute : MyFactoryTagAttribute
		{
			public StartingStateTypeAttribute(Type objectBuilderType)
				: base(objectBuilderType)
			{
			}
		}

		public static class StartingStateFactory
		{
			private static MyObjectFactory<StartingStateTypeAttribute, MyWorldGeneratorStartingStateBase> m_objectFactory;

			static StartingStateFactory()
			{
				m_objectFactory = new MyObjectFactory<StartingStateTypeAttribute, MyWorldGeneratorStartingStateBase>();
				m_objectFactory.RegisterFromCreatedObjectAssembly();
				m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
				m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
				m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
			}

			public static MyWorldGeneratorStartingStateBase CreateInstance(MyObjectBuilder_WorldGeneratorPlayerStartingState builder)
			{
				MyWorldGeneratorStartingStateBase myWorldGeneratorStartingStateBase = m_objectFactory.CreateInstance(builder.TypeId);
				myWorldGeneratorStartingStateBase?.Init(builder);
				return myWorldGeneratorStartingStateBase;
			}

			public static MyObjectBuilder_WorldGeneratorPlayerStartingState CreateObjectBuilder(MyWorldGeneratorStartingStateBase instance)
			{
				return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_WorldGeneratorPlayerStartingState>(instance);
			}
		}

		[StartingStateType(typeof(MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform))]
		public class MyTransformState : MyWorldGeneratorStartingStateBase
		{
			private class Sandbox_Game_World_MyWorldGenerator_003C_003EMyTransformState_003C_003EActor : IActivator, IActivator<MyTransformState>
			{
				private sealed override object CreateInstance()
				{
					return new MyTransformState();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyTransformState CreateInstance()
				{
					return new MyTransformState();
				}

				MyTransformState IActivator<MyTransformState>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public MyPositionAndOrientation? Transform;

			public bool JetpackEnabled;

			public bool DampenersEnabled;

			public override void SetupCharacter(Args generatorArgs)
			{
				if (MySession.Static.LocalHumanPlayer != null)
				{
					MyObjectBuilder_Character myObjectBuilder_Character = MyCharacter.Random();
					if (Transform.HasValue && MyPerGameSettings.CharacterStartsOnVoxel)
					{
						MyPositionAndOrientation value = Transform.Value;
						value.Position = FixPositionToVoxel(value.Position);
						myObjectBuilder_Character.PositionAndOrientation = value;
					}
					else
					{
						myObjectBuilder_Character.PositionAndOrientation = Transform;
					}
					myObjectBuilder_Character.JetpackEnabled = JetpackEnabled;
					myObjectBuilder_Character.DampenersEnabled = DampenersEnabled;
					if (myObjectBuilder_Character.Inventory == null)
					{
						myObjectBuilder_Character.Inventory = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Inventory>();
					}
					FillInventoryWithDefaults(myObjectBuilder_Character.Inventory, generatorArgs.Scenario);
					MyCharacter myCharacter = new MyCharacter();
					myCharacter.Name = "Player";
					myCharacter.Init(myObjectBuilder_Character);
					MyEntities.RaiseEntityCreated(myCharacter);
					MyEntities.Add(myCharacter);
					CreateAndSetPlayerFaction();
					MySession.Static.LocalHumanPlayer.SpawnIntoCharacter(myCharacter);
				}
			}

			public override void Init(MyObjectBuilder_WorldGeneratorPlayerStartingState builder)
			{
				base.Init(builder);
				MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform myObjectBuilder_WorldGeneratorPlayerStartingState_Transform = builder as MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform;
				Transform = myObjectBuilder_WorldGeneratorPlayerStartingState_Transform.Transform;
				JetpackEnabled = myObjectBuilder_WorldGeneratorPlayerStartingState_Transform.JetpackEnabled;
				DampenersEnabled = myObjectBuilder_WorldGeneratorPlayerStartingState_Transform.DampenersEnabled;
			}

			public override MyObjectBuilder_WorldGeneratorPlayerStartingState GetObjectBuilder()
			{
				MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform obj = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform;
				obj.Transform = Transform;
				obj.JetpackEnabled = JetpackEnabled;
				obj.DampenersEnabled = DampenersEnabled;
				return obj;
			}

			public override Vector3D? GetStartingLocation()
			{
				if (Transform.HasValue && MyPerGameSettings.CharacterStartsOnVoxel)
				{
					return FixPositionToVoxel(Transform.Value.Position);
				}
				return null;
			}
		}

		private static List<MyCubeGrid> m_tmpSpawnedGridList;

		public static event ActionRef<Args> OnAfterGenerate;

		static MyWorldGenerator()
		{
			m_tmpSpawnedGridList = new List<MyCubeGrid>();
			if (!MyFakes.TEST_PREFABS_FOR_INCONSISTENCIES)
			{
				return;
			}
			string[] files = Directory.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Data", "Prefabs"));
			foreach (string text in files)
			{
				if (!(Path.GetExtension(text) != ".sbc"))
				{
					MyObjectBuilder_CubeGrid objectBuilder = null;
					MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.ContentPath, text), out objectBuilder);
					if (objectBuilder != null)
					{
						using (List<MyObjectBuilder_CubeBlock>.Enumerator enumerator = objectBuilder.CubeBlocks.GetEnumerator())
						{
							while (enumerator.MoveNext() && enumerator.Current.IntegrityPercent != 0f)
							{
							}
						}
					}
				}
			}
			files = Directory.GetDirectories(Path.Combine(MyFileSystem.ContentPath, "Worlds"));
			for (int i = 0; i < files.Length; i++)
			{
				string[] files2 = Directory.GetFiles(files[i]);
				foreach (string text2 in files2)
				{
					if (!(Path.GetExtension(text2) != ".sbs"))
					{
						MyObjectBuilder_Sector objectBuilder2 = null;
						MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.ContentPath, text2), out objectBuilder2);
						foreach (MyObjectBuilder_EntityBase sectorObject in objectBuilder2.SectorObjects)
						{
							if (sectorObject.TypeId == typeof(MyObjectBuilder_CubeGrid))
							{
								using (List<MyObjectBuilder_CubeBlock>.Enumerator enumerator = ((MyObjectBuilder_CubeGrid)sectorObject).CubeBlocks.GetEnumerator())
								{
									while (enumerator.MoveNext() && enumerator.Current.IntegrityPercent != 0f)
									{
									}
								}
							}
						}
					}
				}
			}
		}

		public static string GetPrefabTypeName(MyObjectBuilder_EntityBase entity)
		{
			if (entity is MyObjectBuilder_VoxelMap)
			{
				return "Asteroid";
			}
			if (entity is MyObjectBuilder_CubeGrid)
			{
				MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (MyObjectBuilder_CubeGrid)entity;
				if (myObjectBuilder_CubeGrid.IsStatic)
				{
					return "Station";
				}
				if (myObjectBuilder_CubeGrid.GridSizeEnum == MyCubeSize.Large)
				{
					return "LargeShip";
				}
				return "SmallShip";
			}
			if (entity is MyObjectBuilder_Character)
			{
				return "Character";
			}
			return "Unknown";
		}

		public static void GenerateWorld(Args args)
		{
			MySandboxGame.Log.WriteLine("MyWorldGenerator.GenerateWorld - START");
			using (MySandboxGame.Log.IndentUsing())
			{
				RunGeneratorOperations(ref args);
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					SetupPlayer(ref args);
				}
				CallOnAfterGenerate(ref args);
			}
			MySandboxGame.Log.WriteLine("MyWorldGenerator.GenerateWorld - END");
		}

		public static void CallOnAfterGenerate(ref Args args)
		{
			if (MyWorldGenerator.OnAfterGenerate != null)
			{
				MyWorldGenerator.OnAfterGenerate(ref args);
			}
		}

		public static void InitInventoryWithDefaults(MyInventory inventory)
		{
			MyObjectBuilder_Inventory myObjectBuilder_Inventory = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Inventory>();
			FillInventoryWithDefaults(myObjectBuilder_Inventory, MySession.Static.Scenario);
			inventory.Init(myObjectBuilder_Inventory);
		}

		private static void SetupPlayer(ref Args args)
		{
			MyIdentity identity = Sync.Players.CreateNewIdentity(Sync.Clients.LocalClient.DisplayName);
			MyPlayer myPlayer = Sync.Players.CreateNewPlayer(identity, Sync.Clients.LocalClient, Sync.MyName, realPlayer: true);
			MyWorldGeneratorStartingStateBase[] possiblePlayerStarts = args.Scenario.PossiblePlayerStarts;
			if (possiblePlayerStarts == null || possiblePlayerStarts.Length == 0)
			{
				Sync.Players.RespawnComponent.SetupCharacterDefault(myPlayer, args);
			}
			else
			{
				Sync.Players.RespawnComponent.SetupCharacterFromStarts(myPlayer, possiblePlayerStarts, args);
			}
			MyObjectBuilder_Toolbar defaultToolbar = args.Scenario.DefaultToolbar;
			if (defaultToolbar != null)
			{
				MyToolbar myToolbar = new MyToolbar(MyToolbarType.Character);
				myToolbar.Init(defaultToolbar, myPlayer.Character, skipAssert: true);
				MySession.Static.Toolbars.RemovePlayerToolbar(myPlayer.Id);
				MySession.Static.Toolbars.AddPlayerToolbar(myPlayer.Id, myToolbar);
				MyToolbarComponent.InitToolbar(MyToolbarType.Character, defaultToolbar);
				MyToolbarComponent.InitCharacterToolbar(defaultToolbar);
			}
		}

		public static void FillInventoryWithDefaults(MyObjectBuilder_Inventory inventory, MyScenarioDefinition scenario)
		{
			if (inventory.Items == null)
			{
				inventory.Items = new List<MyObjectBuilder_InventoryItem>();
			}
			else
			{
				inventory.Items.Clear();
			}
			if (scenario == null || !MySession.Static.Settings.SpawnWithTools)
			{
				return;
			}
			MyStringId[] array = (!MySession.Static.CreativeMode) ? scenario.SurvivalModeWeapons : scenario.CreativeModeWeapons;
			uint nextItemId = 0u;
			if (array != null)
			{
				MyStringId[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					MyStringId myStringId = array2[i];
					MyObjectBuilder_InventoryItem myObjectBuilder_InventoryItem = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
					myObjectBuilder_InventoryItem.Amount = 1;
					myObjectBuilder_InventoryItem.PhysicalContent = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(myStringId.ToString());
					myObjectBuilder_InventoryItem.ItemId = nextItemId++;
					inventory.Items.Add(myObjectBuilder_InventoryItem);
				}
				inventory.nextItemId = nextItemId;
			}
			MyScenarioDefinition.StartingItem[] array3 = (!MySession.Static.CreativeMode) ? scenario.SurvivalModeComponents : scenario.CreativeModeComponents;
			if (array3 != null)
			{
				MyScenarioDefinition.StartingItem[] array4 = array3;
				for (int i = 0; i < array4.Length; i++)
				{
					MyScenarioDefinition.StartingItem startingItem = array4[i];
					MyObjectBuilder_InventoryItem myObjectBuilder_InventoryItem2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
					myObjectBuilder_InventoryItem2.Amount = startingItem.amount;
					MyStringId itemName = startingItem.itemName;
					myObjectBuilder_InventoryItem2.PhysicalContent = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Component>(itemName.ToString());
					myObjectBuilder_InventoryItem2.ItemId = nextItemId++;
					inventory.Items.Add(myObjectBuilder_InventoryItem2);
				}
				inventory.nextItemId = nextItemId;
			}
			MyScenarioDefinition.StartingPhysicalItem[] array5 = (!MySession.Static.CreativeMode) ? scenario.SurvivalModePhysicalItems : scenario.CreativeModePhysicalItems;
			if (array5 != null)
			{
				MyScenarioDefinition.StartingPhysicalItem[] array6 = array5;
				for (int i = 0; i < array6.Length; i++)
				{
					MyScenarioDefinition.StartingPhysicalItem startingPhysicalItem = array6[i];
					MyObjectBuilder_InventoryItem myObjectBuilder_InventoryItem3 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
					myObjectBuilder_InventoryItem3.Amount = startingPhysicalItem.amount;
					MyStringId itemName = startingPhysicalItem.itemType;
					if (itemName.ToString().Equals("Ore"))
					{
						itemName = startingPhysicalItem.itemName;
						myObjectBuilder_InventoryItem3.PhysicalContent = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(itemName.ToString());
					}
					else
					{
						itemName = startingPhysicalItem.itemType;
						if (itemName.ToString().Equals("Ingot"))
						{
							itemName = startingPhysicalItem.itemName;
							myObjectBuilder_InventoryItem3.PhysicalContent = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ingot>(itemName.ToString());
						}
						else
						{
							itemName = startingPhysicalItem.itemType;
							if (itemName.ToString().Equals("OxygenBottle"))
							{
								myObjectBuilder_InventoryItem3.Amount = 1;
								itemName = startingPhysicalItem.itemName;
								myObjectBuilder_InventoryItem3.PhysicalContent = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_OxygenContainerObject>(itemName.ToString());
								(myObjectBuilder_InventoryItem3.PhysicalContent as MyObjectBuilder_GasContainerObject).GasLevel = (float)startingPhysicalItem.amount;
							}
							else
							{
								itemName = startingPhysicalItem.itemType;
								if (itemName.ToString().Equals("GasBottle"))
								{
									myObjectBuilder_InventoryItem3.Amount = 1;
									itemName = startingPhysicalItem.itemName;
									myObjectBuilder_InventoryItem3.PhysicalContent = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GasContainerObject>(itemName.ToString());
									(myObjectBuilder_InventoryItem3.PhysicalContent as MyObjectBuilder_GasContainerObject).GasLevel = (float)startingPhysicalItem.amount;
								}
							}
						}
					}
					myObjectBuilder_InventoryItem3.ItemId = nextItemId++;
					inventory.Items.Add(myObjectBuilder_InventoryItem3);
				}
				inventory.nextItemId = nextItemId;
			}
			array3 = ((!MySession.Static.CreativeMode) ? scenario.SurvivalModeAmmoItems : scenario.CreativeModeAmmoItems);
			if (array3 != null)
			{
				MyScenarioDefinition.StartingItem[] array4 = array3;
				for (int i = 0; i < array4.Length; i++)
				{
					MyScenarioDefinition.StartingItem startingItem2 = array4[i];
					MyObjectBuilder_InventoryItem myObjectBuilder_InventoryItem4 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
					myObjectBuilder_InventoryItem4.Amount = startingItem2.amount;
					MyStringId itemName = startingItem2.itemName;
					myObjectBuilder_InventoryItem4.PhysicalContent = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_AmmoMagazine>(itemName.ToString());
					myObjectBuilder_InventoryItem4.ItemId = nextItemId++;
					inventory.Items.Add(myObjectBuilder_InventoryItem4);
				}
				inventory.nextItemId = nextItemId;
			}
			MyObjectBuilder_InventoryItem[] array7 = MySession.Static.CreativeMode ? scenario.CreativeInventoryItems : scenario.SurvivalInventoryItems;
			if (array7 != null)
			{
				MyObjectBuilder_InventoryItem[] array8 = array7;
				for (int i = 0; i < array8.Length; i++)
				{
					MyObjectBuilder_InventoryItem myObjectBuilder_InventoryItem5 = array8[i].Clone() as MyObjectBuilder_InventoryItem;
					myObjectBuilder_InventoryItem5.ItemId = nextItemId++;
					inventory.Items.Add(myObjectBuilder_InventoryItem5);
				}
				inventory.nextItemId = nextItemId;
			}
		}

		private static void RunGeneratorOperations(ref Args args)
		{
			MyWorldGeneratorOperationBase[] worldGeneratorOperations = args.Scenario.WorldGeneratorOperations;
			if (worldGeneratorOperations != null && worldGeneratorOperations.Length != 0)
			{
				MyWorldGeneratorOperationBase[] array = worldGeneratorOperations;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Apply();
				}
			}
		}

		public static MyVoxelMap AddAsteroidPrefab(string prefabName, MatrixD worldMatrix, string name)
		{
			MyStorageBase storage = LoadRandomizedVoxelMapPrefab(GetVoxelPrefabPath(prefabName));
			return AddVoxelMap(name, storage, worldMatrix, 0L);
		}

		public static MyVoxelMap AddAsteroidPrefab(string prefabName, Vector3D position, string name)
		{
			MyStorageBase storage = LoadRandomizedVoxelMapPrefab(GetVoxelPrefabPath(prefabName));
			return AddVoxelMap(name, storage, position, 0L);
		}

		public static MyVoxelMap AddAsteroidPrefabCentered(string prefabName, Vector3D position, MatrixD rotation, string name)
		{
			MyStorageBase myStorageBase = LoadRandomizedVoxelMapPrefab(GetVoxelPrefabPath(prefabName));
			Vector3 value = myStorageBase.Size * 0.5f;
			rotation.Translation = position - value;
			return AddVoxelMap(name, myStorageBase, rotation, 0L);
		}

		public static MyVoxelMap AddAsteroidPrefabCentered(string prefabName, Vector3D position, string name)
		{
			MyStorageBase myStorageBase = LoadRandomizedVoxelMapPrefab(GetVoxelPrefabPath(prefabName));
			Vector3 value = myStorageBase.Size * 0.5f;
			return AddVoxelMap(name, myStorageBase, position - value, 0L);
		}

		public static MyVoxelMap AddVoxelMap(string storageName, MyStorageBase storage, Vector3D positionMinCorner, long entityId = 0L)
		{
			MyVoxelMap myVoxelMap = new MyVoxelMap();
			if (entityId != 0L)
			{
				myVoxelMap.EntityId = entityId;
			}
			myVoxelMap.Init(storageName, storage, positionMinCorner);
			MyEntities.RaiseEntityCreated(myVoxelMap);
			MyEntities.Add(myVoxelMap);
			return myVoxelMap;
		}

		public static MyVoxelMap AddVoxelMap(string storageName, MyStorageBase storage, MatrixD worldMatrix, long entityId = 0L, bool lazyPhysics = false, bool useVoxelOffset = true)
		{
			if (entityId != 0L && MyEntityIdentifier.ExistsById(entityId))
			{
				MyVoxelMap myVoxelMap = MyEntityIdentifier.GetEntityById(entityId) as MyVoxelMap;
				if (myVoxelMap != null && myVoxelMap.StorageName == storageName)
				{
					MyLog.Default.WriteLine($"CRITICAL-VOXEL MAP!!! ---- VoxelMap already loaded. This must not happen ({storageName})", LoggingOptions.VOXEL_MAPS);
				}
				else
				{
					IMyEntity entityById = MyEntityIdentifier.GetEntityById(entityId);
					if (entityById != null)
					{
						MyLog.Default.WriteLine($"CRITICAL-VOXEL MAP!!! ---- VoxelMap entity collision. Entity with id {entityId} is already registered in place of VoxelMap{storageName}. ( entity ({entityById.DisplayName}) ({entityById.GetType()}) ({entityById.ToString()}) )", LoggingOptions.VOXEL_MAPS);
					}
					else
					{
						MyLog.Default.WriteLine($"CRITICAL-VOXEL MAP!!! ---- VoxelMap entity collision. Entity (null) with id {entityId} is already registered in place of VoxelMap{storageName}.", LoggingOptions.VOXEL_MAPS);
					}
				}
				return null;
			}
			MyVoxelMap myVoxelMap2 = new MyVoxelMap();
			if (entityId != 0L)
			{
				myVoxelMap2.EntityId = entityId;
			}
			myVoxelMap2.DelayRigidBodyCreation = lazyPhysics;
			myVoxelMap2.Init(storageName, storage, worldMatrix, useVoxelOffset);
			MyEntities.Add(myVoxelMap2);
			MyEntities.RaiseEntityCreated(myVoxelMap2);
			return myVoxelMap2;
		}

		public static void AddEntity(MyObjectBuilder_EntityBase entityBuilder)
		{
			MyEntities.CreateFromObjectBuilderAndAdd(entityBuilder, fadeIn: false);
		}

		private static void AddObjectsPrefab(string prefabName)
		{
			foreach (MyObjectBuilder_EntityBase item in LoadObjectsPrefab(prefabName))
			{
				MyEntities.CreateFromObjectBuilderAndAdd(item, fadeIn: false);
			}
		}

		private static void SetupBase(string basePrefabName, Vector3 offset, string voxelFilename, string beaconName = null, long ownerId = 0L)
		{
			MyPrefabManager.Static.SpawnPrefab(basePrefabName, new Vector3(-3f, 11f, 15f) + offset, Vector3.Forward, Vector3.Up, default(Vector3), default(Vector3), beaconName, null, SpawningOptions.None, 0L);
			MyPrefabManager.Static.AddShipPrefab("SmallShip_SingleBlock", Matrix.CreateTranslation(new Vector3(-5.20818424f, -0.442984432f, -8.31522751f) + offset), ownerId);
			if (voxelFilename != null)
			{
				MyStorageBase storage = LoadRandomizedVoxelMapPrefab(GetVoxelPrefabPath("VerticalIsland_128x128x128"));
				AddVoxelMap(voxelFilename, storage, new Vector3(-20f, -110f, -60f) + offset, 0L);
			}
		}

		public static MyStorageBase LoadRandomizedVoxelMapPrefab(string prefabFilePath)
		{
			MyStorageBase myStorageBase = MyStorageBase.LoadFromFile(prefabFilePath);
			myStorageBase.DataProvider = MyCompositeShapeProvider.CreateAsteroidShape(MyUtils.GetRandomInt(2147483646) + 1, (float)myStorageBase.Size.AbsMax() * 1f);
			myStorageBase.Reset(MyStorageDataTypeFlags.Material);
			return myStorageBase;
		}

		private static string GetObjectsPrefabPath(string prefabName)
		{
			return Path.Combine("Data", "Prefabs", prefabName + ".sbs");
		}

		public static string GetVoxelPrefabPath(string prefabName)
		{
			if (MyDefinitionManager.Static.TryGetVoxelMapStorageDefinition(prefabName, out MyVoxelMapStorageDefinition definition))
			{
				if (definition.Context.IsBaseGame)
				{
					return Path.Combine(MyFileSystem.ContentPath, definition.StorageFile);
				}
				return definition.StorageFile;
			}
			return Path.Combine(MyFileSystem.ContentPath, "VoxelMaps", prefabName + ".vx2");
		}

		private static List<MyObjectBuilder_EntityBase> LoadObjectsPrefab(string file)
		{
			MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.ContentPath, GetObjectsPrefabPath(file)), out MyObjectBuilder_Sector objectBuilder);
			foreach (MyObjectBuilder_EntityBase sectorObject in objectBuilder.SectorObjects)
			{
				sectorObject.EntityId = 0L;
			}
			return objectBuilder.SectorObjects;
		}

		public static void SetProceduralSettings(int? asteroidAmount, MyObjectBuilder_SessionSettings sessionSettings)
		{
			sessionSettings.ProceduralSeed = MyRandom.Instance.Next();
			switch (asteroidAmount.Value)
			{
			case -4:
				sessionSettings.ProceduralDensity = 0f;
				break;
			case -1:
				sessionSettings.ProceduralDensity = 0.25f;
				break;
			case -2:
				sessionSettings.ProceduralDensity = 0.35f;
				break;
			case -3:
				sessionSettings.ProceduralDensity = 0.5f;
				break;
			default:
				throw new InvalidBranchException();
			}
		}

		public static void AddPlanetPrefab(string planetName, string definitionName, Vector3D position, bool addGPS, bool fadeIn)
		{
			foreach (MyPlanetPrefabDefinition planetsPrefabsDefinition in MyDefinitionManager.Static.GetPlanetsPrefabsDefinitions())
			{
				if (planetsPrefabsDefinition.Id.SubtypeName == planetName)
				{
					MyPlanetGeneratorDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(definitionName));
					MyPlanet myPlanet = new MyPlanet();
					MyObjectBuilder_Planet planetBuilder = planetsPrefabsDefinition.PlanetBuilder;
					string absoluteFilePath = MyFileSystem.ContentPath + "\\VoxelMaps\\" + planetBuilder.StorageName + ".vx2";
					myPlanet.EntityId = planetBuilder.EntityId;
					MyPlanetInitArguments arguments = default(MyPlanetInitArguments);
					arguments.StorageName = planetBuilder.StorageName;
					arguments.Seed = planetBuilder.Seed;
					arguments.Storage = MyStorageBase.LoadFromFile(absoluteFilePath);
					arguments.PositionMinCorner = position;
					arguments.Radius = planetBuilder.Radius;
					arguments.AtmosphereRadius = planetBuilder.AtmosphereRadius;
					arguments.MaxRadius = planetBuilder.MaximumHillRadius;
					arguments.MinRadius = planetBuilder.MinimumSurfaceRadius;
					arguments.HasAtmosphere = definition.HasAtmosphere;
					arguments.AtmosphereWavelengths = planetBuilder.AtmosphereWavelengths;
					arguments.GravityFalloff = definition.GravityFalloffPower;
					arguments.MarkAreaEmpty = true;
					arguments.AtmosphereSettings = (planetBuilder.AtmosphereSettings ?? MyAtmosphereSettings.Defaults());
					arguments.SurfaceGravity = definition.SurfaceGravity;
					arguments.AddGps = addGPS;
					arguments.SpherizeWithDistance = true;
					arguments.Generator = definition;
					arguments.UserCreated = false;
					arguments.InitializeComponents = true;
					arguments.FadeIn = fadeIn;
					myPlanet.Init(arguments);
					MyEntities.Add(myPlanet);
					MyEntities.RaiseEntityCreated(myPlanet);
				}
			}
		}

		public static MyPlanet AddPlanet(string storageName, string planetName, string definitionName, Vector3D positionMinCorner, int seed, float size, bool fadeIn, long entityId = 0L, bool addGPS = false, bool userCreated = false)
		{
			MyPlanetGeneratorDefinition generatorDef = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(definitionName));
			return CreatePlanet(storageName, planetName, ref positionMinCorner, seed, size, entityId, ref generatorDef, addGPS, userCreated, fadeIn);
		}

		private static MyPlanet CreatePlanet(string storageName, string planetName, ref Vector3D positionMinCorner, int seed, float size, long entityId, ref MyPlanetGeneratorDefinition generatorDef, bool addGPS, bool userCreated = false, bool fadeIn = false)
		{
			if (!MyFakes.ENABLE_PLANETS && !MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				return null;
			}
			MyRandom instance = MyRandom.Instance;
			using (MyRandom.Instance.PushSeed(seed))
			{
				MyPlanetStorageProvider myPlanetStorageProvider = new MyPlanetStorageProvider();
				myPlanetStorageProvider.Init(seed, generatorDef, size / 2f);
				VRage.Game.Voxels.IMyStorage storage = new MyOctreeStorage(myPlanetStorageProvider, myPlanetStorageProvider.StorageSize);
				float num = myPlanetStorageProvider.Radius * generatorDef.HillParams.Min;
				float num2 = myPlanetStorageProvider.Radius * generatorDef.HillParams.Max;
				float radius = myPlanetStorageProvider.Radius;
				float maxRadius = radius + num2;
				float minRadius = radius + num;
				float num3 = (generatorDef.AtmosphereSettings.HasValue && generatorDef.AtmosphereSettings.Value.Scale > 1f) ? (1f + generatorDef.AtmosphereSettings.Value.Scale) : 1.75f;
				num3 *= myPlanetStorageProvider.Radius;
				float num4 = instance.NextFloat(generatorDef.HostileAtmosphereColorShift.R.Min, generatorDef.HostileAtmosphereColorShift.R.Max);
				float num5 = instance.NextFloat(generatorDef.HostileAtmosphereColorShift.G.Min, generatorDef.HostileAtmosphereColorShift.G.Max);
				float num6 = instance.NextFloat(generatorDef.HostileAtmosphereColorShift.B.Min, generatorDef.HostileAtmosphereColorShift.B.Max);
				Vector3 atmosphereWavelengths = new Vector3(0.65f + num4, 0.57f + num5, 0.475f + num6);
				atmosphereWavelengths.X = MathHelper.Clamp(atmosphereWavelengths.X, 0.1f, 1f);
				atmosphereWavelengths.Y = MathHelper.Clamp(atmosphereWavelengths.Y, 0.1f, 1f);
				atmosphereWavelengths.Z = MathHelper.Clamp(atmosphereWavelengths.Z, 0.1f, 1f);
				MyPlanet obj = new MyPlanet
				{
					EntityId = entityId
				};
				MyPlanetInitArguments arguments = default(MyPlanetInitArguments);
				arguments.StorageName = storageName;
				arguments.Seed = seed;
				arguments.Storage = storage;
				arguments.PositionMinCorner = positionMinCorner;
				arguments.Radius = myPlanetStorageProvider.Radius;
				arguments.AtmosphereRadius = num3;
				arguments.MaxRadius = maxRadius;
				arguments.MinRadius = minRadius;
				arguments.HasAtmosphere = generatorDef.HasAtmosphere;
				arguments.AtmosphereWavelengths = atmosphereWavelengths;
				arguments.GravityFalloff = generatorDef.GravityFalloffPower;
				arguments.MarkAreaEmpty = true;
				arguments.AtmosphereSettings = (generatorDef.AtmosphereSettings ?? MyAtmosphereSettings.Defaults());
				arguments.SurfaceGravity = generatorDef.SurfaceGravity;
				arguments.AddGps = addGPS;
				arguments.SpherizeWithDistance = true;
				arguments.Generator = generatorDef;
				arguments.UserCreated = userCreated;
				arguments.InitializeComponents = true;
				arguments.FadeIn = fadeIn;
				obj.Init(arguments);
				obj.AsteroidName = planetName;
				MyEntities.RaiseEntityCreated(obj);
				MyEntities.Add(obj);
				return obj;
			}
		}
	}
}
