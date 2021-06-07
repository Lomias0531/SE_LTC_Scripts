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
	public class MyBrushCapsule : IMyVoxelBrush
	{
		public static MyBrushCapsule Static = new MyBrushCapsule();

		private MyShapeCapsule m_shape;

		private MatrixD m_transform;

		private MyBrushGUIPropertyNumberSlider m_radius;

		private MyBrushGUIPropertyNumberSlider m_length;

		private List<MyGuiControlBase> m_list;

		public float MinScale => 1.5f;

		public float MaxScale => MySessionComponentVoxelHand.GRID_SIZE * 40f;

		public bool AutoRotate => true;

		public string SubtypeName => "Capsule";

		public string BrushIcon => "Textures\\GUI\\Icons\\Voxelhand_Capsule.dds";

		private MyBrushCapsule()
		{
			m_shape = new MyShapeCapsule();
			m_transform = MatrixD.Identity;
			m_radius = new MyBrushGUIPropertyNumberSlider(MinScale, MinScale, MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.First, MyCommonTexts.VoxelHandProperty_Capsule_Radius);
			MyBrushGUIPropertyNumberSlider radius = m_radius;
			radius.ValueChanged = (Action)Delegate.Combine(radius.ValueChanged, new Action(RecomputeShape));
			m_length = new MyBrushGUIPropertyNumberSlider(MinScale, MinScale, MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.Second, MyCommonTexts.VoxelHandProperty_Capsule_Length);
			MyBrushGUIPropertyNumberSlider length = m_length;
			length.ValueChanged = (Action)Delegate.Combine(length.ValueChanged, new Action(RecomputeShape));
			m_list = new List<MyGuiControlBase>();
			m_radius.AddControlsToList(m_list);
			m_length.AddControlsToList(m_list);
			RecomputeShape();
		}

		private void RecomputeShape()
		{
			m_shape.Radius = m_radius.Value;
			double num = (double)m_length.Value * 0.5;
			m_shape.A.X = (m_shape.A.Z = 0.0);
			m_shape.B.X = (m_shape.B.Z = 0.0);
			m_shape.A.Y = 0.0 - num;
			m_shape.B.Y = num;
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
			MySimpleObjectDraw.DrawTransparentCapsule(ref m_transform, m_shape.Radius, m_length.Value, ref color, 20, null, -1, MyBillboard.BlendTypeEnum.LDR);
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
			MySliderScaleHelper.ScaleSliderUp(ref m_radius);
			MySliderScaleHelper.ScaleSliderUp(ref m_length);
			RecomputeShape();
		}

		public void ScaleShapeDown()
		{
			MySliderScaleHelper.ScaleSliderDown(ref m_radius);
			MySliderScaleHelper.ScaleSliderDown(ref m_length);
			RecomputeShape();
		}
	}
}
