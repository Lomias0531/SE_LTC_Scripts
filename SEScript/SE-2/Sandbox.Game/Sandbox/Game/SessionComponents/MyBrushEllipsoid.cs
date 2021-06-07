using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	public class MyBrushEllipsoid : IMyVoxelBrush
	{
		public static MyBrushEllipsoid Static = new MyBrushEllipsoid();

		private MyShapeEllipsoid m_shape;

		private MatrixD m_transform;

		private MyBrushGUIPropertyNumberSlider m_radiusX;

		private MyBrushGUIPropertyNumberSlider m_radiusY;

		private MyBrushGUIPropertyNumberSlider m_radiusZ;

		private List<MyGuiControlBase> m_list;

		public float MinScale => 0.25f;

		public float MaxScale => MySessionComponentVoxelHand.GRID_SIZE * 40f;

		public bool AutoRotate => false;

		public string SubtypeName => "Ellipsoid";

		public string BrushIcon => "Textures\\GUI\\Icons\\Voxelhand_Sphere.dds";

		private MyBrushEllipsoid()
		{
			m_shape = new MyShapeEllipsoid();
			m_transform = MatrixD.Identity;
			float valueStep = 0.25f;
			m_radiusX = new MyBrushGUIPropertyNumberSlider(MinScale, MinScale, MaxScale, valueStep, MyVoxelBrushGUIPropertyOrder.First, MyStringId.GetOrCompute("Radius X"));
			MyBrushGUIPropertyNumberSlider radiusX = m_radiusX;
			radiusX.ValueChanged = (Action)Delegate.Combine(radiusX.ValueChanged, new Action(RadiusChanged));
			m_radiusY = new MyBrushGUIPropertyNumberSlider(MinScale, MinScale, MaxScale, valueStep, MyVoxelBrushGUIPropertyOrder.Second, MyStringId.GetOrCompute("Radius Y"));
			MyBrushGUIPropertyNumberSlider radiusY = m_radiusY;
			radiusY.ValueChanged = (Action)Delegate.Combine(radiusY.ValueChanged, new Action(RadiusChanged));
			m_radiusZ = new MyBrushGUIPropertyNumberSlider(MinScale, MinScale, MaxScale, valueStep, MyVoxelBrushGUIPropertyOrder.Third, MyStringId.GetOrCompute("Radius Z"));
			MyBrushGUIPropertyNumberSlider radiusZ = m_radiusZ;
			radiusZ.ValueChanged = (Action)Delegate.Combine(radiusZ.ValueChanged, new Action(RadiusChanged));
			m_list = new List<MyGuiControlBase>();
			m_radiusX.AddControlsToList(m_list);
			m_radiusY.AddControlsToList(m_list);
			m_radiusZ.AddControlsToList(m_list);
			RecomputeShape();
		}

		private void RadiusChanged()
		{
			RecomputeShape();
		}

		private void RecomputeShape()
		{
			m_shape.Radius = new Vector3(m_radiusX.Value, m_radiusY.Value, m_radiusZ.Value);
		}

		public void Fill(MyVoxelBase map, byte matId)
		{
			MyVoxelGenerator.RequestFillInShape(map, m_shape, matId);
		}

		public void Paint(MyVoxelBase map, byte matId)
		{
			MyVoxelGenerator.RequestPaintInShape(map, m_shape, matId);
		}

		public void CutOut(MyVoxelBase map)
		{
			MyVoxelGenerator.RequestCutOutShape(map, m_shape);
		}

		public void Revert(MyVoxelBase map)
		{
			MyVoxelGenerator.RequestCutOutShape(map, m_shape);
		}

		public Vector3D GetPosition()
		{
			return m_transform.Translation;
		}

		public void SetPosition(ref Vector3D targetPosition)
		{
			m_transform.Translation = targetPosition;
			m_shape.Transformation = m_transform;
		}

		public void SetRotation(ref MatrixD rotationMat)
		{
			if (rotationMat.IsRotation())
			{
				m_transform.M11 = rotationMat.M11;
				m_transform.M12 = rotationMat.M12;
				m_transform.M13 = rotationMat.M13;
				m_transform.M21 = rotationMat.M21;
				m_transform.M22 = rotationMat.M22;
				m_transform.M23 = rotationMat.M23;
				m_transform.M31 = rotationMat.M31;
				m_transform.M32 = rotationMat.M32;
				m_transform.M33 = rotationMat.M33;
				m_shape.Transformation = m_transform;
			}
		}

		public BoundingBoxD GetBoundaries()
		{
			return m_shape.GetWorldBoundaries();
		}

		public BoundingBoxD PeekWorldBoundingBox(ref Vector3D targetPosition)
		{
			return m_shape.PeekWorldBoundaries(ref targetPosition);
		}

		public BoundingBoxD GetWorldBoundaries()
		{
			return m_shape.GetWorldBoundaries();
		}

		public void Draw(ref Color color)
		{
			BoundingBoxD localbox = m_shape.Boundaries;
			MySimpleObjectDraw.DrawTransparentBox(ref m_transform, ref localbox, ref color, MySimpleObjectRasterizer.Solid, 1, 0.04f, null, null, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
		}

		public List<MyGuiControlBase> GetGuiControls()
		{
			return m_list;
		}

		public bool ShowRotationGizmo()
		{
			return true;
		}

		public void ScaleShapeUp()
		{
			MySliderScaleHelper.ScaleSliderUp(ref m_radiusX);
			MySliderScaleHelper.ScaleSliderUp(ref m_radiusY);
			MySliderScaleHelper.ScaleSliderUp(ref m_radiusZ);
			RecomputeShape();
		}

		public void ScaleShapeDown()
		{
			MySliderScaleHelper.ScaleSliderDown(ref m_radiusX);
			MySliderScaleHelper.ScaleSliderDown(ref m_radiusY);
			MySliderScaleHelper.ScaleSliderDown(ref m_radiusZ);
			RecomputeShape();
		}
	}
}
