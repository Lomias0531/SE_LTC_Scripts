using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Fractures;
using VRageRender.Messages;
using VRageRender.Models;
using VRageRender.Utils;

namespace Sandbox
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyDestructionData : MySessionComponentBase
	{
		private static List<HkdShapeInstanceInfo> m_tmpChildrenList = new List<HkdShapeInstanceInfo>();

		private static MyPhysicsMesh m_tmpMesh = new MyPhysicsMesh();

		private HkDestructionStorage Storage;

		private static Dictionary<string, MyPhysicalMaterialDefinition> m_physicalMaterials;

		public static MyDestructionData Static
		{
			get;
			set;
		}

		public HkWorld TemporaryWorld
		{
			get;
			private set;
		}

		public MyBlockShapePool BlockShapePool
		{
			get;
			private set;
		}

		public override bool IsRequiredByGame => MyPerGameSettings.Destruction;

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			BlockShapePool.RefillPools();
		}

		public override void LoadData()
		{
			if (!HkBaseSystem.DestructionEnabled)
			{
				MyLog.Default.WriteLine("Havok Destruction is not availiable in this build.");
				throw new InvalidOperationException("Havok Destruction is not availiable in this build.");
			}
			if (Static != null)
			{
				MyLog.Default.WriteLine("Destruction data was not freed. Unloading now...");
				UnloadData();
			}
			Static = this;
			BlockShapePool = new MyBlockShapePool();
			TemporaryWorld = new HkWorld(enableGlobalGravity: true, 50000f, MyPhysics.RestingVelocity, MyFakes.ENABLE_HAVOK_MULTITHREADING, 4);
			TemporaryWorld.MarkForWrite();
			TemporaryWorld.DestructionWorld = new HkdWorld(TemporaryWorld);
			TemporaryWorld.UnmarkForWrite();
			Storage = new HkDestructionStorage(TemporaryWorld.DestructionWorld);
			foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
				if (definitionGroup.Large != null)
				{
					MyModel model = MyModels.GetModel(definitionGroup.Large.Model);
					if (model == null)
					{
						continue;
					}
					if (!MyFakes.LAZY_LOAD_DESTRUCTION || (model != null && model.HavokBreakableShapes != null))
					{
						LoadModelDestruction(definitionGroup.Large.Model, definitionGroup.Large, definitionGroup.Large.Size * MyDefinitionManager.Static.GetCubeSize(definitionGroup.Large.CubeSize));
					}
					MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = definitionGroup.Large.BuildProgressModels;
					foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in buildProgressModels)
					{
						model = MyModels.GetModel(buildProgressModel.File);
						if (model != null && (!MyFakes.LAZY_LOAD_DESTRUCTION || (model != null && model.HavokBreakableShapes != null)))
						{
							LoadModelDestruction(buildProgressModel.File, definitionGroup.Large, definitionGroup.Large.Size * MyDefinitionManager.Static.GetCubeSize(definitionGroup.Large.CubeSize));
						}
					}
					if (MyFakes.CHANGE_BLOCK_CONVEX_RADIUS && model != null && model.HavokBreakableShapes != null)
					{
						HkShape shape = model.HavokBreakableShapes[0].GetShape();
						if (shape.ShapeType != 0 && shape.ShapeType != HkShapeType.Capsule)
						{
							SetConvexRadius(model.HavokBreakableShapes[0], 0.05f);
						}
					}
				}
				if (definitionGroup.Small != null)
				{
					MyModel model2 = MyModels.GetModel(definitionGroup.Small.Model);
					if (model2 != null)
					{
						if (!MyFakes.LAZY_LOAD_DESTRUCTION || (model2 != null && model2.HavokBreakableShapes != null))
						{
							LoadModelDestruction(definitionGroup.Small.Model, definitionGroup.Small, definitionGroup.Small.Size * MyDefinitionManager.Static.GetCubeSize(definitionGroup.Small.CubeSize));
						}
						MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = definitionGroup.Small.BuildProgressModels;
						foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel2 in buildProgressModels)
						{
							model2 = MyModels.GetModel(buildProgressModel2.File);
							if (model2 != null && (!MyFakes.LAZY_LOAD_DESTRUCTION || (model2 != null && model2.HavokBreakableShapes != null)))
							{
								LoadModelDestruction(buildProgressModel2.File, definitionGroup.Small, definitionGroup.Large.Size * MyDefinitionManager.Static.GetCubeSize(definitionGroup.Large.CubeSize));
							}
						}
						if (MyFakes.CHANGE_BLOCK_CONVEX_RADIUS && model2 != null && model2.HavokBreakableShapes != null)
						{
							HkShape shape2 = model2.HavokBreakableShapes[0].GetShape();
							if (shape2.ShapeType != 0 && shape2.ShapeType != HkShapeType.Capsule)
							{
								SetConvexRadius(model2.HavokBreakableShapes[0], 0.05f);
							}
						}
					}
				}
			}
			if (!MyFakes.LAZY_LOAD_DESTRUCTION)
			{
				BlockShapePool.Preallocate();
			}
			foreach (MyPhysicalModelDefinition allDefinition in MyDefinitionManager.Static.GetAllDefinitions<MyPhysicalModelDefinition>())
			{
				LoadModelDestruction(allDefinition.Model, allDefinition, Vector3.One, destructionRequired: false, useShapeVolume: true);
			}
		}

		protected override void UnloadData()
		{
			TemporaryWorld.MarkForWrite();
			Storage.Dispose();
			Storage = null;
			TemporaryWorld.DestructionWorld.Dispose();
			TemporaryWorld.Dispose();
			TemporaryWorld = null;
			BlockShapePool.Free();
			BlockShapePool = null;
			Static = null;
		}

		private HkReferenceObject CreateGeometryFromSplitPlane(string splitPlane)
		{
			MyModel modelOnlyData = MyModels.GetModelOnlyData(splitPlane);
			if (modelOnlyData != null)
			{
				IPhysicsMesh graphicsData = CreatePhysicsMesh(modelOnlyData);
				return Storage.CreateGeometry(graphicsData, Path.GetFileNameWithoutExtension(splitPlane));
			}
			return null;
		}

		private void FractureBreakableShape(HkdBreakableShape bShape, MyModelFractures modelFractures, string modPath)
		{
			HkdFracture hkdFracture = null;
			HkReferenceObject hkReferenceObject = null;
			if (modelFractures.Fractures[0] is RandomSplitFractureSettings)
			{
				RandomSplitFractureSettings randomSplitFractureSettings = (RandomSplitFractureSettings)modelFractures.Fractures[0];
				hkdFracture = new HkdRandomSplitFracture
				{
					NumObjectsOnLevel1 = randomSplitFractureSettings.NumObjectsOnLevel1,
					NumObjectsOnLevel2 = randomSplitFractureSettings.NumObjectsOnLevel2,
					RandomRange = randomSplitFractureSettings.RandomRange,
					RandomSeed1 = randomSplitFractureSettings.RandomSeed1,
					RandomSeed2 = randomSplitFractureSettings.RandomSeed2,
					SplitGeometryScale = Vector4.One
				};
				if (!string.IsNullOrEmpty(randomSplitFractureSettings.SplitPlane))
				{
					string text = randomSplitFractureSettings.SplitPlane;
					if (!string.IsNullOrEmpty(modPath))
					{
						text = Path.Combine(modPath, randomSplitFractureSettings.SplitPlane);
					}
					hkReferenceObject = CreateGeometryFromSplitPlane(text);
					if (hkReferenceObject != null)
					{
						((HkdRandomSplitFracture)hkdFracture).SetGeometry(hkReferenceObject);
						MyRenderProxy.PreloadMaterials(text);
					}
				}
			}
			if (modelFractures.Fractures[0] is VoronoiFractureSettings)
			{
				VoronoiFractureSettings voronoiFractureSettings = (VoronoiFractureSettings)modelFractures.Fractures[0];
				hkdFracture = new HkdVoronoiFracture
				{
					Seed = voronoiFractureSettings.Seed,
					NumSitesToGenerate = voronoiFractureSettings.NumSitesToGenerate,
					NumIterations = voronoiFractureSettings.NumIterations
				};
				if (!string.IsNullOrEmpty(voronoiFractureSettings.SplitPlane))
				{
					string text2 = voronoiFractureSettings.SplitPlane;
					if (!string.IsNullOrEmpty(modPath))
					{
						text2 = Path.Combine(modPath, voronoiFractureSettings.SplitPlane);
					}
					hkReferenceObject = CreateGeometryFromSplitPlane(text2);
					MyModels.GetModel(text2);
					if (hkReferenceObject != null)
					{
						((HkdVoronoiFracture)hkdFracture).SetGeometry(hkReferenceObject);
						MyRenderProxy.PreloadMaterials(text2);
					}
				}
			}
			if (modelFractures.Fractures[0] is WoodFractureSettings)
			{
				_ = (WoodFractureSettings)modelFractures.Fractures[0];
				hkdFracture = new HkdWoodFracture();
			}
			if (hkdFracture != null)
			{
				Storage.FractureShape(bShape, hkdFracture);
				hkdFracture.Dispose();
			}
			if (hkReferenceObject != null)
			{
				hkReferenceObject.Dispose();
			}
		}

		private IPhysicsMesh CreatePhysicsMesh(MyModel model)
		{
			IPhysicsMesh physicsMesh = new MyPhysicsMesh();
			physicsMesh.SetAABB(model.BoundingBox.Min, model.BoundingBox.Max);
			for (int i = 0; i < model.GetVerticesCount(); i++)
			{
				Vector3 vertex = model.GetVertex(i);
				Vector3 vertexNormal = model.GetVertexNormal(i);
				Vector3 vertexTangent = model.GetVertexTangent(i);
				if (model.TexCoords == null)
				{
					model.LoadTexCoordData();
				}
				Vector2 texCoord = model.TexCoords[i].ToVector2();
				physicsMesh.AddVertex(vertex, vertexNormal, vertexTangent, texCoord);
			}
			for (int j = 0; j < model.Indices16.Length; j++)
			{
				physicsMesh.AddIndex(model.Indices16[j]);
			}
			for (int k = 0; k < model.GetMeshList().Count; k++)
			{
				MyMesh myMesh = model.GetMeshList()[k];
				physicsMesh.AddSectionData(myMesh.IndexStart, myMesh.TriCount, myMesh.Material.Name);
			}
			return physicsMesh;
		}

		private void CreateBreakableShapeFromCollisionShapes(MyModel model, Vector3 defaultSize, MyPhysicalModelDefinition modelDef)
		{
			HkShape shape;
			if (model.HavokCollisionShapes != null && model.HavokCollisionShapes.Length != 0)
			{
				if (model.HavokCollisionShapes.Length > 1)
				{
					shape = HkListShape.Create(model.HavokCollisionShapes, model.HavokCollisionShapes.Length, HkReferencePolicy.None);
				}
				else
				{
					shape = model.HavokCollisionShapes[0];
					shape.AddReference();
				}
			}
			else
			{
				shape = new HkBoxShape(defaultSize * 0.5f, MyPerGameSettings.PhysicsConvexRadius);
			}
			HkdBreakableShape hkdBreakableShape = new HkdBreakableShape(shape);
			hkdBreakableShape.Name = model.AssetName;
			hkdBreakableShape.SetMass(modelDef.Mass);
			model.HavokBreakableShapes = new HkdBreakableShape[1]
			{
				hkdBreakableShape
			};
			shape.RemoveReference();
		}

		public void LoadModelDestruction(string modelName, MyPhysicalModelDefinition modelDef, Vector3 defaultSize, bool destructionRequired = true, bool useShapeVolume = false)
		{
			MyModel modelOnlyData = MyModels.GetModelOnlyData(modelName);
			if (modelOnlyData.HavokBreakableShapes != null)
			{
				return;
			}
			bool flag = false;
			MyCubeBlockDefinition myCubeBlockDefinition = modelDef as MyCubeBlockDefinition;
			if (myCubeBlockDefinition != null)
			{
				flag = !myCubeBlockDefinition.CreateFracturedPieces;
			}
			MyPhysicalMaterialDefinition physicalMaterial = modelDef.PhysicalMaterial;
			if (modelOnlyData == null)
			{
				return;
			}
			bool flag2 = false;
			modelOnlyData.LoadUV = true;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			if (modelOnlyData.ModelFractures != null)
			{
				if (modelOnlyData.HavokCollisionShapes != null && modelOnlyData.HavokCollisionShapes.Length != 0)
				{
					CreateBreakableShapeFromCollisionShapes(modelOnlyData, defaultSize, modelDef);
					IPhysicsMesh graphicsData = CreatePhysicsMesh(modelOnlyData);
					Storage.RegisterShapeWithGraphics(graphicsData, modelOnlyData.HavokBreakableShapes[0], modelName);
					string modPath = null;
					if (Path.IsPathRooted(modelOnlyData.AssetName))
					{
						modPath = modelOnlyData.AssetName.Remove(modelOnlyData.AssetName.LastIndexOf("Models"));
					}
					FractureBreakableShape(modelOnlyData.HavokBreakableShapes[0], modelOnlyData.ModelFractures, modPath);
					flag4 = true;
					flag5 = true;
					flag3 = true;
				}
			}
			else if (modelOnlyData.HavokDestructionData != null && !flag2)
			{
				try
				{
					if (modelOnlyData.HavokBreakableShapes == null)
					{
						modelOnlyData.HavokBreakableShapes = Storage.LoadDestructionDataFromBuffer(modelOnlyData.HavokDestructionData);
						flag3 = true;
						flag4 = true;
						flag5 = true;
					}
				}
				catch
				{
					modelOnlyData.HavokBreakableShapes = null;
				}
			}
			modelOnlyData.HavokDestructionData = null;
			modelOnlyData.HavokData = null;
			if (modelOnlyData.HavokBreakableShapes == null && destructionRequired)
			{
				MyLog.Default.WriteLine(modelOnlyData.AssetName + " does not have destruction data");
				CreateBreakableShapeFromCollisionShapes(modelOnlyData, defaultSize, modelDef);
				flag4 = true;
				flag5 = true;
			}
			if (modelOnlyData.HavokBreakableShapes == null)
			{
				MyLog.Default.WriteLine($"Model {modelOnlyData.AssetName} - Unable to load havok destruction data", LoggingOptions.LOADING_MODELS);
				return;
			}
			HkdBreakableShape hkdBreakableShape = modelOnlyData.HavokBreakableShapes[0];
			if (flag)
			{
				hkdBreakableShape.SetFlagRecursively(HkdBreakableShape.Flags.DONT_CREATE_FRACTURE_PIECE);
			}
			if (flag5)
			{
				hkdBreakableShape.AddReference();
				Storage.RegisterShape(hkdBreakableShape, modelName);
			}
			MyRenderProxy.PreloadMaterials(modelOnlyData.AssetName);
			if (flag3)
			{
				CreatePieceData(modelOnlyData, hkdBreakableShape);
			}
			if (flag4)
			{
				float num = hkdBreakableShape.CalculateGeometryVolume();
				if (num <= 0f || useShapeVolume)
				{
					num = hkdBreakableShape.Volume;
				}
				float m = num * physicalMaterial.Density;
				hkdBreakableShape.SetMassRecursively(MyDestructionHelper.MassToHavok(m));
			}
			if (modelDef.Mass > 0f)
			{
				hkdBreakableShape.SetMassRecursively(MyDestructionHelper.MassToHavok(modelDef.Mass));
			}
			DisableRefCountRec(hkdBreakableShape);
			if (MyFakes.CHANGE_BLOCK_CONVEX_RADIUS && modelOnlyData != null && modelOnlyData.HavokBreakableShapes != null)
			{
				HkShape shape = modelOnlyData.HavokBreakableShapes[0].GetShape();
				if (shape.ShapeType != 0 && shape.ShapeType != HkShapeType.Capsule)
				{
					SetConvexRadius(modelOnlyData.HavokBreakableShapes[0], 0.05f);
				}
			}
			if (MyFakes.LAZY_LOAD_DESTRUCTION)
			{
				BlockShapePool.AllocateForDefinition(modelName, modelDef, 50);
			}
		}

		private void SetConvexRadius(HkdBreakableShape bShape, float radius)
		{
			HkShape shape = bShape.GetShape();
			if (shape.IsConvex)
			{
				HkConvexShape hkConvexShape = (HkConvexShape)shape;
				if (hkConvexShape.ConvexRadius > radius)
				{
					hkConvexShape.ConvexRadius = radius;
				}
			}
			else
			{
				if (!shape.IsContainer())
				{
					return;
				}
				HkShapeContainerIterator container = shape.GetContainer();
				while (container.IsValid)
				{
					if (container.CurrentValue.IsConvex)
					{
						HkConvexShape hkConvexShape2 = (HkConvexShape)container.CurrentValue;
						if (hkConvexShape2.ConvexRadius > radius)
						{
							hkConvexShape2.ConvexRadius = radius;
						}
					}
					container.Next();
				}
			}
		}

		private bool CheckVolumeMassRec(HkdBreakableShape bShape, float minVolume, float minMass)
		{
			if (bShape.Name.Contains("Fake"))
			{
				return true;
			}
			if (bShape.Volume <= minVolume)
			{
				return false;
			}
			HkMassProperties massProperties = default(HkMassProperties);
			bShape.BuildMassProperties(ref massProperties);
			if (massProperties.Mass <= minMass)
			{
				return false;
			}
			if (massProperties.InertiaTensor.M11 == 0f || massProperties.InertiaTensor.M22 == 0f || massProperties.InertiaTensor.M33 == 0f)
			{
				return false;
			}
			for (int i = 0; i < bShape.GetChildrenCount(); i++)
			{
				if (!CheckVolumeMassRec(bShape.GetChildShape(i), minVolume, minMass))
				{
					return false;
				}
			}
			return true;
		}

		public static MyPhysicalMaterialDefinition GetPhysicalMaterial(MyPhysicalModelDefinition modelDef, string physicalMaterial)
		{
			if (m_physicalMaterials == null)
			{
				m_physicalMaterials = new Dictionary<string, MyPhysicalMaterialDefinition>();
				foreach (MyPhysicalMaterialDefinition physicalMaterialDefinition in MyDefinitionManager.Static.GetPhysicalMaterialDefinitions())
				{
					m_physicalMaterials.Add(physicalMaterialDefinition.Id.SubtypeName, physicalMaterialDefinition);
				}
				m_physicalMaterials["Default"] = new MyPhysicalMaterialDefinition
				{
					Density = 1920f,
					HorisontalTransmissionMultiplier = 1f,
					HorisontalFragility = 2f,
					CollisionMultiplier = 1.4f,
					SupportMultiplier = 1.5f
				};
			}
			if (!string.IsNullOrEmpty(physicalMaterial))
			{
				if (m_physicalMaterials.ContainsKey(physicalMaterial))
				{
					return m_physicalMaterials[physicalMaterial];
				}
				string msg = "ERROR: Physical material " + physicalMaterial + " does not exist!";
				MyLog.Default.WriteLine(msg);
			}
			if (modelDef.Id.SubtypeName.Contains("Stone") && m_physicalMaterials.ContainsKey("Stone"))
			{
				return m_physicalMaterials["Stone"];
			}
			if (modelDef.Id.SubtypeName.Contains("Wood") && m_physicalMaterials.ContainsKey("Wood"))
			{
				return m_physicalMaterials["Wood"];
			}
			if (modelDef.Id.SubtypeName.Contains("Timber") && m_physicalMaterials.ContainsKey("Timber"))
			{
				return m_physicalMaterials["Wood"];
			}
			return m_physicalMaterials["Default"];
		}

		private void DisableRefCountRec(HkdBreakableShape bShape)
		{
			bShape.DisableRefCount();
			List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
			bShape.GetChildren(list);
			foreach (HkdShapeInstanceInfo item in list)
			{
				DisableRefCountRec(item.Shape);
			}
		}

		private void CreatePieceData(MyModel model, HkdBreakableShape breakableShape)
		{
			MyRenderMessageAddRuntimeModel myRenderMessageAddRuntimeModel = MyRenderProxy.PrepareAddRuntimeModel();
			m_tmpMesh.Data = myRenderMessageAddRuntimeModel.ModelData;
			Static.Storage.GetDataFromShape(breakableShape, m_tmpMesh);
			if (myRenderMessageAddRuntimeModel.ModelData.Sections.Count > 0)
			{
				if (MyFakes.USE_HAVOK_MODELS)
				{
					myRenderMessageAddRuntimeModel.ReplacedModel = model.AssetName;
				}
				MyRenderProxy.AddRuntimeModel(breakableShape.ShapeName, myRenderMessageAddRuntimeModel);
			}
			using (m_tmpChildrenList.GetClearToken())
			{
				breakableShape.GetChildren(m_tmpChildrenList);
				LoadChildrenShapes(m_tmpChildrenList);
			}
		}

		private static void LoadChildrenShapes(List<HkdShapeInstanceInfo> children)
		{
			foreach (HkdShapeInstanceInfo child in children)
			{
				if (child.IsValid())
				{
					MyRenderMessageAddRuntimeModel myRenderMessageAddRuntimeModel = MyRenderProxy.PrepareAddRuntimeModel();
					m_tmpMesh.Data = myRenderMessageAddRuntimeModel.ModelData;
					Static.Storage.GetDataFromShapeInstance(child, m_tmpMesh);
					m_tmpMesh.Transform(child.GetTransform());
					if (myRenderMessageAddRuntimeModel.ModelData.Sections.Count > 0)
					{
						MyRenderProxy.AddRuntimeModel(child.ShapeName, myRenderMessageAddRuntimeModel);
					}
					List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
					child.GetChildren(list);
					LoadChildrenShapes(list);
				}
			}
		}

		public float GetBlockMass(string model, MyCubeBlockDefinition def)
		{
			HkdBreakableShape breakableShape = BlockShapePool.GetBreakableShape(model, def);
			float mass = breakableShape.GetMass();
			BlockShapePool.EnqueShape(model, def.Id, breakableShape);
			return mass;
		}
	}
}
