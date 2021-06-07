using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.CoordinateSystem;
using System;
using System.Linq;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyProjectorClipboard : MyGridClipboard
	{
		private MyProjectorBase m_projector;

		private Vector3I m_oldProjectorRotation;

		private Vector3I m_oldProjectorOffset;

		private float m_oldScale;

		private MatrixD m_oldProjectorMatrix;

		private bool m_firstUpdateAfterNewBlueprint;

		private bool m_hasPreviewBBox;

		private bool m_projectionCanBePlaced;

		public override bool HasPreviewBBox
		{
			get
			{
				return m_hasPreviewBBox;
			}
			set
			{
				m_hasPreviewBBox = value;
			}
		}

		protected override float Transparency => 0f;

		protected override bool CanBePlaced => m_projectionCanBePlaced;

		public float GridSize
		{
			get
			{
				if (base.CopiedGrids != null && base.CopiedGrids.Count > 0)
				{
					return MyDefinitionManager.Static.GetCubeSize(base.CopiedGrids[0].GridSizeEnum);
				}
				return 0f;
			}
		}

		public MyProjectorClipboard(MyProjectorBase projector, MyPlacementSettings settings)
			: base(settings)
		{
			m_projector = projector;
			m_calculateVelocity = false;
		}

		public void Clear()
		{
			base.CopiedGrids.Clear();
			m_copiedGridOffsets.Clear();
			m_oldScale = 1f;
		}

		protected override void TestBuildingMaterials()
		{
			m_characterHasEnoughMaterials = true;
		}

		public bool HasGridsLoaded()
		{
			if (base.CopiedGrids != null)
			{
				return base.CopiedGrids.Count > 0;
			}
			return false;
		}

		public void ProcessCubeGrid(MyObjectBuilder_CubeGrid gridBuilder)
		{
			gridBuilder.IsStatic = false;
			gridBuilder.DestructibleBlocks = false;
			foreach (MyObjectBuilder_CubeBlock cubeBlock in gridBuilder.CubeBlocks)
			{
				cubeBlock.Owner = 0L;
				cubeBlock.ShareMode = MyOwnershipShareModeEnum.None;
				cubeBlock.EntityId = 0L;
				MyObjectBuilder_FunctionalBlock myObjectBuilder_FunctionalBlock = cubeBlock as MyObjectBuilder_FunctionalBlock;
				if (myObjectBuilder_FunctionalBlock != null)
				{
					myObjectBuilder_FunctionalBlock.Enabled = false;
				}
			}
		}

		protected override void UpdatePastePosition()
		{
			m_pastePositionPrevious = m_pastePosition;
			m_pastePosition = m_projector.WorldMatrix.Translation;
		}

		protected override bool TestPlacement()
		{
			return MyEntities.IsInsideWorld(m_pastePosition);
		}

		public bool ActuallyTestPlacement()
		{
			m_projectionCanBePlaced = base.TestPlacement();
			MyCoordinateSystem.Static.Visible = false;
			return m_projectionCanBePlaced;
		}

		protected override MyEntity GetClipboardBuilder()
		{
			return null;
		}

		public void ResetGridOrientation()
		{
			m_pasteDirForward = Vector3.Forward;
			m_pasteDirUp = Vector3.Up;
			m_pasteOrientationAngle = 0f;
		}

		protected override void UpdateGridTransformations()
		{
			MatrixD other = m_projector.WorldMatrix;
			if (!m_firstUpdateAfterNewBlueprint && !(m_oldProjectorRotation != m_projector.ProjectionRotation) && !(m_oldProjectorOffset != m_projector.ProjectionOffset) && m_oldProjectorMatrix.EqualsFast(ref other) && m_projector.Scale == m_oldScale)
			{
				return;
			}
			m_oldProjectorRotation = m_projector.ProjectionRotation;
			m_oldProjectorMatrix = other;
			m_oldProjectorOffset = m_projector.ProjectionOffset;
			other = Matrix.Multiply(Matrix.CreateFromQuaternion(m_projector.ProjectionRotationQuaternion), other);
			Vector3 projectionTranslationOffset = m_projector.GetProjectionTranslationOffset();
			projectionTranslationOffset = Vector3D.Transform(projectionTranslationOffset, m_projector.WorldMatrix.GetOrientation());
			other.Translation -= projectionTranslationOffset;
			float scale = m_projector.Scale;
			MatrixD matrix = MatrixD.Invert(base.PreviewGrids[0].WorldMatrix);
			Vector3D vector3D = Vector3D.Zero;
			for (int i = 0; i < base.PreviewGrids.Count; i++)
			{
				MatrixD worldMatrix = other;
				if (i != 0)
				{
					MatrixD matrix2 = base.PreviewGrids[i].WorldMatrix * matrix;
					matrix2.Translation *= (double)m_projector.Scale;
					worldMatrix = matrix2 * other;
				}
				if (!m_projector.AllowScaling && i == 0)
				{
					MySlimBlock mySlimBlock = base.PreviewGrids[i].CubeBlocks.First();
					Vector3D value = MyCubeGrid.GridIntegerToWorld(base.PreviewGrids[i].GridSize, mySlimBlock.Position, worldMatrix);
					vector3D = worldMatrix.Translation - value;
				}
				worldMatrix.Translation += vector3D;
				base.PreviewGrids[i].PositionComp.Scale = scale;
				base.PreviewGrids[i].PositionComp.SetWorldMatrix(worldMatrix, null, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: true);
			}
			m_oldScale = m_projector.Scale;
			m_firstUpdateAfterNewBlueprint = false;
		}

		public override void Activate(Action callback = null)
		{
			ActivateNoAlign(callback);
			m_firstUpdateAfterNewBlueprint = true;
		}
	}
}
