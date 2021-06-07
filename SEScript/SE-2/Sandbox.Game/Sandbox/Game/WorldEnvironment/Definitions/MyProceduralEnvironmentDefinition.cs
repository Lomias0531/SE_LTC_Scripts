using Sandbox.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ProceduralWorldEnvironment), typeof(MyProceduralEnvironmentDefinitionPostprocessor))]
	public class MyProceduralEnvironmentDefinition : MyWorldEnvironmentDefinition
	{
		private class Sandbox_Game_WorldEnvironment_Definitions_MyProceduralEnvironmentDefinition_003C_003EActor : IActivator, IActivator<MyProceduralEnvironmentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyProceduralEnvironmentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProceduralEnvironmentDefinition CreateInstance()
			{
				return new MyProceduralEnvironmentDefinition();
			}

			MyProceduralEnvironmentDefinition IActivator<MyProceduralEnvironmentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly int[] ArrayOfZero = new int[1];

		private MyObjectBuilder_ProceduralWorldEnvironment m_ob;

		public Dictionary<string, MyItemTypeDefinition> ItemTypes = new Dictionary<string, MyItemTypeDefinition>();

		public Dictionary<MyBiomeMaterial, List<MyEnvironmentItemMapping>> MaterialEnvironmentMappings;

		public MyProceduralScanningMethod ScanningMethod;

		public override Type SectorType => typeof(MyEnvironmentSector);

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			ScanningMethod = (m_ob = (MyObjectBuilder_ProceduralWorldEnvironment)builder).ScanningMethod;
		}

		public void Prepare()
		{
			if (m_ob.ItemTypes != null)
			{
				MyEnvironmentItemTypeDefinition[] itemTypes = m_ob.ItemTypes;
				foreach (MyEnvironmentItemTypeDefinition myEnvironmentItemTypeDefinition in itemTypes)
				{
					try
					{
						MyItemTypeDefinition value = new MyItemTypeDefinition(myEnvironmentItemTypeDefinition);
						ItemTypes.Add(myEnvironmentItemTypeDefinition.Name, value);
					}
					catch (ArgumentException)
					{
						MyLog.Default.Error("Duplicate environment item definition for item {0}.", myEnvironmentItemTypeDefinition.Name);
					}
					catch (Exception ex2)
					{
						MyLog.Default.Error("Error preparing environment item definition for item {0}:\n {1}", myEnvironmentItemTypeDefinition.Name, ex2.Message);
					}
				}
			}
			MaterialEnvironmentMappings = new Dictionary<MyBiomeMaterial, List<MyEnvironmentItemMapping>>(MyBiomeMaterial.Comparer);
			List<MyRuntimeEnvironmentItemInfo> list = new List<MyRuntimeEnvironmentItemInfo>();
			MyProceduralEnvironmentMapping[] environmentMappings = m_ob.EnvironmentMappings;
			if (environmentMappings != null && environmentMappings.Length != 0)
			{
				MaterialEnvironmentMappings = new Dictionary<MyBiomeMaterial, List<MyEnvironmentItemMapping>>(MyBiomeMaterial.Comparer);
				foreach (MyProceduralEnvironmentMapping myProceduralEnvironmentMapping in environmentMappings)
				{
					MyEnvironmentRule rule = new MyEnvironmentRule
					{
						Height = myProceduralEnvironmentMapping.Height,
						Slope = myProceduralEnvironmentMapping.Slope,
						Latitude = myProceduralEnvironmentMapping.Latitude,
						Longitude = myProceduralEnvironmentMapping.Longitude
					};
					if (myProceduralEnvironmentMapping.Materials == null)
					{
						MyLog.Default.Warning("Mapping in definition {0} does not define any materials, it will not be applied.", Id);
						continue;
					}
					if (myProceduralEnvironmentMapping.Biomes == null)
					{
						myProceduralEnvironmentMapping.Biomes = ArrayOfZero;
					}
					bool flag = false;
					MyRuntimeEnvironmentItemInfo[] array = new MyRuntimeEnvironmentItemInfo[myProceduralEnvironmentMapping.Items.Length];
					for (int k = 0; k < myProceduralEnvironmentMapping.Items.Length; k++)
					{
						if (!ItemTypes.ContainsKey(myProceduralEnvironmentMapping.Items[k].Type))
						{
							MyLog.Default.Error("No definition for item type {0}", myProceduralEnvironmentMapping.Items[k].Type);
							continue;
						}
						array[k] = new MyRuntimeEnvironmentItemInfo(this, myProceduralEnvironmentMapping.Items[k], list.Count);
						list.Add(array[k]);
						flag = true;
					}
					if (!flag)
					{
						continue;
					}
					MyEnvironmentItemMapping item = new MyEnvironmentItemMapping(array, rule, this);
					int[] biomes = myProceduralEnvironmentMapping.Biomes;
					foreach (int num in biomes)
					{
						string[] materials = myProceduralEnvironmentMapping.Materials;
						foreach (string name in materials)
						{
							if (MyDefinitionManager.Static.GetVoxelMaterialDefinition(name) != null)
							{
								MyBiomeMaterial key = new MyBiomeMaterial((byte)num, MyDefinitionManager.Static.GetVoxelMaterialDefinition(name).Index);
								if (!MaterialEnvironmentMappings.TryGetValue(key, out List<MyEnvironmentItemMapping> value2))
								{
									value2 = new List<MyEnvironmentItemMapping>();
									MaterialEnvironmentMappings[key] = value2;
								}
								value2.Add(item);
							}
						}
					}
				}
			}
			Items = list.ToArray();
			m_ob = null;
		}

		public static MyWorldEnvironmentDefinition FromLegacyPlanet(MyObjectBuilder_PlanetGeneratorDefinition pgdef, MyModContext context)
		{
			MyObjectBuilder_ProceduralWorldEnvironment myObjectBuilder_ProceduralWorldEnvironment = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ProceduralWorldEnvironment>(pgdef.Id.SubtypeId);
			myObjectBuilder_ProceduralWorldEnvironment.Id = new SerializableDefinitionId(myObjectBuilder_ProceduralWorldEnvironment.TypeId, myObjectBuilder_ProceduralWorldEnvironment.SubtypeName);
			SerializableDefinitionId value = new SerializableDefinitionId(typeof(MyObjectBuilder_ProceduralEnvironmentModuleDefinition), "Static");
			SerializableDefinitionId value2 = new SerializableDefinitionId(typeof(MyObjectBuilder_ProceduralEnvironmentModuleDefinition), "Memory");
			SerializableDefinitionId serializableDefinitionId = new SerializableDefinitionId(typeof(MyObjectBuilder_EnvironmentModuleProxyDefinition), "Breakable");
			SerializableDefinitionId serializableDefinitionId2 = new SerializableDefinitionId(typeof(MyObjectBuilder_EnvironmentModuleProxyDefinition), "VoxelMap");
			SerializableDefinitionId serializableDefinitionId3 = new SerializableDefinitionId(typeof(MyObjectBuilder_EnvironmentModuleProxyDefinition), "BotSpawner");
			new SerializableDefinitionId(typeof(MyObjectBuilder_EnvironmentModuleProxyDefinition), "EnvironmentalParticles");
			myObjectBuilder_ProceduralWorldEnvironment.ItemTypes = new MyEnvironmentItemTypeDefinition[4]
			{
				new MyEnvironmentItemTypeDefinition
				{
					LodFrom = -1,
					LodTo = 1,
					Name = "Tree",
					Provider = value,
					Proxies = new SerializableDefinitionId[1]
					{
						serializableDefinitionId
					}
				},
				new MyEnvironmentItemTypeDefinition
				{
					LodFrom = 0,
					LodTo = -1,
					Name = "Bush",
					Provider = value,
					Proxies = new SerializableDefinitionId[1]
					{
						serializableDefinitionId
					}
				},
				new MyEnvironmentItemTypeDefinition
				{
					LodFrom = 0,
					LodTo = -1,
					Name = "VoxelMap",
					Provider = value2,
					Proxies = new SerializableDefinitionId[1]
					{
						serializableDefinitionId2
					}
				},
				new MyEnvironmentItemTypeDefinition
				{
					LodFrom = 0,
					LodTo = -1,
					Name = "Bot",
					Provider = null,
					Proxies = new SerializableDefinitionId[1]
					{
						serializableDefinitionId3
					}
				}
			};
			myObjectBuilder_ProceduralWorldEnvironment.ScanningMethod = MyProceduralScanningMethod.Random;
			myObjectBuilder_ProceduralWorldEnvironment.ItemsPerSqMeter = 0.0034;
			myObjectBuilder_ProceduralWorldEnvironment.MaxSyncLod = 0;
			myObjectBuilder_ProceduralWorldEnvironment.SectorSize = 200.0;
			List<MyProceduralEnvironmentMapping> list = new List<MyProceduralEnvironmentMapping>();
			List<MyEnvironmentItemInfo> list2 = new List<MyEnvironmentItemInfo>();
			MyPlanetSurfaceRule myPlanetSurfaceRule = new MyPlanetSurfaceRule();
			if (pgdef.EnvironmentItems != null)
			{
				PlanetEnvironmentItemMapping[] environmentItems = pgdef.EnvironmentItems;
				for (int i = 0; i < environmentItems.Length; i++)
				{
					PlanetEnvironmentItemMapping planetEnvironmentItemMapping = environmentItems[i];
					MyProceduralEnvironmentMapping myProceduralEnvironmentMapping = new MyProceduralEnvironmentMapping();
					myProceduralEnvironmentMapping.Biomes = planetEnvironmentItemMapping.Biomes;
					myProceduralEnvironmentMapping.Materials = planetEnvironmentItemMapping.Materials;
					MyPlanetSurfaceRule myPlanetSurfaceRule2 = planetEnvironmentItemMapping.Rule ?? myPlanetSurfaceRule;
					myProceduralEnvironmentMapping.Height = myPlanetSurfaceRule2.Height;
					myProceduralEnvironmentMapping.Latitude = myPlanetSurfaceRule2.Latitude;
					myProceduralEnvironmentMapping.Longitude = myPlanetSurfaceRule2.Longitude;
					myProceduralEnvironmentMapping.Slope = myPlanetSurfaceRule2.Slope;
					list2.Clear();
					MyPlanetEnvironmentItemDef[] items = planetEnvironmentItemMapping.Items;
					foreach (MyPlanetEnvironmentItemDef myPlanetEnvironmentItemDef in items)
					{
						MyEnvironmentItemInfo myEnvironmentItemInfo = new MyEnvironmentItemInfo
						{
							Density = myPlanetEnvironmentItemDef.Density,
							Subtype = MyStringHash.GetOrCompute(myPlanetEnvironmentItemDef.SubtypeId)
						};
						switch (myPlanetEnvironmentItemDef.TypeId)
						{
						case "MyObjectBuilder_DestroyableItems":
							myEnvironmentItemInfo.Type = "Bush";
							myEnvironmentItemInfo.Density *= 0.5f;
							break;
						case "MyObjectBuilder_Trees":
							myEnvironmentItemInfo.Type = "Tree";
							break;
						case "MyObjectBuilder_VoxelMapStorageDefinition":
							myEnvironmentItemInfo.Type = "VoxelMap";
							myEnvironmentItemInfo.Density *= 0.5f;
							if (myPlanetEnvironmentItemDef.SubtypeId == null)
							{
								MyStringHash orCompute = MyStringHash.GetOrCompute($"G({myPlanetEnvironmentItemDef.GroupId})M({myPlanetEnvironmentItemDef.ModifierId})");
								MyVoxelMapCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyVoxelMapCollectionDefinition>(orCompute);
								if (definition == null)
								{
									MyObjectBuilder_VoxelMapCollectionDefinition myObjectBuilder_VoxelMapCollectionDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_VoxelMapCollectionDefinition>(orCompute.ToString());
									myObjectBuilder_VoxelMapCollectionDefinition.Id = new SerializableDefinitionId(myObjectBuilder_VoxelMapCollectionDefinition.TypeId, myObjectBuilder_VoxelMapCollectionDefinition.SubtypeName);
									myObjectBuilder_VoxelMapCollectionDefinition.StorageDefs = new MyObjectBuilder_VoxelMapCollectionDefinition.VoxelMapStorage[1]
									{
										new MyObjectBuilder_VoxelMapCollectionDefinition.VoxelMapStorage
										{
											Storage = myPlanetEnvironmentItemDef.GroupId
										}
									};
									myObjectBuilder_VoxelMapCollectionDefinition.Modifier = myPlanetEnvironmentItemDef.ModifierId;
									definition = new MyVoxelMapCollectionDefinition();
									definition.Init(myObjectBuilder_VoxelMapCollectionDefinition, context);
									MyDefinitionManager.Static.Definitions.AddDefinition(definition);
								}
								myEnvironmentItemInfo.Subtype = orCompute;
							}
							break;
						default:
							MyLog.Default.Error("Planet Generator {0}: Invalid Item Type: {1}", pgdef.SubtypeName, myPlanetEnvironmentItemDef.SubtypeId);
							continue;
						}
						_ = myEnvironmentItemInfo.Subtype;
						list2.Add(myEnvironmentItemInfo);
					}
					myProceduralEnvironmentMapping.Items = list2.ToArray();
					list.Add(myProceduralEnvironmentMapping);
				}
			}
			list.Capacity = list.Count;
			myObjectBuilder_ProceduralWorldEnvironment.EnvironmentMappings = list.ToArray();
			MyProceduralEnvironmentDefinition myProceduralEnvironmentDefinition = new MyProceduralEnvironmentDefinition();
			myProceduralEnvironmentDefinition.Context = context;
			myProceduralEnvironmentDefinition.Init(myObjectBuilder_ProceduralWorldEnvironment);
			return myProceduralEnvironmentDefinition;
		}

		public void GetItemDefinition(ushort definitionIndex, out MyRuntimeEnvironmentItemInfo def)
		{
			if (definitionIndex >= Items.Length)
			{
				def = null;
			}
			else
			{
				def = Items[definitionIndex];
			}
		}
	}
}
