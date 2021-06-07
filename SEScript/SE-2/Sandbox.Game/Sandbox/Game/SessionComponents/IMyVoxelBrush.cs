using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	public interface IMyVoxelBrush
	{
		float MinScale
		{
			get;
		}

		float MaxScale
		{
			get;
		}

		bool AutoRotate
		{
			get;
		}

		string BrushIcon
		{
			get;
		}

		string SubtypeName
		{
			get;
		}

		void Fill(MyVoxelBase map, byte matId);

		void Paint(MyVoxelBase map, byte matId);

		void CutOut(MyVoxelBase map);

		void Revert(MyVoxelBase map);

		void SetRotation(ref MatrixD rotationMat);

		void SetPosition(ref Vector3D targetPosition);

		Vector3D GetPosition();

		bool ShowRotationGizmo();

		BoundingBoxD PeekWorldBoundingBox(ref Vector3D targetPosition);

		BoundingBoxD GetBoundaries();

		BoundingBoxD GetWorldBoundaries();

		void Draw(ref Color color);

		List<MyGuiControlBase> GetGuiControls();

		void ScaleShapeUp();

		void ScaleShapeDown();
	}
}
