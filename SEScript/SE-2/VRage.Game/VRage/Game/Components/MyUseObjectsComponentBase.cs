using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Entity.UseObject;
using VRageMath;

namespace VRage.Game.Components
{
	[MyComponentType(typeof(MyUseObjectsComponentBase))]
	public abstract class MyUseObjectsComponentBase : MyEntityComponentBase
	{
		protected Dictionary<string, List<Matrix>> m_detectors = new Dictionary<string, List<Matrix>>();

		public abstract MyPhysicsComponentBase DetectorPhysics
		{
			get;
			protected set;
		}

		public override string ComponentTypeDebugString => "Use Objects";

		public abstract uint AddDetector(string name, Matrix matrix);

		public abstract void RemoveDetector(uint id);

		public abstract void RecreatePhysics();

		public abstract void LoadDetectorsFromModel();

		public abstract IMyUseObject GetInteractiveObject(uint shapeKey);

		public abstract IMyUseObject GetInteractiveObject(string detectorName);

		public abstract void GetInteractiveObjects<T>(List<T> objects) where T : class, IMyUseObject;

		public string RaycastDetectors(Vector3D worldFrom, Vector3D worldTo)
		{
			MatrixD worldMatrixNormalizedInv = base.Container.Get<MyPositionComponentBase>().WorldMatrixNormalizedInv;
			Vector3D position = Vector3D.Transform(worldFrom, worldMatrixNormalizedInv);
			Vector3D position2 = Vector3D.Transform(worldTo, worldMatrixNormalizedInv);
			BoundingBox boundingBox = new BoundingBox(-Vector3.One, Vector3.One);
			string result = null;
			float num = float.MaxValue;
			foreach (KeyValuePair<string, List<Matrix>> detector in m_detectors)
			{
				foreach (Matrix item in detector.Value)
				{
					Vector3 position3 = Vector3D.Transform(position, item);
					Vector3 direction = Vector3D.Transform(position2, item);
					float? num2 = boundingBox.Intersects(new Ray(position3, direction));
					if (num2.HasValue && num2.Value < num)
					{
						num = num2.Value;
						result = detector.Key;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Determine whether the given ray intersects any detector. If so, returns
		/// the parametric value of the point of first intersection.
		/// PARAMATER IS NOT DISTANCE!
		/// </summary>
		public abstract IMyUseObject RaycastDetectors(Vector3D worldFrom, Vector3D worldTo, out float parameter);

		public ListReader<Matrix> GetDetectors(string detectorName)
		{
			List<Matrix> value = null;
			m_detectors.TryGetValue(detectorName, out value);
			if (value == null || value.Count == 0)
			{
				return ListReader<Matrix>.Empty;
			}
			return new ListReader<Matrix>(value);
		}

		public virtual void ClearPhysics()
		{
			if (DetectorPhysics != null)
			{
				DetectorPhysics.Close();
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			ClearPhysics();
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			if (DetectorPhysics != null)
			{
				DetectorPhysics.Activate();
			}
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
			if (DetectorPhysics != null)
			{
				DetectorPhysics.Deactivate();
			}
		}

		public abstract void PositionChanged(MyPositionComponentBase obj);
	}
}
