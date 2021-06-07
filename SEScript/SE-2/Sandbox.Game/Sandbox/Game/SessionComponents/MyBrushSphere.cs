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
	public class MyBrushSphere : IMyVoxelBrush
	{
		public static MyBrushSphere Static = new MyBrushSphere();

		private MyShapeSphere m_shape;

		private MatrixD m_transform;

		private MyBrushGUIPropertyNumberSlider m_radius;

		private List<MyGuiControlBase> m_list;

		public float MinScale => 1.5f;

		public float MaxScale => MySessionComponentVoxelHand.GRID_SIZE * 40f;

		public bool AutoRotate => false;

		public string SubtypeName => "Sphere";

		public string BrushIcon => "Textures\\GUI\\Icons\\Voxelhand_Sphere.dds";

		private MyBrushSphere()
		{
			m_shape = new MyShapeSphere();
			m_shape.Radius = MinScale;
			m_transform = MatrixD.Identity;
			m_radius = new MyBrushGUIPropertyNumberSlider(m_shape.Radius, MinScale, MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.First, MyCommonTexts.VoxelHandProperty_Sphere_Radius);
			MyBrushGUIPropertyNumberSlider radius = m_radius;
			radius.ValueChanged = (Action)Delegate.Combine(radius.ValueChanged, new Action(RadiusChanged));
			m_list = new List<MyGuiControlBase>();
			m_radius.AddControlsToList(m_list);
		}

		private void RadiusChanged()
		{
			m_shape.Radius = m_radius.Value;
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
			MyVoxelGenerator.RequestRevertShape(map, m_shape);
		}

		public Vector3D GetPosition()
		{
			return m_transform.Translation;
		}

		public void SetPosition(ref Vector3D targetPosition)
		{
			m_shape.Center = targetPosition;
			m_transform.Translation = targetPosition;
		}

		public void SetRotation(ref MatrixD rotationMat)
		{
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
			MySimpleObjectDraw.DrawTransparentSphere(ref m_transform, m_shape.Radius, ref color, MySimpleObjectRasterizer.Solid, 20, null, null, -1f, -1, null, MyBillboard.BlendTypeEnum.LDR);
		}

		public List<MyGuiControlBase> GetGuiControls()
		{
			return m_list;
		}

		public bool ShowRotationGizmo()
		{
			return false;
		}

		public void ScaleShapeUp()
		{
			MySliderScaleHelper.ScaleSliderUp(ref m_radius);
			RadiusChanged();
		}

		public void ScaleShapeDown()
		{
			MySliderScaleHelper.ScaleSliderDown(ref m_radius);
			RadiusChanged();
		}
	}
}
