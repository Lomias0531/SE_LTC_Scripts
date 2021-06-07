using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	public class MyBrushRamp : IMyVoxelBrush
	{
		public static MyBrushRamp Static = new MyBrushRamp();

		private MyShapeRamp m_shape;

		private MatrixD m_transform;

		private MyBrushGUIPropertyNumberSlider m_width;

		private MyBrushGUIPropertyNumberSlider m_height;

		private MyBrushGUIPropertyNumberSlider m_depth;

		private List<MyGuiControlBase> m_list;

		public float MinScale => 4.5f;

		public float MaxScale => MySessionComponentVoxelHand.GRID_SIZE * 40f;

		public bool AutoRotate => true;

		public string SubtypeName => "Ramp";

		public string BrushIcon => "Textures\\GUI\\Icons\\Voxelhand_Ramp.dds";

		private MyBrushRamp()
		{
			m_shape = new MyShapeRamp();
			m_transform = MatrixD.Identity;
			m_width = new MyBrushGUIPropertyNumberSlider(MinScale, MinScale, MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.First, MyCommonTexts.VoxelHandProperty_Box_Width);
			MyBrushGUIPropertyNumberSlider width = m_width;
			width.ValueChanged = (Action)Delegate.Combine(width.ValueChanged, new Action(RecomputeShape));
			m_height = new MyBrushGUIPropertyNumberSlider(MinScale, MinScale, MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.Second, MyCommonTexts.VoxelHandProperty_Box_Height);
			MyBrushGUIPropertyNumberSlider height = m_height;
			height.ValueChanged = (Action)Delegate.Combine(height.ValueChanged, new Action(RecomputeShape));
			m_depth = new MyBrushGUIPropertyNumberSlider(MinScale, MinScale, MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.Third, MyCommonTexts.VoxelHandProperty_Box_Depth);
			MyBrushGUIPropertyNumberSlider depth = m_depth;
			depth.ValueChanged = (Action)Delegate.Combine(depth.ValueChanged, new Action(RecomputeShape));
			m_list = new List<MyGuiControlBase>();
			m_width.AddControlsToList(m_list);
			m_height.AddControlsToList(m_list);
			m_depth.AddControlsToList(m_list);
			RecomputeShape();
		}

		private void RecomputeShape()
		{
			Vector3D vector3D = new Vector3D(m_width.Value, m_height.Value, m_depth.Value) * 0.5;
			m_shape.Boundaries.Min = -vector3D;
			m_shape.Boundaries.Max = vector3D;
			Vector3D min = m_shape.Boundaries.Min;
			min.X -= m_shape.Boundaries.Size.Z;
			Vector3D rampNormal = Vector3D.Normalize((m_shape.Boundaries.Min - min).Cross(m_shape.Boundaries.Max - min));
			double num = rampNormal.Dot(min);
			m_shape.RampNormal = rampNormal;
			m_shape.RampNormalW = 0.0 - num;
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
			return m_shape.Boundaries;
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
			MySimpleObjectDraw.DrawTransparentRamp(ref m_transform, ref m_shape.Boundaries, ref color, null, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
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
			MySliderScaleHelper.ScaleSliderUp(ref m_width);
			MySliderScaleHelper.ScaleSliderUp(ref m_height);
			MySliderScaleHelper.ScaleSliderUp(ref m_depth);
			RecomputeShape();
		}

		public void ScaleShapeDown()
		{
			MySliderScaleHelper.ScaleSliderDown(ref m_width);
			MySliderScaleHelper.ScaleSliderDown(ref m_height);
			MySliderScaleHelper.ScaleSliderDown(ref m_depth);
			RecomputeShape();
		}
	}
}
