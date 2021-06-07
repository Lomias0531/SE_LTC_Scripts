using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_UseObjectsComponent), true)]
	public class MyUseObjectsComponent : MyUseObjectsComponentBase
	{
		private struct DetectorData
		{
			public IMyUseObject UseObject;

			public Matrix Matrix;

			public string DetectorName;

			public DetectorData(IMyUseObject useObject, Matrix mat, string name)
			{
				UseObject = useObject;
				Matrix = mat;
				DetectorName = name;
			}
		}

		private class Sandbox_Game_Components_MyUseObjectsComponent_003C_003EActor : IActivator, IActivator<MyUseObjectsComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyUseObjectsComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUseObjectsComponent CreateInstance()
			{
				return new MyUseObjectsComponent();
			}

			MyUseObjectsComponent IActivator<MyUseObjectsComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ThreadStatic]
		private static Vector3[] m_detectorVertices;

		[ThreadStatic]
		private static MyList<HkShape> m_shapes;

		private Dictionary<uint, DetectorData> m_detectorInteractiveObjects = new Dictionary<uint, DetectorData>();

		private Dictionary<string, uint> m_detectorShapeKeys = new Dictionary<string, uint>();

		private List<uint> m_customAddedDetectors = new List<uint>();

		private MyPhysicsBody m_detectorPhysics;

		private MyObjectBuilder_UseObjectsComponent m_objectBuilder;

		private MyUseObjectsComponentDefinition m_definition;

		public override MyPhysicsComponentBase DetectorPhysics
		{
			get
			{
				return m_detectorPhysics;
			}
			protected set
			{
				m_detectorPhysics = (value as MyPhysicsBody);
			}
		}

		public override void LoadDetectorsFromModel()
		{
			m_detectors.Clear();
			m_detectorInteractiveObjects.Clear();
			if (m_detectorPhysics != null)
			{
				m_detectorPhysics.Close();
			}
			MyRenderComponentBase obj = base.Container.Get<MyRenderComponentBase>();
			if (obj.GetModel() != null)
			{
				foreach (KeyValuePair<string, MyModelDummy> dummy in obj.GetModel().Dummies)
				{
					string text = dummy.Key.ToLower();
					if (text.StartsWith("detector_") && text.Length > "detector_".Length)
					{
						string[] array = text.Split(new char[1]
						{
							'_'
						});
						if (array.Length >= 2)
						{
							MyModelDummy value = dummy.Value;
							AddDetector(array[1], text, value);
						}
					}
				}
			}
			if (m_detectorInteractiveObjects.Count > 0)
			{
				RecreatePhysics();
			}
		}

		private IMyUseObject CreateInteractiveObject(string detectorName, string dummyName, MyModelDummy dummyData, uint shapeKey)
		{
			if (base.Container.Entity is MyDoor && detectorName == "terminal")
			{
				return new MyUseObjectDoorTerminal(base.Container.Entity, dummyName, dummyData, shapeKey);
			}
			return MyUseObjectFactory.CreateUseObject(detectorName, base.Container.Entity, dummyName, dummyData, shapeKey);
		}

		private uint AddDetector(string detectorName, string dummyName, MyModelDummy dummyData)
		{
			if (!m_detectors.TryGetValue(detectorName, out List<Matrix> value))
			{
				value = new List<Matrix>();
				m_detectors[detectorName] = value;
			}
			Matrix matrix = dummyData.Matrix;
			if (base.Entity is MyCubeBlock)
			{
				float gridScale = (base.Entity as MyCubeBlock).CubeGrid.GridScale;
				matrix.Translation *= gridScale;
				Matrix.Rescale(ref matrix, gridScale);
			}
			value.Add(Matrix.Invert(matrix));
			uint count = (uint)m_detectorInteractiveObjects.Count;
			IMyUseObject myUseObject = CreateInteractiveObject(detectorName, dummyName, dummyData, count);
			if (myUseObject != null)
			{
				m_detectorInteractiveObjects.Add(count, new DetectorData(myUseObject, matrix, detectorName));
				m_detectorShapeKeys[detectorName] = count;
			}
			return count;
		}

		public override void RemoveDetector(uint id)
		{
			if (m_detectorInteractiveObjects.ContainsKey(id))
			{
				m_detectorShapeKeys.Remove(m_detectorInteractiveObjects[id].DetectorName);
				m_detectorInteractiveObjects.Remove(id);
			}
		}

		public override uint AddDetector(string name, Matrix dummyMatrix)
		{
			string text = name.ToLower();
			string text2 = "detector_" + text;
			MyModel model = base.Container.Entity.Render.GetModel();
			Dictionary<string, object> customData = null;
			if (model != null && model.Dummies.TryGetValue(text2, out MyModelDummy value))
			{
				customData = value.CustomData;
			}
			MyModelDummy dummyData = new MyModelDummy
			{
				Name = text2,
				CustomData = customData,
				Matrix = dummyMatrix
			};
			uint num = AddDetector(text, text2, dummyData);
			m_customAddedDetectors.Add(num);
			return num;
		}

		public void SetUseObjectIDs(uint renderId, int instanceId)
		{
			foreach (KeyValuePair<uint, DetectorData> detectorInteractiveObject in m_detectorInteractiveObjects)
			{
				detectorInteractiveObject.Value.UseObject.SetRenderID(renderId);
				detectorInteractiveObject.Value.UseObject.SetInstanceID(instanceId);
			}
		}

		public unsafe override void RecreatePhysics()
		{
			if (m_detectorPhysics != null)
			{
				m_detectorPhysics.Close();
				m_detectorPhysics = null;
			}
			if (m_shapes == null)
			{
				m_shapes = new MyList<HkShape>();
			}
			if (m_detectorVertices == null)
			{
				m_detectorVertices = new Vector3[8];
			}
			m_shapes.Clear();
			BoundingBox boundingBox = new BoundingBox(-Vector3.One / 2f, Vector3.One / 2f);
			MyPositionComponentBase myPositionComponentBase = base.Container.Get<MyPositionComponentBase>();
			foreach (KeyValuePair<uint, DetectorData> detectorInteractiveObject in m_detectorInteractiveObjects)
			{
				try
				{
					fixed (Vector3* corners = m_detectorVertices)
					{
						boundingBox.GetCornersUnsafe(corners);
					}
				}
				finally
				{
				}
				for (int i = 0; i < 8; i++)
				{
					m_detectorVertices[i] = Vector3.Transform(m_detectorVertices[i], detectorInteractiveObject.Value.Matrix);
				}
				m_shapes.Add(new HkConvexVerticesShape(m_detectorVertices, 8, shrink: false, 0f));
			}
			if (m_shapes.Count > 0)
			{
				HkListShape shape = new HkListShape(m_shapes.GetInternalArray(), m_shapes.Count, HkReferencePolicy.TakeOwnership);
				m_detectorPhysics = new MyPhysicsBody(base.Container.Entity, RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE);
				m_detectorPhysics.CreateFromCollisionObject(shape, Vector3.Zero, myPositionComponentBase.WorldMatrix);
				shape.Base.RemoveReference();
			}
		}

		public override void PositionChanged(MyPositionComponentBase obj)
		{
			if (m_detectorPhysics != null)
			{
				m_detectorPhysics.OnWorldPositionChanged(obj);
			}
		}

		private void positionComponent_OnPositionChanged(MyPositionComponentBase obj)
		{
			m_detectorPhysics.OnWorldPositionChanged(obj);
		}

		public override IMyUseObject RaycastDetectors(Vector3D worldFrom, Vector3D worldTo, out float parameter)
		{
			MyPositionComponentBase myPositionComponentBase = base.Container.Get<MyPositionComponentBase>();
			_ = myPositionComponentBase.WorldMatrixNormalizedInv;
			RayD ray = new RayD(worldFrom, worldTo - worldFrom);
			IMyUseObject result = null;
			parameter = float.MaxValue;
			foreach (KeyValuePair<uint, DetectorData> detectorInteractiveObject in m_detectorInteractiveObjects)
			{
				MatrixD matrix = detectorInteractiveObject.Value.Matrix * myPositionComponentBase.WorldMatrix;
				double? num = new MyOrientedBoundingBoxD(matrix).Intersects(ref ray);
				if (num.HasValue && num.Value < (double)parameter)
				{
					parameter = (float)num.Value;
					result = detectorInteractiveObject.Value.UseObject;
				}
			}
			return result;
		}

		public override IMyUseObject GetInteractiveObject(uint shapeKey)
		{
			if (!m_detectorInteractiveObjects.TryGetValue(shapeKey, out DetectorData value))
			{
				return null;
			}
			return value.UseObject;
		}

		public override IMyUseObject GetInteractiveObject(string detectorName)
		{
			if (!m_detectorShapeKeys.TryGetValue(detectorName, out uint value))
			{
				return null;
			}
			return GetInteractiveObject(value);
		}

		public override void GetInteractiveObjects<T>(List<T> objects)
		{
			foreach (KeyValuePair<uint, DetectorData> detectorInteractiveObject in m_detectorInteractiveObjects)
			{
				T val = detectorInteractiveObject.Value.UseObject as T;
				if (val != null)
				{
					objects.Add(val);
				}
			}
		}

		public override bool IsSerialized()
		{
			return m_customAddedDetectors.Count > 0;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_UseObjectsComponent myObjectBuilder_UseObjectsComponent = MyComponentFactory.CreateObjectBuilder(this) as MyObjectBuilder_UseObjectsComponent;
			myObjectBuilder_UseObjectsComponent.CustomDetectorsCount = (uint)m_customAddedDetectors.Count;
			int num = 0;
			if (myObjectBuilder_UseObjectsComponent.CustomDetectorsCount != 0)
			{
				myObjectBuilder_UseObjectsComponent.CustomDetectorsMatrices = new Matrix[myObjectBuilder_UseObjectsComponent.CustomDetectorsCount];
				myObjectBuilder_UseObjectsComponent.CustomDetectorsNames = new string[myObjectBuilder_UseObjectsComponent.CustomDetectorsCount];
				{
					foreach (uint customAddedDetector in m_customAddedDetectors)
					{
						myObjectBuilder_UseObjectsComponent.CustomDetectorsNames[num] = m_detectorInteractiveObjects[customAddedDetector].DetectorName;
						myObjectBuilder_UseObjectsComponent.CustomDetectorsMatrices[num] = m_detectorInteractiveObjects[customAddedDetector].Matrix;
						num++;
					}
					return myObjectBuilder_UseObjectsComponent;
				}
			}
			return myObjectBuilder_UseObjectsComponent;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			m_objectBuilder = (builder as MyObjectBuilder_UseObjectsComponent);
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			if (m_definition != null)
			{
				if (m_definition.LoadFromModel)
				{
					LoadDetectorsFromModel();
				}
				if (m_definition.UseObjectFromModelBBox != null)
				{
					Matrix matrix = Matrix.CreateScale(base.Entity.PositionComp.LocalAABB.Size) * Matrix.CreateTranslation(base.Entity.PositionComp.LocalAABB.Center);
					AddDetector(m_definition.UseObjectFromModelBBox, matrix);
				}
			}
			if (m_objectBuilder != null)
			{
				for (int i = 0; i < m_objectBuilder.CustomDetectorsCount; i++)
				{
					if (!m_detectors.ContainsKey(m_objectBuilder.CustomDetectorsNames[i]))
					{
						AddDetector(m_objectBuilder.CustomDetectorsNames[i], m_objectBuilder.CustomDetectorsMatrices[i]);
					}
				}
			}
			RecreatePhysics();
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			m_definition = (definition as MyUseObjectsComponentDefinition);
		}
	}
}
