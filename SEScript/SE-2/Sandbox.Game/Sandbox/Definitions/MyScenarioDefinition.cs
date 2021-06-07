using Sandbox.Game.World;
using System;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ScenarioDefinition), null)]
	public class MyScenarioDefinition : MyDefinitionBase
	{
		public struct StartingItem
		{
			public MyFixedPoint amount;

			public MyStringId itemName;
		}

		public struct StartingPhysicalItem
		{
			public MyFixedPoint amount;

			public MyStringId itemName;

			public MyStringId itemType;
		}

		private class Sandbox_Definitions_MyScenarioDefinition_003C_003EActor : IActivator, IActivator<MyScenarioDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyScenarioDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyScenarioDefinition CreateInstance()
			{
				return new MyScenarioDefinition();
			}

			MyScenarioDefinition IActivator<MyScenarioDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId GameDefinition;

		public MyDefinitionId Environment;

		public BoundingBoxD? WorldBoundaries;

		public MyWorldGeneratorStartingStateBase[] PossiblePlayerStarts;

		public MyWorldGeneratorOperationBase[] WorldGeneratorOperations;

		public bool AsteroidClustersEnabled;

		public float AsteroidClustersOffset;

		public bool CentralClusterEnabled;

		public MyEnvironmentHostilityEnum DefaultEnvironment;

		public MyStringId[] CreativeModeWeapons;

		public MyStringId[] SurvivalModeWeapons;

		public StartingItem[] CreativeModeComponents;

		public StartingItem[] SurvivalModeComponents;

		public StartingPhysicalItem[] CreativeModePhysicalItems;

		public StartingPhysicalItem[] SurvivalModePhysicalItems;

		public StartingItem[] CreativeModeAmmoItems;

		public StartingItem[] SurvivalModeAmmoItems;

		public MyObjectBuilder_InventoryItem[] CreativeInventoryItems;

		public MyObjectBuilder_InventoryItem[] SurvivalInventoryItems;

		public MyObjectBuilder_Toolbar CreativeDefaultToolbar;

		public MyObjectBuilder_Toolbar SurvivalDefaultToolbar;

		public MyStringId MainCharacterModel;

		public DateTime GameDate;

		public Vector3 SunDirection;

		public bool HasPlanets
		{
			get
			{
				if (WorldGeneratorOperations != null)
				{
					return WorldGeneratorOperations.Any((MyWorldGeneratorOperationBase s) => s is MyWorldGenerator.OperationAddPlanetPrefab || s is MyWorldGenerator.OperationCreatePlanet);
				}
				return false;
			}
		}

		public MyObjectBuilder_Toolbar DefaultToolbar
		{
			get
			{
				if (!MySession.Static.CreativeMode)
				{
					return SurvivalDefaultToolbar;
				}
				return CreativeDefaultToolbar;
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ScenarioDefinition myObjectBuilder_ScenarioDefinition = (MyObjectBuilder_ScenarioDefinition)builder;
			GameDefinition = myObjectBuilder_ScenarioDefinition.GameDefinition;
			Environment = myObjectBuilder_ScenarioDefinition.EnvironmentDefinition;
			AsteroidClustersEnabled = myObjectBuilder_ScenarioDefinition.AsteroidClusters.Enabled;
			AsteroidClustersOffset = myObjectBuilder_ScenarioDefinition.AsteroidClusters.Offset;
			CentralClusterEnabled = myObjectBuilder_ScenarioDefinition.AsteroidClusters.CentralCluster;
			DefaultEnvironment = myObjectBuilder_ScenarioDefinition.DefaultEnvironment;
			CreativeDefaultToolbar = myObjectBuilder_ScenarioDefinition.CreativeDefaultToolbar;
			SurvivalDefaultToolbar = myObjectBuilder_ScenarioDefinition.SurvivalDefaultToolbar;
			MainCharacterModel = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.MainCharacterModel);
			GameDate = new DateTime(myObjectBuilder_ScenarioDefinition.GameDate);
			SunDirection = myObjectBuilder_ScenarioDefinition.SunDirection;
			if (myObjectBuilder_ScenarioDefinition.PossibleStartingStates != null && myObjectBuilder_ScenarioDefinition.PossibleStartingStates.Length != 0)
			{
				PossiblePlayerStarts = new MyWorldGeneratorStartingStateBase[myObjectBuilder_ScenarioDefinition.PossibleStartingStates.Length];
				for (int i = 0; i < myObjectBuilder_ScenarioDefinition.PossibleStartingStates.Length; i++)
				{
					PossiblePlayerStarts[i] = MyWorldGenerator.StartingStateFactory.CreateInstance(myObjectBuilder_ScenarioDefinition.PossibleStartingStates[i]);
				}
			}
			if (myObjectBuilder_ScenarioDefinition.WorldGeneratorOperations != null && myObjectBuilder_ScenarioDefinition.WorldGeneratorOperations.Length != 0)
			{
				WorldGeneratorOperations = new MyWorldGeneratorOperationBase[myObjectBuilder_ScenarioDefinition.WorldGeneratorOperations.Length];
				for (int j = 0; j < myObjectBuilder_ScenarioDefinition.WorldGeneratorOperations.Length; j++)
				{
					WorldGeneratorOperations[j] = MyWorldGenerator.OperationFactory.CreateInstance(myObjectBuilder_ScenarioDefinition.WorldGeneratorOperations[j]);
				}
			}
			if (myObjectBuilder_ScenarioDefinition.CreativeModeWeapons != null && myObjectBuilder_ScenarioDefinition.CreativeModeWeapons.Length != 0)
			{
				CreativeModeWeapons = new MyStringId[myObjectBuilder_ScenarioDefinition.CreativeModeWeapons.Length];
				for (int k = 0; k < myObjectBuilder_ScenarioDefinition.CreativeModeWeapons.Length; k++)
				{
					CreativeModeWeapons[k] = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.CreativeModeWeapons[k]);
				}
			}
			if (myObjectBuilder_ScenarioDefinition.SurvivalModeWeapons != null && myObjectBuilder_ScenarioDefinition.SurvivalModeWeapons.Length != 0)
			{
				SurvivalModeWeapons = new MyStringId[myObjectBuilder_ScenarioDefinition.SurvivalModeWeapons.Length];
				for (int l = 0; l < myObjectBuilder_ScenarioDefinition.SurvivalModeWeapons.Length; l++)
				{
					SurvivalModeWeapons[l] = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.SurvivalModeWeapons[l]);
				}
			}
			if (myObjectBuilder_ScenarioDefinition.CreativeModeComponents != null && myObjectBuilder_ScenarioDefinition.CreativeModeComponents.Length != 0)
			{
				CreativeModeComponents = new StartingItem[myObjectBuilder_ScenarioDefinition.CreativeModeComponents.Length];
				for (int m = 0; m < myObjectBuilder_ScenarioDefinition.CreativeModeComponents.Length; m++)
				{
					CreativeModeComponents[m].amount = (MyFixedPoint)myObjectBuilder_ScenarioDefinition.CreativeModeComponents[m].amount;
					CreativeModeComponents[m].itemName = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.CreativeModeComponents[m].itemName);
				}
			}
			if (myObjectBuilder_ScenarioDefinition.SurvivalModeComponents != null && myObjectBuilder_ScenarioDefinition.SurvivalModeComponents.Length != 0)
			{
				SurvivalModeComponents = new StartingItem[myObjectBuilder_ScenarioDefinition.SurvivalModeComponents.Length];
				for (int n = 0; n < myObjectBuilder_ScenarioDefinition.SurvivalModeComponents.Length; n++)
				{
					SurvivalModeComponents[n].amount = (MyFixedPoint)myObjectBuilder_ScenarioDefinition.SurvivalModeComponents[n].amount;
					SurvivalModeComponents[n].itemName = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.SurvivalModeComponents[n].itemName);
				}
			}
			if (myObjectBuilder_ScenarioDefinition.CreativeModePhysicalItems != null && myObjectBuilder_ScenarioDefinition.CreativeModePhysicalItems.Length != 0)
			{
				CreativeModePhysicalItems = new StartingPhysicalItem[myObjectBuilder_ScenarioDefinition.CreativeModePhysicalItems.Length];
				for (int num = 0; num < myObjectBuilder_ScenarioDefinition.CreativeModePhysicalItems.Length; num++)
				{
					CreativeModePhysicalItems[num].amount = (MyFixedPoint)myObjectBuilder_ScenarioDefinition.CreativeModePhysicalItems[num].amount;
					CreativeModePhysicalItems[num].itemName = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.CreativeModePhysicalItems[num].itemName);
					CreativeModePhysicalItems[num].itemType = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.CreativeModePhysicalItems[num].itemType);
				}
			}
			if (myObjectBuilder_ScenarioDefinition.SurvivalModePhysicalItems != null && myObjectBuilder_ScenarioDefinition.SurvivalModePhysicalItems.Length != 0)
			{
				SurvivalModePhysicalItems = new StartingPhysicalItem[myObjectBuilder_ScenarioDefinition.SurvivalModePhysicalItems.Length];
				for (int num2 = 0; num2 < myObjectBuilder_ScenarioDefinition.SurvivalModePhysicalItems.Length; num2++)
				{
					SurvivalModePhysicalItems[num2].amount = (MyFixedPoint)myObjectBuilder_ScenarioDefinition.SurvivalModePhysicalItems[num2].amount;
					SurvivalModePhysicalItems[num2].itemName = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.SurvivalModePhysicalItems[num2].itemName);
					SurvivalModePhysicalItems[num2].itemType = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.SurvivalModePhysicalItems[num2].itemType);
				}
			}
			if (myObjectBuilder_ScenarioDefinition.CreativeModeAmmoItems != null && myObjectBuilder_ScenarioDefinition.CreativeModeAmmoItems.Length != 0)
			{
				CreativeModeAmmoItems = new StartingItem[myObjectBuilder_ScenarioDefinition.CreativeModeAmmoItems.Length];
				for (int num3 = 0; num3 < myObjectBuilder_ScenarioDefinition.CreativeModeAmmoItems.Length; num3++)
				{
					CreativeModeAmmoItems[num3].amount = (MyFixedPoint)myObjectBuilder_ScenarioDefinition.CreativeModeAmmoItems[num3].amount;
					CreativeModeAmmoItems[num3].itemName = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.CreativeModeAmmoItems[num3].itemName);
				}
			}
			if (myObjectBuilder_ScenarioDefinition.SurvivalModeAmmoItems != null && myObjectBuilder_ScenarioDefinition.SurvivalModeAmmoItems.Length != 0)
			{
				SurvivalModeAmmoItems = new StartingItem[myObjectBuilder_ScenarioDefinition.SurvivalModeAmmoItems.Length];
				for (int num4 = 0; num4 < myObjectBuilder_ScenarioDefinition.SurvivalModeAmmoItems.Length; num4++)
				{
					SurvivalModeAmmoItems[num4].amount = (MyFixedPoint)myObjectBuilder_ScenarioDefinition.SurvivalModeAmmoItems[num4].amount;
					SurvivalModeAmmoItems[num4].itemName = MyStringId.GetOrCompute(myObjectBuilder_ScenarioDefinition.SurvivalModeAmmoItems[num4].itemName);
				}
			}
			CreativeInventoryItems = myObjectBuilder_ScenarioDefinition.CreativeInventoryItems;
			SurvivalInventoryItems = myObjectBuilder_ScenarioDefinition.SurvivalInventoryItems;
			WorldBoundaries = myObjectBuilder_ScenarioDefinition.WorldBoundaries;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_ScenarioDefinition myObjectBuilder_ScenarioDefinition = base.GetObjectBuilder() as MyObjectBuilder_ScenarioDefinition;
			myObjectBuilder_ScenarioDefinition.AsteroidClusters.Enabled = AsteroidClustersEnabled;
			myObjectBuilder_ScenarioDefinition.AsteroidClusters.Offset = AsteroidClustersOffset;
			myObjectBuilder_ScenarioDefinition.AsteroidClusters.CentralCluster = CentralClusterEnabled;
			myObjectBuilder_ScenarioDefinition.DefaultEnvironment = DefaultEnvironment;
			myObjectBuilder_ScenarioDefinition.CreativeDefaultToolbar = CreativeDefaultToolbar;
			myObjectBuilder_ScenarioDefinition.SurvivalDefaultToolbar = SurvivalDefaultToolbar;
			myObjectBuilder_ScenarioDefinition.MainCharacterModel = MainCharacterModel.ToString();
			myObjectBuilder_ScenarioDefinition.GameDate = GameDate.Ticks;
			if (PossiblePlayerStarts != null && PossiblePlayerStarts.Length != 0)
			{
				myObjectBuilder_ScenarioDefinition.PossibleStartingStates = new MyObjectBuilder_WorldGeneratorPlayerStartingState[PossiblePlayerStarts.Length];
				for (int i = 0; i < PossiblePlayerStarts.Length; i++)
				{
					myObjectBuilder_ScenarioDefinition.PossibleStartingStates[i] = PossiblePlayerStarts[i].GetObjectBuilder();
				}
			}
			if (WorldGeneratorOperations != null && WorldGeneratorOperations.Length != 0)
			{
				myObjectBuilder_ScenarioDefinition.WorldGeneratorOperations = new MyObjectBuilder_WorldGeneratorOperation[WorldGeneratorOperations.Length];
				for (int j = 0; j < WorldGeneratorOperations.Length; j++)
				{
					myObjectBuilder_ScenarioDefinition.WorldGeneratorOperations[j] = WorldGeneratorOperations[j].GetObjectBuilder();
				}
			}
			if (CreativeModeWeapons != null && CreativeModeWeapons.Length != 0)
			{
				myObjectBuilder_ScenarioDefinition.CreativeModeWeapons = new string[CreativeModeWeapons.Length];
				for (int k = 0; k < CreativeModeWeapons.Length; k++)
				{
					myObjectBuilder_ScenarioDefinition.CreativeModeWeapons[k] = CreativeModeWeapons[k].ToString();
				}
			}
			if (SurvivalModeWeapons != null && SurvivalModeWeapons.Length != 0)
			{
				myObjectBuilder_ScenarioDefinition.SurvivalModeWeapons = new string[SurvivalModeWeapons.Length];
				for (int l = 0; l < SurvivalModeWeapons.Length; l++)
				{
					myObjectBuilder_ScenarioDefinition.SurvivalModeWeapons[l] = SurvivalModeWeapons[l].ToString();
				}
			}
			return myObjectBuilder_ScenarioDefinition;
		}
	}
}
