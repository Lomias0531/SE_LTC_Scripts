using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.EnvironmentItems
{
	[MyEntityType(typeof(MyObjectBuilder_EnvironmentItems), true)]
	public class MyEnvironmentItems : MyEntity, IMyEventProxy, IMyEventOwner
	{
		protected struct MyEnvironmentItemData
		{
			public int Id;

			public MyTransformD Transform;

			public MyStringHash SubtypeId;

			public bool Enabled;

			public int SectorInstanceId;

			public int UserData;

			public MyModel Model;
		}

		public class MyEnvironmentItemsSpawnData
		{
			public MyEnvironmentItems EnvironmentItems;

			public Dictionary<MyStringHash, HkShape> SubtypeToShapes = new Dictionary<MyStringHash, HkShape>(MyStringHash.Comparer);

			public HkStaticCompoundShape SectorRootShape;

			public BoundingBoxD AabbWorld = BoundingBoxD.CreateInvalid();
		}

		public struct ItemInfo
		{
			public int LocalId;

			public MyTransformD Transform;

			public MyStringHash SubtypeId;

			public int UserData;
		}

		private struct AddItemData
		{
			public Vector3D Position;

			public MyStringHash SubtypeId;
		}

		private struct ModifyItemData
		{
			public int LocalId;

			public MyStringHash SubtypeId;
		}

		private struct RemoveItemData
		{
			public int LocalId;
		}

		private class MyEnviromentItemsDebugDraw : MyDebugRenderComponentBase
		{
			private MyEnvironmentItems m_items;

			public MyEnviromentItemsDebugDraw(MyEnvironmentItems items)
			{
				m_items = items;
			}

			public override void DebugDraw()
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_ENVIRONMENT_ITEMS)
				{
					foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in m_items.Sectors)
					{
						sector.Value.DebugDraw(sector.Key, m_items.m_definition.SectorSize);
						if (sector.Value.IsValid)
						{
							Vector3D vector3D = sector.Value.SectorBox.Center + sector.Value.SectorMatrix.Translation;
							if (Vector3D.Distance(MySector.MainCamera.Position, vector3D) < 1000.0)
							{
								MyRenderProxy.DebugDrawText3D(vector3D, m_items.Definition.Id.SubtypeName + " Sector: " + sector.Key, Color.SaddleBrown, 1f, depthRead: true);
							}
						}
					}
				}
			}

			public override void DebugDrawInvalidTriangles()
			{
			}
		}

		private class Sandbox_Game_Entities_EnvironmentItems_MyEnvironmentItems_003C_003EActor : IActivator, IActivator<MyEnvironmentItems>
		{
			private sealed override object CreateInstance()
			{
				return new MyEnvironmentItems();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEnvironmentItems CreateInstance()
			{
				return new MyEnvironmentItems();
			}

			MyEnvironmentItems IActivator<MyEnvironmentItems>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly MyInstanceFlagsEnum m_instanceFlags;

		protected readonly Dictionary<int, MyEnvironmentItemData> m_itemsData = new Dictionary<int, MyEnvironmentItemData>();

		protected readonly Dictionary<int, int> m_physicsShapeInstanceIdToLocalId = new Dictionary<int, int>();

		protected readonly Dictionary<int, int> m_localIdToPhysicsShapeInstanceId = new Dictionary<int, int>();

		protected static readonly Dictionary<MyStringHash, int> m_subtypeToModels;

		protected readonly Dictionary<Vector3I, MyEnvironmentSector> m_sectors = new Dictionary<Vector3I, MyEnvironmentSector>(Vector3I.Comparer);

		protected List<HkdShapeInstanceInfo> m_childrenTmp = new List<HkdShapeInstanceInfo>();

		private HashSet<Vector3I> m_updatedSectorsTmp = new HashSet<Vector3I>();

		private List<HkdBreakableBodyInfo> m_tmpBodyInfos = new List<HkdBreakableBodyInfo>();

		protected static List<HkBodyCollision> m_tmpResults;

		protected static List<MyEnvironmentSector> m_tmpSectors;

		private List<int> m_tmpToDisable = new List<int>();

		private MyEnvironmentItemsDefinition m_definition;

		private List<AddItemData> m_batchedAddItems = new List<AddItemData>();

		private List<ModifyItemData> m_batchedModifyItems = new List<ModifyItemData>();

		private List<RemoveItemData> m_batchedRemoveItems = new List<RemoveItemData>();

		private float m_batchTime;

		private const float BATCH_DEFAULT_TIME = 10f;

		public Vector3 BaseColor;

		public Vector2 ColorSpread;

		private Vector3D m_cellsOffset;

		public Dictionary<Vector3I, MyEnvironmentSector> Sectors => m_sectors;

		public MyEnvironmentItemsDefinition Definition => m_definition;

		public bool IsBatching => m_batchTime > 0f;

		public float BatchTime => m_batchTime;

		public new MyPhysicsBody Physics
		{
			get
			{
				return base.Physics as MyPhysicsBody;
			}
			set
			{
				base.Physics = value;
			}
		}

		public Vector3D CellsOffset
		{
			get
			{
				return m_cellsOffset;
			}
			set
			{
				m_cellsOffset = value;
				base.PositionComp.SetPosition(m_cellsOffset);
			}
		}

		public event Action<MyEnvironmentItems, ItemInfo> ItemAdded;

		public event Action<MyEnvironmentItems, ItemInfo> ItemRemoved;

		public event Action<MyEnvironmentItems, ItemInfo> ItemModified;

		public event Action<MyEnvironmentItems> BatchEnded;

		static MyEnvironmentItems()
		{
			m_subtypeToModels = new Dictionary<MyStringHash, int>(MyStringHash.Comparer);
			m_tmpResults = new List<HkBodyCollision>();
			m_tmpSectors = new List<MyEnvironmentSector>();
			foreach (MyEnvironmentItemDefinition environmentItemDefinition in MyDefinitionManager.Static.GetEnvironmentItemDefinitions())
			{
				CheckModelConsistency(environmentItemDefinition);
			}
		}

		public MyEnvironmentItems()
		{
			m_instanceFlags = (MyInstanceFlagsEnum.CastShadows | MyInstanceFlagsEnum.ShowLod1 | MyInstanceFlagsEnum.EnableColorMask);
			m_definition = null;
			base.Render = new MyRenderComponentEnvironmentItems(this);
			AddDebugRenderComponent(new MyEnviromentItemsDebugDraw(this));
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			Init(null, null, null, null);
			BoundingBoxD aabbWorld = BoundingBoxD.CreateInvalid();
			Dictionary<MyStringHash, HkShape> dictionary = new Dictionary<MyStringHash, HkShape>(MyStringHash.Comparer);
			HkStaticCompoundShape sectorRootShape = new HkStaticCompoundShape(HkReferencePolicy.None);
			MyObjectBuilder_EnvironmentItems myObjectBuilder_EnvironmentItems = (MyObjectBuilder_EnvironmentItems)objectBuilder;
			MyDefinitionId defId = new MyDefinitionId(myObjectBuilder_EnvironmentItems.TypeId, myObjectBuilder_EnvironmentItems.SubtypeId);
			CellsOffset = myObjectBuilder_EnvironmentItems.CellsOffset;
			if (myObjectBuilder_EnvironmentItems.SubtypeId == MyStringHash.NullOrEmpty)
			{
				if (objectBuilder is MyObjectBuilder_Bushes)
				{
					defId = new MyDefinitionId(typeof(MyObjectBuilder_DestroyableItems), "Bushes");
				}
				else if (objectBuilder is MyObjectBuilder_TreesMedium)
				{
					defId = new MyDefinitionId(typeof(MyObjectBuilder_Trees), "TreesMedium");
				}
				else if (objectBuilder is MyObjectBuilder_Trees)
				{
					defId = new MyDefinitionId(typeof(MyObjectBuilder_Trees), "Trees");
				}
			}
			if (!MyDefinitionManager.Static.TryGetDefinition(defId, out m_definition))
			{
				return;
			}
			if (myObjectBuilder_EnvironmentItems.Items != null)
			{
				MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[] items = myObjectBuilder_EnvironmentItems.Items;
				for (int i = 0; i < items.Length; i++)
				{
					MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData myOBEnvironmentItemData = items[i];
					MyStringHash orCompute = MyStringHash.GetOrCompute(myOBEnvironmentItemData.SubtypeName);
					if (m_definition.ContainsItemDefinition(orCompute))
					{
						MyPositionAndOrientation positionAndOrientation = myOBEnvironmentItemData.PositionAndOrientation;
						MatrixD worldMatrix = positionAndOrientation.GetMatrix();
						AddItem(m_definition.GetItemDefinition(orCompute), ref worldMatrix, ref aabbWorld);
					}
				}
			}
			PrepareItemsPhysics(sectorRootShape, ref aabbWorld, dictionary);
			PrepareItemsGraphics();
			foreach (KeyValuePair<MyStringHash, HkShape> item in dictionary)
			{
				item.Value.RemoveReference();
			}
			sectorRootShape.Base.RemoveReference();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_EnvironmentItems myObjectBuilder_EnvironmentItems = (MyObjectBuilder_EnvironmentItems)base.GetObjectBuilder(copy);
			myObjectBuilder_EnvironmentItems.SubtypeName = Definition.Id.SubtypeName;
			if (IsBatching)
			{
				EndBatch(sync: true);
			}
			int num = 0;
			foreach (KeyValuePair<int, MyEnvironmentItemData> itemsDatum in m_itemsData)
			{
				if (itemsDatum.Value.Enabled)
				{
					num++;
				}
			}
			myObjectBuilder_EnvironmentItems.Items = new MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[num];
			int num2 = 0;
			foreach (KeyValuePair<int, MyEnvironmentItemData> itemsDatum2 in m_itemsData)
			{
				if (itemsDatum2.Value.Enabled)
				{
					myObjectBuilder_EnvironmentItems.Items[num2].SubtypeName = itemsDatum2.Value.SubtypeId.ToString();
					myObjectBuilder_EnvironmentItems.Items[num2].PositionAndOrientation = new MyPositionAndOrientation(itemsDatum2.Value.Transform.TransformMatrix);
					num2++;
				}
			}
			myObjectBuilder_EnvironmentItems.CellsOffset = CellsOffset;
			return myObjectBuilder_EnvironmentItems;
		}

		public static MyEnvironmentItemsSpawnData BeginSpawn(MyEnvironmentItemsDefinition itemsDefinition, bool addToScene = true, long withEntityId = 0L)
		{
			MyObjectBuilder_EnvironmentItems myObjectBuilder_EnvironmentItems = MyObjectBuilderSerializer.CreateNewObject(itemsDefinition.Id.TypeId, itemsDefinition.Id.SubtypeName) as MyObjectBuilder_EnvironmentItems;
			myObjectBuilder_EnvironmentItems.EntityId = withEntityId;
			myObjectBuilder_EnvironmentItems.PersistentFlags |= (MyPersistentEntityFlags2)(2 | (addToScene ? 16 : 0) | 4);
			MyEnvironmentItems environmentItems = (!addToScene) ? (MyEntities.CreateFromObjectBuilder(myObjectBuilder_EnvironmentItems, fadeIn: true) as MyEnvironmentItems) : (MyEntities.CreateFromObjectBuilderAndAdd(myObjectBuilder_EnvironmentItems, fadeIn: true) as MyEnvironmentItems);
			return new MyEnvironmentItemsSpawnData
			{
				EnvironmentItems = environmentItems
			};
		}

		public static bool SpawnItem(MyEnvironmentItemsSpawnData spawnData, MyEnvironmentItemDefinition itemDefinition, Vector3D position, Vector3D up, int userdata = -1, bool silentOverlaps = true)
		{
			if (!MyFakes.ENABLE_ENVIRONMENT_ITEMS)
			{
				return true;
			}
			if (spawnData == null || spawnData.EnvironmentItems == null || itemDefinition == null)
			{
				return false;
			}
			Vector3D randomPerpendicularVector = MyUtils.GetRandomPerpendicularVector(ref up);
			MatrixD worldMatrix = MatrixD.CreateWorld(position, randomPerpendicularVector, up);
			return spawnData.EnvironmentItems.AddItem(itemDefinition, ref worldMatrix, ref spawnData.AabbWorld, userdata, silentOverlaps);
		}

		public static void EndSpawn(MyEnvironmentItemsSpawnData spawnData, bool updateGraphics = true, bool updatePhysics = true)
		{
			if (updatePhysics)
			{
				spawnData.EnvironmentItems.PrepareItemsPhysics(spawnData);
				spawnData.SubtypeToShapes.Clear();
				foreach (KeyValuePair<MyStringHash, HkShape> subtypeToShape in spawnData.SubtypeToShapes)
				{
					subtypeToShape.Value.RemoveReference();
				}
				spawnData.SubtypeToShapes.Clear();
			}
			if (updateGraphics)
			{
				spawnData.EnvironmentItems.PrepareItemsGraphics();
			}
			spawnData.EnvironmentItems.UpdateGamePruningStructure();
		}

		public void UnloadGraphics()
		{
			foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in m_sectors)
			{
				sector.Value.UnloadRenderObjects();
			}
		}

		public void ClosePhysics(MyEnvironmentItemsSpawnData data)
		{
			if (Physics != null)
			{
				Physics.Close();
				Physics = null;
			}
		}

		public static string GetModelName(MyStringHash itemSubtype)
		{
			return MyModel.GetById(GetModelId(itemSubtype));
		}

		public static int GetModelId(MyStringHash subtypeId)
		{
			return m_subtypeToModels[subtypeId];
		}

		private bool AddItem(MyEnvironmentItemDefinition itemDefinition, ref MatrixD worldMatrix, ref BoundingBoxD aabbWorld, int userData = -1, bool silentOverlaps = false)
		{
			if (!MyFakes.ENABLE_ENVIRONMENT_ITEMS)
			{
				return true;
			}
			if (!m_definition.ContainsItemDefinition(itemDefinition))
			{
				return false;
			}
			if (itemDefinition.Model == null)
			{
				return false;
			}
			int modelId = GetModelId(itemDefinition.Id.SubtypeId);
			MyModel modelOnlyData = MyModels.GetModelOnlyData(MyModel.GetById(modelId));
			if (modelOnlyData == null)
			{
				return false;
			}
			CheckModelConsistency(itemDefinition);
			int hashCode = worldMatrix.Translation.GetHashCode();
			if (m_itemsData.ContainsKey(hashCode))
			{
				if (!silentOverlaps)
				{
					MyLog.Default.WriteLine("WARNING: items are on the same place.");
				}
				return false;
			}
			MyEnvironmentItemData myEnvironmentItemData = default(MyEnvironmentItemData);
			myEnvironmentItemData.Id = hashCode;
			myEnvironmentItemData.SubtypeId = itemDefinition.Id.SubtypeId;
			myEnvironmentItemData.Transform = new MyTransformD(ref worldMatrix);
			myEnvironmentItemData.Enabled = true;
			myEnvironmentItemData.SectorInstanceId = -1;
			myEnvironmentItemData.Model = modelOnlyData;
			myEnvironmentItemData.UserData = userData;
			MyEnvironmentItemData value = myEnvironmentItemData;
			aabbWorld.Include(modelOnlyData.BoundingBox.Transform(worldMatrix));
			MatrixD transformMatrix = value.Transform.TransformMatrix;
			float num = MyFakes.ENVIRONMENT_ITEMS_ONE_INSTANCEBUFFER ? 20000f : m_definition.SectorSize;
			Vector3I sectorId = MyEnvironmentSector.GetSectorId(transformMatrix.Translation - CellsOffset, num);
			if (!m_sectors.TryGetValue(sectorId, out MyEnvironmentSector value2))
			{
				value2 = new MyEnvironmentSector(sectorId, sectorId * num + CellsOffset);
				m_sectors.Add(sectorId, value2);
			}
			MatrixD matrix = MatrixD.CreateTranslation(-sectorId * num - CellsOffset);
			Matrix localMatrix = value.Transform.TransformMatrix * matrix;
			Color color = BaseColor;
			if (ColorSpread.LengthSquared() > 0f)
			{
				float randomFloat = MyUtils.GetRandomFloat(0f, ColorSpread.X);
				float randomFloat2 = MyUtils.GetRandomFloat(0f, ColorSpread.Y);
				color = ((MyUtils.GetRandomSign() > 0f) ? Color.Lighten(color, randomFloat) : Color.Darken(color, randomFloat2));
			}
			Vector3 colorMaskHsv = color.ColorToHSVDX11();
			value.SectorInstanceId = value2.AddInstance(itemDefinition.Id.SubtypeId, modelId, hashCode, ref localMatrix, modelOnlyData.BoundingBox, m_instanceFlags, m_definition.MaxViewDistance, colorMaskHsv);
			value.Transform = new MyTransformD(transformMatrix);
			m_itemsData.Add(hashCode, value);
			if (this.ItemAdded != null)
			{
				this.ItemAdded(this, new ItemInfo
				{
					LocalId = hashCode,
					SubtypeId = value.SubtypeId,
					Transform = value.Transform
				});
			}
			return true;
		}

		private static void CheckModelConsistency(MyEnvironmentItemDefinition itemDefinition)
		{
			if (!m_subtypeToModels.TryGetValue(itemDefinition.Id.SubtypeId, out int _) && itemDefinition.Model != null)
			{
				m_subtypeToModels.Add(itemDefinition.Id.SubtypeId, MyModel.GetId(itemDefinition.Model));
			}
		}

		public void PrepareItemsGraphics()
		{
			foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in m_sectors)
			{
				sector.Value.UpdateRenderInstanceData();
				sector.Value.UpdateRenderEntitiesData(base.WorldMatrix);
			}
		}

		public void PrepareItemsPhysics(MyEnvironmentItemsSpawnData spawnData)
		{
			spawnData.SectorRootShape = new HkStaticCompoundShape(HkReferencePolicy.None);
			spawnData.EnvironmentItems.PrepareItemsPhysics(spawnData.SectorRootShape, ref spawnData.AabbWorld, spawnData.SubtypeToShapes);
		}

		private void PrepareItemsPhysics(HkStaticCompoundShape sectorRootShape, ref BoundingBoxD aabbWorld, Dictionary<MyStringHash, HkShape> subtypeIdToShape)
		{
			foreach (KeyValuePair<int, MyEnvironmentItemData> itemsDatum in m_itemsData)
			{
				if (itemsDatum.Value.Enabled)
				{
					MatrixD worldMatrix = itemsDatum.Value.Transform.TransformMatrix;
					if (AddPhysicsShape(itemsDatum.Value.SubtypeId, itemsDatum.Value.Model, ref worldMatrix, sectorRootShape, subtypeIdToShape, out int physicsShapeInstanceId))
					{
						m_physicsShapeInstanceIdToLocalId[physicsShapeInstanceId] = itemsDatum.Value.Id;
						m_localIdToPhysicsShapeInstanceId[itemsDatum.Value.Id] = physicsShapeInstanceId;
					}
				}
			}
			base.PositionComp.WorldAABB = aabbWorld;
			if (sectorRootShape.InstanceCount > 0)
			{
				Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_STATIC)
				{
					MaterialType = m_definition.Material,
					AngularDamping = MyPerGameSettings.DefaultAngularDamping,
					LinearDamping = MyPerGameSettings.DefaultLinearDamping,
					IsStaticForCluster = true
				};
				sectorRootShape.Bake();
				HkMassProperties value = default(HkMassProperties);
				MatrixD worldTransform = MatrixD.CreateTranslation(CellsOffset);
				Physics.CreateFromCollisionObject(sectorRootShape, Vector3.Zero, worldTransform, value);
				if (Sync.IsServer)
				{
					Physics.ContactPointCallback += Physics_ContactPointCallback;
					Physics.RigidBody.ContactPointCallbackEnabled = true;
					Physics.RigidBody.IsEnvironment = true;
				}
				Physics.Enabled = true;
			}
		}

		public bool IsValidPosition(Vector3D position)
		{
			return !m_itemsData.ContainsKey(position.GetHashCode());
		}

		public void BeginBatch(bool sync)
		{
			m_batchTime = 10f;
			if (sync)
			{
				MySyncEnvironmentItems.SendBeginBatchAddMessage(base.EntityId);
			}
		}

		public void BatchAddItem(Vector3D position, MyStringHash subtypeId, bool sync)
		{
			if (m_definition.ContainsItemDefinition(subtypeId))
			{
				m_batchedAddItems.Add(new AddItemData
				{
					Position = position,
					SubtypeId = subtypeId
				});
				if (sync)
				{
					MySyncEnvironmentItems.SendBatchAddItemMessage(base.EntityId, position, subtypeId);
				}
			}
		}

		public void BatchModifyItem(int localId, MyStringHash subtypeId, bool sync)
		{
			if (m_itemsData.ContainsKey(localId))
			{
				m_batchedModifyItems.Add(new ModifyItemData
				{
					LocalId = localId,
					SubtypeId = subtypeId
				});
				if (sync)
				{
					MySyncEnvironmentItems.SendBatchModifyItemMessage(base.EntityId, localId, subtypeId);
				}
			}
		}

		public void BatchRemoveItem(int localId, bool sync)
		{
			if (m_itemsData.ContainsKey(localId))
			{
				m_batchedRemoveItems.Add(new RemoveItemData
				{
					LocalId = localId
				});
				if (sync)
				{
					MySyncEnvironmentItems.SendBatchRemoveItemMessage(base.EntityId, localId);
				}
			}
		}

		public void EndBatch(bool sync)
		{
			m_batchTime = 0f;
			if (m_batchedAddItems.Count > 0 || m_batchedModifyItems.Count > 0 || m_batchedRemoveItems.Count > 0)
			{
				ProcessBatch();
			}
			m_batchedAddItems.Clear();
			m_batchedModifyItems.Clear();
			m_batchedRemoveItems.Clear();
			if (sync)
			{
				MySyncEnvironmentItems.SendEndBatchAddMessage(base.EntityId);
			}
		}

		private void ProcessBatch()
		{
			foreach (RemoveItemData batchedRemoveItem in m_batchedRemoveItems)
			{
				RemoveItem(batchedRemoveItem.LocalId, sync: false, immediateUpdate: false);
			}
			foreach (ModifyItemData batchedModifyItem in m_batchedModifyItems)
			{
				ModifyItemModel(batchedModifyItem.LocalId, batchedModifyItem.SubtypeId, updateSector: false, sync: false);
			}
			if (Physics != null)
			{
				if (Sync.IsServer)
				{
					Physics.ContactPointCallback -= Physics_ContactPointCallback;
				}
				Physics.Close();
				Physics = null;
			}
			BoundingBoxD aabbWorld = BoundingBoxD.CreateInvalid();
			Dictionary<MyStringHash, HkShape> dictionary = new Dictionary<MyStringHash, HkShape>(MyStringHash.Comparer);
			HkStaticCompoundShape sectorRootShape = new HkStaticCompoundShape(HkReferencePolicy.None);
			m_physicsShapeInstanceIdToLocalId.Clear();
			m_localIdToPhysicsShapeInstanceId.Clear();
			foreach (KeyValuePair<int, MyEnvironmentItemData> itemsDatum in m_itemsData)
			{
				if (itemsDatum.Value.Enabled)
				{
					MyModel modelOnlyData = MyModels.GetModelOnlyData(MyModel.GetById(m_subtypeToModels[itemsDatum.Value.SubtypeId]));
					MatrixD transformMatrix = itemsDatum.Value.Transform.TransformMatrix;
					aabbWorld.Include(modelOnlyData.BoundingBox.Transform(transformMatrix));
				}
			}
			foreach (AddItemData batchedAddItem in m_batchedAddItems)
			{
				MatrixD worldMatrix = MatrixD.CreateWorld(batchedAddItem.Position, Vector3D.Forward, Vector3D.Up);
				MyEnvironmentItemDefinition itemDefinition = m_definition.GetItemDefinition(batchedAddItem.SubtypeId);
				AddItem(itemDefinition, ref worldMatrix, ref aabbWorld);
			}
			PrepareItemsPhysics(sectorRootShape, ref aabbWorld, dictionary);
			PrepareItemsGraphics();
			foreach (KeyValuePair<MyStringHash, HkShape> item in dictionary)
			{
				item.Value.RemoveReference();
			}
			dictionary.Clear();
		}

		public bool ModifyItemModel(int itemInstanceId, MyStringHash newSubtypeId, bool updateSector, bool sync)
		{
			if (!m_itemsData.TryGetValue(itemInstanceId, out MyEnvironmentItemData value))
			{
				return false;
			}
			int modelId = GetModelId(value.SubtypeId);
			int modelId2 = GetModelId(newSubtypeId);
			if (value.Enabled)
			{
				Matrix localMatrix = value.Transform.TransformMatrix;
				Vector3I sectorId = MyEnvironmentSector.GetSectorId(localMatrix.Translation - CellsOffset, Definition.SectorSize);
				MyModel modelOnlyData = MyModels.GetModelOnlyData(MyModel.GetById(modelId));
				MyEnvironmentSector myEnvironmentSector = Sectors[sectorId];
				Matrix matrix = Matrix.Invert(myEnvironmentSector.SectorMatrix);
				localMatrix *= matrix;
				myEnvironmentSector.DisableInstance(value.SectorInstanceId, modelId);
				int sectorInstanceId = myEnvironmentSector.AddInstance(newSubtypeId, modelId2, itemInstanceId, ref localMatrix, modelOnlyData.BoundingBox, m_instanceFlags, m_definition.MaxViewDistance);
				value.SubtypeId = newSubtypeId;
				value.SectorInstanceId = sectorInstanceId;
				m_itemsData[itemInstanceId] = value;
				if (updateSector)
				{
					myEnvironmentSector.UpdateRenderInstanceData();
					myEnvironmentSector.UpdateRenderEntitiesData(base.WorldMatrix);
				}
				if (this.ItemModified != null)
				{
					this.ItemModified(this, new ItemInfo
					{
						LocalId = value.Id,
						SubtypeId = value.SubtypeId,
						Transform = value.Transform
					});
				}
				if (sync)
				{
					MySyncEnvironmentItems.SendModifyModelMessage(base.EntityId, itemInstanceId, newSubtypeId);
				}
			}
			return true;
		}

		public bool TryGetItemInfoById(int itemId, out ItemInfo result)
		{
			result = default(ItemInfo);
			if (m_itemsData.TryGetValue(itemId, out MyEnvironmentItemData value) && value.Enabled)
			{
				result = new ItemInfo
				{
					LocalId = itemId,
					SubtypeId = value.SubtypeId,
					Transform = value.Transform
				};
				return true;
			}
			return false;
		}

		public void GetPhysicalItemsInRadius(Vector3D position, float radius, List<ItemInfo> result)
		{
			double num = radius * radius;
			if (Physics == null || !(Physics.RigidBody != null))
			{
				return;
			}
			HkStaticCompoundShape hkStaticCompoundShape = (HkStaticCompoundShape)Physics.RigidBody.GetShape();
			HkShapeContainerIterator iterator = hkStaticCompoundShape.GetIterator();
			while (iterator.IsValid)
			{
				uint currentShapeKey = iterator.CurrentShapeKey;
				hkStaticCompoundShape.DecomposeShapeKey(currentShapeKey, out int instanceId, out uint _);
				if (m_physicsShapeInstanceIdToLocalId.TryGetValue(instanceId, out int value) && m_itemsData.TryGetValue(value, out MyEnvironmentItemData value2) && value2.Enabled && Vector3D.DistanceSquared(value2.Transform.Position, position) < num)
				{
					result.Add(new ItemInfo
					{
						LocalId = value,
						SubtypeId = value2.SubtypeId,
						Transform = value2.Transform
					});
				}
				iterator.Next();
			}
		}

		public void GetAllItemsInRadius(Vector3D point, float radius, List<ItemInfo> output)
		{
			GetSectorsInRadius(point, radius, m_tmpSectors);
			foreach (MyEnvironmentSector tmpSector in m_tmpSectors)
			{
				tmpSector.GetItemsInRadius(point, radius, output);
			}
			m_tmpSectors.Clear();
		}

		public void GetItemsInSector(Vector3I sectorId, List<ItemInfo> output)
		{
			if (m_sectors.ContainsKey(sectorId))
			{
				m_sectors[sectorId].GetItems(output);
			}
		}

		public int GetItemsCount(MyStringHash id)
		{
			int num = 0;
			foreach (KeyValuePair<int, MyEnvironmentItemData> itemsDatum in m_itemsData)
			{
				if (itemsDatum.Value.SubtypeId == id)
				{
					num++;
				}
			}
			return num;
		}

		public void GetSectorsInRadius(Vector3D position, float radius, List<MyEnvironmentSector> sectors)
		{
			foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in m_sectors)
			{
				if (sector.Value.IsValid)
				{
					BoundingBoxD sectorWorldBox = sector.Value.SectorWorldBox;
					sectorWorldBox.Inflate(radius);
					if (sectorWorldBox.Contains(position) == ContainmentType.Contains)
					{
						sectors.Add(sector.Value);
					}
				}
			}
		}

		public void GetSectorIdsInRadius(Vector3D position, float radius, List<Vector3I> sectorIds)
		{
			foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in m_sectors)
			{
				if (sector.Value.IsValid)
				{
					BoundingBoxD sectorWorldBox = sector.Value.SectorWorldBox;
					sectorWorldBox.Inflate(radius);
					if (sectorWorldBox.Contains(position) == ContainmentType.Contains)
					{
						sectorIds.Add(sector.Key);
					}
				}
			}
		}

		public void RemoveItemsAroundPoint(Vector3D point, double radius)
		{
			double radiusSq = radius * radius;
			if (Physics != null && Physics.RigidBody != null)
			{
				HkStaticCompoundShape hkStaticCompoundShape = (HkStaticCompoundShape)Physics.RigidBody.GetShape();
				HkShapeContainerIterator iterator = hkStaticCompoundShape.GetIterator();
				while (iterator.IsValid)
				{
					uint currentShapeKey = iterator.CurrentShapeKey;
					hkStaticCompoundShape.DecomposeShapeKey(currentShapeKey, out int instanceId, out uint _);
					if (m_physicsShapeInstanceIdToLocalId.TryGetValue(instanceId, out int value) && DisableRenderInstanceIfInRadius(point, radiusSq, value, hasPhysics: true))
					{
						hkStaticCompoundShape.EnableInstance(instanceId, enable: false);
						m_tmpToDisable.Add(value);
					}
					iterator.Next();
				}
			}
			else
			{
				foreach (KeyValuePair<int, MyEnvironmentItemData> itemsDatum in m_itemsData)
				{
					if (itemsDatum.Value.Enabled && DisableRenderInstanceIfInRadius(point, radiusSq, itemsDatum.Key))
					{
						m_tmpToDisable.Add(itemsDatum.Key);
					}
				}
			}
			foreach (int item in m_tmpToDisable)
			{
				MyEnvironmentItemData value2 = m_itemsData[item];
				value2.Enabled = false;
				m_itemsData[item] = value2;
			}
			m_tmpToDisable.Clear();
			foreach (Vector3I item2 in m_updatedSectorsTmp)
			{
				Sectors[item2].UpdateRenderInstanceData();
			}
			m_updatedSectorsTmp.Clear();
		}

		public bool RemoveItem(int itemInstanceId, bool sync, bool immediateUpdate = true)
		{
			if (m_localIdToPhysicsShapeInstanceId.TryGetValue(itemInstanceId, out int value))
			{
				return RemoveItem(itemInstanceId, value, sync, immediateUpdate);
			}
			if (m_itemsData.ContainsKey(itemInstanceId))
			{
				return RemoveNonPhysicalItem(itemInstanceId, sync, immediateUpdate);
			}
			return false;
		}

		protected bool RemoveItem(int itemInstanceId, int physicsInstanceId, bool sync, bool immediateUpdate)
		{
			m_physicsShapeInstanceIdToLocalId.Remove(physicsInstanceId);
			m_localIdToPhysicsShapeInstanceId.Remove(itemInstanceId);
			if (!m_itemsData.ContainsKey(itemInstanceId))
			{
				return false;
			}
			MyEnvironmentItemData myEnvironmentItemData = m_itemsData[itemInstanceId];
			m_itemsData.Remove(itemInstanceId);
			if (Physics != null)
			{
				((HkStaticCompoundShape)Physics.RigidBody.GetShape()).EnableInstance(physicsInstanceId, enable: false);
			}
			Matrix matrix = myEnvironmentItemData.Transform.TransformMatrix;
			Vector3I sectorId = MyEnvironmentSector.GetSectorId(matrix.Translation - m_cellsOffset, Definition.SectorSize);
			int modelId = GetModelId(myEnvironmentItemData.SubtypeId);
			if (Sectors.TryGetValue(sectorId, out MyEnvironmentSector value))
			{
				value.DisableInstance(myEnvironmentItemData.SectorInstanceId, modelId);
			}
			foreach (KeyValuePair<int, MyEnvironmentItemData> itemsDatum in m_itemsData)
			{
				if (itemsDatum.Value.SectorInstanceId == Sectors[sectorId].SectorItemCount)
				{
					MyEnvironmentItemData value2 = itemsDatum.Value;
					value2.SectorInstanceId = myEnvironmentItemData.SectorInstanceId;
					m_itemsData[itemsDatum.Key] = value2;
					break;
				}
			}
			if (immediateUpdate)
			{
				value?.UpdateRenderInstanceData(modelId);
			}
			OnRemoveItem(itemInstanceId, ref matrix, myEnvironmentItemData.SubtypeId, myEnvironmentItemData.UserData);
			if (sync)
			{
				MySyncEnvironmentItems.RemoveEnvironmentItem(base.EntityId, itemInstanceId);
			}
			return true;
		}

		protected bool RemoveNonPhysicalItem(int itemInstanceId, bool sync, bool immediateUpdate)
		{
			MyEnvironmentItemData value = m_itemsData[itemInstanceId];
			value.Enabled = false;
			m_itemsData[itemInstanceId] = value;
			Matrix matrix = value.Transform.TransformMatrix;
			Vector3I sectorId = MyEnvironmentSector.GetSectorId(matrix.Translation, Definition.SectorSize);
			int modelId = GetModelId(value.SubtypeId);
			Sectors[sectorId].DisableInstance(value.SectorInstanceId, modelId);
			if (immediateUpdate)
			{
				Sectors[sectorId].UpdateRenderInstanceData(modelId);
			}
			OnRemoveItem(itemInstanceId, ref matrix, value.SubtypeId, value.UserData);
			if (sync)
			{
				MySyncEnvironmentItems.RemoveEnvironmentItem(base.EntityId, itemInstanceId);
			}
			return true;
		}

		public void RemoveItemsOfSubtype(HashSet<MyStringHash> subtypes)
		{
			BeginBatch(sync: true);
			foreach (int item in new List<int>(m_itemsData.Keys))
			{
				MyEnvironmentItemData myEnvironmentItemData = m_itemsData[item];
				if (myEnvironmentItemData.Enabled && subtypes.Contains(myEnvironmentItemData.SubtypeId))
				{
					BatchRemoveItem(item, sync: true);
				}
			}
			EndBatch(sync: true);
		}

		protected virtual void OnRemoveItem(int localId, ref Matrix matrix, MyStringHash myStringId, int userData)
		{
			if (this.ItemRemoved != null)
			{
				this.ItemRemoved(this, new ItemInfo
				{
					LocalId = localId,
					SubtypeId = myStringId,
					Transform = new MyTransformD(matrix),
					UserData = userData
				});
			}
		}

		private bool DisableRenderInstanceIfInRadius(Vector3D center, double radiusSq, int itemInstanceId, bool hasPhysics = false)
		{
			MyEnvironmentItemData myEnvironmentItemData = m_itemsData[itemInstanceId];
			if (Vector3D.DistanceSquared(new Vector3D((Vector3)myEnvironmentItemData.Transform.Position), center) <= radiusSq)
			{
				bool flag = false;
				if (m_localIdToPhysicsShapeInstanceId.TryGetValue(itemInstanceId, out int value))
				{
					m_physicsShapeInstanceIdToLocalId.Remove(value);
					m_localIdToPhysicsShapeInstanceId.Remove(itemInstanceId);
					flag = true;
				}
				if (!hasPhysics || flag)
				{
					Vector3I sectorId = MyEnvironmentSector.GetSectorId(((Matrix)myEnvironmentItemData.Transform.TransformMatrix).Translation - m_cellsOffset, m_definition.SectorSize);
					if (Sectors.TryGetValue(sectorId, out MyEnvironmentSector _) && Sectors[sectorId].DisableInstance(myEnvironmentItemData.SectorInstanceId, GetModelId(myEnvironmentItemData.SubtypeId)))
					{
						m_updatedSectorsTmp.Add(sectorId);
					}
					return true;
				}
			}
			return false;
		}

		public virtual void DoDamage(float damage, int instanceId, Vector3D position, Vector3 normal, MyStringHash type)
		{
		}

		private void Physics_ContactPointCallback(ref MyPhysics.MyContactPointEvent e)
		{
			float num = Math.Abs(e.ContactPointEvent.SeparatingVelocity);
			IMyEntity otherEntity = e.ContactPointEvent.GetOtherEntity(this);
			if (otherEntity == null || otherEntity.Physics == null || otherEntity is MyFloatingObject || otherEntity is IMyHandheldGunObject<MyDeviceBase> || (otherEntity.Physics.RigidBody != null && otherEntity.Physics.RigidBody.Layer == 20))
			{
				return;
			}
			float num2 = MyDestructionHelper.MassFromHavok(otherEntity.Physics.Mass);
			if (otherEntity is MyCharacter)
			{
				num2 = otherEntity.Physics.Mass;
			}
			double num3 = num * num * num2;
			if (!(num3 > 200000.0))
			{
				return;
			}
			int bodyIdx = 0;
			Vector3 normal = e.ContactPointEvent.ContactPoint.Normal;
			if (e.ContactPointEvent.Base.BodyA.GetEntity(0u) != this)
			{
				bodyIdx = 1;
				normal *= -1f;
			}
			uint shapeKey = e.ContactPointEvent.GetShapeKey(bodyIdx);
			if (shapeKey != uint.MaxValue)
			{
				((HkStaticCompoundShape)Physics.RigidBody.GetShape()).DecomposeShapeKey(shapeKey, out int instanceId, out uint _);
				if (m_physicsShapeInstanceIdToLocalId.TryGetValue(instanceId, out int value))
				{
					Vector3D position = Physics.ClusterToWorld(e.ContactPointEvent.ContactPoint.Position);
					DestroyItemAndCreateDebris(position, normal, num3, value);
				}
			}
		}

		public void DestroyItemAndCreateDebris(Vector3D position, Vector3 normal, double energy, int itemId)
		{
			if (MyPerGameSettings.Destruction)
			{
				DoDamage(100f, itemId, position, normal, MyStringHash.NullOrEmpty);
				return;
			}
			MyEntity myEntity = DestroyItem(itemId);
			if (myEntity != null && myEntity.Physics != null)
			{
				MyParticlesManager.TryCreateParticleEffect("Tree Destruction", MatrixD.CreateTranslation(position), out MyParticleEffect _);
				float mass = myEntity.Physics.Mass;
				Vector3 value = (float)Math.Sqrt(energy / (double)mass) / (0.0166666675f * MyFakes.SIMULATION_SPEED) * 0.8f * normal;
				Vector3D value2 = myEntity.Physics.CenterOfMassWorld + 0.5 * Vector3D.Dot(position - myEntity.Physics.CenterOfMassWorld, myEntity.WorldMatrix.Up) * myEntity.WorldMatrix.Up;
				myEntity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, value, value2, null);
			}
		}

		protected virtual MyEntity DestroyItem(int itemInstanceId)
		{
			return null;
		}

		private void DestructionBody_AfterReplaceBody(ref HkdReplaceBodyEvent e)
		{
			e.GetNewBodies(m_tmpBodyInfos);
			foreach (HkdBreakableBodyInfo tmpBodyInfo in m_tmpBodyInfos)
			{
				Matrix rigidBodyMatrix = tmpBodyInfo.Body.GetRigidBody().GetRigidBodyMatrix();
				Vector3 translation = rigidBodyMatrix.Translation;
				Quaternion rotation = Quaternion.CreateFromRotationMatrix(rigidBodyMatrix.GetOrientation());
				Physics.HavokWorld.GetPenetrationsShape(tmpBodyInfo.Body.BreakableShape.GetShape(), ref translation, ref rotation, m_tmpResults, 15);
				foreach (HkBodyCollision tmpResult in m_tmpResults)
				{
					if (tmpResult.GetCollisionEntity() is MyVoxelMap)
					{
						tmpBodyInfo.Body.GetRigidBody().Quality = HkCollidableQualityType.Fixed;
						break;
					}
				}
				m_tmpResults.Clear();
				tmpBodyInfo.Body.GetRigidBody();
				tmpBodyInfo.Body.Dispose();
			}
		}

		private bool AddPhysicsShape(MyStringHash subtypeId, MyModel model, ref MatrixD worldMatrix, HkStaticCompoundShape sectorRootShape, Dictionary<MyStringHash, HkShape> subtypeIdToShape, out int physicsShapeInstanceId)
		{
			physicsShapeInstanceId = 0;
			if (!subtypeIdToShape.TryGetValue(subtypeId, out HkShape value))
			{
				HkShape[] havokCollisionShapes = model.HavokCollisionShapes;
				if (havokCollisionShapes == null || havokCollisionShapes.Length == 0)
				{
					return false;
				}
				value = havokCollisionShapes[0];
				value.AddReference();
				subtypeIdToShape[subtypeId] = value;
			}
			if (value.ReferenceCount != 0)
			{
				Matrix transform = worldMatrix * MatrixD.CreateTranslation(-CellsOffset);
				physicsShapeInstanceId = sectorRootShape.AddInstance(value, transform);
				return true;
			}
			return false;
		}

		public void GetItems(ref Vector3D point, List<Vector3D> output)
		{
			Vector3I sectorId = MyEnvironmentSector.GetSectorId(point, m_definition.SectorSize);
			MyEnvironmentSector value = null;
			if (m_sectors.TryGetValue(sectorId, out value))
			{
				value.GetItems(output);
			}
		}

		public void GetItemsInRadius(ref Vector3D point, float radius, List<Vector3D> output)
		{
			Vector3I sectorId = MyEnvironmentSector.GetSectorId(point, m_definition.SectorSize);
			MyEnvironmentSector value = null;
			if (m_sectors.TryGetValue(sectorId, out value))
			{
				value.GetItemsInRadius(point, radius, output);
			}
		}

		public bool HasItem(int localId)
		{
			if (m_itemsData.ContainsKey(localId))
			{
				return m_itemsData[localId].Enabled;
			}
			return false;
		}

		public void GetAllItems(List<ItemInfo> output)
		{
			foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in m_sectors)
			{
				sector.Value.GetItems(output);
			}
		}

		public void GetItemsInSector(ref Vector3D point, List<ItemInfo> output)
		{
			Vector3I sectorId = MyEnvironmentSector.GetSectorId(point, m_definition.SectorSize);
			MyEnvironmentSector value = null;
			if (m_sectors.TryGetValue(sectorId, out value))
			{
				value.GetItems(output);
			}
		}

		public MyEnvironmentSector GetSector(ref Vector3D worldPosition)
		{
			Vector3I sectorId = MyEnvironmentSector.GetSectorId(worldPosition, m_definition.SectorSize);
			MyEnvironmentSector value = null;
			if (m_sectors.TryGetValue(sectorId, out value))
			{
				return value;
			}
			return null;
		}

		public MyEnvironmentSector GetSector(ref Vector3I sectorId)
		{
			MyEnvironmentSector value = null;
			if (m_sectors.TryGetValue(sectorId, out value))
			{
				return value;
			}
			return null;
		}

		public Vector3I GetSectorId(ref Vector3D worldPosition)
		{
			return MyEnvironmentSector.GetSectorId(worldPosition, m_definition.SectorSize);
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (Sync.IsServer && IsBatching)
			{
				m_batchTime -= 1.66666675f;
				if (m_batchTime <= 0f)
				{
					EndBatch(sync: true);
				}
				if (this.BatchEnded != null)
				{
					this.BatchEnded(this);
				}
			}
		}

		protected override void ClampToWorld()
		{
		}

		public int GetItemInstanceId(uint shapeKey)
		{
			HkStaticCompoundShape hkStaticCompoundShape = (HkStaticCompoundShape)Physics.RigidBody.GetShape();
			if (shapeKey == uint.MaxValue)
			{
				return -1;
			}
			hkStaticCompoundShape.DecomposeShapeKey(shapeKey, out int instanceId, out uint _);
			if (!m_physicsShapeInstanceIdToLocalId.TryGetValue(instanceId, out int value))
			{
				return -1;
			}
			return value;
		}

		public bool IsItemEnabled(int localId)
		{
			return m_itemsData[localId].Enabled;
		}

		public MyStringHash GetItemSubtype(int localId)
		{
			return m_itemsData[localId].SubtypeId;
		}

		public MyEnvironmentItemDefinition GetItemDefinition(int itemInstanceId)
		{
			MyEnvironmentItemData myEnvironmentItemData = m_itemsData[itemInstanceId];
			MyDefinitionId id = new MyDefinitionId(m_definition.ItemDefinitionType, myEnvironmentItemData.SubtypeId);
			return MyDefinitionManager.Static.GetEnvironmentItemDefinition(id);
		}

		public MyEnvironmentItemDefinition GetItemDefinitionFromShapeKey(uint shapeKey)
		{
			int itemInstanceId = GetItemInstanceId(shapeKey);
			if (itemInstanceId == -1)
			{
				return null;
			}
			MyEnvironmentItemData myEnvironmentItemData = m_itemsData[itemInstanceId];
			MyDefinitionId id = new MyDefinitionId(m_definition.ItemDefinitionType, myEnvironmentItemData.SubtypeId);
			return MyDefinitionManager.Static.GetEnvironmentItemDefinition(id);
		}

		public bool GetItemWorldMatrix(int itemInstanceId, out MatrixD worldMatrix)
		{
			worldMatrix = MatrixD.Identity;
			worldMatrix = m_itemsData[itemInstanceId].Transform.TransformMatrix;
			return true;
		}
	}
}
