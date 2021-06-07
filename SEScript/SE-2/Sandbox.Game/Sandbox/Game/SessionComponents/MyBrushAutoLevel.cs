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
	public class MyBrushAutoLevel : IMyVoxelBrush
	{
		public static MyBrushAutoLevel Static = new MyBrushAutoLevel();

		private MyShapeBox m_shape;

		private MatrixD m_transform;

		private MyBrushGUIPropertyNumberCombo m_axis;

		private MyBrushGUIPropertyNumberSlider m_area;

		private MyBrushGUIPropertyNumberSlider m_height;

		private List<MyGuiControlBase> m_list;

		private const long X_ASIS = 0L;

		private const long Y_ASIS = 1L;

		private const long Z_ASIS = 2L;

		private bool m_painting;

		private double m_Xpos;

		private double m_Ypos;

		private double m_Zpos;

		public float MinScale => MySessionComponentVoxelHand.GRID_SIZE;

		public float MaxScale => MySessionComponentVoxelHand.GRID_SIZE * 40f;

		public bool AutoRotate => true;

		public string SubtypeName => "AutoLevel";

		public string BrushIcon => "Textures\\GUI\\Icons\\Voxelhand_AutoLevel.dds";

		private MyBrushAutoLevel()
		{
			m_shape = new MyShapeBox();
			m_transform = MatrixD.Identity;
			m_axis = new MyBrushGUIPropertyNumberCombo(MyVoxelBrushGUIPropertyOrder.First, MyCommonTexts.VoxelHandProperty_AutoLevel_Axis);
			m_axis.AddItem(0L, MyCommonTexts.VoxelHandProperty_AutoLevel_AxisX);
			m_axis.AddItem(1L, MyCommonTexts.VoxelHandProperty_AutoLevel_AxisY);
			m_axis.AddItem(2L, MyCommonTexts.VoxelHandProperty_AutoLevel_AxisZ);
			m_axis.SelectItem(1L);
			m_area = new MyBrushGUIPropertyNumberSlider(MinScale * 2f, MinScale, MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.Second, MyCommonTexts.VoxelHandProperty_AutoLevel_Area);
			MyBrushGUIPropertyNumberSlider area = m_area;
			area.ValueChanged = (Action)Delegate.Combine(area.ValueChanged, new Action(RecomputeShape));
			m_height = new MyBrushGUIPropertyNumberSlider(MinScale, MinScale, MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.Third, MyCommonTexts.VoxelHandProperty_Box_Height);
			MyBrushGUIPropertyNumberSlider height = m_height;
			height.ValueChanged = (Action)Delegate.Combine(height.ValueChanged, new Action(RecomputeShape));
			m_list = new List<MyGuiControlBase>();
			m_axis.AddControlsToList(m_list);
			m_area.AddControlsToList(m_list);
			m_height.AddControlsToList(m_list);
			RecomputeShape();
		}

		private void RecomputeShape()
		{
			double num = (double)m_area.Value * 0.5;
			double num2 = (double)m_height.Value * 0.5;
			m_shape.Boundaries.Min.X = 0.0 - num;
			m_shape.Boundaries.Min.Y = 0.0 - num2;
			m_shape.Boundaries.Min.Z = 0.0 - num;
			m_shape.Boundaries.Max.X = num;
			m_shape.Boundaries.Max.Y = num2;
			m_shape.Boundaries.Max.Z = num;
		}

		public void FixAxis()
		{
			m_painting = true;
			Vector3D center = m_shape.Boundaries.TransformFast(m_transform).Center;
			long selectedKey = m_axis.SelectedKey;
			if ((ulong)selectedKey <= 2uL)
			{
				switch (selectedKey)
				{
				case 0L:
					m_Xpos = center.X;
					break;
				case 1L:
					m_Ypos = center.Y;
					break;
				case 2L:
					m_Zpos = center.Z;
					break;
				}
			}
		}

		public void UnFix()
		{
			m_painting = false;
		}

		public void Fill(MyVoxelBase map, byte matId)
		{
			MyVoxelGenerator.RequestFillInShape(map, m_shape, matId);
		}

		public void Paint(MyVoxelBase map, byte matId)
		{
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
			if (m_painting)
			{
				long selectedKey = m_axis.SelectedKey;
				if ((ulong)selectedKey <= 2uL)
				{
					switch (selectedKey)
					{
					case 0L:
						targetPosition.X = m_Xpos;
						break;
					case 1L:
						targetPosition.Y = m_Ypos;
						break;
					case 2L:
						targetPosition.Z = m_Zpos;
						break;
					}
				}
			}
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
			MySimpleObjectDraw.DrawTransparentBox(ref m_transform, ref m_shape.Boundaries, ref color, MySimpleObjectRasterizer.Solid, 1, 0.04f, null, null, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
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
			MySliderScaleHelper.ScaleSliderUp(ref m_area);
			MySliderScaleHelper.ScaleSliderUp(ref m_height);
			RecomputeShape();
		}

		public void ScaleShapeDown()
		{
			MySliderScaleHelper.ScaleSliderDown(ref m_area);
			MySliderScaleHelper.ScaleSliderDown(ref m_height);
			RecomputeShape();
		}
	}
}
