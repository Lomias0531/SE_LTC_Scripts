using Sandbox.Game.WorldEnvironment.Definitions;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PlanetGeneratorDefinition), typeof(Postprocessor))]
	public class MyPlanetGeneratorDefinition : MyDefinitionBase
	{
		internal class Postprocessor : MyDefinitionPostprocessor
		{
			public override int Priority => 1000;

			public override void AfterLoaded(ref Bundle definitions)
			{
			}

			public override void AfterPostprocess(MyDefinitionSet set, Dictionary<MyStringHash, MyDefinitionBase> definitions)
			{
				List<int> list = new List<int>();
				foreach (MyDefinitionBase value in definitions.Values)
				{
					MyPlanetGeneratorDefinition myPlanetGeneratorDefinition = (MyPlanetGeneratorDefinition)value;
					if (!myPlanetGeneratorDefinition.EnvironmentId.HasValue)
					{
						myPlanetGeneratorDefinition.EnvironmentDefinition = MyProceduralEnvironmentDefinition.FromLegacyPlanet(myPlanetGeneratorDefinition.m_pgob, value.Context);
						set.AddOrRelaceDefinition(myPlanetGeneratorDefinition.EnvironmentDefinition);
						myPlanetGeneratorDefinition.m_pgob = null;
					}
					else
					{
						myPlanetGeneratorDefinition.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyWorldEnvironmentDefinition>(myPlanetGeneratorDefinition.EnvironmentId.Value);
					}
					if (myPlanetGeneratorDefinition.EnvironmentDefinition != null)
					{
						myPlanetGeneratorDefinition.EnvironmentSectorType = myPlanetGeneratorDefinition.EnvironmentDefinition.SectorType;
						foreach (Dictionary<string, List<MyPlanetEnvironmentMapping>> value2 in myPlanetGeneratorDefinition.MaterialEnvironmentMappings.Values)
						{
							foreach (List<MyPlanetEnvironmentMapping> value3 in value2.Values)
							{
								foreach (MyPlanetEnvironmentMapping item in value3)
								{
									for (int i = 0; i < item.Items.Length; i++)
									{
										if (item.Items[i].IsEnvironemntItem && !MyDefinitionManager.Static.TryGetDefinition(item.Items[i].Definition, out MyEnvironmentItemsDefinition _))
										{
											MyLog.Default.WriteLine($"Could not find environment item definition for {item.Items[i].Definition}.");
											list.Add(i);
										}
									}
									if (list.Count > 0)
									{
										item.Items = item.Items.RemoveIndices(list);
										item.ComputeDistribution();
										list.Clear();
									}
								}
							}
						}
					}
				}
			}
		}

		private class Sandbox_Definitions_MyPlanetGeneratorDefinition_003C_003EActor : IActivator, IActivator<MyPlanetGeneratorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetGeneratorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetGeneratorDefinition CreateInstance()
			{
				return new MyPlanetGeneratorDefinition();
			}

			MyPlanetGeneratorDefinition IActivator<MyPlanetGeneratorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId? EnvironmentId;

		public MyWorldEnvironmentDefinition EnvironmentDefinition;

		private MyObjectBuilder_PlanetGeneratorDefinition m_pgob;

		public bool HasAtmosphere;

		public List<MyCloudLayerSettings> CloudLayers;

		public MyPlanetMaps PlanetMaps;

		public SerializableRange HillParams;

		public SerializableRange MaterialsMaxDepth;

		public SerializableRange MaterialsMinDepth;

		public MyPlanetOreMapping[] OreMappings = new MyPlanetOreMapping[0];

		public float GravityFalloffPower = 7f;

		public MyObjectBuilder_PlanetMapProvider MapProvider;

		public MyAtmosphereColorShift HostileAtmosphereColorShift = new MyAtmosphereColorShift();

		public MyPlanetMaterialDefinition[] SurfaceMaterialTable = new MyPlanetMaterialDefinition[0];

		public MyPlanetDistortionDefinition[] DistortionTable = new MyPlanetDistortionDefinition[0];

		public MyPlanetMaterialDefinition DefaultSurfaceMaterial;

		public MyPlanetMaterialDefinition DefaultSubSurfaceMaterial;

		public MyPlanetEnvironmentalSoundRule[] SoundRules;

		public List<MyMusicCategory> MusicCategories;

		public MyPlanetMaterialGroup[] MaterialGroups = new MyPlanetMaterialGroup[0];

		public Dictionary<int, Dictionary<string, List<MyPlanetEnvironmentMapping>>> MaterialEnvironmentMappings = new Dictionary<int, Dictionary<string, List<MyPlanetEnvironmentMapping>>>();

		public float SurfaceGravity = 1f;

		public float AtmosphereHeight;

		public float SectorDensity = 0.0017f;

		public MyPlanetAtmosphere Atmosphere = new MyPlanetAtmosphere();

		public MyAtmosphereSettings? AtmosphereSettings;

		public MyPlanetMaterialBlendSettings MaterialBlending = new MyPlanetMaterialBlendSettings
		{
			Texture = "Data/PlanetDataFiles/Extra/material_blend_grass",
			CellSize = 64
		};

		public string FolderName;

		public MyPlanetSurfaceDetail Detail;

		public MyPlanetAnimalSpawnInfo AnimalSpawnInfo;

		public MyPlanetAnimalSpawnInfo NightAnimalSpawnInfo;

		public Type EnvironmentSectorType;

		public MyObjectBuilder_VoxelMesherComponentDefinition MesherPostprocessing;

		public float MinimumSurfaceLayerDepth;

		public List<MyDefinitionId> StationBlockingMaterials;

		private void InheritFrom(string generator)
		{
			MyPlanetGeneratorDefinition value = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(generator));
			if (value == null)
			{
				MyDefinitionManager.Static.LoadingSet.m_planetGeneratorDefinitions.TryGetValue(new MyDefinitionId(typeof(MyObjectBuilder_PlanetGeneratorDefinition), generator), out value);
			}
			if (value == null)
			{
				MyLog.Default.WriteLine($"Could not find planet generator definition for '{generator}'.");
				return;
			}
			PlanetMaps = value.PlanetMaps;
			HasAtmosphere = value.HasAtmosphere;
			Atmosphere = value.Atmosphere;
			CloudLayers = value.CloudLayers;
			SoundRules = value.SoundRules;
			MusicCategories = value.MusicCategories;
			HillParams = value.HillParams;
			MaterialsMaxDepth = value.MaterialsMaxDepth;
			MaterialsMinDepth = value.MaterialsMinDepth;
			GravityFalloffPower = value.GravityFalloffPower;
			HostileAtmosphereColorShift = value.HostileAtmosphereColorShift;
			SurfaceMaterialTable = value.SurfaceMaterialTable;
			DistortionTable = value.DistortionTable;
			DefaultSurfaceMaterial = value.DefaultSurfaceMaterial;
			DefaultSubSurfaceMaterial = value.DefaultSubSurfaceMaterial;
			MaterialGroups = value.MaterialGroups;
			MaterialEnvironmentMappings = value.MaterialEnvironmentMappings;
			SurfaceGravity = value.SurfaceGravity;
			AtmosphereSettings = value.AtmosphereSettings;
			FolderName = value.FolderName;
			MaterialBlending = value.MaterialBlending;
			OreMappings = value.OreMappings;
			AnimalSpawnInfo = value.AnimalSpawnInfo;
			NightAnimalSpawnInfo = value.NightAnimalSpawnInfo;
			Detail = value.Detail;
			SectorDensity = value.SectorDensity;
			EnvironmentSectorType = value.EnvironmentSectorType;
			MesherPostprocessing = value.MesherPostprocessing;
			MinimumSurfaceLayerDepth = value.MinimumSurfaceLayerDepth;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PlanetGeneratorDefinition myObjectBuilder_PlanetGeneratorDefinition = builder as MyObjectBuilder_PlanetGeneratorDefinition;
			if (myObjectBuilder_PlanetGeneratorDefinition.InheritFrom != null && myObjectBuilder_PlanetGeneratorDefinition.InheritFrom.Length > 0)
			{
				InheritFrom(myObjectBuilder_PlanetGeneratorDefinition.InheritFrom);
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.Environment.HasValue)
			{
				EnvironmentId = myObjectBuilder_PlanetGeneratorDefinition.Environment.Value;
			}
			else
			{
				m_pgob = myObjectBuilder_PlanetGeneratorDefinition;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.PlanetMaps.HasValue)
			{
				PlanetMaps = myObjectBuilder_PlanetGeneratorDefinition.PlanetMaps.Value;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.HasAtmosphere.HasValue)
			{
				HasAtmosphere = myObjectBuilder_PlanetGeneratorDefinition.HasAtmosphere.Value;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.CloudLayers != null)
			{
				CloudLayers = myObjectBuilder_PlanetGeneratorDefinition.CloudLayers;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.SoundRules != null)
			{
				SoundRules = new MyPlanetEnvironmentalSoundRule[myObjectBuilder_PlanetGeneratorDefinition.SoundRules.Length];
				for (int i = 0; i < myObjectBuilder_PlanetGeneratorDefinition.SoundRules.Length; i++)
				{
					MyPlanetEnvironmentalSoundRule myPlanetEnvironmentalSoundRule = default(MyPlanetEnvironmentalSoundRule);
					myPlanetEnvironmentalSoundRule.Latitude = myObjectBuilder_PlanetGeneratorDefinition.SoundRules[i].Latitude;
					myPlanetEnvironmentalSoundRule.Height = myObjectBuilder_PlanetGeneratorDefinition.SoundRules[i].Height;
					myPlanetEnvironmentalSoundRule.SunAngleFromZenith = myObjectBuilder_PlanetGeneratorDefinition.SoundRules[i].SunAngleFromZenith;
					myPlanetEnvironmentalSoundRule.EnvironmentSound = MyStringHash.GetOrCompute(myObjectBuilder_PlanetGeneratorDefinition.SoundRules[i].EnvironmentSound);
					MyPlanetEnvironmentalSoundRule myPlanetEnvironmentalSoundRule2 = myPlanetEnvironmentalSoundRule;
					myPlanetEnvironmentalSoundRule2.Latitude.ConvertToSine();
					myPlanetEnvironmentalSoundRule2.SunAngleFromZenith.ConvertToCosine();
					SoundRules[i] = myPlanetEnvironmentalSoundRule2;
				}
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.MusicCategories != null)
			{
				MusicCategories = myObjectBuilder_PlanetGeneratorDefinition.MusicCategories;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.HillParams.HasValue)
			{
				HillParams = myObjectBuilder_PlanetGeneratorDefinition.HillParams.Value;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.Atmosphere != null)
			{
				Atmosphere = myObjectBuilder_PlanetGeneratorDefinition.Atmosphere;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.GravityFalloffPower.HasValue)
			{
				GravityFalloffPower = myObjectBuilder_PlanetGeneratorDefinition.GravityFalloffPower.Value;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.HostileAtmosphereColorShift != null)
			{
				HostileAtmosphereColorShift = myObjectBuilder_PlanetGeneratorDefinition.HostileAtmosphereColorShift;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.MaterialsMaxDepth.HasValue)
			{
				MaterialsMaxDepth = myObjectBuilder_PlanetGeneratorDefinition.MaterialsMaxDepth.Value;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.MaterialsMinDepth.HasValue)
			{
				MaterialsMinDepth = myObjectBuilder_PlanetGeneratorDefinition.MaterialsMinDepth.Value;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.CustomMaterialTable != null && myObjectBuilder_PlanetGeneratorDefinition.CustomMaterialTable.Length != 0)
			{
				SurfaceMaterialTable = new MyPlanetMaterialDefinition[myObjectBuilder_PlanetGeneratorDefinition.CustomMaterialTable.Length];
				for (int j = 0; j < SurfaceMaterialTable.Length; j++)
				{
					SurfaceMaterialTable[j] = (myObjectBuilder_PlanetGeneratorDefinition.CustomMaterialTable[j].Clone() as MyPlanetMaterialDefinition);
					if (SurfaceMaterialTable[j].Material == null && !SurfaceMaterialTable[j].HasLayers)
					{
						MyLog.Default.WriteLine("Custom material does not contain any material ids.");
					}
					else if (SurfaceMaterialTable[j].HasLayers)
					{
						float depth = SurfaceMaterialTable[j].Layers[0].Depth;
						for (int k = 1; k < SurfaceMaterialTable[j].Layers.Length; k++)
						{
							SurfaceMaterialTable[j].Layers[k].Depth += depth;
							depth = SurfaceMaterialTable[j].Layers[k].Depth;
						}
					}
				}
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.DistortionTable != null && myObjectBuilder_PlanetGeneratorDefinition.DistortionTable.Length != 0)
			{
				DistortionTable = myObjectBuilder_PlanetGeneratorDefinition.DistortionTable;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.DefaultSurfaceMaterial != null)
			{
				DefaultSurfaceMaterial = myObjectBuilder_PlanetGeneratorDefinition.DefaultSurfaceMaterial;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.DefaultSubSurfaceMaterial != null)
			{
				DefaultSubSurfaceMaterial = myObjectBuilder_PlanetGeneratorDefinition.DefaultSubSurfaceMaterial;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.SurfaceGravity.HasValue)
			{
				SurfaceGravity = myObjectBuilder_PlanetGeneratorDefinition.SurfaceGravity.Value;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.AtmosphereSettings.HasValue)
			{
				AtmosphereSettings = myObjectBuilder_PlanetGeneratorDefinition.AtmosphereSettings;
			}
			FolderName = ((myObjectBuilder_PlanetGeneratorDefinition.FolderName != null) ? myObjectBuilder_PlanetGeneratorDefinition.FolderName : myObjectBuilder_PlanetGeneratorDefinition.Id.SubtypeName);
			if (myObjectBuilder_PlanetGeneratorDefinition.ComplexMaterials != null && myObjectBuilder_PlanetGeneratorDefinition.ComplexMaterials.Length != 0)
			{
				MaterialGroups = new MyPlanetMaterialGroup[myObjectBuilder_PlanetGeneratorDefinition.ComplexMaterials.Length];
				for (int l = 0; l < myObjectBuilder_PlanetGeneratorDefinition.ComplexMaterials.Length; l++)
				{
					MaterialGroups[l] = (myObjectBuilder_PlanetGeneratorDefinition.ComplexMaterials[l].Clone() as MyPlanetMaterialGroup);
					MyPlanetMaterialGroup myPlanetMaterialGroup = MaterialGroups[l];
					MyPlanetMaterialPlacementRule[] array = myPlanetMaterialGroup.MaterialRules;
					List<int> list = new List<int>();
					for (int m = 0; m < array.Length; m++)
					{
						if (array[m].Material == null && (array[m].Layers == null || array[m].Layers.Length == 0))
						{
							MyLog.Default.WriteLine("Material rule does not contain any material ids.");
							list.Add(m);
							continue;
						}
						if (array[m].Layers != null && array[m].Layers.Length != 0)
						{
							float depth2 = array[m].Layers[0].Depth;
							for (int n = 1; n < array[m].Layers.Length; n++)
							{
								array[m].Layers[n].Depth += depth2;
								depth2 = array[m].Layers[n].Depth;
							}
						}
						array[m].Slope.ConvertToCosine();
						array[m].Latitude.ConvertToSine();
						array[m].Longitude.ConvertToCosineLongitude();
					}
					if (list.Count > 0)
					{
						array = array.RemoveIndices(list);
					}
					myPlanetMaterialGroup.MaterialRules = array;
				}
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.OreMappings != null)
			{
				OreMappings = myObjectBuilder_PlanetGeneratorDefinition.OreMappings;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.MaterialBlending.HasValue)
			{
				MaterialBlending = myObjectBuilder_PlanetGeneratorDefinition.MaterialBlending.Value;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.SurfaceDetail != null)
			{
				Detail = myObjectBuilder_PlanetGeneratorDefinition.SurfaceDetail;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.AnimalSpawnInfo != null)
			{
				AnimalSpawnInfo = myObjectBuilder_PlanetGeneratorDefinition.AnimalSpawnInfo;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.NightAnimalSpawnInfo != null)
			{
				NightAnimalSpawnInfo = myObjectBuilder_PlanetGeneratorDefinition.NightAnimalSpawnInfo;
			}
			if (myObjectBuilder_PlanetGeneratorDefinition.SectorDensity.HasValue)
			{
				SectorDensity = myObjectBuilder_PlanetGeneratorDefinition.SectorDensity.Value;
			}
			StationBlockingMaterials = new List<MyDefinitionId>();
			if (myObjectBuilder_PlanetGeneratorDefinition.StationBlockingMaterials != null)
			{
				foreach (SerializableDefinitionId stationBlockingMaterial in myObjectBuilder_PlanetGeneratorDefinition.StationBlockingMaterials)
				{
					StationBlockingMaterials.Add(stationBlockingMaterial);
				}
			}
			MapProvider = (myObjectBuilder_PlanetGeneratorDefinition.MapProvider ?? new MyObjectBuilder_PlanetTextureMapProvider
			{
				Path = FolderName
			});
			MesherPostprocessing = myObjectBuilder_PlanetGeneratorDefinition.MesherPostprocessing;
			if (MesherPostprocessing == null)
			{
				MyLog.Default.Warning("PERFORMANCE WARNING: Postprocessing voxel triangle decimation steps not defined for " + this);
			}
			MinimumSurfaceLayerDepth = myObjectBuilder_PlanetGeneratorDefinition.MinimumSurfaceLayerDepth;
		}

		public override string ToString()
		{
			string text = base.ToString();
			FieldInfo[] fields = typeof(MyPlanetGeneratorDefinition).GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.IsPublic)
				{
					object value = fieldInfo.GetValue(this);
					text = text + "\n   " + fieldInfo.Name + " = " + (value ?? "<null>");
				}
			}
			PropertyInfo[] properties = typeof(MyPlanetGeneratorDefinition).GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				object value2 = propertyInfo.GetValue(this, null);
				text = text + "\n   " + propertyInfo.Name + " = " + (value2 ?? "<null>");
			}
			return text;
		}
	}
}
